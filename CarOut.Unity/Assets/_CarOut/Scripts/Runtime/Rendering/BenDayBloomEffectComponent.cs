using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[VolumeComponentMenuForRenderPipeline("Custom/Ben Day Bloom", typeof(UniversalRenderPipeline))]
public class BenDayBloomEffectComponent : VolumeComponent, IPostProcessComponent
{
    [Header("Bloom Settings")] 
    public FloatParameter Threshold = new(0.9f, true);
    public FloatParameter Intensity = new(1, true);
    public ClampedFloatParameter Scatter = new(0.7f, 0, 1, true);
    public IntParameter Clamp = new(65472, true);
    public ClampedIntParameter MaxIterations = new(6, 0, 10);
    public NoInterpColorParameter Tint = new(Color.white);

    [Header("Ben Day")] 
    public IntParameter DotsDenstiy = new(10, true);
    public ClampedFloatParameter DotsCutOff = new(0.4f, 0, 1, true);
    public Vector2Parameter ScrollDirection = new(new Vector2());
    
    public bool IsActive()
    {
        return true;
    }

    public bool IsTileCompatible()
    {
        return false;
    }
}
