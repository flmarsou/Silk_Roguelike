using System.Numerics;

public class	Button(float x, float y, float width, float height)
{
	public float	X { get; set; } = x;
	public float	Y { get; set; } = y;
	public float	Width { get; set; } = width;
	public float	Height { get; set; } = height;
	public bool		Hovered { get; private set; } = false;

	public float	Left => X - Width / 2;
	public float	Right => X + Width / 2;
	public float	Top => Y - Height / 2;
	public float	Bottom => Y + Height / 2;

	public void		IsHovered(Vector2 mouse)
	{
		Hovered = mouse.X >= Left && mouse.X <= Right && mouse.Y >= Top && mouse.Y <= Bottom;
	}
}
