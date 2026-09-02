using UnityEngine;

public class TileCosts : IAStarMovementCost
{
    private readonly float WallTileCost, HallwayCost, RoomCost, DefaultCost;

    public TileCosts(float wallTileCost, float hallwayCost, float roomCost, float defaultCost = 1)
    {
        WallTileCost = wallTileCost;
        HallwayCost = hallwayCost;
        RoomCost = roomCost;
        DefaultCost = defaultCost;
    }

    // The starting cell does not matter for this type of cost function
    public float Compute(Dungeon dungeon, Vector2Int startingCell, Vector2Int targetCell)
    {
        return dungeon[targetCell] switch
        {
            DungeonTile.Wall => WallTileCost,
            DungeonTile.Hallway => HallwayCost,
            DungeonTile.Room => RoomCost,
            _ => DefaultCost,
        };
    }
}