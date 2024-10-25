using Godot;
using Godot.Collections;

namespace Inputs
{
    public partial class KeyMap : Node
	{
		public delegate void KeyRebindEventHandler(string input, InputEvent @event);
		public KeyRebindEventHandler OnKeyRebind;

		private const string MAP_PATH = "user://keymaps.dat";
		public static KeyMap Instance;

		private Dictionary Map = new();

		public override void _Ready()
		{
			Instance = this;

			foreach (var item in GetSystemInputs()) AddInput(item);
			foreach (var item in GetGeneralInputs()) AddInput(item);
			foreach (var item in GetHotbarInputs()) AddInput(item);

			foreach (var item in InputMap.GetActions())
			{
				if (InputMap.ActionGetEvents(item).Count > 0)
				{
					Map[item] = InputMap.ActionGetEvents(item)[0];
				}
			}
			LoadMap();
		}
		private void AddInput(in string name, in InputEvent @event = default)
		{
			InputMap.AddAction(name);
			Map.Add(name, @event);
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
					if (Map[item].AsGodotObject() == null) continue; // no event assigned, do nothing
					InputMap.ActionEraseEvents(item);
					InputMap.ActionAddEvent(item, Map[item].AsGodotObject() as InputEvent);
				}
			}
		}

		public bool RebindEvent(in string input, in InputEvent @event)
		{
			InputMap.ActionEraseEvents(input);
			InputMap.ActionAddEvent(input, @event);

			Map[input] = @event;
			SaveMap();

			OnKeyRebind?.Invoke(input, @event);
			return true;
		}

		public bool UnbindEvent(in string input)
		{
			InputMap.ActionEraseEvents(input);
			Map[input] = default;
			SaveMap();
			OnKeyRebind?.Invoke(input, null);
			return true;
		}

		private void SaveMap()
		{
			FileAccess file = FileAccess.Open(MAP_PATH, FileAccess.ModeFlags.Write);
			file.StoreVar(Map, true);
			file.Close();
		}

		public bool GetKeyLabelForInput(in string input, out string output)
		{
			output = null;
			if (!Map.TryGetValue(input, out Variant iv)) return false;
			if (iv.AsGodotObject() is not InputEvent) return false;
			output = (iv.AsGodotObject() as InputEvent).AsText();
			return true;
		}
	} 
}