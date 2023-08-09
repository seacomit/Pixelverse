using Assets.Core.Voxels.Common;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Assets.Core.Voxels.Compute.Rendering
{
    public class StyledVoxelRenderer : VoxelRenderer
    {
        private enum RenderFunctions
        {
            // Must be in the exact same order as the compute shader kernel definitions.
            RayTraceRender = 0,
            GenerateNormal = 1,
            LightVar1 = 2,
        }

        private ComputeShader renderComputeShader;
        private ComputeBuffer voxelColorsBuffer;

        public RenderTexture albedoRenderTexture;
        public RenderTexture depthRenderTexture;

        public int size;

        public StyledVoxelRenderer(ComputeShader renderComputeShader, int size)
        {
            this.renderComputeShader = renderComputeShader;
            this.size = size;
        }

        public void initialize()
        {
            renderComputeShader.SetInt("voxelBufferSideLength", size);
            int colorDepth = 32;
            albedoRenderTexture = new RenderTexture(size, size, colorDepth);
            depthRenderTexture = new RenderTexture(size, size, colorDepth);
            albedoRenderTexture.enableRandomWrite = true;
            depthRenderTexture.enableRandomWrite = true;
            albedoRenderTexture.Create();
            depthRenderTexture.Create();

            VoxelSet voxelSet = VoxelConfiguration.build();
            voxelColorsBuffer = new ComputeBuffer(voxelSet.colors.Length, Marshal.SizeOf(new Vector4()));
            voxelColorsBuffer.SetData(voxelSet.colors);
        }

        public void render(ComputeBuffer voxelBuffer, SpriteRenderer spriteRenderer)
        {
            // Render the voxel structure and depth out to the render texture targets.
            renderComputeShader.SetBuffer((int)RenderFunctions.RayTraceRender, "VoxelColors", voxelColorsBuffer);
            renderComputeShader.SetBuffer((int)RenderFunctions.RayTraceRender, "Voxels", voxelBuffer);
            renderComputeShader.SetTexture((int)RenderFunctions.RayTraceRender, "Albedo", albedoRenderTexture);
            renderComputeShader.SetTexture((int)RenderFunctions.RayTraceRender, "Depth", depthRenderTexture);
            renderComputeShader.Dispatch(
                (int)RenderFunctions.RayTraceRender, albedoRenderTexture.width / 8, albedoRenderTexture.height / 8, 1);

            renderComputeShader.SetTexture((int)RenderFunctions.LightVar1, "Albedo", albedoRenderTexture);
            renderComputeShader.SetTexture((int)RenderFunctions.LightVar1, "Depth", depthRenderTexture);
            renderComputeShader.Dispatch(
                (int)RenderFunctions.LightVar1, albedoRenderTexture.width / 8, albedoRenderTexture.height / 8, 1);

            // Transfer RenderTexture to Texture2D Sprite.

            // Pin current active RenderTexture.
            RenderTexture currentActiveRT = RenderTexture.active;

            // Read out Albedo.
            RenderTexture.active = albedoRenderTexture;
            Texture2D texture2D = new Texture2D(size, size);
            // A bigger sprite can be created and then read out to the right sprite position here.
            texture2D.ReadPixels(new Rect(0, 0, size, size), 0, 0);
            texture2D.Apply();

            // Restore active RenderTexture.
            RenderTexture.active = currentActiveRT;

            // Create the sprite.
            spriteRenderer.sprite = Sprite.Create(
                texture2D,
                new Rect(0, 0, size, size),
                new Vector2(0.5f, 0.5f));
        }

        public void cleanup()
        {
            voxelColorsBuffer.Release();
        }
    }
}