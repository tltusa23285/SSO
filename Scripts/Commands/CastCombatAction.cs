using Godot;
using System.Collections.Generic;
using Strategies;
using Actors;
using System;

namespace Commands.Combat
{
    [GlobalClass]
    public partial class CastCombatAction : CombatAction
    {
        [Export] private float CastTime = 1.5f;

        private bool CastInterrupted = false;

        protected override bool ExecuteCommand()
        {

            if (this.FlaggedOnly && !Flagged) return false;
            if (!Targeting.GetTargets(Source, out HashSet<Actor> targets)) return false;

            if (IsComboBreaker && Source.NextComboAction != this.Name) Source.NextComboAction = null;

            CastInterrupted = false;

            Source.CastingStatus += CastUpdate;
            Source.StartCasting(CastTime);

            DelayedExecution(targets);

            return true;
        }

        private void CastUpdate(bool isCasting, float _)
        {
            if (!isCasting)
            {
                CastInterrupted = true;
                Source.CastingStatus -= CastUpdate;
            }
        }

        private async void DelayedExecution(HashSet<Actor> targets)
        {
            await ToSignal(Source.GetTree().CreateTimer(this.CastTime), SceneTreeTimer.SignalName.Timeout);

            if (CastInterrupted) return;

            Source.CastingStatus -= CastUpdate;

            Source.StopCasting();

            foreach (var item in VFXs)
            {
                item.ApplyVFX(Source, targets);
            }

            foreach (var app in Flagged ? FlaggedApplications : Applications)
            {
                app.Apply(Source, targets);
            }
        }
    }
}