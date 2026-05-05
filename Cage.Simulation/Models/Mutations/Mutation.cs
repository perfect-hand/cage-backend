using System.Text.Json;
using System.Text.Json.Serialization;
using Cage.Simulation.Models.Expressions;

namespace Cage.Simulation.Models.Mutations;

[JsonConverter(typeof(MutationJsonConverter))]
public abstract class Mutation
{
    public abstract void Apply(Match match, EvaluationContext context);

    internal abstract void WriteToJson(Utf8JsonWriter writer, JsonSerializerOptions options);

    internal abstract void ReadFromJson(JsonElement root, JsonSerializerOptions options);
}
