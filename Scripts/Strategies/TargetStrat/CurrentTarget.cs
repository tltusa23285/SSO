using Actors;
using Godot;
using System.Collections.Generic;

namespace Strategies
{
    [GlobalClass]
    public partial class CurrentTarget : TargetStratResource
	{
        [Export] private float MaxRange;
        public override bool GetTargets(Actor source, out HashSet<Actor> targets)
        {
            targets = null;
            if (source.CurrentTarget == null || source.CurrentTarget == source) return false;
            if (source.GlobalPosition.DistanceTo(source.CurrentTarget.GlobalPosition) > MaxRange) return false;
            targets = new HashSet<Actor>() { source.CurrentTarget };
            return true;
        }
    } 
}