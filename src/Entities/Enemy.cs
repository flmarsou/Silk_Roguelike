class	Enemy(int y, int x)
{
	public int	Y { get; set; } = y;
	public int	X { get; set; } = x;

	public void	Act(Tile[,] map, int playerY, int playerX)
	{
		BFSPathfinding(map, playerY, playerX);
	}

	private void	BFSPathfinding(Tile[,] map, int playerY, int playerX)
	{
		Queue<(int y, int x, int fromY, int fromX)>	queue = new Queue<(int y, int x, int fromY, int fromX)>();
		HashSet<(int y, int x)>						visited = new HashSet<(int y, int x)>();
		Dictionary<(int y, int x), (int y, int x)>	cameFrom = new Dictionary<(int y, int x), (int y, int x)>();

		queue.Enqueue((this.Y, this.X, -1, -1));	// Starting tile, no predecessor

		while (queue.Count > 0)
		{
			(int y, int x, int fromY, int fromX) = queue.Dequeue();

			// Bounds check
			if (y < 0 || y >= map.GetLength(0) || x < 0 || x >= map.GetLength(0))
				continue ;

			// Already visited
			if (!visited.Add((y, x)))
				continue ;

			// Tile check
			if (map[y, x] == Tile.Wall)
				continue ;

			// Track path
			if (fromY != -1 && fromX != -1)
				cameFrom[(y, x)] = (fromY, fromX);

			// Goal check
			if (y == playerY && x == playerX)
			{
				MoveOneStepToward(map, (y, x), cameFrom);
				break ;
			}

			queue.Enqueue((y - 1, x, y, x));	// Add top
			queue.Enqueue((y, x + 1, y, x));	// Add right
			queue.Enqueue((y + 1, x, y, x));	// Add bottom
			queue.Enqueue((y, x - 1, y, x));	// Add left
		}
	}

	private void	MoveOneStepToward(Tile[,] map, (int y, int x) playerPos, Dictionary<(int y, int x), (int y, int x)> cameFrom)
	{
		(int y, int x)	current = playerPos;

		while (cameFrom.ContainsKey(current) && cameFrom[current] != (this.Y, this.X))
			current = cameFrom[current];

		if (map[current.y, current.x] == Tile.Door)
		{
			map[current.y, current.x] = Tile.DoorOpen;
			return ;
		}

		this.Y = current.y;
		this.X = current.x;
	}
}
