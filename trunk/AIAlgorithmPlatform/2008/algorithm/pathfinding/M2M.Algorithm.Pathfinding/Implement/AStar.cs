using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataStructure;
using DataStructure.PriorityQueue;
using M2M.Algorithm.Pathfinding.Implement.AStar_Dijkstra_DataStructure;
using M2M.Algorithm.Pathfinding.Interface;
using M2M.Position.Implement;
using M2M.Position.Interface;

using Real = System.Double;
using IPosition_ConnectedSet = System.Collections.Generic.ICollection<M2M.Position.Interface.IPosition_Connected>;

namespace M2M.Algorithm.Pathfinding.Implement
{
    public class AStar : SearchPathEngine
    {
        protected IEvaluator evaluator;

        public AStar()
        {
            com = new AStarTagComparer(list);
            evaluator = new EuclidDistanceEvaluator();
        }

        public IEvaluator Evaluator
        {
            get { return evaluator; }
            set { evaluator = value; }
        }

        #region ISearchPathEngine Members

        //没有路径则返回null
        public override ICollection<IPosition_Connected> SearchPath(IPosition_Connected start, IPosition_Connected end)
        {
            List<IPosition_Connected> path = new List<IPosition_Connected>();
            if (start == end)
            {
                path.Add(start);
                return path;
            }

            //初始化
            Init(start, end);

            IPosition_Connected p = null, p_adj;
            Tag p_tag, p_adj_tag;
            Real newF, newG;

            open.clear();
            //将起点加入open表
            open.add(start);

            //当open表非空时
            while (open.getSize() > 0)
            {
                //获得下一个最近的点
                p = open.removeFirst();
                //到达终点则结束
                if (p == end)
                    break;

                p_tag = list[p.GetTagIndex()];
                p_tag.closed = true;

                IEnumerable<IAdjacency> adj_set = p.GetAdjacencyOut();
                foreach (IAdjacency adjacency in adj_set)
                {
                    p_adj = adjacency.GetPosition_Connected();
                    p_adj_tag = list[p_adj.GetTagIndex()];

                    if (p_adj_tag.timeStamp != time_stamp)
                    {
                        newG = p_tag.g + adjacency.GetDistance();
                        newF = newG + evaluator.GetDistance(p_adj, end);
                        p_adj_tag.parent = p;
                        p_adj_tag.g = newG;
                        p_adj_tag.f = newF;
                        p_adj_tag.closed = false;
                        p_adj_tag.timeStamp = time_stamp;
                        open.add(p_adj);
                    }
                    else
                    {
                        if (!p_adj_tag.closed)
                        {
                            newG = p_tag.g + adjacency.GetDistance();
                            newF = newG + evaluator.GetDistance(p_adj, end);
                            if (newF < p_adj_tag.f)
                            {
                                p_adj_tag.parent = p;
                                p_adj_tag.g = newG;
                                p_adj_tag.f = newF;
                                open.update(p_adj);
                            }
                        }
                    }
                }
            }

            GetPath(path, p, end);
            return path;
        }

        #endregion
    }
}
