using UnityEngine;

public class DiagonalDistanceHeuristic : IAStarGridHeuristic
{
    private readonly float LateralMovementCost = 1f;
    private readonly float DiagonalMovementCost = 1f;

    public DiagonalDistanceHeuristic(float lateralMovementCost, float diagonalMovementCost)
    {
        LateralMovementCost = lateralMovementCost;
        DiagonalMovementCost = diagonalMovementCost;
    }

    public float Compute(Dungeon _, Vector2Int cell, Vector2Int goal)
    {
        int dx = Mathf.Abs(cell.x - goal.x);
        int dy = Mathf.Abs(cell.y - goal.y);

        return LateralMovementCost * (dx + dy) + (DiagonalMovementCost - 2 * LateralMovementCost) + Mathf.Min(dx, dy);
    }
}