using Godot;

namespace Dungeon
{
    /// <summary>
    /// Stores information on a room
    /// </summary>
	[GlobalClass]
    public partial class RoomFactory : Resource
	{
        [Export] public PackedScene Closed;
        [Export] public PackedScene OneExit;
        [Export] public PackedScene LShape;
        [Export] public PackedScene Straight;
        [Export] public PackedScene TShape;
        [Export] public PackedScene Cross;

        public void SpawnRoom(in byte b, in Vector3 position, in Node parent, out DungeonRoom room)
        {
            PackedScene source = GetShape(b, out float rotation);
            room = source.Instantiate<DungeonRoom>();
            room.Position = position;
            room.RotateY(Mathf.DegToRad(-rotation)); // negating to rotate CW instead of CCW
            room.Name = $"{b}::{rotation}::{position}";
            parent.AddChild(room);
        }

        private PackedScene GetShape(in byte b, out float rotation)
        {
            PackedScene result = null;

            rotation = 0;

            int count = 0; // counting number of set bits
            byte n = b;
            while (n > 0)
            {
                count += n & 1;
                n >>= 1;
            }

            // determining prefab to use
            switch (count)
            {
                case 0: result = Closed; break;
                case 1: result = OneExit; break;
                case 3: result = TShape; break;
                case 4: result = Cross; break;
                case 2:
                    if      ((b & (byte)DIRECTION.NORTH) != 0 && (b & (byte)DIRECTION.SOUTH) != 0) result = Straight;
                    else if ((b & (byte)DIRECTION.EAST)  != 0 && (b & (byte)DIRECTION.WEST)  != 0) result = Straight;
                    else      result = LShape;
                    break;
                default:
                    GD.PrintErr("???");
                    break;
            }

            // determining rotation
            // assumies default orientation has an exit to the north, with openings clockwise
            switch (count)
            {
                case 0:
                case 4: break;
                case 1:
                    if      ((b & (byte)DIRECTION.EAST) != 0) rotation = 90;
                    else if ((b & (byte)DIRECTION.SOUTH)!= 0) rotation = 180;
                    else if ((b & (byte)DIRECTION.WEST) != 0) rotation = 270;
                    break;
                case 3:
                    if      ((b & (byte)DIRECTION.NORTH) == 0) rotation = 90;
                    else if ((b & (byte)DIRECTION.EAST)  == 0) rotation = 180;
                    else if ((b & (byte)DIRECTION.SOUTH) == 0) rotation = 270;
                    break;
                case 2:
                    if (result == Straight)
                    {
                        if ((b & (byte)DIRECTION.EAST) != 0) rotation = 90;
                    }
                    else
                    {
                        if (((b & (byte)DIRECTION.NORTH) != 0) && ((b & (byte)DIRECTION.WEST) != 0)) rotation = 270;
                        if ((b & (byte)DIRECTION.SOUTH) != 0)
                        {
                            if ((b & (byte)DIRECTION.EAST) != 0) rotation = 90;
                            else                                 rotation = 180;
                        }
                    }
                    break;
                default:
                    GD.PrintErr("???");
                    break;
            }

            return result;
        }

    } 
}