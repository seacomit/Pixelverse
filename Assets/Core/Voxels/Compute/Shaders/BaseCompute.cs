namespace Assets.Core.Voxels.Compute.Shaders
{
    public class BaseCompute
    {
        protected int getThreadSize(int bufferSize)
        {
            return bufferSize / 8;
        }
    }
}