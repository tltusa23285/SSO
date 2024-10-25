using Actors;
using Godot;
using System.Collections.Generic;

namespace Strategies
{
    [GlobalClass]
    public partial class PlayAttackAnimation : VFXStratResource, IVfxStrat
    {
        [Export] private string Anim;
        [Export] private int Priority;

        public override void ApplyVFX(Actor source, HashSet<Actor> targets)
        {
            source.GCon.SetAnimation(Anim, Priority);
        }
    } 
}