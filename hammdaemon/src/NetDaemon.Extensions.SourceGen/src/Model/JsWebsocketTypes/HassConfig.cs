using System.Text.Json.Serialization;
// export type HassConfig = {
//   latitude: number;
//   longitude: number;
//   elevation: number;
//   radius: number;
//   unit_system: {
//     length: string;
//     mass: string;
//     volume: string;
//     temperature: string;
//     pressure: string;
//     wind_speed: string;
//     accumulated_precipitation: string;
//   };
//   location_name: string;
//   time_zone: string;
//   components: string[];
//   config_dir: string;
//   allowlist_external_dirs: string[];
//   allowlist_external_urls: string[];
//   version: string;
//   config_source: string;
//   recovery_mode: boolean;
//   safe_mode: boolean;
//   state: "NOT_RUNNING" | "STARTING" | "RUNNING" | "STOPPING" | "FINAL_WRITE";
//   external_url: string | null;
//   internal_url: string | null;
//   currency: string;
//   country: string | null;
//   language: string;
// };
namespace NetDaemon.Extensions.SourceGen.Model.JsWebsocket;

public class HassConfig
{
    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }

    [JsonPropertyName("elevation")]
    public double Elevation { get; set; }

    [JsonPropertyName("radius")]
    public double Radius { get; set; }

    [JsonPropertyName("unit_system")]
    public UnitSystem UnitSystem { get; set; }

    [JsonPropertyName("location_name")]
    public string LocationName { get; set; }

    [JsonPropertyName("time_zone")]
    public string TimeZone { get; set; }

    [JsonPropertyName("components")]
    public List<string> Components { get; set; }

    [JsonPropertyName("config_dir")]
    public string ConfigDir { get; set; }

    [JsonPropertyName("allowlist_external_dirs")]
    public List<string> AllowlistExternalDirs { get; set; }

    [JsonPropertyName("allowlist_external_urls")]
    public List<string> AllowlistExternalUrls { get; set; }

    [JsonPropertyName("version")]
    public string Version { get; set; }

    [JsonPropertyName("config_source")]
    public string ConfigSource { get; set; }

    [JsonPropertyName("recovery_mode")]
    public bool RecoveryMode { get; set; }

    [JsonPropertyName("safe_mode")]
    public bool SafeMode { get; set; }

    [JsonPropertyName("state")]
    public string State { get; set; }

    [JsonPropertyName("external_url")]
    public string ExternalUrl { get; set; }

    [JsonPropertyName("internal_url")]
    public string InternalUrl { get; set; }

    [JsonPropertyName("currency")]
    public string Currency { get; set; }

    [JsonPropertyName("country")]
    public string Country { get; set; }

    [JsonPropertyName("language")]
    public string Language { get; set; }
}