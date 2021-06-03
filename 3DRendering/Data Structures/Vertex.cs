using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace _3DRendering
{
    public class Vertex
    {
        public float[] p = new float[4];
        public Vector4 pv;
        public Vector4 n;
        public Vector2 t;

        public Vertex(float[] p, int[] n)
        {
            for (int index = 0; index < 4; ++index)
            {
                this.p[index] = p[index];
            }
            this.pv = new Vector4(p[0], p[1], p[2], p[3]);
            this.n = new Vector4((float)n[0], (float)n[1], (float)n[2], (float)n[3]);
            this.t = new Vector2();
        }
    }
}
