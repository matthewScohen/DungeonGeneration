using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

[CreateAssetMenu(menuName = "Dungeon/Dungeon2D Random Room With Paths Strategy")]
public class RandomRoomsWithPaths : Dungeon2DRandomRoomStrategy
{
    [SerializeField] private float EmptyTileCost = 1f;
    [SerializeField] private float HallwayTileCost = 1f;
    [SerializeField] private float RoomTileCost = 1f;
    [SerializeField] private float MaxRandomAddedCost = 0f;

    public override Dungeon2D Generate(int seed)
    {
        Profiler.BeginSample("Dungeon Generate");
        Dungeon2D dungeon = new(DungeonWdith, DungeonHeight);
        DungeonGenerationContext context = new(dungeon, seed);

        List<Vector2Int> roomCenters = PlaceRandomRooms(context);

        ManhattanHeuristic manhattanHeuristic = new(minimumMoveCost: Mathf.Min(EmptyTileCost, HallwayTileCost, RoomTileCost));
        TileCosts baseMovementCost = new(EmptyTileCost, HallwayTileCost, RoomTileCost);
        RandomizedCostFunction randomMovementCost = new(baseMovementCost, MaxRandomAddedCost);

        foreach(Vector2Int roomCenter1 in roomCenters)
            foreach(Vector2Int roomCenter2 in roomCenters)
                CreatePath(dungeon, roomCenter1, roomCenter2, manhattanHeuristic, randomMovementCost);

        Profiler.EndSample();
        return dungeon;
    }

    private void CreatePath(Dungeon2D dungeon, Vector2Int start, Vector2Int goal, IAStarGridHeuristic heuristic, IAStarMovementCost movementCost)
    {
        Dungeon2DAStar path_creator = new(dungeon, heuristic, movementCost);
        List<Vector2Int> path = path_creator.GeneratePath(start, goal);

        foreach(Vector2Int pathCell in path)
            dungeon[pathCell] = dungeon[pathCell] == Dungeon2DTile.Room ? Dungeon2DTile.Room : Dungeon2DTile.Hallway;
    }
}