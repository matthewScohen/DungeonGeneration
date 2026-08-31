using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

[CreateAssetMenu(menuName = "Dungeon/Dungeon2D Random Room With Paths Strategy")]
public class RandomRoomsWithPaths : Dungeon2DRandomRoomStrategy
{
    [SerializeField] private float EmptyTileCost = 1f;
    [SerializeField] private float HallwayTileCost = 1f;
    [SerializeField] private float RoomTileCost = 1f;
    [SerializeField] private float MaxRandomAddedCost = 0f;

    public override Dungeon2D Generate(int seed)
    {
        Dungeon2D dungeon = new(DungeonWdith, DungeonHeight);
        DungeonGenerationContext context = new(dungeon, seed);

        List<Vector2Int> roomCenters = PlaceRandomRooms(context);

        ManhattanHeuristic manhattanHeuristic = new(minimumMoveCost: Mathf.Min(EmptyTileCost, HallwayTileCost, RoomTileCost));
        TileCosts baseMovementCost = new(EmptyTileCost, HallwayTileCost, RoomTileCost);
        RandomizedCostFunction randomMovementCost = new(baseMovementCost, MaxRandomAddedCost);

        List<Triangle> triangulation = DelaunayTriangulation.Triangulate(roomCenters);
        foreach(Triangle triangle in triangulation)
        {
            Debug.Log($"{triangle.P1}, {triangle.P2}, {triangle.P3}");
            CreatePath(dungeon, triangle.P1, triangle.P2, manhattanHeuristic, randomMovementCost);
            CreatePath(dungeon, triangle.P2, triangle.P3, manhattanHeuristic, randomMovementCost);
            CreatePath(dungeon, triangle.P3, triangle.P1, manhattanHeuristic, randomMovementCost);
        }

        return dungeon;
    }

    private void CreatePath(Dungeon2D dungeon, Vector2Int start, Vector2Int goal, IAStarGridHeuristic heuristic, IAStarMovementCost movementCost)
    {
        Dungeon2DAStar path_creator = new(dungeon, heuristic, movementCost);
        List<Vector2Int> path = path_creator.GeneratePath(start, goal);

        foreach(Vector2Int pathCell in path)
            dungeon[pathCell] = dungeon[pathCell] == Dungeon2DTile.Room ? Dungeon2DTile.Room : Dungeon2DTile.Hallway;
    }

    private void CreatePath(Dungeon2D dungeon, Vector2 start, Vector2 goal, IAStarGridHeuristic heuristic, IAStarMovementCost movementCost)
    {
        CreatePath(dungeon, Vector2Int.RoundToInt(start), Vector2Int.RoundToInt(goal), heuristic, movementCost);
    }
}