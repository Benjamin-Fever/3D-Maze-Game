using Godot;
using System;

public class PlayerController : KinematicBody
{
    
    [Export] public int sensitivity = 10;
    [Export] public int speed = 10;
    [Export] public int jumpForce = 10;
    [Export] public float gravity = 9.8f;
    private Camera cam;
    private RayCast groundRay;
    private bool groundCheck = true;
    private float xRotation = 0;
    private float yRotation = 0;
    private Vector3 gravityLocal = new Vector3(0, 0, 0);

    public override void _Ready(){
        Input.MouseMode = Input.MouseModeEnum.Captured;
        cam = GetNode<Camera>("Player Cam");
        groundRay = GetNode<RayCast>("RayCast");
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey eventKey){
            if (eventKey.Pressed && eventKey.Scancode == (int)KeyList.Escape){
                Input.MouseMode = (Input.MouseMode == Input.MouseModeEnum.Captured) ? Input.MouseModeEnum.Visible : Input.MouseModeEnum.Captured;
            }
        }
        if (@event is InputEventMouseMotion eventMouseMotion){
            xRotation = -Util.deg2rad(eventMouseMotion.Relative.y) * sensitivity;
            yRotation = -Util.deg2rad(eventMouseMotion.Relative.x) * sensitivity;
        }
            
    }

    public override void _PhysicsProcess(float delta){
        groundCheck = groundRay.IsColliding();
        Vector2 inputVec = Input.GetVector("move_up", "move_down", "move_left", "move_right");
        Vector3 inputMove = Transform.basis.Xform(new Vector3(inputVec.y, 0, inputVec.x)).Normalized() * speed;
        if (!groundCheck){ gravityLocal += gravity * Vector3.Down * delta; }
        else{ gravityLocal = Vector3.Zero; }
        if (Input.IsActionJustPressed("jump") && groundCheck){ gravityLocal = Vector3.Up * jumpForce * delta * 500; }
        MoveAndSlide(inputMove + gravityLocal, Vector3.Up);
        
    }

    public override void _Process(float delta)
    {
        Vector3 camRotation = cam.Rotation;

        camRotation.x += xRotation * delta;
        camRotation.x = Mathf.Clamp(camRotation.x, Util.deg2rad(-80), Util.deg2rad(80));

        RotateY(yRotation * delta);
        cam.Rotation = camRotation;

        xRotation = 0;
        yRotation = 0;
    }
}
