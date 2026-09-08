using UnityEngine;

public abstract class DungeonGenerationStrategy : ScriptableObject
{
    public abstract Dungeon Generate(int seed);
}