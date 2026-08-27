using UnityEngine;

public class ManhattanHeuristic : IAStarGridHeuristic
{
    private readonly float MinimumMoveCost = 1f;

    public ManhattanHeuristic(float minimumMoveCost)
    {
        MinimumMoveCost = minimumMoveCost;
    }

    public float Compute(Dungeon2D _, Vector2Int cell, Vector2Int goal)
    {
        int dx = Mathf.Abs(cell.x - goal.x);
        int dy = Mathf.Abs(cell.y - goal.y);

        return MinimumMoveCost * (dx + dy);
    }
}