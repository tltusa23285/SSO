using Godot;

public partial class HealthBar : ProgressBar
{
    [Export] private Label Label;
    private void SetBar(int current, int max)
    {
        this.Value = ((float)current / max);
        Label.Text = $"{current} / {max}";
    }
}
