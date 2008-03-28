using System;
using System.Collections.Generic;
using System.Text;
using DataStructure.PriorityQueue;
using Position_Interface;
using Position_Connected_Interface;
using DataStructure;
using SearchEngineLibrary;

namespace M2M
{
    /// <summary>
    /// 用于比较两个包含Tag的IPosition_Connected对象
    /// </summary>
    //[Serializable]
    //public class M2M_Part_TagEndComparer : IComparer<IPosition_Connected>
    //{
    //    public int Compare(IPosition_Connected p1, IPosition_Connected p2)
    //    {
    //        Tag_M2M_Part t1 = (Tag_M2M_Part)p1.GetAttachment(), t2 = (Tag_M2M_Part)p2.GetAttachment();
    //        if (t1 != null && t2 != null)
    //        {
    //            if (t1.ge < t2.ge)
    //                return -1;
    //            if (t1.ge > t2.ge)
    //                return 1;
    //        }
    //        return 0;
    //    }
    //}
    
    [Serializable]
    public class M2M_Part_TagComparer : IComparer<IPosition_Connected>
    {
        //public bool isEnd = true;

        public int state = 0;        

        public int Compare(IPosition_Connected p1, IPosition_Connected p2)
        {
            if (state == 0)
            {
                Tag_M2M_Part t1 = (Tag_M2M_Part)p1.GetAttachment(), t2 = (Tag_M2M_Part)p2.GetAttachment();
                if (t1 != null && t2 != null)
                {
                    if (t1.ge < t2.ge)
                        return -1;
                    if (t1.ge > t2.ge)
                        return 1;
                }
                return 0;
            }
            else if(state == 1)
            {
                Tag_M2M_Part t1 = (Tag_M2M_Part)p1.GetAttachment(), t2 = (Tag_M2M_Part)p2.GetAttachment();
                if (t1 != null && t2 != null)
                {
                    if (t1.gs < t2.gs)
                        return -1;
                    if (t1.gs > t2.gs)
                        return 1;
                }
                return 0;
            }
            else if (state == 2)
            {
                Tag_M2M_Position t1 = (Tag_M2M_Position)p1.GetAttachment(), t2 = (Tag_M2M_Position)p2.GetAttachment();
                if (t1 != null && t2 != null)
                {
                    if (t1.g < t2.g)
                        return -1;
                    if (t1.g > t2.g)
                        return 1;
                }
                return 0;
            }
            else
            {
                return 0;
            }
        }
    }
    
    public class M2M_PF : ISearchPathEngine
    {
        #region code for algorithm demo
        public delegate void dGetTimeStamp(ushort timeStamp);
        public event dGetM2MStructure GetM2MStructure;
        public event dGetM2MStructure GetM2MStructureInPreprocess;
        public event dGetTimeStamp GetTimeStamp;
        #endregion

        IComparer<IPosition_Connected> com;
        IPositionSet_Connected mapPositionSet_Connected;
        ushort time_stamp;//每次寻径之前都生成一个新的时间戳，用于判断某个节点的信息是有效还是过期的

        IM2MStructure m2mStructure;

        float pathLength = 0;

        public float GetPathLength()
        {
            return pathLength;
        }

        private static float CalculatePathLengthBound(int levelNum, int levelSequence, float nearestPathLengthInCurLevel)
        {
            //float interval = 0.1f / (float)(levelNum - 1);
            //return nearestPathLengthInCurLevel * (1.12f - levelSequence * interval);
            //return nearestPathLengthInCurLevel * (1.0002f + (levelNum - levelSequence - 1) * 0.001f) + 1.5f;
            //return nearestPathLengthInCurLevel * 1.02f;

            //双向连通图
            //return nearestPathLengthInCurLevel * (1.002f + (levelNum - levelSequence - 1) * 0.1f) + 1f;

            return nearestPathLengthInCurLevel * (1f + (levelNum - levelSequence - 1) * 0f) + (levelNum - levelSequence - 1) * 0.2f + 0.4f;

            //
            //return nearestPathLengthInCurLevel * (1.002f + (levelNum - levelSequence - 1) * 0.01f) + 2f;
        }

