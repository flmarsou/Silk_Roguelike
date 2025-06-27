class	Player(int y, int x)
{
	public int	Y { get; set; } = y;
	public int	X { get; set; } = x;
	public int	Health { get; set; } = 10;

	public void	Move(int dy, int dx, Tile[,] map)
	{
		int	newY = this.Y + dy;
		int	newX = this.X + dx;

		if (map[newY, newX] == Tile.Door)
		{
			map[newY, newX] = Tile.DoorOpen;
			return ;
		}

		if (map[newY, newX] != Tile.Wall)
		{
			this.Y = newY;
			this.X = newX;
		}
	}

	public void	Hurt(int hitPoint)	=>	this.Health -= hitPoint;
}
