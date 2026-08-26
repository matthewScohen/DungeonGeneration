using UnityEngine;
using System.Collections.Generic;

public class Dungeon2DAStar
{
    private readonly IAStarGridHeuristic Heuristic;
    private readonly IAStarMovementCost MovementCost;
    private Dungeon2D Dungeon;

    private float[] PathCosts;
    private Vector2Int[] Parents;
    private PriorityQueue<Vector2Int, float> Open;

    public Dungeon2DAStar(Dungeon2D dungeon, IAStarGridHeuristic heuristic, IAStarMovementCost movementCost)
    {
        Heuristic = heuristic;
        MovementCost = movementCost;
        Dungeon = dungeon;
        PathCosts = new float[Dungeon.Width * Dungeon.Height];
    }

    public void GeneratePath(Vector2Int startingCell, Vector2Int goalCell)
    {

    }

    private Vector2Int[] GetNeighbors(Vector2Int cell)
    {
        Vector2Int[] neighbors = new Vector2Int[8];

        int neighborCount = 0;
        for(int x = -1; x <= 1; x++)
            for(int y = -1; y <= 1; y++)
                neighbors[neighborCount] = new(cell.x + x, cell.y + y);

        return neighbors;
    }

    private float GetPathCost(Vector2Int cell)
    {
        return PathCosts[Dungeon.Index(cell)];
    }

    private Vector2Int GetParent(Vector2Int cell)
    {
        return Parents[Dungeon.Index(cell)];
    }
}