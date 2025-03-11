using System.Collections.Generic;
using Hammlet.NetDaemon.Models;
using NetDaemon.Extensions.Hammlet;
using NetDaemon.HassModel.Entities;

namespace HassModel;

public static class EntityExtensions
{
        
    public static string Domain(this Entity entity) => EntityId.From(entity.EntityId).Domain ?? "";
    public static EventEntity Event(this IHaContext @this, EntityId entityId) =>
        @this.Entity<EventEntity>(entityId.InDomain("event"));
    public static IEnumerable<EventEntity> Events(this IHaContext @this, IEnumerable<string> entityIds) =>
        @this.Entities<EventEntity>(entityIds);
    public static LightEntity LightEntity(this IHaContext @this, string entityId) =>
        new (@this.Entity(entityId));
    public static BinarySensorEntity BinarySensor(this IHaContext @this, EntityId entityId) =>
        new (@this.Entity(entityId.InDomain("binary_sensor")));


}