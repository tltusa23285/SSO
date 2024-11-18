using Actors;
using Godot;

namespace Actors
{
	public class ActorStateContainer
	{
		private readonly Actor Actor;
		public ActorStateContainer(Actor actor)
		{
			Actor = actor;
		}

		private ACTOR_ACTION_STATE _cActionState;

		public ACTOR_ACTION_STATE ActionState
		{
			get { return _cActionState; }
			set
			{
				if (_cActionState == value) return;
                //GD.Print($"Acton State Change {_cActionState} -> {value}");
                ACTOR_ACTION_STATE p = _cActionState;
				_cActionState = value;
				Actor.Events.FireActionChange(
					new EventArgs.ActorStateChangeArgs<ACTOR_ACTION_STATE>(Actor, p , _cActionState));
			}
		}

		private ACTOR_MOVE_STATE _cMoveState;

		public ACTOR_MOVE_STATE MoveState
		{
			get { return _cMoveState; }
			set
			{
				if (_cMoveState == value) return;
                //GD.Print($"Move State Change {_cMoveState} -> {value}");
                ACTOR_MOVE_STATE p = _cMoveState;
				_cMoveState = value;
                Actor.Events.FireMoveChange(
                    new EventArgs.ActorStateChangeArgs<ACTOR_MOVE_STATE>(Actor, p, _cMoveState));
            }
		}
	}
}
