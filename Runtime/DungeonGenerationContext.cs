using UnityEngine;

public class DungeonGenerationContext
{
    private readonly Dungeon Dungeon;

    public int Width => Dungeon.Width;
    public int Height => Dungeon.Height;

    public DungeonGenerationContext(Dungeon dungeon, int seed)
    {
        Dungeon = dungeon;
        Random.InitState(seed);
    }

    public void SetAreaToTile(DungeonTile tile, int x, int y, int width, int height)
    {
        for(int i = x; i < x + width; i++)
            for(int j = y; j < y + height; j++)
                Dungeon[i, j] = tile;
    }

    public bool AreaContainsTile(DungeonTile tile, int x, int y, int width, int height)
    {
        for(int i = x; i < x + width; i++)
            for(int j = y; j < y + height; j++)
                if(Dungeon[i, j] == tile)
                    return true;

        return false;
    }

    public bool AreaContainsOnly(DungeonTile tile, int x, int y, int width, int height)
    {
        for(int i = x; i < x + width; i++)
            for(int j = y; j < y + height; j++)
                if(Dungeon[i, j] != tile)
                    return false;

        return true;
    }
}