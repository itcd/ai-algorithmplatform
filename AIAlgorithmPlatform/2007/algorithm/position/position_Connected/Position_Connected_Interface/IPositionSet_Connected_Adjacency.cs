using System;
using System.Collections.Generic;
using System.Text;
using Position_Interface;

namespace Position_Connected_Interface
{
    //Ѱ���㷨�õĽӿ�
    public interface IPositionSet_Connected_Adjacency : IPositionSet_Connected
    {
        float GetDistanceToAdjacency();
    }
}
