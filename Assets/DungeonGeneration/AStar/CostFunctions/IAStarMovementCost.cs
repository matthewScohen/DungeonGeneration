using UnityEngine;

public interface IAStarMovementCost
{
    public float Compute(Dungeon2D dungeon, Vector2Int startingCell, Vector2Int targetCell);
}