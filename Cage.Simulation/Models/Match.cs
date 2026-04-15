using System.Linq;

namespace Cage.Simulation.Models;

public class Match
{
    private List<Entity> _entities = new();
    private int _nextEntityId = 1;

    public IReadOnlyList<Entity> Entities => _entities.AsReadOnly();

    public Entity CreateEntity()
    {
        var entity = new Entity(_nextEntityId++);
        _entities.Add(entity);
        return entity;
    }

    public Entity? FindEntity(int id)
    {
        return _entities.FirstOrDefault(entity => entity.Id == id);
    }

    public Entity? GetLastCreatedEntity()
    {
        return _entities.LastOrDefault();
    }
}