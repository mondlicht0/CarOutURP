using System.Collections.Generic;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class MultiPassRenderFeature : ScriptableRendererFeature
{
    public List<string> LightModePasses;
    private MultiPassPass _mainPass;
    
    public override void Create()
    {
        _mainPass = new MultiPassPass(LightModePasses);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(_mainPass);
    }
}
