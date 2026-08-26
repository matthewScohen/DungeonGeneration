using UnityEngine;

public interface IAStarGridHeuristic
{
    public float Heuristic(Vector2Int cell, Vector2Int goal);
}