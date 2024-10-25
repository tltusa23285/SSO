using Godot;
using System;
using Actors;
using System.Collections.Generic;

namespace Strategies
{
    [GlobalClass]
    public partial class ApplyDamage : ApplicationStratResource
    {
        [Export] private int Potency = 100;

        public override void Apply(Actor source, HashSet<Actor> targets)
        {
            float damage = source.Stats.AttackPower;
            damage *= (Potency / 100f);

            foreach (var item in targets)
            {
                item.Health.Damage(Mathf.FloorToInt(damage));
            }
        }
    } 
}
