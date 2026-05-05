using System;
using System.Reflection;
using Cage.Simulation.Models.Expressions;

namespace Cage.Simulation.Models.Events;

public class EvaluationContextBuilder
{
    public EvaluationContext Build(Match match, GameEvent e)
    {
        var context = new EvaluationContext(match);
        PopulateEventProperties(e, context);
        return context;
    }

    private void PopulateEventProperties(GameEvent e, EvaluationContext context)
    {
        var eventType = e.GetType();
        var properties = eventType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            if (!property.CanRead)
                continue;

            var attribute = property.GetCustomAttribute<EventPropertyAttribute>();
            if (attribute is null)
                continue;

            var value = property.GetValue(e);
            if (value is null)
                continue;

            var typedValue = ConvertToTypedValue(value, attribute.Type, context);

            if (typedValue is not null)
            {
                var propertyName = attribute.PropertyName ?? property.Name;
                var variableName = $"event{propertyName}";
                context.Variables[variableName] = typedValue;
            }
        }
    }

    private TypedValue? ConvertToTypedValue(object value, CageType explicitType, EvaluationContext context)
    {
        return explicitType switch
        {
            CageType.Int when value is int intVal => new TypedValue(intVal),
            CageType.String when value is string strVal => new TypedValue(strVal),
            CageType.Entity when value is int entityIdVal => ResolveEntity(entityIdVal, context),
            CageType.Entity when value is Entity entityVal => new TypedValue(entityVal),
            CageType.EntityList when value is List<Entity> listVal => new TypedValue(listVal),
            _ => null
        };
    }

    private TypedValue? ResolveEntity(int entityId, EvaluationContext context)
    {
        var entity = context.Match.FindEntity(entityId);
        if (entity is not null)
        {
            return new TypedValue(entity);
        }
        return null;
    }
}