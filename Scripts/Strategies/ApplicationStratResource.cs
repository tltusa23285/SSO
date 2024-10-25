using Godot;
using System;
using Actors;
using System.Collections.Generic;

namespace Strategies
{
	[GlobalClass]
	public abstract partial class ApplicationStratResource : Resource, IApplicationStrat
	{
		public abstract void Apply(Actor source, HashSet<Actor> targets);
	} 
}
