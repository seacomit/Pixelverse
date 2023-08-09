using UnityEngine;

namespace Assets.Core.Voxels.Compute.Shaders
{
    public class VoxelCompute
    {
        public static TransformCompute transform;
        public static GeometryCompute geometry;
        public static MathCompute math;

        public VoxelCompute(ComputeShader geometryComputeShader,
                          ComputeShader transformComputeShader,
                          ComputeShader mathComputeShader)
        {
            geometry = new GeometryCompute(geometryComputeShader);
            transform = new TransformCompute(transformComputeShader);
            math = new MathCompute(mathComputeShader);
        }

        public void cleanup()
        {
            transform.cleanup();
            geometry.cleanup();
            math.cleanup();

        }
    }
}