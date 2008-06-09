using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

using M2M.Position;

namespace M2M.Media3D
{
    /// <summary>
    /// Warper for IPosition in Vector3D operation
    /// </summary>
    public class Vector3D_IPosition
    {
        public static Vector3D ToVector3D(IPosition p)
        {
            return new Vector3D(p.GetValue(0), p.GetValue(1), p.GetValue(2));
        }

        public static Point3D ToPoint3D(IPosition p)
        {
            return new Point3D(p.GetValue(0), p.GetValue(1), p.GetValue(2));
        }

        public static double AngleBetween(IPosition p1, IPosition p2)
        {
            return Vector3D.AngleBetween(ToVector3D(p1), ToVector3D(p2));
        }

        public static Vector3D CrossProduct(IPosition p1, IPosition p2)
        {
            return Vector3D.CrossProduct(ToVector3D(p1), ToVector3D(p2));
        }

        public static Vector3D Divide(IPosition p, double scalar)
        {
            return Vector3D.Divide(ToVector3D(p), scalar);
        }

        public static double DotProduct(IPosition p1, IPosition p2)
        {
            return Vector3D.DotProduct(ToVector3D(p1), ToVector3D(p2));
        }

        public static Vector3D Subtract(IPosition p1, IPosition p2)
        {
            return Vector3D.Subtract(ToVector3D(p1), ToVector3D(p2));
        }
    }
}
