using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace _3DRendering
{
    public class Drawing
    {
        public void FillMeshes(Cylinder cylinder, Calculate calculate, Bitmap bitmap)
        {
            SolidBrush solidBrush = new SolidBrush(Color.Red);
            Point[] trianglePoints = new Point[3];
            foreach (var triangle in cylinder.triangles)
            {
                switch (calculate.BackFaceCulling(triangle))
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

        public void DrawMeshes(Cylinder cylinder, Calculate calculate, Bitmap bitmap)
        {
            Pen pen = new Pen(Color.LightSeaGreen);
            Point[] trianglePoints = new Point[3];
            foreach (var triangle in cylinder.triangles)
            {
                switch (calculate.BackFaceCulling(triangle))
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
    }
}
