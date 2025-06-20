#version 460 core

in vec2		tex;

out vec4	color;

layout (binding = 0) uniform sampler2D	tex0;

void	main()
{
	color = texture(tex0, tex);
}
