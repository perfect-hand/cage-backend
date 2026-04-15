using System.Text.Json;
using System.Text.Json.Serialization;
using Cage.Simulation.Models;

namespace Cage.Simulation.Models.Expressions;

public class ExpressionJsonConverter : JsonConverter<Expression>
{
    public override Expression Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var document = JsonDocument.ParseValue(ref reader);
        var root = document.RootElement;
        var typeString = root.GetProperty("$type").GetString();

        return typeString switch
        {
            "FunctionExpression" => new FunctionExpression(root.GetProperty("functionName").GetString() ?? throw new JsonException("Missing functionName")),
            "VariableExpression" => new VariableExpression(root.GetProperty("name").GetString() ?? throw new JsonException("Missing name")),
            "LiteralExpression" => new LiteralExpression(JsonSerializer.Deserialize<TypedValue>(root.GetProperty("value").GetRawText(), options) ?? throw new JsonException("Missing literal value")),
            _ => throw new JsonException($"Unknown expression type: {typeString}")
        };
    }

    public override void Write(Utf8JsonWriter writer, Expression value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        switch (value)
        {
            case FunctionExpression function:
                writer.WriteString("$type", "FunctionExpression");
                writer.WriteString("functionName", function.FunctionName);
                break;
            case VariableExpression variable:
                writer.WriteString("$type", "VariableExpression");
                writer.WriteString("name", variable.Name);
                break;
            case LiteralExpression literal:
                writer.WriteString("$type", "LiteralExpression");
                writer.WritePropertyName("value");
                JsonSerializer.Serialize(writer, literal.Value, options);
                break;
            default:
                throw new JsonException($"Unsupported Expression type: {value.GetType().Name}");
        }

        writer.WriteEndObject();
    }
}
