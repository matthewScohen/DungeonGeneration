using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dungeon/Dungeon2D Random Room With Paths Strategy")]
public class RandomRoomsWithPaths : Dungeon2DRandomRoomStrategy
{
    public override Dungeon2D Generate(int seed)
    {
        Dungeon2D dungeon = new(DungeonWdith, DungeonHeight);
        DungeonGenerationContext context = new(dungeon, seed);

        PlaceRandomRooms(context);

        for(int i = 0; i < 10; i++)
        {
            Vector2Int random1 = new(Random.Range(0, DungeonWdith), Random.Range(0, DungeonHeight));
            Vector2Int random2 = new(Random.Range(0, DungeonWdith), Random.Range(0, DungeonHeight));
            CreatePath(dungeon, random1, random2);
        }

        return dungeon;
    }

    private void CreatePath(Dungeon2D dungeon, Vector2Int start, Vector2Int goal)
    {
        ManhattanHeuristic manhattanHeuristic = new(minimumMoveCost: 1f);
        ConstantMovementCost movementCost = new(cost: 1f);
        Dungeon2DAStar path_creator = new(dungeon, manhattanHeuristic, movementCost);
        List<Vector2Int> path = path_creator.GeneratePath(start, goal);

        foreach(Vector2Int pathCell in path)
            dungeon[pathCell] = Dungeon2DTile.Hallway;
    }
}