public partial class	Dungeon
{
	// Settings
	private static readonly int					_mapWidth = 41;
	private static readonly int					_mapLength = 41;
	private static readonly (int min, int max)	_roomSize = (4, 14);
	private static readonly int					_roomAttempts = 1000;
	private static readonly int					_roomPadding = 1;

	private readonly static Random	_rng = new Random();

	public static Tile[,]	GenerateDungeon()
	{
		Tile[,]	map = new Tile[_mapLength, _mapWidth];

		for (int y = 0; y < map.GetLength(0); y++)
			for (int x = 0; x < map.GetLength(1); x++)
				map[y, x] = Tile.Empty;

		List<Room>	rooms = GenerateRooms(map);
		ConnectRooms(map, rooms);

		return (map);
	}

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

			Room	newRoom = new Room(posX, posY, roomWidth, roomHeight);

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

		while (connected.Count < centers.Count)
		{
			int	bestTo = -1;				// Index of the closest unconnected room
			int	bestFrom = -1;				// Index of the closest connected room
			int	bestDist = int.MaxValue;	// Min distance between `bestTo` and `bestFrom`

			foreach (int i in connected)
			{
				for (int j = 0; j < centers.Count; j++)
				{
					if (connected.Contains(j))
						continue ;

					// Use Manhattan distance between connected to unconnected room
					int	dist = Math.Abs(centers[i].x - centers[j].x)
							 + Math.Abs(centers[i].y - centers[j].y);

					// Store newly shortest rooms found
					if (dist < bestDist)
					{
						bestDist = dist;
						bestFrom = i;
						bestTo = j;
					}
				}
			}

			// TODO: Replace center (y, x) room coordinates by Room objects themselves.
			DigTunnel(map, centers[bestFrom], centers[bestTo]);
			connected.Add(bestTo);
		}
	}

	// TODO: Replace center starting point by the closest edges of from/to rooms for better results.
	private static void	DigTunnel(Tile[,] map, (int y, int x)from, (int y, int x)to)
	{
		int	x = from.x;
		int	y = from.y;

		while (x != to.x)
		{
			map[y, x] = Tile.Tunnel;
			x += Math.Sign(to.x - x);
		}
		while (y != to.y)
		{
			map[y, x] = Tile.Tunnel;
			y += Math.Sign(to.y - y);
		}
	}
}
