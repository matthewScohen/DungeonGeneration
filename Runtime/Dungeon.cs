using System;
using UnityEngine;

[Serializable]
public class Dungeon
{
    public int Width => width;
    public int Height => height;

    [SerializeField] [HideInInspector] private DungeonTile[] tileMap;
    private int width = 10;
    private int height = 10;

    public Dungeon(int width, int height)
    {
        this.width = Mathf.Max(1, width);
        this.height = Mathf.Max(1, height);
        tileMap = new DungeonTile[this.width * this.height];
        ResetTileMap();
    }

    public DungeonTile this[int x, int y]
    {
        get 
        {
            int index = Index(x, y);
            if(x < 0 || x >= width || y < 0 || y >= height) return DungeonTile.Invalid;
            return (index >= 0 && index < tileMap.Length) ? tileMap[index] : DungeonTile.Invalid;
        }
        set 
        {
            int index = Index(x, y);

            if(index >= 0 && index < tileMap.Length)
                tileMap[index] = value;
            else
                throw new ArgumentOutOfRangeException(nameof(value), $"Cell ({x},{y}) is out of range for dungeon {this}");
        }
    }

    public DungeonTile this[Vector2Int cell]
    {
        get { return this[cell.x, cell.y]; }
        set { this[cell.x, cell.y] = value; }
    }

    public int Index(int x, int y)
    {
        return y * width + x;
    }

    public int Index(Vector2Int cell)
    {
        return Index(cell.x, cell.y);
    }

    private void ResetTileMap()
    {
        for(int i = 0; i < width * height; i++)
            tileMap[i] = DungeonTile.Empty;
    }
}
