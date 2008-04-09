using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using M2M.Position.Interface;
using Real = System.Double;
using IPosition_ConnectedSet = System.Collections.Generic.ICollection<M2M.Position.Interface.IPosition_Connected>;

namespace M2M.Algorithm.Pathfinding.Implement.AStar_Dijkstra_DataStructure
{
    [Serializable]
    public class DijkstraTagComparer : TagComparer
    {
        protected IList<Tag> list;

        public DijkstraTagComparer(IList<Tag> list)
        {
            this.list = list;
        }

        #region IComparer<IPosition_Connected> Members

        public override int Compare(IPosition_Connected p1, IPosition_Connected p2)
        {
            return diff(list[p1.GetTagIndex()].g, list[p2.GetTagIndex()].g);
        }

        #endregion
    }
}
