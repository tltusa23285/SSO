using Actors;
using Godot;
using System;
using System.Collections.Generic;

namespace Strategies
{
    public interface IApplicationStrat
    {
        public void Apply(Actor source, HashSet<Actor> targets);
    }
}