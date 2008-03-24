using System;
using System.Collections.Generic;
using System.Text;
using Position_Interface;
using Position_Connected_Interface;

namespace Position_Connected_Implement
{
    /// <summary>
    /// 用于判断两个点的坐标是否相等，相等返回0
    /// </summary>
    [Serializable]
    public class PositionComparer : IComparer<IPosition_Connected>
    {
        public int Compare(IPosition_Connected p1, IPosition_Connected p2)
        {
            if (Math.Abs(p1.GetX() - p2.GetX()) < float.Epsilon && Math.Abs(p1.GetY() - p2.GetY()) < float.Epsilon)
                return 0;
            if (p1.GetX() * p1.GetY() < p2.GetX() * p2.GetY())
                return -1;
            return 1;
        }
    }
}
