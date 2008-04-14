using System;
using System.Collections.Generic;
using System.Text;
using Position_Interface;

namespace Position_Connected_Interface
{
    //寻径算法用的接口
    public interface IPositionSet_Connected : IPositionSet
    {
        IPosition_Connected GetPosition_Connected();
    }
}
