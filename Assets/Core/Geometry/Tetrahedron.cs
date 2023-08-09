using System.Collections;
using UnityEngine;

namespace Assets.Core.Geometry
{
    public class Tetrahedron
    {
        public Vector3 v4;
        public Vector3 v1;
        public Vector3 v2;
        public Vector3 v3;

        public int size;

        public Tetrahedron(int size)
        {
            this.size = size;

            // Unit Tetrahedron.
            // https://en.wikipedia.org/wiki/Tetrahedron
            v4 = new Vector3(0, 1.0f, 0);
            v1 = new Vector3(0, -1.0f / 3.0f, Mathf.Sqrt(8.0f / 9.0f));
            v2 = new Vector3(Mathf.Sqrt(2.0f / 3.0f), -1.0f / 3.0f, -Mathf.Sqrt(2.0f / 9.0f));
            v3 = new Vector3(-Mathf.Sqrt(2.0f / 3.0f), -1.0f / 3.0f, -Mathf.Sqrt(2.0f / 9.0f));

            // Scale the unit tetrahedron then adjust with a scaled alignment.
            // Create a vector that will adjust the unit tetrahedron so that it sits in the positive x,y,z volume.
            Vector3 alignment = new Vector3(Mathf.Sqrt(2.0f / 3.0f), 1.0f / 3.0f, Mathf.Sqrt(2.0f / 9.0f)) * size;
            v1 = v1 * size + alignment;
            v2 = v2 * size + alignment;
            v3 = v3 * size + alignment;
            v4 = v4 * size + alignment;
        }

        public Vector4 getCenteredOffset()
        {
            float xAdj = v2.x / 2;
            float yAdj = v4.y / 2;
            float zAdj = v1.z / 2;
            return new Vector3(xAdj, yAdj, zAdj);
        }

        public Vector4 getCentroid()
        {
            float xAdj = (v1.x + v2.x + v3.x + v4.x) / 4;
            float yAdj = (v1.y + v2.y + v3.y + v4.y) / 4;
            float zAdj = (v1.z + v2.z + v3.z + v4.z) / 4;
            return new Vector3(xAdj, yAdj, zAdj);
        }
    }
}