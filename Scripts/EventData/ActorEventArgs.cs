using Godot;
using System;
using Actors;

namespace EventArgs
{
    /// <summary>
    /// Event args are informational only, and should only be used/generated when no further modification should occur
    /// For example, only used after all damage has been calculated and applied, displaying the final amount
    /// </summary>
    public abstract class ActorEventArgs
    {
        public readonly Actor Actor;
        public ActorEventArgs(Actor actor)
        {
            Actor = actor;
        }
    }
}
