class	Enemy(int x, int y)
{
	public int	X { get; set; } = x;
	public int	Y { get; set; } = y;

	public void	Move(int dx, int dy, Tile[,] map)
	{
		int	newX = X + dx;
		int	newY = Y + dy;

		if (map[newY, newX] != Tile.Wall)
		{
			X = newX;
			Y = newY;
		}
	}
}
