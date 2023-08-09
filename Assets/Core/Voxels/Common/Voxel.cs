using UnityEngine;

namespace Assets.Core.Voxels.Common
{
    public struct Voxel
    {
        public int type;
    }

    public enum VoxelType
    {
        Void = 0,
        Dirt = 1,
        Grass = 2,
        Stone = 3,
        Water = 4,
    }

    public class VoxelSet
    {
        public Voxel[] voxels;
        public Vector4[] colors;
    }
}