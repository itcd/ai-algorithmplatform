using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using M2M.Position.Interface;

using Real = System.Double;

namespace M2M.Algorithm.Pathfinding.Implement.AStar_Dijkstra_DataStructure
{
    public abstract class TagComparer : IComparer<IPosition_Connected>
    {
        #region IComparer<IPosition_Connected> Members

        public abstract int Compare(IPosition_Connected p1, IPosition_Connected p2);

        #endregion

        public int diff(Real value1, Real value2)
        {
            Real d = value1 - value2;
            if (d > float.Epsilon)
                return 1;
            if (d < -float.Epsilon)
                return -1;
            return 0;
        }
    }
}
