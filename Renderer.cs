using OpenTK.Mathematics;

namespace PPR;

/// <summary>
/// This class will be the entry point and main loop of the program
/// You can either edit this class or create a new one and let it
///  inherit from this class
/// </summary>
public class Renderer {

    // Reference to the template window
    protected Window Window;


    public Renderer() {

        // Create a new window
        Window = new Window(new Vector2i(Settings.Width, Settings.Height));
        
        // Subscribe to the window's events
        Window.SubscribeInit(Init);
        Window.SubscribeUpdate(Update);
        Window.SubscribeRender(Render);

        // Start the window
        Window.Run();
    }


    public virtual void Init() { }

    public virtual void Update() { }

    public virtual void Render() { }
}