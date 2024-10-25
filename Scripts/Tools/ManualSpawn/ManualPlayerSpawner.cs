using Godot;
using System;
using Actors;

namespace Tools
{
    public partial class ManualPlayerSpawner : Node3D
    {
        public override void _Ready()
        {
            PlayerSpawner.Spawn(Vector3.Zero, this, out _);
        }
    }
}