using Godot;

public static class Vector3Ext
{
    public static Vector3 Scale(this Vector3 v, float x = 1, float y = 1, float z = 1)
    {
        v.X *= x; 
        v.Y *= y; 
        v.Z *= z; 
        return v;
    }

    /// <summary>
    /// Ext, performing Mathf.RotateToward on each axis to the given vector
    /// </summary>
    /// <returns></returns>
    public static Vector3 RotateToward(this Vector3 v, in Vector3 to, float delta)
    {
        v.X = Mathf.RotateToward(v.X, to.X, delta);
        v.Y = Mathf.RotateToward(v.Y, to.Y, delta);
        v.Z = Mathf.RotateToward(v.Z, to.Z, delta);
        return v;
    }
}
