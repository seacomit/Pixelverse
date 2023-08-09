using UnityEngine;

namespace Assets.Core.Voxels.Common
{
    public class VoxelConfiguration
    {
        private static Vector4 createColor(int r, int g, int b)
        {
            return new Vector4(r / 255.0f, g / 255.0f, b / 255.0f, 1);
        }

        public static VoxelSet build()
        {
            VoxelSet voxelSet = new VoxelSet();

            Voxel[] voxels = new Voxel[10];
            voxels[0].type = (int) VoxelType.Void;
            voxels[1].type = (int) VoxelType.Dirt;
            voxels[2].type = (int) VoxelType.Grass;
            voxels[3].type = (int) VoxelType.Stone;
            voxels[4].type = (int) VoxelType.Water;
            
            Vector4[] colors = new Vector4[10];
            colors[0] = createColor(0, 0, 0); // Transparent
            colors[1] = createColor(86, 49, 26); // Brown
            colors[2] = createColor(50, 205, 50); // Lime Green
            colors[3] = createColor(155, 155, 155); // Gray
            colors[4] = createColor(50, 50, 205); // Blue

            voxelSet.colors = colors;
            voxelSet.voxels = voxels;
            return voxelSet;
        }
    }
}