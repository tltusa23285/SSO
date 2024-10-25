using Godot;
using Data;
using Commands;
using Inputs;

namespace Actors
{
    public partial class PlayerController : ActorController
    {
        [ExportGroup("Player Controller")]
        [ExportSubgroup("Player")]
        [Export] private Aimlook LookControl;
        [Export] private PlayerClass PlayerClass;

        [ExportSubgroup("Target Selection")]
        [Export] private Camera3D Camera;
        [Export] private PlayerUI PlayerUI;

        #region Node Overrides
        protected override void OnReady()
        {
            PlayerUI.Intialize();
            PlayerUI.FillActionBook(C_Actor.ActionBook);

            foreach (var item in C_Actor.ActionBook.Actions)
            {
                item.CommandExecuted += CommandExecuted;

                // action has assigned hotbar slots, update ui
                if (!HotbarMap.Instance.GetInputsForAction(item.Name, out string[] inputs)) continue;
                {
                    foreach (var button in inputs)
                    {
                        PlayerUI.AssignAction(button, item);
                    }
                }
            }
        }

        private double oGcdTimer = 0;
        private double GcdTimer = 0;
        private double AutoTimer = 0;
        private Vector2 InputDir;
        private Vector3 MoveDir;

        protected override void OnProcess(in double delta)
        {
            AutoTimer += delta;
            GcdTimer += delta;
            oGcdTimer += delta;
            if (AutoTimer >= 2.5)
            {
                AutoTimer = C_Actor.ActionBook.DefaultAttack.Execute() ? 0 : 1;
            }

            InputDir = Input.GetVector(KeyMap.StrafeLeft, KeyMap.StrafeRight, KeyMap.Forward, KeyMap.Backward);

            MoveDir.X = InputDir.X;
            MoveDir.Z = InputDir.Y;

            C_Actor.SetMoveDirection((LookControl.CamBasis * MoveDir).Scale(y:0).Normalized());
            //C_Actor.SetMoveDirection(C_Actor.Transform.Basis * new Vector3(InputDir.X, 0, InputDir.Y));
        }
        protected override void OnPhysicsProcess(in double delta) {}
        #endregion

        #region Input Handling
        public override void _Input(InputEvent @event)
        {
            if (@event.IsActionPressed(KeyMap.Jump))
            {
                C_Actor.TryJump(7.5f);
                GetTree().Root.SetInputAsHandled();
            }
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (@event.IsActionPressed(KeyMap.SelectEntity) && TrySelectTarget(@event as InputEventMouse))
            {
            }
            else if (LookControl.HandleLookInput(@event))
            {
                GetTree().Root.SetInputAsHandled();
            }
        }

        private void CommandExecuted(bool success, BarAction comm, string msg)
        {
            if (success)
            {
                if (comm.OnGcd)
                {
                    C_Actor.LastCommand = comm.Name;
                    foreach (var item in C_Actor.ActionBook.Actions)
                    {
                        if (item.OnGcd) item.SetCooldown(Actor.GCD);
                    }
                }
                else
                {
                    comm.SetCooldown(comm.Cd);
                }
            }
        }

        private bool TrySelectTarget(InputEventMouse input)
        {
            Actor prev = C_Actor.CurrentTarget;
            C_Actor.SetTarget(null);
            Vector3 from = Camera.ProjectRayOrigin(input.Position);
            Vector3 to = from + Camera.ProjectRayNormal(input.Position) * 1000f;

            if (Camera.GetWorld3D().DirectSpaceState.Raycast(out var result, from, to))
            {
                if (result["collider"].AsGodotObject() is ISelectable)
                {
                    ISelectable select = result["collider"].AsGodotObject() as ISelectable;
                    if (select is Actor)
                    {
                        C_Actor.SetTarget(select as Actor);
                    }
                    GetTree().Root.SetInputAsHandled();
                }
            }
            return C_Actor.CurrentTarget != prev;
        } 
        #endregion

        #region ActorEvents
        protected override void ActorTargetChanged()
        {
            AutoTimer = 0;
        }

        protected override void ActorDied()
        {
            GetTree().ReloadCurrentScene();
        } 
        #endregion
    }
}