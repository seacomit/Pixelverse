using System.Runtime.InteropServices;
using UnityEngine;

namespace Assets.Core.Voxels.Common
{
    public interface ComputableFactory<T>
    {
        T build(int size);
    }

    public class VoxelBufferFactory : ComputableFactory<VoxelBuffer>
    {
        public VoxelBuffer build(int size)
        {
            int volume = size * size * size;
            Voxel[] elements = new Voxel[volume];
            ComputeBuffer buffer = new ComputeBuffer(volume, Marshal.SizeOf(new Voxel()));
            buffer.SetData(elements);
            return new VoxelBuffer(buffer, size);
        }
    }

    public class PointBufferFactory : ComputableFactory<PointBuffer>
    {
        public PointBuffer build(int size)
        {
            Vector4[] elements = new Vector4[size];
            for (int i = 0; i < size; i++)
            {
                elements[i] = new Vector4(0, i - (size / 2), i - (size / 2));
            }

            ComputeBuffer buffer = new ComputeBuffer(size, Marshal.SizeOf(new Vector4()));
            buffer.SetData(elements);
            return new PointBuffer(buffer, size);
        }
    }
}