using Actors;
using Godot;
using System;
using System.Collections.Generic;

namespace Strategies
{
	[GlobalClass]
    public partial class SelfTarget : TargetStratResource
	{
        public override bool GetTargets(Actor source, out HashSet<Actor> targets)
        {
            targets = new HashSet<Actor>() { source };
            return true;
        }
    } 
}
