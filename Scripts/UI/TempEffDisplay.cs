using Godot;
using System;
using TemporaryEffects;

public partial class TempEffDisplay : Control
{
    [Export] public TextureRect IconRect;
    [Export] public Label Time;

    public void Setup(TemporaryEffect eff)
    {
        IconRect.Texture = eff.Data.Icon;
        UpdateTimer(eff.Timer);
    }

    public void UpdateTimer(in float timer)
    {
        Time.Text = timer >= 1 ? Mathf.FloorToInt(timer).ToString() : "";
    }
}
