using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;

namespace _3DRendering
{
    public partial class Form1 : Form
    {
        Calculate Calculate = new Calculate();

        double angX = 0;
        double angY = 0;
        float scale = 1;

        double h = 100;
        double r = 50;
        Cylinder cylinder;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            cylinder = new Cylinder(h, r);
            TestDraw();
        }

        private void TestDraw()
        {
            Bitmap bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Matrix4x4 M = Calculate.CreateTransformationMatrix(pictureBox1.Width,
                                                               pictureBox1.Height,
                                                               new Vector3(pictureBox1.Width / 2,
                                                                           pictureBox1.Height / 2,
                                                                           0),
                                                               new Vector3(0,
                                                                           0,
                                                                           0),
                                                               new Vector3(0,
                                                                           1,
                                                                           0),
                                                               (Math.PI / 3),
                                                               angX,
                                                               angY,
                                                               scale);
            for (int i = 0; i < 4 * cylinder.n + 2; i++)
            {
                cylinder.vertices[i].pv = Calculate.MyMultiply(cylinder.vertices[i].pv, M);
                cylinder.vertices[i].pv /= cylinder.vertices[i].pv.W;
                cylinder.vertices[i].pv.CopyTo(cylinder.vertices[i].p);
            }
            cylinder.CreateTriangles();
            bitmap = DrawCylinderVertices(bitmap);
            bitmap = DrawCylinderTriangles(bitmap);

            cylinder = new Cylinder(h, r);
            pictureBox1.Image = bitmap;
        }

        private Bitmap DrawCylinderVertices(Bitmap cbmp)
        {
            Color color = Color.Black;
            int x, y;

            foreach (var vertex in cylinder.vertices)
            {
                x = (int)vertex.p[0];
                y = (int)vertex.p[1];
                if (x > 0 && x < cbmp.Width && y > 0 && y < cbmp.Height)
                {
                    cbmp.SetPixel(x, y, color);
                }
            }

            return cbmp;
        }

        private Bitmap DrawCylinderTriangles(Bitmap cbmp)
        {
            Pen pen = new Pen(System.Drawing.Color.LightSeaGreen);
            Point[] trianglePoints = new Point[3];
            foreach (var triangle in cylinder.triangles)
            {
                if (BackFaceCulling(triangle))
                {
                    trianglePoints[0] = new Point((int)triangle.vertices[0].pv.X, (int)triangle.vertices[0].pv.Y);
                    trianglePoints[1] = new Point((int)triangle.vertices[1].pv.X, (int)triangle.vertices[1].pv.Y);
                    trianglePoints[2] = new Point((int)triangle.vertices[2].pv.X, (int)triangle.vertices[2].pv.Y);
                    using (var graphics = Graphics.FromImage(cbmp))
                    {
                        graphics.DrawLines(pen, trianglePoints);
                    }
                }
            }
            return cbmp;
        }

        private bool BackFaceCulling(Triangle t)
        {
            Vector3 v1 = new Vector3(t.vertices[1].pv.X - t.vertices[0].pv.X,
                                     t.vertices[1].pv.Y - t.vertices[0].pv.Y,
                                                                           0);

            Vector3 v2 = new Vector3(t.vertices[2].pv.X - t.vertices[0].pv.X,
                                     t.vertices[2].pv.Y - t.vertices[0].pv.Y,
                                                                           0);

            Vector3 res = Vector3.Cross(v1, v2);

            if (res.Z < 0)
                return true;
            else
                return false;
        }
    }
}
