using UnityEngine;
using System;

// When making a DungeonPiece prefab the North side should coorespond to the Z+ axis
public class DungeonPiece : MonoBehaviour
{

    private readonly bool[] SideOpen = new bool[Enum.GetNames(typeof(DungeonPieceSide)).Length];
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

    public void SetSideOpen(DungeonPieceSide side, bool open)
    {
        SideOpen[(int)side] = open;
        SideToObjectMapping[(int)side].gameObject.SetActive(open);
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