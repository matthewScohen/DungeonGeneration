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
    public float Compute(Dungeon2D dungeon, Vector2Int startingCell, Vector2Int targetCell)
    {
        return dungeon[targetCell] switch
        {
            Dungeon2DTile.Wall => WallTileCost,
            Dungeon2DTile.Hallway => HallwayCost,
            Dungeon2DTile.Room => RoomCost,
            _ => DefaultCost,
        };
    }
}