using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

[CreateAssetMenu(menuName = "Dungeon/Dungeon Random Room With Paths Strategy")]
public class RandomRoomsWithPaths : DungeonRandomRoomStrategy
{
    [SerializeField] private float WallTileCost = 1f;
    [SerializeField] private float HallwayTileCost = 1f;
    [SerializeField] private float RoomTileCost = 1f;
    [SerializeField] private float MaxRandomAddedCost = 0f;

    public override Dungeon Generate(int seed)
    {
        Dungeon dungeon = new(DungeonWdith, DungeonHeight);
        DungeonGenerationContext context = new(dungeon, seed);

        List<Vector2Int> roomCenters = PlaceRandomRooms(context);

        ManhattanHeuristic manhattanHeuristic = new(minimumMoveCost: Mathf.Min(WallTileCost, HallwayTileCost, RoomTileCost));
        TileCosts baseMovementCost = new(WallTileCost, HallwayTileCost, RoomTileCost);
        RandomizedCostFunction randomMovementCost = new(baseMovementCost, MaxRandomAddedCost);

        List<Triangle> triangulation = DelaunayTriangulation.Triangulate(roomCenters);
        foreach(Triangle triangle in triangulation)
        {
            CreatePath(dungeon, triangle.P1, triangle.P2, manhattanHeuristic, randomMovementCost);
            CreatePath(dungeon, triangle.P2, triangle.P3, manhattanHeuristic, randomMovementCost);
            CreatePath(dungeon, triangle.P3, triangle.P1, manhattanHeuristic, randomMovementCost);
        }

        return dungeon;
    }

    private void CreatePath(Dungeon dungeon, Vector2Int start, Vector2Int goal, IAStarGridHeuristic heuristic, IAStarMovementCost movementCost)
    {
        DungeonAStar path_creator = new(dungeon, heuristic, movementCost);
        List<Vector2Int> path = path_creator.GeneratePath(start, goal);

        foreach(Vector2Int pathCell in path)
            dungeon[pathCell] = dungeon[pathCell] == DungeonTile.Room ? DungeonTile.Room : DungeonTile.Hallway;
    }

    private void CreatePath(Dungeon dungeon, Vector2 start, Vector2 goal, IAStarGridHeuristic heuristic, IAStarMovementCost movementCost)
    {
        CreatePath(dungeon, Vector2Int.RoundToInt(start), Vector2Int.RoundToInt(goal), heuristic, movementCost);
    }
}