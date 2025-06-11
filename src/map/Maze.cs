public partial class	Dungeon
{
	private static List<(int, int, bool)>	AddFrontiers(Tile[,] map, List<Room> rooms)
	{
		List<(int, int, bool)>	frontiers = new List<(int, int, bool)>(); 

		int	fx = 0;
		int	fy = 0;

		for (int y = 1; y < map.GetLength(0) - 1; y += 2)
		{
			for (int x = 1; x < map.GetLength(1) - 1; x += 2)
			{
				bool	overlap = false;

				foreach (Room room in rooms)
				{
					if (x >= room.X && x < room.Width && y >= room.Y && y < room.Height)
						overlap = true;
				}

				if (overlap)
					continue ;

				frontiers.Add((fy, fx, false));
				map[y, x] = Tile.Test;
				fx++;
			}
			fy++;
		}

		return (frontiers);
	}

	private static void	ConnectFrontiers(Tile[,] map, List<(int, int, bool)> frontiers)
	{
		(int y, int x, bool connected)	startingPoint = frontiers[0];

		int	roll = _rng.Next(5);
		switch (roll)
		{
			case (0):

				break ;
			case (1):

				break ;
			case (2):

				break ;
			case (3):

				break ;
		}
	}
}
