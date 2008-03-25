using System;
using System.Collections.Generic;
using System.Text;
using Position_Interface;

namespace Position_Connected_Interface
{
    //寻径算法用的接口
    public interface IPositionSet_Connected_Adjacency : IPositionSet_Connected
    {
        float GetDistanceToAdjacency();
    }
}
