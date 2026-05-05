using System.Collections.Generic;
using System.Linq;

namespace Cage.Simulation.Models.Events;

public class EventManager
{
    public IReadOnlyList<Trigger> HandleEvent(Match match, IEnumerable<Trigger> triggers, GameEvent e)
    {
        var executed = new List<Trigger>();
        var contextBuilder = new EvaluationContextBuilder();
        var context = contextBuilder.Build(match, e);

        foreach (var trigger in triggers)
        {
            if (!trigger.MatchesEvent(e))
                continue;

            if (!trigger.AreAllConditionsFulfilled(context))
                continue;

            trigger.ExecuteActions(context);
            executed.Add(trigger);
        }

        return executed;
    }
}
