// Use unique namespaces for your apps if you going to share with others to avoid
// conflicting names

using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Hammlet.Models.Enums;
using Hammlet.NetDaemon.Extensions;
using Hammlet.NetDaemon.Models;
using HassModel;
using NetDaemon.Extensions.Hammlet;
using NetDaemon.Extensions.MqttEntityManager;
using NetDaemon.Extensions.Observables;
using NetDaemon.HassModel.Entities;
using PuppeteerSharp;

namespace Hammlet.Apps.Lights;

/// <summary>
///     Showcase using the new HassModel API and turn on light on movement
/// </summary>
[NetDaemonApp]
public class WatchReferenceLight(IHaContext ha, 
    ILogger<WatchReferenceLight> logger,
    IMqttEntityManager entityManager, 
    IHaRegistry registry,
    IAppConfig<ReferenceLight> config) : IAsyncInitializable
{
    private LightEntity[]? _trackedEntities;

    private IEnumerable<string> GetEntities(IHaContext ha, string s)
    {
        if (new LightEntity(ha, s).EntityState?.Attributes?.EntityId is { } entities)
        {
            foreach(var entity in entities)
            {
                foreach(var e in GetEntities(ha,entity))
                    yield return entity;
            }
        }
        else
            yield return s;
    }

    private async Task CreateReferenceLight(EntityId entityId, LightEntity[] tracking, CancellationToken token)
    {
        if (Regex.Match(entityId.Entity, @"^(?<base>([\w\d]+_)*([\w\d]+))(_(?<num>\d+))?") is not { Success: true } m)
        {
            logger.LogError("Could not create reference light, invalid id {entityId}",entityId);
            return;
        }

        var rootName = m.Groups["base"].Value;
        var num = m.Groups["num"] is { Success: true } n ? Int32.Parse(n.Value.Substring(1)) : 1;
        var baseName = Regex.Replace(rootName, @"(_|^)\w", v => v.Value.Replace("_"," ").ToUpper());
        var name = baseName + " " +
                   num.ToString();
        await entityManager.CreateAsync(entityId, new EntityCreationOptions()
        {
            Name = name,
            Persist = true,
            DeviceClass = entityId.Domain,
            UniqueId = entityId.Entity,
        }, new
        {
            brightness = true,
            color_temp_kelvin = true,
            device = new
            {
                manufacturer = "Hammlet",
                model = baseName,
                name = baseName,
                identifiers = new[] { rootName },
                sw_version = "1.0",
            },
            effect = true,
            effect_list = tracking.SelectMany(e => e.Attributes?.EffectList ?? []).Distinct().Order().ToArray(),
            min_kelvin = tracking.Min(e => e.Attributes?.MinColorTempKelvin),
            max_kelvin = tracking.Max(e => e.Attributes?.MaxColorTempKelvin ),
            min_mireds = tracking.Min(e => e.Attributes?.MinMireds),
            max_mireds = tracking.Max(e => e.Attributes?.MaxMireds),
            optimistic = true,
            platform = "light",
            retain = true,
            schema = "json",
            supported_color_modes = GetSupportedModes(tracking).Select(m => 
                JsonNamingPolicy.SnakeCaseLower.ConvertName(m.ToString())),
        });
        (await entityManager.PrepareCommandSubscriptionAsync(entityId).ConfigureAwait(false))
            .Subscribe(new Action<string>(async state =>
            {
                await entityManager.SetStateAsync(entityId, state).ConfigureAwait(false);
            }));
        await entityManager.SetStateAsync(entityId,
            JsonSerializer.Serialize(
            new { state="on", brightness=255,color_mode="color_temp",color_temp_kelvin=3000}));
    }

    private static ColorMode[]? GetSupportedModes(LightEntity[] tracking)
    {
        var modes = tracking.SelectMany(e => e.Attributes?.SupportedColorModes ?? []).Distinct().ToArray();
        if (modes.Length == 0)
            return null;
        if(modes.Length > 1)
            return modes.Where(m => m is not ColorMode.Brightness and not ColorMode.OnOff).ToArray();
        return modes;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {

        _trackedEntities = UpdateEntitiesList();

        ha.Events.SubscribeSafe(e =>
        {
            if (e.EventType == "label_registry_updated" || e.EventType == "entity_registry_updated")
                _trackedEntities = UpdateEntitiesList();
        });
        string refId = config.Value.EntityId;
        if (ha.GetState(refId) == null && config.Value.CreateReferenceLight)
        {
            await CreateReferenceLight(config.Value.EntityId,_trackedEntities,cancellationToken);
        }

        ha.StateChanges().Where(e =>
                _trackedEntities.Any(t => t.EntityId == e.Entity.EntityId) && e.New?.IsOn() == true && e.Old?.IsOn() != true)
            .SubscribeSafe(e =>
            {
                var referenceLight = ha.LightEntity(refId);
                if (e.Entity.IsOn() && referenceLight.IsOn() && 
                    new LightEntity(e.Entity) is {} l && l.CopyParametersFrom(referenceLight)
                    is { } p)
                {
                    logger.LogDebug("Entity {entityId} turned on copying parameters from reference light {@parameters}", e.Entity.EntityId, p);
                    l.TurnOn(p);
                }
            });

//            (entities.ColorBulbs.EntityState?.Attributes?.EntityId ?? []).Union(trackedEntities.Select(s => s.EntityId).ToArray());
        ha.LightEntity(refId).StateAllChanges()
            .SubscribeSafe(e =>
        {
            if (e.Entity.IsOff() || e.New?.Attributes?.ToParameters() is not {} refParameters) return;
            logger.LogDebug("Reference entity {entityId} changed, {@parameters} updating child lights", e.Entity.EntityId, e.New?.Attributes);
            foreach (var entity in _trackedEntities)
            {
                if (entity.IsOn())
                {
                    entity.TurnOn(entity.CopyParametersFrom(refParameters));
                }
            }
        });

    }

    private LightEntity[] UpdateEntitiesList()
    {
        logger.LogDebug("Updating Tracked Entities");
        var entitiesList =
            config.Value.TrackedEntities
                .SelectMany(e => GetEntities(ha, e))
                .Distinct()
                .ToArray();
        
        IEnumerable<LightEntity> labelEntities;
        if (registry.Labels.FirstOrDefault(l => l.Name == config.Value.TracksReferenceLabel) is { } l)
        {
            labelEntities = l.Entities
                .Where(e => e.Domain() == "light" && !entitiesList.Contains(e.EntityId))
                .Select(e => new LightEntity(e));
        }
        else
            labelEntities = Enumerable.Empty<LightEntity>();
        
        var entities = entitiesList
            .Select(e => new LightEntity(ha, e))
            .Concat(labelEntities)
            .ToArray();
        if (logger.IsEnabled(LogLevel.Debug))
        {
            foreach (var eId in entities)
            {
                if(_trackedEntities == null || _trackedEntities.All(e => e.EntityId != eId.EntityId))
                    logger.LogDebug("Found entity {entityId} with {state}", eId.EntityId, eId.State);
            }
        }

        return _trackedEntities = entities;
    }
}