        public M2M_PF()
        {
            com = new M2M_Part_TagComparer();
            time_stamp = 0;
        }

        //初始化地图，在地图被更改之后调用
        public void InitEngineForMap(IPositionSet_Connected mapPositionSet_Connected)
        {
            this.mapPositionSet_Connected = mapPositionSet_Connected;

            M2MSCreater_ForGeneralM2MStruture m2m_Creater_ForGeneralM2MStruture = new M2MSCreater_ForGeneralM2MStruture();
            m2m_Creater_ForGeneralM2MStruture.PartType = typeof(Part_Multi);
            m2m_Creater_ForGeneralM2MStruture.SetPointInPartFactor(50);
            m2m_Creater_ForGeneralM2MStruture.SetUnitNumInGridLength(3);
            m2mStructure = m2m_Creater_ForGeneralM2MStruture.CreateAutomatically(mapPositionSet_Connected);
            m2mStructure.Preprocessing(mapPositionSet_Connected);
            BuildPartSetConnectionForM2MStructure buildPartSetConnectionForM2MStructure = new BuildPartSetConnectionForM2MStructure();
            buildPartSetConnectionForM2MStructure.TraversalEveryLevelAndBuild(m2mStructure);

            #region code for algorithm demo
            if (GetM2MStructureInPreprocess != null)
            {
                GetM2MStructureInPreprocess(m2mStructure);
            }
            #endregion

            int num = (int)(Math.Sqrt((double)mapPositionSet_Connected.GetNum()));

            path = new List<IPosition_Connected>(num * 2);

            if (num > 0)
                open = new PriorityQueue<IPosition_Connected>(num * 4, com);
            else
                open = new PriorityQueue<IPosition_Connected>(com);
        }

        //每次寻径之前根据起点、终点信息做初始化
        bool Init(IPosition_Connected start, IPosition_Connected end)
        {
            //判断起点和终点是否在地图上，并初始化起点的标签
            Tag tag = (Tag)start.GetAttachment();

            ////生成这次寻径使用的time stamp
            //time_stamp = TimeStamp.getNextTimeStamp(time_stamp);

            if (tag != null)
            {
                tag.g = 0;
                tag.parent = null;
                tag.closed = false;
                tag.timeStamp = time_stamp;
            }
            else
                return false;
            if (end.GetAttachment() == null)
                return false;

            return true;
        }

        List<IPosition_Connected> path;
        IPriorityQueue<IPosition_Connected> open;

