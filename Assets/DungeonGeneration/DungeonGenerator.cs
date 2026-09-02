using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Dungeon/Dungeon 2D")]
public class DungeonGenerator : ScriptableObject
{
    [SerializeField] private int seed = 42;
    [SerializeField] private DungeonGenerationStrategy GenerationStrategy;

    [SerializeField] private DungeonObject DungeonObject;
    private Dungeon Dungeon;

    public DungeonObject SavedDungeonObject => DungeonObject;

    public int Width => Dungeon != null ? Dungeon.Width : 0;
    public int Height => Dungeon != null ? Dungeon.Height : 0;
    public bool CanGenerate => GenerationStrategy != null;
    public event Action Generated;

    public DungeonTile this[int x, int y]
    {
        get => Dungeon[x, y];
        set => Dungeon[x, y] = value;
    }

    public bool Generate()
    {
        if (GenerationStrategy == null)
            return false;

        Dungeon generatedDungeon = GenerationStrategy.Generate(seed);
        if (generatedDungeon == null)
            return false;

        if (DungeonObject == null)
            DungeonObject = CreateInstance<DungeonObject>();

        Dungeon = generatedDungeon;
        DungeonObject.Dungeon = Dungeon;
        Generated?.Invoke();
        return true;
    }

    private void OnValidate()
    {
        Dungeon ??= new Dungeon(10, 10);
    }
}