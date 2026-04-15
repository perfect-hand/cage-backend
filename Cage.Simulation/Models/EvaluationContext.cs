namespace Cage.Simulation.Models;

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
