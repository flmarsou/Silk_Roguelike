using System.Numerics;
using Silk.NET.OpenGL;

public partial class	Program
{
	private static Vector2	_cameraPos = Vector2.Zero;
	private static float	_zoom = 10.0f;

	// ====================================================================== //
	//                            Main Render Loop                            //
	// ====================================================================== //
	private static unsafe void	OnRender(double deltaTime)
	{
		// --- Clear ---
		_gl.Clear((uint)ClearBufferMask.ColorBufferBit);
		_gl.ClearColor(0.294f, 0.224f, 0.243f, 1.0f);

		// --- Setup ---
		_gl.UseProgram(_program);
		_gl.BindVertexArray(_vao);

		// --- Zoom Handling ---
		if (_scrollAmount != 0)
		{
			_zoom -= _scrollAmount * (float)deltaTime * 100.0f;
			_zoom = Math.Clamp(_zoom, 4.99f, _map.GetLength(0));
			_scrollAmount = 0;
		}

		// --- Draw Map ---
		int	rows = _map.GetLength(0);
		int	cols = _map.GetLength(1);

		SetProjection();

		for (int y = 0; y < rows; y++)
		{
			for (int x = 0; x < cols; x++)
			{
				TextureID	tile = _texMap[y, x];
				if (tile == TextureID.Empty)
					continue ;

				uint	texture = _textures[(uint)tile];

				_gl.BindTexture(TextureTarget.Texture2D, texture);

				SetTransform(Matrix4x4.CreateTranslation(new Vector3(x, y, 0)));

				_gl.DrawElements(PrimitiveType.Triangles, (uint)_indices.Length, DrawElementsType.UnsignedInt, null);
			}
		}
	}

	// ====================================================================== //
	//                             Shader Helpers                             //
	// ====================================================================== //
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
		float	aspect = _windowWidth / (float)_windowHeight;
		float	viewHeight = _zoom;
		float	viewWidth = viewHeight * aspect;

		Matrix4x4	projection = Matrix4x4.CreateOrthographicOffCenter(
			left: _cameraPos.X - (viewWidth / 2f),
			right: _cameraPos.X + (viewWidth / 2f),
			bottom: _cameraPos.Y + (viewHeight / 2f),
			top: _cameraPos.Y - (viewHeight / 2f),
			zNearPlane: -1.0f,
			zFarPlane: 1.0f
		);

		int	projectionLocation = _gl.GetUniformLocation(_program, "projection");
		_gl.UniformMatrix4(projectionLocation, 1, false, (float *)&projection);
	}
}
