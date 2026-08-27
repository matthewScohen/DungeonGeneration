using UnityEngine;

public interface IAStarGridHeuristic
{
    public float Compute(Dungeon2D dungeon, Vector2Int cell, Vector2Int goal);
}