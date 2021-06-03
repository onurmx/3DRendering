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
        public int n = 12;
        public Vertex[] vertices = (Vertex[])null;
        public Triangle[] triangles = (Triangle[])null;

        public Cylinder(double h, double r)
        {
            int length1 = 4 * this.n + 2;
            int length2 = 4 * this.n;
            this.vertices = new Vertex[length1];
            this.triangles = new Triangle[length2];
            int[] n = new int[4] { 0, 1, 0, 0 };
            float[] p1 = new float[4] { 0.0f, (float)h, 0.0f, 1f };
            this.vertices[0] = new Vertex(p1, n);
            for (int index = 0; index < this.n; ++index)
            {
                p1[0] = (float)(r * Math.Cos(2.0 * Math.PI * (double)index / (double)this.n));
                p1[2] = (float)(r * Math.Sin(2.0 * Math.PI * (double)index / (double)this.n));
                this.vertices[index + 1] = new Vertex(p1, n);
            }
            n[1] = -1;
            p1[0] = 0.0f;
            p1[1] = 0.0f;
            p1[2] = 0.0f;
            this.vertices[4 * this.n + 1] = new Vertex(p1, n);
            for (int index = 0; index < this.n; ++index)
            {
                p1[0] = (float)(r * Math.Cos(2.0 * Math.PI * (double)index / (double)this.n));
                p1[2] = (float)(r * Math.Sin(2.0 * Math.PI * (double)index / (double)this.n));
                this.vertices[3 * this.n + 1 + index] = new Vertex(p1, n);
            }
            n[1] = 0;
            n[3] = 0;
            for (int index = this.n + 1; index < 2 * this.n + 1; ++index)
            {
                float[] p2 = this.vertices[index - this.n].p;
                n[0] = (int)((double)p2[0] / r);
                n[2] = (int)((double)p2[2] / r);
                this.vertices[index] = new Vertex(p2, n);
            }
            for (int index = 2 * this.n + 1; index < 3 * this.n + 1; ++index)
            {
                float[] p2 = this.vertices[index + this.n].p;
                n[0] = (int)((double)p2[0] / r);
                n[2] = (int)((double)p2[2] / r);
                this.vertices[index] = new Vertex(p2, n);
            }
            this.SetTextureCoordinates();
            this.CreateTriangles();
        }

        public void CreateTriangles()
        {
            this.triangles[this.n - 1] = new Triangle(this.vertices[0], this.vertices[1], this.vertices[this.n]);
            for (int index = 0; index < this.n - 1; ++index)
                this.triangles[index] = new Triangle(this.vertices[0], this.vertices[index + 2], this.vertices[index + 1]);
            this.triangles[4 * this.n - 1] = new Triangle(this.vertices[4 * this.n + 1], this.vertices[4 * this.n], this.vertices[3 * this.n + 1]);
            for (int index = 3 * this.n; index < 4 * this.n - 1; ++index)
                this.triangles[index] = new Triangle(this.vertices[4 * this.n + 1], this.vertices[index + 1], this.vertices[index + 2]);
            this.triangles[2 * this.n - 1] = new Triangle(this.vertices[2 * this.n], this.vertices[this.n + 1], this.vertices[3 * this.n]);
            this.triangles[3 * this.n - 1] = new Triangle(this.vertices[3 * this.n], this.vertices[this.n + 1], this.vertices[2 * this.n + 1]);
            for (int n = this.n; n < 2 * this.n - 1; ++n)
                this.triangles[n] = new Triangle(this.vertices[n + 1], this.vertices[n + 2], this.vertices[n + 1 + this.n]);
            for (int index = 2 * this.n; index < 3 * this.n - 1; ++index)
                this.triangles[index] = new Triangle(this.vertices[index + 1], this.vertices[index + 2 - this.n], this.vertices[index + 2]);
        }

        private void SetTextureCoordinates()
        {
            this.vertices[0].t.X = 0.25f;
            this.vertices[0].t.Y = 0.25f;
            for (int index = 1; index <= this.n; ++index)
            {
                double num = 2.0 * Math.PI * (double)(index - 1) / (double)this.n;
                this.vertices[index].t.X = (float)(1.0 + Math.Cos(num)) / 4f;
                this.vertices[index].t.Y = (float)(1.0 + Math.Sin(num)) / 4f;
            }
            this.vertices[4 * this.n + 1].t.X = 0.75f;
            this.vertices[4 * this.n + 1].t.Y = 0.25f;
            for (int index = 3 * this.n + 1; index <= 4 * this.n; ++index)
            {
                double num = 2.0 * Math.PI * (double)(index - 1) / (double)this.n;
                this.vertices[index].t.X = (float)(3.0 + Math.Cos(num)) / 4f;
                this.vertices[index].t.Y = (float)(1.0 + Math.Sin(num)) / 4f;
            }
            for (int index = 1; index <= this.n; ++index)
            {
                this.vertices[index + this.n].t.X = (float)(index - 1) / (float)(this.n - 1);
                this.vertices[index + this.n].t.Y = 1f;
            }
            for (int index = 1; index <= this.n; ++index)
            {
                this.vertices[index + 2 * this.n].t.X = (float)(index - 1) / (float)(this.n - 1);
                this.vertices[index + 2 * this.n].t.Y = 0.5f;
            }
        }

        public void Scaling(float sx, float sy, float sz)
        {
            Matrix4x4 matrix = new Matrix4x4(sx, 0.0f, 0.0f, 0.0f, 0.0f, sy, 0.0f, 0.0f, 0.0f, 0.0f, sz, 0.0f, 0.0f, 0.0f, 0.0f, 1f);
            for (int index = 0; index < 4 * this.n + 2; ++index)
            {
                this.vertices[index].pv = Vector4.Transform(this.vertices[index].pv, matrix);
                this.vertices[index].pv.CopyTo(this.vertices[index].p);
            }
            this.CreateTriangles();
        }

        public void RotatingY(float theta)
        {
            double num1 = (double)theta * Math.PI / 180.0;
            float num2 = (float)Math.Cos(num1);
            float m13 = (float)Math.Sin(num1);
            Matrix4x4 matrix = new Matrix4x4(num2, 0.0f, m13, 0.0f, 0.0f, 1f, 0.0f, 0.0f, -m13, 0.0f, num2, 0.0f, 0.0f, 0.0f, 0.0f, 1f);
            for (int index = 0; index < 4 * this.n + 2; ++index)
            {
                this.vertices[index].pv = Vector4.Transform(this.vertices[index].pv, matrix);
                this.vertices[index].pv.CopyTo(this.vertices[index].p);
            }
            this.CreateTriangles();
        }

        public void RotatingX(float theta)
        {
            double num1 = (double)theta * Math.PI / 180.0;
            float num2 = (float)Math.Cos(num1);
            float m32 = (float)Math.Sin(num1);
            Matrix4x4 matrix = new Matrix4x4(1f, 0.0f, 0.0f, 0.0f, 0.0f, num2, -m32, 0.0f, 0.0f, m32, num2, 0.0f, 0.0f, 0.0f, 0.0f, 1f);
            for (int index = 0; index < 4 * this.n + 2; ++index)
            {
                this.vertices[index].pv = Vector4.Transform(this.vertices[index].pv, matrix);
                this.vertices[index].pv.CopyTo(this.vertices[index].p);
            }
            this.CreateTriangles();
        }

        public void View(float x0, float y0, Matrix4x4 CVM, Matrix4x4 PPM)
        {
            Vector4 right = new Vector4(x0, y0, 0.0f, 0.0f);
            for (int index = 0; index < 4 * this.n + 2; ++index)
            {
                this.vertices[index].pv = Vector4.Add(this.vertices[index].pv, right);
                this.vertices[index].pv.CopyTo(this.vertices[index].p);
            }
            for (int index = 0; index < 4 * this.n + 2; ++index)
            {
                this.vertices[index].pv = Vector4.Transform(this.vertices[index].pv, CVM);
                this.vertices[index].pv.CopyTo(this.vertices[index].p);
            }
            for (int index = 0; index < 4 * this.n + 2; ++index)
            {
                this.vertices[index].pv = Vector4.Transform(this.vertices[index].pv, PPM);
                this.vertices[index].pv = Vector4.Divide(this.vertices[index].pv, this.vertices[index].pv.W);
                this.vertices[index].pv.CopyTo(this.vertices[index].p);
            }
            this.CreateTriangles();
        }
    }
}
