using System;
using System.Collections.Generic;
using System.Text;
using Position_Interface;

namespace ConvexHullEngine
{
    //凸包算法接口
    public interface IConvexHullEngine
    {
        IPositionSet ConvexHull(IPositionSet ps);
    }
}
