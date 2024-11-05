using Actors;
using Godot;
using System;
using System.Collections.Generic;
using Hazards;

namespace Strategies
{
    [GlobalClass]
    public partial class CreateDamageArea : ApplicationStratResource
	{

        [Export] private PackedScene Area;

        [Export] private float CastTime = 3;
        [Export] private float Size = 3;
        [Export] private ApplicationStratResource[] AreaApps;

        public override void Apply(Actor source, HashSet<Actor> targets)
        {
            OneShotDamageArea area;
            foreach (var item in targets)
            {
                area = Area.Instantiate<OneShotDamageArea>();
                source.GetTree().Root.AddChild(area);

                area.Place(item.GlobalPosition, source, AreaApps, CastTime, Size);
            }
        }
    } 
}
