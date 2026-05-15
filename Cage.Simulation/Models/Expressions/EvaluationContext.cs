using Cage.Simulation.Models.Types;

namespace Cage.Simulation.Models.Expressions;

public class EvaluationContext
{
    public Dictionary<string, TypedValue> Variables { get; } = new();
}
