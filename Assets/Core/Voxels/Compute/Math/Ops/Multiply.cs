using Assets.Core.Voxels.Common;
using Assets.Core.Voxels.Compute.Shaders;
using UnityEngine;

namespace Assets.Core.Voxels.Compute.Math.Ops
{
    public class Multiply : UnaryOp
    {
        private PointBuffer pointBuffer;

        public Multiply(Expression a, PointBuffer pointBuffer) : base(a) 
        {
            this.pointBuffer = pointBuffer;
        }

        public override void op(VoxelBuffer a)
        {
            VoxelCompute.math.multiply(a, pointBuffer);
        }
    }
}
