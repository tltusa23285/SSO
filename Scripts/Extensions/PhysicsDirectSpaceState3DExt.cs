using Godot;
using Godot.Collections;
using System.Collections.Generic;

public static partial class PhysicsDirectSpaceState3DExt
{
    public static HashSet<GodotObject> OverlapSphere(this PhysicsDirectSpaceState3D space, in Vector3 origin, in float radius)
    {
        HashSet<GodotObject> result = new HashSet<GodotObject>();


        PhysicsShapeQueryParameters3D query = new PhysicsShapeQueryParameters3D()
        {
            Transform = new Transform3D() { Basis = Basis.Identity, Origin = origin },
            Shape = new SphereShape3D() { Radius = radius }
        };

        foreach (var item in space.IntersectShape(query))
        {
            result.Add(item["collider"].AsGodotObject());
        }

        return result;
    }

    public static bool Raycast(this PhysicsDirectSpaceState3D space, out Dictionary result, 
        Vector3 from, Vector3 to, uint collisionMask = uint.MaxValue, params Rid[] exclude)
    {
        PhysicsRayQueryParameters3D query = PhysicsRayQueryParameters3D.Create(from, to, collisionMask, new Array<Rid>(exclude));
        result = space.IntersectRay(query);
        return result.Count > 0;
    }
}
