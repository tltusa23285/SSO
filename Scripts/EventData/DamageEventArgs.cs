using Actors;
using Godot;
using System;

namespace EventArgs
{
    public class DamageEventArgs : ActorEventArgs
    {
        public readonly int Damage;
        public readonly DMG_TYPE Type;
        public readonly string SourceID;

        public DamageEventArgs
            (
            Actor actor,
            int damage, 
            DMG_TYPE type = DMG_TYPE.Undefined,
            string sourceId = null
            )
            : base(actor)
        {
            Damage = damage;
            Type = type;
            SourceID = sourceId;
        }
    }
}
