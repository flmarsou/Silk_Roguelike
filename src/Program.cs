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
		_window.Closing += OnClosing;
		_window.FramebufferResize += OnResize;

		_window.Run();
	}

	private static void	OnResize(Vector2D<int> newSize)
	{
		_gl.Viewport(newSize);
	}

	private static void	OnClosing()
	{
		_gl.DeleteBuffer(_vbo);
		_gl.DeleteBuffer(_ebo);
		_gl.DeleteVertexArray(_vao);
		_gl.DeleteProgram(_program);
	}
}
