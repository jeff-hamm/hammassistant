using System.Text.Json.Serialization;

namespace NetDaemon.Extensions.SourceGen.Model.JsWebsocket;

public class HassServiceResponse
{
    [JsonPropertyName("optional")]
    public bool Optional { get; set; }
}