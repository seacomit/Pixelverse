using Assets.Core.Voxels.Common;
using Assets.Core.Voxels.Compute.Shaders;

namespace Assets.Core.Voxels.Compute.Math.Elements
{
    public class Tetrahedron : Expression
    {
        private VoxelTransform transform;

        public Tetrahedron(VoxelTransform transform)
        {
            this.transform = transform;
        }

        public VoxelBuffer evaluate()
        {
            Geometry.Tetrahedron tetra = new Geometry.Tetrahedron((int)transform.scale.x);
            VoxelBuffer voxels = VoxelBuffer.Cache.GetOrBuild();
            VoxelCompute.geometry.tetrahedron(
                voxels, 
                tetra, 
                transform.position, 
                (int)transform.voxelType);

            return transform.rotate(voxels);
        }
    }
}