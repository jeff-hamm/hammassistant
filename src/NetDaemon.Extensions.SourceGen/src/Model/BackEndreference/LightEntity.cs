using System;
using System.Collections.Generic;

namespace Hammlet.Models.BackEnd
{
    public partial class LightEntity : ToggleEntity
    {
        // Attributes
        // #src: https://raw.githubusercontent.com/home-assistant/core/refs/heads/dev/homeassistant/components/light/__init__.py
        // _entity_component_unrecorded_attributes = frozenset(
        //     {
        //         ATTR_SUPPORTED_COLOR_MODES,
        //         ATTR_EFFECT_LIST,
        //         _DEPRECATED_ATTR_MIN_MIREDS.value,
        //         _DEPRECATED_ATTR_MAX_MIREDS.value,
        //         ATTR_MIN_COLOR_TEMP_KELVIN,
        //         ATTR_MAX_COLOR_TEMP_KELVIN,
        //         ATTR_BRIGHTNESS,
        //         ATTR_COLOR_MODE,
        //         _DEPRECATED_ATTR_COLOR_TEMP.value,
        //         ATTR_COLOR_TEMP_KELVIN,
        //         ATTR_EFFECT,
        //         ATTR_HS_COLOR,
        //         ATTR_RGB_COLOR,
        //         ATTR_RGBW_COLOR,
        //         ATTR_RGBWW_COLOR,
        //         ATTR_XY_COLOR,
        //     }
        // )
        private static readonly HashSet<LightAttributes> _entityComponentUnrecordedAttributes = new HashSet<LightAttributes>
        {
            LightAttributes.SupportedColorModes,
            LightAttributes.EffectList,
            LightAttributes.MinMireds,
            LightAttributes.MaxMireds,
            LightAttributes.MinColorTempKelvin,
            LightAttributes.MaxColorTempKelvin,
            LightAttributes.Brightness,
            LightAttributes.ColorMode,
            LightAttributes.ColorTemp,
            LightAttributes.ColorTempKelvin,
            LightAttributes.Effect,
            LightAttributes.HsColor,
            LightAttributes.RgbColor,
            LightAttributes.RgbwColor,
            LightAttributes.RgbwwColor,
            LightAttributes.XyColor,
        };

        // #src: https://raw.githubusercontent.com/home-assistant/core/refs/heads/dev/homeassistant/components/light/__init__.py
        // entity_description: LightEntityDescription
        // _attr_brightness: int | None = None
        // _attr_color_mode: ColorMode | str | None = None
        // _attr_color_temp_kelvin: int | None = None
        // _attr_effect_list: list[str] | None = None
        // _attr_effect: str | None = None
        // _attr_hs_color: tuple[float, float] | None = None
        // _attr_max_color_temp_kelvin: int | None = None
        // _attr_min_color_temp_kelvin: int | None = None
        // _attr_rgb_color: tuple[int, int, int] | None = None
        // _attr_rgbw_color: tuple[int, int, int, int] | None = None
        // _attr_rgbww_color: tuple[int, int, int, int, int] | None = None
        // _attr_supported_color_modes: set[ColorMode] | set[str] | None = None
        // _attr_supported_features: LightEntityFeature = LightEntityFeature(0)
        // _attr_xy_color: tuple[float, float] | None = None
        public EntityDescription EntityDescription { get; set; }
        public int? Brightness { get; set; }
        public string? ColorMode { get; set; }
        public int? ColorTempKelvin { get; set; }
        public List<string>? EffectList { get; set; }
        public string? Effect { get; set; }
        public Tuple<float, float>? HsColor { get; set; }
        public int? MaxColorTempKelvin { get; set; }
        public int? MinColorTempKelvin { get; set; }
        public Tuple<int, int, int>? RgbColor { get; set; }
        public Tuple<int, int, int, int>? RgbwColor { get; set; }
        public Tuple<int, int, int, int, int>? RgbwwColor { get; set; }
        public HashSet<string>? SupportedColorModes { get; set; }
        public LightEntityFeature SupportedFeatures { get; set; } = new LightEntityFeature(0);
        public Tuple<float, float>? XyColor { get; set; }

