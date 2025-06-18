public partial class	Program
{
	private static Tile[,]	_map;
	private static Player	_player;
	private static Enemy	_enemy;

	private static void	OnLoad()
	{
		LoadKeyDown();

		_map = Dungeon.GenerateDungeon();

		PlaceEntities();
	}

	private static void	PlaceEntities()
	{
		for (int y = 0; y < _map.GetLength(0); y++)
		{
			for (int x = 0; x < _map.GetLength(1); x++)
			{
				if (_map[y, x] == Tile.PlayerSpawn)
					_player = new Player(y, x);
				if (_map[y, x] == Tile.EnemySpawn)
					_enemy = new Enemy(y, x);
			}
		}
	}
}
