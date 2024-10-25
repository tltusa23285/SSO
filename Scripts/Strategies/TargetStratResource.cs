using Actors;
using Godot;
using System;
using System.Collections.Generic;

namespace Strategies
{
	[GlobalClass]
	public abstract partial class TargetStratResource : Resource, ITargetStrat
	{
		public abstract bool GetTargets(Actor source, out HashSet<Actor> targets);
	} 
}
