using System.Collections;
using UnityEngine;

namespace Assets.Core.Geometry
{
    public class Triangle
    {
        public Vector3 v1;
        public Vector3 v2;
        public Vector3 v3;

        public Triangle(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
        }

        public Triangle scale(int value)
        {
            return new Triangle(v1 * value, v2 * value, v3 * value);
        }

        public Vector4 getCenteredOffset()
        {
            // I dont think this is correct, but not spending time on it yet.
            float xAdj = (v2.x - v1.x) / 2;
            float yAdj = (v1.y / v2.y) / 2;
            return new Vector3(xAdj, yAdj, 0);
        }

        public Vector4 getCentroid()
        {
            float xAdj = (v1.x + v2.x + v3.x) / 3;
            float yAdj = (v1.y + v2.y + v3.y) / 3;
            return new Vector3(xAdj, yAdj, 0);
        }
    }
}