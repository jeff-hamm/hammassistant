using System.Text.Json.Serialization;

namespace NetDaemon.Extensions.SourceGen.Model.JsWebsocket;

public class HassEntityBase
{
    [JsonPropertyName("entity_id")]
    public string EntityId { get; set; }

    [JsonPropertyName("state")]
    public string State { get; set; }

    [JsonPropertyName("last_changed")]
    public string LastChanged { get; set; }

    [JsonPropertyName("last_updated")]
    public string LastUpdated { get; set; }

    [JsonPropertyName("attributes")]
    public virtual HassEntityAttributeBase Attributes { get; set; }

    [JsonPropertyName("context")]
    public Context Context { get; set; }
}