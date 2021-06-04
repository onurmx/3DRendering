using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace _3DRendering
{
    public class Calculate
    {
        public Matrix4x4 CalculateTransformationMatrix(float sx,
                                                       float sy,
                                                       double FoV,
                                                       double angleX,
                                                       double angleY,
                                                       int distance)
        {
            Matrix4x4 PPM = PerspectiveProjectionMatrix(sx, sy, FoV);
            Matrix4x4 rotX = RotationX(angleX);
            Matrix4x4 rotY = RotationY(angleY);
            Matrix4x4 T1 = TranslationMatrix1();
            Matrix4x4 CDTM = CameraDistanceTranslationMatrix(distance);

            Matrix4x4 M = Matrix4x4.Multiply(PPM, CDTM);
            M = Matrix4x4.Multiply(M, rotY);
            M = Matrix4x4.Multiply(M, rotX);
            M = Matrix4x4.Multiply(M, T1);

            return M;
        }

        private Matrix4x4 PerspectiveProjectionMatrix(float sx, float sy, double FoV)
        {
            Matrix4x4 M = new Matrix4x4((float)(-sx / 2 / Math.Tan(FoV / 2)), 0, (float)(sx / 2), 0,
                                        0, (float)(sx / 2 / Math.Tan(FoV / 2)), (float)(sy / 2), 0,
                                        0, 0, 0, 1,
                                        0, 0, 1, 0);
            return M;
        }

        private Matrix4x4 RotationX(double angleX)
        {
            Matrix4x4 M = new Matrix4x4(1,                       0,                          0, 0,
                                        0, (float)Math.Cos(angleX), (float)(-Math.Sin(angleX)), 0,
                                        0, (float)Math.Sin(angleX),    (float)Math.Cos(angleX), 0,
                                        0,                       0,                          0, 1);
            return M;
        }

        private Matrix4x4 RotationY(double angleY)
        {
            Matrix4x4 M = new Matrix4x4(   (float)Math.Cos(angleY), 0, (float)Math.Sin(angleY), 0,
                                                                 0, 1,                       0, 0,
                                        (float)(-Math.Sin(angleY)), 0, (float)Math.Cos(angleY), 0,
                                                                 0, 0,                       0, 1);
            return M;
        }

        private Matrix4x4 TranslationMatrix1()
        {
            Matrix4x4 M = new Matrix4x4(1, 0, 0,   0,
                                        0, 1, 0, -50,
                                        0, 0, 1,   0,
                                        0, 0, 0,   1);
            return M;
        }

        private Matrix4x4 CameraDistanceTranslationMatrix(int distance)
        {
            Matrix4x4 M = new Matrix4x4(1, 0, 0,        0,
                                        0, 1, 0,        0,
                                        0, 0, 1, distance,
                                        0, 0, 0,        1);
            return M;
        }

        public Vector4 Multiply(Vector4 a, Matrix4x4 matrix)
        {
            return new Vector4(
                matrix.M11 * a.X + matrix.M12 * a.Y + matrix.M13 * a.Z + matrix.M14 * a.W,
                matrix.M21 * a.X + matrix.M22 * a.Y + matrix.M23 * a.Z + matrix.M24 * a.W,
                matrix.M31 * a.X + matrix.M32 * a.Y + matrix.M33 * a.Z + matrix.M34 * a.W,
                matrix.M41 * a.X + matrix.M42 * a.Y + matrix.M43 * a.Z + matrix.M44 * a.W
            );
        }

        public bool BackFaceCulling(Triangle triangle)
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
