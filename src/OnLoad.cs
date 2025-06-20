using Silk.NET.OpenGL;
using StbImageSharp;

public partial class	Program
{
	private static GL	_gl;

	private static uint	_vao;
	private static uint	_vbo;
	private static uint	_ebo;

	private static uint	_program;

	private static readonly uint[]	_textures = new uint[2];

	private static readonly float[]	_vertices =
	{
		// Pos (x, y)      UV (u, v)
		 0.5f,  0.5f,	 1.0f,  1.0f,	// 0 - Top Right
		 0.5f, -0.5f,	 1.0f,  0.0f,	// 1 - Bottom Right
		-0.5f, -0.5f,	 0.0f,  0.0f,	// 2 - Bottom Left
		-0.5f,  0.5f,	 0.0f,  1.0f	// 3 - Top Left
	};

	private static readonly uint[]	_indices =
	{
		0u, 1u, 3u,
		1u, 2u, 3u
	};

	private static readonly string	_vertexCode = File.ReadAllText("shaders/vertex.glsl");
	private static readonly string	_fragmentCode = File.ReadAllText("shaders/fragment.glsl");

	private static unsafe void	OnLoad()
	{
		_gl = GL.GetApi(_window);

		InitBuffers();
		InitShaders();

		LoadTexture("assets/wall.png", 0);
		LoadTexture("assets/floor.png", 1);
 
		_gl.UseProgram(_program);
	}

	/// <summary>
	/// Initializes vertex data by generating and binding a VAO, VBO, and EBO. <br/>
	/// Configures the vertex attribute pointers for position and texture coordinates.
	/// </summary>
	private static unsafe void	InitBuffers()
	{
		// Vertex Array Objects (VAO)
		// The VAO stores the configuration of vertex attribute pointers
		_vao = _gl.GenVertexArray();
		_gl.BindVertexArray(_vao);

		// Vertex Buffer Object (VBO)
		// The VBO stores the actual vertex data (positions + texture coordinates)
		_vbo = _gl.GenBuffer();
		_gl.BindBuffer(BufferTargetARB.ArrayBuffer, _vbo);

		fixed (float *vBuffer = &_vertices[0])
			_gl.BufferData(
				target: BufferTargetARB.ArrayBuffer,
				size: (nuint)(_vertices.Length * sizeof(float)),
				data: vBuffer,
				usage: BufferUsageARB.StaticDraw
			);

		// Element Buffer Object (EBO)
		// The EBO stores the indices used for indexed drawing
		_ebo = _gl.GenBuffer();
		_gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, _ebo);

		fixed (uint *iBuffer = &_indices[0])
			_gl.BufferData(
				target: BufferTargetARB.ElementArrayBuffer,
				size: (nuint)(_indices.Length * sizeof(uint)),
				data: iBuffer,
				usage: BufferUsageARB.StaticDraw
			);

		// Define the layout for the vertex position attribute (location = 0)
		_gl.EnableVertexAttribArray(0);
		_gl.VertexAttribPointer(
			index: 0,
			size: 2,
			type: VertexAttribPointerType.Float,
			normalized: false,
			stride: 4 * sizeof(float),
			pointer: (void *)0
		);

		// Define the layout for the texture coordinate attribute (location = 1)
		_gl.EnableVertexAttribArray(1);
		_gl.VertexAttribPointer(
			index: 1,
			size: 2,
			type: VertexAttribPointerType.Float,
			normalized: false,
			stride: 4 * sizeof(float),
			pointer: (void *)(2 * sizeof(float))
		);
	}

	/// <summary>
	/// Compiles the vertex and fragment shaders and links them into a Shader Program. <br/>
	/// Throws exceptions if any step fails. <br/>
	/// Cleans up shader objects after linking as they are no longer needed.
	/// </summary>
	private static void	InitShaders()
	{
		// Create and compile the Vertex Shader
		uint	vertexShader = _gl.CreateShader(ShaderType.VertexShader);
		_gl.ShaderSource(vertexShader, _vertexCode);
		_gl.CompileShader(vertexShader);

		_gl.GetShader(vertexShader, ShaderParameterName.CompileStatus, out int vStatus);
		if (vStatus != (int)GLEnum.True)
			throw new Exception($"Vertex Shader failed to compile: {_gl.GetShaderInfoLog(vertexShader)}");

		// Create and compile the Fragment Shader
		uint	fragmentShader = _gl.CreateShader(ShaderType.FragmentShader);
		_gl.ShaderSource(fragmentShader, _fragmentCode);
		_gl.CompileShader(fragmentShader);

		_gl.GetShader(fragmentShader, ShaderParameterName.CompileStatus, out int fStatus);
		if (fStatus != (int)GLEnum.True)
			throw new Exception($"Fragment Shader failed to compile: {_gl.GetShaderInfoLog(fragmentShader)}");

		// Create, attach, and link both shaders to the Shader Program
		_program = _gl.CreateProgram();
		_gl.AttachShader(_program, vertexShader);
		_gl.AttachShader(_program, fragmentShader);
		_gl.LinkProgram(_program);

		_gl.GetProgram(_program, ProgramPropertyARB.LinkStatus, out int lStatus);
		if (lStatus != (int)GLEnum.True)
			throw new Exception($"Program Shader failed to link: {_gl.GetProgramInfoLog(_program)}");

		// Detach and Delete shaders after successful linking
		_gl.DetachShader(_program, vertexShader);
		_gl.DetachShader(_program, fragmentShader);
		_gl.DeleteShader(vertexShader);
		_gl.DeleteShader(fragmentShader);
	}

	/// <summary>
	/// Loads an RGBa texture from Disk into GPU Memory and returns its OpenGL handle. <br/>
	/// The texture is configured with repeat wrapping and nearest-neighbor filtering.
	/// </summary>
	private static unsafe void	LoadTexture(string path, uint index)
	{
		// Load the image file into memory and decode it as RGBa
		ImageResult	result = ImageResult.FromMemory(File.ReadAllBytes(path), ColorComponents.RedGreenBlueAlpha);

		// Generate and bind a new OpenGL texture object
		uint	texture = _gl.GenTexture();
		_gl.BindTexture(TextureTarget.Texture2D, texture);

		// Define the decoded image data to the GPU
		fixed (byte *tBuffer = result.Data)
			_gl.TexImage2D(
				target: TextureTarget.Texture2D,
				level: 0, 
				internalformat: InternalFormat.Rgba,
				width: (uint)result.Width,
				height: (uint)result.Height,
				border: 0,
				format: PixelFormat.Rgba,
				type: PixelType.UnsignedByte,
				pixels: tBuffer
			);

		// Set texture wrapping mode on both S (horizontal) and T (vertical) axes
		int	wrap = (int)TextureWrapMode.Repeat;
		_gl.TexParameterI(GLEnum.Texture2D, GLEnum.TextureWrapS, in wrap);
		_gl.TexParameterI(GLEnum.Texture2D, GLEnum.TextureWrapT, in wrap);
		// Set texture filtering mode to nearest for both minification and magnification
		int	filter = (int)TextureMinFilter.Nearest;
		_gl.TexParameterI(GLEnum.Texture2D, GLEnum.TextureMinFilter, in filter);
		_gl.TexParameterI(GLEnum.Texture2D, GLEnum.TextureMagFilter, in filter);

		// Unbind the texture to prevent accidental modification
		_gl.BindTexture(TextureTarget.Texture2D, 0);

		_textures[index] = texture;
	}
}
