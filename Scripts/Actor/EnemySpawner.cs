using Godot;

namespace Actors
{
    public static class EnemySpawner
    {
        private const string RAT_PATH = "res://Prefabs/Actors/Enemies/rat.tscn";
        private const string DUMMY_PATH = "res://Prefabs/Actors/Enemies/dummy.tscn";
        public static void Spawn(in Vector3 pos, in Node parent, out Actor enemy)
        {
            PackedScene scn = GD.Load<PackedScene>(RAT_PATH);
            enemy = scn.Instantiate<Actor>();
            enemy.Position = pos;
            parent.AddChild(enemy);
        }

        public static void SpawnDummy(in Vector3 pos, in Node parent, out Actor enemy)
        {
            PackedScene scn = GD.Load<PackedScene>(DUMMY_PATH);
            enemy = scn.Instantiate<Actor>();
            enemy.Position = pos;
            parent.AddChild(enemy);
        }
    }
}