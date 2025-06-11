class	Program
{
	private static void	Main()
	{
		Tile[,]	map = Dungeon.GenerateDungeon();

		for (int y = 0; y < map.GetLength(0); y++)
		{
			for (int x = 0; x < map.GetLength(1); x++)
			{
				switch (map[y, x])
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
					case (Tile.Test):
						Console.Write("F ");
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
