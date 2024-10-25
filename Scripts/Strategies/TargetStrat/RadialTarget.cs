using Actors;
using Godot;
using System.Collections.Generic;

namespace Strategies
{
    [GlobalClass]
    public partial class RadialTarget : TargetStratResource
	{
        [Export] private float Radius;
        public override bool GetTargets(Actor source, out HashSet<Actor> targets)
        {
            targets = new HashSet<Actor>();
            foreach (var item in source.GetWorld3D().DirectSpaceState.OverlapSphere(source.GlobalPosition, Radius))
            {
                GodotObject obj = item["collider"].AsGodotObject();
                if (obj is not Actor) continue;
                Actor other = obj as Actor;
                if (other == source) continue;
                targets.Add(obj as Actor);
            }
            return true;
        }
    } 
}