using Godot;
using System;

public partial class ActionButtonClearField : Control
{
    public override bool _CanDropData(Vector2 atPosition, Variant data)
    {
        GodotObject obj = data.AsGodotObject();
        return  (obj is ActionButton) && // or if incoming is not an action button
                (obj != this);                          // or if we are trying to drop ourselves
    }
    public override void _DropData(Vector2 atPosition, Variant data)
    {
        ActionButton other = data.As<ActionButton>();

        if (other.CanReceiveDrops)
        {
            other.SetCommand(null);
            Inputs.HotbarMap.Instance.SetSlot(other.Name, null);
        }
    }
}
