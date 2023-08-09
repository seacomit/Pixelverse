using Assets.Core.Voxels.Common;

namespace Assets.Core.Voxels.Compute.Math.Ops
{
    public abstract class UnaryOp : Expression
    {
        protected Expression a;

        protected UnaryOp(Expression a)
        {
            this.a = a;
        }

        public VoxelBuffer evaluate()
        {
            VoxelBuffer voxelsA = a.evaluate();

            op(voxelsA);

            return voxelsA;
        }

        public abstract void op(VoxelBuffer a);
    }
}