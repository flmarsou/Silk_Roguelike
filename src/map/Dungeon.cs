public partial class	Dungeon
{
	// Settings
	private static readonly int					_mapWidth = 41;			// Horizontal (x) amount of tiles
	private static readonly int					_mapLength = 41;		// Vertical (y) amount of tiles
	private static readonly (int min, int max)	_roomSize = (4, 14);	// Min and Max dimensions for rooms
	private static readonly int					_roomAttempts = 1000;	// Amount of attemps to generate rooms
	private static readonly int					_roomPadding = 1;		// Minimum empty tiles required between rooms

	private readonly static Random	_rng = new Random();

	public static Tile[,]	GenerateDungeon()
	{
		Tile[,]	map = new Tile[_mapLength, _mapWidth];

		for (int y = 0; y < map.GetLength(0); y++)
			for (int x = 0; x < map.GetLength(1); x++)
				map[y, x] = Tile.Empty;

		List<Room>	rooms = GenerateRooms(map);
		ConnectRooms(map, rooms);
		GenerateTunnelWalls(map);
		GenerateDoors(map, rooms);
		GenerateMisc(map, rooms);

		return (map);
	}

	public static bool	IsInBound(int y, int x)	=>	!(y < 0 || y >= _mapLength || x < 0 || x >= _mapWidth);

	private static List<Room>	GenerateRooms(Tile[,] map)
	{
		List<Room>	rooms = new List<Room>();

		for (int i = 0; i < _roomAttempts; i++)
		{
			// 1. Create a randomly-sized rectangle room
			int	roomWidth = _rng.Next(_roomSize.min, _roomSize.max);
			int	roomHeight = _rng.Next(_roomSize.min, _roomSize.max);
			int	posX = _rng.Next(0, _mapWidth - roomWidth);
			int	posY = _rng.Next(0, _mapLength - roomHeight);

			Room	newRoom = new Room(posY, posX, roomWidth, roomHeight);

			// 2. Checks if `newRoom` doesn't overlap with another room
			bool	overlap = false;

			foreach (Room room in rooms)
			{
				if (newRoom.Overlap(room, _roomPadding))
				{
					overlap = true;
					break ;
				}
			}

			if (overlap)
				continue ;

			// 3. Add `newRoom` into the map and the list
			AddRoom(map, newRoom);
			rooms.Add(newRoom);
		}

		return (rooms);
	}

	private static void	ConnectRooms(Tile[,] map, List<Room> rooms)
	{
		List<(int y, int x)>	centers = new List<(int y, int x)>();
		HashSet<int>			connected = new HashSet<int> { 0 };

		// Store the center point (y, x) of each room
		for (int i = 0; i < rooms.Count; i++)
			centers.Add(rooms[i].Center);

		// Run while not all rooms are connected
		while (connected.Count < centers.Count)
		{
			int	bestTo = -1;				// Index of the closest unconnected room
			int	bestFrom = -1;				// Index of the closest connected room
			int	bestDist = int.MaxValue;	// Min distance between `bestTo` and `bestFrom`

			// 1. Loop through all connected rooms
			foreach (int i in connected)
			{
				// 2. Compare with all unconnected rooms
				for (int j = 0; j < centers.Count; j++)
				{
					if (connected.Contains(j))
						continue ;

					// 3. Use Manhattan distance between connected to unconnected room
					int	dist = Math.Abs(centers[i].x - centers[j].x)
							 + Math.Abs(centers[i].y - centers[j].y);

					// 4. Store newly shortest rooms found
					if (dist < bestDist)
					{
						bestDist = dist;
						bestFrom = i;
						bestTo = j;
					}
				}
			}

			// 5. Dig a tunnel between the closest pair found, and mark it as `connected`
			DigTunnel(map, centers[bestFrom], centers[bestTo], rooms);
			connected.Add(bestTo);
		}
	}

	private static void	GenerateTunnelWalls(Tile[,] map)
	{
		for (int y = 0; y < map.GetLength(0); y++)
		{
			for (int x = 0; x < map.GetLength(1); x++)
			{
				// Surround every `Tunnel Tiles` and `Door Tiles` with walls
				if (map[y, x] == Tile.Tunnel || map[y, x] == Tile.Door)
				{
					if (map[y - 1, x - 1] == Tile.Empty)	// Top Left
						map[y - 1, x - 1] = Tile.Wall;
					if (map[y - 1, x] == Tile.Empty)		// Top
						map[y - 1, x] = Tile.Wall;
					if (map[y - 1, x + 1] == Tile.Empty)	// Top Right
						map[y - 1, x + 1] = Tile.Wall;
					if (map[y, x + 1] == Tile.Empty)		// Right
						map[y, x + 1] = Tile.Wall;
					if (map[y + 1, x + 1] == Tile.Empty)	// Bottom Right
						map[y + 1, x + 1] = Tile.Wall;
					if (map[y + 1, x] == Tile.Empty)		// Bottom
						map[y + 1, x] = Tile.Wall;
					if (map[y + 1, x - 1] == Tile.Empty)	// Buttom Left
						map[y + 1, x - 1] = Tile.Wall;
					if (map[y, x - 1] == Tile.Empty)		// Left
						map[y, x - 1] = Tile.Wall;
				}
			}
		}
	}

	private static void	GenerateDoors(Tile[,] map, List<Room> rooms)
	{
		for (int y = 0; y < map.GetLength(0); y++)
		{
			for (int x = 0; x < map.GetLength(1); x++)
			{
				if (map[y, x] == Tile.Tunnel && IsEdgeAnyRoom(rooms, y, x)			// `Tunnel Tile` on the edge of a room
					&& ((map[y - 1, x] == Tile.Wall && map[y + 1, x] == Tile.Wall)	// AND `Wall Tiles` top and bottom
					|| (map[y, x + 1] == Tile.Wall && map[y, x - 1] == Tile.Wall)))	// OR `Wall Tiles` right and left
				{
					map[y, x] = Tile.Door;

					if (map[y - 2, x] == Tile.Door			// `Door Tile` two tiles higher
						&& map[y - 1, x - 1] == Tile.Wall	// `Wall Tile` top left
						&& map[y - 1, x + 1] == Tile.Wall)	// `Wall Tile` top right
					{
						int	roll = _rng.Next(3);

						map[y, x] = Tile.Tunnel;
						map[y - 2, x] = Tile.Tunnel;
						map[y - roll, x] = Tile.Door;
					}
					else if (map[y, x - 2] == Tile.Door		// `Door Tile` two tiles left
						&& map[y - 1, x - 1] == Tile.Wall	// `Wall Tile` top left
						&& map[y + 1, x - 1] == Tile.Wall)	// `Wall Tile` bottom left
					{
						int	roll = _rng.Next(3);

						map[y, x] = Tile.Tunnel;
						map[y, x - 2] = Tile.Tunnel;
						map[y, x - roll] = Tile.Door;
					}
				}
			}
		}
	}

	private static void	GenerateMisc(Tile[,] map, List<Room> rooms)
	{
		map[rooms[0].Center.y, rooms[0].Center.x] = Tile.PlayerSpawn;
		map[rooms[rooms.Count - 1].Center.y, rooms[rooms.Count - 1].Center.x] = Tile.EnemySpawn;
	}
}
