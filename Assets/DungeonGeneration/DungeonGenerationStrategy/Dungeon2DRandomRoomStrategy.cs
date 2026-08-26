using UnityEngine;

[CreateAssetMenu(menuName = "Dungeon/Dungeon2D Random Room Strategy")]
public class Dungeon2DRandomRoomStrategy : Dungeon2DGenerationStrategy
{
    [SerializeField] private int NumberOfRoomsToAttempt = 5;
    [SerializeField] private int DungeonWdith = 50;
    [SerializeField] private int DungeonHeight = 50;

    [SerializeField] private int MinRoomWidth = 2;
    [SerializeField] private int MaxRoomWidth = 10;
    [SerializeField] private int MinRoomHeight = 2;
    [SerializeField] private int MaxRoomHeight = 10;
    [SerializeField] private int Border = 1;

    public override Dungeon2D Generate(int seed)
    {
        Dungeon2D dungeon = new(DungeonWdith, DungeonHeight);
        DungeonGenerationContext context = new(dungeon, seed);

        context.PlaceRandomRooms(NumberOfRoomsToAttempt, MaxRoomWidth, MaxRoomHeight, MinRoomWidth, MinRoomHeight, Border);

        return dungeon;
    }
}