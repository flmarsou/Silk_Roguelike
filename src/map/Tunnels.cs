public partial class	Dungeon
{
	private static void	DigTunnel(Tile[,] map, (int y, int x)from, (int y, int x)to, List<Room> rooms)
	{
		int		y = from.y;
		int		x = from.x;
		bool	hasPlacedDoor = false;

		// Dig vertically
		while (y != to.y)
		{
			// Skip digging if inside a room
			if (IsInsideAnyRoom(rooms, y, x))
			{
				y += Math.Sign(to.y - y);
				continue ;
			}

			// With 25% chance, place a door if not already placed, and if the position is valid
			if (!hasPlacedDoor && IsCorrectPlacement(map, y, x) && _rng.Next(3) == 0)
			{
				map[y, x] = Tile.Door;
				hasPlacedDoor = true;
			}
			else
				map[y, x] = Tile.Tunnel;

			y += Math.Sign(to.y - y);
		}

		// Dig horizontally
		while (x != to.x)
		{
			// Skip digging if inside a room
			if (IsInsideAnyRoom(rooms, y, x))
			{
				x += Math.Sign(to.x - x);
				continue ;
			}

			// With 25% chance, place a door if not already placed, and if the position is valid
			if (!hasPlacedDoor && IsCorrectPlacement(map, y, x) && _rng.Next(3) == 0)
			{
				map[y, x] = Tile.Door;
				hasPlacedDoor = true;
			}
			else
				map[y, x] = Tile.Tunnel;

			x += Math.Sign(to.x - x);
		}
	}

	/// <summary>
	/// Returns true if the given tile has `Empty Tiles` above and below OR left and right.
	/// </summary>
	private static bool	IsCorrectPlacement(Tile[,] map, int y, int x)
	{
		return ((map[y - 1, x] == Tile.Empty && map[y + 1, x] == Tile.Empty)
				|| (map[y, x - 1] == Tile.Empty && map[y, x + 1] == Tile.Empty));
	}
}
