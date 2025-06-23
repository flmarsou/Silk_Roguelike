using Silk.NET.Input;

public partial class	Program
{
	private static void	LoadKeyDown()
	{
		IInputContext	input = _window.CreateInput();

		for (int i = 0; i < input.Keyboards.Count; i++)
			input.Keyboards[i].KeyDown += KeyDown;
	}

	private static void	KeyDown(IKeyboard keyboard, Key key, int keyCode)
	{
		if (key == Key.Escape)
		{
			_window.Close();
			Console.Clear();
		}

		if (key == Key.W)
		{
			_cameraPos.Y -= 0.5f;
		}
		if (key == Key.A)
		{
			_cameraPos.X -= 0.5f;
		}
		if (key == Key.S)
		{
			_cameraPos.Y += 0.5f;
		}
		if (key == Key.D)
		{
			_cameraPos.X += 0.5f;
		}
	}
}