        // Deprecated attributes
        // #src: https://raw.githubusercontent.com/home-assistant/core/refs/heads/dev/homeassistant/components/light/__init__.py
        // _attr_color_temp: Final[int | None] = None
        // _attr_max_mireds: Final[int] = 500  # = 2000 K
        // _attr_min_mireds: Final[int] = 153  # = 6535.94 K (~ 6500 K)
        public readonly int? ColorTemp = null;
        public readonly int MaxMireds = 500; // = 2000 K
        public readonly int MinMireds = 153; // = 6535.94 K (~ 6500 K)

        private bool _colorModeReported = false;

        // Methods
        // #src: https://raw.githubusercontent.com/home-assistant/core/refs/heads/dev/homeassistant/components/light/__init__.py
        // @property
        // def _light_internal_color_mode(self) -> str:
        //     """Return the color mode of the light with backwards compatibility."""
        //     if (color_mode := self.color_mode) is None:
        //         # Backwards compatibility for color_mode added in 2021.4
        //         # Warning added in 2024.3, break in 2025.3
        //         if not self.__color_mode_reported and self.__should_report_light_issue():
        //             self.__color_mode_reported = True
        //             report_issue = self._suggest_report_issue()
        //             _LOGGER.warning(
        //                 (
        //                     "%s (%s) does not report a color mode, this will stop working "
        //                     "in Home Assistant Core 2025.3, please %s"
        //                 ),
        //                 self.entity_id,
        //                 type(self),
        //                 report_issue,
        //             )

        //         supported = self._light_internal_supported_color_modes

        //         if ColorMode.HS in supported and self.hs_color is not None:
        //             return ColorMode.HS
        //         if ColorMode.COLOR_TEMP in supported and self.color_temp_kelvin is not None:
        //             return ColorMode.COLOR_TEMP
        //         if ColorMode.BRIGHTNESS in supported and self.brightness is not None:
        //             return ColorMode.BRIGHTNESS
        //         if ColorMode.ONOFF in supported:
        //             return ColorMode.ONOFF
        //         return ColorMode.UNKNOWN

        //     return color_mode
        public string GetInternalColorMode()
        {
            if (ColorMode == null)
            {
                if (!_colorModeReported)
                {
                    _colorModeReported = true;
                    // Log warning
                }
                var supported = GetInternalSupportedColorModes();

                if (supported.Contains("hs") && HsColor != null) return "hs";
                if (supported.Contains("color_temp") && ColorTempKelvin != null) return "color_temp";
                if (supported.Contains("brightness") && Brightness != null) return "brightness";
                if (supported.Contains("onoff")) return "onoff";
                return "unknown";
            }
            return ColorMode;
        }

        // #src: https://raw.githubusercontent.com/home-assistant/core/refs/heads/dev/homeassistant/components/light/__init__.py
        // @property
        // def color_temp_kelvin(self) -> int | None:
        //     """Return the CT color value in Kelvin."""
        //     if self._attr_color_temp_kelvin is None and (color_temp := self.color_temp):
        //         report_usage(
        //             "is using mireds for current light color temperature, when "
        //             "it should be adjusted to use the kelvin attribute "
        //             "`_attr_color_temp_kelvin` or override the kelvin property "
        //             "`color_temp_kelvin` (see "
        //             "https://github.com/home-assistant/core/pull/79591)",
        //             breaks_in_ha_version="2026.1",
        //             core_behavior=ReportBehavior.LOG,
        //             integration_domain=self.platform.platform_name
        //             if self.platform
        //             else None,
        //             exclude_integrations={DOMAIN},
        //         )
        //         return color_util.color_temperature_mired_to_kelvin(color_temp)
        //     return self._attr_color_temp_kelvin
        public int? GetColorTempKelvin()
        {
            if (ColorTempKelvin == null && ColorTemp != null)
            {
                // Log usage
                return ColorUtil.ColorTemperatureMiredToKelvin(ColorTemp.Value);
            }
            return ColorTempKelvin;
        }

