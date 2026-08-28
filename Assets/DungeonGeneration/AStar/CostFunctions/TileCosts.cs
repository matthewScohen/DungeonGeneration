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

    public float Compute(Dungeon2D dungeon, Vector2Int startingCell, Vector2Int targetCell)
    {
        return dungeon[targetCell] switch
        {
            Dungeon2DTile.Wall => EmptyTileCost,
            Dungeon2DTile.Hallway => HallwayCost,
            Dungeon2DTile.Room => RoomCost,
            _ => DefaultCost,
        };
    }
}