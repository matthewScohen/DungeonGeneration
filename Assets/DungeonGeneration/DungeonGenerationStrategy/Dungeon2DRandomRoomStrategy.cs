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

        PlaceRandomRooms(context);

        return dungeon;
    }

    private void PlaceRandomRooms(DungeonGenerationContext context)
    {
        for(int i = 0 ; i < NumberOfRoomsToAttempt; i++)
        {
            int x = Random.Range(0, context.Width);
            int y = Random.Range(0, context.Height);
            int roomWidth = Random.Range(MinRoomWidth, MaxRoomWidth);
            int roomHeight = Random.Range(MinRoomHeight, MaxRoomHeight);

            // Need to check an additional area of 2 * border because padding must be on both sides of the room
            if(context.AreaContainsOnly(Dungeon2DTile.Wall, x - Border, y - Border, roomWidth + 2 * Border, roomHeight + 2 * Border))
                context.SetAreaToTile(Dungeon2DTile.Room, x, y, roomWidth, roomHeight);
        }
    }
}