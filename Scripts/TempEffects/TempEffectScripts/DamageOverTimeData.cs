using Actors;
using Godot;
using System;
using System.Collections.Generic;

namespace TemporaryEffects
{
    [GlobalClass]
    public partial class DamageOverTimeData : TemporaryEffectData
	{
        [Export] public int PotencyPerTick;
        [Export] public float TimePerTick;

        protected override bool CanApply(in Actor source, in Actor target)
        {
            return true;
        }

        protected override TemporaryEffect GetEffect(in Actor source, in Actor target)
        {
            return new DamageOverTime(this);
        }

        public class DamageOverTime : TemporaryEffect
        {
            private int DamagePerTick = 0;
            private DamageOverTimeData DotData => Data as DamageOverTimeData;

            public DamageOverTime(DamageOverTimeData dat) : base(dat)
            {
            }

            protected override void OnApply()
            {
                DamagePerTick = Mathf.FloorToInt(Source.Stats.AttackPower * (DotData.PotencyPerTick / 100f));
            }

            protected override void OnExpire() {}

            protected override void OnRefreshed()
            {
                DamagePerTick = Mathf.FloorToInt(Source.Stats.AttackPower * (DotData.PotencyPerTick / 100f));
            }

            private float TickTime;
            public override void UpdateTime(float delta)
            {
                base.UpdateTime(delta);
                TickTime += delta;
                if (TickTime >= DotData.TimePerTick)
                {
                    TickTime = 0;
                    Target.Health.Damage(DamagePerTick);
                }
            }
        }
    }
}