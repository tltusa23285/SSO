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
                if (item is not Actor) continue;
                Actor other = item as Actor;
                if (other == source) continue;
                targets.Add(item as Actor);
            }
            return true;
        }
    } 
}