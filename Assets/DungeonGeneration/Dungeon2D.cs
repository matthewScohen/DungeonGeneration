using UnityEngine;

[CreateAssetMenu(fileName = "Dungeon2D", menuName = "Scriptable Objects/Dungeon2D")]
public class Dungeon2D : ScriptableObject
{
    [Min(1)] public int Width = 10;
    [Min(1)] public int Height = 10;

    [SerializeField, HideInInspector] private int[] tileMap;

    private void OnValidate()
    {
        int requiredSize = Width * Height;
        
        if (tileMap == null || tileMap.Length != requiredSize)
        {
            int[] oldMap = tileMap;
            tileMap = new int[requiredSize];

            if (oldMap != null)
            {
                int minLength = Mathf.Min(oldMap.Length, tileMap.Length);
                System.Array.Copy(oldMap, tileMap, minLength);
            }
        }
    }

    public int this[int x, int y]
    {
        get 
        {
            int index = y * Width + x;
            return (index >= 0 && index < tileMap.Length) ? tileMap[index] : -1;
        }
        set 
        {
            int index = y * Width + x;
            if (index >= 0 && index < tileMap.Length)
            {
                tileMap[index] = value;
            }
        }
    }
}
