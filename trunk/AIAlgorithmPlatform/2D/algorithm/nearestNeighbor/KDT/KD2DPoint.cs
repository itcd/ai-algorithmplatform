using System;
using Position_Interface;

namespace KDT
{
    public class KD2DPoint : KDTree.IKDTreeDomain, IPosition
    {
        #region IKDTreeDomain 成员

        public float X;
        public float Y;
        public float GetX()
        {
            return X;
        }

        public float GetY()
        {
            return Y;
        }

        public KD2DPoint(IPosition p)
        {
            X = p.GetX();
            Y = p.GetY();
        }
        public KD2DPoint(float x, float y)
        {
            X = x;
            Y = y;
        }
        public int DimensionCount
        {
            get
            {
                return 2;
            }
        }

        public bool Equals(KD2DPoint obj)
        {
            if (this.X == obj.X && this.Y == obj.Y)
                return true;
            else
                return false;
        }

        public float GetDimensionElement(int dim)
        {
            if (dim == 0)
            {
                return X;
            }
            else if (dim == 1)
            {
                return Y;
            }
            else
                throw new Exception("超出维数范围！");
        }

        #endregion

    }
}
