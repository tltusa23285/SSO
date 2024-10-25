using Godot;
using Inputs;
using System.Collections.Generic;

public partial class HotbarContainer : Container
{
	[Export] private int ColumnCount = 12;
	[Export] private GridContainer Parent;
	[Export] private PackedScene ActionButtonPrefab;
	private ActionButton[] AButtons;

    public void Initialize()
    {
		Parent.Columns = ColumnCount;
		AButtons = new ActionButton[HotbarUtilities.BAR_SLOTS];
		for (int i = 0; i < HotbarUtilities.BAR_SLOTS; i++)
		{
			ActionButton b = ActionButtonPrefab.Instantiate<ActionButton>();
			b.CanReceiveDrops = true;
			Parent.AddChild(b);
			AButtons[i] = b;
		}
    }

	public IEnumerable<ActionButton> Buttons
	{
		get
		{
			foreach (var item in AButtons)
			{
				yield return item;
			}
		}
	}

	public ActionButton GetButton(ushort slot)
	{
		return AButtons[slot];
	}
}
