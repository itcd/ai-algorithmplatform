using System;
using System.Collections.Generic;
using System.Text;

namespace Position
{
    //点的简单实现
    public class SimplePosition : IPosition
    {
        float X, Y;
        public SimplePosition(float X, float Y)
        {
            this.X = X;
            this.Y = Y;
        }
        public float GetX() { return X; }
        public float GetY() { return Y; }
    }

    //点集的链表实现
    public class SimplePositionSet : IPositionSet
    {
        List<IPosition> posSet = new List<IPosition>();
        int p = -1, n = 0;

        public void AddPosition(IPosition pos)
        {
            posSet.Add(pos);
            n++;
        }

        public void InitToTraverseSet()
        {
            p = -1;
        }

        public IPosition GetPosition()
        {
            if (p == n)
                return null;
            else
                return posSet[p];
        }

        public bool NextPosition()
        {
            p++;
            return !(p == n);
        }

        public int GetNum()
        {
            return this.ToArray().Length;
        }

        public Array ToArray()
        {
            return posSet.ToArray();
        }
    }
}
