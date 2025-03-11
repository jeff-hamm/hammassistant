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
        if (mode == ColorMode.Unknown && attributes.ColorMode is {} and not ColorMode.Unknown)
        {
            mode = attributes.ColorMode.Value;
        }
        
        return ToParameters(attributes, mode);
    }

    public static LightTurnOnParameters CopyParametersFrom(this LightEntity @this, LightTurnOnParameters parameters)
    {
        var supportedColorModes = @this.Attributes?.SupportedColorModes?.ToArray() ?? [];
        var colorMode = parameters.DetermineColorMode();
        if (colorMode == ColorMode.Unknown || supportedColorModes.Length == 0)
            return parameters;
        var hasBrightness = parameters.Brightness  != null || parameters.White != null;
        var hasColorTemp = parameters.ColorTemp != null || parameters.ColorTempKelvin != null;
        var hasColor = colorMode.SupportsColor();
        ColorMode bestMode = supportedColorModes.First();
        foreach (var mode in supportedColorModes)
        {
            if (mode == colorMode)
                return parameters;
            if (hasBrightness)
            {
                if (!mode.SupportsBrightness()) continue;
                if(!bestMode.SupportsBrightness())
                    bestMode = mode;
            }
            if (hasColorTemp && mode.SupportsColorTemp() && !bestMode.SupportsColorTemp())
                bestMode = mode;
            else if (hasColor && mode.SupportsColor() && !bestMode.SupportsColor())
                return parameters;
        }   
        if(hasBrightness && !bestMode.SupportsBrightness())
            parameters = parameters with { Brightness = null, White = null };
        if (hasColorTemp && !bestMode.SupportsColorTemp())
            parameters = parameters with { ColorTemp = null, ColorTempKelvin = null };
        if(hasColor && !bestMode.SupportsColor())
            parameters = parameters with { RgbColor = null, HsColor = null, XyColor = null, RgbwColor = null, RgbwwColor = null };
        return parameters;
    }

    public static LightTurnOnParameters ToParameters(this LightAttributes attributes, ColorMode? mode=null)
    {
        mode ??= attributes.ColorMode ?? ColorMode.Unknown;
        double? kelvin = null;
        double? colorTemp = null;
        if (mode == ColorMode.ColorTemp)
        {
            if(attributes.ColorTempKelvin != null)
                kelvin = attributes.ColorTempKelvin;
            else if (attributes.ColorTemp != null)
                colorTemp = attributes.ColorTemp;
        }

        return new LightTurnOnParameters
        {
            Brightness = mode?.HasFlag(ColorMode.Brightness) == true ? attributes.Brightness : null,
            ColorTempKelvin = kelvin,
            ColorTemp = colorTemp,
            Effect = attributes.Effect?.ToString(),
            White = mode == ColorMode.White ? attributes.Brightness : null,
            RgbColor = mode == ColorMode.Rgb ? attributes.RgbColor?.Select(v => (int)v).ToArray() : null,
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
    public static ColorMode DetermineColorMode(this LightTurnOnParameters parameters)
    {
        if (parameters.ColorTemp != null || parameters.ColorTempKelvin != null)
            return ColorMode.ColorTemp;
        if (parameters.RgbColor != null)
            return ColorMode.Rgb;
        if (parameters.HsColor != null)
            return ColorMode.Hs;
        if (parameters.XyColor != null)
            return ColorMode.Xy;
        if (parameters.RgbwColor != null)
            return ColorMode.Rgbw;
        if (parameters.RgbwwColor != null)
            return ColorMode.Rgbww;
        if (parameters.White != null)
            return ColorMode.White;
        if (parameters.Brightness.HasValue)
            return ColorMode.Brightness;

        return ColorMode.Unknown;
    }
}