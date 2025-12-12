using System.Text.Json.Serialization;

namespace NetDaemon.Extensions.SourceGen.Model.JsWebsocket;

// export type MessageBase = {
//   id?: number;
//   type: string;
//   [key: string]: any;
// };
public class MessageBase
{
    [JsonPropertyName("id")]
    public int? Id { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonAny> AdditionalData { get; set; }
}