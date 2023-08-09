using Assets.Core.Voxels.Common;
using UnityEngine;

namespace Assets.Core.Voxels.Compute.Shaders
{
    public class MathCompute : BaseCompute
    {
        private enum MathFunctions
        {
            // Must be in the exact same order as the compute shader kernel definitions.
            Add = 0,
            Subtract = 1,
            Intersect = 2,
            Fill = 3,
            Multiply = 4,
        }

        private ComputeShader mathComputeShader;

        public MathCompute(ComputeShader mathComputeShader)
        {
            this.mathComputeShader = mathComputeShader;
        }

        // Sets voxels from B into A voxels with the given offset, replacing voxels in A if individual voxel
        // positions overlap.
        public void add(VoxelBuffer voxelsA, VoxelBuffer voxelsB, Vector3 offset)
        {
            dispatch(MathFunctions.Add, voxelsA, voxelsB, offset);
        }

        // Removes voxels from A using the structure of the voxels from B as a reference. Ignores actual Voxel type.
        public void subtract(VoxelBuffer voxelsA, VoxelBuffer voxelsB, Vector3 offset)
        {
            dispatch(MathFunctions.Subtract, voxelsA, voxelsB, offset);
        }

        // Removes all voxels from A to which there is no overlapping Voxel from B, for the remaining matching positions
        // B's voxels are used replacing the voxels in A.
        public void intersect(VoxelBuffer voxelsA, VoxelBuffer voxelsB, Vector3 offset)
        {
            dispatch(MathFunctions.Intersect, voxelsA, voxelsB, offset);
        }

        // Adds voxels from B into A voxels with the given offset, does not replace any existing voxels
        // in A. If there is voxel overlap it is simply ignored.
        public void fill(VoxelBuffer voxelsA, VoxelBuffer voxelsB, Vector3 offset)
        {
            dispatch(MathFunctions.Fill, voxelsA, voxelsB, offset);
        }

        // Sets voxels from B into A voxels with the given offset, replacing voxels in A if individual voxel
        // positions overlap.
        public void multiply(VoxelBuffer voxelsA, PointBuffer points)
        {
            VoxelBuffer temp = VoxelBuffer.Cache.GetOrBuild(voxelsA.size());
            mathComputeShader.SetBuffer((int)MathFunctions.Multiply, "Points", points.buffer());
            mathComputeShader.SetBuffer((int)MathFunctions.Multiply, "VoxelsTemp", temp.buffer());
            mathComputeShader.SetInt("pointsSize", points.size());
            dispatch(MathFunctions.Multiply, voxelsA);
            VoxelCompute.transform.copy(temp, voxelsA);
            temp.Release();
        }

        private void setVoxelBuffers(MathFunctions fn, VoxelBuffer voxelsA, VoxelBuffer voxelsB)
        {
            mathComputeShader.SetBuffer((int)fn, "VoxelsA", voxelsA.buffer());
            mathComputeShader.SetBuffer((int)fn, "VoxelsB", voxelsB.buffer());
        }

        private void dispatch(MathFunctions fn, VoxelBuffer voxelsA)
        {
            int threadSize = getThreadSize(voxelsA.size());
            mathComputeShader.SetBuffer((int)fn, "VoxelsA", voxelsA.buffer());
            mathComputeShader.SetInt("sizeA", voxelsA.size());
            mathComputeShader.Dispatch((int)fn, threadSize, threadSize, threadSize);
        }

        private void dispatch(MathFunctions fn, VoxelBuffer voxelsA, VoxelBuffer voxelsB, Vector3 offset)
        {
            int threadSize = getThreadSize(voxelsA.size());
            setVoxelBuffers(fn, voxelsA, voxelsB);
            mathComputeShader.SetVector("offset", offset);
            mathComputeShader.SetInt("sizeA", voxelsA.size());
            mathComputeShader.SetInt("sizeB", voxelsB.size());
            mathComputeShader.Dispatch((int)fn, threadSize, threadSize, threadSize);
        }

        public void cleanup()
        {
            // Cleanup.
        }
    }
}
