using Godot;
using System;
using System.Collections.Generic;

namespace Dungeon
{
	public partial class DungeonRoom : Node3D
	{
		[Export] private Node3D[] EnemySpawns;

		public IEnumerable<Vector3> EnemySpawnPoints
		{
			get
			{
				if (EnemySpawns == null || EnemySpawns.Length < 1) yield return this.GlobalPosition;
				else foreach (var item in EnemySpawns) yield return item.GlobalPosition;
			}
		}

		public Vector3 GetRandomSpawnPoint()
        {
            if (EnemySpawns == null || EnemySpawns.Length < 1) return this.GlobalPosition;
			return EnemySpawns[new RandomNumberGenerator().RandiRange(0, EnemySpawns.Length - 1)].Position;
        }
	} 
}
