using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DRendering
{
    public class Session
    {
        public double AngleX { get; set; }
        public double AngleY { get; set; }
        public Cylinder Cylinder { get; set; }
        public int Distance { get; set; }
        public double Height { get; set; }
        public int Mesh { get; set; }
        public double Radius { get; set; }
        public double Theta { get; set; }

        public Session()
        {
            AngleX = 0;
            AngleY = 0;
            Cylinder = new Cylinder(100, 50, 30);
            Distance = -400;
            Height = 100;
            Mesh = 30;
            Radius = 50;
            Theta = 5 * Math.PI / 180;
        }
    }
}
