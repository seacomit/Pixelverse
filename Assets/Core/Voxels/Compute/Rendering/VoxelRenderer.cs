using UnityEngine;

namespace Assets.Core.Voxels.Compute.Rendering
{
    public interface VoxelRenderer
    {
        void render(ComputeBuffer voxelBuffer, SpriteRenderer spriteRenderer);
        void initialize();
        void cleanup();
    }
}