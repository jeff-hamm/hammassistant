using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDaemon.Extensions.SourceGen.src.Model.Area;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Dunet;

namespace NetDaemon.Extensions.SourceGen.src.Model.Area;
// TypeScript definition:
// export interface AreaRegistryEntry {
//     area_id: string;
//     name: string;
//     normalized_name: string;
//     picture?: string;
// }

public class AreaRegistryEntry
{
    [JsonPropertyName("area_id")]
    public string AreaId { get; set; } = null!;

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("normalized_name")]
    public string NormalizedName { get; set; } = null!;

    [JsonPropertyName("picture")]
    public string? Picture { get; set; }
}

// TypeScript definition:
// export interface AreaRegistryEntryMutableParams {
//     name: string;
//     picture?: string;
// }

public class AreaRegistryEntryMutableParams
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("picture")]
    public string? Picture { get; set; }
}

// TypeScript definition:
// export interface DeviceRegistryEntry {
//     id: string;
//     name: string;
//     area_id: string;
//     config_entries: string[];
//     connections: [string, string][];
//     identifiers: [string, string][];
//     manufacturer?: string;
//     model?: string;
//     sw_version?: string;
//     via_device_id?: string;
//     disabled_by?: string;
//     entry_type?: string;
//     name_by_user?: string;
//     device_class?: string;
// }

public class DeviceRegistryEntry
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("area_id")]
    public string AreaId { get; set; } = null!;

    [JsonPropertyName("config_entries")]
    public List<string> ConfigEntries { get; set; } = new();

    [JsonPropertyName("connections")]
    public List<(string, string)> Connections { get; set; } = new();

    [JsonPropertyName("identifiers")]
    public List<(string, string)> Identifiers { get; set; } = new();

    [JsonPropertyName("manufacturer")]
    public string? Manufacturer { get; set; }

    [JsonPropertyName("model")]
    public string? Model { get; set; }

    [JsonPropertyName("sw_version")]
    public string? SwVersion { get; set; }

    [JsonPropertyName("via_device_id")]
    public string? ViaDeviceId { get; set; }

    [JsonPropertyName("disabled_by")]
    public string? DisabledBy { get; set; }

    [JsonPropertyName("entry_type")]
    public string? EntryType { get; set; }

    [JsonPropertyName("name_by_user")]
    public string? NameByUser { get; set; }

    [JsonPropertyName("device_class")]
    public string? DeviceClass { get; set; }
}

// TypeScript definition:
// export interface ConfigEntry {
//     entry_id: string;
//     domain: string;
//     title: string;
//     source: string;
//     state: string;
//     connection_class: string;
//     supports_options: boolean;
//     supports_unload: boolean;
//     disabled_by?: string;
// }

public class ConfigEntry
{
    [JsonPropertyName("entry_id")]
    public string EntryId { get; set; } = null!;

    [JsonPropertyName("domain")]
    public string Domain { get; set; } = null!;

    [JsonPropertyName("title")]
    public string Title { get; set; } = null!;

    [JsonPropertyName("source")]
    public string Source { get; set; } = null!;

    [JsonPropertyName("state")]
    public string State { get; set; } = null!;

    [JsonPropertyName("connection_class")]
    public string ConnectionClass { get; set; } = null!;

    [JsonPropertyName("supports_options")]
    public bool SupportsOptions { get; set; }

    [JsonPropertyName("supports_unload")]
    public bool SupportsUnload { get; set; }

    [JsonPropertyName("disabled_by")]
    public string? DisabledBy { get; set; }
}

// TypeScript definition:
// export interface EntityRegistryEntry {
//     entity_id: string;
//     name: string;
//     platform: string;
//     config_entry_id: string;
//     device_id?: string;
//     area_id?: string;
//     disabled_by?: string;
//     hidden_by?: string;
//     icon?: string;
//     unit_of_measurement?: string;
//     original_name?: string;
//     original_icon?: string;
// }

