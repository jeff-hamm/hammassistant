using System.Text.Json.Serialization;

namespace NetDaemon.Extensions.SourceGen.Model.JsWebsocket;

public class StateChangedEvent : HassEventBase
{
    [JsonPropertyName("event_type")]
    public string EventType { get; set; } = "state_changed";

    [JsonPropertyName("data")]
    public StateChangedEventData Data { get; set; }
}