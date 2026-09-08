using UnityEngine;
using System;

// When making a DungeonPiece prefab the North side should coorespond to the Z+ axis
public class DungeonPiece : MonoBehaviour
{
    [SerializeField] private SideToGameObjectMapping[] SideToObjectMapping;

    private void Reset()
    {
        // Set default side values for inspector
        DungeonPieceSide[] sideValues = (DungeonPieceSide[])Enum.GetValues(typeof(DungeonPieceSide));
        SideToObjectMapping = new SideToGameObjectMapping[sideValues.Length];

        for (int i = 0; i < sideValues.Length; i++)
        {
            SideToObjectMapping[i] = new SideToGameObjectMapping
            {
                side = sideValues[i],
                gameObject = null
            };
        }
    }

    public void SetSideOpen(DungeonTile neighbor, DungeonPieceSide side)
    {
        SideToObjectMapping[(int)side].gameObject.SetActive(SideShouldBeClosed(neighbor));
    }

    // Can be overridden for pieces that have special rules for when a side should be open or closed
    protected bool SideShouldBeClosed(DungeonTile neighbor)
    {
        bool NeighborFilled = neighbor != DungeonTile.Invalid && neighbor != DungeonTile.Empty;
        return !NeighborFilled;
    }
}

public enum DungeonPieceSide
{
    North,
    East,
    South,
    West,
}

[Serializable] public struct SideToGameObjectMapping
{
    public DungeonPieceSide side;
    public GameObject gameObject;
}