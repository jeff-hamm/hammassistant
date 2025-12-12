using System.Collections;
using System.Text.Json;
using System.Text.Json.Serialization;
using NetDaemon.Extensions.SourceGen.Model;
using NetDaemon.Extensions.SourceGen.Model.JsWebsocket;
using NetDaemon.Extensions.SourceGen.src.Model.UtilityTypes;
using static Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary;

namespace NetDaemon.Extensions.SourceGen.Model;

// import type {
//   HassEntityAttributeBase,
//   HassEntityBase,
// } from "home-assistant-js-websocket";
// import { temperature2rgb } from "../common/color/convert-light-color";

// export const enum LightEntityFeature {
//   EFFECT = 4,
//   FLASH = 8,
//   TRANSITION = 32,
// }
public enum LightEntityFeature
{
  Effect = 4,
  Flash = 8,
  Transition = 32
}

// export const enum LightColorMode {
//   UNKNOWN = "unknown",
//   ONOFF = "onoff",
//   BRIGHTNESS = "brightness",
//   COLOR_TEMP = "color_temp",
//   HS = "hs",
//   XY = "xy",
//   RGB = "rgb",
//   RGBW = "rgbw",
//   RGBWW = "rgbww",
//   WHITE = "white",
// }
public enum LightColorMode
{
  Unknown = 0,
  OnOff,
  Brightness,
  ColorTemp,
  Hs,
  Xy,
  Rgb,
  Rgbw,
  Rgbww,
  White
}
public static class Light
{
  private const int COLOR_TEMP_COUNT = 4;

  private static readonly LightColor[] DEFAULT_COLORED_COLORS = new LightColor[]
  {
        new LightColor { RgbColor = new[] { 127, 172, 255 } }, // blue #7FACFF
        new LightColor { RgbColor = new[] { 215, 150, 255 } }, // purple #D796FF
        new LightColor { RgbColor = new[] { 255, 158, 243 } }, // pink #FF9EF3
        new LightColor { RgbColor = new[] { 255, 110, 84 } }   // red #FF6E54
  };

  // export const computeDefaultFavoriteColors = (
  //   stateObj: LightEntity
  // ) => {
  //   const colors = [...DEFAULT_COLORED_COLORS];
  //   if (stateObj.attributes.min_color_temp_kelvin) {
  //     const step =
  //       (stateObj.attributes.max_color_temp_kelvin! -
  //         stateObj.attributes.min_color_temp_kelvin) /
  //       (COLOR_TEMP_COUNT - 1);
  //     for (let i = 0; i < COLOR_TEMP_COUNT; i++) {
  //       colors.push({
  //         color_temp_kelvin:
  //           stateObj.attributes.min_color_temp_kelvin + step * i,
  //       });
  //     }
  //   }
  //   return colors;
  // };
  public static List<LightColor> ComputeDefaultFavoriteColors(LightEntity stateObj)
  {
    var colors = new List<LightColor>(DEFAULT_COLORED_COLORS);
    if (stateObj.Attributes.MinColorTempKelvin.HasValue)
    {
      var step = (stateObj.Attributes.MaxColorTempKelvin.Value - stateObj.Attributes.MinColorTempKelvin.Value) / (COLOR_TEMP_COUNT - 1);
      for (int i = 0; i < COLOR_TEMP_COUNT; i++)
      {
        colors.Add(new LightColor
        {
          ColorTempKelvin = stateObj.Attributes.MinColorTempKelvin.Value + (int)(step * i)
        });
      }
    }
    return colors;
  }

  private static readonly LightColorMode[] ModesSupportingColor =
  {
        LightColorMode.Hs,
        LightColorMode.Xy,
        LightColorMode.Rgb,
        LightColorMode.Rgbw,
        LightColorMode.Rgbww
    };

  private static readonly LightColorMode[] ModesSupportingBrightness =
  {
        LightColorMode.Hs,
        LightColorMode.Xy,
        LightColorMode.Rgb,
        LightColorMode.Rgbw,
        LightColorMode.Rgbww,
        LightColorMode.ColorTemp,
        LightColorMode.Brightness,
        LightColorMode.White
    };

  // export const lightSupportsColorMode = (
  //   entity: LightEntity,
  //   mode: LightColorMode
  // ) => entity.attributes.supported_color_modes?.includes(mode) || false;
  public static bool LightSupportsColorMode(LightEntity entity, LightColorMode mode)
  {
    return entity.Attributes.SupportedColorModes?.Contains(mode) ?? false;
  }

  // export const lightIsInColorMode = (entity: LightEntity) =>
  //   (entity.attributes.color_mode &&
  //     modesSupportingColor.includes(entity.attributes.color_mode)) ||
  //   false;
  public static bool LightIsInColorMode(LightEntity entity)
  {
    return entity.Attributes.ColorMode != null && ModesSupportingColor.Contains(entity.Attributes.ColorMode.Value);
  }

  // export const lightSupportsColor = (entity: LightEntity) =>
  //   entity.attributes.supported_color_modes?.some((mode) =>
  //     modesSupportingColor.includes(mode)
  //   ) || false;
  public static bool LightSupportsColor(LightEntity entity)
  {
    return entity.Attributes.SupportedColorModes?.Any(mode => ModesSupportingColor.Contains(mode)) ?? false;
  }

