using System.Linq;

namespace Cage.Simulation.Models;

public class Match
{
    private readonly EntityManager _entityManager;

    public IReadOnlyList<Entity> Entities => _entityManager.Entities;
    public EntityManager EntityManager => _entityManager;

    public Match() : this(new EntityManager())
    {
    }

    public Match(EntityManager entityManager)
    {
        _entityManager = entityManager;
    }

    public Entity CreateEntity()
    {
        return _entityManager.CreateEntity();
    }

    public Entity? FindEntity(int id)
    {
        return _entityManager.FindEntity(id);
    }

    public Entity? GetLastCreatedEntity()
    {
        return _entityManager.GetLastCreatedEntity();
    }
}