using Assets.Core.Voxels.Common;
using Assets.Core.Voxels.Compute.Shaders;

namespace Assets.Core.Voxels.Compute.Math.Elements
{
    public class Cube : Expression
    {
        private VoxelTransform transform;

        public Cube(VoxelTransform transform)
        {
            this.transform = transform;
        }

        public VoxelBuffer evaluate()
        {
            VoxelBuffer voxels = VoxelBuffer.Cache.GetOrBuild();
            VoxelCompute.geometry.cube(
                voxels, 
                (int)transform.scale.x, 
                transform.position, 
                (int)transform.voxelType);

            return transform.rotate(voxels);
        }
    }
}