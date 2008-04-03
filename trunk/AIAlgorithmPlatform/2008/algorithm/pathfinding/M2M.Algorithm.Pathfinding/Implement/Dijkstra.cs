using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataStructure;
using DataStructure.PriorityQueue;
using M2M.Algorithm.Pathfinding.Implement.AStar_Dijkstra_DataStructure;
using M2M.Algorithm.Pathfinding.Interface;
using M2M.Position.Interface;

using Real = System.Double;
using IPosition_ConnectedSet = System.Collections.Generic.ICollection<M2M.Position.Interface.IPosition_Connected>;
using Position_ConnectedSet = System.Collections.Generic.List<M2M.Position.Interface.IPosition_Connected>;

namespace M2M.Algorithm.Pathfinding.Implement
{
/// <summary>
    /// Dijkstra算法
    /// </summary>
    public class Dijkstra : ISearchPathEngine
    {
        IComparer<IPosition_Connected> com;
        IPosition_ConnectedSet positionSet;
        ushort time_stamp;//每次寻径之前都生成一个新的时间戳，用于判断某个节点的信息是有效还是过期的
        float pathLength = 0;
        List<IPosition_Connected> path;
        IPriorityQueue<IPosition_Connected> open;

        public Dijkstra()
        {
            com = new DijkstraTagComparer();
            time_stamp = 0;
        }

        public Real GetPathLength()
        {
            return pathLength;
        }

        //每次寻径之前根据起点、终点信息做初始化
        bool Init(IPosition_Connected end, IPosition_Connected start)
        {
            ////判断起点和终点是否在地图上，并初始化起点的标签
            //Tag tag = (Tag)end.GetAttachment();

            //生成这次寻径使用的time stamp
            time_stamp = TimeStamp.getNextTimeStamp(time_stamp);

            //if (tag != null)
            //{
            //    tag.g = 0;
            //    tag.parent = null;
            //    tag.closed = false;
            //    tag.timeStamp = time_stamp;
            //}
            //else
            //    return false;
            //if (start.GetAttachment() == null)
            //    return false;

            return true;
        }

        #region ISearchPathEngine Members

        //初始化地图，在地图被更改之后调用
        public void InitEngineForMap(ICollection<IPosition_Connected> map)
        {
            positionSet = map;
            //IPosition_Connected p;

            ////初始化
            //positionSet.InitToTraverseSet();
            //while (positionSet.NextPosition())
            //{
            //    p = positionSet.GetPosition_Connected();
            //    p.SetAttachment(new Tag());
            //}

            int num = (int)(Math.Sqrt((double)positionSet.Count));

            path = new List<IPosition_Connected>(num * 2);

            if (num > 0)
                open = new IntervalHeap_C5<IPosition_Connected>(num * 4, com);
            else
                open = new IntervalHeap_C5<IPosition_Connected>(com);
        }

        //没有路径则返回null
        public ICollection<IPosition_Connected> SearchPath(IPosition_Connected start, IPosition_Connected end)
        {
            //path.Clear();

            //if (end == start)
            //{
            //    path.Add(end);
            //    return path;
            //}

            ////初始化
            //if (!Init(end, start))
            //    return null;

            //IPosition_Connected p = null, p_adj;
            //IAdjacency adj_set;
            //Tag p_tag, p_adj_tag;
            //float newG;

            ////int count = 0;
            ////if (debug)
            ////    Console.WriteLine("SearchPath:");

            //open.clear();

            ////将起点加入open表
            //open.add(end);

            ////当open表非空时
            //while (open.getSize() > 0)
            //{
            //    //获得下一个最近的点
            //    p = open.removeFirst();
            //    //到达终点则结束
            //    if (p == start)
            //    {
            //        pathLength = ((Tag)start.GetAttachment()).g;
            //        break;
            //    }
            //    p_tag = (Tag)p.GetAttachment();
            //    p_tag.closed = true;
            //    adj_set = p.GetAdjacencyPositionSet();
            //    adj_set.InitToTraverseSet();

            //    //if (debug)
            //    //{
            //    //    count++;
            //    //    Console.Write(p.ToString() + "\t");
            //    //}

            //    while (adj_set.NextPosition())
            //    {
            //        p_adj = adj_set.GetPosition_Connected();
            //        p_adj_tag = (Tag)p_adj.GetAttachment();

            //        //如果未被搜索
            //        if (p_adj_tag.timeStamp != time_stamp)
            //        {
            //            newG = p_tag.g + adj_set.GetDistanceToAdjacency();

            //            p_adj_tag.parent = p;
            //            p_adj_tag.g = newG;
            //            p_adj_tag.closed = false;
            //            p_adj_tag.timeStamp = time_stamp;
            //            open.add(p_adj);
            //        }
            //        else
            //        {
            //            if (!p_adj_tag.closed)
            //            {
            //                newG = p_tag.g + adj_set.GetDistanceToAdjacency();
            //                if (newG < p_adj_tag.g)
            //                {
            //                    p_adj_tag.parent = p;
            //                    p_adj_tag.g = newG;
            //                    open.update(p_adj);
            //                }
            //            }
            //        }

            //        //if (debug)
            //        //    Console.Write(p_adj.ToString() + ":" + p_adj.GetAttachment().ToString() + "\t");
            //    }
            //    //if (debug)
            //    //    Console.WriteLine();
            //}

            ////if (debug)
            ////{
            ////    Console.Write("position count:");
            ////    Console.WriteLine(count);
            ////}

            ////从终点根据标签中记录的父节点找到起点，生成路径       
            //if (p == start)
            //{
            //    path.Add(start);
            //    p_tag = (Tag)start.GetAttachment();
            //    while (p_tag.parent != null)
            //    {
            //        path.Add(p_tag.parent);
            //        p_tag = (Tag)p_tag.parent.GetAttachment();
            //    }
            //    //path.Reverse();
            //    return path;
            //}
            //else
                return null;
        }


        #endregion
    }
}
