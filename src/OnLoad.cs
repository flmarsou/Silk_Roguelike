public partial class	Program
{
	private static Tile[,]	_map;
	private static Player	_player;

	private static void	OnLoad()
	{
		LoadKeyDown();

		_map = Dungeon.GenerateDungeon();

		PlacePlayer();
	}

	private static void	PlacePlayer()
	{
		for (int y = 0; y < _map.GetLength(0); y++)
		{
			for (int x = 0; x < _map.GetLength(1); x++)
			{
				if (_map[y, x] == Tile.PlayerSpawn)
				{
					Console.WriteLine(y + " " + x);
					_player = new Player(x, y);
					Console.WriteLine(_player.Y + " " + _player.X);
					return ;
				}
			}
		}
	}
}