        // #src: https://raw.githubusercontent.com/home-assistant/core/refs/heads/dev/homeassistant/components/light/__init__.py
        // @property
        // def min_color_temp_kelvin(self) -> int:
        //     """Return the warmest color_temp_kelvin that this light supports."""
        //     if self._attr_min_color_temp_kelvin is None:
        //         report_usage(
        //             "is using mireds for warmest light color temperature, when "
        //             "it should be adjusted to use the kelvin attribute "
        //             "`_attr_min_color_temp_kelvin` or override the kelvin property "
        //             "`min_color_temp_kelvin`, possibly with default DEFAULT_MIN_KELVIN "
        //             "(see https://github.com/home-assistant/core/pull/79591)",
        //             breaks_in_ha_version="2026.1",
        //             core_behavior=ReportBehavior.LOG,
        //             integration_domain=self.platform.platform_name
        //             if self.platform
        //             else None,
        //             exclude_integrations={DOMAIN},
        //         )
        //         return color_util.color_temperature_mired_to_kelvin(self.max_mireds)
        //     return self._attr_min_color_temp_kelvin
        public int GetMinColorTempKelvin()
        {
            if (MinColorTempKelvin == null)
            {
                // Log usage
                return ColorUtil.ColorTemperatureMiredToKelvin(MaxMireds);
            }
            return MinColorTempKelvin.Value;
        }

        // #src: https://raw.githubusercontent.com/home-assistant/core/refs/heads/dev/homeassistant/components/light/__init__.py
        // @property
        // def max_color_temp_kelvin(self) -> int:
        //     """Return the coldest color_temp_kelvin that this light supports."""
        //     if self._attr_max_color_temp_kelvin is None:
        //         report_usage(
        //             "is using mireds for coldest light color temperature, when "
        //             "it should be adjusted to use the kelvin attribute "
        //             "`_attr_max_color_temp_kelvin` or override the kelvin property "
        //             "`max_color_temp_kelvin`, possibly with default DEFAULT_MAX_KELVIN "
        //             "(see https://github.com/home-assistant/core/pull/79591)",
        //             breaks_in_ha_version="2026.1",
        //             core_behavior=ReportBehavior.LOG,
        //             integration_domain=self.platform.platform_name
        //             if self.platform
        //             else None,
        //             exclude_integrations={DOMAIN},
        //         )
        //         return color_util.color_temperature_mired_to_kelvin(self.min_mireds)
        //     return self._attr_max_color_temp_kelvin
        public int GetMaxColorTempKelvin()
        {
            if (MaxColorTempKelvin == null)
            {
                // Log usage
                return ColorUtil.ColorTemperatureMiredToKelvin(MinMireds);
            }
            return MaxColorTempKelvin.Value;
        }

        // #src: https://raw.githubusercontent.com/home-assistant/core/refs/heads/dev/homeassistant/components/light/__init__.py
        // @property
        // def capability_attributes(self) -> dict[str, Any]:
        //     """Return capability attributes."""
        //     data: dict[str, Any] = {}
        //     supported_features = self.supported_features_compat
        //     supported_color_modes = self._light_internal_supported_color_modes

