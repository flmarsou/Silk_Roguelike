using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.Windowing;

class	Program
{
	private static IWindow	_window;
	private static Tile[,]	_map;
	private static Player	_player;
	private static Enemy	_enemy;

	private static void	Main()
	{
		WindowOptions	options = WindowOptions.Default with
		{
			Size = new Vector2D<int>(800, 600),
			Title = "Silk Roguelike",
		};

		_window = Window.Create(options);

		_window.Load += OnLoad;

		_window.Run();
	}

	private static void	OnLoad()
	{
		IInputContext	input = _window.CreateInput();

		for (int i = 0; i < input.Keyboards.Count; i++)
			input.Keyboards[i].KeyDown += KeyDown;

		_map = Dungeon.GenerateDungeon();

		PlacePlayer();
		PlaceEnemy();
	}

	private static void	PlacePlayer()
	{
		for (int y = 0; y < _map.GetLength(0); y++)
		{
			for (int x = 0; x < _map.GetLength(1); x++)
			{
				if (_map[y, x] == Tile.Floor)
				{
					_player = new Player(x, y);
					return ;
				}
			}
		}
	}

	private static void	PlaceEnemy()
	{
		for (int y = _map.GetLength(0) - 2; y >= 0 ; y--)
		{
			for (int x = _map.GetLength(1) - 2; x >= 0 ; x--)
			{
				if (_map[y, x] == Tile.Floor)
				{
					_enemy = new Enemy(x, y);
					return ;
				}
			}
		}
	}

	private static void	KeyDown(IKeyboard keyboard, Key key, int keyCode)
	{
		if (key == Key.Escape)
			_window.Close();

		if (key == Key.W)
		{
			_player.Move(0, -1, _map);
			DisplayMap();
		}
		if (key == Key.A)
		{
			_player.Move(-1, 0, _map);
			DisplayMap();
		}
		if (key == Key.S)
		{
			_player.Move(0, 1, _map);
			DisplayMap();
		}
		if (key == Key.D)
		{
			_player.Move(1, 0, _map);
			DisplayMap();
		}
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

				if (_enemy.X == x && _enemy.Y == y)
				{
					Console.Write("E ");
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
						Console.Write(". ");
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
