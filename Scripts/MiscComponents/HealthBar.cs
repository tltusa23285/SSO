using Godot;

public partial class HealthBar : ProgressBar
{
    [Export] private bool IsMiniBar = false;
    [Export] private Label Label;
    public void SetBar(int current, int max)
    {
        this.Value = ((float)current / max);
        Label.Text = $"{current} / {max}";

        if (IsMiniBar) this.Visible = current != max;
    }
}
