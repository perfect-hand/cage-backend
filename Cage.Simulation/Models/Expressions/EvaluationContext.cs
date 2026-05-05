namespace Cage.Simulation.Models.Expressions;

public class EvaluationContext
{
    public Match Match { get; }
    public Dictionary<string, TypedValue> Variables { get; } = new();
    public Func<string, TypedValue?>? FunctionResolver { get; set; }

    public EvaluationContext(Match match)
    {
        Match = match;
    }

    public EvaluationContext(Match match, Func<string, TypedValue?>? functionResolver)
        : this(match)
    {
        FunctionResolver = functionResolver;
    }
}
