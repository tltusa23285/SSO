using Godot;
using System;
using Actors;

public partial class PlayerTargetDisplay : Control
{
    [Export] private Actor Source;
    [Export] private HealthBar Bar;

    private Actor CurrentTarget;

    public override void _Ready()
    {
        Source.TargetChanged += TargetUpdated;
        TargetUpdated();
    }

    private void TargetUpdated()
    {
        if (CurrentTarget != null)
        {
            CurrentTarget.Health.HealthChange -= Bar.SetBar;
        }

        CurrentTarget = Source.CurrentTarget;
        this.Visible = CurrentTarget != null;
        if (CurrentTarget != null)
        {
            CurrentTarget.Health.HealthChange += Bar.SetBar;
            Bar.SetBar(CurrentTarget.Health.CurrentHealth, CurrentTarget.Health.MaxHealth);
        }
    }
}
