public partial class	Dungeon
{
	private static void	DigTunnel(Tile[,] map, (int y, int x)from, (int y, int x)to, List<Room> rooms)
	{
		int		x = from.x;
		int		y = from.y;
		bool	shouldPlaceDoor;
		bool	hasPlacedDoor = false;

		while (x != to.x)
		{
			if (IsInsideAnyRoom(rooms, x, y))
			{
				x += Math.Sign(to.x - x);
				continue ;
			}

			shouldPlaceDoor = IsCorrectPlacement(map, x, y);

			if (!hasPlacedDoor && shouldPlaceDoor && _rng.Next(3) == 0)
			{
				map[y, x] = Tile.Door;
				hasPlacedDoor = true;
			}
			else
				map[y, x] = Tile.Tunnel;

			x += Math.Sign(to.x - x);
		}

		while (y != to.y)
		{
			if (IsInsideAnyRoom(rooms, x, y))
			{
				y += Math.Sign(to.y - y);
				continue ;
			}

			shouldPlaceDoor = IsCorrectPlacement(map, x, y);

			if (!hasPlacedDoor && shouldPlaceDoor && _rng.Next(3) == 0)
			{
				map[y, x] = Tile.Door;
				hasPlacedDoor = true;
			}
			else
				map[y, x] = Tile.Tunnel;

			y += Math.Sign(to.y - y);
		}
	}

	/// <summary>
	/// Checks if there are `Empty Tiles` above and below OR right and left.
	/// </summary>
	private static bool	IsCorrectPlacement(Tile[,] map, int x, int y)
	{
		return ((map[y - 1, x] == Tile.Empty && map[y + 1, x] == Tile.Empty)
				|| (map[y, x + 1] == Tile.Empty && map[y, x - 1] == Tile.Empty));
	}
}
