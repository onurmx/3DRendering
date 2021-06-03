using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace _3DRendering
{
    public class Cylinder
    {
        public int n;
        public Vertex[] vertices;
        public Triangle[] triangles;

        public Cylinder(double h, double r, int numberOfMesh)
        {
            n = numberOfMesh;

            // Create mesh
            int numberOfVertices = 4 * n + 2;
            int numberOfTriangles = 4 * n;

            vertices = new Vertex[numberOfVertices];
            triangles = new Triangle[numberOfTriangles];

            // Top base
            int[] normal = { 0, 1, 0, 0 };
            float[] points = { 0, (float)h, 0, 1 };

            vertices[0] = new Vertex(points, normal);

            for (int i = 0; i < n; i++)
            {
                points[0] = (float)(r * Math.Cos(2 * Math.PI * i / n));
                points[2] = (float)(r * Math.Sin(2 * Math.PI * i / n));

                vertices[i + 1] = new Vertex(points, normal);
            }

            // Bottom base
            normal[1] = -1;

            points[0] = 0;
            points[1] = 0;
            points[2] = 0;

            vertices[4 * n + 1] = new Vertex(points, normal);

            for (int i = 0; i < n; i++)
            {
                points[0] = (float)(r * Math.Cos(2 * Math.PI * i / n));
                points[2] = (float)(r * Math.Sin(2 * Math.PI * i / n));

                vertices[3 * n + 1 + i] = new Vertex(points, normal);
            }

            // Sides
            normal[1] = 0;
            normal[3] = 0;
            for (int i = n + 1; i < 2 * n + 1; i++)
            {
                points = vertices[i - n].p;

                normal[0] = (int)(points[0] / r);
                normal[2] = (int)(points[2] / r);

                vertices[i] = new Vertex(points, normal);
            }

            for (int i = 2 * n + 1; i < 3 * n + 1; i++)
            {
                points = vertices[i + n].p;

                normal[0] = (int)(points[0] / r);
                normal[2] = (int)(points[2] / r);

                vertices[i] = new Vertex(points, normal);
            }

            // Create Triangles
            CreateTriangles();
        }

        public void CreateTriangles()
        {
            // Top base
            triangles[n - 1] = new Triangle(vertices[0], vertices[1], vertices[n]);
            for (int i = 0; i < n - 1; i++)
            {
                triangles[i] = new Triangle(vertices[0], vertices[i + 2], vertices[i + 1]);
            }

            // Bottom base
            triangles[4 * n - 1] = new Triangle(vertices[4 * n + 1], vertices[4 * n], vertices[3 * n + 1]);
            for (int i = 3 * n; i < 4 * n - 1; i++)
            {
                triangles[i] = new Triangle(vertices[4 * n + 1], vertices[i + 1], vertices[i + 2]);
            }

            // Sides
            triangles[2 * n - 1] = new Triangle(vertices[2 * n], vertices[n + 1], vertices[3 * n]);
            triangles[3 * n - 1] = new Triangle(vertices[3 * n], vertices[n + 1], vertices[2 * n + 1]);

            for (int i = n; i < 2 * n - 1; i++)
            {
                triangles[i] = new Triangle(vertices[i + 1], vertices[i + 2], vertices[i + 1 + n]);
            }

            for (int i = 2 * n; i < 3 * n - 1; i++)
            {
                triangles[i] = new Triangle(vertices[i + 1], vertices[i + 2 - n], vertices[i + 2]);
            }
        }
    }
}
