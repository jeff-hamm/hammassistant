﻿using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NetDaemon.Extensions.Hammlet;

/// <summary>
/// Converts a Json element that can be a string or a string array
/// </summary>
public class StringAsArrayConverter : JsonConverter<string[]>
{
    public override string[]? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            return [reader.GetString() ?? throw new UnreachableException("Token is expected to be a string")];
        }

        return JsonSerializer.Deserialize<string[]>(ref reader, options);
    }

    public override void Write(Utf8JsonWriter writer, string[] value, JsonSerializerOptions options) => throw new NotSupportedException();
}

public class SingleOrArrayConverter<T> : JsonConverter<T[]>
{
    public override T[]? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType == JsonTokenType.StartArray ? JsonSerializer.Deserialize<T[]>(ref reader, options) : JsonSerializer.Deserialize<T>(ref reader, options) is { } v ? [v] : [];
    }

    public override void Write(Utf8JsonWriter writer, T[] value, JsonSerializerOptions options) => throw new NotSupportedException();
}
