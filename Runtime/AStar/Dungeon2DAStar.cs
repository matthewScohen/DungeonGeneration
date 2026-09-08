using UnityEngine;
using System.Collections.Generic;
using System;

public class DungeonAStar
{
    private readonly IAStarGridHeuristic Heuristic;
    private readonly IAStarMovementCost MovementCost;
    private readonly bool AllowDiagonalMovement;
    private readonly Dungeon Dungeon;

    private readonly float[] PathCosts;
    private readonly Vector2Int[] Parents;
    private PriorityQueue<Vector2Int, float> Open;

    public DungeonAStar(Dungeon dungeon, IAStarGridHeuristic heuristic, IAStarMovementCost movementCost, bool allowDiagonalMovement = false)
    {
        Dungeon = dungeon;
        Heuristic = heuristic;
        MovementCost = movementCost;
        AllowDiagonalMovement = allowDiagonalMovement;
        
        PathCosts = new float[Dungeon.Width * Dungeon.Height];
        Parents = new Vector2Int[Dungeon.Width * Dungeon.Height];
    }

    public List<Vector2Int> GeneratePath(Vector2Int startingCell, Vector2Int goalCell)
    {
        Array.Fill(PathCosts, float.PositiveInfinity);
        SetPathCost(startingCell, 0f);
        
        Vector2Int current = startingCell;
        Open = new();
        Open.Enqueue(startingCell, FScore(startingCell, goalCell));

        while(Open.Count > 0)
        {
            current = Open.Dequeue();
            if(current == goalCell)
                break;

            foreach(Vector2Int neighbor in GetNeighbors(current))
            {
                float cost = GetPathCost(current) + MovementCost.Compute(Dungeon, current, neighbor);
                if(cost < GetPathCost(neighbor))
                {
                    SetPathCost(neighbor, cost);
                    SetParent(neighbor, current);
                    if(!Open.Contains(neighbor))
                        Open.Enqueue(neighbor, FScore(neighbor, goalCell));
                }
            }
        }

        if(current != goalCell)
            return new();

        List<Vector2Int> path = new();
        while(current != startingCell)
        {
            path.Add(current);
            current = GetParent(current);
        }
        path.Add(startingCell);
        return path;
    }

    private List<Vector2Int> GetNeighbors(Vector2Int cell)
    {
        List<Vector2Int> neighbors = new();

        for(int x = -1; x <= 1; x++)
            for(int y = -1; y <= 1; y++)
            {
                bool isSelf = x == 0 && y == 0;
                bool isDiagonalNeighbor = x != 0 && y != 0;
                if(isSelf) continue;
                if(!AllowDiagonalMovement && isDiagonalNeighbor) continue;
                
                int neighborX = cell.x + x;
                int neighborY = cell.y + y;
                if(Dungeon[neighborX, neighborY] == DungeonTile.Invalid) continue;
                neighbors.Add(new(neighborX, neighborY));
            }

        return neighbors;
    }

    private float FScore(Vector2Int cell, Vector2Int goal)
    {
        return GetPathCost(cell) + Heuristic.Compute(Dungeon, cell, goal);
    }

    private float GetPathCost(Vector2Int cell)
    {
        return PathCosts[Dungeon.Index(cell)];
    }

    private void SetPathCost(Vector2Int cell, float cost)
    {
        PathCosts[Dungeon.Index(cell)] = cost;
    }

    private Vector2Int GetParent(Vector2Int cell)
    {
        return Parents[Dungeon.Index(cell)];
    }

    private void SetParent(Vector2Int child, Vector2Int newParent)
    {
        Parents[Dungeon.Index(child)] = newParent;
    }
}