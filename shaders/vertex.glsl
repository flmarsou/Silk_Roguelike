#version 460 core

layout (location = 0) in vec2	pos;	// x, y positions
layout (location = 1) in vec2	uv;		// u, v texture coordinates

out vec2						tex;

void	main()
{
	gl_Position = vec4(pos, 0.0, 1.0);
	tex = uv;
}