public class EntityRegistryEntry
{
    [JsonPropertyName("entity_id")]
    public string EntityId { get; set; } = null!;

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("platform")]
    public string Platform { get; set; } = null!;

    [JsonPropertyName("config_entry_id")]
    public string ConfigEntryId { get; set; } = null!;

    [JsonPropertyName("device_id")]
    public string? DeviceId { get; set; }

    [JsonPropertyName("area_id")]
    public string? AreaId { get; set; }

    [JsonPropertyName("disabled_by")]
    public string? DisabledBy { get; set; }

    [JsonPropertyName("hidden_by")]
    public string? HiddenBy { get; set; }

    [JsonPropertyName("icon")]
    public string? Icon { get; set; }

    [JsonPropertyName("unit_of_measurement")]
    public string? UnitOfMeasurement { get; set; }

    [JsonPropertyName("original_name")]
    public string? OriginalName { get; set; }

    [JsonPropertyName("original_icon")]
    public string? OriginalIcon { get; set; }
}

// TypeScript definition:
// export interface DeviceRegistryEntryMutableParams {
//     area_id?: string;
//     name_by_user?: string;
//     disabled_by?: string;
// }

public class DeviceRegistryEntryMutableParams
{
    [JsonPropertyName("area_id")]
    public string? AreaId { get; set; }

    [JsonPropertyName("name_by_user")]
    public string? NameByUser { get; set; }

    [JsonPropertyName("disabled_by")]
    public string? DisabledBy { get; set; }
}

// TypeScript definition:
// export interface EntityRegistryEntryMutableParams {
//     area_id?: string;
//     name?: string;
//     icon?: string;
//     disabled_by?: string;
//     hidden_by?: string;
//     unit_of_measurement?: string;
// }

public class EntityRegistryEntryMutableParams
{
    [JsonPropertyName("area_id")]
    public string? AreaId { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("icon")]
    public string? Icon { get; set; }

    [JsonPropertyName("disabled_by")]
    public string? DisabledBy { get; set; }

    [JsonPropertyName("hidden_by")]
    public string? HiddenBy { get; set; }

    [JsonPropertyName("unit_of_measurement")]
    public string? UnitOfMeasurement { get; set; }
}

// TypeScript definition:
// export interface ConfigEntryMutableParams {
//     disabled_by?: string;
// }

public class ConfigEntryMutableParams
{
    [JsonPropertyName("disabled_by")]
    public string? DisabledBy { get; set; }
}

// TypeScript definition:
// export interface DeviceRegistryEntryMutableParams {
//     area_id?: string;
//     name_by_user?: string;
//     disabled_by?: string;
// }

public class DeviceRegistryEntryMutableParams
{
    [JsonPropertyName("area_id")]
    public string? AreaId { get; set; }

    [JsonPropertyName("name_by_user")]
    public string? NameByUser { get; set; }

    [JsonPropertyName("disabled_by")]
    public string? DisabledBy { get; set; }
}

// TypeScript definition:
// export interface EntityRegistryEntryMutableParams {
//     area_id?: string;
//     name?: string;
//     icon?: string;
//     disabled_by?: string;
//     hidden_by?: string;
//     unit_of_measurement?: string;
// }

public class EntityRegistryEntryMutableParams
{
    [JsonPropertyName("area_id")]
    public string? AreaId { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("icon")]
    public string? Icon { get; set; }

    [JsonPropertyName("disabled_by")]
    public string? DisabledBy { get; set; }

    [JsonPropertyName("hidden_by")]
    public string? HiddenBy { get; set; }

    [JsonPropertyName("unit_of_measurement")]
    public string? UnitOfMeasurement { get; set; }
}

// TypeScript definition:
// export interface ConfigEntryMutableParams {
//     disabled_by?: string;
// }

public class ConfigEntryMutableParams
{
    [JsonPropertyName("disabled_by")]
    public string? DisabledBy { get; set; }
}

// TypeScript definition:
// export interface DeviceRegistryEntryMutableParams {
//     area_id?: string;
//     name_by_user?: string;
//     disabled_by?: string;
// }

public class DeviceRegistryEntryMutableParams
{
    [JsonPropertyName("area_id")]
    public string? AreaId { get; set; }

    [JsonPropertyName("name_by_user")]
    public string? NameByUser { get; set; }

    [JsonPropertyName("disabled_by")]
    public string? DisabledBy { get; set; }
}

// TypeScript definition:
// export interface EntityRegistryEntryMutableParams {
//     area_id?: string;
//     name?: string;
//     icon?: string;
//     disabled_by?: string;
//     hidden_by?: string;
//     unit_of_measurement?: string;
// }

