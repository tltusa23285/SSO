using Actors;
using Godot;
using System;

namespace Commands
{
    /// <summary>
    /// Any command/action that can exist on a hotbar
    /// </summary>
    public abstract partial class BarAction : Resource, ICommand
    {
        public delegate void CommandExecutedEventHandler(bool success, BarAction comm, string msg);
        public delegate void CooldownEventHandler(float time);
        public delegate void CommandFlaggedEventHandler(bool isFlagged);

        public event CommandExecutedEventHandler CommandExecuted;
        public event CooldownEventHandler CooldownStart;
        public event CooldownEventHandler CooldownEnd;
        public event CommandFlaggedEventHandler CommandFlagChange;


        [Export] public string Name { get; set; }
        [Export] public bool OnGcd { get; set; } = true;
        [Export] public float Cd { get; set; } = 0;
        [Export] public Texture2D Icon { get; set; } = null;

        public Actor Source {  get; protected set; }
        private bool _flagged;
        public bool Flagged
        {
            get { return _flagged; }
            set
            {
                _flagged = value;
                CommandFlagChange?.Invoke(_flagged);
            }
        }
        [Export] public bool FlaggedOnly = false;

        public bool OnCD => CdTimer != null;
        public double CdLeft => CdTimer != null ? CdTimer.TimeLeft : 1;
        [NonSerialized] private SceneTreeTimer CdTimer;

        public bool Initialize(Actor source)
        {
            Source = source;
            return ConstructCommand();
        }
        protected abstract bool ConstructCommand();

        public bool Execute()
        {
            if (OnCD)
            {
                CommandExecuted?.Invoke(false, this, $"On Cooldown {CdLeft}");
                return false;
            }
            bool success = ExecuteCommand();
            CommandExecuted?.Invoke(success, this, success ? string.Empty : "Failed to execute");
            return success;
        }

        public void SetCooldown(in float duration)
        {
            CdTimer = Source.GetTree().CreateTimer(duration);
            CdTimer.Timeout += EndCd;
            CooldownStart?.Invoke(duration);
        }

        public void EndCd()
        {
            CdTimer.Timeout -= EndCd;
            CdTimer = null;
            CooldownEnd?.Invoke(0);
        }

        protected abstract bool ExecuteCommand();
    } 
}