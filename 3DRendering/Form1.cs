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
        double theta = 5 * Math.PI / 180;

        double angleX = 0;
        double angleY = 0;
        float scale = 1;

        int mesh = 30;
        double h = 100;
        double r = 50;
        Cylinder cylinder;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            checkBox1.Checked = true;
            checkBox2.Checked = true;
            checkBox1.CheckStateChanged += CheckBox_CheckStateChanged;
            checkBox2.CheckStateChanged += CheckBox_CheckStateChanged;

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            cylinder = new Cylinder(h, r, mesh);
            Draw();
        }

        private void CheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            Draw();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Left:
                    angleY -= theta;
                    Draw();
                    break;
                case Keys.Right:
                    angleY += theta;
                    Draw();
                    break;
                case Keys.Up:
                    angleX += theta;
                    Draw();
                    break;
                case Keys.Down:
                    angleX -= theta;
                    Draw();
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            scale += 0.1f;
            Draw();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            scale -= 0.1f;
            Draw();
        }

        private void Draw()
        {
            Bitmap bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Matrix4x4 M = Calculate.CalculateTransformationMatrix(pictureBox1.Width,
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
                                                                  angleX,
                                                                  angleY,
                                                                  scale);
            for (int i = 0; i < 4 * cylinder.n + 2; i++)
            {
                cylinder.vertices[i].pv = Calculate.Multiply(cylinder.vertices[i].pv, M);
                cylinder.vertices[i].pv /= cylinder.vertices[i].pv.W;
                cylinder.vertices[i].pv.CopyTo(cylinder.vertices[i].p);
            }
            cylinder.CreateTriangles();

            if (checkBox2.Checked)
            {
                FillCylinderTriangles(bitmap);
            }
            if (checkBox1.Checked)
            {
                DrawCylinderTriangles(bitmap);
            }
            
            

            cylinder = new Cylinder(h, r, mesh);
            pictureBox1.Image = bitmap;
        }

        private void FillCylinderTriangles(Bitmap bitmap)
        {
            SolidBrush solidBrush = new SolidBrush(Color.Red);
            Point[] trianglePoints = new Point[3];
            foreach (var triangle in cylinder.triangles)
            {
                switch (BackFaceCulling(triangle))
                {
                    case true:
                        trianglePoints[0] = new Point((int)triangle.vertices[0].pv.X, (int)triangle.vertices[0].pv.Y);
                        trianglePoints[1] = new Point((int)triangle.vertices[1].pv.X, (int)triangle.vertices[1].pv.Y);
                        trianglePoints[2] = new Point((int)triangle.vertices[2].pv.X, (int)triangle.vertices[2].pv.Y);
                        using (var graphics = Graphics.FromImage(bitmap))
                        {
                            graphics.FillPolygon(solidBrush, trianglePoints);
                        }
                        continue;
                }
            }
        }

        private void DrawCylinderTriangles(Bitmap bitmap)
        {
            Pen pen = new Pen(Color.LightSeaGreen);
            Point[] trianglePoints = new Point[3];
            foreach (var triangle in cylinder.triangles)
            {
                switch (BackFaceCulling(triangle))
                {
                    case true:
                        trianglePoints[0] = new Point((int)triangle.vertices[0].pv.X, (int)triangle.vertices[0].pv.Y);
                        trianglePoints[1] = new Point((int)triangle.vertices[1].pv.X, (int)triangle.vertices[1].pv.Y);
                        trianglePoints[2] = new Point((int)triangle.vertices[2].pv.X, (int)triangle.vertices[2].pv.Y);
                        using (var graphics = Graphics.FromImage(bitmap))
                        {
                            graphics.DrawLines(pen, trianglePoints);
                        }
                        continue;
                }
            }
        }

        private bool BackFaceCulling(Triangle triangle)
        {
            Vector3 v1 = new Vector3(triangle.vertices[1].pv.X - triangle.vertices[0].pv.X,
                                     triangle.vertices[1].pv.Y - triangle.vertices[0].pv.Y,
                                                                                         0);

            Vector3 v2 = new Vector3(triangle.vertices[2].pv.X - triangle.vertices[0].pv.X,
                                     triangle.vertices[2].pv.Y - triangle.vertices[0].pv.Y,
                                                                                         0);

            Vector3 res = Vector3.Cross(v1, v2);

            return res.Z < 0 ? true : false;
        }
    }
}
