using Actors;
using Godot;
using System;
using System.Collections.Generic;

namespace Strategies
{
    [GlobalClass]
    public partial class RemoveTemporaryEffect : ApplicationStratResource
    {
        private enum APP_TYPE
        {
            Targets,
            Source,
        }

        [Export] private APP_TYPE ApplyTo;
        [Export] private string EffectID;
        public override void Apply(Actor source, HashSet<Actor> targets)
        {
            switch (ApplyTo)
            {
                case APP_TYPE.Targets:
                    foreach (var item in targets)
                    {
                        item.TempEffects.RemoveEffect(EffectID);
                    }
                    break;
                case APP_TYPE.Source:
                    source.TempEffects.RemoveEffect(EffectID);
                    break;
                default:
                    break;
            }
        }
    } 
}
