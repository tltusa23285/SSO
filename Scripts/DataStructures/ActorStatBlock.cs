using Godot;
using StatModifiers;

namespace Data
{
    using GDict = Godot.Collections.Dictionary;
    using SDict = System.Collections.Generic.Dictionary<string, Stat>;
    /// <summary>
    /// Aggregates stats and modifiers for an actor
    /// </summary>
    public partial class ActorStatBlock : Node
    {
        [Export] public string DisplayName;
        [Export] public ACTOR_CATEGORY Category;
        [Export] public int Level = 1;

        [ExportGroup("Stat Blocks")]
        [Export] private GDict BaseStats = new GDict()
        {
            { Stat.Vit, 10 },
            { Stat.Dex, 10 },
            { Stat.Str, 10 },
            { Stat.Int, 10 },
        };

        [Export]
        private GDict StatsPerLevel = new GDict()
        {
            { Stat.Vit, 0 },
            { Stat.Dex, 0 },
            { Stat.Str, 0 },
            { Stat.Int, 0 },
        };

        private SDict Stats;

        public override void _Ready()
        {
            Stats = new SDict();
            int val;
            string key;
            foreach (var item in BaseStats)
            {
                key = item.Key.AsString();
                val = item.Value.AsInt32();
                if (StatsPerLevel.TryGetValue(key, out Variant v))
                {
                    val += ((Level - 1) * v.AsInt32());
                }
                AddStat(key, val);
            }
        }

        #region Stat Accessors
        public bool GetStat(in string statKey, out int value)
        {
            value = default;
            if (!Stats.TryGetValue(statKey, out Stat stat))
            {
                GD.PushError($"Tried to access unknown stat {statKey}");
                return false;
            }
            value = stat.Value;
            return true;
        }

        public bool AddStatModifier(in string statKey, in string sourceID, IStatModifier mod)
        {
            if (!Stats.TryGetValue(statKey, out Stat stat))
            {
                GD.PushError($"Tried to access unknown stat {statKey}");
                return false;
            }
            return stat.AddModifier(sourceID, mod);
        }

        public bool RemoveStatModifier(in string statKey, in string sourceID)
        {
            if (!Stats.TryGetValue(statKey, out Stat stat))
            {
                GD.PushError($"Tried to access unknown stat {statKey}");
                return false;
            }
            return stat.RemoveModifier(sourceID);
        }

        public bool AddStat(in string statKey, in int baseValue)
        {
            if (Stats.ContainsKey(statKey))
            {
                GD.PushError($"Tried to add duplicate stat {statKey}");
                return false;
            }
            Stats.Add(statKey, new Stat(baseValue));
            return true;
        }
        public bool RemoveStat(in string statKey)
        {
            if (!Stats.ContainsKey(statKey))
            {
                GD.PushError($"Tried to remove unknown stat {statKey}");
                return false;
            }
            Stats.Remove(statKey);
            return true;
        }
        #endregion

        #region StatCalcs
        public virtual int MaxHP => (GetStat(Stat.Vit, out int v) ? v * 10 : 100);
        public virtual int AttackPower => (GetStat(Stat.Str, out int v) ? v : 1);
        #endregion
    } 
}