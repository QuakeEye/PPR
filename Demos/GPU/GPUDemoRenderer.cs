using OpenTK;
using OpenTK.Mathematics;
using PPR;
using PPR.GPU;

namespace PPR.Demos.GPU;

/// <summary>
/// This class shows how a renderer could be implemented
///  using the template
/// It shows how you could randomly fill pixels on the screen with a colour
/// </summary>
public class GPUDemoRenderer : Renderer {

    public GPUDemoRenderer() : base() { }

    public override void Init() { 

        // Create a new shader
        Shader shader = new("Demos/GPU/Demo.comp");

        // Add the shader to the pipeline, Window will handle the rest
        Window.AddShader(shader);
    }

    public override void Update() {

        
    }
}