        //     if ColorMode.COLOR_TEMP in supported_color_modes:
        //         min_color_temp_kelvin = self.min_color_temp_kelvin
        //         max_color_temp_kelvin = self.max_color_temp_kelvin
        //         data[ATTR_MIN_COLOR_TEMP_KELVIN] = min_color_temp_kelvin
        //         data[ATTR_MAX_COLOR_TEMP_KELVIN] = max_color_temp_kelvin
        //         if not max_color_temp_kelvin:
        //             data[_DEPRECATED_ATTR_MIN_MIREDS.value] = None
        //         else:
        //             data[_DEPRECATED_ATTR_MIN_MIREDS.value] = (
        //                 color_util.color_temperature_kelvin_to_mired(max_color_temp_kelvin)
        //             )
        //         if not min_color_temp_kelvin:
        //             data[_DEPRECATED_ATTR_MAX_MIREDS.value] = None
        //         else:
        //             data[_DEPRECATED_ATTR_MAX_MIREDS.value] = (
        //                 color_util.color_temperature_kelvin_to_mired(min_color_temp_kelvin)
        //             )
        //     if LightEntityFeature.EFFECT in supported_features:
        //         data[ATTR_EFFECT_LIST] = self.effect_list

        //     data[ATTR_SUPPORTED_COLOR_MODES] = sorted(supported_color_modes)

        //     return data
        public Dictionary<string, object?> GetCapabilityAttributes()
        {
            var data = new Dictionary<string, object?>();
            var supportedFeatures = GetSupportedFeaturesCompat();
            var supportedColorModes = GetInternalSupportedColorModes();

            if (supportedColorModes.Contains("color_temp"))
            {
                var minColorTempKelvin = GetMinColorTempKelvin();
                var maxColorTempKelvin = GetMaxColorTempKelvin();
                data["min_color_temp_kelvin"] = minColorTempKelvin;
                data["max_color_temp_kelvin"] = maxColorTempKelvin;
                if (maxColorTempKelvin == 0)
                {
                    data["min_mireds"] = null;
                }
                else
                {
                    data["min_mireds"] = ColorUtil.ColorTemperatureKelvinToMired(maxColorTempKelvin);
                }
                if (minColorTempKelvin == 0)
                {
                    data["max_mireds"] = null;
                }
                else
                {
                    data["max_mireds"] = ColorUtil.ColorTemperatureKelvinToMired(minColorTempKelvin);
                }
            }
            if (supportedFeatures.HasFlag(LightEntityFeature.Effect))
            {
                data["effect_list"] = EffectList;
            }

            data["supported_color_modes"] = new List<string>(supportedColorModes);

            return data;
        }

