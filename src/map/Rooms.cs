public partial class	Dungeon
{
	private class	Room(int y, int x, int width, int height)
	{
		public int	Y { get; } = y;
		public int	X { get; } = x;
		public int	Height { get; } = y + height;
		public int	Width { get; } = x + width;

		/// <summary>
		/// Returns true if the context room is within the boundries of the param room. <br/>
		/// Returns false otherwise.
		/// </summary>
		public bool	Overlap(Room room, int padding)	=>	!(this.Height + padding <= room.Y
														|| this.Y - padding >= room.Height
														|| this.Width + padding <= room.X
														|| this.X - padding >= room.Width);

		/// <summary>
		/// Returns true if the (y, x) coordinates are inside the context room, including its walls. <br/>
		/// Returns false otherwise.
		/// </summary>
		public bool	IsInRoom(int y, int x)	=>	(y >= this.Y
												&& y < this.Height
												&& x >= this.X
												&& x < this.Width);

		/// <summary>
		/// Returns true if the (y, x) coordinates are inside the context room, excluding its walls. <br/>
		/// Returns false otherwise.
		/// </summary>
		public bool	IsInsideRoom(int y, int x)	=>	(y >= this.Y + 1
													&& y < this.Height - 1
													&& x >= this.X + 1
													&& x < this.Width - 1);

		/// <summary>
		/// Returns the approximate center (y, x) coordinates of the context room.
		/// </summary>
		public (int y, int x)	Center	=>	((Y + Height) / 2, (X + Width) / 2);
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
	private static bool	IsInAnyRoom(List<Room> rooms, int y, int x)
	{
		foreach (Room room in rooms)
			if (room.IsInRoom(y, x))
				return (true);
		return (false);
	}

	/// <summary>
	/// Checks if the (x, y) coordinates are inside any room, excluding its walls.
	/// </summary>
	private static bool	IsInsideAnyRoom(List<Room> rooms, int y, int x)
	{
		foreach (Room room in rooms)
			if (room.IsInsideRoom(y, x))
				return (true);
		return (false);
	}
}
