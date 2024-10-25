using Godot;
using Godot.Collections;

public static partial class PhysicsDirectSpaceState3DExt
{
	public static Array<Dictionary> OverlapSphere(this PhysicsDirectSpaceState3D space, Vector3 origin, float radius)
    {
        PhysicsShapeQueryParameters3D query = new PhysicsShapeQueryParameters3D()
        {
            Transform = new Transform3D() { Basis = Basis.Identity, Origin = origin },
            Shape = new SphereShape3D() { Radius = radius }
        };

        return space.IntersectShape(query);
    }

    public static bool Raycast(this PhysicsDirectSpaceState3D space, out Dictionary result, 
        Vector3 from, Vector3 to, uint collisionMask = uint.MaxValue, params Rid[] exclude)
    {
        PhysicsRayQueryParameters3D query = PhysicsRayQueryParameters3D.Create(from, to, collisionMask, new Array<Rid>(exclude));
        result = space.IntersectRay(query);
        return result.Count > 0;
    }
}
