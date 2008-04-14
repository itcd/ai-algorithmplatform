using System;
using System.Collections.Generic;
using System.Text;
using M2M.Position;

namespace M2M.Algorithm.Pathfinding.NodeTag
{
    /// <summary>
    /// ���ڱȽ���������AStarTag��IPosition_Connected����
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
            Tag t1 = list[p1.GetTagIndex()];
            Tag t2 = list[p2.GetTagIndex()];
            int r = diff(t1.f, t2.f);
            if (r != 0)
                return r;
            return diff(t1.g, t2.g);
        }

        #endregion
    }
}