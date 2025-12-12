using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace Hammlet.Models.BackEnd
{
    public partial class DeviceInfo
    {
        [JsonPropertyName("manufacturer")] public string? Manufacturer { get; init; }

        [JsonPropertyName("model")]
        public string? Model { get; init; }

        [JsonPropertyName("id")] public string? Id { get; init; }

        [JsonPropertyName("area_id")] public string? AreaId { get; init; }
        [JsonPropertyName("name")] public string? Name { get; init; }

        [JsonPropertyName("name_by_user")] public string? NameByUser { get; init; }

        [JsonPropertyName("labels")] public IReadOnlyList<string> Labels { get; init; } = [];

#pragma warning disable CA1056 // It's ok for this URL to be a string
        [JsonPropertyName("configuration_url")] public string? ConfigurationUrl { get; init; }
#pragma warning restore CA1056
        [JsonPropertyName("hw_version")] public string? HardwareVersion { get; init; }
        [JsonPropertyName("sw_version")] public string? SoftwareVersion { get; init; }
        [JsonPropertyName("serial_number")] public string? SerialNumber { get; init; }
    }

    public partial class Entity
    {
        // Python: entity_id: str = None
        public string EntityId { get; set; }

        // Python: hass: HomeAssistant = None
        public JsonElement Hass { get; set; }

        // Python: platform: EntityPlatform = None
        public JsonElement Platform { get; set; }

        // Python: entity_description: EntityDescription
        public EntityDescription EntityDescription { get; set; }

        // Python: _attr_state: StateType = STATE_UNKNOWN
        public virtual StateType State { get; set; } = StateType.STATE_UNKNOWN;

        // Python: @property
        // def state(self) -> StateType:
        //     """Return the state of the entity."""
        //     return self._attr_state
//        public virtual StateType State => State;

        // Python: @property
        // def name(self) -> str | None:
        //     """Return the name of the entity."""
        //     return self._attr_name
        public virtual string Name => EntityDescription.Name;

        // Python: @property
        // def unique_id(self) -> str | None:
        //     """Return a unique ID."""
        //     return self._attr_unique_id
        public virtual string UniqueId => EntityDescription.UniqueId;

        // Python: @property
        // def available(self) -> bool:
        //     """Return True if entity is available."""
        //     return self._attr_available
        public virtual bool Available => EntityDescription.Available;

        // Python: @property
        // def should_poll(self) -> bool:
        //     """Return True if entity has to be polled for state."""
        //     return self._attr_should_poll
        public virtual bool ShouldPoll => EntityDescription.ShouldPoll;

        // Python: @property
        // def capability_attributes(self) -> dict[str, Any] | None:
        //     """Return the capability attributes."""
        //     return self._attr_capability_attributes
        public virtual Dictionary<string, object> CapabilityAttributes => EntityDescription.CapabilityAttributes;

        // Python: @property
        // def state_attributes(self) -> dict[str, Any] | None:
        //     """Return the state attributes."""
        //     return self._attr_state_attributes
        public virtual Dictionary<string, object> StateAttributes => EntityDescription.StateAttributes;

        // Python: @property
        // def extra_state_attributes(self) -> dict[str, Any] | None:
        //     """Return entity specific state attributes."""
        //     return self._attr_extra_state_attributes
        public virtual Dictionary<string, object> ExtraStateAttributes => EntityDescription.ExtraStateAttributes;

        // Python: @property
        // def device_info(self) -> DeviceInfo | None:
        //     """Return device specific attributes."""
        //     return self._attr_device_info
        public virtual DeviceInfo DeviceInfo => EntityDescription.DeviceInfo;

        // Python: @property
        // def device_class(self) -> str | None:
        //     """Return the class of this device, from component DEVICE_CLASSES."""
        //     return self._attr_device_class
        public virtual string DeviceClass => EntityDescription.DeviceClass;

        // Python: @property
        // def unit_of_measurement(self) -> str | None:
        //     """Return the unit of measurement of this entity, if any."""
        //     return self._attr_unit_of_measurement
        public virtual string UnitOfMeasurement => EntityDescription.UnitOfMeasurement;

        // Python: @property
        // def icon(self) -> str | None:
        //     """Return the icon to use in the frontend, if any."""
        //     return self._attr_icon
        public virtual string Icon => EntityDescription.Icon;

        // Python: @property
        // def entity_picture(self) -> str | None:
        //     """Return the entity picture to use in the frontend, if any."""
        //     return self._attr_entity_picture
        public virtual string EntityPicture => EntityDescription.EntityPicture;

        // Python: @property
        // def assumed_state(self) -> bool:
        //     """Return True if unable to access real state of the entity."""
        //     return self._attr_assumed_state
        public virtual bool AssumedState => EntityDescription.AssumedState;

        // Python: @property
        // def force_update(self) -> bool:
        //     """Return True if state updates should be forced."""
        //     return self._attr_force_update
        public virtual bool ForceUpdate => EntityDescription.ForceUpdate;

        // Python: @property
        // def supported_features(self) -> int | None:
        //     """Flag supported features."""
        //     return self._attr_supported_features
        public virtual int? SupportedFeatures => EntityDescription.SupportedFeatures;

        // Python: @property
        // def entity_registry_enabled_default(self) -> bool:
        //     """Return if the entity should be enabled when first added."""
        //     return self._attr_entity_registry_enabled_default
        public virtual bool EntityRegistryEnabledDefault => EntityDescription.EntityRegistryEnabledDefault;

        // Python: @property
        // def entity_registry_visible_default(self) -> bool:
        //     """Return if the entity should be visible when first added."""
        //     return self._attr_entity_registry_visible_default
        public virtual bool EntityRegistryVisibleDefault => EntityDescription.EntityRegistryVisibleDefault;

        // Python: @property
        // def attribution(self) -> str | None:
        //     """Return the attribution."""
        //     return self._attr_attribution
        public virtual string Attribution => EntityDescription.Attribution;

        // Python: @property
        // def entity_category(self) -> EntityCategory | None:
        //     """Return the category of the entity, if any."""
        //     return self._attr_entity_category
        public virtual string EntityCategory => EntityDescription.EntityCategory;

        // Python: @property
        // def translation_key(self) -> str | None:
        //     """Return the translation key to translate the entity's states."""
        //     return self._attr_translation_key
        public virtual string TranslationKey => EntityDescription.TranslationKey;

        // Python: async def async_update_ha_state(self, force_refresh: bool = False) -> None:
        //     """Update Home Assistant with current state of entity."""
        //     if force_refresh:
        //         await self.async_device_update()
        //     self._async_write_ha_state()
        public virtual async Task AsyncUpdateHaState(bool forceRefresh = false)
        {
            if (forceRefresh)
            {
                await AsyncDeviceUpdate();
            }
            AsyncWriteHaState();
        }

        // Python: async def async_device_update(self, warning: bool = True) -> None:
        //     """Process 'update' or 'async_update' from entity."""
        //     if hasattr(self, "async_update"):
        //         await self.async_update()
        //     elif hasattr(self, "update"):
        //         await self.hass.async_add_executor_job(self.update)
        public virtual async Task AsyncDeviceUpdate(bool warning = true)
        {
            throw new NotImplementedException();
            //if (this is IAsyncUpdate asyncUpdate)
            //{
            //    await asyncUpdate.AsyncUpdate();
            //}
            //else if (this is IUpdate update)
            //{
            //    await Hass.AsyncAddExecutorJob(update.Update);
            //}
        }

        // Python: @callback
        // def async_write_ha_state(self) -> None:
        //     """Write the state to the state machine."""
        //     self.hass.states.async_set(self.entity_id, self.state, self.state_attributes)
        public virtual void AsyncWriteHaState()
        {
            throw new NotImplementedException();
//            Hass.States.AsyncSet(EntityId, State, GetStateAttributes());
        }
    }
}
