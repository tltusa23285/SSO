using Actors;
using Godot;
using System;
using System.Collections.Generic;

namespace Strategies
{
    /// <summary>
    /// Clears combo action, generally used at the end of a combo to clear the last used action
    /// </summary>
    [GlobalClass]
    public partial class BreakCombo : ApplicationStratResource
	{
        public override void Apply(Actor source, HashSet<Actor> targets)
        {
            source.NextComboAction = null;
        }
    } 
}
