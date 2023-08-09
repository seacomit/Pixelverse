using System.Runtime.InteropServices;
using System.Collections.Generic;
using UnityEngine;
using Assets.Core.Voxels.Compute.Shaders;

namespace Assets.Core.Voxels.Common
{
    public class PointBuffer : Computable
    {
        // Globally available Points cache.
        public static VoxelBufferCache<PointBuffer> Cache = new VoxelBufferCache<PointBuffer>(new PointBufferFactory());

        private ComputeBuffer _buffer;
        private int _size;

        public PointBuffer(ComputeBuffer buffer, int size)
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

        public void Release()
        {
            Cache.ReleaseBuffer(this);
        }
    }
}
