using Assets.Core.Voxels.Common;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Assets.Core.Voxels.Compute.Rendering
{
    public class RealisticVoxelRenderer : VoxelRenderer
    {
        private ComputeShader renderComputeShader;
        private ComputeBuffer voxelColorsBuffer;

        public RenderTexture albedoRenderTexture;
        public RenderTexture depthRenderTexture;
        public RenderTexture normalTexture;

        public int size;

        public RealisticVoxelRenderer(ComputeShader renderComputeShader, int size)
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
            normalTexture = new RenderTexture(size, size, colorDepth);
            albedoRenderTexture.enableRandomWrite = true;
            depthRenderTexture.enableRandomWrite = true;
            normalTexture.enableRandomWrite = true;
            albedoRenderTexture.Create();
            depthRenderTexture.Create();
            normalTexture.Create();

            VoxelSet voxelSet = VoxelConfiguration.build();
            voxelColorsBuffer = new ComputeBuffer(voxelSet.colors.Length, Marshal.SizeOf(new Vector4()));
            voxelColorsBuffer.SetData(voxelSet.colors);
        }

        public void render(ComputeBuffer voxelBuffer, SpriteRenderer spriteRenderer)
        {
            // Render the voxel structure and depth out to the render texture targets.
            renderComputeShader.SetBuffer(0, "VoxelColors", voxelColorsBuffer);
            renderComputeShader.SetBuffer(0, "Voxels", voxelBuffer);
            renderComputeShader.SetTexture(0, "Albedo", albedoRenderTexture);
            renderComputeShader.SetTexture(0, "Depth", depthRenderTexture);
            renderComputeShader.Dispatch(0, albedoRenderTexture.width / 8, albedoRenderTexture.height / 8, 1);

            // Generate normal render texture from depth render texture.
            renderComputeShader.SetTexture(1, "Depth", depthRenderTexture);
            renderComputeShader.SetTexture(1, "Normal", normalTexture);
            renderComputeShader.Dispatch(1, normalTexture.width / 8, albedoRenderTexture.height / 8, 1);

            // Transfer RenderTexture to Texture2D Sprite.

            // Pin current active RenderTexture.
            RenderTexture currentActiveRT = RenderTexture.active;

            // Read out Albedo.
            RenderTexture.active = albedoRenderTexture;
            Texture2D texture2D = new Texture2D(size, size);
            // A bigger sprite can be created and the read out to the right sprite position can be done here.
            texture2D.ReadPixels(new Rect(0, 0, size, size), 0, 0);
            texture2D.Apply();

            // Read out Normal.
            RenderTexture.active = normalTexture;
            Texture2D normalMapTex = new Texture2D(size, size);
            normalMapTex.ReadPixels(new Rect(0, 0, size, size), 0, 0);
            normalMapTex.Apply();

            // Restore active RenderTexture.
            RenderTexture.active = currentActiveRT;

            // Create the sprite with a bumpmap.
            spriteRenderer.sprite = Sprite.Create(
                texture2D,
                new Rect(0, 0, size, size),
                new Vector2(0.5f, 0.5f));
            //spriteRenderer.material.EnableKeyword ("_NORMALMAP");
            spriteRenderer.material.SetTexture("_BumpMap", normalMapTex); // Is there a better way?
        }

        public void cleanup()
        {
            voxelColorsBuffer.Release();
        }
    }
}