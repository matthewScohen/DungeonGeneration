using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dungeon/Dungeon Random Room Strategy")]
public class DungeonRandomRoomStrategy : DungeonGenerationStrategy
{
    [SerializeField] protected int NumberOfRoomsToAttempt = 5;
    [SerializeField] protected int DungeonWdith = 50;
    [SerializeField] protected int DungeonHeight = 50;
    [SerializeField] protected int MinRoomWidth = 2;
    [SerializeField] protected int MaxRoomWidth = 10;
    [SerializeField] protected int MinRoomHeight = 2;
    [SerializeField] protected int MaxRoomHeight = 10;
    [SerializeField] protected int Border = 1;

    public override Dungeon Generate(int seed)
    {
        Dungeon dungeon = new(DungeonWdith, DungeonHeight);
        DungeonGenerationContext context = new(dungeon, seed);

        PlaceRandomRooms(context);

        return dungeon;
    }

    protected List<Vector2Int> PlaceRandomRooms(DungeonGenerationContext context)
    {
        List<Vector2Int> RoomCenters = new();

        for(int i = 0 ; i < NumberOfRoomsToAttempt; i++)
        {
            int x = Random.Range(0, context.Width);
            int y = Random.Range(0, context.Height);
            int roomWidth = Random.Range(MinRoomWidth, MaxRoomWidth);
            int roomHeight = Random.Range(MinRoomHeight, MaxRoomHeight);

            // Need to check an additional area of 2 * border because padding must be on both sides of the room
            if(context.AreaContainsOnly(DungeonTile.Wall, x - Border, y - Border, roomWidth + 2 * Border, roomHeight + 2 * Border))
            {
                context.SetAreaToTile(DungeonTile.Room, x, y, roomWidth, roomHeight);
                RoomCenters.Add(new(x + roomWidth / 2, y + roomHeight / 2));
            }
        }

        return RoomCenters;
    }
}