  // export const lightSupportsBrightness = (entity: LightEntity) =>
  //   entity.attributes.supported_color_modes?.some((mode) =>
  //     modesSupportingBrightness.includes(mode)
  //   ) || false;
  public static bool LightSupportsBrightness(LightEntity entity)
  {
    return entity.Attributes.SupportedColorModes?.Any(mode => ModesSupportingBrightness.Contains(mode)) ?? false;
  }

  // export const lightSupportsFavoriteColors = (entity: LightEntity) =>
  //   lightSupportsColor(entity) ||
  //   lightSupportsColorMode(entity, LightColorMode.COLOR_TEMP);
  public static bool LightSupportsFavoriteColors(LightEntity entity)
  {
    return LightSupportsColor(entity) || LightSupportsColorMode(entity, LightColorMode.ColorTemp);
  }

  // export const getLightCurrentModeRgbColor = (
  //   entity: LightEntity
  // ): number[] | undefined =>
  //   entity.attributes.color_mode === LightColorMode.RGBWW
  //     ? entity.attributes.rgbww_color
  //     : entity.attributes.color_mode === LightColorMode.RGBW
  //       ? entity.attributes.rgbw_color
  //       : entity.attributes.rgb_color;
  public static int[] GetLightCurrentModeRgbColor(LightEntity entity)
  {
    return entity.Attributes.ColorMode switch
    {
      LightColorMode.Rgbww => entity.Attributes.RgbwwColor,
      LightColorMode.Rgbw => entity.Attributes.RgbwColor,
      _ => entity.Attributes.RgbColor
    };
  }
}

// interface LightEntityAttributes extends HassEntityAttributeBase {
//   min_color_temp_kelvin?: number;
//   max_color_temp_kelvin?: number;
//   min_mireds?: number;
//   max_mireds?: number;
//   brightness?: number;
//   xy_color?: [number, number];
//   hs_color?: [number, number];
//   color_temp?: number;
//   color_temp_kelvin?: number;
//   rgb_color?: [number, number, number];
//   rgbw_color?: [number, number, number, number];
//   rgbww_color?: [number, number, number, number, number];
//   effect?: string;
//   effect_list?: string[] | null;
//   supported_color_modes?: LightColorMode[];
//   color_mode?: LightColorMode;
// }
public class LightEntityAttributes : HassEntityAttributeBase
{
  [JsonPropertyName("min_color_temp_kelvin")]
  public int? MinColorTempKelvin { get; set; }

  [JsonPropertyName("max_color_temp_kelvin")]
  public int? MaxColorTempKelvin { get; set; }

  [JsonPropertyName("min_mireds")]
  public int? MinMireds { get; set; }

  [JsonPropertyName("max_mireds")]
  public int? MaxMireds { get; set; }

  [JsonPropertyName("brightness")]
  public int? Brightness { get; set; }

  [JsonPropertyName("xy_color")]
  public XyColor XyColor { get; set; }

  [JsonPropertyName("hs_color")]
  public HsColor HsColor { get; set; }

  [JsonPropertyName("color_temp")]
  public int? ColorTemp { get; set; }

  [JsonPropertyName("color_temp_kelvin")]
  public int? ColorTempKelvin { get; set; }

  [JsonPropertyName("rgb_color")]
  public RgbColor RgbColor { get; set; }

  [JsonPropertyName("rgbw_color")]
  public RgbwColor RgbwColor { get; set; }

  [JsonPropertyName("rgbww_color")]
  public RgbwwColor RgbwwColor { get; set; }

  [JsonPropertyName("effect")]
  public string? Effect { get; set; }

  [JsonPropertyName("effect_list")]
  public IReadOnlyList<string>? EffectList { get; set; }

  [JsonPropertyName("supported_color_modes")]
  public IReadOnlyList<LightColorMode>? SupportedColorModes { get; set; }

  [JsonPropertyName("color_mode")]
  public LightColorMode? ColorMode { get; set; }
}

// export interface LightEntity extends HassEntityBase {
//   attributes: LightEntityAttributes;
// }
public class LightEntity : HassEntityBase
{
  [JsonPropertyName("attributes")]
  public LightEntityAttributes Attributes { get; set; }
}

// export type LightColor =
//   | {
//       color_temp_kelvin: number;
//     }
//   | {
//       hs_color: [number, number];
//     }
//   | {
//       rgb_color: [number, number, number];
//     }
//   | {
//       rgbw_color: [number, number, number, number];
//     }
//   | {
//       rgbww_color: [number, number, number, number, number];
//     };
public class LightColor
{
  [JsonPropertyName("color_temp_kelvin")]
  public int? ColorTempKelvin { get; set; }
  [JsonPropertyName("xy_color")]
  public XyColor? XyColor { get; set; }

  [JsonPropertyName("hs_color")]
  public HsColor? HsColor { get; set; }

  [JsonPropertyName("rgb_color")]
  public RgbColor? RgbColor { get; set; }

  [JsonPropertyName("rgbw_color")]
  public RgbwColor? RgbwColor { get; set; }

  [JsonPropertyName("rgbww_color")]
  public RgbwwColor? RgbwwColor { get; set; }
}

public interface ITypedArray<out T> : IEnumerable<T>
{
    T[] Values { get; }
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => ((IEnumerable<T>)Values).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)Values).GetEnumerator();
    }
}
