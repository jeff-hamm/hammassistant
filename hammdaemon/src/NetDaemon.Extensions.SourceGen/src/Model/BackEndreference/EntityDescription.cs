using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hammlet.Models.BackEnd
{
    public class EntityDescription
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("device_class")]
        public string DeviceClass { get; set; }

        [JsonPropertyName("entity_category")]
        public string EntityCategory { get; set; }

        [JsonPropertyName("entity_registry_enabled_default")]
        public bool EntityRegistryEnabledDefault { get; set; } = true;

        [JsonPropertyName("entity_registry_visible_default")]
        public bool EntityRegistryVisibleDefault { get; set; } = true;

        [JsonPropertyName("force_update")]
        public bool ForceUpdate { get; set; } = false;

        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        [JsonPropertyName("has_entity_name")]
        public bool HasEntityName { get; set; } = false;

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("translation_key")]
        public string TranslationKey { get; set; }

        [JsonPropertyName("translation_placeholders")]
        public Dictionary<string, string> TranslationPlaceholders { get; set; }

        [JsonPropertyName("unit_of_measurement")]
        public string UnitOfMeasurement { get; set; }

        public string Attribution { get; set; }
        public int? SupportedFeatures { get; set; }
        public bool AssumedState { get; set; }
        public string EntityPicture { get; set; }
        public string UniqueId { get; set; }
        public bool Available { get; set; }
        public bool ShouldPoll { get; set; }
        public Dictionary<string, object> CapabilityAttributes { get; set; }
        public Dictionary<string, object> StateAttributes { get; set; }
        public Dictionary<string, object> ExtraStateAttributes { get; set; }
        public DeviceInfo DeviceInfo { get; set; }
    }
}
