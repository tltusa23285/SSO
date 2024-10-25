using Godot;
using System;

public partial class DamageNumberSpawner : Node
{
    public static DamageNumberSpawner Instance { get; private set; }
    [Export] public PackedScene NumberPre;

    public override void _Ready()
    {
        Instance = this;
    }

    public void Spawn(in Actors.Actor actor, in int amount)
    {
        FlyingDamageNumber num = NumberPre.Instantiate() as FlyingDamageNumber;
        this.AddChild(num);
        num.GlobalPosition = actor.GlobalPosition + (Vector3.Up * 2);
        num.Play(amount.ToString(), 1f);
    }
}
