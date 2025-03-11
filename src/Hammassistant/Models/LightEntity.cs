using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using Hammlet.Models.Enums;
using NetDaemon.HassModel.Entities;

namespace Hammlet.NetDaemon.Models;

public class TurnOnParameterBuilder(LightEntity entity)
{
    private LightTurnOnParameters _parameters;
    public TurnOnParameterBuilder WithTransition(double transition)
    {
        _parameters = _parameters with { Transition = transition };
        return this;
    }
    public TurnOnParameterBuilder WithRgbColor(int r, int g, int b)
    {
        _parameters = _parameters with { RgbColor = new[] { r, g, b } };
        return this;
    }
    public TurnOnParameterBuilder WithKelvin(double kelvin)
    {
        _parameters = _parameters with { ColorTempKelvin = kelvin };
        return this;
    }
    public TurnOnParameterBuilder WithBrightnessPct(double brightnessPct)
    {
        _parameters = _parameters with { BrightnessPct = brightnessPct };
        return this;
    }
    public TurnOnParameterBuilder WithBrightnessStepPct(double brightnessStepPct)
    {
        _parameters = _parameters with { BrightnessStepPct = brightnessStepPct };
        return this;
    }
    public TurnOnParameterBuilder WithEffect(string effect)
    {
        _parameters = _parameters with { Effect = effect };
        return this;
    }
    public TurnOnParameterBuilder WithRgbwColor(int r, int g, int b, int w)
    {
        _parameters = _parameters with { RgbwColor = new[] { r, g, b, w } };
        return this;
    }
    public TurnOnParameterBuilder WithRgbwwColor(int r, int g, int b, int w, int ww)
    {
        _parameters = _parameters with { RgbwwColor = new[] { r, g, b, w, ww } };
        return this;
    }
    public TurnOnParameterBuilder WithColorName(string colorName)
    {
        _parameters = _parameters with { ColorName = colorName };
        return this;
    }
    public TurnOnParameterBuilder WithHsColor(double h, double s)
    {
        _parameters = _parameters with { HsColor = new[] { h, s } };
        return this;
    }
    public TurnOnParameterBuilder WithXyColor(double x, double y)
    {
        _parameters = _parameters with { XyColor = new[] { x, y } };
        return this;
    }
    public TurnOnParameterBuilder WithColorTemp(double colorTemp)
    {
        _parameters = _parameters with { ColorTemp = colorTemp };
        return this;
    }
    public LightTurnOnParameters Build() => _parameters;
}

public static class LightEntityExtensions
{
    
    public static int MaxBrightnes(this LightEntity @this) => 255;
    public static int MinBrightnes(this LightEntity @this) => 0;
    public static double? Brightness(this LightEntity @this) => 
        Double.TryParse(@this.Attributes?.Brightness?.ToString(), out var r) ? r : null;
    
    public static void Brighten(this LightEntity @this, double pct=20)
    {
        @this.TurnOn(brightnessStepPct: pct);
    }
    public static void Darken(this LightEntity @this, double pct=-20)
    {
        @this.TurnOn(brightnessStepPct: pct);
    }

    public static bool HasColorTemp(this LightEntity @this) =>
        @this.IsOn() && @this.Attributes?.HasColorTemp() == true;
    public static bool HasColorTemp(this LightAttributes @this) =>
        @this is { ColorMode: ColorMode.ColorTemp } att&&
        (att.ColorTempKelvin != null || att.ColorTemp != null);

    public static (double? k, double? r) GetColorTempFromPct(this LightEntity @this, double colorTemp) => @this.Attributes?.GetColorTempFromPct(colorTemp) ?? (null,null);
    public static (double? k, double? r) GetColorTempFromPct(this LightAttributes @this, double colorTemp)
    {
        if (colorTemp > 1.0)
            colorTemp /= 100.0;
        if (@this.SupportedColorModes?.Contains(ColorMode.ColorTemp) == true)
        {
            switch (@this)
            {
                case { MinColorTempKelvin: { } minK, MaxColorTempKelvin: { } maxK }:
                {
                    var dst = ((maxK - minK) * colorTemp) + minK;
                    return (dst, null);
//                @this.TurnOn(kelvin: dst);
                    //              break;
                }
                case { MinMireds: { } min, MaxMireds: { } max }:
                {
                    var dst = ((max - min) * colorTemp) + min;
                    return (null, dst);
//                @this.TurnOn(colorTemp: dst);
//                break;
                }
                default:
                    return (null, null);
            }
        }
        return (null, null);
    }

