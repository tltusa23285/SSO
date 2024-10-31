using Commands.Combat;
using Godot;
using System;

namespace Strategies
{
    [GlobalClass]
    public partial class ActionComboed : FlaggedStratResource
	{
        protected override void SetCondition()
        {
            Action.Source.CommandUsed += CheckCombo;
        }

        private void CheckCombo(string comm)
        {
            Action.SetFlagged(Action.Name == comm);        
        }
    } 
}
