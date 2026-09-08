using UnityEngine;

public interface IAStarMovementCost
{
    public float Compute(Dungeon dungeon, Vector2Int startingCell, Vector2Int targetCell);
}