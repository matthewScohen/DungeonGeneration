using UnityEngine;

public interface IAStarGridHeuristic
{
    public float Compute(Dungeon dungeon, Vector2Int cell, Vector2Int goal);
}