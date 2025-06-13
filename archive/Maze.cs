public partial class	Dungeon
{
	/// <summary>
	/// Main entry point.
	/// </summary>
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

	private static readonly int[][]	_directions = [[-2, 0], [0, 2], [2, 0], [0, -2]];

	/// <summary>
	/// Returns the first available `Empty` tile located at odd coordinates. Returns (-1, -1) if none are available.
	/// </summary>
	private static (int y, int x)	StartingFrontier(Tile[,] map)
	{
		for (int y = 1; y < map.GetLength(0); y += 2)
			for (int x = 1; x < map.GetLength(1); x += 2)
				if (map[y, x] == Tile.Empty)
					return (y, x);

		return (-1, -1);
	}

	/// <summary>
	/// Adds all valid neighboring `Empty Tiles` offset by 2 in each direction from (x, y) to the frontier list.
	/// </summary>
	private static void	AddFrontiers(Tile[,] map, List<(int y, int x)> frontiers, int x, int y)
	{
		foreach (int[] dir in _directions)
		{
			int	ny = y + dir[0];
			int	nx = x + dir[1];

			if (nx > 0 && nx < map.GetLength(1)		// Check X bounds
				&& ny > 0 && ny < map.GetLength(0)	// Check Y bounds
				&& map[ny, nx] == Tile.Empty		// Check if tile is Empty
				&& !frontiers.Contains((ny, nx)))	// Check if tile isn't occupied
			frontiers.Add((ny, nx));
		}
	}

	/// <summary>
	/// Returns a list of neighboring `Tunnel Tiles` offset by 2 in each cardinal direction from (x, y).
	/// </summary>
	private static List<(int y, int x)>	GetNeighbors(Tile[,] map, int x, int y)
	{
		List<(int y, int x)>	neighbors = new List<(int y, int x)>();

		foreach (int[] dir in _directions)
		{
			int	ny = y + dir[0];
			int	nx = x + dir[1];

			if (nx > 0 && nx < map.GetLength(1)		// Check X bounds
				&& ny > 0 && ny < map.GetLength(0)	// Check Y bounds
				&& map[ny, nx] == Tile.Tunnel)		// Check if tile is Tunnel
			neighbors.Add((ny, nx));
		}

		return (neighbors);
	}
}
