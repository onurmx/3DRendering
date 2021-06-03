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
                                                       Vector3 cTarget,
                                                       Vector3 cPos,
                                                       Vector3 cUp,
                                                       double FoV,
                                                       double angX,
                                                       double angY,
                                                       float scale)
        {
            Matrix4x4 model = Model(sx, sy);
            Matrix4x4 CVM = CameraView(cTarget, cPos, cUp);
            Matrix4x4 PPM = PerspectiveProjection(sx, sy, FoV);

            Matrix4x4 rotX = RotationX(angX);
            Matrix4x4 rotY = RotationY(angY);
            Matrix4x4 scaling = Scaling(scale);
            Matrix4x4 trans1 = Trans1();

            Matrix4x4 M = new Matrix4x4(1, 0, 0, 0,
                                        0, 1, 0, 0,
                                        0, 0, 1, 0,
                                        0, 0, 0, 1);

            M = Matrix4x4.Multiply(M, PPM);
            M = Matrix4x4.Multiply(M, CVM);
            M = Matrix4x4.Multiply(M, model);
            M = Matrix4x4.Multiply(M, rotY);
            M = Matrix4x4.Multiply(M, rotX);
            M = Matrix4x4.Multiply(M, scaling);
            M = Matrix4x4.Multiply(M, trans1);

            return M;
        }

        private Matrix4x4 Model(float sx, float sy)
        {
            Matrix4x4 M = new Matrix4x4(1, 0, 0, sx / 2,
                                        0, 1, 0, sy / 2,
                                        0, 0, 1,      0,
                                        0, 0, 0,      1);
            return M;
        }

        private Matrix4x4 CameraView(Vector3 cTarget, Vector3 cPos, Vector3 cUp)
        {
            var tZ = Vector3.Subtract(cPos, cTarget);
            Vector3 cZ = Vector3.Divide(tZ, tZ.Length());

            var tX = Vector3.Cross(cUp, cZ);
            Vector3 cX = Vector3.Divide(tX, tX.Length());

            var tY = Vector3.Cross(cZ, cX);
            Vector3 cY = Vector3.Divide(tY, tY.Length());

            Matrix4x4 M = new Matrix4x4(cX.X, cX.Y, cX.Z, Vector3.Dot(cX, cPos),
                                        cY.X, cY.Y, cY.Z, Vector3.Dot(cY, cPos),
                                        cZ.X, cZ.Y, cZ.Z, Vector3.Dot(cZ, cPos),
                                           0,    0,    0,                     1);
            return M;
        }

        private Matrix4x4 PerspectiveProjection(float sx, float sy, double FoV)
        {
            Matrix4x4 M = new Matrix4x4((float)(-sx / 2 / Math.Tan(FoV / 2)), 0, (float)(sx / 2), 0,
                                        0, (float)(sx / 2 / Math.Tan(FoV / 2)), (float)(sy / 2), 0,
                                        0, 0, 0, 1,
                                        0, 0, 1, 0);
            return M;
        }

        private Matrix4x4 RotationX(double angX)
        {
            Matrix4x4 M = new Matrix4x4(1,                     0,                        0, 0,
                                        0, (float)Math.Cos(angX), (float)(-Math.Sin(angX)), 0,
                                        0, (float)Math.Sin(angX),    (float)Math.Cos(angX), 0,
                                        0,                     0,                        0, 1);
            return M;
        }

        private Matrix4x4 RotationY(double angY)
        {
            Matrix4x4 M = new Matrix4x4(   (float)Math.Cos(angY), 0, (float)Math.Sin(angY), 0,
                                                               0, 1,                     0, 0,
                                        (float)(-Math.Sin(angY)), 0, (float)Math.Cos(angY), 0,
                                                               0, 0,                     0, 1);
            return M;
        }

        private Matrix4x4 Scaling(float scale)
        {
            Matrix4x4 M = new Matrix4x4(scale,     0,     0, 0,
                                            0, scale,     0, 0,
                                            0,     0, scale, 0,
                                            0,     0,     0, 1);
            return M;
        }

        private Matrix4x4 Trans1()
        {
            Matrix4x4 M = new Matrix4x4(1, 0, 0,   0,
                                        0, 1, 0, -50,
                                        0, 0, 1,   0,
                                        0, 0, 0,   1);
            return M;
        }

        public Vector4 MyMultiply(Vector4 self, Matrix4x4 matrix)
        {
            return new Vector4(
                matrix.M11 * self.X + matrix.M12 * self.Y + matrix.M13 * self.Z + matrix.M14 * self.W,
                matrix.M21 * self.X + matrix.M22 * self.Y + matrix.M23 * self.Z + matrix.M24 * self.W,
                matrix.M31 * self.X + matrix.M32 * self.Y + matrix.M33 * self.Z + matrix.M34 * self.W,
                matrix.M41 * self.X + matrix.M42 * self.Y + matrix.M43 * self.Z + matrix.M44 * self.W
            );
        }
    }
}
