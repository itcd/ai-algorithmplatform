using System;
using System.Collections.Generic;
using System.Text;
using DataStructure;
using DataStructure.PriorityQueue;
using Position_Connected_Interface;

namespace SearchEngineLibrary
{
    public class Dijkstra_Dictionary : ISearchPathEngine
    {
        Dictionary<IPosition_Connected, Tag> dict;
        IComparer<IPosition_Connected> com;
        IPriorityQueue<IPosition_Connected> open;
        IPositionSet_Connected positionSet;

        public Dijkstra_Dictionary()
        {
            dict = new Dictionary<IPosition_Connected, Tag>();
            com = new TagComparer_Dictionary(dict);
            open = new PriorityQueue_List<IPosition_Connected>(com);
        }

        public void InitEngineForMap(IPositionSet_Connected map)
        {
            positionSet = map;
            IPosition_Connected p;
            Tag tag;
            //初始化
            dict.Clear();
            open.clear();
            positionSet.InitToTraverseSet();
            while (positionSet.NextPosition())
            {
                p = positionSet.GetPosition_Connected();
                if (dict.TryGetValue(p, out tag))
                    dict.Remove(p);
                open.add(p);
                dict.Add(p, new Tag());
            }
        }

        bool Init(IPosition_Connected start, IPosition_Connected end)
        {
            IPosition_Connected p;
            Tag tag;

            //初始化
            positionSet.InitToTraverseSet();
            while (positionSet.NextPosition())
            {
                p = positionSet.GetPosition_Connected();
                if (dict.TryGetValue(p, out tag))
                {
                    tag.Clear();
                }
                else
                {
                    dict.Add(p, new Tag());
                    open.add(p);
                }
            }

            //判断起点和终点是否在地图上，并初始化起点的标签
            if (dict.TryGetValue(start, out tag))
            {
                tag.g = 0;
                tag.parent = null;
            }
            else
                return false;
            if (!dict.TryGetValue(end, out tag))
                return false;

            //更新起点在候选队列中的位置
            open.remove(start);
            open.add(start);
            return true;
        }

        //没有路径则返回null
        public List<IPosition_Connected> SearchPath(IPosition_Connected start, IPosition_Connected end)
        {
            List<IPosition_Connected> path = new List<IPosition_Connected>();
            if (start == end)
            {
                path.Add(start);
                return path;
            }

            //初始化
            if (!Init(start, end))
                return null;

            PriorityQueue_List<IPosition_Connected> open_once = new PriorityQueue_List<IPosition_Connected>(open);
            IPosition_Connected p = null, p_adj;
            IPositionSet_Connected_Adjacency adj;
            Tag p_tag, p_adj_tag;
            float newDistance;

            //当open表非空时
            while (open_once.getSize() > 0)
            {
                p = open_once.getFirst();
                if (p == end)
                    break;
                open_once.removeFirst();
                dict.TryGetValue(p, out p_tag);
                adj = p.GetAdjacencyPositionSet();
                adj.InitToTraverseSet();
                while (adj.NextPosition())
                {
                    p_adj = adj.GetPosition_Connected();
                    dict.TryGetValue(p_adj, out p_adj_tag);
                    newDistance = p_tag.g + adj.GetDistanceToAdjacency();
                    if (p_adj_tag.g > newDistance)
                    {
                        p_adj_tag.g = newDistance;
                        p_adj_tag.parent = p;
                        open_once.remove(p_adj);
                        open_once.add(p_adj);
                    }
                }
            }

            //从终点根据标签中记录的父节点找到起点，生成路径       
            if (p == end)
            {
                path.Add(end);
                p_tag = (Tag)end.GetAttachment();
                while (p_tag.parent != null)
                {
                    path.Add(p_tag.parent);
                    p_tag = (Tag)p_tag.parent.GetAttachment();
                }
                path.Reverse();
                return path;
            }
            else
                return null;
        }
    }
}
