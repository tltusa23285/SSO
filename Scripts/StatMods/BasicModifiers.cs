using Godot;
using System;

namespace StatModifiers
{
    public struct FlatMod : IStatModifier
    {
        IStatModifier.MOD_PRIORITY IStatModifier.Type => IStatModifier.MOD_PRIORITY.Flat;
        private readonly int Value;
        public FlatMod(int val)
        {
            Value = val;
        }

        void IStatModifier.ModifyValue(ref int v)
        {
            v += Value;
        }
    }

    public struct AddiMod : IStatModifier
    {
        IStatModifier.MOD_PRIORITY IStatModifier.Type => IStatModifier.MOD_PRIORITY.Additive;
        private readonly float Value;
        public AddiMod(int val)
        {
            Value = val / 100f;
        }

        void IStatModifier.ModifyValue(ref int v)
        {
            v += Mathf.FloorToInt(v * Value);
        }
    }

    public struct MultiMod : IStatModifier
    {
        IStatModifier.MOD_PRIORITY IStatModifier.Type => IStatModifier.MOD_PRIORITY.Multiplicative;
        private readonly float Value;
        public MultiMod(int val)
        {
            Value = val / 100f;
        }

        void IStatModifier.ModifyValue(ref int v)
        {
            v = Mathf.FloorToInt(v * Value);
        }
    }
}