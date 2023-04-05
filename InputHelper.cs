using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace PPR;


/// <summary>
/// Input helper class, which keeps track of keyboard and mouse states and
///  various input members
/// </summary>
public static class InputHelper {

    // Save previous and current keyboard states to be able to observe their differences
    public static KeyboardState PreviousKeyboardState;  // Previous keyboard state gets updated after it has been used

    public static KeyboardState CurrentKeyboardState;   // Current keyboard state gets updated before it is used


    // Save previous and current mouse states to be able to observe their differences
    public static MouseState PreviousMouseState;        // Previous mouse state gets updated after it has been used

    public static MouseState CurrentMouseState;         // Current mouse state gets updated before it is used


    // Mouse positions of the current and last frame
    public static Vector2 LastMousePos = Vector2.Zero;
    public static Vector2 CurrentMousePos = Vector2.Zero;

    // The difference in mouse position between the current and last frame
    public static Vector2 MouseDeltaPos
        => CurrentMousePos - LastMousePos;


    /// <summary>
    /// Update the input states, as should be done in each frame
    /// </summary>
    /// <param name="keyboardState">Give the currently retrieved keyboard state</param>
    public static void Update(KeyboardState keyboardState, MouseState mouseState) {

        // Update the keyboard states
        PreviousKeyboardState = CurrentKeyboardState;
        CurrentKeyboardState = keyboardState;

        // Update the mouse states
        PreviousMouseState = CurrentMouseState;
        CurrentMouseState = mouseState;

        // Update the mouse positions
        LastMousePos = CurrentMousePos;
        CurrentMousePos = CurrentMouseState.Position;
    }


    /// <summary>
    /// Retrieve whether a mouse button has been pressed, that means it returns true if the button 
    ///  was not being pressed in the last frame and is being pressed in the current frame
    /// </summary>
    /// <param name="button">The button that needs to be checked</param>
    public static bool ButtonPressed(Keys button) {

        return !PreviousKeyboardState.IsKeyDown(button) && CurrentKeyboardState.IsKeyDown(button);
    }


    /// <summary>
    /// Retrieve whether a keyboard key has been pressed, that means it returns true if the key
    ///  was not being pressed in the last frame and is being pressed in the current frame
    /// OpenTK's KeyboardState already provides support for this, so that is what will be used here,
    ///  this function is here purely for consistency
    /// </summary>
    /// <param name="key">The key that needs to be checked</param>
    public static bool KeyPressed(Keys key) {

        return CurrentKeyboardState.IsKeyPressed(key);
    }
}