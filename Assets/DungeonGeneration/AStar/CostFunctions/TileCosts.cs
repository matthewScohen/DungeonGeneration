using UnityEngine;

public class TileCosts : IAStarMovementCost
{
    private readonly float EmptyTileCost, HallwayCost, RoomCost, DefaultCost;

    public TileCosts(float emptyTileCost, float hallwayCost, float roomCost, float defaultCost = 1)
    {
        EmptyTileCost = emptyTileCost;
        HallwayCost = hallwayCost;
        RoomCost = roomCost;
        DefaultCost = defaultCost;
    }

    // The starting cell does not matter for this type of cost function
    public float Compute(Dungeon dungeon, Vector2Int startingCell, Vector2Int targetCell)
    {
        return dungeon[targetCell] switch
        {
            DungeonTile.Empty => EmptyTileCost,
            DungeonTile.Hallway => HallwayCost,
            DungeonTile.Room => RoomCost,
            _ => DefaultCost,
        };
    }
}