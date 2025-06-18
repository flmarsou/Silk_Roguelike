using Silk.NET.Maths;
using Silk.NET.Windowing;

public partial class	Program
{
	private static IWindow	_window;

	private static void	Main()
	{
		WindowOptions	options = WindowOptions.Default with
		{
			Size = new Vector2D<int>(800, 600),
			Title = "Silk Roguelike",
		};

		_window = Window.Create(options);

		_window.Load += OnLoad;
		_window.Update += OnUpdate;
		_window.Render += OnRender;

		_window.Run();
	}
}
