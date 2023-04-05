using OpenTK.Graphics.OpenGL;

namespace PPR;


/// <summary>
/// Render target class that is used to 'draw' our picture onto and to be drawn using openGL
/// It is basically just an array of pixels holding their own color
/// </summary>
public class Target {

    // An integer array representing the pixels of the target
    public int[] Pixels { get; set; }

    // The width and height of our render target,
    //  they can only be set inside this class' constructor
    public int Width { get; private set; }
    public int Height { get; private set; }


    /// <summary>
    /// Constructor for the render target class
    /// It creates the pixels array and sets the correct width and height
    /// </summary>
    /// <param name="width">The width of the render target</param>
    /// <param name="height">The height of the render target</param>
    public Target(int width, int height) {

        // Set the width and height of the render target
        Width = width;
        Height = height;

        // Create the pixels array, using these width and height as defined above
        Pixels = new int[width * height];
    }


    /// <summary>
    /// Clear the entire render target
    /// Set all pixels to the given colour, 
    ///  having these colour defaulting to black, or 0
    /// </summary>
    public void Clear(int c = 0) {

        for (int i = 0; i < Pixels.Length; i++)
            Pixels[i] = c;
    }


    /// <summary>
    /// Plot a pixel onto the render target
    /// Taking in an optional offset, which could be used to 
    ///  iterate through the target more easily at different 
    ///  rendering resolutions
    /// </summary>
    /// <param name="x">The x coordinate of the pixel</param>
    /// <param name="y">The y coordinate of the pixel</param>
    /// <param name="c">The colour that will be plot to the pixel</param>
    /// <param name="offset">The optional offset that will be added to the x and y coordinates</param>
    public void Plot(int x, int y, int c, int offset = 0) {

        if ((x >= 0) && (y >= 0) && (x < Width) && (y < Height))
                Pixels[x + y * Width + offset] = c;
    }


    /// <summary>
    /// Retrieve the colour of a pixel at the given coordinates
    /// Taking into account that the given coordinates could be outside of the render target
    /// </summary>
    /// <param name="x">The x coordinate of the pixel</param>
    /// <param name="y">The y coordinate of the pixel</param>
    /// <returns>The colour of the pixel at the given coordinates</returns>
    public int GetPixel(int x, int y) {

        if ((x >= 0) && (y >= 0) && (x < Width) && (y < Height))
            return Pixels[x + y * Width];
        else
            return -1;  // Return -1 to let the calling function know that
                        //  the given coordinates are outside of the render target
    }


    /// <summary>
    /// Print some pixel's value to the console
    /// </summary>
    /// <param name="x">The x coordinate of the pixel</param>
    /// <param name="y">The y coordinate of the pixel</param>
    public void Print(int x, int y) {

        // Take into account that the given coordinates could be outside of the render target
        if ((x >= 0) && (y >= 0) && (x < Width) && (y < Height))
                Console.WriteLine(Pixels[x + y * Width]);   // Print the pixel's value to the console
    }


    /// <summary>
    /// Create an OpenGL texture id from the render target
    /// </summary>
    /// <returns>The OpenGL texture id</returns>
    public int GenTexture() {

        // Generate a texture id
        int id = GL.GenTexture();

        // Bind and set the target to opengl
        GL.BindTexture(TextureTarget.Texture2D, id);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, Width, Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, Pixels);

        // After doing the OpenGL side effects, also return the texture id
        return id;
    }
}