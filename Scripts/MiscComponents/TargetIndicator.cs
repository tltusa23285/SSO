using Actors;
using Godot;

public partial class TargetIndicator : Node3D
{
	[Export] private Actor Source;
    [Export] private Node3D Reticle;

    public override void _Ready()
    {
        Source.TargetChanged += OnTargetChange;
    }

    protected virtual void OnTargetChange()
    {
        Reticle.Visible = Source.CurrentTarget != null;
    }

    public override void _Process(double delta)
    {
        if (Source.CurrentTarget != null)
        {
            Reticle.GlobalPosition = Source.CurrentTarget.GlobalPosition;
        }
    }
}
