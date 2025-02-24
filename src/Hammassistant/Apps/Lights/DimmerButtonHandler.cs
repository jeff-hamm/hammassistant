using System.Data;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Joins;
using System.Reactive.Subjects;
using System.Security.Cryptography;
using Hammlet.Extensions;
using Hammlet.Models.Base;
using NetDaemon.Client.HomeAssistant.Model;
using NetDaemon.Extensions.Observables;
using Reactive.Boolean;

namespace Hammlet.Apps.Lights;

public record TriToggle(TriToggleState State, ButtonEvent? Data)
    : ButtonTiming.StateChangeEvent<TriToggleState, ButtonEvent?>(State, Data);

//[NetDaemonApp]
//internal class DimmerSyncHandler(IAsyncHaContext ha, IScheduler scheduler, IAppConfig<DimmerSync> appCfg, ILogger<DimmerSyncHandler> logger) : IAsyncInitializable, IAsyncDisposable
//{
//    private DimmerSync Cfg { get; } = appCfg.Value;
//    private bool IsDisposed { get; set; }

//    private void Initialize()
//    {
//        var srcDimmer = ha.LightEntity(Cfg.DimmerId);
//        var upButton = ha.BinarySensor(Cfg.UpButtonId ?? Cfg.DimmerId + "_input_0");
//        var downButton = ha.BinarySensor(Cfg.DownButtonId ?? Cfg.DimmerId + "_input_1");
//        logger.LogDebug("Subscribing to buttonEvents from {dimmer} [{upButton} & {downButton}] to control {targetLightId}", 
//            srcDimmer.EntityId,
//            upButton.EntityId, downButton.EntityId, Cfg.TargetLightId);
//        var timing = Cfg.Timing;
//        var pressTracker = new Subject<ButtonEvent>();

//        var debouncedUp = 
//            upButton.ToBooleanObservable().Throttle(Cfg.Timing.DebounceTime, scheduler)
//                .Where(t => t)
////                .TimeInterval(scheduler)
//            .Publish()
//            .RefCount();
//        debouncedUp.SubscribeTrue(() => pressTracker.OnNext(ButtonEventType.Down.ToEvent()));
//        var heldDown = debouncedUp
//                .WhenTrueFor(Cfg.Timing.HoldDelay, scheduler)
//                    .Do(t => pressTracker.OnNext(new ButtonEvent(ButtonEventType.HeldDown, ButtonHoldingState.Started,
//                        DateTimeOffset.UtcNow)))
//                    .TimeInterval(scheduler)
//                    .Throttle(Cfg.Timing.DebounceTime, scheduler)
//                    .Take(Cfg.Timing.HoldTimeout)
//                    .FirstOrDefaultAsync(v => !v.Value)
//                    .Select(t => new ButtonEvent(ButtonEventType.HoldReleased, ButtonHoldingState.Completed,
//                        ButtonHeldTime: t.Interval))
//                    .Multicast(pressTracker)
//                    .RefCount();
//        //var pressed = debouncedUp
//        //    .Take(Cfg.Timing.HoldDelay).FirstOrDefaultAsync(v => !v)
//        //    .Select(t => new ButtonEvent(ButtonEventType.Pressed))
//        //    .Throttle(Cfg.Timing.DebounceTime, scheduler)
        
//        //Observable.While(() => !IsDisposed, heldDown)
//        //    .Subscribe();

//        //debouncedUp.WhenTrueFor(Cfg.Timing.HoldDelay, scheduler)
        
//        //debouncedUp.Subscribe()
//        //    .Do(v => pressTracker.OnNext(ButtonEventType.Down.ToEvent()))
//        //    .WhenTrueFor(Cfg.Timing.PressDelay, scheduler)
//        //    .Publish()
//        upButton.ToTriToggle(downButton)
//            .Then(s => s == TriToggleState.Up)
//            .DistinctUntilChanged()
//            .TimeInterval(scheduler)
//            .Collect(() => new TriToggle(TriToggleState.None,null),
//                (previous,newStateInterval) =>
//                {
//                    var (newState, interval) = newStateInterval;
//                    if (interval < Cfg.Timing.DebounceTime)
//                        return previous;
//                    var (oldState,data) = previous;
//                    if (newState == oldState)
//                    {
//                        if(newState == TriToggleState.None) return previous;
//                        data.HoldStart ??= DateTimeOffset.UtcNow;
//                        if (DateTimeOffset.UtcNow - previous.HoldStart > Cfg.Timing.HoldDelay)
//                        {
//                            return new TriToggle(newState, previous.Data with {
//                                HoldingState = ButtonHoldingState.Started});


