using System.Numerics;

public partial class	Program
{
	private static Button	_playButton;
	private static Button	_settingsButton;
	private static Button	_exitButton;

	private static Tile[,]		_map = null;
	private static TextureID[,]	_texMap = null;

	private static Player		_player;

	private static double		_enemyActionTimer = 0.0;
	private static double		_enemyActionInterval = 1.0;
	private static Enemy		_enemy;

	// ====================================================================== //
	//                            Main Update Loop                            //
	// ====================================================================== //
	private static void	OnUpdate(double deltaTime)
	{
		switch (_state)
		{
			case (GameState.MainMenu): UpdateMenu(); return ;
			case (GameState.Game): UpdateGame(deltaTime); return ;
		}
	}

	// ====================================================================== //
	//                             Menu Updating                              //
	// ====================================================================== //
	private static void	UpdateMenu()
	{
		_playButton = new Button(_windowWidth / 2, _windowHeight / 2, 300, 100);
		_settingsButton = new Button(_windowWidth / 2, _windowHeight / 2 + 110, 300, 100);
		_exitButton = new Button(_windowWidth / 2, _windowHeight / 2 + 220, 300, 100);
	}

	// ====================================================================== //
	//                             Game Updating                              //
	// ====================================================================== //
	private static void	UpdateGame(double deltaTime)
	{
		if (_map == null)
		{
			_map = Dungeon.GenerateDungeon();
			_texMap = Dungeon.GenerateTextureMap(_map);
			(int y, int x) = FindPlayerPosition();
			_player = new Player(y, x);
			(int y2, int x2) = FindEnemyPosition();
			_enemy = new Enemy(y2, x2);
		}

		_enemyActionTimer += deltaTime;

		if (_enemyActionTimer >= _enemyActionInterval)
		{
			_enemy.Act(_map, _player.Y, _player.X);
			_enemyActionTimer = 0.0;
		}
	}

	private static (int y, int x)	FindPlayerPosition()
	{
		int	height = _map.GetLength(0);
		int	width = _map.GetLength(1);

		for (int y = 0; y < height; y++)
			for (int x = 0; x < width; x++)
				if (_map[y, x] == Tile.PlayerSpawn)
					return ((y, x));

		return (-1, -1);
	}

	private static (int y, int x)	FindEnemyPosition()
	{
		int	height = _map.GetLength(0);
		int	width = _map.GetLength(1);

		for (int y = 0; y < height; y++)
			for (int x = 0; x < width; x++)
				if (_map[y, x] == Tile.EnemySpawn)
					return ((y, x));

		return (-1, -1);
	}
}
