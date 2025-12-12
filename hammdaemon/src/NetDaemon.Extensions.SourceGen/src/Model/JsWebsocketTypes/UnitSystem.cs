using System.Text.Json.Serialization;

namespace NetDaemon.Extensions.SourceGen.Model.JsWebsocket;

public class UnitSystem
{
    [JsonPropertyName("length")]
    public string Length { get; set; }

    [JsonPropertyName("mass")]
    public string Mass { get; set; }

    [JsonPropertyName("volume")]
    public string Volume { get; set; }

    [JsonPropertyName("temperature")]
    public string Temperature { get; set; }

    [JsonPropertyName("pressure")]
    public string Pressure { get; set; }

    [JsonPropertyName("wind_speed")]
    public string WindSpeed { get; set; }

    [JsonPropertyName("accumulated_precipitation")]
    public string AccumulatedPrecipitation { get; set; }
}