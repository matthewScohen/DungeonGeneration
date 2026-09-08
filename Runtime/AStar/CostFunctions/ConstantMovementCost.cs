using UnityEngine;

public class ConstantMovementCost : IAStarMovementCost
{
    private readonly float Cost;

    public ConstantMovementCost(float cost)
    {
        Cost = cost;
    }

    public float Compute(Dungeon _, Vector2Int startingCell, Vector2Int targetCell)
    {
        return Cost;
    }
}