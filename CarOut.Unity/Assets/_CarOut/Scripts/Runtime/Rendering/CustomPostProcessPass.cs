using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class CustomPostProcessPass : ScriptableRenderPass
{
    private Material _bloomMaterial;
    private Material _compositeMaterial;

    private RenderTextureDescriptor _descriptor;

    private RTHandle _cameraColorTarget;
    private RTHandle _cameraDepthTarget;

    private const int MaxPyramideSize = 16;
    private int[] _bloomMipUp;
    private int[] _bloomMipDown;
    private RTHandle[] _rtBloomMipUp;
    private RTHandle[] _rtBloomMipDown;
    private GraphicsFormat _hdrFormat;
    private BenDayBloomEffectComponent _bloomEffect;
    
    public CustomPostProcessPass(Material bloomMaterial, Material compositeMaterial)
    {
        _bloomMaterial = bloomMaterial;
        _compositeMaterial = compositeMaterial;
        renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        _descriptor = new RenderTextureDescriptor();

        _bloomMipUp = new int[MaxPyramideSize];
        _bloomMipDown = new int[MaxPyramideSize];
        _rtBloomMipUp = new RTHandle[MaxPyramideSize];
        _rtBloomMipDown = new RTHandle[MaxPyramideSize];

        for (int i = 0; i < MaxPyramideSize; i++)
        {
            _bloomMipUp[i] = Shader.PropertyToID("_BloomMipUp" + i);
            _bloomMipDown[i] = Shader.PropertyToID("_BloomMipDown" + i);
            _rtBloomMipUp[i] = RTHandles.Alloc(_bloomMipUp[i], name: "_BloomMipUp" + i);
            _rtBloomMipDown[i] = RTHandles.Alloc(_bloomMipUp[i], name: "_BloomMipDown" + i);
        }

        const FormatUsage usage = FormatUsage.Linear | FormatUsage.Render;
        if (SystemInfo.IsFormatSupported(GraphicsFormat.B10G11R11_UFloatPack32, usage))
        {
            _hdrFormat = GraphicsFormat.B10G11R11_UFloatPack32;
        }

        else
        {
            _hdrFormat = QualitySettings.activeColorSpace == ColorSpace.Linear
                ? GraphicsFormat.R8G8B8_SRGB
                : GraphicsFormat.R8G8B8_UNorm;
        }
    }

    private void SetupBloom(CommandBuffer cmd, RTHandle source)
    {
        int downres = 1;
        int tw = _descriptor.width >> downres;
        int th = _descriptor.height >> downres;

        int maxSize = Mathf.Max(tw, th);
        int iterations = Mathf.FloorToInt(Mathf.Log(maxSize, 2f) - 1);
        int mipCount = Mathf.Clamp(iterations, 1, _bloomEffect.MaxIterations.value);

        float clamp = _bloomEffect.Clamp.value;
        float threshold = Mathf.GammaToLinearSpace(_bloomEffect.Threshold.value);
        float thresholdKnee = threshold * 0.5f;

        float scatter = Mathf.Lerp(0.05f, 0.95f, _bloomEffect.Scatter.value);
        var bloomMaterial = _bloomMaterial;
        
        bloomMaterial.SetVector("_Params", new Vector4(scatter, clamp, threshold, thresholdKnee));

        var desc = GetCompatibleDescriptor(tw, th, _hdrFormat);
        for (int i = 0; i < mipCount; i++)
        {
            RenderingUtils.ReAllocateIfNeeded(ref _rtBloomMipUp[i], desc, FilterMode.Bilinear, TextureWrapMode.Clamp, name: _rtBloomMipUp[i].name);
            RenderingUtils.ReAllocateIfNeeded(ref _rtBloomMipDown[i], desc, FilterMode.Bilinear, TextureWrapMode.Clamp, name: _rtBloomMipUp[i].name);
            desc.width = Mathf.Max(1, desc.width >> 1);
            desc.height = Mathf.Max(1, desc.height >> 1);
        }
        
        Blitter.BlitCameraTexture(cmd, source, _rtBloomMipDown[0], RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, _bloomMaterial, 0);

        var lastDown = _rtBloomMipDown[0];
        for (int i = 0; i < mipCount; i++)
        {
            Blitter.BlitCameraTexture(cmd, lastDown, _rtBloomMipUp[i], RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, _bloomMaterial, 1);
            Blitter.BlitCameraTexture(cmd, _rtBloomMipUp[i], _rtBloomMipDown[i], RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, _bloomMaterial, 2);
            lastDown = _rtBloomMipDown[i];
        }
        
        for (int i = mipCount - 2; i >= 0; i--)
        {
            var lowMip = (i == mipCount - 2) ? _rtBloomMipDown[i + 1] : _rtBloomMipUp[i + 1];
            var highMip = _rtBloomMipDown[i];
            var dst = _rtBloomMipUp[i];
            
            cmd.SetGlobalTexture("_SourceTexLowMip", lowMip);
            Blitter.BlitCameraTexture(cmd, highMip, dst, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, _bloomMaterial, 3);
        }
        
        cmd.SetGlobalTexture("_Bloom_Texture", _rtBloomMipUp[0]);
        cmd.SetGlobalFloat("_BloomIntensity", _bloomEffect.Intensity.value);
    }

    private RenderTextureDescriptor GetCompatibleDescriptor() =>
        GetCompatibleDescriptor(_descriptor.width, _descriptor.height, _descriptor.graphicsFormat);

    private RenderTextureDescriptor GetCompatibleDescriptor(int width, int height, GraphicsFormat format,
        DepthBits depthBufferBits = DepthBits.None) =>
        GetCompatibleDescriptor(_descriptor, width, height, format, depthBufferBits);

    internal static RenderTextureDescriptor GetCompatibleDescriptor(RenderTextureDescriptor desc, int width, int height,
        GraphicsFormat format, DepthBits depthBufferBits = DepthBits.None)
    {
        desc.depthBufferBits = (int)depthBufferBits;
        desc.msaaSamples = 1;
        desc.width = width;
        desc.height = height;
        desc.graphicsFormat = format;
        return desc;
    }

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        _descriptor = renderingData.cameraData.cameraTargetDescriptor;
    }

    public void SetTarget(RTHandle cameraColorTargetHandle, RTHandle cameraDepthTargetHandle)
    {
        _cameraColorTarget = cameraColorTargetHandle;
        _cameraDepthTarget = cameraDepthTargetHandle;
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        VolumeStack stack = VolumeManager.instance.stack;
        _bloomEffect = stack.GetComponent<BenDayBloomEffectComponent>();

        CommandBuffer cmd = CommandBufferPool.Get();
        
        using (new ProfilingScope(cmd, new ProfilingSampler("Custom Post Process Effects")))
        {
            SetupBloom(cmd, _cameraColorTarget);
            _compositeMaterial.SetFloat("_Cutoff", _bloomEffect.DotsCutOff.value);
            _compositeMaterial.SetFloat("_Density", _bloomEffect.DotsDenstiy.value);
            _compositeMaterial.SetVector("_Direction", _bloomEffect.ScrollDirection.value);
            
            Blitter.BlitCameraTexture(cmd, _cameraColorTarget, _cameraColorTarget, _compositeMaterial, 0);
        }
        
        context.ExecuteCommandBuffer(cmd);
        cmd.Clear();

        CommandBufferPool.Release(cmd);
    }
}
