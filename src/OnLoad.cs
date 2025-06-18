using Silk.NET.OpenGL;

public partial class	Program
{
	private static GL	_gl;

	private static uint	_vao;
	private static uint	_vbo;
	private static uint	_ebo;
	private static uint	_program;

	private static readonly float[]	_vertices =
	{
		//  X      Y
		 0.5f,  0.5f,	// 0 - Top Right
		 0.5f, -0.5f,	// 1 - Bottom Right
		-0.5f, -0.5f,	// 2 - Bottom Left
		-0.5f,  0.5f	// 3 - Top Left
	};

	private static readonly uint[]	_indices =
	{
		0u, 1u, 3u,
		1u, 2u, 3u
	};

	private static readonly string	_vertexCode =
	@"
		#version 460 core

		layout (location = 0) in vec2 aPos;

		void main()
		{
			gl_Position = vec4(aPos, 0.0, 1.0);
		}
	";

	private static readonly string	_fragmentCode =
	@"
		#version 460 core

		out vec4 fragmentColor;

		void main()
		{
			fragmentColor = vec4(1.0, 0.5, 0.2, 1.0);
		}
	";

	private static unsafe void	OnLoad()
	{
		// Get the OpenGL API for drawing to the screen
		_gl = GL.GetApi(_window);

		// Vertex Array Objects (VAO)
		_vao = _gl.GenVertexArray();
		_gl.BindVertexArray(_vao);

		// Vertex Buffer Object (VBO)
		_vbo = _gl.GenBuffer();
		_gl.BindBuffer(BufferTargetARB.ArrayBuffer, _vbo);

		fixed (float *vBuffer = &_vertices[0])
			_gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint)(_vertices.Length * sizeof(float)), vBuffer, BufferUsageARB.StaticDraw);

		// Element Buffer Object (EBO)
		_ebo = _gl.GenBuffer();
		_gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, _ebo);

		fixed (uint *iBuffer = &_indices[0])
			_gl.BufferData(BufferTargetARB.ElementArrayBuffer, (nuint)(_indices.Length * sizeof(uint)), iBuffer, BufferUsageARB.StaticDraw);

		// Vertex Shader
		uint	vertexShader = _gl.CreateShader(ShaderType.VertexShader);
		_gl.ShaderSource(vertexShader, _vertexCode);
		_gl.CompileShader(vertexShader);

		_gl.GetShader(vertexShader, ShaderParameterName.CompileStatus, out int vStatus);
		if (vStatus != (int)GLEnum.True)
			throw new Exception($"Vertex Shader failed to compile: {_gl.GetShaderInfoLog(vertexShader)}");

		// Fragment Shader
		uint	fragmentShader = _gl.CreateShader(ShaderType.FragmentShader);
		_gl.ShaderSource(fragmentShader, _fragmentCode);
		_gl.CompileShader(fragmentShader);

		_gl.GetShader(fragmentShader, ShaderParameterName.CompileStatus, out int fStatus);
		if (fStatus != (int)GLEnum.True)
			throw new Exception($"Fragment Shader failed to compile: {_gl.GetShaderInfoLog(fragmentShader)}");

		// Shader Program
		_program = _gl.CreateProgram();
		_gl.AttachShader(_program, vertexShader);
		_gl.AttachShader(_program, fragmentShader);
		_gl.LinkProgram(_program);

		_gl.GetProgram(_program, ProgramPropertyARB.LinkStatus, out int lStatus);
		if (lStatus != (int)GLEnum.True)
			throw new Exception($"Program Shader failed to link: {_gl.GetProgramInfoLog(_program)}");

		// Delete the no longer used individual shaders
		_gl.DetachShader(_program, vertexShader);
		_gl.DetachShader(_program, fragmentShader);
		_gl.DeleteShader(vertexShader);
		_gl.DeleteShader(fragmentShader);

		// Tell OpenGL how to give the data to the shaders
		_gl.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), null);
		_gl.EnableVertexAttribArray(0);
	}
}
