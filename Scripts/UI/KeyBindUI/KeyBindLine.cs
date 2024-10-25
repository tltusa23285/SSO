using Godot;
using Inputs;

public partial class KeyBindLine : HBoxContainer
{
    [Export] private Label Label;
    [Export] private Button Input;

    public string InputName
    {
        get { return Label.Text;}
        set { Label.Text = value; }
    }
    public override void _Ready()
    {
        SetProcessUnhandledKeyInput(false);
    }
    public void Init(string input)
    {
        InputName = input;
        Input.Toggled += OnInputToggle;
        DisplayCurrent();
    }

    private void OnInputToggle(bool toggled)
    {
        SetProcessUnhandledKeyInput(toggled);
        if (toggled)
        {
            Input.Text = "<Press a Key>";
            ReleaseFocus();
        }
        else
        {
            DisplayCurrent();
        }
    }

    public void ClearBind()
    {
        KeyMap.Instance.UnbindEvent(InputName);
        Input.Text = string.Empty;
    }

    public override void _UnhandledKeyInput(InputEvent @event)
    {
        Remap(@event);
        Input.ButtonPressed = false;
    }

    private void Remap(InputEvent @event)
    {
        KeyMap.Instance.RebindEvent(InputName, @event);
        Input.Text = @event.AsText();
    }

    private void DisplayCurrent()
    {
        KeyMap.Instance.GetKeyLabelForInput(InputName, out string key);
        Input.Text = key;
    }
}
