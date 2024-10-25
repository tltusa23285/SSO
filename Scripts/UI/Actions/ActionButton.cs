using Commands;
using Godot;
using Inputs;

public partial class ActionButton : Button
{
    [Export] public TextureRect IconRect;
    [Export] public TextureRect PressedRect;
    [Export] public TextureRect FlagRect;
    [Export] public Label Key;
    [Export] public Timer Timer;
    [Export] public Label CdTime;
    [Export] public TextureProgressBar CDFill;
    public bool CanReceiveDrops = false;

    private string InputName = null;

    private BarAction CombatCommand;
    public bool UsesGCD { get; private set; }

    public override void _Ready()
    {
        this.ButtonDown += OnButtonDown;
        this.ButtonUp += OnButtonUp;
    }

    /// <summary>
    /// Keys with modifiers, ie shift, trigger properly on pressed
    /// They do not obey modifiers on release, so we need a flag to ensure this button was properly presseed to avoid eating input
    /// </summary>
    private bool WasPressed = false;

    public async void StartCdTimer(float duration) 
    {
        Timer.OneShot = true;
        Timer.Start(duration);
        CDFill.MaxValue = duration;
        while (!Timer.IsStopped())
        {
            CDFill.Value = Timer.TimeLeft;
            CdTime.Text = Timer.TimeLeft >= 1 ? Mathf.FloorToInt(Timer.TimeLeft).ToString() : Timer.TimeLeft.ToString("0.0");
            await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        }
        CdTime.Text = string.Empty;
        CDFill.Value = 0;
    }

    private void CooldownEnded(float _)
    {
        Timer.Stop();
    }

    public override void _Input(InputEvent @event)
    {
        if (string.IsNullOrEmpty(InputName)) return;
        if (@event.IsActionPressed(InputName))
        {
            WasPressed = true;
            OnButtonDown();
            GetTree().Root.SetInputAsHandled();
        }
        if (@event.IsActionReleased(InputName))
        {
            if (WasPressed)
            {
                WasPressed = false;
                OnButtonUp();
                GetTree().Root.SetInputAsHandled();
            }
        }
    }

    #region Data assignment
    /// <summary>
    /// Sets the button id/nanm, and the input mapped to this button
    /// </summary>
    /// <param name="input"></param>
    public void SetButtonName(string name)
    {
        this.Name = string.IsNullOrEmpty(name) ? "NULL_NAME" : name;
    }

    public void SetInput(string input)
    {
        this.InputName = input;
        Key.Text = KeyMap.Instance.GetKeyLabelForInput(InputName, out string output) ? output : string.Empty;
    }

    /// <summary>
    /// Sets the command this button will trigger
    /// </summary>
    /// <param name="comm"></param>
    public void SetCommand(BarAction comm)
    {
        if (CombatCommand != null)
        {
            SetCommandFlag(false);
            CombatCommand.CooldownStart -= StartCdTimer;
            CombatCommand.CooldownEnd -= CooldownEnded;
            CombatCommand.CommandFlagChange -= SetCommandFlag;
        }

        SetIcon(null);

        CombatCommand = comm;

        UsesGCD = CombatCommand != null ? CombatCommand.OnGcd : false;

        CdTime.Visible = CombatCommand == null ? false : (!CombatCommand.OnGcd || CombatCommand.Cd > Actors.Actor.GCD);

        if (CombatCommand == null) return;

        SetIcon(comm.Icon);
        CombatCommand.CooldownStart += StartCdTimer;
        CombatCommand.CooldownEnd += CooldownEnded;
        CombatCommand.CommandFlagChange += SetCommandFlag;
    }

    /// <summary>
    /// Flushes command data from this button
    /// TODO : currently this is equivalent to doing SetCommand(null), consider if the distinction needs to be made
    /// </summary>
    public void ClearData()
    {
        SetCommand(null);
    }

    private void SetCommandFlag(bool isFlagged)
    {
        FlagRect.Visible = isFlagged;
    }

    /// <summary>
    /// Fills icon rect with the given texture
    /// </summary>
    /// <param name="tex"></param>
    private void SetIcon(in Texture2D tex)
    {
        IconRect.Texture = tex;
        IconRect.Visible = tex != null;
    } 
    #endregion

    #region Button up/down vfx
    private void OnButtonDown()
    {
        PressedRect.Visible = true;
    }

    private void OnButtonUp()
    {
        PressedRect.Visible = false;
        CombatCommand?.Execute();
    }

    #endregion
    #region Drag and Drop
    public override Variant _GetDragData(Vector2 atPosition)
    {
        // dont allow drag if button has no command, or is currently on cd
        if (this.CombatCommand == null || !this.Timer.IsStopped()) return default;
        PressedRect.Visible = false;
        SetDragPreview(MakePreview());

        return this;
    }

    private Control MakePreview()
    {
        return (this.Duplicate() as ActionButton);
    }

    public override bool _CanDropData(Vector2 atPosition, Variant data)
    {
        GodotObject obj = data.AsGodotObject();
        if (!this.CanReceiveDrops) return false;
        return (this.Timer.IsStopped()) && // dont allow drop if we are on cd
                (obj is ActionButton) &&   // or if incoming is not an action button
                (obj != this);             // or if we are trying to drop ourselves
    }

    public override void _DropData(Vector2 atPosition, Variant data)
    {
        ActionButton other = data.As<ActionButton>();
        BarAction c_com = this.CombatCommand;
        
        this.SetCommand(other.CombatCommand);
        HotbarMap.Instance.SetSlot(this.Name, this.CombatCommand != null ? this.CombatCommand.Name : null);

        if (other.CanReceiveDrops)
        {
            other.SetCommand(c_com);
            HotbarMap.Instance.SetSlot(other.Name, c_com != null ? c_com.Name : null);
        }
    } 
    #endregion
}