        //没有路径则返回null
        public List<IPosition_Connected> SearchPath(IPosition_Connected start, IPosition_Connected end)
        {
            IPart_Connected startParentPart = ((Tag_M2M_Position)(start.GetAttachment())).parentPart;
            IPart_Connected endParentPart = ((Tag_M2M_Position)(end.GetAttachment())).parentPart;

            ushort lastLevelTimeStamp = TimeStamp.getRandomTimeStamp();
            ushort endTime_stamp = TimeStamp.getRandomTimeStamp();
            ushort startTime_stamp = TimeStamp.getRandomTimeStamp();

            //从最上层的下一层开始进行搜索空间收缩
            for (int levelSequence = 1; levelSequence < m2mStructure.GetLevelNum(); levelSequence++)
            {
                IPart_Connected currentLevelAncestorPartOfStartPosition = startParentPart;
                IPart_Connected currentLevelAncestorPartOfEndPosition = endParentPart;

                for (int i = 0; i < m2mStructure.GetLevelNum() - levelSequence - 1; i++)
                {
                    currentLevelAncestorPartOfStartPosition = (IPart_Connected)currentLevelAncestorPartOfStartPosition.GetParentPart();
                    currentLevelAncestorPartOfEndPosition = (IPart_Connected)currentLevelAncestorPartOfEndPosition.GetParentPart();
                }

                Tag_M2M_Part StartPartTag = (Tag_M2M_Part)currentLevelAncestorPartOfStartPosition.GetAttachment();
                StartPartTag.isNeedToSearch = true;

                Tag_M2M_Part EndPartTag = (Tag_M2M_Part)currentLevelAncestorPartOfEndPosition.GetAttachment();
                EndPartTag.isNeedToSearch = true;

                bool isTopLevel = false;
                if (levelSequence == 1)
                {
                    isTopLevel = true;
                }

                //在该层中标记需要搜索的分块
                {
                    //归入搜索空间的路径的最长长度
                    float PathLengthBound = float.MaxValue;

                    endTime_stamp = TimeStamp.getNextTimeStamp(startTime_stamp);

                    EndPartTag.timeStamp = endTime_stamp;
                    EndPartTag.ge = 0;

                    open.clear();
                    ((M2M_Part_TagComparer)com).state = 0;

                    //将起点加入open表
                    open.add(currentLevelAncestorPartOfEndPosition);

                    //当open表非空时
                    while (open.getSize() > 0)
                    {
                        //获得下一个最近的点
                        IPart_Connected currentPosition_Connected = (IPart_Connected)open.removeFirst();
                        Tag_M2M_Part TagOfCurrentPart_Connected = (Tag_M2M_Part)currentPosition_Connected.GetAttachment();

                        //当open表里面所有的点的g值都超出路径最大长度范围，则结束循环
                        if (TagOfCurrentPart_Connected.ge >= PathLengthBound)
                        {
                            break;
                        }

                        //到达终点则记录路径的长度，并计算出搜索范围
                        if (currentPosition_Connected == currentLevelAncestorPartOfStartPosition)
                        {
                            if (PathLengthBound == float.MaxValue)
                            {
                                PathLengthBound = CalculatePathLengthBound(m2mStructure.GetLevelNum(), levelSequence, TagOfCurrentPart_Connected.ge);
                            }
                        }

                        TagOfCurrentPart_Connected.isClose = true;

                        IPositionSet_Connected_Adjacency CurrentPartAdjSet = currentPosition_Connected.GetAdjacencyPositionSet();
                        CurrentPartAdjSet.InitToTraverseSet();
                        while (CurrentPartAdjSet.NextPosition())
                        {
                            IPart_Connected adjPosition = (IPart_Connected)CurrentPartAdjSet.GetPosition_Connected();
                            Tag_M2M_Part tagOfAdjPosition = (Tag_M2M_Part)adjPosition.GetAttachment();

                            bool isInSearchingBound = false;

                            if(isTopLevel)
                            {
                                isInSearchingBound = true;
                            }
                            else
                            {
                                Tag_M2M_Part parenPartTag = (Tag_M2M_Part)adjPosition.GetParentPart().GetAttachment();
                                if((parenPartTag.timeStamp == lastLevelTimeStamp) && (parenPartTag.isNeedToSearch == true))
                                {
                                    isInSearchingBound = true;
                                }
                            }

                            //如果属于搜索空间
                            if (isInSearchingBound)
                            {
                                //如果未被搜索
                                if (tagOfAdjPosition.timeStamp != endTime_stamp)
                                {
                                    float newG = TagOfCurrentPart_Connected.ge + CurrentPartAdjSet.GetDistanceToAdjacency();

                                    if (newG < PathLengthBound)
                                    {
                                        tagOfAdjPosition.ge = newG;
                                        tagOfAdjPosition.isClose = false;
                                        tagOfAdjPosition.timeStamp = endTime_stamp;
                                        open.add(adjPosition);
                                    }
                                }
                                else
                                {
                                    if (!tagOfAdjPosition.isClose)
                                    {
                                        float newG = TagOfCurrentPart_Connected.ge + CurrentPartAdjSet.GetDistanceToAdjacency();
                                        if (newG < tagOfAdjPosition.ge)
                                        {
                                            tagOfAdjPosition.ge = newG;
                                            open.update(adjPosition);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    startTime_stamp = TimeStamp.getNextTimeStamp(endTime_stamp);
                                        
                    StartPartTag.timeStamp = startTime_stamp;
                    StartPartTag.gs = 0;

                    open.clear();
                    ((M2M_Part_TagComparer)com).state = 1;

                    //将起点加入open表
                    open.add(currentLevelAncestorPartOfStartPosition);

                    //当open表非空时
                    while (open.getSize() > 0)
                    {
                        //获得下一个最近的点
                        IPart_Connected currentPosition_Connected = (IPart_Connected)open.removeFirst();
                        Tag_M2M_Part TagOfCurrentPart_Connected = (Tag_M2M_Part)currentPosition_Connected.GetAttachment();

                        //当open表里面所有的点的g值都超出路径最大长度范围，则结束循环
                        if (TagOfCurrentPart_Connected.gs >= PathLengthBound)
                        {
                            break;
                        }

                        TagOfCurrentPart_Connected.isClose = true;

                        IPositionSet_Connected_Adjacency CurrentPartAdjSet = currentPosition_Connected.GetAdjacencyPositionSet();
                        CurrentPartAdjSet.InitToTraverseSet();
                        while (CurrentPartAdjSet.NextPosition())
                        {
                            IPart_Connected adjPosition = (IPart_Connected)CurrentPartAdjSet.GetPosition_Connected();
                            Tag_M2M_Part tagOfAdjPosition = (Tag_M2M_Part)adjPosition.GetAttachment();
                                                       
                            bool isInSearchingBound = false;
                            if (isTopLevel)
                            {
                                isInSearchingBound = true;
                            }
                            else
                            {
                                Tag_M2M_Part parenPartTag = (Tag_M2M_Part)adjPosition.GetParentPart().GetAttachment();
                                
                                if ((parenPartTag.timeStamp == lastLevelTimeStamp) && (parenPartTag.isNeedToSearch == true))
                                {
                                    isInSearchingBound = true;
                                }
                            }

                            //如果属于搜索空间
                            if (isInSearchingBound)
                            {
                                //如果未被搜索
                                if (tagOfAdjPosition.timeStamp != startTime_stamp)
                                {
                                    float newG = TagOfCurrentPart_Connected.gs + CurrentPartAdjSet.GetDistanceToAdjacency();

                                    if (newG < PathLengthBound)
                                    {
                                        tagOfAdjPosition.gs = newG;
                                        tagOfAdjPosition.isClose = false;

                                        //如果在上面的搜索过程中被搜索过
                                        if (tagOfAdjPosition.timeStamp == endTime_stamp)
                                        {
                                            //如果两次搜索的g值之和小于路径的最大长度则标记该分块为下一层的待搜索空间
                                            if (tagOfAdjPosition.ge + newG < PathLengthBound)
                                            {
                                                tagOfAdjPosition.isNeedToSearch = true;
                                            }
                                            else
                                            {
                                                tagOfAdjPosition.isNeedToSearch = false;
                                            }
                                        }
                                        else
                                        {
                                            //如果在上面的搜索过程中没有被搜索过，则把它的ge值设大，使得以后改变g值的时候不会因为两个g值之和小于PathLengthBound而把该点标记为待搜索点
                                            tagOfAdjPosition.ge = PathLengthBound + 1;
                                            tagOfAdjPosition.isNeedToSearch = false;
                                        }

                                        tagOfAdjPosition.timeStamp = startTime_stamp;

                                        open.add(adjPosition);
                                    }
                                }
                                else
                                {
                                    if (!tagOfAdjPosition.isClose)
                                    {
                                        float newG = TagOfCurrentPart_Connected.gs + CurrentPartAdjSet.GetDistanceToAdjacency();
                                        if (newG < tagOfAdjPosition.gs)
                                        {
                                            if (tagOfAdjPosition.isNeedToSearch == false)
                                            {
                                                //如果两次搜索的g值之和小于路径的最大长度则标记该分块为下一层的待搜索空间
                                                if (tagOfAdjPosition.ge + newG < PathLengthBound)
                                                {
                                                    tagOfAdjPosition.isNeedToSearch = true;
                                                }
                                            }

                                            tagOfAdjPosition.gs = newG;
                                            open.update(adjPosition);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    lastLevelTimeStamp = startTime_stamp;
                    if(GetTimeStamp != null)
                    {
                        GetTimeStamp(lastLevelTimeStamp);
                    }
                }
            }

            if(GetM2MStructure != null)
            {
                GetM2MStructure(m2mStructure);
            }

            //最底层寻找真实路径

            path.Clear();
            ((M2M_Part_TagComparer)com).state = 2;

            if (end == start)
            {
                path.Add(end);
                return path;
            }

            //判断起点和终点是否在地图上，并初始化起点的标签
            Tag_M2M_Position tag = (Tag_M2M_Position)end.GetAttachment();

            //生成这次寻径使用的time stamp
            time_stamp = TimeStamp.getNextTimeStamp(lastLevelTimeStamp);

            if (tag != null)
            {
                tag.g = 0;
                tag.parent = null;
                tag.isClose = false;
                tag.timeStamp = time_stamp;
            }

            IPosition_Connected p = null, p_adj;
            IPositionSet_Connected_Adjacency adj_set;
            Tag_M2M_Position p_tag, p_adj_tag;

            open.clear();

            //将起点加入open表
            open.add(end);

            //当open表非空时
            while (open.getSize() > 0)
            {                
                float newG;

                //获得下一个最近的点
                p = open.removeFirst();
                //到达终点则结束
                if (p == start)
                {
                    pathLength = ((Tag_M2M_Position)start.GetAttachment()).g;
                    break;
                }
                p_tag = (Tag_M2M_Position)p.GetAttachment();
                p_tag.isClose = true;
                adj_set = p.GetAdjacencyPositionSet();
                adj_set.InitToTraverseSet();

                while (adj_set.NextPosition())
                {
                    p_adj = adj_set.GetPosition_Connected();
                    p_adj_tag = (Tag_M2M_Position)p_adj.GetAttachment();

                    //如果需要搜索
                    Tag_M2M_Part parentPartTag = (Tag_M2M_Part)p_adj_tag.parentPart.GetAttachment();
                    if (parentPartTag.isNeedToSearch && parentPartTag.timeStamp == lastLevelTimeStamp)
                    {
                        //如果未被搜索
                        if (p_adj_tag.timeStamp != time_stamp)
                        {
                            newG = p_tag.g + adj_set.GetDistanceToAdjacency();

                            p_adj_tag.parent = p;
                            p_adj_tag.g = newG;
                            p_adj_tag.isClose = false;
                            p_adj_tag.timeStamp = time_stamp;
                            open.add(p_adj);
                        }
                        else
                        {
                            if (!p_adj_tag.isClose)
                            {
                                newG = p_tag.g + adj_set.GetDistanceToAdjacency();
                                if (newG < p_adj_tag.g)
                                {
                                    p_adj_tag.parent = p;
                                    p_adj_tag.g = newG;
                                    open.update(p_adj);
                                }
                            }
                        }
                    }
                }
            }



            //从终点根据标签中记录的父节点找到起点，生成路径       
            if (p == start)
            {
                path.Add(start);
                p_tag = (Tag_M2M_Position)start.GetAttachment();
                while (p_tag.parent != null)
                {
                    path.Add(p_tag.parent);
                    p_tag = (Tag_M2M_Position)p_tag.parent.GetAttachment();
                }
                //path.Reverse();
                return path;
            }
            else
                return null;
        }
    }
}
