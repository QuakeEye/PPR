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
    private string shaderPath;


    /// <summary>
    /// Constructor for the shader
    /// Creates a shader from a source code, retrieved from a file path
    /// </summary>
    public Shader(string _shaderPath) {

        // Set the correct shader path
        shaderPath = _shaderPath;
    }


    /// <summary>
    /// Function that compiles the shader
    /// </summary>
    public void Compile(int programID) {

        // Read the source code from the file
        string source = File.ReadAllText(shaderPath);

        // Create the shader
        ShaderID = GL.CreateShader(Type);

        // Compile the shader
        GL.ShaderSource(ShaderID, source);
        GL.CompileShader(ShaderID);

        // Attach the shader to the program
        GL.AttachShader(programID, ShaderID);

        // Check if the shader compiled correctly
        string infoLog = GL.GetShaderInfoLog(ShaderID);
        if (infoLog != string.Empty)
            Console.WriteLine(infoLog);
    }
}