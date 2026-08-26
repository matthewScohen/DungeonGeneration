using UnityEngine;

public interface IAStarMovementCost
{
    public float MoveCost(Vector2Int startingCell, Vector2Int targetCell);
}