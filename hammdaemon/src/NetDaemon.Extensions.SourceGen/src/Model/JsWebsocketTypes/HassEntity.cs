using System.Text.Json.Serialization;

namespace NetDaemon.Extensions.SourceGen.Model.JsWebsocket;

public class HassEntity : HassEntityBase
// export type HassEntity = HassEntityBase & {
//   attributes: { [key: string]: any };
// };
{
    [JsonPropertyName("attributes")]
    public Dictionary<string, JsonAny> AdditionalAttributes { get; set; }
}