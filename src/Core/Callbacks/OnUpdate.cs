public partial class	Program
{
	private static Button	_playButton;
	private static Button	_settingsButton;
	private static Button	_exitButton;

	private static Tile[,]		_map = null;
	private static TextureID[,]	_texMap = null;

	// ====================================================================== //
	//                            Main Update Loop                            //
	// ====================================================================== //
	private static void	OnUpdate(double deltaTime)
	{
		switch (_state)
		{
			case (GameState.MainMenu): UpdateMenu(); return ;
			case (GameState.Game): UpdateGame(); return ;
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
	private static void	UpdateGame()
	{
		if (_map == null)
		{
			_map = Dungeon.GenerateDungeon();
			_texMap = Dungeon.GenerateTextureMap(_map);
		}
	}
}
