using OpenTK.Graphics.OpenGL4;


namespace PPR.GPU;

/// <summary>
/// This class represents a shader
/// By default it is a compute shader, but textures and buffers can be added to it
/// </summary>
public class Shader {

    // The pointer to the shader
    public int ShaderID;

    // Type of the shader, it will only support compute shaders for now
    public ShaderType Type = ShaderType.ComputeShader;

    // The source code of the shader
    // (Currently debating whether this should be stored)
    // public string Source;


    /// <summary>
    /// Constructor for the shader
    /// Creates a shader from a source code, retrieved from a file path
    /// </summary>
    public Shader(string shaderPath, int programID) {

        // Read the source code from the file
        string source = File.ReadAllText(shaderPath);

        // Create the shader
        ShaderID = GL.CreateShader(Type);

        // Compile the shader
        GL.ShaderSource(ShaderID, source);
        GL.CompileShader(ShaderID);

        // Attach the shader to the program
        GL.AttachShader(programID, ShaderID);
    }
}