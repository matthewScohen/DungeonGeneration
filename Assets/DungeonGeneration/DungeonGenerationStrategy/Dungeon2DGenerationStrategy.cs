using UnityEngine;

public abstract class Dungeon2DGenerationStrategy : ScriptableObject
{
    public abstract Dungeon2D Generate(int seed);
}