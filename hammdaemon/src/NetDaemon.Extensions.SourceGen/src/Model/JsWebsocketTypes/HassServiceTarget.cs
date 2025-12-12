using System.Text.Json.Serialization;

namespace NetDaemon.Extensions.SourceGen.Model.JsWebsocket;
public class HassServiceTarget
// export type HassServiceTarget = {
//   entity_id?: string | string[];
//   device_id?: string | string[];
//   area_id?: string | string[];
//   floor_id?: string | string[];
//   label_id?: string | string[];
// };
{
    [JsonPropertyName("entity_id")]
    [JsonConverter(typeof(StringArrayConverter))]
    public StringArray? EntityId { get; set; }

    [JsonPropertyName("device_id")]
    [JsonConverter(typeof(StringArrayConverter))]
    public StringArray? DeviceId { get; set; }

    [JsonPropertyName("area_id")]
    [JsonConverter(typeof(StringArrayConverter))]
    public StringArray? AreaId { get; set; }

    [JsonPropertyName("floor_id")]
    [JsonConverter(typeof(StringArrayConverter))]
    public StringArray? FloorId { get; set; }

    [JsonPropertyName("label_id")]
    [JsonConverter(typeof(StringArrayConverter))]
    public StringArray? LabelId { get; set; }
}