using System.Numerics;
using Silk.NET.OpenGL;

public partial class	Program
{
	private static readonly int[,]	_map =
	{
		{ 1, 1, 1, 1, 1 },
		{ 1, 0, 0, 0, 1 },
		{ 1, 0, 1, 0, 1 },
		{ 1, 1, 1, 1, 1 },
		{ 1, 0, 0, 0, 1 },
		{ 1, 1, 1, 1, 1 }
	};

	private static unsafe void	OnRender(double deltaTime)
	{
		_gl.Clear((uint)ClearBufferMask.ColorBufferBit);
		_gl.UseProgram(_program);
		_gl.BindVertexArray(_vao);

		int	rows = _map.GetLength(0);
		int	cols = _map.GetLength(1);

		SetProjection(rows, cols);

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
		int	location = _gl.GetUniformLocation(_program, "transform");

		float[]	buffer = new float[16]
		{
			matrix.M11, matrix.M12, matrix.M13, matrix.M14,
			matrix.M21, matrix.M22, matrix.M23, matrix.M24,
			matrix.M31, matrix.M32, matrix.M33, matrix.M34,
			matrix.M41, matrix.M42, matrix.M43, matrix.M44
		};

		fixed (float *tBuffer = buffer)
			_gl.UniformMatrix4(location, 1, false, tBuffer);
	}

	private static unsafe void	SetProjection(int rows, int cols)
	{
		Matrix4x4	projection = Matrix4x4.CreateOrthographicOffCenter(
			left: -0.5f,
			right: cols - 0.5f,
			bottom: -rows + 0.5f,
			top: 0.5f,
			zNearPlane: -1.0f,
			zFarPlane: 1.0f
		);

		int	projectionLocation = _gl.GetUniformLocation(_program, "projection");
		_gl.UniformMatrix4(projectionLocation, 1, false, (float *)&projection);
	}
}
