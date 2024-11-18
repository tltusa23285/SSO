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

		public event ActorEventHandler<DamageEventArgs> ActorDamaged;
		public void FireActorDamaged(DamageEventArgs args) => ActorDamaged?.Invoke(args);
		public void FlushActorDamaged() => ActorDamaged = null;
	} 
}
