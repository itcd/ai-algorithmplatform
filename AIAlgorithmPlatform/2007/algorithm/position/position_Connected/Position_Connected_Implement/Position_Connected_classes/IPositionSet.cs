using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructure
{
    //<Ѱ���㷨�õĽӿ�>
    public interface IPositionSet
    {
        void InitToTraverseSet();
        bool NextPosition();
        IPosition GetPosition();
        int GetNum();
        Array ToArray();
    }
}