public class EntityRegistryEntryMutableParams
{
    [JsonPropertyName("area_id")]
    public string? AreaId { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("icon")]
    public string? Icon { get; set; }

    [JsonPropertyName("disabled_by")]
    public string? DisabledBy { get; set; }

    [JsonPropertyName("hidden_by")]
    public string? HiddenBy { get; set; }

    [JsonPropertyName("unit_of_measurement")]
    public string? UnitOfMeasurement { get; set; }
}

// TypeScript definition:
// export interface ConfigEntryMutableParams {
//     disabled_by?: string;
// }

public class ConfigEntryMutableParams
{
    [JsonPropertyName("disabled_by")]
    public string? DisabledBy { get; set; }
}

// TypeScript definition:
// export interface DeviceRegistryEntryMutableParams {
//     area_id?: string;
//     name_by_user?: string;
//     disabled_by?: string;
// }

public class DeviceRegistryEntryMutableParams
{
    [JsonPropertyName("area_id")]
    public string? AreaId { get; set; }

    [JsonPropertyName("name_by_user")]
    public string? NameByUser { get; set; }

    [JsonPropertyName("disabled_by")]
    public string? DisabledBy { get; set; }
}

// TypeScript definition:
// export interface EntityRegistryEntryMutableParams {
//     area_id?: string;
//     name?: string;
//     icon?: string;
//     disabled_by?: string;
//     hidden_by?: string;
//     unit_of_measurement?: string;
// }

public class EntityRegistryEntryMutableParams
{
    [JsonPropertyName("area_id")]
    public string? AreaId { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("icon")]
    public string? Icon { get; set; }

    [JsonPropertyName("disabled_by")]
    public string? DisabledBy { get; set; }

    [JsonPropertyName("hidden_by")]
    public string? HiddenBy { get; set; }

    [JsonPropertyName("unit_of_measurement")]
    public string? UnitOfMeasurement { get; set; }
}

// TypeScript definition:
// export interface ConfigEntryMutableParams {
//     disabled_by?: string;
// }

public class ConfigEntryMutableParams
{
    [JsonPropertyName("disabled_by")]
    public string? DisabledBy { get; set; }
}

// TypeScript definition:
// export interface DeviceRegistryEntryMutableParams {
//     area_id?: string;
//     name_by_user?: string;
//     disabled_by?: string;
// }

public class DeviceRegistryEntryMutableParams
{
    [JsonPropertyName("area_id")]
    public string? AreaId { get; set; }

    [JsonPropertyName("name_by_user")]
    public string? NameByUser { get; set; }

    [JsonPropertyName("disabled_by")]
    public string? DisabledBy { get; set; }
}

// TypeScript definition:
// export interface EntityRegistryEntryMutableParams {
//     area_id?: string;
//     name?: string;
//     icon?: string;
//     disabled_by?: string;
//     hidden_by?: string;
//     unit_of_measurement?: string;
// }

public class EntityRegistryEntryMutableParams
{
    [JsonPropertyName("area_id")]
    public string? AreaId { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("icon")]
    public string? Icon { get; set; }

    [JsonPropertyName("disabled_by")]
    public string? DisabledBy { get; set; }

    [JsonPropertyName("hidden_by")]
    public string? HiddenBy { get; set; }

    [JsonPropertyName("unit_of_measurement")]
    public string? UnitOfMeasurement { get; set; }
}

// TypeScript definition:
// export interface ConfigEntryMutableParams {
//     disabled_by?: string;
// }

public class ConfigEntryMutableParams
{
    [JsonPropertyName("disabled_by")]
    public string? DisabledBy { get; set; }
}

// TypeScript definition:
// export interface DeviceRegistryEntryMutableParams {
//     area_id?: string;
//     name_by_user?: string;
//     disabled_by?: string;
// }

public class DeviceRegistryEntryMutableParams
{
    [JsonPropertyName("area_id")]
    public string? AreaId { get; set; }

    [JsonPropertyName("name_by_user")]
    public string? NameByUser { get; set; }

    [JsonPropertyName("disabled_by")]
    public string? DisabledBy { get; set; }
}

