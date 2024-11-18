using EventArgs;
using Godot;
using System;

namespace Actors
{
	public class ActorCastingContainer
	{
		private readonly Actor Source;
		private readonly double SlideBuffer;
		public ActorCastingContainer(Actor source, in double slideBuffer)
		{
			Source = source;
			SlideBuffer = slideBuffer;

		}

		public bool IsCasting => Source.States.ActionState == ACTOR_ACTION_STATE.Casting;

        private double CastTimer;

        public bool TryStartCasting(in float castTime)
		{
			if (Source.States.ActionState != ACTOR_ACTION_STATE.Free) return false;
            if (Source.States.MoveState != ACTOR_MOVE_STATE.Standing) return false;

            Source.States.ActionState = ACTOR_ACTION_STATE.Casting;
			Source.Events.MoveChange += CheckMoveState;
			CastTimer = castTime;
			Source.EmitSignal(Actor.SignalName.CastingStatus, true, castTime);

            return true;
        }

        /// <summary>
        /// Called when a cast should naturally finish, ie full cast time and effects applied
        /// </summary>
        public void StopCast()
        {
            if (Source.States.ActionState == ACTOR_ACTION_STATE.Casting)
                Source.States.ActionState = ACTOR_ACTION_STATE.Free;

            Source.Events.MoveChange -= CheckMoveState;
            Source.EmitSignal(Actor.SignalName.CastingStatus, false, 0);
        }

        private void CheckMoveState(ActorStateChangeArgs<ACTOR_MOVE_STATE> args)
        {
            if (args.Next != ACTOR_MOVE_STATE.Standing)
            {
                if (CastTimer > SlideBuffer) StopCast();
            }
        }

        public void Process(in double delta)
        {
            CastTimer -= delta;
        }
    } 
}
