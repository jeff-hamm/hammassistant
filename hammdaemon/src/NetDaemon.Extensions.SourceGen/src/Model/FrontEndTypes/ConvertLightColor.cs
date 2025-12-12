namespace NetDaemon.Extensions.SourceGen.Model.FrontEndTypes;

public static class ConvertLightColor
{
    public const int DEFAULT_MIN_KELVIN = 2700;
    public const int DEFAULT_MAX_KELVIN = 6500;

    public static int[] Temperature2Rgb(int temperature)
    {
        var value = temperature / 100.0;
        return new[]
        {
            (int)Math.Round(TemperatureRed(value)),
            (int)Math.Round(TemperatureGreen(value)),
            (int)Math.Round(TemperatureBlue(value))
        };
    }

    private static double TemperatureRed(double temperature)
    {
        if (temperature <= 66)
            return 255;

        var red = 329.698727446 * Math.Pow(temperature - 60, -0.1332047592);
        return Clamp(red, 0, 255);
    }

    private static double TemperatureGreen(double temperature)
    {
        double green;
        if (temperature <= 66)
            green = 99.4708025861 * Math.Log(temperature) - 161.1195681661;
        else
            green = 288.1221695283 * Math.Pow(temperature - 60, -0.0755148492);

        return Clamp(green, 0, 255);
    }

    private static double TemperatureBlue(double temperature)
    {
        if (temperature >= 66)
            return 255;

        if (temperature <= 19)
            return 0;

        var blue = 138.5177312231 * Math.Log(temperature - 10) - 305.0447927307;
        return Clamp(blue, 0, 255);
    }

    private static int[] MatchMaxScale(int[] inputColors, int[] outputColors)
    {
        var maxIn = inputColors.Max();
        var maxOut = outputColors.Max();
        double factor = maxOut == 0 ? 0.0 : (double)maxIn / maxOut;
        return Array.ConvertAll(outputColors, value => (int)Math.Round(value * factor));
    }

    public static int Mired2Kelvin(int miredTemperature)
    {
        return miredTemperature == 0 ? 1000000 : (int)Math.Floor(1000000.0 / miredTemperature);
    }

    public static int Kelvin2Mired(int kelvinTemperature)
    {
        return kelvinTemperature == 0 ? 1000000 : (int)Math.Floor(1000000.0 / kelvinTemperature);
    }

    public static int[] Rgbww2Rgb(int[] rgbww, int? minKelvin = null, int? maxKelvin = null)
    {
        var (r, g, b, cw, ww) = (rgbww[0], rgbww[1], rgbww[2], rgbww[3], rgbww[4]);
        var maxMireds = Kelvin2Mired(minKelvin ?? DEFAULT_MIN_KELVIN);
        var minMireds = Kelvin2Mired(maxKelvin ?? DEFAULT_MAX_KELVIN);
        var miredRange = maxMireds - minMireds;
        double ctRatio;
        try
        {
            ctRatio = (double)ww / (cw + ww);
        }
        catch
        {
            ctRatio = 0.5;
        }

        var colorTempMired = minMireds + ctRatio * miredRange;
        var colorTempKelvin = colorTempMired != 0 ? Mired2Kelvin((int)colorTempMired) : 0;
        var (wR, wG, wB) = (Temperature2Rgb(colorTempKelvin)[0], Temperature2Rgb(colorTempKelvin)[1], Temperature2Rgb(colorTempKelvin)[2]);
        var whiteLevel = Math.Max(cw, ww) / 255.0;

        var rgb = new[]
        {
            (int)(r + wR * whiteLevel),
            (int)(g + wG * whiteLevel),
            (int)(b + wB * whiteLevel)
        };

        return MatchMaxScale(new[] { r, g, b, cw, ww }, rgb);
    }

    public static int[] Rgbw2Rgb(int[] rgbw)
    {
        var (r, g, b, w) = (rgbw[0], rgbw[1], rgbw[2], rgbw[3]);
        var rgb = new[]
        {
            r + w,
            g + w,
            b + w
        };
        return MatchMaxScale(new[] { r, g, b, w }, rgb);
    }

    public static double Clamp(double value, double min, double max)
    {
        return Math.Min(Math.Max(value, min), max);
    }

    public static double ConditionalClamp(double value, double? min = null, double? max = null)
    {
        var result = min.HasValue ? Math.Max(value, min.Value) : value;
        result = max.HasValue ? Math.Min(result, max.Value) : result;
        return result;
    }
}

