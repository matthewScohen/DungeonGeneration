using UnityEngine;

public class RandomizedCostFunction : IAStarMovementCost
{
    TileCosts BaseCost;
    float MaxAddedCost;

    public RandomizedCostFunction(TileCosts baseCosts, float maxAddedCost)
    {
        BaseCost = baseCosts;
        MaxAddedCost = maxAddedCost;
    }

    public float Compute(Dungeon2D dungeon, Vector2Int startingCell, Vector2Int targetCell)
    {
        float baseCost = BaseCost.Compute(dungeon, startingCell, targetCell);
        return baseCost + Random.Range(0f, MaxAddedCost);
    }
}