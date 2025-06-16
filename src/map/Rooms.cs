public partial class	Dungeon
{
	private class	Room(int x, int y, int width, int height)
	{
		public int	X { get; } = x;
		public int	Y { get; } = y;
		public int	Width { get; } = x + width;
		public int	Height { get; } = y + height;

		public bool	Overlap(Room room, int padding)	=>	!(this.Width + padding <= room.X
														|| this.X - padding >= room.Width
														|| this.Height + padding <= room.Y
														|| this.Y - padding >= room.Height);

		public bool	IsInRoom(int x, int y) 			=>	(x >= this.X
														&& x < this.Width
														&& y >= this.Y
														&& y < this.Height);

		public bool	IsInsideRoom(int x, int y)		=>	(x >= this.X + 1
														&& x < this.Width - 1
														&& y >= this.Y + 1
														&& y < this.Height - 1);

		public (int y, int x)	Center =>	((Y + Height) / 2, (X + Width) / 2);
	}

	/// <summary>
	/// Stores the room into the map matrix, filling it with `Floor Tiles` and `Wall Tiles` around.
	/// </summary>
	private static void	AddRoom(Tile[,] map, Room room)
	{
		for (int y = room.Y; y < room.Height; y++)
		{
			for (int x = room.X; x < room.Width; x++)
			{
				// Place `Wall Tiles` on the edges of the room
				if (y == room.Y || y == room.Height - 1 || x == room.X || x == room.Width - 1)
					map[y, x] = Tile.Wall;
				// Place `Floor Tiles` on the inside of the room
				else
					map[y, x] = Tile.Floor;
			}
		}
	}

	/// <summary>
	/// Checks if the (x, y) coordinates are inside any room, including its walls.
	/// </summary>
	private static bool	IsInAnyRoom(List<Room> rooms, int x, int y)
	{
		foreach (Room room in rooms)
			if (room.IsInRoom(x, y))
				return (true);
		return (false);
	}

	/// <summary>
	/// Checks if the (x, y) coordinates are inside any room, excluding its walls.
	/// </summary>
	private static bool	IsInsideAnyRoom(List<Room> rooms, int x, int y)
	{
		foreach (Room room in rooms)
			if (room.IsInsideRoom(x, y))
				return (true);
		return (false);
	}
}
