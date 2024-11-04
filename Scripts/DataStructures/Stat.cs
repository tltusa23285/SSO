using Godot;
using StatModifiers;
using System;
using System.Collections.Generic;

namespace Data
{
    public class Stat
    {
        #region Globals

        #region Stat Strings
        public const string MaxHP = nameof(MaxHP);
        public const string Atk = nameof(Atk);
        public const string Def = nameof(Def);
        #endregion
        #endregion

        #region Instanced
        public readonly int Base;
        public int Value { get; private set; }
        private Dictionary<string, IStatModifier> Modifiers;
        public Stat(int baseValue)
        {
            this.Base = baseValue;
            Modifiers = new Dictionary<string, IStatModifier>();
            Value = baseValue;
        }

        public bool AddModifier(in string id, IStatModifier mod)
        {
            if (Modifiers.ContainsKey(id))
            {
                GD.PushError($"Duplicate modifier id {id}");
                return false;
            }

            Modifiers.Add(id, mod);
            UpdateValue();
            return true;
        }

        public bool RemoveModifier(in string id)
        {
            if (!Modifiers.ContainsKey(id))
            {
                GD.PushError($"Tried to remove untracked modifier {id}");
                return false;
            }
            Modifiers.Remove(id);
            UpdateValue();
            return true;
        }

        private void UpdateValue()
        {
            int v = Base;
            IStatModifier[] mods = new IStatModifier[Modifiers.Count];
            Modifiers.Values.CopyTo(mods, 0);
            Array.Sort(mods,
                delegate (IStatModifier a, IStatModifier b)
                {
                    return a.Type.CompareTo(b.Type);
                }
            );
            foreach (var item in mods)
            {
                item.ModifyValue(ref v);
            }
            Value = v;
        }
        #endregion
    } 
}