    public static void SetColorTempPct(this LightEntity @this, double colorTemp, LightTurnOnParameters? mapTo=null)
    {
        if (@this.Attributes?.SupportedColorModes?.Contains(ColorMode.ColorTemp) != true) return;
        switch (@this.Attributes)
        {
            case { MinColorTempKelvin: { } minK, MaxColorTempKelvin: { } maxK}: 
                var dstK = ((maxK - minK) * colorTemp) + minK;
                @this.TurnOn(kelvin:dstK);
                break;
            
            case { MinMireds: { } min, MaxMireds: { } max }:
                var dst = ((max - min) * colorTemp) + min;
                @this.TurnOn(colorTemp: dst);
                break;
        }
    }
    public static bool IsWarm(this LightEntity @this) =>
        @this.ColorTempPct() is < .5;
    public static double? ColorTempPct(this LightEntity @this)  => @this.Attributes?.ColorTempPct();

    public static double? ColorTempPct(this LightAttributes @this)
    {
        try
        {
            if (!@this.HasColorTemp())
                return null;
            if (@this.ColorTempKelvin is { } k && @this is { MinColorTempKelvin: { } minK, MaxColorTempKelvin: { } maxK })
            {
                return (k - minK) / (maxK - minK);
            }

            if (@this.ColorTemp is { } colorTemp && @this is { MinMireds: { } min, MaxMireds: { } max })
            {
                return (colorTemp - min) / (max - min);
            }

            return null;
        }
        catch (Exception e)
        {

            throw;
        }
    }

    public static void MakeWarm(this LightEntity @this)
    {
        if (!@this.IsWarm())
        {
            @this.SetColorTempPct(.1);
        }
    }

    public static void ToggleWarm(this LightEntity @this)
    {
        if (!@this.IsOn()) return;
        double dstColorTempPct = .1;
        if (@this.IsWarm())
        {
            @this.SetColorTempPct(.9);
        }
        else
        {
            @this.SetColorTempPct(.1);
        }
        //if (att is { MinMireds: { } min, MaxMireds: { } max })
        //{
        //    if (a.ColorMode is ColorMode.ColorTemp && att.ColorTemp is { } colorTemp)
        //    {
        //        var colorTempPct = (colorTemp - min) / (max - min);
        //        if (colorTempPct < .5)
        //        {
        //            dstColorTempPct = .9;
        //        }
        //        else
        //        {
        //            dstColorTempPct = .1;
        //        }
        //    }

        //    var dst = ((max - min) * dstColorTempPct) + min;
        //    @this.TurnOn(colorTemp: dst);
        //}
        //else if (att is { MinColorTempKelvin: { } minK, MaxColorTempKelvin: { } maxK })
        //{
        //    if (a.ColorMode is ColorMode.ColorTemp && att.ColorTemp is { } colorTemp)
        //    {
        //        var colorTempPct = (colorTemp - minK) / (maxK - minK);
        //        if (colorTempPct < .5)
        //        {
        //            dstColorTempPct = .9;
        //        }
        //        else
        //        {
        //            dstColorTempPct = .1;
        //        }
        //    }

        //    var dst = ((maxK - minK) * dstColorTempPct) + minK;
        //    @this.TurnOn(kelvin:dst);
        //}


    }
}

public record SafeEntityState<TAttributes> : EntityState<TAttributes> where TAttributes:class
{
    public static readonly JsonSerializerOptions DefaultJsonSerializerOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower) }
    };
    private readonly Lazy<TAttributes?> _attributesLazy;

    public SafeEntityState(EntityState state) : base(state)
    {
        this._attributesLazy = new Lazy<TAttributes?>((Func<TAttributes>)(() =>
        {
            var attributesJson = this.AttributesJson;
            try
            {
                return (attributesJson.HasValue
                    ? attributesJson.GetValueOrDefault()
                        .Deserialize<TAttributes>(
                        DefaultJsonSerializerOptions
                        )
                    : default(TAttributes)) ?? default(TAttributes);
            }
            catch (Exception e)
            {
                return null;
            }
        }));
    }


    /// <inheritdoc />
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
    public override TAttributes? Attributes => _attributesLazy.Value;
}
public partial record LightEntity
{
    public const double WarmLightPct = 0.1;
    public const double CoolLightPct = 0.1;


    private EntityState<LightAttributes>? _state;

    public override EntityState<LightAttributes>? EntityState => 
        
        _state ??= 
            this.HaContext.GetState(this.EntityId) is {} s ?
            new SafeEntityState<LightAttributes>(s) : null;
}