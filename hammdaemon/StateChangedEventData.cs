using System.Text.Json.Serialization;

namespace NetDaemon.Extensions.SourceGen.Model.JsWebsocket;

public class StateChangedEventData
{
    [JsonPropertyName("entity_id")]
    public string EntityId { get; set; }

    [JsonPropertyName("new_state")]
    public HassEntity? NewState { get; set; }

    [JsonPropertyName("old_state")]
    public HassEntity? OldState { get; set; }
}