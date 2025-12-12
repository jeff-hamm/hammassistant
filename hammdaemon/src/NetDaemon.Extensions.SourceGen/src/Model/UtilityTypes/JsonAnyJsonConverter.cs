using NetDaemon.Extensions.SourceGen.Model;
using System.Text.Json;
using System.Text.Json.Serialization;
using static NetDaemon.Extensions.SourceGen.Model.JsonAny;
using DateTime = System.DateTime;

namespace NetDaemon.Extensions.SourceGen.src.Model.UtilityTypes;

public class JsonAnyJsonConverter : JsonConverter<JsonAny?>
{
    public override JsonAny? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        try
        {
            // Attempt to deserialize to a known type
            if (reader.TokenType == JsonTokenType.Number && reader.TryGetInt32(out int intValue))
                return new Int(intValue);
            if (reader.TokenType == JsonTokenType.Number && reader.TryGetDouble(out double doubleValue))
                return new JsonAny.Double(doubleValue);
            if (reader.TokenType == JsonTokenType.String && reader.TryGetDateTime(out DateTime dateTimeValue))
                return new JsonAny.DateTime(dateTimeValue);
            if (reader.TokenType == JsonTokenType.String)
            {
                var stringValue = reader.GetString();
                if (stringValue == null) return new Null();
                return new JsonAny.String(stringValue);
            }
            if (reader.TokenType == JsonTokenType.True || reader.TokenType == JsonTokenType.False)
                return new Bool(reader.GetBoolean());
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                var list = new List<JsonAny>();
                Type? valueType = null;
                bool allSameType = false;
                while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                {
                    if (Read(ref reader, typeof(JsonAny), options) is not { } v) continue;
                    valueType ??= v.GetType();
                    if (valueType != v.GetType())
                        allSameType = false;
                    list.Add(v);
                }
                return new JsonArray(list, allSameType ? valueType : null);
            }
            if (reader.TokenType == JsonTokenType.StartObject)
            {
                var dictionary = new Dictionary<string, JsonAny>();
                Type? valueType = null;
                bool allSameType = false;
                while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
                {
                    if (reader.TokenType != JsonTokenType.PropertyName) continue;
                    if (reader.GetString() is not { } propertyName || !reader.Read()) continue;
                    if (Read(ref reader, typeof(JsonAny), options) is not { } v) continue;
                    valueType ??= v.GetType();
                    if (valueType != v.GetType())
                        allSameType = false;
                    dictionary[propertyName] = v;
                }
                return new JsonObject(dictionary, allSameType ? valueType : null);
            }

            // Fallback to JsonElement
            JsonElement element = JsonDocument.ParseValue(ref reader).RootElement;
            return new Element(element);
        }
        catch
        {
            // Fallback to JsonElement in case of any error
            JsonElement element = JsonDocument.ParseValue(ref reader).RootElement;
            return new Element(element);
        }
    }

    public override void Write(Utf8JsonWriter writer, JsonAny? value, JsonSerializerOptions options)
    {
        if (value == null)
        {
            writer.WriteNullValue();
            return;
        }
        switch (value)
        {
            case Int intValue:
                writer.WriteNumberValue(intValue.Value);
                break;
            case JsonAny.Double doubleValue:
                writer.WriteNumberValue(doubleValue.Value);
                break;
            case JsonAny.String stringValue:
                writer.WriteStringValue(stringValue.Value);
                break;
            case Bool boolValue:
                writer.WriteBooleanValue(boolValue.Value);
                break;
            case JsonAny.DateTime dateTimeValue:
                writer.WriteStringValue(dateTimeValue.Value);
                break;
            case JsonArray array:
                writer.WriteStartArray();
                foreach (var item in array.Value)
                    Write(writer, item, options);
                writer.WriteEndArray();
                break;
            case JsonObject obj:
                writer.WriteStartObject();
                foreach (var (key, item) in obj.Value)
                {
                    writer.WritePropertyName(key);
                    Write(writer, item, options);
                }
                writer.WriteEndObject();
                break;
            case Element elementValue:
                elementValue.Value.WriteTo(writer);
                break;
            default:
                JsonSerializer.Serialize(writer, value.ToObject(), options);
                break;
        }
    }
}
