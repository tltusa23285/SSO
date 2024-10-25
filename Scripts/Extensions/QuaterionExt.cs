using Godot;

public static class QuaterionExt
{
    public static Quaternion LookRotation(in Vector3 dir)
    {
        return LookRotation(dir, Vector3.Up);
    }
    public static Quaternion LookRotation(in Vector3 dir, in Vector3 up)
    {
        return Basis.LookingAt(dir, up).GetRotationQuaternion();
    }
}
