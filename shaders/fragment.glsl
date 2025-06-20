#version 460 core

in vec2	frag_TextureCoord;

out vec4	fragmentColor;

uniform sampler2D	uTexture;

void	main()
{
	fragmentColor = texture(uTexture, frag_TextureCoord);
}
