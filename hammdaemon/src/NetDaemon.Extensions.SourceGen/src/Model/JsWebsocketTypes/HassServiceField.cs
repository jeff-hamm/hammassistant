using System.Text.Json.Serialization;
using Dunet;
namespace NetDaemon.Extensions.SourceGen.Model.JsWebsocket;

[Union]
public partial record JsonNumber
{
    public partial record Int(int Value);
    public partial record Double(double Value);
}

[Union]
public partial record StringBoolNumber
{
    public partial record String(string Value);
    public partial record Bool(bool Value);
    public partial record Number(JsonNumber Value);
}

//  {
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
public class HassServiceField
{
    [JsonPropertyName("example")]
    public StringBoolNumber? Example { get; set; }

    [JsonPropertyName("default")]
    public JsonAny? Default { get; set; }

    [JsonPropertyName("required")]
    public bool? Required { get; set; }

    [JsonPropertyName("advanced")]
    public bool? Advanced { get; set; }

    [JsonPropertyName("selector")]
    public JsonAny.JsonObject? Selector { get; set; }

    [JsonPropertyName("filter")]
    public HassServiceFieldFilter Filter { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}