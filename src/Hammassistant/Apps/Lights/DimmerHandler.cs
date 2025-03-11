using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Hammlet.Apps.SceneOnButton;
using Hammlet.Extensions;
using Hammlet.Models.Enums;
using Hammlet.Models.Extensions;
using Hammlet.NetDaemon.Extensions;
using Hammlet.NetDaemon.Models;
using HassModel;
using Microsoft.AspNetCore.Routing;
using NetDaemon.Client;
using NetDaemon.Extensions.Hammlet;
using NetDaemon.Extensions.Hammlet.Model;
using NetDaemon.Extensions.Hammlet.Services;
using NetDaemon.Extensions.Observables;
using NetDaemon.HassModel.Entities;
using Reactive.Boolean;

namespace Hammlet.Apps.Lights;

[NetDaemonApp]
internal class DimmerHandler(
    IAsyncHaContext ha,
    IHomeAssistantRunner hassRunner,
    AsyncServiceCaller serviceCaller,
    BinarySensorEntities binarySensors,
    EventEntities eventEntities,
    LightEntities lights,
    ILogger<DimmerHandler> logger,
    IScheduler scheduler,
    IAppConfig<DimmerSync> appCfg) : IAsyncInitializable, IAsyncDisposable
{


    private void ToggleWarm(LightEntity @this)
    {
        if (!@this.IsOn()) return;
        if (@this.@IsWarm())
            DstState = DstState with
            {
                ColorTemp = 90
            };
        else
            DstState = DstState with
            {
                ColorTemp = 10
            };
        _loopTokenSource?.Cancel();
    }

    private void SetState(bool state)
    {
        DstState = DstState with
        {
            State = state
        };
        _loopTokenSource?.Cancel();
    }


    private void SetBrightness(int newBrightness)
    {
        DstState = DstState with
        {
            BrightnessStep = newBrightness
        };
        _loopTokenSource?.Cancel();
    }

    public bool Disposed { get; set; }

    private record Desiredstate(
        bool? State = null,
        int? BrightnessStep = null,
        int? ColorTemp = null,
        bool? Dimming = null,
        int? DimmingAmount = null);

    private CancellationTokenSource? _globalCts;
    private CancellationToken cancellationToken;
    private CancellationTokenSource? _loopTokenSource;
    public int LoopDelay { get; set; } = 10000;
    private Desiredstate DstState { get; set; } = new Desiredstate();
    private int _expectedBrightness;
    private Task? _longRunningTask;
    private bool? IsDimming
    {
        get => DstState.Dimming;
        set
        {
            if(value == DstState.Dimming)
                return;
            DstState = DstState with
            {
                Dimming = value,
                DimmingAmount = (value == true ? appCfg.Value.TickBrightness : (value == false ? -1 * appCfg.Value.TickBrightness : null))
            };
            _loopTokenSource?.Cancel();
        }
    }

    public async Task InitializeAsync(CancellationToken callerToken)
    {
        _globalCts = CancellationTokenSource.CreateLinkedTokenSource(callerToken);
        cancellationToken = _globalCts.Token;
        await serviceCaller.InitializeAsync(cancellationToken);
        var cfg = Initialize();
        _longRunningTask = Task.Run(async () =>
        {
            while (!cancellationToken.IsCancellationRequested && !Disposed)
            {
                try
                {
                    var token = NewLoop(cancellationToken);
                    var dst = DstState;
                    if (Disposed || IsWaiting ||
                        ha.LightEntity(cfg.TargetLightId) is not { } entity ||
                        !NeedsTransition(entity, dst))
                    {
                        try
                        {
                            await Task.Delay(LoopDelay, token);
                        }
                        catch (TaskCanceledException)
                        {
                            // Ignore
                        }

                        continue;
                    }

                    if (dst.State == false)
                    {
                        await CallServiceAsyc(entity, "turn_off", s => s.IsOff(),
                            new LightTurnOffParameters()
                            {
                                Transition = 1
                            },
                            cancellationToken).ConfigureAwait(false);
                        DstState = new();
                        continue;
                    }

                    if (dst.State == true)
                    {
                        if (!entity.IsOn())
                        {
                            logger.LogDebug("Entity was not on, ignoring previous and turning it on first");
                            await CallServiceAsyc(entity, "turn_on", s => s.IsOn(),
                                new LightTurnOnParameters()
                                {
                                    Transition = cfg.Transition,
                                    ColorTemp = DstState.ColorTemp,
                                    Brightness = cfg.DefaultBrightness
                                },
                                cancellationToken).ConfigureAwait(false);
                            if (cfg.DefaultBrightness.HasValue)
                                _expectedBrightness = cfg.DefaultBrightness.Value;
                            DstState = new();
                            continue;
                        }
                        else
                        {
                            _expectedBrightness = (int?)entity.EntityState?.Attributes?.Brightness ?? _expectedBrightness;
                            logger.LogDebug("Entity was on, continuing with transition");
                        }
                    }

                    var (k,r) =
                        dst.ColorTemp.HasValue ? entity.GetColorTempFromPct(dst.ColorTemp.Value) : (null, null);
                    var newBrightness =
                        (int)(_expectedBrightness) +
                        (dst.BrightnessStep ?? dst.DimmingAmount??0);

                    var oldBrightness = newBrightness;
                    newBrightness = Int32.Clamp(newBrightness, 0, 255);
                    var data = new LightTurnOnParameters()
                    {
//                        Transition = cfg.DimmerDelay/1000,
                        ColorTempKelvin = k,
                        ColorTemp = r,
                        Transition = cfg.Transition,
                        Brightness =_expectedBrightness = newBrightness
                    };
                    if (logger.IsEnabled(LogLevel.Debug) && entity.Attributes is {} existing)
                    {
                        var deltas = new Dictionary<string, object>();
                        if(k.HasValue && Math.Abs(k.Value - (existing.ColorTempKelvin ?? 0.0)) > .001)
                            deltas["ColorTempKelvin"] = new { From=existing.ColorTempKelvin, To=k};
                        if(r.HasValue && Math.Abs(r.Value - (existing.ColorTemp ?? 0.0)) > .001)
                            deltas["ColorTemp"] = new { From=existing.ColorTemp, To=r};
                        if(newBrightness != (int?)existing.Brightness)
                            deltas["Brightness"] = new { From = existing.Brightness, To = newBrightness };
                        logger.LogDebug("Turning on light with {@deltas}, {Step}, {Dimming}",deltas, dst.BrightnessStep, dst.DimmingAmount);
                    }
                     await CallServiceAsyc(entity, "turn_on",
                            s => s.IsOn()
                            ,data
                            , cancellationToken, TimeSpan.FromMilliseconds(cfg.DimmerDelay), TimeSpan.FromSeconds(1))
                        .ConfigureAwait(false);
                    var dimming = oldBrightness != newBrightness ? null : DstState.Dimming;
                    DstState = new();
                    IsDimming = dimming;
                    LoopDelay = DstState.Dimming.HasValue ? 0 : 2000;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error in dimmer loop");
                }
                finally
                {
                    var t = _loopTokenSource;
                    _loopTokenSource = null;
                    t?.Dispose();
                }


            }
        }, cancellationToken);
    }

    private readonly System.Reactive.Disposables.CompositeDisposable _disposables = new();


    private DimmerSync Initialize()
    {
        var cfg = appCfg.Value;
//        cfg.DimmerId = "light.liminal_dimmer";
//        cfg.TargetLightId = "light.liminal_ceiling";
        var upButton = ha.BinarySensor(cfg.UpSensorId ?? cfg.DimmerId + "_input_0");
        var downButton = ha.BinarySensor(cfg.DownSensorId ?? cfg.DimmerId + "_input_1");
        var upEvent = ha.Event(cfg.UpEventId ?? cfg.DimmerId + "_button_0");
        var downEvent = ha.Event(cfg.DownButtonId ?? cfg.DimmerId + "_button_1");
        var target = ha.LightEntity(cfg.TargetLightId);

        _disposables.Add( OnConsecutiveUpEvents(upEvent,"s",cfg.Timing.PressDelay));
        _disposables.Add(OnConsecutiveDownEvents(downEvent, "s", cfg.Timing.PressDelay));
        _expectedBrightness = (int?)target.EntityState?.Attributes?.Brightness ?? 0;
        _disposables.Add(target.StateAllChanges().SubscribeSafe(s =>
        {
            if (IsDimming == null)
                _expectedBrightness = (int?)s.New?.Attributes?.Brightness ?? 0;
        }));
        _disposables.Add(EnableDimmer(upButton, cfg,true));
        _disposables.Add(EnableDimmer(downButton, cfg, false));
        logger.LogDebug(
            "Subscribing to buttonEvents from {dimmer} [{upButton} & {downButton}] to control {targetLightId}",
            cfg.DimmerId,
            upButton.EntityId, downButton.EntityId, cfg.TargetLightId);
        return cfg;
    }

    private IDisposable OnConsecutiveUpEvents(EventEntity upEvent, string eventType, TimeSpan eventDelay)
    {
        return upEvent.ConsecutiveEvents(eventDelay,eventType).Subscribe(e =>
            {
                if(e.Count == 0)
                    return;
                logger.LogDebug("Consecutive events {count} {states}", e.Count, e.StringJoin(s => s.New?.State??""));
                switch (e.Count)
                {
                    case 1:
                        if (!TargetEntity.IsOn())
                            SetState(true);
                        else
                        {
                            _expectedBrightness = (int?)TargetEntity.EntityState?.Attributes?.Brightness ?? _expectedBrightness;
                            SetBrightness(50);
                        }

                        break;
                    case 2:
                        ToggleWarm(TargetEntity);
                        break;
                    case 3:
                        SetBrightness(255);
                        break;
                }
            });
    }
    private IDisposable OnConsecutiveDownEvents(EventEntity upEvent, string eventType, TimeSpan eventDelay)
    {
        return upEvent.ConsecutiveEvents(eventDelay,eventType).SubscribeSafe(e =>
        {
            if(e.Count == 0)
                return;
            logger.LogDebug("Consecutive events {count} {states}", e.Count, e.StringJoin(s => s.New?.State??""));
            switch (e.Count)
            {
                case 1:
                    SetState(false);
                    break;
                case 2:
                case 3:
                    ToggleWarm(TargetEntity);
                    break;
            }
        });
    }

    private LightEntity TargetEntity => ha.LightEntity(appCfg.Value.TargetLightId);


    private IDisposable EnableDimmer(BinarySensorEntity upButton, DimmerSync cfg, bool dimming) => upButton.ToBooleanObservable()
            .Throttle(cfg.Timing.DebounceTime)
            .WhenTrueFor(cfg.Timing.HoldDelay, scheduler)
            .SubscribeSafe(v => { IsDimming = (v ? dimming : null); });


    private bool IsWaiting { get; set; }
    private DateTimeOffset LastServiceCall { get; set; }
    public async Task CallServiceAsyc(IEntityCore entity, string service, Func<EntityState,bool> monitor, object? data = null, CancellationToken token=default, TimeSpan? minTime=null,TimeSpan? maxTime=null)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(service);

        var serviceDomain = EntityId.From(service).Domain;

        serviceDomain ??= EntityId.From(entity.EntityId).Domain ?? throw new InvalidOperationException("EntityId must be formatted 'domain.name'");
        minTime??= TimeSpan.Zero;
        var call =entity.ToServiceCall(service,data);
        var stateMonitor = entity.ToStateMonitor(monitor);
        try
        {
            IsWaiting = true;
            DateTimeOffset start = DateTimeOffset.UtcNow;
            await serviceCaller.CallServiceAndWaitForChangeAsync(call, stateMonitor, maxTime??TimeSpan.FromSeconds(1), token);
            LastServiceCall = DateTimeOffset.UtcNow;
            if (LastServiceCall - start < minTime)
            {
//                logger.LogDebug("Waiting for minTime {minTime} after {elapsed}", minTime.Value.TotalMilliseconds, (DateTimeOffset.UtcNow - start).TotalMilliseconds);
                await Task.Delay(minTime.Value - (DateTimeOffset.UtcNow - start), token);
            }
        }
        finally
        {
            IsWaiting = false;
        }
    }
    private CancellationToken NewLoop(CancellationToken cancellationToken)
    {
        _loopTokenSource?.Dispose();
        _loopTokenSource = null;
        _loopTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        return _loopTokenSource.Token;
        
    }

    private bool NeedsTransition(LightEntity e, Desiredstate dst)
    {
        if (dst.State.HasValue && !e.IsToggleState(dst.State.Value))
            return true;
        var a = e.Attributes;
        if (a == null)
            return false;
        return dst.Dimming.HasValue ||
               dst.BrightnessStep.HasValue && ((int?)a.Brightness) != dst.BrightnessStep ||
               dst.ColorTemp.HasValue && ((int?)a.ColorTempPct()) != dst.ColorTemp;
    }

    private bool TryParseAttributes(EntityState? state,[NotNullWhen(true)] out LightAttributes? a)
    {
        a = state?.AttributesJson?.Deserialize<LightAttributes>(HassJsonContext.DefaultOptions);
        if (a == null)
        {
            logger.LogError("Attributes missing for entity {@entity}", state.EntityId);
            return false;
        }

        return true;
    }

    public async ValueTask DisposeAsync()
    {
        Disposed = true;
        if (_loopTokenSource != null) await CastAndDispose(_loopTokenSource);
        if (_longRunningTask != null) await CastAndDispose(_longRunningTask);
        serviceCaller.Dispose();
        _disposables?.Dispose();
        static async ValueTask CastAndDispose(IDisposable resource)
        {
            if (resource is IAsyncDisposable resourceAsyncDisposable)
                await resourceAsyncDisposable.DisposeAsync();
            else
                resource.Dispose();
        }
    }
}