using UnityEngine;

public class DungeonGenerationContext
{
    private readonly Dungeon2D Dungeon;

    public int Width => Dungeon.Width;
    public int Height => Dungeon.Height;

    public DungeonGenerationContext(Dungeon2D dungeon, int seed)
    {
        Dungeon = dungeon;
        Random.InitState(seed);
    }

    public void SetAreaToTile(Dungeon2DTile tile, int x, int y, int width, int height)
    {
        for(int i = x; i < x + width; i++)
            for(int j = y; j < y + height; j++)
                Dungeon[i, j] = tile;
    }

    public bool AreaContainsTile(Dungeon2DTile tile, int x, int y, int width, int height)
    {
        for(int i = x; i < x + width; i++)
            for(int j = y; j < y + height; j++)
                if(Dungeon[i, j] == tile)
                    return true;

        return false;
    }

    public bool AreaContainsOnly(Dungeon2DTile tile, int x, int y, int width, int height)
    {
        for(int i = x; i < x + width; i++)
            for(int j = y; j < y + height; j++)
                if(Dungeon[i, j] != tile)
                    return false;

        return true;
    }
}