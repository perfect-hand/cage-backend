using Cage.Simulation.Models.Functions;
using Cage.Simulation.Models.Types;

namespace Cage.Simulation.Models.Expressions;

public sealed class FunctionExpression : Expression
{
    public Function Function { get; set; } = null!;

    public FunctionExpression() { }

    public FunctionExpression(Function function)
    {
        Function = function;
    }

    internal override TypedValue Evaluate(EvaluationContext context)
    {
        return Function.Call(context);
    }
}
