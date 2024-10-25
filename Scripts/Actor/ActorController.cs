using Godot;
using System;

namespace Actors
{
	public abstract partial class ActorController : Node
	{
		[Export] public Actor C_Actor { get; protected set; }

        #region Node Overrides
        public override void _Ready()
        {
            C_Actor.TargetChanged += ActorTargetChanged;
            C_Actor.ActorDied += ActorDied;
            OnReady();
        }

        protected abstract void OnReady();

        public override void _Process(double delta)
        {
            if (C_Actor == null) return;
            OnProcess(delta);
        }
        public override void _PhysicsProcess(double delta)
        {
            if (C_Actor == null) return;
            OnPhysicsProcess(delta);
        }

        protected abstract void OnProcess(in double delta);
        protected abstract void OnPhysicsProcess(in double delta);
        #endregion

        #region Actor Events
        protected abstract void ActorTargetChanged();
        protected abstract void ActorDied(); 
        #endregion
    }
}