        // #src: https://raw.githubusercontent.com/home-assistant/core/refs/heads/dev/homeassistant/components/light/__init__.py
        // def _light_internal_convert_color(
        //     self, color_mode: ColorMode | str
        // ) -> dict[str, tuple[float, ...]]:
        //     data: dict[str, tuple[float, ...]] = {}
        //     if color_mode == ColorMode.HS and (hs_color := self.hs_color):
        //         data[ATTR_HS_COLOR] = (round(hs_color[0], 3), round(hs_color[1], 3))
        //         data[ATTR_RGB_COLOR] = color_util.color_hs_to_RGB(*hs_color)
        //         data[ATTR_XY_COLOR] = color_util.color_hs_to_xy(*hs_color)
        //     elif color_mode == ColorMode.XY and (xy_color := self.xy_color):
        //         data[ATTR_HS_COLOR] = color_util.color_xy_to_hs(*xy_color)
        //         data[ATTR_RGB_COLOR] = color_util.color_xy_to_RGB(*xy_color)
        //         data[ATTR_XY_COLOR] = (round(xy_color[0], 6), round(xy_color[1], 6))
        //     elif color_mode == ColorMode.RGB and (rgb_color := self.rgb_color):
        //         data[ATTR_HS_COLOR] = color_util.color_RGB_to_hs(*rgb_color)
        //         data[ATTR_RGB_COLOR] = tuple(int(x) for x in rgb_color[0:3])
        //         data[ATTR_XY_COLOR] = color_util.color_RGB_to_xy(*rgb_color)
        //     elif color_mode == ColorMode.RGBW and (
        //         rgbw_color := self._light_internal_rgbw_color
        //     ):
        //         rgb_color = color_util.color_rgbw_to_rgb(*rgbw_color)
        //         data[ATTR_HS_COLOR] = color_util.color_RGB_to_hs(*rgb_color)
        //         data[ATTR_RGB_COLOR] = tuple(int(x) for x in rgb_color[0:3])
        //         data[ATTR_RGBW_COLOR] = tuple(int(x) for x in rgbw_color[0:4])
        //         data[ATTR_XY_COLOR] = color_util.color_RGB_to_xy(*rgb_color)
        //     elif color_mode == ColorMode.RGBWW and (rgbww_color := self.rgbww_color):
        //         rgb_color = color_util.color_rgbww_to_rgb(
        //             *rgbww_color, self.min_color_temp_kelvin, self.max_color_temp_kelvin
        //         )
        //         data[ATTR_HS_COLOR] = color_util.color_RGB_to_hs(*rgb_color)
        //         data[ATTR_RGB_COLOR] = tuple(int(x) for x in rgb_color[0:3])
        //         data[ATTR_RGBWW_COLOR] = tuple(int(x) for x in rgbww_color[0:5])
        //         data[ATTR_XY_COLOR] = color_util.color_RGB_to_xy(*rgb_color)
        //     elif color_mode == ColorMode.COLOR_TEMP and (
        //         color_temp_kelvin := self.color_temp_kelvin
        //     ):
        //         hs_color = color_util.color_temperature_to_hs(color_temp_kelvin)
        //         data[ATTR_HS_COLOR] = (round(hs_color[0], 3), round(hs_color[1], 3))
        //         data[ATTR_RGB_COLOR] = color_util.color_hs_to_RGB(*hs_color)
        //         data[ATTR_XY_COLOR] = color_util.color_hs_to_xy(*hs_color)
        //     return data
        private Dictionary<string, Tuple<float, float>> InternalConvertColor(string colorMode)
        {
            var data = new Dictionary<string, Tuple<float, float>>();
            if (colorMode == "hs" && HsColor != null)
            {
                data["hs_color"] = Tuple.Create((float)Math.Round(HsColor.Item1, 3), (float)Math.Round(HsColor.Item2, 3));
                data["rgb_color"] = ColorUtil.ColorHsToRgb(HsColor.Item1, HsColor.Item2);
                data["xy_color"] = ColorUtil.ColorHsToXy(HsColor.Item1, HsColor.Item2);
            }
            else if (colorMode == "xy" && XyColor != null)
            {
                data["hs_color"] = ColorUtil.ColorXyToHs(XyColor.Item1, XyColor.Item2);
                data["rgb_color"] = ColorUtil.ColorXyToRgb(XyColor.Item1, XyColor.Item2);
                data["xy_color"] = Tuple.Create((float)Math.Round(XyColor.Item1, 6), (float)Math.Round(XyColor.Item2, 6));
            }
            else if (colorMode == "rgb" && RgbColor != null)
            {
                data["hs_color"] = ColorUtil.ColorRgbToHs(RgbColor.Item1, RgbColor.Item2, RgbColor.Item3);
                data["rgb_color"] = Tuple.Create(RgbColor.Item1, RgbColor.Item2, RgbColor.Item3);
                data["xy_color"] = ColorUtil.ColorRgbToXy(RgbColor.Item1, RgbColor.Item2, RgbColor.Item3);
            }
            else if (colorMode == "rgbw" && RgbwColor != null)
            {
                var rgbColor = ColorUtil.ColorRgbwToRgb(RgbwColor.Item1, RgbwColor.Item2, RgbwColor.Item3, RgbwColor.Item4);
                data["hs_color"] = ColorUtil.ColorRgbToHs(rgbColor.Item1, rgbColor.Item2, rgbColor.Item3);
                data["rgb_color"] = Tuple.Create(rgbColor.Item1, rgbColor.Item2, rgbColor.Item3);
                data["rgbw_color"] = Tuple.Create(RgbwColor.Item1, RgbwColor.Item2, RgbwColor.Item3, RgbwColor.Item4);
                data["xy_color"] = ColorUtil.ColorRgbToXy(rgbColor.Item1, rgbColor.Item2, rgbColor.Item3);
            }
            else if (colorMode == "rgbww" && RgbwwColor != null)
            {
                var rgbColor = ColorUtil.ColorRgbwwToRgb(RgbwwColor.Item1, RgbwwColor.Item2, RgbwwColor.Item3, RgbwwColor.Item4, RgbwwColor.Item5, MinColorTempKelvin.Value, MaxColorTempKelvin.Value);
                data["hs_color"] = ColorUtil.ColorRgbToHs(rgbColor.Item1, rgbColor.Item2, rgbColor.Item3);
                data["rgb_color"] = Tuple.Create(rgbColor.Item1, rgbColor.Item2, rgbColor.Item3);
                data["rgbww_color"] = Tuple.Create(RgbwwColor.Item1, RgbwwColor.Item2, RgbwwColor.Item3, RgbwwColor.Item4, RgbwwColor.Item5);
                data["xy_color"] = ColorUtil.ColorRgbToXy(rgbColor.Item1, rgbColor.Item2, rgbColor.Item3);
            }
            else if (colorMode == "color_temp" && ColorTempKelvin != null)
            {
                var hsColor = ColorUtil.ColorTemperatureToHs(ColorTempKelvin.Value);
                data["hs_color"] = Tuple.Create((float)Math.Round(hsColor.Item1, 3), (float)Math.Round(hsColor.Item2, 3));
                data["rgb_color"] = ColorUtil.ColorHsToRgb(hsColor.Item1, hsColor.Item2);
                data["xy_color"] = ColorUtil.ColorHsToXy(hsColor.Item1, hsColor.Item2);
            }
            return data;
        }

