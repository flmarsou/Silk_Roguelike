public partial class	Dungeon
{
	private class	Room(int x, int y, int width, int height)
	{
		public int	X { get; } = x;
		public int	Y { get; } = y;
		public int	Width { get; } = x + width;
		public int	Height { get; } = y + height;

		public bool				Overlap(Room room, int padding) => !(this.Width + padding <= room.X
																	|| this.X - padding >= room.Width
																	|| this.Height + padding <= room.Y
																	|| this.Y - padding >= room.Height);

		public (int y, int x)	Center => ((Y + Height) / 2, (X + Width) / 2);
	}

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
}
