using System.Collections.Generic;
using Cage.Simulation.Models;
using Cage.Simulation.Models.Events;
using Cage.Simulation.Models.Expressions;
using Cage.Simulation.Models.Mutations;
using Cage.Simulation.Models.Types;

namespace Cage.Simulation.Tests;

public class EventManagerTests
{
    [Fact]
    public void IntegerAttributeChangedEvent_FiresTriggerAndAppliesAction()
    {
        var match = new Match();
        var entity = match.EntityManager.CreateEntity();
        entity.Attributes["Health"] = new TypedValue(1);
        entity.Attributes["Knockout"] = new TypedValue(0);

        var trigger = new Trigger(typeof(IntegerAttributeChangedEvent))
        {
            Conditions = new List<ExpressionCondition>
            {
                new ExpressionCondition
                {
                    Left = new VariableExpression("eventAttribute"),
                    Operator = ComparisonOperator.Equal,
                    Right = new LiteralExpression(new TypedValue("Health"))
                },
                new ExpressionCondition
                {
                    Left = new VariableExpression("eventNewValue"),
                    Operator = ComparisonOperator.LessThanOrEqual,
                    Right = new LiteralExpression(new TypedValue(0))
                }
            },
            Actions = new List<Mutation>
            {
                new CompositeMutation
                {
                    Mutations = new List<Mutation>
                    {
                        new SetIntegerAttributeMutation
                        {
                            Entity = new VariableExpression("eventEntity"),
                            Attribute = "Knockout",
                            NewValue = new LiteralExpression(new TypedValue(1))
                        }
                    }
                }
            }
        };

        var e = new IntegerAttributeChangedEvent
        {
            EntityId = entity.Id,
            Attribute = "Health",
            OldValue = 1,
            NewValue = 0
        };

        var eventManager = new EventManager();
        var executed = eventManager.HandleEvent(match, new[] { trigger }, e);

        Assert.Single(executed);
        Assert.Equal(1, entity.Attributes["Knockout"].AsInt());
    }

    [Fact]
    public void IntegerAttributeChangedEvent_DoesNotFireWhenConditionFails()
    {
        var match = new Match();
        var entity = match.EntityManager.CreateEntity();
        entity.Attributes["Health"] = new TypedValue(10);
        entity.Attributes["Knockout"] = new TypedValue(0);

        var trigger = new Trigger(typeof(IntegerAttributeChangedEvent))
        {
            Conditions = new List<ExpressionCondition>
            {
                new ExpressionCondition
                {
                    Left = new VariableExpression("eventAttribute"),
                    Operator = ComparisonOperator.Equal,
                    Right = new LiteralExpression(new TypedValue("Health"))
                },
                new ExpressionCondition
                {
                    Left = new VariableExpression("eventNewValue"),
                    Operator = ComparisonOperator.LessThanOrEqual,
                    Right = new LiteralExpression(new TypedValue(0))
                }
            },
            Actions = new List<Mutation>
            {
                new SetIntegerAttributeMutation
                {
                    Entity = new VariableExpression("entity"),
                    Attribute = "Knockout",
                    NewValue = new LiteralExpression(new TypedValue(1))
                }
            }
        };

        var e = new IntegerAttributeChangedEvent
        {
            EntityId = entity.Id,
            Attribute = "Health",
            OldValue = 10,
            NewValue = 5
        };

        var eventManager = new EventManager();
        var executed = eventManager.HandleEvent(match, new[] { trigger }, e);

        Assert.Empty(executed);
        Assert.Equal(0, entity.Attributes["Knockout"].AsInt());
    }
}
