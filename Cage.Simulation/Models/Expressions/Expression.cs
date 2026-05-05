using System.Text.Json.Serialization;

namespace Cage.Simulation.Models.Expressions;

[JsonConverter(typeof(ExpressionJsonConverter))]
public abstract class Expression
{
    internal abstract TypedValue Evaluate(EvaluationContext context);
}
