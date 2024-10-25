using Godot;
using System;

public partial class FlyingDamageNumber : Node3D
{
    [Export] private Label Num;
    [Export] private float Speed = 1;
    [Export] private Curve Curve;

    private double Duration;
    private double Timer;
    private Color StartColor;
    private Color EndColor;

    private Vector3 TravelDir;

    public void Play(in string text, in float duration)
    {
        Num.Text = text;

        StartColor = new Color(1, 1, 1, 1);
        EndColor =   new Color(1, 1, 1, 0);

        Duration = duration;
        Timer = Duration;

        RandomNumberGenerator rng = new RandomNumberGenerator();
        TravelDir = Vector3.Up.Rotated(Vector3.Right, Mathf.DegToRad(rng.RandfRange(-15, 15)))
                              .Rotated(Vector3.Forward, Mathf.DegToRad(rng.RandfRange(-15, 15)));
    }

    private float CurveDelta;

    public override void _Process(double delta)
    {
        Timer -= delta;
        this.GlobalPosition += TravelDir * Speed * (float)delta;
        CurveDelta = Curve.Sample((float)Mathf.InverseLerp(Duration, 0, Timer));
        this.Scale = Vector3.Zero.Lerp(Vector3.One, CurveDelta);
        Num.Modulate = EndColor.Lerp(StartColor, CurveDelta);

        if (Timer <= 0)
        {
            this.QueueFree();
        }
    }
}
