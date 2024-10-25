using Godot;

namespace Actors
{
    public partial class ActorGraphicController : Node3D
    {
        private Actor Actor;

        [ExportGroup("Follow Settings")]
        [Export] public double PosSpeed = 10;
        [Export] public double RotSpeed = 10;

        public void Setup(Actor actor)
        {
            Actor = actor;
            this.GlobalPosition = actor.GlobalPosition;
            this.GlobalRotation = actor.GlobalRotation;

            //if (AnimPlayer != null)
            //{
            //    foreach (var item in AnimPlayer.GetAnimationList())
            //    {
            //        GD.PushWarning(item);
            //    }
            //}
        }

        public override void _Process(double delta)
        {
            if (Actor == null) return;
            this.GlobalPosition = this.GlobalPosition.MoveToward(Actor.GlobalPosition, (float)(delta * PosSpeed));
            if (Actor.CurrentTarget != null && Actor.CurrentTarget != Actor)
            {
                Vector3 dir = (Actor.CurrentTarget.GlobalPosition - Actor.GlobalPosition).Scale(y: 0).Normalized();
                Vector3 target = QuaterionExt.LookRotation(dir).GetEuler();
                this.GlobalRotation = this.GlobalRotation.RotateToward(target, (float)(delta * RotSpeed));
            }
            else
            {
                if (Actor.Facing != Vector3.Zero)
                {
                    this.GlobalRotation = this.GlobalRotation.RotateToward(QuaterionExt.LookRotation(Actor.Facing).GetEuler(), (float)(delta * RotSpeed));
                }
                else
                {
                    //this.GlobalRotation = this.GlobalRotation.MoveToward(Actor.GlobalRotation, ((float)(delta * RotSpeed)));
                }
            }
            HandleAnimUpdate();
        }

        #region Animation

        [ExportGroup("Animation Settings")]
        [Export] public string AnimPath = "PlayerAnimLibrary/";
        [Export] public AnimationPlayer AnimPlayer;
        [ExportSubgroup("Animation Names")]
        [Export] private string Runanim = "run";
        [Export] private string IdleAnim = "idle";

        private string c_name;
        private int c_prio;
        private SceneTreeTimer ResetTimer;

        private string GetAnimName(in string anim) => $"{AnimPath}{anim}";
        public void SetAnimation(in string animName, in int priority)
        {
            if (c_name == animName) return; // is current, do nothing
            if (c_prio > priority) return; // lower priority, do nothing, when equal, we use the newest anim

            Animation anim = AnimPlayer.GetAnimation(GetAnimName(animName)); // looking for animation with matching name
            if (anim == null) return; // failed to find, do nothing

            if (ResetTimer != null) ResetTimer.Timeout -= ResetAnim; // clear any active reset timer

            if (anim.LoopMode == Animation.LoopModeEnum.None) // if not looping set a reset timer to return to idle
            {
                ResetTimer = GetTree().CreateTimer(anim.Length);
                ResetTimer.Timeout += ResetAnim;
            }

            // assign current name and priority
            c_name = animName;
            c_prio = priority;

            AnimPlayer.Play(GetAnimName(animName), customBlend: 0.1f);
        }

        private void ResetAnim()
        {
            c_prio = int.MinValue;
            c_name = null;
            ResetTimer = null;
            SetAnimation(IdleAnim, 0);
        }

        private void HandleAnimUpdate()
        {
            if (AnimPlayer != null)
            {
                if (Actor.Velocity != Vector3.Zero)
                {
                    SetAnimation(Runanim, 0);
                }
                else
                {
                    SetAnimation(IdleAnim, 0);
                }
            }
        }
        #endregion
    }
}