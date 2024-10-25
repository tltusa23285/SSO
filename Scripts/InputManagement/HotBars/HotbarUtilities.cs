using System.Collections.Generic;

namespace Inputs
{
    public static class HotbarUtilities
    {
        public const ushort BAR_COUNT = 2;
        public const ushort BAR_SLOTS = 12;
        public static HashSet<string> GetHotbarInputs()
        {
            HashSet<string> result = new HashSet<string>();
            for (ushort i = 0; i < BAR_COUNT; i++)
            {
                for (ushort j = 0; j < BAR_SLOTS; j++)
                {
                    result.Add(GenHotbarActionName(i, j));
                }
            }
            return result;
        }

        public static string GenHotbarActionName(in ushort bar, in ushort slot)
        {
            return $"Hotbar_{bar}_{slot}";
        }

        public static bool ParseHotbarAction(in string name, out ushort bar, out ushort slot)
        {
            bar = default; slot = default;
            string[] to_parse = name.Split('_');
            if (to_parse.Length != 3) return false;
            if (!ushort.TryParse(to_parse[1], out bar)) return false;
            if (!ushort.TryParse(to_parse[2], out slot)) return false;
            return true;
        }

        //public static bool GetDefaultInputEvent(in ushort bar, in ushort slot, out InputEvent input)
        //{
        //    input = null;
        //    InputEventKey key;
        //    switch (bar)
        //    {
        //        case 0: if (GetSlotInput(slot, out key)) input = key; break;
        //        case 1:
        //            if (GetSlotInput(slot, out key))
        //            {
        //                key.ShiftPressed = true;
        //                input = key;
        //            }
        //            break;
        //        default: break;
        //    }
        //    return input != null;
        //}

        //private static bool GetSlotInput(in ushort slot, out InputEventKey key)
        //{
        //    key = new InputEventKey();
        //    switch (slot)
        //    {
        //        case 0: key.Keycode = Key.Key1; break;
        //        case 1: key.Keycode = Key.Q; break;
        //        case 2: key.Keycode = Key.E; break;
        //        case 3: key.Keycode = Key.R; break;
        //        default: return false;
        //    }
        //    return true;
        //}
    } 
}