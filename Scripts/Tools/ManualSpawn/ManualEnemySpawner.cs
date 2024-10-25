using Godot;
using System;
using Actors;

namespace Tools
{
    public partial class ManualEnemySpawner : Node3D
    {
        public override void _Ready()
        {
            EnemySpawner.SpawnDummy(Vector3.Zero, this, out _);
        }
    }
}