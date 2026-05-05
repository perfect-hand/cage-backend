using System.Linq;
using Cage.Simulation.Models.Entities;

namespace Cage.Simulation.Models;

public class Match
{
    private readonly EntityManager entityManager;

    public EntityManager EntityManager => entityManager;

    public Match() : this(new EntityManager())
    {
    }

    public Match(EntityManager entityManager)
    {
        this.entityManager = entityManager;
    }
}