using Actors;
using Godot;
using System;

namespace TemporaryEffects
{
    /// <summary>
    /// Empty temporary effect, generally only used for action-displays
    /// Example : one action enables usage of another anytime within the next 30 seconds, we use this flagging effect as a trigger for the flagging conditional
    /// </summary>
    [GlobalClass]
    public partial class FlaggingData : TemporaryEffectData
	{
        protected override bool CanApply(in Actor source, in Actor target)
        {
            return true;
        }

        protected override TemporaryEffect GetEffect(in Actor source, in Actor target)
        {
            return new FlaggingEffect (this);
        }

        private class FlaggingEffect : TemporaryEffect
        {
            public FlaggingEffect(TemporaryEffectData dat) : base(dat) {}

            protected override void OnApply() {}

            protected override void OnExpire() {}

            protected override void OnRefreshed() {}
        }
    } 
}
