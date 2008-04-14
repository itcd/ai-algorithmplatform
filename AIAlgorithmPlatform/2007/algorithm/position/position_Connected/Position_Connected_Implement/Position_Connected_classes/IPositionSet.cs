using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructure
{
    //<寻径算法用的接口>
    public interface IPositionSet
    {
        void InitToTraverseSet();
        bool NextPosition();
        IPosition GetPosition();
        int GetNum();
        Array ToArray();
    }
}
