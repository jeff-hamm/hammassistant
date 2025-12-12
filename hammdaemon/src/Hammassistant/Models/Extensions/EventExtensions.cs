using System.Collections.Generic;
using Hammlet.NetDaemon.Models;
using NetDaemon.Extensions.Observables;
using NetDaemon.HassModel.Entities;

namespace Hammlet.Models.Extensions;

public static class EventExtensions
{
    public static IObservable<IList<StateChange<EventEntity,EntityState<EventAttributes>>>> ConsecutiveEvents(this EventEntity upEvent, TimeSpan eventDelay, string eventType)
    {
        return upEvent.ConsecutiveChanges<EventEntity,EventAttributes>(eventDelay,
            e => e.New?.Attributes?.EventType?.ToLowerInvariant() == eventType);
    }
    public static IObservable<IList<StateChange<TEntity,EntityState<TAttributes>>>> ConsecutiveChanges<TEntity,TAttributes>(this TEntity upEvent, TimeSpan eventDelay, Func<StateChange<TEntity,EntityState<TAttributes>>,bool> predicate) 
        where TEntity : Entity<TEntity,EntityState<TAttributes>,TAttributes> 
        where TAttributes : class
    {
        return upEvent.StatefulAll()
            .Where(predicate)
            .Publish((sq => 
                sq.Buffer(sq.Throttle(eventDelay))));
    }

    public static IObservable<Event> Event(this EventEntity @this) =>
        @this.HaContext.Event(@this);
    public static IObservable<Event> Event(this IHaContext @this, EventEntity entity) =>
        @this.Events.Where(e => e.EventType == entity.EntityId);

    public static string? EventType(this EventEntity @this) => @this.Attributes?.EventType;
    public static TEnum? EventType<TEnum>(this EventEntity @this)
        where TEnum : struct
        => Enum.TryParse<TEnum>(@this.Attributes?.EventType, out var r) ? r : null;


    public static void PrintEntityInfo(this Entity? entity)
    {
        if (entity == null)
        {
            Console.WriteLine("Entity is null");
            return;
        }
        Console.WriteLine($"Info: {entity.EntityId}");
        Console.WriteLine(entity.ToString());
        Console.WriteLine(entity.HaContext.GetState(entity.EntityId)?.ToString());
        Console.WriteLine(entity.Attributes?.ToString());
        Console.WriteLine("Registration");
        Console.WriteLine(entity.Registration);
        Console.WriteLine("Device");
        Console.WriteLine(entity.Registration?.Device);
    }

}