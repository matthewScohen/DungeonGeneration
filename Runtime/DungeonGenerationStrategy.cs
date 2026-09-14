using UnityEngine;

public abstract class DungeonGenerationStrategy : ScriptableObject
{
    [SerializeField] protected DungeonTileSet TileSet;
    
    public abstract Dungeon Generate(int seed);

    protected DungeonTile FindTileByName(string TileName)
    {
        DungeonTile locatedTile = TileSet.FindTileByName(TileName);
        if(locatedTile == DungeonTile.Invalid)
        {
            Debug.LogWarning($"No tile with name \"{TileName}\" found in tileset");
            return DungeonTile.Invalid;
        }
        
        return locatedTile;
    }
}