using Commands.Combat;
using Godot;
using System;

namespace Strategies
{
    [GlobalClass]
    public partial class ActionComboed : FlaggedStratResource
	{
        [Export] protected string ComboAction;
        protected override void SetCondition()
        {
            Action.Source.CommandUsed += CheckCombo;
        }

        private void CheckCombo(string comm)
        {
            Action.SetFlagged
                (
                    ComboAction == comm
                );        
        }
    } 
}
