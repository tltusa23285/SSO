using Godot;
using System;

public partial class CastBar : Control
{
    [Export] private ProgressBar Bar;
    [Export] private Label TimerText;
    [Export] private Timer Timer;

    public override void _Ready()
    {
        DisplayBar(false, 0);
    }

    public void DisplayBar(bool isCasting, float castTime)
    {
        Timer.Stop();

        this.Visible = isCasting;
        Bar.MaxValue = castTime;
        Bar.Value = 0;

        if (this.Visible)
        {
            StartTimeDisplay(castTime);
        }
    }

    private async void StartTimeDisplay(float castTime)
    {
        Timer.OneShot = true;
        Timer.Start(castTime);
        while (!Timer.IsStopped())
        {
            Bar.Value = Bar.MaxValue - Timer.TimeLeft;
            TimerText.Text = Timer.TimeLeft.ToString("0.0");
            await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        }
        DisplayBar(false, 0);
    }
}
