using Godot;
using System.Collections.Generic;
using Strategies;
using Actors;
using System;

namespace Commands.Combat
{
    /// <summary>
    /// Base class for any action which is an attack on another target
    /// </summary>
    [GlobalClass]
    public partial class CombatAction : BarAction
    {
        [Export] public bool IsComboBreaker = false;

        [ExportGroup("Targeting")]
        [Export] public TargetStratResource Targeting;
        [ExportGroup("Visuals")]
        [Export] public VFXStratResource[] VFXs;
        [ExportGroup("Applications")]
        [Export] public ApplicationStratResource[] Applications;
        [Export] public ApplicationStratResource[] FlaggedApplications;
        [ExportGroup("Flagged")]
        [Export] public FlaggedStratResource FlagCondition;

        protected override bool ConstructCommand()
        {
            if (FlagCondition != null)
            {
                FlagCondition.SetupFlagCondition(this);
            }
            return true;
        }

        public void SetFlagged(bool isFlagged)
        {
            this.Flagged = isFlagged;
        }

        protected override bool ExecuteCommand()
        {
            if (this.FlaggedOnly && !Flagged) return false;
            if (!Targeting.GetTargets(Source, out HashSet<Actor> targets)) return false;

            if(IsComboBreaker && Source.NextComboAction != this.Name) Source.NextComboAction = null;

            foreach (var item in VFXs)
            {
                item.ApplyVFX(Source, targets);
            }

            foreach (var app in Flagged ? FlaggedApplications : Applications)
            {
                app.Apply(Source, targets);
            }

            return true;
        }
    } 
}