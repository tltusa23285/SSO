using Actors;
using System.Collections.Generic;
using Godot;

namespace Strategies
{
    [GlobalClass]
    public abstract partial class VFXStratResource : Resource, IVfxStrat
	{
		public abstract void ApplyVFX(Actor source, HashSet<Actor> targets);
	} 
}
