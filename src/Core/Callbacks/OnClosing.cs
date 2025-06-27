public partial class	Program
{
	private static void	OnClosing()
	{
		_gl.DeleteBuffer(_vbo);
		_gl.DeleteBuffer(_ebo);
		_gl.DeleteVertexArray(_vao);
		_gl.DeleteProgram(_program);
	}
}
