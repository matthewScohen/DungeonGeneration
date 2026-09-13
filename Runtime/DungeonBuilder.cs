using UnityEngine;
using System;
using System.Collections.Generic;

public class DungeonBuilder : MonoBehaviour
{
    [SerializeField] private List<DungeonTilePieceMapping> DungeonTilePieceMapping;
    [SerializeField] private DungeonObject DungeonObject;
    [SerializeField] private float TileSize = 10f;
    [SerializeField] private Vector3 DungeonScale = new(1f, 1f, 1f);
    
    private readonly Dictionary<string, DungeonPiece> DungeonPieces = new();
    private Dungeon Dungeon => DungeonObject.Dungeon ?? null;

    private void Awake()
    {
        InitializeDungeonPieces();
    }

    public void BuildDungeon()
    {
        if(Dungeon == null)
        {
            Debug.LogWarning("Dungeon is null, cannot build dungeon");
            return;
        }

        for(int x = 0; x < Dungeon.Width; x++)
        {
            for(int y = 0; y < Dungeon.Height; y++)
            {
                DungeonTile tile = Dungeon[x, y];
                if(tile == DungeonTile.Invalid || tile == DungeonTile.Empty)
                    continue;

                if(!DungeonPieces.TryGetValue(tile.TileName, out DungeonPiece prefab))
                {
                    Debug.LogWarning($"No prefab found for tile {tile}");
                    continue;
                }

                // North = Z+ axis, East = X+ axis, South = Z- axis, West = X- axis
                Vector3 position = new(x * TileSize * DungeonScale.x, 0, y * TileSize * DungeonScale.z);
                DungeonPiece instance = Instantiate(prefab, position, Quaternion.identity, transform);
                instance.transform.localScale = new Vector3(DungeonScale.x, DungeonScale.y, DungeonScale.z);

                // Set sides of prefab to open/closed depending on its adjacent tiles
                DungeonTile[] neighbors = new DungeonTile[4];
                neighbors[(int)DungeonPieceSide.North] = Dungeon[x, y + 1];
                neighbors[(int)DungeonPieceSide.East] = Dungeon[x + 1, y];
                neighbors[(int)DungeonPieceSide.South] = Dungeon[x, y - 1];
                neighbors[(int)DungeonPieceSide.West] = Dungeon[x - 1, y];
                
                foreach(DungeonPieceSide side in Enum.GetValues(typeof(DungeonPieceSide)))
                    instance.SetSideOpen(neighbors[(int)side], side);
            }
        }
    }

    private void InitializeDungeonPieces()
    {
        foreach(DungeonTilePieceMapping mapping in DungeonTilePieceMapping)
        {
            if(mapping.prefab == null && !(mapping.tileName == DungeonTile.Invalid.TileName || mapping.tileName == DungeonTile.Empty.TileName))
            {
                Debug.LogWarning($"Missing prefab for tile with name {mapping.tileName}");
                continue;
            }

            if(!DungeonPieces.ContainsKey(mapping.tileName))
            {
                Debug.Assert(mapping.prefab.GetComponent<DungeonPiece>() != null, $"{mapping.prefab} is needs DungeonPiece component to be used in DungeonBuilder");
                DungeonPieces[mapping.tileName] = mapping.prefab;
            }
            else
                Debug.LogWarning($"Found two prefabs that map to tile with tile name {mapping.tileName}");
        }
    }
}

[Serializable] public struct DungeonTilePieceMapping
{
    public string tileName;
    public DungeonPiece prefab;
}