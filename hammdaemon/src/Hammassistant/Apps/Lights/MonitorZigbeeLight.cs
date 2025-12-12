//using Hammlet.NetDaemon.Models;
//using NetDaemon.HassModel.Entities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Hammlet.Apps.Lights;
//[NetDaemonApp]
//internal class MonitorZigbeeLight
//{
//    public MonitorZigbeeLight(IHaContext ha, LightEntities entities, IAppConfig<ZigbeeMonitorConifg> config)
//    {

//        var configId = config.Value.Id;
//        string refId = config.Value.EntityId;
//        var ids =
//            config.Value.TrackedEntities.SelectMany(e => GetEntities(ha, e)).Distinct().ToArray();
////            (entities.ColorBulbs.EntityState?.Attributes?.EntityId ?? []).Union(trackedEntities.Select(s => s.EntityId).ToArray());
//        ha.StateChanges().Where(s => 
//            ids.Contains(s.Entity.EntityId) &&
//                (s.New?.IsOn() == true && s.Old?.IsOff() == true))
//            .Subscribe(e =>
//            {
//                var referenceEntity = new LightEntity(ha.Entity(refId));
//                    if (referenceEntity.IsOn())
//                    {
//                        var l = new LightEntity(e.Entity);
//                        if (l.CopyParameters(referenceEntity) is { } parameters)
//                        {
//                            l.TurnOn(parameters);
//                        }
//                    }
//            });
//        entities.DefaultLight.StateAllChanges().Subscribe(e =>
//        {
//            if (e.Entity.IsOff()) return;
//            foreach (var id in ids)
//            {
//                var l = new LightEntity(ha.Entity(id));
//                if (l.IsOn() &&
//                    l.CopyParameters(e.Entity) is { } parameters)
//                {
//                    l.TurnOn(parameters);
//                }
//            }
//        });
//    }

//    private IEnumerable<string> GetEntities(IHaContext ha, string s)
//    {
//        if (new LightEntity(ha, s).EntityState?.Attributes?.EntityId is { } entities)
//        {
//            foreach(var entity in entities)
//            {
//                foreach(var e in GetEntities(ha,entity))
//                    yield return entity;
//            }
//        }
//        else
//            yield return s;
//    }
//}

//public class ZigbeeMonitorConifg
//{
//    internal Guid Id { get; } = Guid.NewGuid();
//    public string EntityId { get; set; } = "light.default_light";
//}
