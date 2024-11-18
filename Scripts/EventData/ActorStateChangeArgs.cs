using Actors;
using Godot;
using System;

namespace EventArgs
{
    public class ActorStateChangeArgs<T> : ActorEventArgs where T : Enum
    {
        public readonly T Previous;
        public readonly T Next;
        public ActorStateChangeArgs
            (
            Actor actor,
            T prev, T next
            ) 
            : base(actor)
        {
            Previous = prev;
            Next = next;
        }
    }
}
