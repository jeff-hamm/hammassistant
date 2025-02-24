using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Hammlet.Models.Enums;
using Hammlet.NetDaemon.Models;
using NetDaemon.HassModel.Entities;

namespace Hammlet.NetDaemon.Extensions;

public static class  LightExtensions
{

    public static IEnumerable<ColorMode> ParseModes(this IEnumerable<string> states) =>
        states.Select( m => m == "color_temp" ? ColorMode.ColorTemp : m.ParseState<ColorMode>()).Where(s => s != null).Select(s => s!.Value);
    public static bool IsToggleState(this EntityState @this, bool toggleState) => toggleState ? @this.IsOn() : @this.IsOff();

    public static bool IsToggleState(this Entity @this, bool toggleState) => toggleState ? @this.IsOn() : @this.IsOff();

    public static void ToggleState(this LightEntity @this, bool toggleState)
    {
        if (toggleState)
        {
            if (!@this.IsOn())
                @this.TurnOn();
        }
        else if (@this.IsOn())
            @this.TurnOff();
    }

    public static LightTurnOnParameters? CopyParametersFrom(this LightEntity @this, LightEntity target) => 
        target.EntityState?.Attributes != null ? @this.CopyParametersFrom(target.EntityState.Attributes) : null;


    public static LightTurnOnParameters CopyParametersFrom(this LightEntity @this, LightAttributes attributes)
    {
        if (attributes == null)
            return new();
        var supportedColorModes = @this.Attributes?.SupportedColorModes?.ToArray() ?? [];
        ColorMode mode = 
            attributes.ColorMode.HasValue && supportedColorModes.Contains(attributes.ColorMode.Value) ? attributes.ColorMode.Value : ColorMode.Unknown;
        if (mode == ColorMode.Unknown && attributes.ColorMode != ColorMode.Unknown)
        {
            if (
                attributes.ColorMode?.HasFlag(ColorMode.IsColor) == true)
            {
                mode = supportedColorModes.FirstOrDefault(s => s.HasFlag(ColorMode.IsColor) &&
                                                               HasAttributes(s, attributes)
                );
            }
        }
        return new LightTurnOnParameters
        {
            Brightness = mode.HasFlag(ColorMode.Brightness) ? attributes.Brightness : null,
//            Transition = @this.Transition,
            Effect = attributes.Effect?.ToString(),
            White = mode == ColorMode.White ? attributes.Brightness : null,
            ColorTemp = mode == ColorMode.ColorTemp ? attributes.ColorTemp : null,
            RgbColor = mode == ColorMode.Rgb ? attributes.RgbColor as IReadOnlyCollection<int> : null,
            HsColor = mode == ColorMode.Hs ? attributes.HsColor : null,
            XyColor =  mode == ColorMode.Xy ? attributes.XyColor : null,
            RgbwColor = mode == ColorMode.Rgbw ? ToInArray(attributes.RgbwColor ) : null,
            RgbwwColor = mode == ColorMode.Rgbww ? ToInArray(attributes.RgbwColor ) : null,
        };
    }

    private static bool HasAttributes(ColorMode mode, LightAttributes attributes) =>
        mode switch
        {
            ColorMode.White => attributes.Brightness != null,
            ColorMode.ColorTemp => attributes.ColorTemp != null,
            ColorMode.Rgb => attributes.RgbColor != null,
            ColorMode.Hs => attributes.HsColor != null,
            ColorMode.Xy => attributes.XyColor != null,
            ColorMode.Rgbw => attributes.RgbwColor != null,
            ColorMode.Rgbww => attributes.RgbwColor != null,
            _ => false
        };

    private static double? ToDouble(object? value)
    {
        return value is JsonElement { ValueKind: JsonValueKind.Number } el ? el.GetDouble() : null;
    }

    private static int[]? ToInArray(object? value) => value is JsonElement element && element.ValueKind == JsonValueKind.Array ? element.EnumerateArray().Select(e => e.GetInt32()).ToArray() : null;

    private static double[]? ToArray(object? value) => value is JsonElement element && element.ValueKind == JsonValueKind.Array ? element.EnumerateArray().Select(e => e.GetDouble()).ToArray() : null;
}