using OpenTK;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using PPR.GPU;

namespace PPR;


/// <summary>
/// The main window class, which is used to render the application
/// </summary>
public class Window : GameWindow {

    // Keep track of whether this window should be closed off
    bool terminated = false;    // True if app is terminated, will result in closing of app


    // Texture and ID for per-pixel rendering
    public Target Screen;
    public int ScreenID;


    // List of functions that will be called on app initialisation
    List<Action> initFunctions = new List<Action>();

    // List of functions that will be called on every frame update
    List<Action> updateFunctions = new List<Action>();

    // List of functions that will be called on every frame render
    List<Action> renderFunctions = new List<Action>();


    // OpenGL pointers for the vertex and fragment shaders
    public int vertexArrayObject;
    public int vertexBufferObject;
    public int programID;   // Pointer to the main opengl program

    // Vertices set-up that make sure the texture can be drawn to the window
    // All the data for the vertices interleaved in one array:
    // - XYZ in normalized device coordinates
    // - UV
    readonly float[] vertices =
    { //  X      Y     Z     U     V
        -1.0f, -1.0f, 0.0f, 0.0f, 1.0f, // bottom-left  2-----3 triangles:
        1.0f, -1.0f, 0.0f, 1.0f, 1.0f,  // bottom-right | \   |     012
        -1.0f,  1.0f, 0.0f, 0.0f, 0.0f, // top-left     |   \ |     123
        1.0f,  1.0f, 0.0f, 1.0f, 0.0f,  // top-right    0-----1
    };


    // Pointer to the opengl program represented by this window instance
    public int ProgramID { get; private set; }

    // The GPU Shader pipeline
    public List<Shader> Pipeline { get; private set; } = new List<Shader>();


    /// <summary>
    /// Constructor for the window class
    /// </summary>
    /// <param name="size">The size of the window</param>
    public Window(Vector2i size)
        : base (GameWindowSettings.Default, new NativeWindowSettings()
        {
            Size = size,
            Title = "Per-Pixel Rendering"
        }) { }


    /// <summary>
    /// Called when the window is created
    /// </summary>
    protected override void OnLoad() {

        base.OnLoad();

        // Application is initialised empty
        GL.ClearColor(0, 0, 0, 0);
        GL.Disable(EnableCap.DepthTest);

        // Initialise the screen render target
        Screen = new Target(Size.X, Size.Y);

        // Load all opengl related pointers, buffers and others
        LoadOpenGL(Screen);

        // Call all the functions that were added to the initFunctions list
        foreach (var function in initFunctions)
            function.Invoke();

        // Set up the input helper
        InputHelper.PreviousMouseState = MouseState;
        InputHelper.CurrentMouseState = MouseState;
    }


    /// <summary>
    /// This function loads in all the opengl related pointers, buffers and others
    /// It should be ignored by the user of the template
    /// </summary>
    /// <param name="screen">The screen render target</param>
    void LoadOpenGL(Target screen) {

        ScreenID = screen.GenTexture();

        // Setting up a Modern OpenGL pipeline takes a lot of code
        // Vertex Array Object: will store the meaning of the data in the buffer
        vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(vertexArrayObject);


        // Vertex Buffer Object: a buffer of raw data
        vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);


