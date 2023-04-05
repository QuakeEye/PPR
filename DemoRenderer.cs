using OpenTK;
using PPR;


namespace Demo;

/// <summary>
/// This class shows how a renderer could be implemented
///  using the template
/// It shows how you could randomly fill pixels on the screen with a colour
/// </summary>
public class DemoRenderer : Renderer {

    Random random = new Random();


    public DemoRenderer() : base() { }

    public override void Init() { }

    public override void Update() { 

        int x = random.Next(0, window.screen.Width);
        int y = random.Next(0, window.screen.Height);

        window.screen.Plot(x, y, 0xFF0000);
    }
}