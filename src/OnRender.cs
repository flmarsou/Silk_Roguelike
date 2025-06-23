using System.Numerics;
using Silk.NET.OpenGL;

public partial class	Program
{
	private static readonly int[,]	_map =
	{
		{ 1, 0, 1, 0, 1 },
		{ 0, 1, 0, 1, 0 },
		{ 1, 0, 1, 0, 1 },
		{ 0, 1, 0, 1, 0 },
		{ 1, 0, 1, 0, 1 },
		{ 0, 1, 0, 1, 0 }
	};

	private static Vector2	_cameraPos = Vector2.Zero;

	private static unsafe void	OnRender(double deltaTime)
	{
		_gl.Clear((uint)ClearBufferMask.ColorBufferBit);
		_gl.UseProgram(_program);
		_gl.BindVertexArray(_vao);

		int	rows = _map.GetLength(0);
		int	cols = _map.GetLength(1);

		SetProjection();

		for (int y = 0; y < rows; y++)
		{
			for (int x = 0; x < cols; x++)
			{
				int		tile = _map[y, x];
				uint	texture = _textures[tile];

				_gl.BindTexture(TextureTarget.Texture2D, texture);

				Matrix4x4	transform = Matrix4x4.CreateTranslation(new Vector3(x, -y, 0));
				SetTransform(transform);

				_gl.DrawElements(PrimitiveType.Triangles, (uint)_indices.Length, DrawElementsType.UnsignedInt, null);
			}
		}
	}

	private static unsafe void SetTransform(Matrix4x4 matrix)
	{
		Matrix4x4	view = Matrix4x4.CreateTranslation(new Vector3(-_cameraPos, 0));
		Matrix4x4	transform = view * matrix;

		int	location = _gl.GetUniformLocation(_program, "transform");

		float[]	buffer = new float[16]
		{
			transform.M11, transform.M12, transform.M13, transform.M14,
			transform.M21, transform.M22, transform.M23, transform.M24,
			transform.M31, transform.M32, transform.M33, transform.M34,
			transform.M41, transform.M42, transform.M43, transform.M44
		};

		fixed (float *tBuffer = buffer)
			_gl.UniformMatrix4(location, 1, false, tBuffer);
	}

	private static unsafe void	SetProjection()
	{
		Matrix4x4	projection = Matrix4x4.CreateOrthographicOffCenter(
			left: 0.0f,
			right: 10.0f,
			bottom: 10.0f,
			top: 0.0f,
			zNearPlane: -1.0f,
			zFarPlane: 1.0f
		);

		int	projectionLocation = _gl.GetUniformLocation(_program, "projection");
		_gl.UniformMatrix4(projectionLocation, 1, false, (float *)&projection);
	}
}
