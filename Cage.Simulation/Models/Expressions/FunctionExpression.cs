using Cage.Simulation.Models.Types;

namespace Cage.Simulation.Models.Expressions;

public sealed class FunctionExpression : Expression
{
    public string FunctionName { get; set; } = null!;

    public FunctionExpression() { }

    public FunctionExpression(string functionName)
    {
        FunctionName = functionName;
    }

    internal override TypedValue Evaluate(EvaluationContext context)
    {
        if (context.FunctionResolver is null)
            throw new InvalidOperationException("No function resolver is available for function expression evaluation.");

        var result = context.FunctionResolver(FunctionName);
        return result ?? throw new InvalidOperationException($"Function '{FunctionName}' returned no value.");
    }
}
