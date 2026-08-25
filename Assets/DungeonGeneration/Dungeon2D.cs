using System;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Dungeon2D", menuName = "Scriptable Objects/Dungeon2D")]
public class Dungeon2D : ScriptableObject
{
    [Min(1)] public int Width = 10;
    [Min(1)] public int Height = 10;

    [SerializeField, HideInInspector] private Dungeon2DTile[] tileMap;

    private void OnValidate()
    {
        int requiredSize = Width * Height;
        
        if (tileMap == null || tileMap.Length != requiredSize)
        {
            Dungeon2DTile[] oldMap = tileMap;
            tileMap = new Dungeon2DTile[requiredSize];

            if (oldMap != null)
            {
                int minLength = Mathf.Min(oldMap.Length, tileMap.Length);
                System.Array.Copy(oldMap, tileMap, minLength);
            }
        }
    }

    public Dungeon2DTile this[int x, int y]
    {
        get 
        {
            int index = y * Width + x;
            return (index >= 0 && index < tileMap.Length) ? (Dungeon2DTile)tileMap[index] : Dungeon2DTile.Invalid;
        }
        set 
        {
            int index = y * Width + x;

            if(!Enum.IsDefined(typeof(Dungeon2DTile), value))
                tileMap[index] = Dungeon2DTile.Invalid;
            else if(index >= 0 && index < tileMap.Length)
                tileMap[index] = value;
            else
                throw new ArgumentOutOfRangeException(nameof(value), $"Cell ({x},{y}) is out of range for dungeon {this}");
        }
    }
}
