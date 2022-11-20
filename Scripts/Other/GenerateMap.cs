using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public class GenerateMap : Node2D
{
    [Export] public Texture t;
    private bool done;
    public override void _Ready()
    {
        
    }

    public override void _Process(float delta)
    {
        if (!done){ generateMazeMesh(BackTraceMaze.createMaze(30, 30, GameManager.seed, new Vector2(1, 1), new Vector2(18, 19))); }
    }

    private void generateMazeMesh(Array<Array<int>> m)
    {
        if (m == null) { return; }
        done = true;
        for (int x = 0; x < m.Count; x++){
            for (int y = 0; y < m[x].Count; y++){
                    MeshInstance2D meshInstance = new MeshInstance2D();
                    meshInstance.Mesh = new QuadMesh();
                    meshInstance.Position = new Vector2(x, y);
                    if (m[x][y] != 0){ meshInstance.Texture = t; }
                    AddChild(meshInstance);
                    
            }
        }
    }
}
