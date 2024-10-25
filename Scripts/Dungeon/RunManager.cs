using Actors;
using Godot;
using System;


namespace Dungeon
{
	// Handles dungeon run setup, ie ordering dungeon gen and population
	public partial class RunManager : Node
	{
		[Export] private DungeonGenerator Generator;

        public override void _Ready()
        {
            RandomNumberGenerator rng = new RandomNumberGenerator();
            Generator.Generate();

            PlayerSpawner.Spawn(Generator.StartRoom.Position, this, out Actor player);

            int c = 0;
            foreach (var item in Generator.Rooms)
            {
                for (int i = 0; i < rng.RandiRange(2,6); i++)
                {
                    Vector3 pos = item.GetRandomSpawnPoint();
                    pos = new Vector3(pos.X + rng.RandfRange(-1,1), pos.Y, pos.Z + rng.RandfRange(-1,1));
                    EnemySpawner.Spawn(pos, this, out Actor e);
                    e.Name = $"Enemy_{c++}";
                }
            }
        }
    } 
}
