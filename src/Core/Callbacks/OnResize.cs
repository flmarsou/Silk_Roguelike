using Silk.NET.Maths;

public partial class	Program
{
	private static int	_windowWidth;
	private static int	_windowHeight;

	private static void	OnResize(Vector2D<int> newSize)
	{
		_windowWidth = _window.FramebufferSize.X;
		_windowHeight = _window.FramebufferSize.Y;
		_gl.Viewport(newSize);
	}
}
