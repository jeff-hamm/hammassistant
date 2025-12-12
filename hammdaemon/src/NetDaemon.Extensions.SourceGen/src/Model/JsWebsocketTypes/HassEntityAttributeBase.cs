using System.Text.Json.Serialization;

namespace NetDaemon.Extensions.SourceGen.Model.JsWebsocket;

public class HassEntityAttributeBase
// export type HassEntityAttributeBase = {
//   friendly_name?: string;
//   unit_of_measurement?: string;
//   icon?: string;
//   entity_picture?: string;
//   supported_features?: number;
//   hidden?: boolean;
//   assumed_state?: boolean;
//   device_class?: string;
//   state_class?: string;
//   restored?: boolean;
// };
{
    [JsonPropertyName("friendly_name")]
    public string FriendlyName { get; set; }

    [JsonPropertyName("unit_of_measurement")]
    public string UnitOfMeasurement { get; set; }

    [JsonPropertyName("icon")]
    public string Icon { get; set; }

    [JsonPropertyName("entity_picture")]
    public string EntityPicture { get; set; }

    [JsonPropertyName("supported_features")]
    public int? SupportedFeatures { get; set; }

    [JsonPropertyName("hidden")]
    public bool? Hidden { get; set; }

    [JsonPropertyName("assumed_state")]
    public bool? AssumedState { get; set; }

    [JsonPropertyName("device_class")]
    public string DeviceClass { get; set; }

    [JsonPropertyName("state_class")]
    public string StateClass { get; set; }

    [JsonPropertyName("restored")]
    public bool? Restored { get; set; }
}