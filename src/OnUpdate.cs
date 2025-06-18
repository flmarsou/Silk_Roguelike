public partial class	Program
{
	private static float	_accumulator = 0.0f;
	private const float		_updateInterval = 0.2f;

	private static void	OnUpdate(double deltaTime)
	{
		_accumulator += (float)deltaTime;

		if (_accumulator >= _updateInterval)
		{
			_accumulator = 0.0f;
		}
	}
}
