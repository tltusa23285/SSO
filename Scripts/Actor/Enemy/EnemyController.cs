using Commands;
using Data;
using Godot;

namespace Actors
{
    public partial class EnemyController : ActorController
    {
        [Export] private Area3D AggroArea;
        [Export] private NavigationAgent3D Agent;

        private BarAction Attack => C_Actor.ActionBook.DefaultAttack;

        protected override void OnReady()
        {
            AggroArea.BodyEntered += TryAggro;
            Agent.VelocityComputed += VelocityComputed;
        }

        private void TryAggro(Node3D body)
        {
            if (body is not Actor) return;
            Actor other = (Actor)body;
            if (other.Stats.Category != ACTOR_CATEGORY.Player) return;
            C_Actor.SetTarget(body as Actor);

            Agent.TargetPosition = C_Actor.CurrentTarget.GlobalPosition;
        }

        protected override void OnProcess(in double delta) {}

        private double CachedDelta;
        private float AiCd = 0;
        protected override void OnPhysicsProcess(in double delta)
        {
            CachedDelta = delta;
            if (AiCd > 0)
            {
                AiCd -= (float)delta;
                return;
            }
            if (C_Actor.CurrentTarget == null || C_Actor.CurrentTarget.Equals(null)) return;

            if (C_Actor.CurrentTarget.GlobalPosition.DistanceTo(C_Actor.GlobalPosition) < 1.5 && Attack.Execute())
            {
                Attack.SetCooldown(Attack.Cd);
                AiCd = Attack.Cd / 2.0f;

                C_Actor.SetMoveDirection(Vector3.Zero);
            }
            else
            {
                PerformChase(delta);
            }
        }

        protected override void ActorTargetChanged() {}

        protected override void ActorDied() {}

        private void PerformAttack()
        {

        }

        private void PerformChase(in double delta)
        {
            // look at target
            C_Actor.LookAt(C_Actor.CurrentTarget.GlobalPosition, Vector3.Up);
            // if targets position is past a threshold, update it
            if (C_Actor.CurrentTarget.GlobalPosition.DistanceTo(Agent.TargetPosition) > 0.5f)
            {
                Agent.TargetPosition = C_Actor.CurrentTarget.GlobalPosition;
            }
            if (Agent.IsNavigationFinished()) // enabling avoidance only when pathing considerd finished
            {
                Agent.AvoidanceEnabled = true;
                return;
            }
            Agent.AvoidanceEnabled = false;

            //VelocityComputed (Chaser.GlobalPosition.DirectionTo(Agent.GetNextPathPosition()) 
            //               * (float)(Speed * Delta));
            VelocityComputed(C_Actor.GlobalPosition.DirectionTo(Agent.GetNextPathPosition()));
        }
        private void VelocityComputed(Vector3 v)
        {
            if (AiCd > 0) return;
            // Avoidance is on whenever we are within attack range of the target
            // taking the avoidance vector, and limiting it to only strafing the direction its weighted towards
            if (Agent.AvoidanceEnabled)
            {
                //v = Chaser.Basis.X * v.Length() * v.Dot(Chaser.Basis.X);
                v = (C_Actor.Basis.X * v.Length() * v.Dot(C_Actor.Basis.X)) * (float)CachedDelta * 0.5f;
                v = v.MoveToward(Vector3.Zero, ((float)CachedDelta));
                if (v.Length() > 0) C_Actor.SetMoveDirection(v, v.Length());
                else C_Actor.SetMoveDirection(v);
            }
            else
            {
                //Chaser.GlobalPosition = Chaser.GlobalPosition.MoveToward(Chaser.GlobalPosition + v, (float)(Speed * Delta));
                C_Actor.SetMoveDirection(v);
            }
        }
    } 
}