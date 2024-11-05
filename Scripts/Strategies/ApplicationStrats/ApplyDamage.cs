using Godot;
using System;
using Actors;
using System.Collections.Generic;

namespace Strategies
{
    [GlobalClass]
    public partial class ApplyDamage : ApplicationStratResource
    {
        private enum DAMAGE_TYPE { StatCoefficient, Flat}
        [Export] private DAMAGE_TYPE PotencyContext = DAMAGE_TYPE.StatCoefficient;
        [Export] private int Potency = 100;

        public override void Apply(Actor source, HashSet<Actor> targets)
        {
            float damage;
            switch (PotencyContext)
            {
                case DAMAGE_TYPE.StatCoefficient:
                    damage = source.Stats.AttackPower;
                    damage *= (Potency / 100f);
                    break;
                case DAMAGE_TYPE.Flat:
                    damage = Potency;
                    break;
                default:
                    throw new NotImplementedException();
            }

            foreach (var item in targets)
            {
                item.Health.Damage(Mathf.FloorToInt(damage));
            }
        }
    } 
}