// TypeScript definition:
// export interface EntityRegistryEntryMutableParams {
//     area_id?: string;
//     name?: string;
//     icon?: string;
//     disabled_by?: string;
//     hidden_by?: string;
//     unit_of_measurement?: string;
// }

public class EntityRegistryEntryMutableParams
{
    [JsonPropertyName("area_id")]
    public string? AreaId { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("icon")]
    public string? Icon { get; set; }

    [JsonPropertyName("disabled_by")]
    public string? DisabledBy { get; set; }

    [JsonPropertyName("hidden_by")]
    public string? HiddenBy { get; set; }

    [JsonPropertyName("unit_of_measurement")]
    public string? UnitOfMeasurement { get; set; }
}

// TypeScript definition:
// export interface ConfigEntryMutableParams {
//     disabled_by?: string;
// }

public class ConfigEntryMutableParams
{
    [JsonPropertyName("disabled_by")]
    public string? DisabledBy { get; set; }
}

// TypeScript definition:
// export interface DeviceRegistryEntryMutableParams {
//     area_id?: string;
//     name_by_user?: string;
//     disabled_by?: string;
// }

public class DeviceRegistryEntryMutableParams
{
    [JsonPropertyName("area_id")]
    public string? AreaId { get; set; }

    [JsonPropertyName("name_by_user")]
    public string? NameByUser { get; set; }

    [JsonPropertyName("disabled_by")]
    public string? DisabledBy { get; set; }
}

// TypeScript definition:
// export interface EntityRegistryEntryMutableParams {
//     area_id?: string;
//     name?: string;
//     icon?: string;
//     disabled_by?: string;
//     hidden_by?: string;
//     unit_of_measurement?: string;
// }

public class EntityRegistryEntryMutableParams
{
    [JsonPropertyName("area_id")]
    public string? AreaId { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("icon")]
    public string? Icon { get; set; }

    [JsonPropertyName("disabled_by")]
    public string? DisabledBy { get; set; }

    [JsonPropertyName("hidden_by")]
    public string? HiddenBy { get; set; }

    [JsonPropertyName("unit_of_measurement")]
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Dunet;

namespace NetDaemon.Extensions.SourceGen.src.Model.Area
{
    // TypeScript definition:
    // export interface AreaRegistryEntry {
    //     area_id: string;
    //     name: string;
    //     normalized_name: string;
    //     picture?: string;
    // }

    public class AreaRegistryEntry
    {
        [JsonPropertyName("area_id")]
        public string AreaId { get; set; } = null!;

        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;

        [JsonPropertyName("normalized_name")]
        public string NormalizedName { get; set; } = null!;

        [JsonPropertyName("picture")]
        public string? Picture { get; set; }
    }

    // TypeScript definition:
    // export interface AreaRegistryEntryMutableParams {
    //     name: string;
    //     picture?: string;
    // }

    public class AreaRegistryEntryMutableParams
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;

        [JsonPropertyName("picture")]
        public string? Picture { get; set; }
    }

    // TypeScript definition:
    // export interface DeviceRegistryEntry {
    //     id: string;
    //     name: string;
    //     area_id: string;
    //     config_entries: string[];
    //     connections: [string, string][];
    //     identifiers: [string, string][];
    //     manufacturer?: string;
    //     model?: string;
    //     sw_version?: string;
    //     via_device_id?: string;
    //     disabled_by?: string;
    //     entry_type?: string;
    //     name_by_user?: string;
    //     device_class?: string;
    // }

    public class DeviceRegistryEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = null!;

        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;

        [JsonPropertyName("area_id")]
        public string AreaId { get; set; } = null!;

        [JsonPropertyName("config_entries")]
        public List<string> ConfigEntries { get; set; } = new();

        [JsonPropertyName("connections")]
        public List<(string, string)> Connections { get; set; } = new();

        [JsonPropertyName("identifiers")]
        public List<(string, string)> Identifiers { get; set; } = new();

        [JsonPropertyName("manufacturer")]
        public string? Manufacturer { get; set; }

        [JsonPropertyName("model")]
        public string? Model { get; set; }

        [JsonPropertyName("sw_version")]
        public string? SwVersion { get; set; }

