using System.Text.Json;
using System.Text.Json.Serialization;
using Cage.Simulation.Models.Expressions;
using Cage.Simulation.Models.Types;

namespace Cage.Simulation.Models.Functions;

[JsonConverter(typeof(FunctionJsonConverter))]
public abstract class Function
{
    public abstract TypedValue Call(EvaluationContext context);

    internal abstract void WriteToJson(Utf8JsonWriter writer, JsonSerializerOptions options);

    internal abstract void ReadFromJson(JsonElement root, JsonSerializerOptions options);
}
