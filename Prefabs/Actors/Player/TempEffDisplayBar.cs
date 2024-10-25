using Godot;
using System;
using System.Collections.Generic;
using TemporaryEffects;

public partial class TempEffDisplayBar : BoxContainer
{
    [Export] private TemporaryEffectContainer Container;

    [Export] private PackedScene DisplayPre;

    private Dictionary<TemporaryEffect, TempEffDisplay> Displays = new Dictionary<TemporaryEffect, TempEffDisplay>();

    public override void _Ready()
    {
        Container.OnEffectAdded += AddEffect;
        Container.OnEffectRemoved += RemoveEffect;
        Container.OnEffectUpdate += UpdateEffect;
    }

    public override void _ExitTree()
    {
        Container.OnEffectAdded -= AddEffect;
        Container.OnEffectRemoved -= RemoveEffect;
        Container.OnEffectUpdate -= UpdateEffect;
    }

    private void AddEffect(TemporaryEffect eff)
    {
        if (Displays.ContainsKey(eff))
        {
            GD.PushWarning($"Duplicate effect to track {eff.Data.EffectId}");
            return;
        }

        TempEffDisplay n = DisplayPre.Instantiate<TempEffDisplay>();
        this.AddChild(n);
        n.Setup(eff);

        Displays.Add(eff, n);
    }

    private void RemoveEffect(TemporaryEffect eff)
    {
        if (!Displays.ContainsKey(eff))
        {
            GD.PushWarning($"Tried to remove untracked effect {eff.Data.EffectId}");
            return;
        }

        Displays[eff].QueueFree();
        Displays.Remove(eff);
    }

    private void UpdateEffect(TemporaryEffect eff)
    {
        if (!Displays.ContainsKey(eff))
        {
            GD.PushWarning($"Tried to update untracked effect {eff.Data.EffectId}");
            return;
        }
        Displays[eff].UpdateTimer(eff.Timer);
    }
}
