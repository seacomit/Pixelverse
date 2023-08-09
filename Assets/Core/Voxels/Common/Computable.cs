using System.Collections;
using UnityEngine;

namespace Assets.Core.Voxels.Common
{
    public interface Computable
    {

        int size();

        ComputeBuffer buffer();
    }
}