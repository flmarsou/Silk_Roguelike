#version 460 core

layout (location = 0) in vec2	pos;
layout (location = 1) in vec2	uv;

uniform mat4					transform;
uniform mat4					projection;

out vec2						tex;

void	main()
{
	gl_Position = projection * transform * vec4(pos, 0.0, 1.0);
	tex = uv;
}
