using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cage.Simulation.Models;

public class AttributeValueJsonConverter : JsonConverter<AttributeValue>
{
    public override AttributeValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        var typeString = root.GetProperty("type").GetString();
        var type = Enum.Parse<AttributeType>(typeString!);

        var valueElement = root.GetProperty("value");

        object? value = type switch
        {
            AttributeType.Int => valueElement.GetInt32(),
            AttributeType.Entity => JsonSerializer.Deserialize<Entity>(valueElement.GetRawText(), options),
            AttributeType.EntityList => JsonSerializer.Deserialize<List<Entity>>(valueElement.GetRawText(), options),
            _ => throw new JsonException($"Unknown AttributeType: {type}")
        };

        return type switch
        {
            AttributeType.Int => new AttributeValue((int)value!),
            AttributeType.Entity => new AttributeValue((Entity)value!),
            AttributeType.EntityList => new AttributeValue((List<Entity>)value!),
            _ => throw new JsonException($"Unknown AttributeType: {type}")
        };
    }

    public override void Write(Utf8JsonWriter writer, AttributeValue value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("type", value.Type.ToString());

        writer.WritePropertyName("value");
        switch (value.Type)
        {
            case AttributeType.Int:
                writer.WriteNumberValue((int)value.Value!);
                break;
            case AttributeType.Entity:
                JsonSerializer.Serialize(writer, value.Value, options);
                break;
            case AttributeType.EntityList:
                JsonSerializer.Serialize(writer, value.Value, options);
                break;
            default:
                writer.WriteNullValue();
                break;
        }
        writer.WriteEndObject();
    }
}