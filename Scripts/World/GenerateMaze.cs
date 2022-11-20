using Godot;
using Godot.Collections;

[Tool]
public class GenerateMaze : Spatial{
    /// Initilize Variables
    // Private Variables
    private int width = 20;
    private int height = 20;
    private int seed = 0;
    private bool randomSeed = true;
    private bool randomEnd = true;
    private Vector2 startPos = new Vector2(1, 0);
    private Vector2 endPos = new Vector2(18, 19);

    public override void _Ready()
    {
        // Ready
        if (randomSeed){ seed = System.Guid.NewGuid().GetHashCode(); } // Create random seed
        GameManager.seed = seed;
        Array<Array<int>> a =  BackTraceMaze.createMaze(width, height, seed, startPos, endPos);
        GameManager.maze = a;
        generateMazeMesh(a);
    }

    public override void _Process(float delta)
    {
        // Process
        if (Engine.EditorHint){ return; }
    }

    private void generateMazeMesh(Array<Array<int>> m){
        GridMap gp = GetNode<GridMap>("GridMap");
        gp.Clear();
        for (int x = 0; x < width; x++){
            for (int y = 0; y < height; y++){
                if (m[x][y] == 0) gp.SetCellItem(x, 0, y, m[x][y]);
                //gp.SetCellItem(x, 3, y, 0);
            }
        }
    }


    /**
    *
    *   THIS IS ALL SCRIPT FOR THE GODOT EDITOR!
    *
    **/

    public override Godot.Collections.Array _GetPropertyList()
    {
        Array propertyList = new Array();
        
        Dictionary d = new Dictionary();
        d.Add("type", Variant.Type.Int);
        d.Add("name", "Width");
        d.Add("usage", PropertyUsageFlags.Default);
        propertyList.Add(d);

        d = new Dictionary();
        d.Add("type", Variant.Type.Int);
        d.Add("name", "Height");
        propertyList.Add(d);

        d = new Dictionary();
        d.Add("type", Variant.Type.Vector2);
        d.Add("name", "Start Position");
        propertyList.Add(d);

        d = new Dictionary();
        d.Add("type", Variant.Type.Bool);
        d.Add("name", "Random End Position");
        propertyList.Add(d);

        if (!randomEnd){
            d = new Dictionary();
            d.Add("type", Variant.Type.Vector2);
            d.Add("name", "End Position");
            propertyList.Add(d);
        }

        d = new Dictionary();
        d.Add("type", Variant.Type.Bool);
        d.Add("name", "Random Seed");
        propertyList.Add(d);

        if (!randomSeed){
            d = new Dictionary();
            d.Add("type", Variant.Type.Int);
            d.Add("name", "Seed");
            propertyList.Add(d);
        }

        return propertyList;
    }

    public override object _Get(string property)
    {
        switch (property){
            case "Width":
                return width;
            case "Height":
                return height;
            case "Random Seed":
                return randomSeed;
            case "Seed":
                return seed;
            case "Start Position":
                return startPos;
            case "End Position":
                return endPos;
            case "Random End Position":
                return randomEnd;
            default:
                return null;
        }
    }

    public override bool _Set(string property, object value)
    {
        switch (property){
            case "Width":
                width = (int)value;
                break;
            case "Height":
                height = (int)value;
                break;
            case "Random Seed":
                PropertyListChangedNotify();
                randomSeed = (bool)value;
                return randomSeed;
            case "Seed":
                seed = (int)value;
                break;
            case "Start Position":
                startPos = (Vector2)value;
                break;
            case "End Position":
                endPos = (Vector2)value;  
                break;
            case "Random End Position":
                PropertyListChangedNotify();
                randomEnd = (bool)value;
                return randomEnd;
            default:
                break;
        }
        editorUpdate();
        return true;
    }

    public void editorUpdate(){
        if (!Engine.EditorHint){ return; }
        cornerUpdate();
        solidUpdate();
    }

    private void cornerUpdate(){
        Transform transform = new Transform();
        transform.origin = new Vector3(width * GetNode<GridMap>("GridMap").CellScale, 0, height * GetNode<GridMap>("GridMap").CellScale);
        GetNode<Spatial>("Corner").Transform = transform;
    }

    private void solidUpdate(){
        GetNode<GridMap>("GridMap").Clear();
        for (int x = 0; x < width; x++){
            for (int y = 0; y < height; y++){
                GetNode<GridMap>("GridMap").SetCellItem(x, 0, y, 0);
            }
        }
        GetNode<GridMap>("GridMap").SetCellItem((int)startPos.x, 0, (int)startPos.y, -1);
    }
}