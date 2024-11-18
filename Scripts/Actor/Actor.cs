using Data;
using Godot;
using System;
using TemporaryEffects;

namespace Actors
{
    public partial class Actor : CharacterBody3D, ISelectable
    {
        public const float GCD = 2.0f;
        public delegate void LastCommandEventHandler(string comm);
        public LastCommandEventHandler CommandUsed;


        private string _lastCommand;

        public double ActionLimitTimer = 0;

        public string NextComboAction
        {
            get { return _lastCommand; }
            set
            {
                _lastCommand = value;
                CommandUsed?.Invoke(_lastCommand);
            }
        }

        private double Gravity;

        public ActorEventContainer Events { get; private set; }
        public ActorStateContainer States { get; private set; }

        #region Casting
        [Signal]
        public delegate void CastingStatusEventHandler(bool isCasting, float castTime);

        [ExportGroup("Cast Settings")]
        [Export] public double SlideCastBuffer = 0.3f;
        public ActorCastingContainer Casting { get; private set; }
        #endregion


        public override void _Ready()
        {
            Gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsDouble();
            Health.Setup(Stats.MaxHP);
            Health.HealthChange += HealthUpdate;
            GCon.Setup(this);
            ActionBook.LoadBook(this);

            States = new ActorStateContainer(this);
            Events = new ActorEventContainer(this);
            Casting = new ActorCastingContainer(this, SlideCastBuffer);
        }

        public override void _Process(double delta)
        {
            ActionLimitTimer -= delta;
            Casting.Process(delta);
        }

        #region Statistics
        [ExportGroup("Stats")]
        [Export] public ActorStatBlock Stats { get; private set; }
        [Export] public ActionBook ActionBook { get; private set; }
        [Export] public TemporaryEffectContainer TempEffects { get; private set; }

        #region Health
        [Signal] public delegate void ActorDiedEventHandler();

        [ExportSubgroup("Health")]
        [Export] public Health Health { get; private set; }
        private void HealthUpdate(int current, int max)
        {
            if (current <= 0)
            {
                OnDeath();
            }
        }

        protected virtual void OnDeath()
        {
            EmitSignal(SignalName.ActorDied);
            this.QueueFree();
        }
        #endregion
        #endregion

        #region Graphics
        [ExportGroup("Graphic Settings")]
        [Export] public ActorGraphicController GCon;
        #endregion

        #region Targeting
        [Signal] public delegate void TargetChangedEventHandler();
        public Actor CurrentTarget { get; private set; }
        public void SetTarget(Actor actor)
        {
            if (CurrentTarget == actor) return;

            if (CurrentTarget != null) CurrentTarget.ActorDied -= ClearTargetOnDeath;

            CurrentTarget = actor;
            if (CurrentTarget != null) CurrentTarget.ActorDied += ClearTargetOnDeath;

            EmitSignal(SignalName.TargetChanged);
        }

        private void ClearTargetOnDeath()
        {
            SetTarget(null);
        }
        #endregion

        #region Movement
        [ExportGroup("Movement Settings")]
        [Export] public float Speed = 5;

        public Vector3 Facing => MoveDir.Scale(y:0).Normalized();
        private Vector3 MoveDir;
        public void SetMoveDirection(in Vector3 dir)
        {
            SetMoveDirection(dir, Speed);
        }
        /// <summary>
        /// Set move directio while overriding speed of the actor
        /// </summary>
        public void SetMoveDirection(in Vector3 dir, in float speed)
        {
            MoveDir.X = (dir.Normalized() * speed).X;
            MoveDir.Z = (dir.Normalized() * speed).Z;
        }

        public void TryJump(in float height)
        {
            if (!this.IsOnFloor()) return;
            this.MoveDir = this.Velocity + (Vector3.Up * height);
        }

        private void UpdateGravity(in double delta)
        {
            if (!IsOnFloor())
            {
                MoveDir.Y = this.Velocity.Y - (float)(Gravity * delta);
            }
        }

        public override void _PhysicsProcess(double delta)
        {
            UpdateGravity(delta);
            this.Velocity = MoveDir;
            MoveAndSlide();
            UpdateMoveState();
        }

        private void UpdateMoveState()
        {
            if (this.Velocity == Vector3.Zero) States.MoveState = ACTOR_MOVE_STATE.Standing;
            else if (this.Velocity.Y != 0) States.MoveState = ACTOR_MOVE_STATE.Falling;
            else States.MoveState = ACTOR_MOVE_STATE.Moving;
        }

        #endregion

        #region ISelectable
        string ISelectable.Name => this.Name;
        #endregion
    }
}