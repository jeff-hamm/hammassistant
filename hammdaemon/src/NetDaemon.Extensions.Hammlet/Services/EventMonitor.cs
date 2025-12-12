using System.Text.Json;
using NetDaemon.Client.HomeAssistant.Model;
using NetDaemon.HassModel;
using NetDaemon.HassModel.Entities;

namespace NetDaemon.Extensions.Hammlet.Services;
using Event=HassEvent;
public record class EventMonitor(string EventType, Func<Event, bool> Filter, Func<Event, bool>? FoundFunc);

public record EntityChangedEvent(string EventType, string EntityId, Event Event)
{
    public EntityChangedEvent(Event hassEvent) : this(hassEvent.EventType, hassEvent.DataElement?.GetProperty("entity_id").GetString()!, hassEvent)
    {

    }
    private Lazy<EntityState?> _latestState = new(() => Event.DataElement?.GetProperty("new_state").Deserialize<EntityState>());
    public EntityState State => _latestState.Value!;
}

public record EntityStateEventMonitor(string EntityId, Func<EntityChangedEvent, bool> Found) : EventMonitor("state_changed",
    e => e.EventType == "state_changed" && e.DataElement?.GetProperty("entity_id").GetString() == EntityId, e => Found(new EntityChangedEvent(e)));