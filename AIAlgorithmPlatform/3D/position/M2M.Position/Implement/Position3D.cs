using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using M2M.Position.Interface;
using Real = System.Double;

namespace M2M.Position.Implement
{
    [Serializable]
    public class Position3D : IPosition3D
    {
        protected int dimension = 3;
        protected Real[] dim_value = new Real[3];

        #region IPosition3D Members

        public double GetX()
        {
            return dim_value[0];
        }

        public double GetY()
        {
            return dim_value[1];
        }

        public double GetZ()
        {
            return dim_value[2];
        }

        #endregion

        #region IPosition Members

        public int GetDimension()
        {
            return dimension;
        }

        public double GetValue(int dimensionIndex)
        {
            checkBound(dimensionIndex);
            return dim_value[dimensionIndex];
        }

        #endregion

        protected void checkBound(int dimensionIndex)
        {
            if (dimensionIndex < 0 || dimensionIndex >= dimension)
                throw new InvalidOperationException("The dimension index is out of range.");
        }

        public void SetX(Real value)
        {
            dim_value[0] = value;
        }

        public void SetY(Real value)
        {
            dim_value[1] = value;
        }

        public void SetZ(Real value)
        {
            dim_value[2] = value;
        }

        public void SetValue(int dimensionIndex, Real value)
        {
            checkBound(dimensionIndex);
            dim_value[dimensionIndex] = value;
        }
    }
}
