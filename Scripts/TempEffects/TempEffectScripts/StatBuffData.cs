using Godot;
using System;
using StatModifiers;
using Actors;

namespace TemporaryEffects
{
    [GlobalClass]
    public partial class StatBuffData : TemporaryEffectData
    {
        [Export] public string StatToMod;
        [Export] public StatBuff.MODTYPE ModType;
        [Export] public int ModValue;

        protected override bool CanApply(in Actor source, in Actor target)
        {
            return target.Stats.GetStat(StatToMod, out _);
        }

        protected override TemporaryEffect GetEffect(in Actor source, in Actor target)
        {
            return new StatBuff(this);
        }

        public class StatBuff : TemporaryEffect
        {
            private StatBuffData BuffData => Data as StatBuffData;

            public enum MODTYPE
            {
                Flat, Addi, Multi
            }

            public StatBuff(StatBuffData dat) : base(dat)
            {
            }

            protected override void OnApply()
            {
                IStatModifier mod;
                switch (BuffData.ModType)
                {
                    case MODTYPE.Flat:  mod = new FlatMod(BuffData.ModValue); break;
                    case MODTYPE.Addi:  mod = new AddiMod(BuffData.ModValue); break;
                    case MODTYPE.Multi: mod = new MultiMod(BuffData.ModValue); break;
                    default: 
                        throw new NotImplementedException();
                }

                Target.Stats.AddStatModifier(BuffData.StatToMod, Data.EffectId, mod);
            }

            protected override void OnExpire()
            {
                Target.Stats.RemoveStatModifier(BuffData.StatToMod, Data.EffectId);
            }

            protected override void OnRefreshed(){}
        }
    }
}