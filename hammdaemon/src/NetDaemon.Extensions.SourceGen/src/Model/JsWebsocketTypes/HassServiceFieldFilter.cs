using System.Text.Json.Serialization;

namespace NetDaemon.Extensions.SourceGen.Model.JsWebsocket;

public class HassServiceFieldFilter
{
    [JsonPropertyName("supported_features")]
    public List<int> SupportedFeatures { get; set; }

    [JsonPropertyName("attribute")]
    public Dictionary<string, List<JsonAny>> Attribute { get; set; }
}