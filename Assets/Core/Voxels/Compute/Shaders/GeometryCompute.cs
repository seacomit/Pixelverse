using Assets.Core.Geometry;
using Assets.Core.Voxels.Common;
using UnityEngine;

namespace Assets.Core.Voxels.Compute.Shaders
{
    public class GeometryCompute : BaseCompute
    {
        private enum GeometryFunctions
        {
            // Must be in the exact same order as the compute shader kernel definitions.
            Tetrahedron = 0,
            Cube = 1,
            Sphere = 2,
            Triangle = 3,
        }

        private ComputeShader geometryComputeShader;

        public GeometryCompute(ComputeShader geometryComputeShader)
        {
            this.geometryComputeShader = geometryComputeShader;
        }

        public void tetrahedron(VoxelBuffer voxelBuffer, Tetrahedron tetra, Vector3 offset, int voxelType)
        {
            setVoxelBuffer(GeometryFunctions.Tetrahedron, voxelBuffer);
            geometryComputeShader.SetVector("v1", tetra.v1 + offset);
            geometryComputeShader.SetVector("v2", tetra.v2 + offset);
            geometryComputeShader.SetVector("v3", tetra.v3 + offset);
            geometryComputeShader.SetVector("v4", tetra.v4 + offset);
            geometryComputeShader.SetInt("type", voxelType);
            dispatch(GeometryFunctions.Tetrahedron, voxelBuffer);
        }

        public void cube(VoxelBuffer voxelBuffer, int size, Vector3 offset, int voxelType)
        {
            setVoxelBuffer(GeometryFunctions.Cube, voxelBuffer);
            geometryComputeShader.SetVector("offset", offset);
            geometryComputeShader.SetInt("cubeSize", size);
            geometryComputeShader.SetInt("type", voxelType);
            dispatch(GeometryFunctions.Cube, voxelBuffer);
        }

        public void sphere(VoxelBuffer voxelBuffer, int radius, Vector3 offset, int voxelType)
        {
            setVoxelBuffer(GeometryFunctions.Sphere, voxelBuffer);
            geometryComputeShader.SetVector("offset", offset);
            geometryComputeShader.SetFloat("radius", radius);
            geometryComputeShader.SetInt("type", voxelType);
            dispatch(GeometryFunctions.Sphere, voxelBuffer);
        }

        public void triangle(VoxelBuffer voxelBuffer, Triangle triangle, Vector3 offset, int voxelType)
        {
            setVoxelBuffer(GeometryFunctions.Triangle, voxelBuffer);
            geometryComputeShader.SetVector("v1", triangle.v1 + offset);
            geometryComputeShader.SetVector("v2", triangle.v2 + offset);
            geometryComputeShader.SetVector("v3", triangle.v3 + offset);
            geometryComputeShader.SetInt("type", voxelType);
            dispatch(GeometryFunctions.Triangle, voxelBuffer);
        }

        private void setVoxelBuffer(GeometryFunctions fn, VoxelBuffer voxelBuffer)
        {
            geometryComputeShader.SetBuffer((int)fn, "Voxels", voxelBuffer.buffer());
        }

        private void dispatch(GeometryFunctions fn, VoxelBuffer voxelBuffer)
        {
            int threadSize = getThreadSize(voxelBuffer.size());
            geometryComputeShader.SetInt("size", voxelBuffer.size());
            geometryComputeShader.Dispatch((int)fn, threadSize, threadSize, threadSize);
        }

        public void cleanup()
        {
            // Cleanup.
        }
    }
}