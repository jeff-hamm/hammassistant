using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using NetDaemon.HassModel.Entities;

namespace Hammlet.Models.BackEnd
{
    public partial class ToggleEntityDescription : EntityDescription
    {

    }
    public enum StateType
    {
        STATE_ON,
        STATE_OFF,
        STATE_UNKNOWN
    }
    public partial class ToggleEntity : Entity
    {
        // Python: entity_description: ToggleEntityDescription
        public ToggleEntityDescription EntityDescription { get; set; }

        // Python: _attr_state: None = None
//        public new StateType? State { get; set; }

        // Python: @property
        // @final
        // def state(self) -> Literal["on", "off"] | None:
        //     """Return the state."""
        //     if (is_on := self.is_on) is None:
        //         return None
        //     return STATE_ON if is_on else STATE_OFF
        public override StateType? State => IsOn.HasValue ? (IsOn.Value ? StateType.STATE_ON : StateType.STATE_OFF) : (StateType?)null;

        // Python: @cached_property
        // def is_on(self) -> bool | None:
        //     """Return True if entity is on."""
        //     return self._attr_is_on
        public virtual bool? IsOn => IsOn;

        // Python: def turn_on(self, **kwargs: Any) -> None:
        //     """Turn the entity on."""
        //     raise NotImplementedError
        public virtual void TurnOn(Dictionary<string, object> kwargs)
        {
            throw new NotImplementedException();
        }

        // Python: async def async_turn_on(self, **kwargs: Any) -> None:
        //     """Turn the entity on."""
        //     await self.hass.async_add_executor_job(ft.partial(self.turn_on, **kwargs))
        public virtual async Task AsyncTurnOn(Dictionary<string, object> kwargs)
        {
            throw new NotImplementedException();
            //            await Hass.AsyncAddExecutorJob(() => TurnOn(kwargs));
        }

        // Python: def turn_off(self, **kwargs: Any) -> None:
        //     """Turn the entity off."""
        //     raise NotImplementedError
        public virtual void TurnOff(Dictionary<string, object> kwargs)
        {
            throw new NotImplementedException();
        }

        // Python: async def async_turn_off(self, **kwargs: Any) -> None:
        //     """Turn the entity off."""
        //     await self.hass.async_add_executor_job(ft.partial(self.turn_off, **kwargs))
        public virtual async Task AsyncTurnOff(Dictionary<string, object> kwargs)
        {
            throw new NotImplementedException();
//            await Hass.AsyncAddExecutorJob(() => TurnOff(kwargs));
        }

        // Python: @final
        // def toggle(self, **kwargs: Any) -> None:
        //     """Toggle the entity.
        //     This method will never be called by Home Assistant and should not be implemented
        //     by integrations.
        //     """
        public void Toggle(Dictionary<string, object> kwargs)
        {
            // This method will never be called by Home Assistant and should not be implemented by integrations.
        }

        // Python: async def async_toggle(self, **kwargs: Any) -> None:
        //     """Toggle the entity.
        //     This method should typically not be implemented by integrations, it's enough to
        //     implement async_turn_on + async_turn_off or turn_on + turn_off.
        //     """
        public virtual async Task AsyncToggle(Dictionary<string, object> kwargs)
        {
            if (IsOn == true)
            {
                await AsyncTurnOff(kwargs);
            }
            else
            {
                await AsyncTurnOn(kwargs);
            }
        }
    }
}
