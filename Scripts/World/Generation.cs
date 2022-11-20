using System;
using Godot;
using Godot.Collections;

public static class BackTraceMaze{
    private static Array<Array<int>> maze = new Array<Array<int>>();
    private static Array<Vector2> visited = new Array<Vector2>();
    private static int width = 0;
    private static int height = 0;
    private static Random rng;

    public static Array<Array<int>> createMaze(int w, int h, int seed, Vector2 startPos, Vector2 endPos){
        width = w;
        height = h;
        rng = new Random(seed);

        // Fill array with 0 which is a wall
        for (int x = 0; x < width; x++){
            maze.Add(new Array<int>());
            for (int y = 0; y < height; y++){
                maze[x].Add(0);
            }
        }
        
        // Generate base maze
        generateMaze(startPos);

        // Remove random tiles
        for (int i =0; i < width*height/5; i++) maze[rng.Next(1, width-1)][rng.Next(1, height-1)] = -1;

        // Clean up any randomly placed holes
        for (int x = 0; x < maze.Count; x++){
            for (int y = 0; y < maze[0].Count; y++){
                if (maze[x][y] == -1){
                    if ((x > 1 && maze[x-1][y] == 0) && (x < maze.Count && maze[x+1][y] == 0) && (x > 1 && maze[x][y-1] == 0) && (x < maze[0].Count && maze[x][y+1] == 0)){
                        maze[x][y] = 0;
                    }
                }
            }
        }

        // Clear the end point
        maze[(int)endPos.x][(int)endPos.y] = -1;
        maze[(int)endPos.x][(int)endPos.y-1] = -1;
        maze[(int)endPos.x][(int)endPos.y-2] = -1;
        return maze;
    }

    private static void generateMaze(Vector2 position)
    {
        visited.Add(position);
        
        Array<Vector2> neighbors = new Array<Vector2>();

        // Get neighbors
        neighbors.Add(new Vector2(position.x + 2, position.y));
        neighbors.Add(new Vector2(position.x - 2, position.y));
        neighbors.Add(new Vector2(position.x, position.y + 2));
        neighbors.Add(new Vector2(position.x, position.y - 2));

        // Shuffle
        int n = neighbors.Count;
        while (n > 1) {
            n--;
            int k = rng.Next(n + 1);
            Vector2 value = neighbors[k];
            neighbors[k] = neighbors[n];
            neighbors[n] = value;
        }

        // Iterate Neighbors
        foreach (Vector2 neighbor in neighbors){
            if (validNeighbor(neighbor, visited)){
                maze[(int)position.x][(int)position.y] = -1;
                maze[(int)(position.x + (neighbor.x - position.x) / 2)][(int)(position.y + (neighbor.y - position.y) / 2)] = -1;
                generateMaze(neighbor);
            }
        }
    }

    private static bool validNeighbor(Vector2 neighbor, Array<Vector2> visit){
        return (neighbor.x > 0 && neighbor.x < width-1 && neighbor.y > 0 && neighbor.y < height-1 && !(visit.Contains(neighbor)));
    }
}