using Cage.Simulation.Models;

namespace Cage.Simulation.Models.Expressions;

public sealed class LiteralExpression : Expression
{
    public TypedValue Value { get; set; } = null!;

    public LiteralExpression() { }

    public LiteralExpression(TypedValue value)
    {
        Value = value;
    }

    internal override TypedValue Evaluate(EvaluationContext context) => Value;
}
