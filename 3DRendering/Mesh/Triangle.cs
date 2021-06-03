using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DRendering
{
    public class Triangle
    {
        public Vertex[] vertices = new Vertex[3];

        public Triangle(Vertex v1, Vertex v2, Vertex v3)
        {
            this.vertices[0] = v1;
            this.vertices[1] = v2;
            this.vertices[2] = v3;
        }
    }
}
