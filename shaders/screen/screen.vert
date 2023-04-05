// Only used in Modern OpenGL

#version 330


// Shader input
in vec3 vPosition;		// Vertex position in normalized device coordinates
in vec2 vUV;			// Vertex uv coordinate


// Shader output
out vec2 uv;				


// Vertex shader
void main() {

	// Forward vertex position; will be interpolated for each fragment
	//  no transformation needed because the user already provided NDC
	gl_Position = vec4(vPosition, 1.0);

	// Forward vertex uv coordinate; will be interpolated for each fragment
	uv = vUV;
}