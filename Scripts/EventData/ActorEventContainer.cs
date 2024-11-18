using Godot;
using System;
using EventArgs;

namespace Actors
{
	/// <summary>
	/// Holds and handles all outgoing events related to an actor
	/// </summary>
	public class ActorEventContainer
	{
		public delegate void ActorEventHandler<T>(T args) where T : ActorEventArgs;

		private readonly Actor Source;
		public ActorEventContainer(Actor source)
		{
			Source = source;
		}

		public event ActorEventHandler<DamageEventArgs> Damaged;
		public void FireActorDamaged(DamageEventArgs args) => Damaged?.Invoke(args);


		public event ActorEventHandler<ActorStateChangeArgs<ACTOR_ACTION_STATE>> ActionChange;
		public void FireActionChange(ActorStateChangeArgs<ACTOR_ACTION_STATE> args) => ActionChange?.Invoke(args);

        public event ActorEventHandler<ActorStateChangeArgs<ACTOR_MOVE_STATE>> MoveChange;
        public void FireMoveChange(ActorStateChangeArgs<ACTOR_MOVE_STATE> args) => MoveChange?.Invoke(args);
    } 
}
