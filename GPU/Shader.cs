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

    // The Window owner of this shader
    private Window window;


    /// <summary>
    /// Constructor for the shader
    /// Creates a shader from a source code, retrieved from a file path
    /// </summary>
    public Shader(string _shaderPath, Window _window) {

        // Set the correct shader path
        shaderPath = _shaderPath;

        // Set the owner window
        window = _window;
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
        Console.WriteLine(GL.GetShaderInfoLog(ShaderID));

        // Attach the shader to the program
        GL.AttachShader(programID, ShaderID);

        // Set the output texture
        SetOutputTexture(window.ScreenID);
    }


    /// <summary>
    /// Function to set the output texture of the shader
    /// </summary>
    public void SetOutputTexture(int textureID) {

        // Set up the texture to which will be output
        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2D, textureID);
        GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, 
                        new int[] { (int) TextureWrapMode.ClampToEdge });
        GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureWrapT,
                        new int[] { (int) TextureWrapMode.ClampToEdge });
        GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                        new int[] { (int) TextureMagFilter.Linear });
        GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                        new int[] { (int) TextureMinFilter.Linear });
        GL.TexImage2D(  TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba32f, 
                        window.Screen.Width, window.Screen.Height, 0, PixelFormat.Rgba, 
                        PixelType.Float, IntPtr.Zero);
    }
}