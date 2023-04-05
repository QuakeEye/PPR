using OpenTK.Mathematics;

namespace PPR;

/// <summary>
/// This class will be the entry point and main loop of the program
/// You can either edit this class or create a new one and let it
///  inherit from this class
/// </summary>
public class Renderer {

    // Reference to the template window
    protected Window window;


    public Renderer() {

        // Create a new window
        window = new Window(new Vector2i(Settings.Width, Settings.Height));
        
        // Subscribe to the window's events
        window.SubscribeInit(Init);
        window.SubscribeUpdate(Update);
        window.SubscribeRender(Render);

        // Start the window
        window.Run();
    }


    public virtual void Init() { }

    public virtual void Update() { }

    public virtual void Render() { }
}