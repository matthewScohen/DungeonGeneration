using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Dungeon/Dungeon Tile Set")]
[Serializable]
public class DungeonTileSet : ScriptableObject
{
    [SerializeField] List<DungeonTile> Tiles = new();

    public DungeonTile FindTileByName(string Name)
    {
        foreach(DungeonTile tile in Tiles)
            if(tile.TileName == Name)
                return tile;

        return DungeonTile.Invalid;
    }

    private void OnEnable()
    {
        if(Tiles.Count < 2)
        {
            Tiles.Add(DungeonTile.Invalid);
            Tiles.Add(DungeonTile.Empty);
        }
    }
}

[Serializable]
public struct DungeonTile : IEquatable<DungeonTile>
{
    public static DungeonTile Invalid => new(-1, "Invalid", Color.magenta);
    public static DungeonTile Empty => new(0, "Empty", new Color(0.15f, 0.15f, 0.15f));
    
    public int TileId;
    public string TileName;
    public Color TileColor;

    public DungeonTile(int tileId, string tileName, Color tileColor)
    {
        TileId = tileId;
        TileName = tileName;
        TileColor = tileColor;
    }

    public static bool operator ==(DungeonTile left, DungeonTile right) => left.Equals(right);

    public static bool operator !=(DungeonTile left, DungeonTile right) => !left.Equals(right);

    public readonly bool Equals(DungeonTile other) => TileId == other.TileId;

    public readonly override bool Equals(object obj) => obj is DungeonTile other && Equals(other);

    public override readonly int GetHashCode() => HashCode.Combine(TileId);
}