using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class WorldSpaceUIFeature : ScriptableRendererFeature
{
    
    [SerializeField] private RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
    
    class WorldSpaceUIPass : ScriptableRenderPass
    {
        
        private RenderTargetIdentifier source;
        private FilteringSettings filteringSettings;
        private ShaderTagId shaderTagId;

        public WorldSpaceUIPass()
        {
            // Filtrer les objets à rendre : utiliser un layer dédié
            filteringSettings = new FilteringSettings(RenderQueueRange.transparent, LayerMask.GetMask("3DUi"));
            shaderTagId = new ShaderTagId("UniversalForward");
        }

        public void Setup(RenderTargetIdentifier source)
        {
            this.source = source;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var cmd = CommandBufferPool.Get("WorldSpaceUI");

            var drawSettings = CreateDrawingSettings(shaderTagId, ref renderingData, SortingCriteria.CommonTransparent);
            drawSettings.perObjectData = PerObjectData.None;

            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();

            context.DrawRenderers(renderingData.cullResults, ref drawSettings, ref filteringSettings);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }

    WorldSpaceUIPass pass;

    public override void Create()
    {
        pass = new WorldSpaceUIPass();
        pass.renderPassEvent = renderPassEvent; // Après le rendu principal
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        pass.Setup(renderer.cameraColorTarget);
        renderer.EnqueuePass(pass);
    }
}