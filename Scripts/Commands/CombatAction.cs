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
        [Export] public string ComboAction;

        [ExportGroup("Targeting")]
        [Export] public TargetStratResource Targeting;
        [ExportGroup("Visuals")]
        [Export] public VFXStratResource[] VFXs;
        [ExportGroup("Applications")]
        [Export] public ApplicationStratResource[] Applications;
        [Export] public ApplicationStratResource[] ComboApplications;

        private bool IsComboed() 
        {
            if (string.IsNullOrEmpty(ComboAction)) return false;
            return Source.LastCommand == ComboAction;
        }

        protected override bool ConstructCommand()
        {
            Source.CommandUsed += SetFlagged;
            return true;
        }

        private void SetFlagged(string comm)
        {
            this.Flagged = IsComboed();
        }

        protected override bool ExecuteCommand()
        {
            if (!Targeting.GetTargets(Source, out HashSet<Actor> targets)) return false;

            foreach (var item in VFXs)
            {
                item.ApplyVFX(Source, targets);
            }

            foreach (var app in IsComboed() ? ComboApplications : Applications)
            {
                app.Apply(Source, targets);
            }

            return true;
        }
    } 
}