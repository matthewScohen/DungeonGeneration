using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Dungeon/Dungeon 2D")]
public class Dungeon2DGenerator : ScriptableObject
{
    [SerializeField] private int seed = 42;
    [SerializeField] private Dungeon2D Dungeon = new(10, 10);
    [SerializeField] private Dungeon2DGenerationStrategy GenerationStrategy;

    public int Width => Dungeon != null ? Dungeon.Width : 0;
    public int Height => Dungeon != null ? Dungeon.Height : 0;
    public bool CanGenerate => GenerationStrategy != null;
    public event Action Generated;

    public Dungeon2DTile this[int x, int y]
    {
        get => Dungeon[x, y];
        set => Dungeon[x, y] = value;
    }

    public bool Generate()
    {
        if (GenerationStrategy == null)
            return false;

        Dungeon2D generatedDungeon = GenerationStrategy.Generate(seed);
        if (generatedDungeon == null)
            return false;

        Dungeon = generatedDungeon;
        Generated?.Invoke();
        return true;
    }

    private void OnValidate()
    {
        Dungeon ??= new Dungeon2D(10, 10);
    }
}