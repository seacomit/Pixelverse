using Assets.Core.Voxels.Common;
using Assets.Core.Voxels.Compute.Shaders;
using UnityEngine;

namespace Assets.Core.Voxels.Compute.Math.Ops
{
    public class Subtract : BinaryOp
    {
        private Vector3 offset;

        public Subtract(Expression a, Expression b) : base(a, b)
        {
            offset = Vector3.zero;
        }

        public Subtract(Expression a, Expression b, Vector3 offset) : base(a, b)
        {
            this.offset = offset;
        }

        public override void op(VoxelBuffer a, VoxelBuffer b)
        {
            VoxelCompute.math.subtract(a, b, offset);
        }
    }
}