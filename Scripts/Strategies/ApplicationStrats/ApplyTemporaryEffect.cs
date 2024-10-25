using Actors;
using Godot;
using System;
using System.Collections.Generic;
using TemporaryEffects;

namespace Strategies
{
    [GlobalClass]
	public partial class ApplyTemporaryEffect : ApplicationStratResource
	{
        private enum APP_TYPE
        {
            Targets,
            Source,
        }

        [Export] private APP_TYPE ApplyTo;
        [Export] private TemporaryEffectData Effect;
        public override void Apply(Actor source, HashSet<Actor> targets)
        {
            switch (ApplyTo)
            {
                case APP_TYPE.Targets:
                    foreach (var item in targets)
                    {
                        if (!Effect.ApplyEffect(item))
                            GD.PushError($"Failed to apply temp effect {Effect.EffectId} to {item.Stats.DisplayName}");
                    }
                    break;
                case APP_TYPE.Source:
                    if (!Effect.ApplyEffect(source))
                        GD.PushError($"Failed to apply temp effect {Effect.EffectId} to {source.Stats.DisplayName}");
                    break;
                default:
                    break;
            }
            
        }
    } 
}