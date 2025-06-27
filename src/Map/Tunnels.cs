public partial class	Dungeon
{
	private static void	DigTunnel(Tile[,] map, (int y, int x)from, (int y, int x)to, List<Room> rooms)
	{
		int		y = from.y;
		int		x = from.x;

		// Dig vertically
		while (y != to.y)
		{
			// Skip digging if inside a room
			if (IsInsideAnyRoom(rooms, y, x))
			{
				y += Math.Sign(to.y - y);
				continue ;
			}

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

			map[y, x] = Tile.Tunnel;
			x += Math.Sign(to.x - x);
		}
	}
}
