using Godot;
using Inputs;

public partial class Aimlook : Node
{
    private enum CONTROL_STATE {None, Player}

    [ExportGroup("Nodes")]
    [Export] private CharacterBody3D PlayerRoot;
    [Export] private Node3D FollowTarget;
	[Export] private Node3D Root;
	[Export] private Node3D CameraContainer;
    [Export] private Node3D Cam;

    [ExportGroup("Settings")]
    [Export] private float Distance = 5;
    [ExportSubgroup("Mouse settings")]
    [Export(PropertyHint.Range, "1,100,1")]private int MouseSens = 50;
    [ExportSubgroup("Clamp settings")]
    [Export] private float MinPitch = -80;
    [Export] private float MaxPitch =  80;

    private CONTROL_STATE ControlState;

    [Export] Vector3 TargetRotation;

    public Basis CamBasis => Cam.GlobalBasis;

    public override void _Ready()
    {
        Input.UseAccumulatedInput = false;
        Root.GlobalPosition = FollowTarget.GlobalPosition;
        Cam.GlobalPosition = CameraContainer.GlobalPosition;
    }

    private Vector2 LastMousePos;

    public bool HandleLookInput(InputEvent input)
    {
        switch (ControlState)
        {
            case CONTROL_STATE.None:
                if (input is InputEventKey)
                {
                    if (input.IsActionPressed("ui_cancel"))
                    {
                        GetTree().Quit();
                    }
                }
                if (input is InputEventMouseButton)
                {
                    if (input.IsActionPressed(KeyMap.LookPlayerControl))
                    {
                        LastMousePos = (input as InputEventMouseButton).GlobalPosition;
                        Input.MouseMode = Input.MouseModeEnum.Captured;
                        ControlState = CONTROL_STATE.Player;
                        //PlayerRoot.GlobalRotation = Root.GlobalRotation;
                        return true;
                    }
                }
                break;
            case CONTROL_STATE.Player:

                if (input is InputEventMouseButton)
                {
                    if (input.IsActionReleased(KeyMap.LookPlayerControl))
                    {
                        Input.MouseMode = Input.MouseModeEnum.Visible;
                        Input.WarpMouse(LastMousePos);
                        ControlState = CONTROL_STATE.None;
                        return true;
                    }
                }
                if (input is InputEventMouseMotion)
                {
                    //AimPlayer(input as InputEventMouseMotion);
                    InputEventMouseMotion mouse = input as InputEventMouseMotion;
                    Vector2 motion = mouse.Relative * .001f;
                    //Yaw(PlayerRoot, motion.Y);
                    TargetRotation.X -= motion.Y;
                    TargetRotation.X = Mathf.Clamp(TargetRotation.X, Mathf.DegToRad(MinPitch), Mathf.DegToRad(MaxPitch));
                    TargetRotation.Y -= motion.X;
                    TargetRotation.Y = Mathf.Wrap(TargetRotation.Y,-Mathf.Pi,Mathf.Pi);
                }
                break;
            default: return false;
        }
        return false;
    }

    private Vector3 RotWrap;

    public override void _Process(double delta)
    {
        Root.GlobalPosition = Root.GlobalPosition.MoveToward(FollowTarget.GlobalPosition, (float)delta * 10f);
        Root.GlobalRotation = Root.GlobalRotation.RotateToward(TargetRotation, (float)delta * 10f);

        CameraContainer.Position = Vector3.Back * Distance;

        //Cam.GlobalPosition = Cam.GlobalPosition.MoveToward(CameraContainer.GlobalPosition, (float)delta * 10f);
        //Cam.GlobalRotation = Cam.GlobalRotation.MoveToward(CameraContainer.GlobalRotation, (float)delta * 10f);

        Cam.GlobalPosition = CameraContainer.GlobalPosition;
        Cam.GlobalRotation = CameraContainer.GlobalRotation;

        //if (ControlState == CONTROL_STATE.None && PlayerRoot.Velocity != Vector3.Zero) AimToDefault((float)delta);
    }

    private void AimToDefault(float delta)
    {
        Yaw(Root, (Root.Rotation.Y - TargetRotation.Y) * delta * 100f);
    }


    private void Yaw(Node3D target,float amount)
    {
        if (Mathf.IsZeroApprox(amount)) return;
        target.RotateObjectLocal(Vector3.Down, Mathf.DegToRad(amount));
        target.Orthonormalize();
    }

    private void Pitch(float amount)
    {
        if (Mathf.IsZeroApprox(amount)) return;
        CameraContainer.RotateObjectLocal(Vector3.Left, Mathf.DegToRad(amount));
        CameraContainer.Orthonormalize();
    }

    private void ClampPitch()
    {
        if (CameraContainer.Rotation.X > Mathf.DegToRad(MinPitch) && CameraContainer.Rotation.X < Mathf.DegToRad(MaxPitch)) return;
        CameraContainer.Rotation = new Vector3(Mathf.Clamp(CameraContainer.Rotation.X, Mathf.DegToRad(MinPitch), Mathf.DegToRad(MaxPitch)), CameraContainer.Rotation.Y, CameraContainer.Rotation.Z);
        CameraContainer.Orthonormalize();
    }
}
