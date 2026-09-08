using UnityEngine;
using System;
using System.Collections.Generic;

public class DungeonBuilder : MonoBehaviour
{
    [SerializeField] private List<DungeonTilePieceMapping> DungeonTilePieceMapping;
    [SerializeField] private DungeonObject DungeonObject;
    [SerializeField] private float TileSize = 10f;
    
    private readonly Dictionary<DungeonTile, DungeonPiece> DungeonPieces = new();
    private Dungeon Dungeon => DungeonObject.Dungeon ?? null;

    private void Awake()
    {
        InitializeDungeonPieces();
    }

    private void Start()
    {
        BuildDungeon();
    }

    private void BuildDungeon()
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

                if(!DungeonPieces.TryGetValue(tile, out DungeonPiece prefab))
                {
                    Debug.LogWarning($"No prefab found for tile {tile}");
                    continue;
                }

                // North = Z+ axis, East = X+ axis, South = Z- axis, West = X- axis
                Vector3 position = new(x * TileSize, 0, y * TileSize);
                DungeonPiece instance = Instantiate(prefab, position, Quaternion.identity, transform);

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
            if(mapping.prefab == null && !(mapping.tile == DungeonTile.Invalid || mapping.tile == DungeonTile.Empty))
            {
                Debug.LogWarning($"Missing prefab for tile {mapping.tile}");
                continue;
            }

            if(!DungeonPieces.ContainsKey(mapping.tile))
            {
                Debug.Assert(mapping.prefab.GetComponent<DungeonPiece>() != null, $"{mapping.prefab} is needs DungeonPiece component to be used in DungeonBuilder");
                DungeonPieces[mapping.tile] = mapping.prefab;
            }
            else
                Debug.LogWarning($"Found two prefabs that map to tile {mapping.tile}");
        }
    }
}

[Serializable] public struct DungeonTilePieceMapping
{
    public DungeonTile tile;
    public DungeonPiece prefab;
}