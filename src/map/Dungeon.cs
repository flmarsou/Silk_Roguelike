
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

		GenerateRooms(map);
		GenerateMaze(map);

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

	private static void	GenerateMaze(Tile[,] map)
	{
		List<(int y, int x)>	frontiers = new List<(int y, int x)>();

		while (true)
		{
			// Get an available tile for starting frontier, stop when none are present anymore
			(int y, int x) = StartingFrontier(map);

			if (y == -1 || x == -1)
				break ;

			map[y, x] = Tile.Tunnel;
			AddFrontiers(map, frontiers, x, y);

			while (frontiers.Count > 0)
			{
				// 1. Select, store, and delete a random frontier point
				int	roll = _rng.Next(frontiers.Count);
				(int fy, int fx) = frontiers[roll];
				frontiers.RemoveAt(roll);

				// 2. Store all the neighboring frontiers
				List<(int ny, int nx)>	neighbors = GetNeighbors(map, fx, fy);
				if (neighbors.Count == 0)
					continue ;

				// 3. Get a random neighbor
				(int ny, int nx) = neighbors[_rng.Next(neighbors.Count)];

				// 4. Create the path between the frontier and its neighbor
				map[fy, fx] = Tile.Tunnel;
				map[(fy + ny) / 2, (fx + nx) / 2] = Tile.Tunnel;

				AddFrontiers(map, frontiers, fx, fy);
			}
		}
	}
}
