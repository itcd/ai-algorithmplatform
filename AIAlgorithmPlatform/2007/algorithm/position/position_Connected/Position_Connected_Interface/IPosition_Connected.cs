using System;
using System.Collections.Generic;
using System.Text;
using Position_Interface;

namespace Position_Connected_Interface
{
    //Ѱ���㷨�õĽӿ�
    public interface IPosition_Connected : IPosition
    {
        IPositionSet_Connected_Adjacency GetAdjacencyPositionSet();
        Object GetAttachment();
        void SetAttachment(Object o);
    }
}
