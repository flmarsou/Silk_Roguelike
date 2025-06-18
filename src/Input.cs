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
}
