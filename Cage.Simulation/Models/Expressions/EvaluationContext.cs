using Cage.Simulation.Models.Types;

namespace Cage.Simulation.Models.Expressions;

public class EvaluationContext
{
    public Dictionary<string, TypedValue> Variables { get; } = new();
    public Func<string, TypedValue?>? FunctionResolver { get; set; }

    public EvaluationContext()
    {
    }

    public EvaluationContext(Func<string, TypedValue?>? functionResolver)
    {
        FunctionResolver = functionResolver;
    }
}