        private void ValidateColorMode(string? colorMode, HashSet<string> supportedColorModes, string? effect)
        {
            if (colorMode == null || colorMode == "unknown")
            {
                return;
            }

            if (string.IsNullOrEmpty(effect) || effect == "off")
            {
                if (supportedColorModes.Contains(colorMode))
                {
                    return;
                }
                if (!_colorModeReported)
                {
                    _colorModeReported = true;
                    // Log warning
                }
                return;
            }

            var effectColorModes = new HashSet<string>(supportedColorModes) { "onoff" };
            if (BrightnessSupported(effectColorModes))
            {
                effectColorModes.Add("brightness");
            }

            if (effectColorModes.Contains(colorMode))
            {
                return;
            }

            if (!_colorModeReported)
            {
                _colorModeReported = true;
                // Log warning
            }
        }

        private void ValidateSupportedColorModes(HashSet<string> supportedColorModes)
        {
            if (_colorModeReported)
            {
                return;
            }

            try
            {
                ValidSupportedColorModes(supportedColorModes);
            }
            catch (Exception)
            {
                if (!_colorModeReported)
                {
                    _colorModeReported = true;
                    // Log warning
                }
            }
        }

        public Dictionary<string, object?> GetStateAttributes()
        {
            var data = new Dictionary<string, object?>();
            var supportedFeatures = GetSupportedFeaturesCompat();
            var supportedColorModes = SupportedColorModes ?? GetInternalSupportedColorModes();
            var supportedFeaturesValue = supportedFeatures.Value;
            var isOn = IsOn;
            var colorMode = isOn ? GetInternalColorMode() : null;

            if (supportedFeatures.HasFlag(LightEntityFeature.Effect))
            {
                data["effect"] = isOn ? Effect : null;
            }

            ValidateColorMode(colorMode, supportedColorModes, isOn ? Effect : null);

            data["color_mode"] = colorMode;

            if (BrightnessSupported(supportedColorModes))
            {
                data["brightness"] = ColorMode == "brightness" ? Brightness : null;
            }
            else if (supportedFeaturesValue.HasFlag(LightEntityFeature.Brightness))
            {
                data["brightness"] = isOn ? Brightness : null;
            }

            if (ColorTempSupported(supportedColorModes))
            {
                if (ColorMode == "color_temp")
                {
                    data["color_temp_kelvin"] = GetColorTempKelvin();
                    data["color_temp"] = GetColorTempKelvin() != null ? (int?)ColorUtil.ColorTemperatureKelvinToMired(GetColorTempKelvin().Value) : null;
                }
                else
                {
                    data["color_temp_kelvin"] = null;
                    data["color_temp"] = null;
                }
            }
            else if (supportedFeaturesValue.HasFlag(LightEntityFeature.ColorTemp))
            {
                data["color_temp_kelvin"] = isOn ? GetColorTempKelvin() : null;
                data["color_temp"] = isOn ? (int?)ColorUtil.ColorTemperatureKelvinToMired(GetColorTempKelvin().Value) : null;
            }

            if (ColorSupported(supportedColorModes) || ColorTempSupported(supportedColorModes))
            {
                data["hs_color"] = null;
                data["rgb_color"] = null;
                data["xy_color"] = null;
                if (supportedColorModes.Contains("rgbw"))
                {
                    data["rgbw_color"] = null;
                }
                if (supportedColorModes.Contains("rgbww"))
                {
                    data["rgbww_color"] = null;
                }
                if (colorMode != null)
                {
                    var colorData = InternalConvertColor(colorMode);
                    foreach (var item in colorData)
                    {
                        data[item.Key] = item.Value;
                    }
                }
            }

            return data;
        }

