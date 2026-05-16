using System.Collections.Generic;
using System.Text.Json;
using Cage.Simulation.Models;
using Cage.Simulation.Models.Entities;
using Cage.Simulation.Models.Expressions;
using Cage.Simulation.Models.Functions;
using Cage.Simulation.Models.Mutations;
using Cage.Simulation.Models.Types;

namespace Cage.Simulation.Tests;

public class UnitTest1
{
    [Fact]
    public void SetIntegerAttributeMutation_IsJsonSerializableAndAppliesToMatch()
    {
        var match = new Match();
        var entity = match.EntityManager.CreateEntity();
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

        var context = new EvaluationContext(match.EntityManager);
        context.Variables["target"] = new TypedValue(entity);

        var deserializedMutation = deserialized!;
        deserializedMutation.Apply(context);
        Assert.Equal(42, entity.Attributes["Health"].AsInt());
    }

    [Fact]
    public void Mutation_JsonFormatIsClean()
    {
        var entity = new Entity(1);
        var list = new List<Entity> { entity };

        var mutation = new SetIntegerAttributeMutation
        {
            Entity = new FunctionExpression(new EntityAtIndexFunction(
                new LiteralExpression(new TypedValue(list)),
                new LiteralExpression(new TypedValue(0))
            )),
            Attribute = "Damage",
            NewValue = new LiteralExpression(new TypedValue(5))
        };

        var options = new JsonSerializerOptions { WriteIndented = true };
        var json = JsonSerializer.Serialize<Mutation>(mutation, options);

        // Verify it contains the $type discriminator
        Assert.Contains("$type", json);
        Assert.Contains("SetIntegerAttributeMutation", json);
        Assert.Contains("FunctionExpression", json);
        Assert.Contains("EntityAtIndexFunction", json);
        
        // Verify it can be round-tripped
        var deserialized = JsonSerializer.Deserialize<Mutation>(json, options) as SetIntegerAttributeMutation;
        Assert.NotNull(deserialized);

        var context = new EvaluationContext(new EntityManager());
        deserialized!.Apply(context);
        Assert.Equal(5, entity.Attributes["Damage"].AsInt());
    }

    [Fact]
    public void EntityAttributeValueFunction_CanEvaluateAndJsonRoundTrip()
    {
        var entity = new Entity(1);
        entity.Attributes["Health"] = new TypedValue(77);

        var functionExpression = new FunctionExpression(new EntityAttributeValueFunction(
            new LiteralExpression(new TypedValue(entity)),
            new LiteralExpression(new TypedValue("Health"))
        ));

        var options = new JsonSerializerOptions { WriteIndented = true };
        var json = JsonSerializer.Serialize<Expression>(functionExpression, options);

        Assert.Contains("$type", json);
        Assert.Contains("FunctionExpression", json);
        Assert.Contains("EntityAttributeValueFunction", json);

        var deserializedExpression = JsonSerializer.Deserialize<Expression>(json, options) as FunctionExpression;
        Assert.NotNull(deserializedExpression);

        var result = deserializedExpression!.Evaluate(new EvaluationContext(new EntityManager()));
        Assert.Equal(77, result.AsInt());
    }
}

