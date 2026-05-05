
using Cage.Simulation.Models;
using Cage.Simulation.Models.Entities;
using Cage.Simulation.Models.Types;

namespace Cage.Simulation.Tests;

public class MatchTests
{
    [Fact]
    public void SimulateWholeMatch()
    {
        // Setup data.
        var resourceCard = new EntityBlueprint("Resource");
        resourceCard.Tags.Add("Resource");

        var monsterCard = new EntityBlueprint("Monster");
        monsterCard.Tags.Add("Monster");
        monsterCard.Attributes["Costs"] = new TypedValue(1);
        monsterCard.Attributes["Power"] = new TypedValue(1);

        var blueprintRegistry = new EntityBlueprintRegistry();
        blueprintRegistry.Register(resourceCard);
        blueprintRegistry.Register(monsterCard);

        var entityManager = new EntityManager(blueprintRegistry);

        // Create match players.
        var match = new Match(entityManager);

        var deckCardIds = new List<string> { "Resource", "Resource", "Resource", "Resource", "Monster", "Monster", "Monster", "Monster" };

        var player1 = match.EntityManager.CreateEntity();
        var player1Deck = entityManager.CreateEntitiesFromBlueprintIds(deckCardIds);
        player1.Attributes["Deck"] = new TypedValue(player1Deck.ToList());

        var player2 = match.EntityManager.CreateEntity();
        var player2Deck = entityManager.CreateEntitiesFromBlueprintIds(deckCardIds);
        player2.Attributes["Deck"] = new TypedValue(player2Deck.ToList());

        // TODO: Add fuction for getting first entity in a list.
        // TODO: Add event for match initialization.
        // TODO: Add trigger listening for match initialization.
        // TODO: Shuffle decks, draw hands, set initial life.
        // TODO: Add trigger listening for health changes, and raising victory and defeat events.
    }
}