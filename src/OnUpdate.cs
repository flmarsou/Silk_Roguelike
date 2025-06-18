public partial class	Program
{
	private static float	_accumulator = 0.0f;
	private const float		_updateInterval = 0.5f;

	private static void	OnUpdate(double deltaTime)
	{
		_accumulator += (float)deltaTime;

		if (_accumulator >= _updateInterval)
		{
			_enemy.Act(_map, _player.Y, _player.X);
			_accumulator = 0.0f;
		}
	}
}
