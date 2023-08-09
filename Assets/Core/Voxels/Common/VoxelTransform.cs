using Assets.Core.Voxels.Compute.Shaders;
using UnityEngine;

namespace Assets.Core.Voxels.Common
{
    public class VoxelTransform
    {
        public static readonly VoxelTransform zero = new VoxelTransform();
        public Vector3 position = Vector3.zero;
        public Vector3 scale = Vector3.one;
        public Quaternion rotation = Quaternion.identity;
        public Vector3 origin = Vector3.zero;
        public int voxelType = 0;

        public VoxelTransform() { }

        public VoxelTransform(int voxelType, Vector3 position)
        {
            this.voxelType = voxelType;
            this.position = position;
        }

        public VoxelTransform(int voxelType, Vector3 position, Vector3 scale)
        {
            this.voxelType = voxelType;
            this.position = position;
            this.scale = scale;
        }

        public VoxelTransform(int voxelType, Vector3 position, Vector3 scale, Quaternion rotation)
        {
            this.voxelType = voxelType;
            this.position = position;
            this.scale = scale;
            this.rotation = rotation;
        }

        public VoxelTransform(int voxelType, Vector3 position, Vector3 scale, Quaternion rotation, Vector3 origin)
        {
            this.voxelType = voxelType;
            this.position = position;
            this.scale = scale;
            this.rotation = rotation;
            this.origin = origin;
        }

        public VoxelBuffer rotate(VoxelBuffer a)
        {
            if (rotation == Quaternion.identity) return a;

            int voxelSize = a.size();
            Vector3 originForRotation = new Vector3(voxelSize / 2.0f, voxelSize / 2.0f, voxelSize / 2.0f) + origin;

            // The rotation has to be inverted since we operate from the destination point in computer shaders.
            // The quaternion should also be normalized, we just want rotation only.
            Quaternion invRotation = Quaternion.Inverse(rotation.normalized);
            VoxelCompute.transform.rotate(a, origin, invRotation);
            return a;
        }
    }
}
