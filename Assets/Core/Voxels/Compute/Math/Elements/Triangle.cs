using Assets.Core.Voxels.Common;
using Assets.Core.Voxels.Compute.Shaders;
using UnityEngine;

namespace Assets.Core.Voxels.Compute.Math.Elements
{
    public class Triangle : Expression
    {
        private VoxelTransform transform;
        private Geometry.Triangle triangle;

        public Triangle(Geometry.Triangle triangle, VoxelTransform transform)
        {
            this.triangle = triangle;
            this.transform = transform;
        }

        public VoxelBuffer evaluate()
        {
            VoxelBuffer voxels = VoxelBuffer.Cache.GetOrBuild();
            VoxelCompute.geometry.triangle(
                voxels,
                triangle.scale((int)transform.scale.x),
                transform.position,
                (int)transform.voxelType);

            return transform.rotate(voxels);
        }

    }
}