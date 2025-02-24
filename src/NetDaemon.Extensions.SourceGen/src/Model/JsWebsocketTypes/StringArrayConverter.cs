using System.Text.Json;
using System.Text.Json.Serialization;

namespace NetDaemon.Extensions.SourceGen.Model.JsWebsocket;

public class StringArrayConverter : JsonConverter<StringArray>
{
    public override StringArray Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            return reader.GetString();
        }
        else if (reader.TokenType == JsonTokenType.StartArray)
        {
            var list = new List<string>();
            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
            {
                if (reader.TokenType == JsonTokenType.String)
                {
                    list.Add(reader.GetString());
                }
            }
            return list.ToArray();
        }
        throw new JsonException("Unexpected token type");
    }

    public override void Write(Utf8JsonWriter writer, StringArray value, JsonSerializerOptions options)
    {
        value.Match(s =>
            {
                writer.WriteStringValue((string?)s.Value);
            },
            m =>
            {
                writer.WriteStartArray();
                foreach (var item in m.Values)
                {
                    writer.WriteStringValue((string?)item);
                }
                writer.WriteEndArray();

            });
    }
}