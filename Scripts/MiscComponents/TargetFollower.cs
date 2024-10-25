using Godot;

public partial class TargetFollower : Node3D
{
	[Export] private Node3D Target;
	[ExportGroup("Follow Settings")]
	[Export]public double PosSpeed = 10;
	[Export]public double RotSpeed = 10;
	[Export]public bool CopyPosition = true;
	[Export]public bool CopyRotation = true;

    public override void _Process(double delta)
    {
		if(CopyPosition) this.GlobalPosition = this.GlobalPosition.MoveToward(Target.GlobalPosition, (float)(delta * PosSpeed));
        if(CopyRotation) this.GlobalRotation = this.GlobalRotation.MoveToward(Target.GlobalRotation, (float)(delta * RotSpeed));
    }
}
