using Cage.Simulation.Models.Types;
using Cage.Simulation.Models.Entities;

namespace Cage.Simulation.Models.Expressions;

public class EvaluationContext
{
    public Dictionary<string, TypedValue> Variables { get; } = new();
    public EntityManager EntityManager { get; }

    public EvaluationContext(EntityManager entityManager)
    {
        EntityManager = entityManager;
    }
}
