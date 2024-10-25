using Actors;
using System.Collections.Generic;
using Godot;

namespace Strategies
{
    /// <summary>
    /// Strategy for determining which targets will be attacked
    /// </summary>
    public interface ITargetStrat
    {
        /// <summary>
        /// Returns valid command targets
        /// </summary>
        /// <param name="source">Actor performing the command</param>
        /// <param name="targets">List of valid targets</param>
        /// <returns>If any valid targets were found</returns>
        public bool GetTargets(Actor source, out HashSet<Actor> targets);
    }
}