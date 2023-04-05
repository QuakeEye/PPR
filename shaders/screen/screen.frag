// Only used in Modern OpenGL

#version 330


// Shader input
in vec2 uv;			        // Interpolated texture coordinates
uniform sampler2D pixels;   // Texture sampler


// Shader output
out vec4 outputColor;


// Fragment shader
void main() {

    // Output the colour
    outputColor = texture(pixels, uv);
}