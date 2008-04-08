using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

using M2M.Position.Interface;
using Real = System.Double;

namespace M2M.Position.Implement
{
    [Serializable]
    public class Position3D : IPosition3D  
    {
        public static implicit operator Point3D(Position3D position3D)
        {
            return new Point3D(position3D.GetX(), position3D.GetY(), position3D.GetZ());
        }

        public static implicit operator Vector3D(Position3D position3D)
        {
            return new Vector3D(position3D.GetX(), position3D.GetY(), position3D.GetZ());
        }

        public static implicit operator Position3D(Point3D point3D)
        {
            return new Position3D(point3D.X, point3D.Y, point3D.Z);
        }

        protected static int dimension = 3;
        protected Real x;
        protected Real y;
        protected Real z;

        public Position3D(Real x, Real y)
        {
            this.x = x;
            this.y = y;
            this.z = 0;           
        }

        public Position3D(Real x, Real y, Real z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        #region IPosition3D Members

        public double GetX()
        {
            return x;
        }

        public double GetY()
        {
            return y;
        }

        public double GetZ()
        {
            return z;
        }

        #endregion

        #region IPosition Members

        public int GetDimension()
        {
            return dimension;
        }

        public double GetValue(int dimensionIndex)
        {
            //checkBound(dimensionIndex);

            switch (dimensionIndex)
            {
                case 0: return x;
                case 1: return y;
                case 2: return z;
                default: throw new Exception("dimension error !");
            }
        }

        #endregion

        //protected void checkBound(int dimensionIndex)
        //{
        //    if (dimensionIndex < 0 || dimensionIndex >= dimension)
        //        throw new InvalidOperationException("The dimension index is out of range.");
        //}

        public void SetX(Real value)
        {
            x = value;
        }

        public void SetY(Real value)
        {
            y = value;
        }

        public void SetZ(Real value)
        {
            z = value;
        }       
    }
}
