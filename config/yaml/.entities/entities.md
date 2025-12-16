# Home Assistant Entities (Curated)

Generated from `http://192.168.1.216:8123/api/states`

> This document is intentionally **not exhaustive**.
> It aggressively trims **unavailable**, noisy, and obvious duplicate entities to keep the list useful for day-to-day automation/script work.
> If something seems missing or incorrect, regenerate from the API and adjust the filters as needed.

## Summary

- Total entities from API: 1200
- Entities included in this curated doc: 168
- Dropped counts (approx):
  - domain_not_kept: 411
  - duplicate_old: 6
  - noise: 397
  - sensor_not_interesting: 74
  - unavailable_or_unknown: 144

## Table of Contents

- [light (30)](#user-content-light)
- [switch (23)](#user-content-switch)
- [automation (31)](#user-content-automation)
- [script (9)](#user-content-script)
- [scene (6)](#user-content-scene)
- [input_boolean (11)](#user-content-input_boolean)
- [input_datetime (4)](#user-content-input_datetime)
- [input_select (2)](#user-content-input_select)
- [input_number (4)](#user-content-input_number)
- [media_player (7)](#user-content-media_player)
- [weather (1)](#user-content-weather)
- [person (4)](#user-content-person)
- [device_tracker (5)](#user-content-device_tracker)
- [timer (2)](#user-content-timer)
- [sensor (28)](#user-content-sensor)
- [binary_sensor (1)](#user-content-binary_sensor)

---

## light

30 entities

| Entity ID | Name | State | Brightness |
|-----------|------|-------|------------|
| `light.alive_fan` | Alive Fan Light | off | None |
| `light.alive_lights` | Alive Lights | off | None |
| `light.asia_big_deck` | Asia Big Deck | off | None |
| `light.asia_s_big_balls_dnd` | Side Balls Dnd | on | - |
| `light.bathroom_ceiling` | Bathroom Ceiling  | off | None |
| `light.big_balls` | Side Balls | off | None |
| `light.big_deck_bistro` | 🍇 on the Vine | off | None |
| `light.butts_bulb_2` | Butts Bulb 2 | off | None |
| `light.butts_bulb_3` | Butts Bulb 3 | off | None |
| `light.butts_bulb_4` | Butts Bulb 4 | off | None |
| `light.color_bulbs` | Color | off | None |
| `light.floodlights` | Floodlights | off | None |
| `light.hammbone_fan_light` | Hammbone Fan | off | None |
| `light.hammscape` | Hammscape | off | None |
| `light.hammscape_bistro` | Bistro | off | None |
| `light.hey_nabu_led_ring` | Hey Nabu LED Ring | off | None |
| `light.liminal_dimmer` | Liminal Dimmer | on | 127 |
| `light.living` | Alive | off | None |
| `light.living_fan` | Alive Fan Light | off | None |
| `light.outdoor_sconce` | Outdoor Sconce | off | None |
| `light.plant_lamp` | Plant Lamp | off | None |
| `light.plant_lamp_2` | Plant Lamp Plant Lamp | off | None |
| `light.reference_light` | Reference Light 4 Reference Light 4 1 | on | None |
| `light.reference_light_2` | Reference Light | on | None |
| `light.scarlet_moon` | Scarlet Moon | off | None |
| `light.spamheart` | Spamheart | off | None |
| `light.spamheart_1` | Spamheart Spamheart | off | None |
| `light.succulent_lamp` | Succulent Lamp | off | None |
| `light.tentacle_lamp` | Tentacle Lamp | off | None |
| `light.tentacle_lightener` | tentacle_lightener | off | None |

## switch

23 entities

| Entity ID | Name | State |
|-----------|------|-------|
| `switch.adapt_alive_lights` | Adaptive Lighting: Alive Lights | off |
| `switch.adapt_brightness_bathroom` | Adaptive Lighting Adapt Brightness: Bathroom | on |
| `switch.adapt_color_bathroom` | Adaptive Lighting Adapt Color: Bathroom | on |
| `switch.adapt_kitchen` | Adaptive Lighting: Kitchen and Pantry | on |
| `switch.adapt_lighting_bathroom` | Adaptive Lighting: Bathroom | on |
| `switch.adapt_liminal` | Adaptive Lighting: Liminal | on |
| `switch.adaptive_lighting_adapt_brightness_alive_lights` | Adaptive Lighting Adapt Brightness: Alive Lights | off |
| `switch.adaptive_lighting_adapt_brightness_kitchen` | Adaptive Lighting Adapt Brightness: Kitchen and Pantry | on |
| `switch.adaptive_lighting_adapt_brightness_liminal` | Adaptive Lighting Adapt Brightness: Liminal | on |
| `switch.adaptive_lighting_adapt_color_alive_lights` | Adaptive Lighting Adapt Color: Alive Lights | off |
| `switch.adaptive_lighting_adapt_color_kitchen` | Adaptive Lighting Adapt Color: Kitchen and Pantry | on |
| `switch.adaptive_lighting_adapt_color_liminal` | Adaptive Lighting Adapt Color: Liminal | on |
| `switch.adaptive_lighting_sleep_mode_alive_lights` | Adaptive Lighting Sleep Mode: Alive Lights | off |
| `switch.adaptive_lighting_sleep_mode_bathroom` | Adaptive Lighting Sleep Mode: Bathroom | off |
| `switch.adaptive_lighting_sleep_mode_kitchen` | Adaptive Lighting Sleep Mode: Kitchen and Pantry | off |
| `switch.adaptive_lighting_sleep_mode_liminal` | Adaptive Lighting Sleep Mode: Liminal | off |
| `switch.hey_nabu_mute` | Hey Nabu Mute | off |
| `switch.hey_nabu_wake_sound` | Hey Nabu Wake sound | on |
| `switch.tentacle_lamp` | Tentacle Lamp | off |
| `switch.tentacle_lamp_auto_update_enabled` | Tentacle Lamp Auto-update enabled | on |
| `switch.tentacle_lamp_led` | Tentacle Lamp LED | on |
| `switch.tentacle_led` | Succulent Lamp LED | on |
| `switch.zigbee2mqtt_bridge_permit_join` | Zigbee2MQTT Bridge Permit join | off |

## automation

31 entities

| Entity ID | Name | State |
|-----------|------|-------|
| `automation.alive_dimmer` | Alive dimmer | on |
| `automation.bedroom_dimmer` | Bedroom: Dimmer | on |
| `automation.change_reference_to_warm_white` | Change Reference to Warm white | on |
| `automation.changesoundmode` | ChangeSoundMode | on |
| `automation.double_tap_dimmer` | Double Tap Dimmer | on |
| `automation.lighting_mode_bright` | lighting mode: Bright | on |
| `automation.lighting_mode_evenink` | lighting mode: Evenink | on |
| `automation.lighting_mode_night` | lighting mode: Night | on |
| `automation.lighting_mode_off` | lighting mode: Off | on |
| `automation.lighting_turn_off_at_2am` | Transition: Bedtime (2am) | on |
| `automation.music_assistant_local_only_voice_support_blueprint` | Music Assistant - Local(only) Voice Support Blueprint | on |
| `automation.new_automation` | New automation | on |
| `automation.party_lights` | Party Lights | on |
| `automation.press_my_knob_find_my_phone` | Press My Knob, Find my phone | on |
| `automation.reminders_create_and_list` | Reminders - Create and List | on |
| `automation.sensor_light_test` | Sensor Light Test | on |
| `automation.set_default_theme_on_start` | Set default theme on start | on |
| `automation.shellies_announce` | Shellies Announce | on |
| `automation.shellies_discovery` | Shellies Discovery | on |
| `automation.shelly_dimming` | Shelly Dimming | on |
| `automation.smart_light_test` | Smart Light Test | on |
| `automation.toggle_liminal_dimmer_warmth` | Toggle Liminal Dimmer Warmth | on |
| `automation.transition_late_11pm` | Transition: Late (11pm) | on |
| `automation.turn_off_pump_if_on_too_long` | Turn off pump if on too long | on |
| `automation.turn_off_water_pump_after_10_mins` | Turn off water pump after 10 mins | on |
| `automation.turn_on_evenink_near_sunset` | Transition: Evenink | on |
| `automation.turn_on_living_room_tv_with_wakeonlan_2` | Turn On Living Room TV with WakeOnLan | on |
| `automation.turn_on_speakers_when_things_come_online` | Turn On speakers when things come online | on |
| `automation.water_fill_timer_finished_turn_off_pump` | Water Fill Timer Finished - Turn Off Pump | on |
| `automation.water_pump_control` | Water Pump Control | on |
| `automation.water_pump_off_pause_timer` | Water Pump Off - Pause Timer | on |

## script

9 entities

| Entity ID | Name | State |
|-----------|------|-------|
| `script.ashley_s_light_fader` | Ashley’s Light Fader | off |
| `script.dimmer_brightness_control` | dimmer_brightness_control | off |
| `script.dimmer_rotate_control` | dimmer_rotate_control | off |
| `script.fade_until_time` | Fade until time | off |
| `script.get_target_lights` | Get Target Lights | off |
| `script.reset_lighting` | Reset Lighting | off |
| `script.target_lights_fader` | Target Lights Fader | off |
| `script.toggle_input` | toggle_input | off |
| `script.toggle_mute` | Toggle Mute | off |

## scene

6 entities

| Entity ID | Name | State |
|-----------|------|-------|
| `scene.all_off` | All Off | 2025-12-16T10:00:00.383577+00:00 |
| `scene.bright` | Bright | 2025-02-06T23:42:48.488453+00:00 |
| `scene.dinner_time` | Dinner Time | 2025-01-07T04:50:49.228447+00:00 |
| `scene.evenink` | Evenink | 2025-12-16T00:14:05.415630+00:00 |
| `scene.hangout` | Hangout | 2025-09-26T00:59:18.936548+00:00 |
| `scene.night` | Late | 2025-12-16T09:19:05.627652+00:00 |

## input_boolean

11 entities

| Entity ID | Name | State |
|-----------|------|-------|
| `input_boolean.controller_flash` | controller_flash | on |
| `input_boolean.disable_turn_off_late` | Disable - Turn-Off: Late | off |
| `input_boolean.dynamic_color_temp` | Dynamic Color Temp | off |
| `input_boolean.finger_loop` | Repeat Fingerblaster | off |
| `input_boolean.iceblast_loop` | Iceblast Loop | off |
| `input_boolean.manual_lighting` | Manual Lighting | off |
| `input_boolean.netdaemon_hammlet_apps_assistant_monitor_available_assistants` | netdaemon_hammlet_apps_assistant_monitor_available_assistants | on |
| `input_boolean.netdaemon_hammlet_apps_lights_dimmer_sync` | netdaemon_hammlet_apps_lights_dimmer_sync | on |
| `input_boolean.netdaemon_hammlet_apps_lights_watch_reference_light` | netdaemon_hammlet_apps_lights_watch_reference_light | on |
| `input_boolean.netdaemon_hammlet_apps_scene_on_button_scene_on_button` | netdaemon_hammlet_apps_scene_on_button_scene_on_button | off |
| `input_boolean.party_mode` | party-mode | off |

## input_datetime

4 entities

| Entity ID | Name | State |
|-----------|------|-------|
| `input_datetime.late_time` | Late Time | 2025-08-10 23:00:00 |
| `input_datetime.sunset_lights_time` | Sunset Lights Time | 2025-08-10 00:00:00 |
| `input_datetime.water_fill_time` | Water Fill Time | 00:10:00 |
| `input_datetime.water_on_time` | Water On Time | 00:01:00 |

## input_select

2 entities

| Entity ID | Name | State |
|-----------|------|-------|
| `input_select.lighting_mode` | lighting_mode | Off |
| `input_select.sound_mode` | Sound Mode | Clear |

## input_number

4 entities

| Entity ID | Name | State |
|-----------|------|-------|
| `input_number.alive_lights_last_temperature` | alive lights last temperature | 2000.0 |
| `input_number.stripper_light_brightness` | Stripper Light Brightness | 66.0 |
| `input_number.warm_white_kelvin` | Warm White Kelvin | 2200.0 |
| `input_number.water_pump_duty_cycle` | Pump Duty Cycle | 40.0 |

## media_player

7 entities

- `media_player.alive_cast` - Alive Cast (idle)
- `media_player.alive_room_tv` - Alive Room TV (off)
- `media_player.alive_tv` - Alive TV (off)
- `media_player.hammlet_oled_tv` - TV (off)
- `media_player.hey_nabu` - Hey Nabu Media Player (idle)
- `media_player.spotify_scarlet_checkers` - Spotify Scarlet Checkers (idle)
- `media_player.ytube_music_player` - ytube_music_player (off)

## weather

1 entities

- `weather.forecast_the_hammlet` - Forecast The Hammlet (fog)

## person

4 entities

- `person.dolph` - Dolph (home)
- `person.jumper` - Jumper (not_home)
- `person.snick` - Snick (not_home)
- `person.stuart` - Stuart (not_home)

## device_tracker

5 entities

- `device_tracker.briana_s_iphone` - Briana's iPhone (not_home)
- `device_tracker.google_pixel_8` - Google Pixel 8 (home)
- `device_tracker.moto_x4` - moto x4 (not_home)
- `device_tracker.rudys_iphone` - Rudy’s iPhone (home)
- `device_tracker.stuarts_iphone` - Stuart’s iPhone (not_home)

## timer

2 entities

| Entity ID | Name | State |
|-----------|------|-------|
| `timer.water_fill_timer` | Water Fill Timer | idle |
| `timer.water_on_timer` | Water On Timer | idle |

## sensor

28 entities

| Entity ID | Name | State | Unit |
|-----------|------|-------|------|
| `sensor.briana_s_iphone_battery_level` | Briana's iPhone Battery Level | 80 | % |
| `sensor.briana_s_iphone_battery_state` | Briana's iPhone Battery State | Charging | - |
| `sensor.laser_monstera_illuminance` | Laser Monstera illuminance | 107.0 | lx |
| `sensor.laser_monstera_temperature` | Laser Monsterra Temperature | 72.14 | °F |
| `sensor.laser_monsterra_illuminance` | Laser Monsterra Illuminance | 107 | lx |
| `sensor.liminal_dimmer_energy_0` | Liminal Dimmer Energy 0 | 0.0 | Wh |
| `sensor.liminal_dimmer_power_0` | Liminal Dimmer Power 0 | 0.00 | W |
| `sensor.liminal_dimmer_temperature` | Liminal Dimmer Temperature | 98.51 | °F |
| `sensor.mikaela_battery_level` | Mikaela Battery level | 100 | % |
| `sensor.mikaela_battery_state` | Mikaela Battery state | full | - |
| `sensor.moto_x4_battery_level` | moto x4 Battery level | 20 | % |
| `sensor.moto_x4_battery_state` | moto x4 Battery state | discharging | - |
| `sensor.outdoor_shade_succulent_battery` | Outdoor Shade Succulent Battery | 25 | % |
| `sensor.outdoor_shade_succulent_battery_state` | Outdoor Shade Succulent Battery state | medium | - |
| `sensor.outdoor_shade_succulent_temperature` | Outdoor Shade Succulent Temperature | 56.12 | °F |
| `sensor.outdoor_shade_succulent_temperature_2` | Outdoor Shade Succulent temperature | 56.12 | °F |
| `sensor.pixel_8_battery_level` | Pixel 8 Battery level | 31 | % |
| `sensor.pixel_8_battery_state` | Pixel 8 Battery state | discharging | - |
| `sensor.rudys_iphone_battery_level` | Rudy’s iPhone Battery Level | 80 | % |
| `sensor.rudys_iphone_battery_state` | Rudy’s iPhone Battery State | Charging | - |
| `sensor.soil_stick_1_battery` | Soil Stick 1 Battery | 73 | % |
| `sensor.soil_stick_1_humidity` | Soil Stick 1 Humidity | 40.5 | % |
| `sensor.soil_stick_1_temperature` | Soil Stick 1 Temperature | 68.9 | °F |
| `sensor.stuarts_iphone_battery_level` | Stuart’s iPhone Battery Level | 91 | % |
| `sensor.stuarts_iphone_battery_state` | Stuart’s iPhone Battery State | Not Charging | - |
| `sensor.wallfire_battery_level` | Wallfire Battery level | 62 | % |
| `sensor.wallfire_battery_state` | Wallfire Battery state | discharging | - |
| `sensor.water_pump_usage` | Water Pump Usage | 0.0 | % |

## binary_sensor

1 entities

| Entity ID | Name | State | Unit |
|-----------|------|-------|------|
| `binary_sensor.liminal_dimmer_overtemperature` | Liminal Dimmer Overtemperature | off | - |

