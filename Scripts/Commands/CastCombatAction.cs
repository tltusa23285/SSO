using Godot;
using System.Collections.Generic;
using Strategies;
using Actors;
using System;
using EventArgs;

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

            if (!Source.Casting.TryStartCasting(CastTime)) return false;

            Source.Events.ActionChange += CastUpdate;

            DelayedExecution(targets);
            return true;
        }

        private void CastUpdate(ActorStateChangeArgs<ACTOR_ACTION_STATE> args)
        {
            if (args.Next != ACTOR_ACTION_STATE.Casting)
            {
                CastInterrupted = true;
                Source.Events.ActionChange -= CastUpdate;
            }
        }

        private async void DelayedExecution(HashSet<Actor> targets)
        {
            await ToSignal(Source.GetTree().CreateTimer(this.CastTime), SceneTreeTimer.SignalName.Timeout);

            if (CastInterrupted) return;

            Source.Events.ActionChange -= CastUpdate;

            Source.Casting.StopCast();

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