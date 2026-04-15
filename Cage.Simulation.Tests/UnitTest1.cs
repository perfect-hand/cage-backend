using System.Text.Json;
using Cage.Simulation.Models;
using Cage.Simulation.Models.Expressions;
using Cage.Simulation.Models.Mutations;

namespace Cage.Simulation.Tests;

public class UnitTest1
{
    [Fact]
    public void SetIntegerAttributeMutation_IsJsonSerializableAndAppliesToMatch()
    {
        var match = new Match();
        var entity = match.CreateEntity();
        entity.Attributes["Health"] = new TypedValue(10);

        var mutation = new SetIntegerAttributeMutation
        {
            Entity = new VariableExpression("target"),
            Attribute = "Health",
            NewValue = new LiteralExpression(new TypedValue(42))
        };

        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        var json = JsonSerializer.Serialize<Mutation>(mutation, options);
        var deserialized = JsonSerializer.Deserialize<Mutation>(json, options) as SetIntegerAttributeMutation;

        Assert.NotNull(deserialized);

        var context = new EvaluationContext();
        context.Variables["target"] = new TypedValue(entity);

        var deserializedMutation = deserialized!;
        deserializedMutation.Apply(match, context);
        Assert.Equal(42, entity.Attributes["Health"].AsInt());
    }

    [Fact]
    public void Mutation_JsonFormatIsClean()
    {
        var mutation = new SetIntegerAttributeMutation
        {
            Entity = new FunctionExpression("LastCreatedEntity"),
            Attribute = "Damage",
            NewValue = new VariableExpression("damageAmount")
        };

        var options = new JsonSerializerOptions { WriteIndented = true };
        var json = JsonSerializer.Serialize<Mutation>(mutation, options);

        // Verify it contains the $type discriminator
        Assert.Contains("$type", json);
        Assert.Contains("SetIntegerAttributeMutation", json);
        
        // Verify it can be round-tripped
        var deserialized = JsonSerializer.Deserialize<Mutation>(json, options);
        Assert.NotNull(deserialized);
        Assert.IsType<SetIntegerAttributeMutation>(deserialized);
    }
}
