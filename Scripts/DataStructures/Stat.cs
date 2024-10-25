using Godot;
using StatModifiers;
using System;
using System.Collections.Generic;

namespace Data
{
    public class Stat
    {
        #region Globals
        public static string[] CoreStats => new string[] { Vit, Str, Dex, Int };

        #region Stat Strings
        public const string Vit = nameof(Vit);
        public const string Str = nameof(Str);
        public const string Dex = nameof(Dex);
        public const string Int = nameof(Int);
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