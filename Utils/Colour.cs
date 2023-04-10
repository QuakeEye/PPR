using OpenTK.Mathematics;

namespace PPR.Utils;


/// <summary>
/// Class that has utility colour functions
/// </summary>
public static class Colour {

    /// <summary>
    /// Function that takes a vector3 and converts it to an integer
    /// </summary>
    /// <param name="colour">The colour to convert</param>
    /// <returns>The integer representation of the colour</returns>
    public static int MixColour(Vector3 colour) {

        // Convert the colour to an integer
        int r = (int)(colour.X * 255);
        int g = (int)(colour.Y * 255);
        int b = (int)(colour.Z * 255);

        // Return the integer
        return (r << 16) + (g << 8) + b;
    }
}