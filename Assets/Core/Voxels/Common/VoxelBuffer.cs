using System.Runtime.InteropServices;
using System.Collections.Generic;
using UnityEngine;
using Assets.Core.Voxels.Compute.Shaders;

namespace Assets.Core.Voxels.Common
{
    public class VoxelBuffer : Computable
    {
        // Globally available Voxels cache.
        public static VoxelBufferCache<VoxelBuffer> Cache = new VoxelBufferCache<VoxelBuffer>(new VoxelBufferFactory());

        private ComputeBuffer _buffer;
        private int _size;

        public VoxelBuffer(ComputeBuffer buffer, int size)
        {
            _size = size;
            _buffer = buffer;
        }
        
        public int size()
        {
            return _size;
        }

        public ComputeBuffer buffer()
        {
            return _buffer;
        }

        public int Volume()
        {
            return _size * _size * _size;
        }

        public void Release()
        {
            VoxelCompute.transform.clear(this);
            Cache.ReleaseBuffer(this);
        }
    }
}