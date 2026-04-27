using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cage.Simulation.Models.Mutations;

public sealed class CompositeMutation : Mutation
{
    public List<Mutation> Mutations { get; set; } = new();

    public override void Apply(Match match, EvaluationContext context)
    {
        foreach (var mutation in Mutations)
        {
            mutation.Apply(match, context);
        }
    }

    internal override void WriteToJson(Utf8JsonWriter writer, JsonSerializerOptions options)
    {
        writer.WritePropertyName("mutations");
        JsonSerializer.Serialize(writer, Mutations, options);
    }

    internal override void ReadFromJson(JsonElement root, JsonSerializerOptions options)
    {
        if (root.TryGetProperty("mutations", out var mutationsProp))
        {
            Mutations = JsonSerializer.Deserialize<List<Mutation>>(mutationsProp.GetRawText(), options)
                ?? new List<Mutation>();
        }
    }
}
