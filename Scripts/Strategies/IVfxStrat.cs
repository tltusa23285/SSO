using Actors;
using System.Collections.Generic;
using Godot;

namespace Strategies
{
    /// <summary>
    /// Strategy for applying visual effects to attack participants
    /// </summary>
    public interface IVfxStrat
    {
        /// <summary>
        /// Applies visuals to participants
        /// </summary>
        /// <param name="source">origin of action</param>
        /// <param name="targets">additional effected targets</param>
        public void ApplyVFX(Actor source, HashSet<Actor> targets);
    } 
}