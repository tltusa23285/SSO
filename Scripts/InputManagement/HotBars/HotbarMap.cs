using Godot;
using Godot.Collections;
using System.Collections.Generic;

namespace Inputs
{
    public partial class HotbarMap : Node
    {
        private const string MAP_PATH = "user://hotbarmap.dat";
        private const string EMPTY_SLOT = "NONE";
        public static HotbarMap Instance;

        private Dictionary Map;

        public bool GetActionForInput(in string input, out string action)
        {
            action = Map.TryGetValue(input, out Variant v) ? v.AsString() : null;
            return action != null;
        }

        public bool GetInputsForAction(in string action, out string[] inputs)
        {
            inputs = null;
            HashSet<string> result = new HashSet<string>();
            foreach (var item in Map)
            {
                if (item.Value.AsString() == action) result.Add(item.Key.AsString());
            }
            if (result.Count < 1) return false;
            inputs = new string[result.Count];
            result.CopyTo(inputs);
            return true;
        }

        public override void _Ready()
        {
            Instance = this;

            Map = new Dictionary();

            foreach (var item in KeyMap.GetHotbarInputs())
            {
                Map.Add(item, EMPTY_SLOT);
            }

            LoadMap();
        }

        private void LoadMap()
        {
            if (!FileAccess.FileExists(MAP_PATH))
            {
                SaveMap();
                return;
            }

            FileAccess file = FileAccess.Open(MAP_PATH, FileAccess.ModeFlags.Read);
            Dictionary temp = file.GetVar(true).AsGodotDictionary();
            foreach (StringName item in Map.Keys)
            {
                if (temp.ContainsKey(item))
                {
                    Map[item] = temp[item];
                }
            }

        }

        private void SaveMap()
        {
            FileAccess file = FileAccess.Open(MAP_PATH, FileAccess.ModeFlags.Write);
            file.StoreVar(Map, true);
            file.Close();
        }

        public void SetSlot(in string slot, in string action)
        {
            if (!Map.ContainsKey(slot)) return;
            Map[slot] = string.IsNullOrEmpty(action) ? EMPTY_SLOT : action;
            SaveMap();
        }
    } 
}
