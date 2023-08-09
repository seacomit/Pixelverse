using Assets.Core.Voxels.Common;
using Assets.Core.Voxels.Compute.Shaders;
using UnityEngine;

namespace Assets.Core.Voxels.Compute.Math.Ops
{
    public class Fill : BinaryOp
    {
        private Vector3 offset;

        public Fill(Expression a, Expression b) : base(a, b)
        {
            offset = Vector3.zero;
        }

        public Fill(Expression a, Expression b, Vector3 offset) : base(a, b)
        {
            this.offset = offset;
        }

        public override void op(VoxelBuffer a, VoxelBuffer b)
        {
            VoxelCompute.math.fill(a, b, offset);
        }
    }
}