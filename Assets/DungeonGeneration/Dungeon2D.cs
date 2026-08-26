using System;
using UnityEngine;

[Serializable]
public class Dungeon2D
{
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;
    [SerializeField] [HideInInspector] private Dungeon2DTile[] tileMap;

    public int Width => width;
    public int Height => height;

    public Dungeon2D(int width, int height)
    {
        this.width = Mathf.Max(1, width);
        this.height = Mathf.Max(1, height);
        tileMap = new Dungeon2DTile[this.width * this.height];
    }

    public Dungeon2DTile this[int x, int y]
    {
        get 
        {
            int index = Index(x, y);
            if(x < 0 || x >= width || y < 0 || y >= height) return Dungeon2DTile.Invalid;
            return (index >= 0 && index < tileMap.Length) ? tileMap[index] : Dungeon2DTile.Invalid;
        }
        set 
        {
            int index = Index(x, y);

            if(!Enum.IsDefined(typeof(Dungeon2DTile), value))
                tileMap[index] = Dungeon2DTile.Invalid;
            else if(index >= 0 && index < tileMap.Length)
                tileMap[index] = value;
            else
                throw new ArgumentOutOfRangeException(nameof(value), $"Cell ({x},{y}) is out of range for dungeon {this}");
        }
    }

    public Dungeon2DTile this[Vector2Int cell]
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
}
