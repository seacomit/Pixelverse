using Assets.Core.Voxels.Common;

namespace Assets.Core.Voxels.Compute.Math.Ops
{
    public abstract class BinaryOp : Expression
    {
        protected Expression a;
        protected Expression b;

        protected BinaryOp(Expression a, Expression b)
        {
            this.a = a;
            this.b = b;
        }

        public VoxelBuffer evaluate()
        {
            VoxelBuffer voxelsA = a.evaluate();
            VoxelBuffer voxelsB = b.evaluate();

            op(voxelsA, voxelsB);
            voxelsB.Release();

            return voxelsA;
        }

        public abstract void op(VoxelBuffer a, VoxelBuffer b);
    }
}