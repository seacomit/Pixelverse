using Assets.Core.Voxels.Common;

namespace Assets.Core.Voxels.Compute.Math.Ops
{
    public class Rotate : UnaryOp
    {
        private VoxelTransform transform;

        public Rotate(VoxelTransform transform, Expression a) : base(a)
        {
            this.transform = transform;
        }

        public override void op(VoxelBuffer a)
        {
            transform.rotate(a);
        }
    }
}