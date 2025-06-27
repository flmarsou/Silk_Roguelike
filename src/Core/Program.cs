using Silk.NET.Maths;
using Silk.NET.Windowing;

public partial class	Program
{
	private static IWindow	_window;

	private enum	GameState
	{
		MainMenu,
		Game
	}

	private static GameState	_state = GameState.MainMenu;

	private static void	Main()
	{
		WindowOptions	options = WindowOptions.Default with
		{
			Size = new Vector2D<int>(1024, 576),
			Title = "Silk Roguelike",
		};

		_window = Window.Create(options);

		_window.Load += OnLoad;
		_window.Update += OnUpdate;
		_window.Render += OnRender;
		_window.FramebufferResize += OnResize;
		_window.Closing += OnClosing;

		_window.Run();
	}
}
