using Data;
using Godot;

public partial class ActionBookUI : Control
{
    [Export] private Container Parent;
	[Export] private PackedScene ActionButtonPrefab;

    public void Initialize(ActionBook book)
    {
        int i = 0;
		foreach (var item in book.Actions)
		{
            ActionButton b = ActionButtonPrefab.Instantiate<ActionButton>();
            b.CanReceiveDrops = false;
            Parent.AddChild(b);
            b.SetCommand(item);
            b.SetButtonName($"Spellbook_{i++}");
        }
    }
}
