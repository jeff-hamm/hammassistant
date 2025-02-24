using System.Text.Json;
using System.Text.Json.Serialization;

namespace NetDaemon.Extensions.SourceGen.Model.FrontEndTypes;

public record EntityRegistryEntry
{
    [JsonPropertyName("id")] public string Id { get; init; }
    [JsonPropertyName("entity_id")] public string EntityId { get; init; }
    [JsonPropertyName("name")] public string? Name { get; init; }
    [JsonPropertyName("icon")] public string? Icon { get; init; }
    [JsonPropertyName("platform")] public string Platform { get; init; }
    [JsonPropertyName("config_entry_id")] public string? ConfigEntryId { get; init; }
    [JsonPropertyName("device_id")] public string? DeviceId { get; init; }
    [JsonPropertyName("area_id")] public string? AreaId { get; init; }
    [JsonPropertyName("labels")] public IReadOnlyList<string> Labels { get; init; } = [];
    [JsonPropertyName("disabled_by")] public string? DisabledBy { get; init; }
    [JsonPropertyName("hidden_by")] public string? HiddenBy { get; init; }
    [JsonPropertyName("entity_category")] public string? EntityCategory { get; init; }
    [JsonPropertyName("has_entity_name")] public bool HasEntityName { get; init; }
    [JsonPropertyName("original_name")] public string? OriginalName { get; init; }
    [JsonPropertyName("unique_id")] public string UniqueId { get; init; }
    [JsonPropertyName("translation_key")] public string? TranslationKey { get; init; }
    [JsonPropertyName("options")] public EntityRegistryOptions? Options { get; init; }
    [JsonPropertyName("categories")] public IReadOnlyDictionary<string, string> Categories { get; init; } = new Dictionary<string, string>();
    [JsonPropertyName("created_at")] public double? CreatedAt { get; init; }
    [JsonPropertyName("modified_at")] public double? ModifiedAt { get; init; }
}

public record ExtEntityRegistryEntry : EntityRegistryEntry
{
    [JsonPropertyName("capabilities")] public IReadOnlyDictionary<string, JsonElement> Capabilities { get; init; } = new Dictionary<string, JsonElement>();
    [JsonPropertyName("original_icon")] public string? OriginalIcon { get; init; }
    [JsonPropertyName("device_class")] public string? DeviceClass { get; init; }
    [JsonPropertyName("original_device_class")] public string? OriginalDeviceClass { get; init; }
    [JsonPropertyName("aliases")] public IReadOnlyList<string> Aliases { get; init; } = [];
}
public record SensorEntityOptions
{
    [JsonPropertyName("display_precision")] public int? DisplayPrecision { get; init; }
    [JsonPropertyName("suggested_display_precision")] public int? SuggestedDisplayPrecision { get; init; }
    [JsonPropertyName("unit_of_measurement")] public string? UnitOfMeasurement { get; init; }
}

public record LightEntityOptions
{
    [JsonPropertyName("favorite_colors")] public IReadOnlyList<LightColor>? FavoriteColors { get; init; }
}

public record NumberEntityOptions
{
    [JsonPropertyName("unit_of_measurement")] public string? UnitOfMeasurement { get; init; }
}

public record LockEntityOptions
{
    [JsonPropertyName("default_code")] public string? DefaultCode { get; init; }
}

public record AlarmControlPanelEntityOptions
{
    [JsonPropertyName("default_code")] public string? DefaultCode { get; init; }
}

public record WeatherEntityOptions
{
    [JsonPropertyName("precipitation_unit")] public string? PrecipitationUnit { get; init; }
    [JsonPropertyName("pressure_unit")] public string? PressureUnit { get; init; }
    [JsonPropertyName("temperature_unit")] public string? TemperatureUnit { get; init; }
    [JsonPropertyName("visibility_unit")] public string? VisibilityUnit { get; init; }
    [JsonPropertyName("wind_speed_unit")] public string? WindSpeedUnit { get; init; }
}

public record SwitchAsXEntityOptions
{
    [JsonPropertyName("entity_id")] public string EntityId { get; init; }
    [JsonPropertyName("invert")] public bool Invert { get; init; }
}

public record EntityRegistryOptions
{
    [JsonPropertyName("number")] public NumberEntityOptions? Number { get; init; }
    [JsonPropertyName("sensor")] public SensorEntityOptions? Sensor { get; init; }
    [JsonPropertyName("alarm_control_panel")] public AlarmControlPanelEntityOptions? AlarmControlPanel { get; init; }
    [JsonPropertyName("lock")] public LockEntityOptions? Lock { get; init; }
    [JsonPropertyName("weather")] public WeatherEntityOptions? Weather { get; init; }
    [JsonPropertyName("light")] public LightEntityOptions? Light { get; init; }
    [JsonPropertyName("switch_as_x")] public SwitchAsXEntityOptions? SwitchAsX { get; init; }
    [JsonPropertyName("conversation")] public IReadOnlyDictionary<string, JsonAny>? Conversation { get; init; }
    [JsonPropertyName("cloud.alexa")] public IReadOnlyDictionary<string, JsonAny>? CloudAlexa { get; init; }
    [JsonPropertyName("cloud.google_assistant")] public IReadOnlyDictionary<string, JsonAny>? CloudGoogleAssistant { get; init; }
}
