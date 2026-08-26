 using UnityEngine;

 public class DungeonGenerationContext
    {
        private readonly Dungeon2D Dungeon;

        public DungeonGenerationContext(Dungeon2D dungeon, int seed)
        {
            Dungeon = dungeon;
            Random.InitState(seed);
        }

        public void PlaceRandomRooms(int numberOfPlacementsToAttempt, int maxRoomWidth, int maxRoomHeight, int minRoomWidth = 1, int minRoomHeight = 1)
        {
            for(int i = 0 ; i < numberOfPlacementsToAttempt; i++)
            {
                int x = Random.Range(0, Dungeon.Width);
                int y = Random.Range(0, Dungeon.Height);
                int roomWidth = Random.Range(minRoomWidth, maxRoomWidth);
                int roomHeight = Random.Range(minRoomHeight, maxRoomHeight);

                if(AreaContainsOnly(Dungeon2DTile.Wall, x, y, roomWidth, roomHeight))
                    SetAreaToTile(Dungeon2DTile.Room, x, y, roomWidth, roomHeight);
            }
        }

        public void SetAreaToTile(Dungeon2DTile tile, int x, int y, int width, int height)
        {
            for(int i = x; i < x + width; i++)
                for(int j = y; j < y + height; j++)
                    Dungeon[i, j] = tile;
        }

        public bool AreaContainsTile(Dungeon2DTile tile, int x, int y, int width, int height)
        {
            for(int i = x; i < x + width; i++)
                for(int j = y; j < y + height; j++)
                    if(Dungeon[i, j] == tile)
                        return true;

            return false;
        }

        public bool AreaContainsOnly(Dungeon2DTile tile, int x, int y, int width, int height)
        {
            for(int i = x; i < x + width; i++)
                for(int j = y; j < y + height; j++)
                    if(Dungeon[i, j] != tile)
                        return false;

            return true;
        }
    }