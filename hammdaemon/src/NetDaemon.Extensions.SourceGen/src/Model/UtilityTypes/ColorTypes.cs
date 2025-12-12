using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace NetDaemon.Extensions.SourceGen.src.Model.UtilityTypes;

public interface ITypedArray<out T>
{
    T[] Values { get; }
}


[JsonConverter(typeof(HsColorConverter))]
public readonly record struct HsColor(double H, double S) : ITypedArray<double>
{
    public HsColor(double[] array) : this(array.Length == 2 ? array[0] : throw new ArgumentException("Array length must be 2."), array[1]) { }

    [JsonIgnore]
    public double[] Values => new[] { H, S };

    public static implicit operator HsColor(double[] array) => new(array);
    public static implicit operator double[](HsColor hsColor) => hsColor.Values;
}

[JsonConverter(typeof(XyColorConverter))]
public readonly record struct XyColor(double X, double Y) : ITypedArray<double>
{
    public XyColor(double[] array) : this(array.Length == 2 ? array[0] : throw new ArgumentException("Array length must be 2."), array[1]) { }

    [JsonIgnore]
    public double[] Values => new[] { X, Y };

    public static implicit operator XyColor(double[] array) => new(array);
    public static implicit operator double[](XyColor xyColor) => xyColor.Values;
}

[JsonConverter(typeof(RgbColorConverter))]
public readonly record struct RgbColor(int R, int G, int B) : ITypedArray<int>
{
    public RgbColor(int[] array) : this(array.Length == 3 ? array[0] : throw new ArgumentException("Array length must be 3."), array[1], array[2]) { }

    [JsonIgnore]
    public int[] Values => new[] { R, G, B };

    public static implicit operator RgbColor(int[] array) => new(array);
    public static implicit operator int[](RgbColor rgbColor) => rgbColor.Values;
}

[JsonConverter(typeof(RgbwColorConverter))]
public readonly record struct RgbwColor(int R, int G, int B, int W) : ITypedArray<int>
{
    public RgbwColor(int[] array) : this(array.Length == 4 ? array[0] : throw new ArgumentException("Array length must be 4."), array[1], array[2], array[3]) { }

    [JsonIgnore]
    public int[] Values => new[] { R, G, B, W };

    public static implicit operator RgbwColor(int[] array) => new(array);
    public static implicit operator int[](RgbwColor rgbwColor) => rgbwColor.Values;
}

[JsonConverter(typeof(RgbwwColorConverter))]
public readonly record struct RgbwwColor(int R, int G, int B, int W1, int W2) : ITypedArray<int>
{
    public RgbwwColor(int[] array) : this(array.Length == 5 ? array[0] : throw new ArgumentException("Array length must be 5."), array[1], array[2], array[3], array[4]) { }

    [JsonIgnore]
    public int[] Values => new[] { R, G, B, W1, W2 };

    public static implicit operator RgbwwColor(int[] array) => new(array);
    public static implicit operator int[](RgbwwColor rgbwwColor) => rgbwwColor.Values;

}

public class ArrayLengthConverter<T, V>(int expectedLength) : JsonConverter<T>
    where T : ITypedArray<V>
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("Expected start of array.");
        }

        var list = new List<V>();
        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            if (typeof(V) == typeof(int))
            {
                list.Add((V)(object)reader.GetInt32());
            }
            else if (typeof(V) == typeof(double))
            {
                list.Add((V)(object)reader.GetDouble());
            }
            else
            {
                throw new JsonException("Expected number value.");
            }
        }

        if (list.Count != expectedLength)
        {
            throw new JsonException($"Expected array length of {expectedLength}, but got {list.Count}.");
        }

        return (T?)Activator.CreateInstance(typeof(T),list.ToArray()) ?? throw new InvalidOperationException($"Could not create {typeof(T)}");
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        var array = value.Values;
        if (array.Length != expectedLength)
        {
            throw new JsonException($"Expected array length of {expectedLength}, but got {array.Length}.");
        }

        writer.WriteStartArray();
        foreach (var item in array)
        {
            writer.WriteNumberValue(Convert.ToDouble(item));
        }
        writer.WriteEndArray();
    }
}

public class HsColorConverter() : ArrayLengthConverter<HsColor, double>(2);

public class XyColorConverter() : ArrayLengthConverter<XyColor, double>(2);

public class RgbColorConverter() : ArrayLengthConverter<RgbColor, int>(3);

public class RgbwColorConverter() : ArrayLengthConverter<RgbwColor, int>(4);

public class RgbwwColorConverter() : ArrayLengthConverter<RgbwwColor, int>(5);