        // Vertex Shader
        string shaderSource = File.ReadAllText("shaders/screen/screen.vert");
        int vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, shaderSource);
        GL.CompileShader(vertexShader);
        GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out int vStatus);


        if (vStatus != (int)All.True) {

            string log = GL.GetShaderInfoLog(vertexShader);
            throw new Exception($"Error occurred whilst compiling vertex shader ({vertexShader}):\n{log}");
        }


        // Fragment Shader
        shaderSource = File.ReadAllText("shaders/screen/screen.frag");
        int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, shaderSource);
        GL.CompileShader(fragmentShader);
        GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out int fStatus);

        if (fStatus != (int)All.True) {

            string log = GL.GetShaderInfoLog(fragmentShader);
            throw new Exception($"Error occurred whilst compiling fragment shader ({fragmentShader}):\n{log}");
        }


        // Program: a set of shaders to be used together in a pipeline
        programID = GL.CreateProgram();
        GL.AttachShader(programID, vertexShader);
        GL.AttachShader(programID, fragmentShader);
        GL.LinkProgram(programID);
        GL.GetProgram(programID, GetProgramParameterName.LinkStatus, out int pStatus);

        if (pStatus != (int)All.True) {

            string log = GL.GetProgramInfoLog(programID);
            throw new Exception($"Error occurred whilst linking program ({programID}):\n{log}");
        }

        // The program pipeline has been set up, we can now delete the shaders
        GL.DetachShader(programID, vertexShader);
        GL.DetachShader(programID, fragmentShader);
        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);

        // All the draw calls should now be directed through this pipeline
        GL.UseProgram(programID);

        // Tell the VAO which part of the VBO data should go to each shader input
        int vPosLocation = GL.GetAttribLocation(programID, "vPosition");
        GL.EnableVertexAttribArray(vPosLocation);
        GL.VertexAttribPointer(vPosLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

        int vUVlocation = GL.GetAttribLocation(programID, "vUV");
        GL.EnableVertexAttribArray(vUVlocation);
        GL.VertexAttribPointer(vUVlocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

        // Connect the texture to the shader uniform variable
        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2D, ScreenID);
        GL.Uniform1(GL.GetUniformLocation(programID, "pixels"), 0); // Retrieves a location in the fragment shader

        if (Settings.Debug)
            Console.WriteLine("OpenGL was initialised without errors");
    }


    /// <summary>
    /// Code that runs when the window gets closed
    /// </summary>
    protected override void OnUnload() {

        base.OnUnload();

        // Called upon app close
        GL.DeleteTextures(1, ref ScreenID);

        // Close the program
        GL.DeleteProgram(programID);

        // Close the windows and terminate the app
        Environment.Exit(0);
    }


    /// <summary>
    /// This function is called when the window is resized by the user
    /// </summary>
    /// <param name="e">The event arguments</param>
    protected override void OnResize(ResizeEventArgs e) {

        base.OnResize(e);

        GL.Viewport(0, 0, e.Width, e.Height);   // Set the viewport to the new window size

        // Resize the screen render target
        Screen = new Target(e.Width, e.Height);
        ScreenID = Screen.GenTexture();

        if (Settings.Debug)
            Console.WriteLine($"Window was resized to {e.Width}x{e.Height}");
    }


    /// <summary>
    /// This function is called as a frame update function, which occurs every frame
    /// </summary>
    /// <param name="e">The event arguments</param>
    protected override void OnUpdateFrame(FrameEventArgs e) {

        base.OnUpdateFrame(e);

        // Get the new keyboard and mouse states
        var keyboard = KeyboardState;
        var mouse = MouseState;

        // Update our static input helping class
        InputHelper.Update(keyboard, mouse);

        // Make sure to call all functions that need to be called every frame
        foreach (var function in updateFunctions)
            function.Invoke();

        // Quit the program once escape key has been pressed
        if (keyboard[Keys.Escape]) terminated = true;
    }


    /// <summary>
    /// This function is called as a frame render function, which occurs every frame
    /// </summary>
    /// <param name="e">The event arguments</param>
    protected override void OnRenderFrame(FrameEventArgs e) {

        base.OnRenderFrame(e);

        // If the program has been terminated, close the window
        if (terminated) { Close(); return; }

        // Call all functions that need to be called every frame
        foreach (var function in renderFunctions)
            function.Invoke();

        // Convert MyApplication.screen to OpenGL texture
        GL.BindTexture(TextureTarget.Texture2D, ScreenID);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                        Screen.Width, Screen.Height, 0,
                        PixelFormat.Bgra,
                        PixelType.UnsignedByte, Screen.Pixels
                        );

        // Draw screen filling quad
        GL.BindVertexArray(vertexArrayObject);
        GL.UseProgram(programID);
        GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);

        // Tell OpenTK we're done rendering
        SwapBuffers();
    }


    /// <summary>
    /// Subscribe a function to the list of initialisation functions
    /// </summary>
    /// <param name="function">The function to be called</param>
    public void SubscribeInit(Action function) {

        initFunctions.Add(function);
    }


    /// <summary>
    /// Subscribe a function to the list of update functions
    /// </summary>
    /// <param name="function">The function to be called</param>
    public void SubscribeUpdate(Action function) {

        updateFunctions.Add(function);
    }
    
    /// <summary>
    /// Subscribe a function to the list of update functions at a specific place
    /// </summary>
    /// <param name="function">The function to be called</param>
    /// <param name="index">The index to insert the function at</param>
    public void SubscribeUpdateAt(Action function, int index) {

        updateFunctions.Insert(index, function);
    }


    /// <summary>
    /// Subscribe a function to the list of render functions
    /// </summary>
    /// <param name="function">The function to be called</param>
    public void SubscribeRender(Action function) {

        renderFunctions.Add(function);
    }
    
    /// <summary>
    /// Subscribe a function to a specific place in the list of render functions
    /// </summary>
    /// <param name="function">The function to be called</param>
    /// <param name="index">The index to insert the function at</param>
    public void SubscribeRenderAt(Action function, int index) {

        renderFunctions.Insert(index, function);
    }


    /// <summary>
    /// Add a shader to the pipeline
    /// </summary>
    /// <param name="shader">The shader to be added</param>
    public void AddShader(Shader shader) {

        // Attach the shader to the program
        GL.AttachShader(programID, shader.ShaderID);

        // Add the shader to the list of shaders
        Pipeline.Add(shader);
    }


    /// <summary>
    /// Add a shader to the pipeline at a specific place
    /// </summary>
    /// <param name="shader">The shader to be added</param>
    /// <param name="index">The index to insert the shader at</param>
    public void AddShaderAt(Shader shader, int index) {

        // Attach the shader to the program
        GL.AttachShader(programID, shader.ShaderID);

        // Add the shader to the list of shaders
        Pipeline.Insert(index, shader);
    }
}