//                        }
//                    }

//                    return (newStateInterval.Value,StateChange: previous.State,previous.Data) switch {
//                        ( var state , TriToggleState.None, null) => new (newState,ButtonEventType.Down.ToEvent(holdStart:DateTimeOffset.UtcNow)),
//                         var tuple when (tuple.Value == tuple.StateChange) => 
//                        _ => throw new ArgumentOutOfRangeException()
//                    })
//                    ;
//                });


//                //if (previous == null)
//                    //    return newState.Value == previous.Value ? null : (newState.Value == TriToggleState.Up ? ButtonEventType.Down) newState.Value : ;
//                    //if (newState.Value == TriToggleState.Up)
//                    //{
//                    //    if (lastState == ButtonEventType.Down)
//                    //        return previous.Append(ButtonEventType.Up);
//                    //    return previous.Append(ButtonEventType.Pressed);
//                    //}
//                    //if (newState.Value == TriToggleState.Down)
//                    //{
//                    //    if (lastState == ButtonEventType.Up)
//                    //        return previous.Append(ButtonEventType.Down);
//                    //    return previous.Append(ButtonEventType.Pressed);
//                    //}
//                    //return previous;
//            .DistinctUntilChanged()
//            .SubscribeAsync()
////        IDisposable? t = null;
////        upButton
////            .ToToggleEvents(scheduler)
////            .Subscribe(e =>
////            {
////                logger.LogInformation("Button 0 Input: {@0}", e.Data);
////                var targetEntity = ha.LightEntity(Cfg.TargetLightId);
////                if(!targetEntity.IsOn())
////                {
////                    targetEntity.TurnOn();
////                    return;
////                }
////                if (e.Data.IsPressAndAHalf)
////                {
////                    SetBrightness(255);
//////                targetEntity.MaxBrightnes();
////                }
////                DimmerButtonHandler(ha.LightEntity(Cfg.DimmerId), targetEntity, true, e, Cfg);
////            });
////        downButton.ToToggleEvents(scheduler).Subscribe(e =>
////        {
////            logger.LogInformation("Button 1 Input: {@0}", e.Data);
////            var targetEntity = ha.LightEntity(Cfg.TargetLightId);
////            if (e.Data.IsPressAndAHalf)
////            {
////                targetEntity.ToggleWarm();
////            }
////            DimmerButtonHandler(ha.LightEntity(Cfg.DimmerId), targetEntity, false, e, Cfg);
////        });
////        return Cfg;
//    }


//    public Task InitializeAsync(CancellationToken cancellationToken)
//    {
//        Initialize();
//            @this.TimeInterval(scheduler)
//                .Collect(() => (ButtonEvent?)null,
//                    (state, interval) =>
//                    {
//                        if(state == null)
//                            return interval.Value == TriToggleState.None ? null : new ButtonEvent(interval.Value == TriToggleState.Up ? ButtonEventType.Up : ButtonEventType.Down);
//                        if (interval.Interval < TimeSpan.FromMilliseconds(50))
//                            return state;
//                        if (interval.Value == TriToggleState.Up)
//                        {
//                            if (lastState == ButtonEventType.Down)
//                                return state.Append(ButtonEventType.Up);
//                            return state.Append(ButtonEventType.Pressed);
//                        }
//                        if (interval.Value == TriToggleState.Down)
//                        {
//                            if (lastState == ButtonEventType.Up)
//                                return state.Append(ButtonEventType.Down);
//                            return state.Append(ButtonEventType.Pressed);
//                        }
//                        return state;
//                    })
//                .Subscribe(x =>
//                {
//                    x.
//                        Console.WriteLine(x);
//                });

//            return new ButtonEventProducer(
//                @this.ToChangesOnlyBooleanObservable(),scheduler);
//        }
//    }

//    public ValueTask DisposeAsync()
//    {
//        throw new NotImplementedException();
//    }
//}