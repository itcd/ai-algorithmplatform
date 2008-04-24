using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

namespace M2M.Util
{
    public class Trackball
    {
        /*
         * Project an x,y pair onto a sphere of radius r OR a hyperbolic sheet
         * if we are away from the center of the sphere.
         */
        static public double projectToSphere(double r, double x, double y)
        {
            double d, t, z;

            d = Math.Sqrt(x * x + y * y);
            if (d < r * 0.70710678118654752440)
            {    /* Inside sphere */
                z = Math.Sqrt(r * r - d * d);
            }
            else
            {           /* On hyperbola */
                t = r / 1.41421356237309504880;
                z = t * t / d;
            }
            return z;
        }

        static public Vector3D projectToVector(double x1, double y1, double x2, double y2, out double radius)
        {
            double d1 = Math.Sqrt(x1 * x1 + y1 * y1);
            double d2 = Math.Sqrt(x2 * x2 + y2 * y2);
            radius = d1 > d2 ? d1 : d2;
            double z1 = projectToSphere(radius, x1, y1);
            double z2 = projectToSphere(radius, x2, y2);
            return new Vector3D(x2 - x1, y2 - y1, z2 - z1);
        }
    }
}
