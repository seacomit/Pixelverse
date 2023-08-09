using Assets.Core.Voxels.Common;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Assets.Core.Voxels.Compute.Shaders
{
    public class TransformCompute : BaseCompute
    {
        private enum TransformFunctions
        {
            Clear = 0,
            Copy = 1,
            Rotate = 2,
        }

        public ComputeShader transformComputeShader;

        // Buffer used for the output of some transformations, necessary when voxel buffer source
        // must remain unchanged during a transform, like when the context of the surrounding voxels matters.
        public VoxelBuffer voxelBufferOutput;

        public TransformCompute(ComputeShader transformComputeShader)
        {
            this.transformComputeShader = transformComputeShader;
        }

        public void clear(VoxelBuffer voxelBuffer)
        {
            setVoxelBuffer(TransformFunctions.Clear, voxelBuffer);
            dispatch(TransformFunctions.Clear, voxelBuffer);
        }

        public void copy(VoxelBuffer source, VoxelBuffer destination)
        {
            setVoxelBuffer(TransformFunctions.Copy, destination);
            transformComputeShader.SetBuffer((int)TransformFunctions.Copy, "VoxelsTemp", source.buffer());
            dispatch(TransformFunctions.Copy, destination);
        }

        // Rotate the voxels in the buffer about the given origin, the origin should be the exact center of rotation,
        // this means that it doesnt necessarily need to be a voxel position integer but can be halfway between to voxel points.
        public void rotate(VoxelBuffer voxelBuffer, Vector4 origin, Quaternion rotation)
        {
            initOutputBuffer(voxelBuffer);

            Vector4 rotationVector = new Vector4(rotation.x, rotation.y, rotation.z, rotation.w);
            Quaternion invRot = Quaternion.Inverse(rotation);
            Vector4 inverseRotationVector = new Vector4(invRot.x, invRot.y, invRot.z, invRot.w);

            setVoxelBuffer(TransformFunctions.Rotate, voxelBuffer);
            transformComputeShader.SetBuffer((int)TransformFunctions.Rotate, "VoxelsTemp", voxelBufferOutput.buffer());
            transformComputeShader.SetVector("origin", origin);
            transformComputeShader.SetVector("rotation", rotationVector);
            transformComputeShader.SetVector("inverseRotation", inverseRotationVector);
            dispatch(TransformFunctions.Rotate, voxelBuffer);
            copy(voxelBufferOutput, voxelBuffer);
            clear(voxelBufferOutput);
        }

        private void setVoxelBuffer(TransformFunctions fn, VoxelBuffer voxelBuffer)
        {
            transformComputeShader.SetInt("size", voxelBuffer.size());
            transformComputeShader.SetBuffer((int)fn, "Voxels", voxelBuffer.buffer());
        }

        private void dispatch(TransformFunctions fn, VoxelBuffer voxelBuffer)
        {
            int threadSize = getThreadSize(voxelBuffer.size());
            transformComputeShader.Dispatch((int)fn, threadSize, threadSize, threadSize);
        }

        private void initOutputBuffer(VoxelBuffer voxelBuffer)
        {
            if (voxelBufferOutput == null)
            {
                voxelBufferOutput = VoxelBuffer.Cache.GetOrBuild(voxelBuffer.size());
            }
            else if (voxelBufferOutput.size() != voxelBuffer.size())
            {
                voxelBufferOutput.Release();
                voxelBufferOutput = VoxelBuffer.Cache.GetOrBuild(voxelBuffer.size());
            }
        }

        public void cleanup()
        {
            // Cleanup.
            if (voxelBufferOutput != null)
            {
                voxelBufferOutput.Release();
            }
        }
    }
}