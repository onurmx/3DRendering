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
        private Calculate Calculate = new Calculate();
        private Drawing Drawing = new Drawing();
        private Session Session = new Session();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            base.MouseWheel += Form1_MouseWheel;
            Draw();
        }

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((ToolStripMenuItem)sender).Checked = !((ToolStripMenuItem)sender).Checked;
        }

        private void ToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            Draw();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.W:
                    Session.AngleX -= Session.Theta;
                    Draw();
                    break;
                case Keys.S:
                    Session.AngleX += Session.Theta;
                    Draw();
                    break;
                case Keys.A:
                    Session.AngleY -= Session.Theta;
                    Draw();
                    break;
                case Keys.D:
                    Session.AngleY += Session.Theta;
                    Draw();
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0 && Session.Distance < -200)
            {
                Session.Distance += 20;
                Draw();
            }
            else if (e.Delta <= 0)
            {
                Session.Distance -= 20;
                Draw();
            }
        }

        private void Draw()
        {
            Bitmap bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            Matrix4x4 M = Calculate.CalculateTransformationMatrix(pictureBox1.Width,
                                                                  pictureBox1.Height,
                                                                  (Math.PI / 3),
                                                                  Session.AngleX,
                                                                  Session.AngleY,
                                                                  Session.Distance);
            for (int i = 0; i < 4 * Session.Cylinder.n + 2; i++)
            {
                Session.Cylinder.vertices[i].pv = Calculate.Multiply(Session.Cylinder.vertices[i].pv, M);
                Session.Cylinder.vertices[i].pv /= Session.Cylinder.vertices[i].pv.W;
                Session.Cylinder.vertices[i].pv.CopyTo(Session.Cylinder.vertices[i].p);
            }
            Session.Cylinder.CreateTriangles();
            if (fillMeshesToolStripMenuItem.Checked)
            {
                Drawing.FillMeshes(Session.Cylinder, Calculate, bitmap);
            }
            if (showWireframeToolStripMenuItem.Checked)
            {
                Drawing.DrawMeshes(Session.Cylinder, Calculate, bitmap);
            }

            Session.Cylinder = new Cylinder(Session.Height, Session.Radius, Session.Mesh);
            pictureBox1.Image = bitmap;
        }
    }
}