namespace Cage.Simulation.Models.Events;

public abstract class GameEvent
{
    public string Name => GetType().Name;
}
