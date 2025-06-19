using Silk.NET.OpenGL;

public partial class	Program
{
	private static unsafe void	OnRender(double deltaTime)
	{
		_gl.Clear((uint)ClearBufferMask.ColorBufferBit);

		_gl.BindTexture(TextureTarget.Texture2D, _textures[0]);

		_gl.DrawElements(PrimitiveType.Triangles, (uint)_indices.Length, DrawElementsType.UnsignedInt, null);
	}

	// private static void	DisplayMap()
	// {
	// 	Console.Clear();

	// 	for (int y = 0; y < _map.GetLength(0); y++)
	// 	{
	// 		for (int x = 0; x < _map.GetLength(1); x++)
	// 		{
	// 			// Player
	// 			if (_player.Y == y && _player.X == x)
	// 			{
	// 				if (_player.Health >= 10)
	// 					Console.ForegroundColor = ConsoleColor.Green;
	// 				else if (_player.Health > 5)
	// 					Console.ForegroundColor = ConsoleColor.Yellow;
	// 				else
	// 					Console.ForegroundColor = ConsoleColor.Red;
	// 				Console.Write("P ");

	// 				Console.ResetColor();
	// 				continue ;
	// 			}

	// 			// Enemy
	// 			if (_enemy.Y == y && _enemy.X == x)
	// 			{
	// 				Console.Write("E ");
	// 				continue ;
	// 			}

	// 			// Tiles
	// 			switch (_map[y, x])
	// 			{
	// 				case (Tile.Empty): Console.Write("  "); break ;
	// 				case (Tile.Wall): Console.Write("# "); break ;
	// 				case (Tile.Floor): Console.Write(". "); break ;
	// 				case (Tile.Tunnel): Console.Write(". "); break ;
	// 				case (Tile.Door): Console.Write("D "); break ;
	// 				case (Tile.DoorOpen): Console.Write("d "); break ;
	// 				default: Console.Write("? "); break ;
	// 			}
	// 		}
	// 		Console.WriteLine();
	// 	}

	// 	Console.WriteLine("Player HP: " + _player.Health);
	// }
}
