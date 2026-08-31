using UnityEngine;
using System;
using System.IO.Hashing;

public class RandomizedCostFunction : IAStarMovementCost
{
    readonly private TileCosts BaseCost;
    readonly private float MaxAddedCost;

    public RandomizedCostFunction(TileCosts baseCosts, float maxAddedCost)
    {
        BaseCost = baseCosts;
        MaxAddedCost = maxAddedCost;
    }

    public float Compute(Dungeon2D dungeon, Vector2Int startingCell, Vector2Int targetCell)
    {
        Span<byte> buffer = stackalloc byte[8];
        BitConverter.TryWriteBytes(buffer.Slice(0, 4), targetCell.x);
        BitConverter.TryWriteBytes(buffer.Slice(4, 4), targetCell.y);

        // Use a hash so any given cell will map to the same random value each time Compute is called
        double hash = XxHash3.HashToUInt64(buffer);
        double randomPercentage = hash / ulong.MaxValue;

        float baseCost = BaseCost.Compute(dungeon, startingCell, targetCell);
        return baseCost + (float)randomPercentage * MaxAddedCost;
    }
}