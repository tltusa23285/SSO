using Godot;
using System;

namespace Tools
{
	public partial class FreeCamera : Node3D
	{
        [Export] public float Sens = 0.5f;
        [Export] public double Speed = 5;
        [Export] public Node3D Cam;
        [Export] public float MinPitch = -80;
        [Export] public float MaxPitch = 80;
        public override void _Process(double delta)
        {
            Vector3 dir = new Vector3();
            if (Input.IsMouseButtonPressed(MouseButton.Right))
            {
                Input.MouseMode = Input.MouseModeEnum.Captured;
                if (Input.IsKeyPressed(Key.W)) dir -= Cam.GlobalTransform.Basis.Z;
                if (Input.IsKeyPressed(Key.S)) dir += Cam.GlobalTransform.Basis.Z;
                if (Input.IsKeyPressed(Key.D)) dir += Cam.GlobalTransform.Basis.X;
                if (Input.IsKeyPressed(Key.A)) dir -= Cam.GlobalTransform.Basis.X;
                if (Input.IsKeyPressed(Key.Q)) dir -= Cam.GlobalTransform.Basis.Y;
                if (Input.IsKeyPressed(Key.E)) dir += Cam.GlobalTransform.Basis.Y;
            }
            else
            {
                Input.MouseMode = Input.MouseModeEnum.Visible;
            }
            this.Position += dir * (float)(delta * (Input.IsKeyPressed(Key.Shift) ? Speed * 5 : Speed));
        }

        public override void _Input(InputEvent @event)
        {
            if (Input.IsMouseButtonPressed(MouseButton.Right))
            {
                if (@event is InputEventMouseMotion)
                {
                    Transform2D viewport_transform = GetTree().Root.GetFinalTransform();
                    Vector2 look = ((InputEventMouseMotion)((InputEventMouseMotion)@event).XformedBy(viewport_transform)).Relative;
                    look *= Sens;


                    if (!Mathf.IsZeroApprox(look.X))
                    {
                        this.Rotate(Vector3.Down, Mathf.DegToRad(look.X));
                        this.Orthonormalize();
                    }
                    if (!Mathf.IsZeroApprox(look.Y))
                    {
                        Cam.RotateObjectLocal(Vector3.Left, Mathf.DegToRad(look.Y));
                        Cam.Orthonormalize();
                        if (Cam.Rotation.X > Mathf.DegToRad(MinPitch) && Cam.Rotation.X < Mathf.DegToRad(MaxPitch)) return;
                        Cam.Rotation = new Vector3(Mathf.Clamp(Cam.Rotation.X, Mathf.DegToRad(MinPitch), Mathf.DegToRad(MaxPitch)), Cam.Rotation.Y, Cam.Rotation.Z);
                        Cam.Orthonormalize();
                    }
                }
            }
        }
    } 
}
