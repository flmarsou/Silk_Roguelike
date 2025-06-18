public partial class	Program
{
	private static void	OnRender(double deltaTime)
	{
		DisplayMap();
	}

	private static void	DisplayMap()
	{
		Console.Clear();

		for (int y = 0; y < _map.GetLength(0); y++)
		{
			for (int x = 0; x < _map.GetLength(1); x++)
			{
				if (_player.X == x && _player.Y == y)
				{
					Console.Write("P ");
					continue ;
				}

				switch (_map[y, x])
				{
					case (Tile.Empty):
						Console.Write("  ");
						break ;
					case (Tile.Wall):
						Console.Write("# ");
						break ;
					case (Tile.Floor):
						Console.Write(". ");
						break ;
					case (Tile.Tunnel):
						Console.Write(". ");
						break ;
					case (Tile.Door):
						Console.Write("D ");
						break ;
					default:
						Console.Write("? ");
						break ;
				}
			}
			Console.WriteLine();
		}
	}
}
