using System;
using System.Collections.Generic;
using System.Text;

using M2M.Position.Interface;

namespace M2M.Algorithm.Pathfinding.Implement.AStar_Dijkstra_DataStructure
{
    /// <summary>
    /// 用于比较两个包含AStarTag的IPosition_Connected对象
    /// </summary>
    /// 
    [Serializable]
    public class AStarTagComparer : TagComparer
    {
        protected IList<Tag> list;

        public AStarTagComparer(IList<Tag> list)
        {
            this.list = list;
        }

        #region IComparer<IPosition_Connected> Members

        public override int Compare(IPosition_Connected p1, IPosition_Connected p2)
        {
            Tag t1 = list[p1.GetIndex()];
            Tag t2 = list[p2.GetIndex()];
            int r = diff(t1.f, t2.f);
            if (r != 0)
                return r;
            return diff(t1.g, t2.g);
        }

        #endregion
    }
}
