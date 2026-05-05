using System.Text.Json.Serialization;
using Cage.Simulation.Models.Types;

namespace Cage.Simulation.Models.Expressions;

[JsonConverter(typeof(ExpressionJsonConverter))]
public abstract class Expression
{
    internal abstract TypedValue Evaluate(EvaluationContext context);
}
