// Use unique namespaces for your apps if you going to share with others to avoid
// conflicting names

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hammlet.NetDaemon.Extensions;
using Hammlet.NetDaemon.Models;
using HassModel;
using NetDaemon.Extensions.Hammlet;
using NetDaemon.Extensions.MqttEntityManager;
using NetDaemon.Extensions.Observables;
using NetDaemon.HassModel.Entities;

namespace Hammlet.Apps.Lights;

/// <summary>
///     Showcase using the new HassModel API and turn on light on movement
/// </summary>
[NetDaemonApp]
public class WatchReferenceLight
{
    private readonly IHaContext _ha;
    private readonly ILogger<WatchReferenceLight> _logger;
    private readonly IAppConfig<ReferenceLight> _config;

    /// <summary>
    ///     Showcase using the new HassModel API and turn on light on movement
    /// </summary>
    public WatchReferenceLight(IHaContext ha, LightEntities entities, ILogger<WatchReferenceLight> logger,
        //IMqttEntityManager entityManager, 
        IAppConfig<ReferenceLight> config)
    {
        _ha = ha;
        _logger = logger;
        _config = config;
        InitializeAsync(CancellationToken.None);
    }

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

    //private async Task CreateReferenceLight(EntityId entityId, CancellationToken token)
    //{
    //    await entityManager.CreateAsync(entityId, new EntityCreationOptions()
    //    {
            
    //    });
    //}

    public void InitializeAsync(CancellationToken cancellationToken)
    {
        string refId = _config.Value.EntityId;
        if (_ha.GetState(refId) == null)
        {
            throw new System.Exception($"Reference light {refId} not found");
//            await CreateReferenceLight(cancellationToken);
        }
        var entities =
            _config.Value.TrackedEntities.SelectMany(e => GetEntities(_ha, e)).Distinct()
                .Select(eid => _ha.LightEntity(eid))
                .ToArray();
        foreach (var e in entities)
        {
            _logger.LogDebug("Found entity {entityId} with {state}", e.EntityId, e.State);
        }

        entities.StateChanges()
            .Where(e => e.New?.IsOn() == true && e.Old?.IsOn() != true)
            .SubscribeSafe(e =>
            {
                var target = _ha.LightEntity(_config.Value.EntityId);
                if (e.Entity.IsOn() && target.IsOn() && e.Entity.CopyParametersFrom(target) is { } p)
                {
                    _logger.LogDebug("Entity {entityId} turned on copying parameters from reference light {@parameters}", e.Entity.EntityId, p);
                    e.Entity.TurnOn(p);
                }
            });

//            (entities.ColorBulbs.EntityState?.Attributes?.EntityId ?? []).Union(trackedEntities.Select(s => s.EntityId).ToArray());
        _ha.LightEntity(_config.Value.EntityId).StateAllChanges()
            .SubscribeSafe(e =>
        {
            if (e.Entity.IsOff()) return;
            _logger.LogDebug("Reference entity {entityId} changed, {@parameters} updating child lights", e.Entity.EntityId, e.Entity.Attributes);
            foreach (var id in entities)
            {
                var l = new LightEntity(id);
                if (l.IsOn() &&
                    l.CopyParametersFrom(e.Entity) is { } parameters)
                {
                    l.TurnOn(parameters);
                }
            }
        });

    }
}