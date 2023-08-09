using System.Collections.Generic;

namespace Assets.Core.Voxels.Common
{
    public class VoxelBufferCache<T> where T : Computable
    {
        public const int defaultVoxelBufferSize = 128;

        // Stores unused voxel buffers keyed by the size of the voxel buffer
        // cube (size of one dimension not volume).
        private Dictionary<int, List<T>> cache;

        // Voxel buffers currently in use.
        private List<T> lockedBuffers;

        private ComputableFactory<T> factory;

        public VoxelBufferCache(ComputableFactory<T> factory)
        {
            cache = new Dictionary<int, List<T>>();
            lockedBuffers = new List<T>();
            this.factory = factory;
        }

        public void ReleaseBuffer(T computable)
        {
            lockedBuffers.Remove(computable);
            List<T> availableBuffers = GetAvailableBuffers(computable.size());
            availableBuffers.Add(computable);
        }

        public T GetOrBuild()
        {
            return this.GetOrBuild(defaultVoxelBufferSize);
        }

        public T GetOrBuild(int size)
        {
            List<T> availableBuffers = GetAvailableBuffers(size);
            if (availableBuffers.Count > 0)
            {
                // Return an existing buffer thats just idling.
                T availableBuffer = availableBuffers[availableBuffers.Count - 1];
                availableBuffers.RemoveAt(availableBuffers.Count - 1);
                lockedBuffers.Add(availableBuffer);
                return availableBuffer;
            }

            // All buffers for this size are currently in use, make a new one.
            T voxelBuffer = factory.build(size);
            
            lockedBuffers.Add(voxelBuffer);
            return voxelBuffer;
        }

        private List<T> GetAvailableBuffers(int size)
        {
            if (cache.ContainsKey(size))
            {
                return cache[size];
            }
            else
            {
                // No list for storing this size of voxel buffer exists, make one.
                List<T> buffers = new List<T>();
                cache.Add(size, buffers);
                return buffers;
            }
        }

        public void Release()
        {
            foreach (T buffer in lockedBuffers)
            {
                buffer.buffer().Release();
            }

            foreach (KeyValuePair<int, List<T>> cacheEntry in cache)
            {
                List<T> cachedBuffers = cacheEntry.Value;
                foreach (T buffer in cachedBuffers)
                {
                    buffer.buffer().Release();
                }
            }
            lockedBuffers.Clear();
            cache.Clear();
        }
    }
}