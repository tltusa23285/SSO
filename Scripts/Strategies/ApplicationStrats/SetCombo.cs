using Actors;
using Godot;
using System;
using System.Collections.Generic;

namespace Strategies
{
    [GlobalClass]
    public partial class SetCombo : ApplicationStratResource
	{
        [Export] private string ComboName;
        public override void Apply(Actor source, HashSet<Actor> targets)
        {
            source.NextComboAction = ComboName;
        }
    } 
}