        [JsonPropertyName("via_device_id")]
        public string? ViaDeviceId { get; set; }

        [JsonPropertyName("disabled_by")]
        public string? DisabledBy { get; set; }

        [JsonPropertyName("entry_type")]
        public string? EntryType { get; set; }

        [JsonPropertyName("name_by_user")]
        public string? NameByUser { get; set; }

        [JsonPropertyName("device_class")]
        public string? DeviceClass { get; set; }
    }

    // TypeScript definition:
    // export interface ConfigEntry {
    //     entry_id: string;
    //     domain: string;
    //     title: string;
    //     source: string;
    //     state: string;
    //     connection_class: string;
    //     supports_options: boolean;
    //     supports_unload: boolean;
    //     disabled_by?: string;
    // }

    public class ConfigEntry
    {
        [JsonPropertyName("entry_id")]
        public string EntryId { get; set; } = null!;

        [JsonPropertyName("domain")]
        public string Domain { get; set; } = null!;

        [JsonPropertyName("title")]
        public string Title { get; set; } = null!;

        [JsonPropertyName("source")]
        public string Source { get; set; } = null!;

        [JsonPropertyName("state")]
        public string State { get; set; } = null!;

        [JsonPropertyName("connection_class")]
        public string ConnectionClass { get; set; } = null!;

        [JsonPropertyName("supports_options")]
        public bool SupportsOptions { get; set; }

        [JsonPropertyName("supports_unload")]
        public bool SupportsUnload { get; set; }

        [JsonPropertyName("disabled_by")]
        public string? DisabledBy { get; set; }
    }

    // TypeScript definition:
    // export interface EntityRegistryEntry {
    //     entity_id: string;
    //     name: string;
    //     platform: string;
    //     config_entry_id: string;
    //     device_id?: string;
    //     area_id?: string;
    //     disabled_by?: string;
    //     hidden_by?: string;
    //     icon?: string;
    //     unit_of_measurement?: string;
    //     original_name?: string;
    //     original_icon?: string;
    // }

    public class EntityRegistryEntry
    {
        [JsonPropertyName("entity_id")]
        public string EntityId { get; set; } = null!;

        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;

        [JsonPropertyName("platform")]
        public string Platform { get; set; } = null!;

        [JsonPropertyName("config_entry_id")]
        public string ConfigEntryId { get; set; } = null!;

        [JsonPropertyName("device_id")]
        public string? DeviceId { get; set; }

        [JsonPropertyName("area_id")]
        public string? AreaId { get; set; }

        [JsonPropertyName("disabled_by")]
        public string? DisabledBy { get; set; }

        [JsonPropertyName("hidden_by")]
        public string? HiddenBy { get; set; }

        [JsonPropertyName("icon")]
        public string? Icon { get; set; }

        [JsonPropertyName("unit_of_measurement")]
        public string? UnitOfMeasurement { get; set; }

        [JsonPropertyName("original_name")]
        public string? OriginalName { get; set; }

        [JsonPropertyName("original_icon")]
        public string? OriginalIcon { get; set; }
    }

    // TypeScript definition:
    // export interface DeviceRegistryEntryMutableParams {
    //     area_id?: string;
    //     name_by_user?: string;
    //     disabled_by?: string;
    // }

    public class DeviceRegistryEntryMutableParams
    {
        [JsonPropertyName("area_id")]
        public string? AreaId { get; set; }

        [JsonPropertyName("name_by_user")]
        public string? NameByUser { get; set; }

        [JsonPropertyName("disabled_by")]
        public string? DisabledBy { get; set; }
    }

    // TypeScript definition:
    // export interface EntityRegistryEntryMutableParams {
    //     area_id?: string;
    //     name?: string;
    //     icon?: string;
    //     disabled_by?: string;
    //     hidden_by?: string;
    //     unit_of_measurement?: string;
    // }

    public class EntityRegistryEntryMutableParams
    {
        [JsonPropertyName("area_id")]
        public string? AreaId { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("icon")]
        public string? Icon { get; set; }

        [JsonPropertyName("disabled_by")]
        public string? DisabledBy { get; set; }

        [JsonPropertyName("hidden_by")]
        public string? HiddenBy { get; set; }

        [JsonPropertyName("unit_of_measurement")]
        public string? UnitOfMeasurement { get; set; }
    }

    // TypeScript definition:
    // export interface ConfigEntryMutableParams {
    //     disabled_by?: string;
    // }

    public class ConfigEntryMutableParams
    {
        [JsonPropertyName("disabled_by")]
        public string? DisabledBy { get; set; }
    }

    // TypeScript definition:
    // export interface DeviceRegistryEntryMutableParams {
    //     area_id?: string;
    //     name_by_user?: string;
    //     disabled_by?: string;
    // }

    public class DeviceRegistryEntryMutableParams
    {
        [JsonPropertyName("area_id")]
        public string? AreaId { get; set; }

        [JsonPropertyName("name_by_user")]
        public string? NameByUser { get; set; }

        [JsonPropertyName("disabled_by")]
        public string? DisabledBy { get; set; }
    }

    // TypeScript definition:
    // export interface EntityRegistryEntryMutableParams {
    //     area_id?: string;
    //     name?: string;
    //     icon?: string;
    //     disabled_by?: string;
    //     hidden_by?: string;
    //     unit_of_measurement?: string;
    // }

    public class EntityRegistryEntryMutableParams
    {
        [JsonPropertyName("area_id")]
        public string? AreaId { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("icon")]
        public string? Icon { get; set; }

        [JsonPropertyName("disabled_by")]
        public string? DisabledBy { get; set; }

        [JsonPropertyName("hidden_by")]
        public string? HiddenBy { get; set; }

        [JsonPropertyName("unit_of_measurement")]
        public string? UnitOfMeasurement { get; set; }
    }

    // TypeScript definition:
    // export interface ConfigEntryMutableParams {
    //     disabled_by?: string;
    // }

    public class ConfigEntryMutableParams
    {
        [JsonPropertyName("disabled_by")]
        public string? DisabledBy { get; set; }
    }

    // TypeScript definition:
    // export interface DeviceRegistryEntryMutableParams {
    //     area_id?: string;
    //     name_by_user?: string;
    //     disabled_by?: string;
    // }

    public class DeviceRegistryEntryMutableParams
    {
        [JsonPropertyName("area_id")]
        public string? AreaId { get; set; }

        [JsonPropertyName("name_by_user")]
        public string? NameByUser { get; set; }

        [JsonPropertyName("disabled_by")]
        public string? DisabledBy { get; set; }
    }

    // TypeScript definition:
    // export interface EntityRegistryEntryMutableParams {
    //     area_id?: string;
    //     name?: string;
    //     icon?: string;
    //     disabled_by?: string;
    //     hidden_by?: string;
    //     unit_of_measurement?: string;
    // }

    public class EntityRegistryEntryMutableParams
    {
        [JsonPropertyName("area_id")]
        public string? AreaId { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("icon")]
        public string? Icon { get; set; }

        [JsonPropertyName("disabled_by")]
        public string? DisabledBy { get; set; }

        [JsonPropertyName("hidden_by")]
        public string? HiddenBy { get; set; }

        [JsonPropertyName("unit_of_measurement")]
        public string? UnitOfMeasurement { get; set; }
    }

    // TypeScript definition:
    // export interface ConfigEntryMutableParams {
    //     disabled_by?: string;
    // }

    public class ConfigEntryMutableParams
    {
        [JsonPropertyName("disabled_by")]
        public string? DisabledBy { get; set; }
    }

    // TypeScript definition:
    // export interface DeviceRegistryEntryMutableParams {
    //     area_id?: string;
    //     name_by_user?: string;
    //     disabled_by?: string;
    // }

    public class DeviceRegistryEntryMutableParams
    {
        [JsonPropertyName("area_id")]
        public string? AreaId { get; set; }

        [JsonPropertyName("name_by_user")]
        public string? NameByUser { get; set; }

        [JsonPropertyName("disabled_by")]
        public string? DisabledBy { get; set; }
    }

    // TypeScript definition:
    // export interface EntityRegistryEntryMutableParams {
    //     area_id?: string;
    //     name?: string;
    //     icon?: string;
    //     disabled_by?: string;
    //     hidden_by?: string;
    //     unit_of_measurement?: string;
    // }

    public class EntityRegistryEntryMutableParams
    {
        [JsonPropertyName("area_id")]
        public string? AreaId { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("icon")]
        public string? Icon { get; set; }

        [JsonPropertyName("disabled_by")]
        public string? DisabledBy { get; set; }

        [JsonPropertyName("hidden_by")]
        public string? HiddenBy { get; set; }

        [JsonPropertyName("unit_of_measurement")]
        public string? UnitOfMeasurement { get; set; }
    }

    // TypeScript definition:
    // export interface ConfigEntryMutableParams {
    //     disabled_by?: string;
    // }

    public class ConfigEntryMutableParams
    {
        [JsonPropertyName("disabled_by")]
        public string? DisabledBy { get; set; }
    }

    // TypeScript definition:
    // export interface DeviceRegistryEntryMutableParams {
    //     area_id?: string;
    //     name_by_user?: string;
    //     disabled_by?: string;
    // }

    public class DeviceRegistryEntryMutableParams
    {
        [JsonPropertyName("area_id")]
        public string? AreaId { get; set; }

        [JsonPropertyName("name_by_user")]
        public string? NameByUser { get; set; }

        [JsonPropertyName("disabled_by")]
        public string? DisabledBy { get; set; }
    }

    // TypeScript definition:
    // export interface EntityRegistryEntryMutableParams {
    //     area_id?: string;
    //     name?: string;
    //     icon?: string;
    //     disabled_by?: string;
    //     hidden_by?: string;
    //     unit_of_measurement?: string;
    // }

    public class EntityRegistryEntryMutableParams
    {
        [JsonPropertyName("area_id")]
        public string? AreaId { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("icon")]
        public string? Icon { get; set; }

        [JsonPropertyName("disabled_by")]
        public string? DisabledBy { get; set; }

        [JsonPropertyName("hidden_by")]
        public string? HiddenBy { get; set; }

        [JsonPropertyName("unit_of_measurement")]
        public string? UnitOfMeasurement { get; set; }
    }

    // TypeScript definition:
    // export interface ConfigEntryMutableParams {
    //     disabled_by?: string;
    // }

    public class ConfigEntryMutableParams
    {
        [JsonPropertyName("disabled_by")]
        public string? DisabledBy { get; set; }
    }

    // TypeScript definition:
    // export interface DeviceRegistryEntryMutableParams {
    //     area_id?: string;
    //     name_by_user?: string;
    //     disabled_by?: string;
    // }

    public class DeviceRegistryEntryMutableParams
    {
        [JsonPropertyName("area_id")]
        public string? AreaId { get; set; }

        [JsonPropertyName("name_by_user")]
        public string? NameByUser { get; set; }

        [JsonPropertyName("disabled_by")]
        public string? DisabledBy { get; set; }
    }

    // TypeScript definition:
    // export interface EntityRegistryEntryMutableParams {
    //     area_id?: string;
    //     name?: string;
    //     icon?: string;
    //     disabled_by?: string;
    //     hidden_by?: string;
    //     unit_of_measurement?: string;
    // }

    public class EntityRegistryEntryMutableParams
    {
        [JsonPropertyName("area_id")]
        public string? AreaId { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("icon")]
        public string? Icon { get; set; }

        [JsonPropertyName("disabled_by")]
        public string? DisabledBy { get; set; }

        [JsonPropertyName("hidden_by")]
        public string? HiddenBy { get; set; }

        [JsonPropertyName("unit_of_measurement")]
        public string? UnitOfMeasurement { get; set; }
    }

    // TypeScript definition:
    // export interface ConfigEntryMutableParams {
    //     disabled_by?: string;
    // }

    public class ConfigEntryMutableParams
    {
        [JsonPropertyName("disabled_by")]
        public string? DisabledBy { get; set; }
    }

    // TypeScript definition:
    // export interface DeviceRegistryEntryMutableParams {
    //     area_id?: string;
    //     name_by_user?: string;
    //     disabled_by?: string;
    // }

    public class DeviceRegistryEntryMutableParams
    {
        [JsonPropertyName("area_id")]
        public string? AreaId { get; set; }

        [JsonPropertyName("name_by_user")]
        public string? NameByUser { get; set; }

        [JsonPropertyName("disabled_by")]
        public string? DisabledBy { get; set; }
    }
}