        private HashSet<string> GetInternalSupportedColorModes()
        {
            if (SupportedColorModes != null)
            {
                ValidateSupportedColorModes(SupportedColorModes);
                return SupportedColorModes;
            }

            if (!_colorModeReported)
            {
                _colorModeReported = true;
                // Log warning
            }

            var supportedFeatures = GetSupportedFeaturesCompat();
            var supportedFeaturesValue = supportedFeatures.Value;
            var supportedColorModes = new HashSet<string>();

            if (supportedFeaturesValue.HasFlag(LightEntityFeature.ColorTemp))
            {
                supportedColorModes.Add("color_temp");
            }
            if (supportedFeaturesValue.HasFlag(LightEntityFeature.Color))
            {
                supportedColorModes.Add("hs");
            }
            if (!supportedColorModes.Contains("brightness") && supportedFeaturesValue.HasFlag(LightEntityFeature.Brightness))
            {
                supportedColorModes.Add("brightness");
            }
            if (!supportedColorModes.Contains("onoff"))
            {
                supportedColorModes.Add("onoff");
            }

            return supportedColorModes;
        }

        public HashSet<string>? SupportedColorModes { get; set; }

        public LightEntityFeature SupportedFeatures { get; set; } = new LightEntityFeature(0);

        private HashSet<string> GetInternalSupportedColorModes()
        {
            if (SupportedColorModes != null)
            {
                ValidateSupportedColorModes(SupportedColorModes);
                return SupportedColorModes;
            }

            if (!_colorModeReported)
            {
                _colorModeReported = true;
                // Log warning
            }

            var supportedFeatures = GetSupportedFeaturesCompat();
            var supportedFeaturesValue = supportedFeatures.Value;
            var supportedColorModes = new HashSet<string>();

            if (supportedFeaturesValue.HasFlag(LightEntityFeature.ColorTemp))
            {
                supportedColorModes.Add("color_temp");
            }
            if (supportedFeaturesValue.HasFlag(LightEntityFeature.Color))
            {
                supportedColorModes.Add("hs");
            }
            if (!supportedColorModes.Contains("brightness") && supportedFeaturesValue.HasFlag(LightEntityFeature.Brightness))
            {
                supportedColorModes.Add("brightness");
            }
            if (!supportedColorModes.Contains("onoff"))
            {
                supportedColorModes.Add("onoff");
            }

            return supportedColorModes;
        }
    }
}