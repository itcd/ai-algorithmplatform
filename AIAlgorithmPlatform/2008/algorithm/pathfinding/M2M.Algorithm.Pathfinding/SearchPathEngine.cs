using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using M2M.Algorithm.Pathfinding.NodeTag;
using M2M.Position;
using M2M.Util.DataStructure;
using M2M.Util.DataStructure.PriorityQueue;
using Real = System.Double;
using IPosition_ConnectedSet = System.Collections.Generic.ICollection<M2M.Position.IPosition_Connected>;

namespace M2M.Algorithm.Pathfinding
{
    public abstract class SearchPathEngine : ISearchPathEngine
    {
        protected IComparer<IPosition_Connected> com;
        protected IPosition_ConnectedSet positionSet;
        protected ushort time_stamp;//每次寻径之前都生成一个新的时间戳，用于判断某个节点的信息是有效还是过期的
        protected IPriorityQueue<IPosition_Connected> open;
        protected IList<Tag> list;

        public SearchPathEngine()
        {
            time_stamp = 0;
            open = new IntervalHeap_C5<IPosition_Connected>(com);
            list = new List<Tag>();
        }

        //每次寻径之前根据起点、终点信息做初始化
        protected void Init(IPosition_Connected start, IPosition_Connected end)
        {
            //判断起点和终点是否在地图上，并初始化起点的标签
            //Tag tag = (Tag)start.GetAttachment();

            //生成这次寻径使用的time stamp
            time_stamp = TimeStamp.getNextTimeStamp(time_stamp);

            Tag tag = list[start.GetTagIndex()];
            tag.Clear();
            tag.timeStamp = time_stamp;
        }

        //从终点根据标签中记录的父节点找到起点，生成路径       
        protected void GetPath(List<IPosition_Connected> path, IPosition_Connected p, IPosition_Connected end)
        {
            if (p == end)
            {
                path.Add(end);
                Tag p_tag = list[end.GetTagIndex()];
                while (p_tag.parent != null)
                {
                    path.Add(p_tag.parent);
                    p_tag = list[p_tag.parent.GetTagIndex()];
                }
                path.Reverse();
            }
        }

        #region ISearchPathEngine Members

        //初始化地图，在地图被更改之后调用
        public virtual void InitEngineForMap(ICollection<IPosition_Connected> map)
        {
            positionSet = map;
            list.Clear();
            foreach (IPosition_Connected p in positionSet)
            {
                Tag t = new Tag();
                t.Clear();
                p.SetTagIndex(list.Count);
                list.Add(t);
            }
        }

        public abstract ICollection<IPosition_Connected> SearchPath(IPosition_Connected start, IPosition_Connected end);

        #endregion
    }
}