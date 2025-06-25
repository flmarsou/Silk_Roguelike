using System.Numerics;
using Silk.NET.Input;

public partial class	Program
{
	private static float	_scrollAmount = 0;
	private static bool		_leftMouseButtonPressed = false;
	private static Vector2	_lastMousePosition;

	private static void	InitInput()
	{
		IInputContext	input = _window.CreateInput();

		for (int i = 0; i < input.Keyboards.Count; i++)
			input.Keyboards[i].KeyDown += KeyDown;

		for (int i = 0; i < input.Mice.Count; i++)
		{
			input.Mice[i].Scroll += Scroll;
			input.Mice[i].MouseDown += MouseDown;
			input.Mice[i].MouseUp += MouseUp;
			input.Mice[i].MouseMove += MouseMove;
		}
	}

	private static void	KeyDown(IKeyboard keyboard, Key key, int keyCode)
	{
		if (key == Key.Escape)
		{
			_window.Close();
			Console.Clear();
		}
	}

	private static void	Scroll(IMouse mouse, ScrollWheel scroll)
	{
		_scrollAmount = scroll.Y;
	}

	private static void	MouseDown(IMouse mouse, MouseButton button)
	{
		if (button == MouseButton.Left)
		{
			_leftMouseButtonPressed = true;
			_lastMousePosition = mouse.Position;
		}
	}

	private static void	MouseUp(IMouse mouse, MouseButton button)
	{
		if (button == MouseButton.Left)
			_leftMouseButtonPressed = false;
	}

	private static void	MouseMove(IMouse mouse, Vector2 position)
	{
		// --- Camera Movements ---
		if (_leftMouseButtonPressed)
		{
			Vector2	delta = position - _lastMousePosition;
			_lastMousePosition = position;

			Vector2	newPos = _cameraPos - delta * (_zoom / (_windowHeight * 2.0f));

			newPos.Y = Math.Clamp(newPos.Y, 0, _map.GetLength(0) / 2);
			newPos.X = Math.Clamp(newPos.X, 0, _map.GetLength(1) / 2);

			_cameraPos = newPos;
		}
	}
}
