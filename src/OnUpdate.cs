public partial class	Program
{
	private static Tile[,]		_map = null;
	private static TextureID[,]	_texMap = null;

	private static void	OnUpdate(double deltaTime)
	{
		if (_map == null)
		{
			_map = Dungeon.GenerateDungeon();
			_texMap = Dungeon.GenerateTextureMap(_map);
		}
	}
}
