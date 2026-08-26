using UnityEngine;

public class ManhattanHeuristic : IAStarGridHeuristic
{
    private readonly float MinimumMoveCost = 1f;

    public ManhattanHeuristic(float minimumMoveCost)
    {
        MinimumMoveCost = minimumMoveCost;
    }

    public float Heuristic(Vector2Int cell, Vector2Int goal)
    {
        int dx = (int)Mathf.Abs(cell.x - goal.x);
        int dy = (int)Mathf.Abs(cell.y - goal.y);

        return MinimumMoveCost * (dx + dy);
    }
}