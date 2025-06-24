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

				// Walls
				if (wallW && wallE)
				{
					if (insideN)
						texMap[y, x] = TextureID.WallNorth;
					else if (insideS)
						texMap[y, x] = TextureID.WallSouth;
				}
				else if (wallN && wallS)
				{
					if (insideW)
						texMap[y, x] = TextureID.WallWest;
					else if (insideE)
						texMap[y, x] = TextureID.WallEast;
				}
				// Corners Walls
				else if (wallS && wallW)
				{
					if (insideSW)
						texMap[y, x] = TextureID.WallCornerSW;
					else if (insideN && insideE)
						texMap[y, x] = TextureID.WallInnerCornerNE;
				}
				else if (wallN && wallW)
				{
					if (insideNW)
						texMap[y, x] = TextureID.WallCornerNW;
					else if (insideS && insideE)
						texMap[y, x] = TextureID.WallInnerCornerSE;
				}
				else if (wallN && wallE)
				{
					if (insideNE)
						texMap[y, x] = TextureID.WallCornerNE;
					else if (insideS && insideW)
						texMap[y, x] = TextureID.WallInnerCornerSW;
				}
				else if (wallS && wallE)
					if (insideSE)
						texMap[y, x] = TextureID.WallCornerSE;
					else if (insideN && insideW)
						texMap[y, x] = TextureID.WallInnerCornerNW;
				// U Shaped Walls
			}
		}

		return (texMap);
	}
}
