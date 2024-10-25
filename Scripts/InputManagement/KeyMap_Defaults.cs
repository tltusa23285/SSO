using Godot;
using System.Collections.Generic;

namespace Inputs
{
    /// <summary>
    /// Generate default events for new KeyMap
    /// </summary>
    public partial class KeyMap : Node
    {
        public const string Jump = nameof(Jump);
        public const string Forward = nameof(Forward);
        public const string Backward = nameof(Backward);
        public const string StrafeLeft = nameof(StrafeLeft);
        public const string StrafeRight = nameof(StrafeRight);
        public const string LookPlayerControl = nameof(LookPlayerControl);
        public const string SelectEntity = nameof(SelectEntity);

        public static IEnumerable<string> GetSystemInputs()
        {
            return new string[]
            {

            };
        }

        public static IEnumerable<string> GetGeneralInputs()
        {
            return new string[]
            {
            Jump,Forward, Backward, StrafeLeft, StrafeRight,
            LookPlayerControl, SelectEntity
            };
        }

        public static IEnumerable<string> GetHotbarInputs()
        {
            foreach (var item in HotbarUtilities.GetHotbarInputs())
            {
                if (HotbarUtilities.ParseHotbarAction(item, out ushort bar, out ushort slot))
                {
                    yield return item;
                }
            }
        }
    } 
}