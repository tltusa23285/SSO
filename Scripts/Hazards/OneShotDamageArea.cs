using Actors;
using Godot;
using Strategies;
using System;
using System.Collections.Generic;

namespace Hazards
{
	public partial class OneShotDamageArea : Node3D
    {
        private float CastTime = 1;
        [Export] private Decal Base;
        [Export] private Decal Fill;
        [Export] private Timer Timer;

        private ApplicationStratResource[] Applications;

        protected Actor Source;

        public void Place(
            in Vector3 position,
            in Actor source,
            in ApplicationStratResource[] applicationEffects,
            in float castTime,
            in float size
            )
        {
            CastTime = castTime;
            this.GlobalPosition = position;
            this.Source = source;
            Base.Size = new Vector3(size, 10, size);
            Fill.Size = Base.Size.Scale(x:0,z:0);
            Applications = applicationEffects;

            Run();
        }

        private async void Run()
        {
            Timer.OneShot = true;
            Timer.Start(CastTime);
            while (!Timer.IsStopped())
            {
                Fill.Size = Vector3.Zero.Lerp(Base.Size, (float)Mathf.InverseLerp(CastTime, 0, Timer.TimeLeft));
                await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
            }

            Apply();

            this.QueueFree();
        }

        protected void Apply()
        {
            HashSet<Actor> targets = new HashSet<Actor>();
            foreach (var item in this.GetWorld3D().DirectSpaceState.OverlapSphere(this.GlobalPosition, Base.Size.X))
            {
                if (item is Actor)
                {
                    targets.Add(item as Actor);
                }
            }
            foreach (var item in Applications)
            {
                item.Apply(Source, targets);
            }
        }
    } 
}
