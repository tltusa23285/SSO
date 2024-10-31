using Godot;
using System;
using TemporaryEffects;

namespace Strategies
{
	[GlobalClass]
    public partial class TempEffectActive : FlaggedStratResource
	{
        [Export] protected string EffectID;
        protected override void SetCondition()
        {
            Action.Source.TempEffects.OnEffectAdded += EffectAdded;
            Action.Source.TempEffects.OnEffectRemoved += EffectRemoved;
        }

        private void EffectAdded(TemporaryEffect eff)
        {
            if (eff.Data.EffectId == EffectID)
            {
                Action.SetFlagged(true);
            }
        }
        private void EffectRemoved(TemporaryEffect eff)
        {
            if (eff.Data.EffectId == EffectID)
            {
                Action.SetFlagged(false);
            }
        }
    } 
}
