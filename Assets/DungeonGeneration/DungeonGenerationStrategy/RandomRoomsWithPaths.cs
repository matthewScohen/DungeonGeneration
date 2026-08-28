using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dungeon/Dungeon2D Random Room With Paths Strategy")]
public class RandomRoomsWithPaths : Dungeon2DRandomRoomStrategy
{
    [SerializeField] private float EmptyTileCost = 1f;
    [SerializeField] private float HallwayTileCost = 1f;
    [SerializeField] private float RoomTileCost = 1f;

    public override Dungeon2D Generate(int seed)
    {
        Dungeon2D dungeon = new(DungeonWdith, DungeonHeight);
        DungeonGenerationContext context = new(dungeon, seed);

        List<Vector2Int> roomCenters = PlaceRandomRooms(context);

        foreach(Vector2Int roomCenter1 in roomCenters)
            foreach(Vector2Int roomCenter2 in roomCenters)
                CreatePath(dungeon, roomCenter1, roomCenter2);

        return dungeon;
    }

    private void CreatePath(Dungeon2D dungeon, Vector2Int start, Vector2Int goal)
    {
        ManhattanHeuristic manhattanHeuristic = new(minimumMoveCost: 1f);
        TileCosts movementCost = new(EmptyTileCost, HallwayTileCost, RoomTileCost);
        Dungeon2DAStar path_creator = new(dungeon, manhattanHeuristic, movementCost);
        List<Vector2Int> path = path_creator.GeneratePath(start, goal);

        foreach(Vector2Int pathCell in path)
            dungeon[pathCell] = Dungeon2DTile.Hallway;
    }
}