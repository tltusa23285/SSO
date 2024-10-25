using Actors;
using Commands;
using Data;
using Godot;
using Inputs;

public partial class PlayerUI : Control
{
	[Export] private Actor Player;

    public void Intialize()
    {
		InitHotbars();
    }

	#region Hotbar Management

	[Export] private HotbarContainer[] Bars;
	[Export] private ActionBookUI BookUI;

	private void InitHotbars()
    {
        foreach (var bar in Bars) { bar.Initialize(); }
		foreach (var item in HotbarUtilities.GetHotbarInputs())
		{
            if (HotbarUtilities.ParseHotbarAction(item, out ushort bar, out ushort slot))
            {
				Bars[bar].GetButton(slot).SetButtonName(item);
                Bars[bar].GetButton(slot).SetInput(item);
            }
		}

		KeyMap.Instance.OnKeyRebind += OnHotbarRebind;
	}

	private void OnHotbarRebind(string input, InputEvent _)
	{
		if (HotbarUtilities.ParseHotbarAction(input, out ushort bar, out ushort slot))
        {
            Bars[bar].GetButton(slot).SetInput(input);
        }
	}

	public void FillActionBook(ActionBook book)
	{
		BookUI.Initialize(book);
	}

	public void AssignAction(in string input, in BarAction comm)
	{
		if (HotbarUtilities.ParseHotbarAction(input, out ushort bar, out ushort slot))
		{
			Bars[bar].GetButton(slot).SetCommand(comm);
		}
	}

	public void ClearAction(in ushort bar, in ushort slot)
	{
		Bars[bar].GetButton(slot).ClearData();
	}
	#endregion
}
