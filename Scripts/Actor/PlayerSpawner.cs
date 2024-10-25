using Data;
using Godot;
using System;

namespace Actors
{
    public static class PlayerSpawner
    {
        private const string PLAYER_PATH = "res://Prefabs/Actors/Player/player_rig.tscn";
        public static void Spawn(in Vector3 pos, in Node parent, out Actor player)
        {
            PackedScene scn = GD.Load<PackedScene>(PLAYER_PATH);
            player = scn.Instantiate<Actor>();
            player.Position = pos;
            parent.AddChild(player);
        }
    } 
}
