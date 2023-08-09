using Assets.Core.Voxels.Common;

namespace Assets.Core.Voxels.Compute.Math.Elements
{
    public class Identity : Expression
    {
        public Identity() {}

        public VoxelBuffer evaluate()
        {
            return VoxelBuffer.Cache.GetOrBuild();
        }
    }
}