using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dungeon
{
    [System.Flags]
    public enum DIRECTION : byte
    {
        NORTH = 1 << 0,
        EAST = 1 << 1,
        SOUTH = 1 << 2,
        WEST = 1 << 3
    }
    public partial class DungeonGenerator : Node
	{

        [Export] public Vector2I Size;
        [Export] public float CellSize;
        [Export] public Node3D StartMarker;
        [Export] public Node3D EndMarker;
        [Export] public Node3D GenMarker;
        [Export] public RoomFactory RoomFactory;

        public DungeonRoom StartRoom { get; private set; }
        private (int, int) StartCoords;

        private byte[,] Map;

        private Dictionary<(int, int), Node> InstancedTiles = new Dictionary<(int, int), Node>();
        public List<DungeonRoom> Rooms = new List<DungeonRoom>();

        private void PrintDebugMap()
        {
            for (int y = 0; y < Size.Y; y++)
            {
                string l = "";
                for (int x = 0; x < Size.X; x++)
                {
                    if      (Map[x, y] == 98) l += $" S ";
                    else if (Map[x, y] == 99) l += $" E ";
                    else l += $" {Map[x,y].ToString("00")}";
                }
                GD.Print(l);
            }
            for (int y = 0; y < Size.Y; y++)
            {
                for (int x = 0; x < Size.X; x++)
                {
                    if (Map[x, y] == 0) continue;
                    GD.Print($"{Map[x,y]}::{((DIRECTION)Map[x, y]).ToString()}");
                }
            }
        }

        public void Generate()
        {
            var rng = new RandomNumberGenerator();
            Map = new byte[Size.X,Size.Y];
            for (int i = 0; i < Size.X * Size.Y; i++)
            {
                Map[i % Size.X, i / Size.X] = 0;
            };

            GenCorePath();
            SpawnMap();
        }


        private bool DirectionValid(in int cx, in int cy, in int x, in int y)
        {
            if (cx + x < 0 || cx + x >= Size.X) return false;
            if (cy + y < 0 || cy + y >= Size.Y) return false;
            return true;
        }

        // Given start coords, generate a path until no direction left to move
        private async Task<List<(int, int)>> GenPathFromPoint(int cx, int cy)
        {
            var rng = new RandomNumberGenerator();
            List<(int, int)> path = new List<(int, int)>
            {
                (cx, cy)
            };

            // Creates a oath by picking random directions until no longer able to progress
            while (true)
            {
                List<(int, int)> dirs = new List<(int, int)>()
                {
                    (1, 0), (-1, 0), (0, 1)/*, (0,-1),*/
                };
                if (path.Count >= 6) dirs.RemoveAll(a => a != (0, 1));
                bool got_dir = false;
                while (dirs.Count > 0)
                {
                    (int, int) d = dirs[rng.RandiRange(0, dirs.Count - 1)];
                    dirs.Remove(d);
                    if (!path.Contains((cx + d.Item1, cy + d.Item2))
                        && DirectionValid(cx, cy, d.Item1, d.Item2)
                        && Map[cx + d.Item1, cy + d.Item2] == 0)
                    {
                        cx += d.Item1;
                        cy += d.Item2;
                        path.Add((cx, cy));
                        got_dir = true;
                        break;
                    }
                }
                if (!got_dir)
                {
                    break;
                }
            }

            // filling direct path connection
            for (int i = 0; i < path.Count; i++)
            {
                int x = path[i].Item1;
                int y = path[i].Item2;
                int dx, dy;
                if (i > 0)
                {
                    int px = path[i - 1].Item1;
                    int py = path[i - 1].Item2;
                    dx = px - x;
                    dy = py - y;
                    if (dx == 1) Map[x, y] |= (byte)DIRECTION.EAST;
                    if (dx == -1) Map[x, y] |= (byte)DIRECTION.WEST;
                    if (dy == 1) Map[x, y] |= (byte)DIRECTION.NORTH;
                    if (dy == -1) Map[x, y] |= (byte)DIRECTION.SOUTH;

                }
                if (i < path.Count - 1)
                {
                    int nx = path[i + 1].Item1;
                    int ny = path[i + 1].Item2;
                    dx = nx - x;
                    dy = ny - y;
                    if (dx == 1) Map[x, y] |= (byte)DIRECTION.EAST;
                    if (dx == -1) Map[x, y] |= (byte)DIRECTION.WEST;
                    if (dy == 1) Map[x, y] |= (byte)DIRECTION.NORTH;
                    if (dy == -1) Map[x, y] |= (byte)DIRECTION.SOUTH;
                }
                //SetTile((x,y));
                //await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
            }
            return path;
        }

        private async void GenCorePath()
        {
            List<(int, int)> possible_starts = new List<(int, int)>(); // initialize all possible starting positions in map
            for (int x = 0; x < Size.X; x++)
            {
                for (int y = 0; y < Size.Y; y++)
                {
                    if (Map[x, y] == 0)
                    {
                        possible_starts.Add((x, y));
                    }
                }
            }
            List<(int, int)> path;
            var rng = new RandomNumberGenerator();
            StartCoords = (rng.RandiRange(0, Size.X - 1), 0);
            path = await GenPathFromPoint(StartCoords.Item1, StartCoords.Item2); // generate core path from start to end, guarenteeing there is always a path between them
            StartMarker.Position = new Vector3(path[0].Item1 * CellSize, 3, path[0].Item2 * CellSize);
            EndMarker.Position = new Vector3(path[path.Count - 1].Item1 * CellSize, 3, path[path.Count - 1].Item2 * CellSize);

            possible_starts.RemoveAll(a => (path.Contains(a)) || a.Item2 != 0); // removing positions on core path that are filled and not on the top row

            // repeats pathgen process until all cells have been used
            //while (possible_starts.Count > 0)
            //{
            //    (int,int) rand_start = possible_starts[rng.RandiRange(0,possible_starts.Count-1)];
            //    path = await GenPathFromPoint(rand_start.Item1, rand_start.Item2);
            //    possible_starts.RemoveAll(a => path.Contains(a));
            //}

            // iterate all cells and randomly create connections if adjacent to existing tile
            for (int x = 0; x < Size.X; x++)
            {
                for (int y = 0; y < Size.Y; y++)
                {
                    if (rng.RandiRange(0, 100) >= 75 && DirectionValid(x, y, 0, 1)  && Map[x, y + 1] != 0) ConnectPair(x, y, x, y + 1, DIRECTION.NORTH);
                    if (rng.RandiRange(0, 100) >= 75 && DirectionValid(x, y, 0, -1) && Map[x, y - 1] != 0) ConnectPair(x, y, x, y - 1, DIRECTION.SOUTH);
                    if (rng.RandiRange(0, 100) >= 75 && DirectionValid(x, y, 1, 0)  && Map[x + 1, y] != 0) ConnectPair(x, y, x + 1, y, DIRECTION.EAST);
                    if (rng.RandiRange(0, 100) >= 75 && DirectionValid(x, y, -1, 0) && Map[x - 1, y] != 0) ConnectPair(x, y, x - 1, y, DIRECTION.WEST);
                    GenMarker.Position = new Vector3(x * CellSize, 3, y * CellSize);
                }
            }
            PrintDebugMap();
        }

        private void ConnectPair(in int sx, in int sy, in int tx, in int ty, DIRECTION dir)
        {
            Map[sx, sy] |= (byte)dir;
            switch (dir)
            {
                case DIRECTION.NORTH: Map[tx, ty] |= (byte)DIRECTION.SOUTH; break;
                case DIRECTION.EAST:  Map[tx, ty] |= (byte)DIRECTION.WEST; break;
                case DIRECTION.SOUTH: Map[tx, ty] |= (byte)DIRECTION.NORTH; break;
                case DIRECTION.WEST:  Map[tx, ty] |= (byte)DIRECTION.EAST; break;
                default:
                    break;
            }
            //SetTile((tx, ty));
            //SetTile((sx, sy));
        }

        private async void SpawnMap()
        {
            for (int x = 0; x < Size.X; x++)
            {
                for (int y = 0; y < Size.Y; y++)
                {
                    if (Map[x,y] != 0)
                    {
                        // negating z pos to adjust for godot using -z as forward
                        RoomFactory.SpawnRoom(Map[x, y], new Vector3(x * CellSize, 0, -y * CellSize), this, out DungeonRoom room);
                        if (StartCoords.Item1 == x && StartCoords.Item2 == y)
                        {
                            StartRoom = room;
                        }
                        else
                        {
                            Rooms.Add(room);
                        }
                        //await ToSignal(GetTree().CreateTimer(0.05f), "timeout");
                    }
                }
            }
        }
    } 
}
