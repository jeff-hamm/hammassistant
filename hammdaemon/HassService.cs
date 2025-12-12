using System.Text.Json.Serialization;

namespace NetDaemon.Extensions.SourceGen.Model.JsWebsocket;



// export type HassService = {
//   name?: string;
//   description: string;
//   target?: {} | null;
//   fields: {
//     [field_name: string]: {
//       example?: string | boolean | number;
//       default?: unknown;
//       required?: boolean;
//       advanced?: boolean;
//       selector?: {};
//       filter?: {
//         supported_features?: number[];
//         attribute?: Record<string, any[]>;
//       };
//       // Custom integrations don't use translations and still have name/description
//       name?: string;
//       description: string;
//     };
//   };
//   response?: { optional: boolean };
// };
public class HassService
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("target")]
    public JsonAny.JsonObject? Target { get; set; }

    [JsonPropertyName("fields")]
    public Dictionary<string, HassServiceField> Fields { get; set; }

    [JsonPropertyName("response")]
    public HassServiceResponse Response { get; set; }
}