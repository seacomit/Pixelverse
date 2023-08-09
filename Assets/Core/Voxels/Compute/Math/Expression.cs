using Assets.Core.Voxels.Common;

namespace Assets.Core.Voxels.Compute.Math
{
    public interface Expression
    {
        VoxelBuffer evaluate();
    }
}