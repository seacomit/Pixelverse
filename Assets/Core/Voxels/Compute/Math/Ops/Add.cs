using Assets.Core.Voxels.Common;
using Assets.Core.Voxels.Compute.Shaders;
using UnityEngine;

namespace Assets.Core.Voxels.Compute.Math.Ops
{
    public class Add : BinaryOp
    {
        private Vector3 offset;

        public Add(Expression a, Expression b) : base(a, b) 
        {
            offset = Vector3.zero;
        }

        public Add(Expression a, Expression b, Vector3 offset) : base(a, b)
        {
            this.offset = offset;
        }

        public override void op(VoxelBuffer a, VoxelBuffer b)
        {
            VoxelCompute.math.add(a, b, offset);
        }
    }
}