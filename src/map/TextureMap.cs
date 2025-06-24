public partial class	Dungeon
{
	public static TextureID[,]	GenerateTextureMap(Tile[,] map)
	{
		TextureID[,]	texMap = new TextureID[_mapHeight, _mapWidth];

		for (int y = 0; y < _mapHeight; y++)
		{
			for (int x = 0; x < _mapWidth; x++)
			{
				texMap[y, x] = TextureID.Empty;

				if (map[y, x] != Tile.Wall)
					continue ;

				bool	wallN = y > 0 && map[y - 1, x] == Tile.Wall;
				bool	wallS = y < _mapHeight - 1 && map[y + 1, x] == Tile.Wall;
				bool	wallW = x > 0 && map[y, x - 1] == Tile.Wall;
				bool	wallE = x < _mapWidth - 1 && map[y, x + 1] == Tile.Wall;

				bool	insideN = y > 0 && map[y - 1, x] != Tile.Wall && map[y - 1, x] != Tile.Empty;
				bool	insideS = y < _mapHeight - 1 && map[y + 1, x] != Tile.Wall && map[y + 1, x] != Tile.Empty;
				bool	insideW = x > 0 && map[y, x - 1] != Tile.Wall && map[y, x - 1] != Tile.Empty;
				bool	insideE = x < _mapWidth - 1 && map[y, x + 1] != Tile.Wall && map[y, x + 1] != Tile.Empty;

				bool	insideSW = y < _mapHeight - 1 && x > 0 && map[y + 1, x - 1] != Tile.Wall && map[y + 1, x - 1] != Tile.Empty;
				bool	insideNW = y > 0 && x > 0 && map[y - 1, x - 1] != Tile.Wall && map[y - 1, x - 1] != Tile.Empty;
				bool	insideNE = y > 0 && x < _mapWidth - 1 && map[y - 1, x + 1] != Tile.Wall && map[y - 1, x + 1] != Tile.Empty;
				bool	insideSE = y < _mapHeight - 1 && x < _mapWidth - 1 && map[y + 1, x + 1] != Tile.Wall && map[y + 1, x + 1] != Tile.Empty;

				if (wallW && wallE)								// North and South walls
				{
					if (insideS)
						texMap[y, x] = TextureID.WallNorth;
					else if (insideN)
						texMap[y, x] = TextureID.WallSouth;
				}
				else if (wallN && wallS)						// West and East walls
				{
					if (insideE)
						texMap[y, x] = TextureID.WallWest;
					else if (insideW)
						texMap[y, x] = TextureID.WallEast;
				}
				else if (wallS && wallW && insideSW)			// Top Right Corner walls
					texMap[y, x] = TextureID.WallEast;
				else if (wallN && wallW && insideNW)			// Bottom Right Corner walls
					texMap[y, x] = TextureID.WallCornerEast;
				else if (wallN && wallE && insideNE)			// Bottom Left Corner walls
					texMap[y, x] = TextureID.WallCornerWest;
				else if (wallS && wallE && insideSE)			// Top Left Corner walls
					texMap[y, x] = TextureID.WallWest;
			}
		}

		return (texMap);
	}
}
