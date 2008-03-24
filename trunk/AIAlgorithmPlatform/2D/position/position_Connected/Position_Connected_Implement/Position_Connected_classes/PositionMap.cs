using System;
using System.Collections.Generic;
using System.Text;
using Position_Connected_Interface;

namespace Position_Connected_Implement
{
    /**
     * 管理地图上节点的类，给地图编辑器使用
     * 如果数组地图的元素都改为List<IPosition_Connected_Edit>类型，
     * 将浮点数的节点的横纵坐标取整之后放到对应的List当中，
     * 寻找的时候也是将横纵坐标取整，然后在对应的List中寻找。
     * 这样就支持在同一个位置有多个节点和节点坐标是浮点数的情况。
     * @author Ark
     * @date 2007-05-07
     */
    [Serializable]
    public class PositionMap
    {
        List<IPosition_Connected_Edit> list;
        List<IPosition_Connected_Edit>[,] mapAry;
        IPosition_Connected_Edit startPosition, endPosition;
        int width, height;


        public PositionMap(int mapWidth, int mapHeight)
        {
            list = new List<IPosition_Connected_Edit>();
            width = mapWidth;
            height = mapHeight;
            CreateArray();
        }

        public PositionMap(int mapWidth, int mapHeight, PositionMap pMap)
        {
            width = mapWidth;
            height = mapHeight;
            if (pMap != null)
            {
                list = pMap.GetPositionList();
                SetStartPosition(pMap.GetStartPosition());
                SetEndPosition(pMap.GetEndPosition());
            }
            else
                list = new List<IPosition_Connected_Edit>();
            RestoreArrayFromList();
        }

        public int GetWidth()
        {
            return width;
        }

        public int GetHeight()
        {
            return height;
        }

        public bool InBound(float x, float y)
        {
            return (x >= 0 && x <= width - 1 && y >= 0 && y <= height - 1);
        }

        //判断List数组的x,y位置元素是否存有节点
        public bool ExistsInPosition(int x, int y)
        {
            if (InBound(x, y))
                return mapAry[(int)x, (int)y].Count > 0;
            else
                return false;
        }

        public IPosition_Connected_Edit FindPosition(float x, float y, List<IPosition_Connected_Edit> l)
        {
            IPosition_Connected_Edit position = null;
            foreach (IPosition_Connected_Edit p in l)
            {
                if (Math.Abs(p.GetX() - x) < float.Epsilon && Math.Abs(p.GetY() - y) < float.Epsilon)
                {
                    position = p;
                    break;
                }
            }
            return position;
        }

        public IPosition_Connected_Edit FindPosition(float x, float y)
        {
            if (ExistsInPosition((int)x, (int)y))
                return FindPosition(x, y, mapAry[(int)x, (int)y]);
            else
                return null;
        }

        //是否存在坐标为x,y的节点
        public bool Exists(float x, float y)
        {
            return FindPosition(x, y) != null;
        }

        public void AddPosition(float x, float y)
        {
            if (InBound(x, y))
            {
                IPosition_Connected_Edit p = new Position_Connected_Edit(x, y);
                list.Add(p);
                mapAry[(int)x, (int)y].Add(p);
            }
        }

        public void RemovePosition(float x, float y)
        {
            IPosition_Connected_Edit p = FindPosition(x, y);
            if (p != null)
            {
                mapAry[(int)x, (int)y].Remove(p);
                list.Remove(p);

                IPositionSet_Connected_AdjacencyEdit adjSet = p.GetAdjacencyPositionSetEdit();
                adjSet.InitToTraverseSet();
                while (adjSet.NextPosition())
                {
                    IPosition_Connected_Edit adj = adjSet.GetPosition_Connected_Edit();
                    adj.GetAdjacencyPositionSetEdit().RemoveAdjacency(p);
                    adjSet.RemoveAdjacency(adj);
                }
            }
        }

        public bool IsConnected(IPosition_Connected_Edit p1, IPosition_Connected_Edit p2)
        {
            if (p1 != null && p2 != null)
            {
                IPositionSet_Connected_AdjacencyEdit adjSet = p1.GetAdjacencyPositionSetEdit();
                adjSet.InitToTraverseSet();
                while (adjSet.NextPosition())
                {
                    if (p2.Equals(adjSet.GetPosition_Connected_Edit()))
                        return true;
                }
            }
            return false;
        }

        public bool IsConnected(float x1, float y1, float x2, float y2)
        {
            IPosition_Connected_Edit p1, p2;
            p1 = FindPosition(x1, y1);
            if (p1 != null)
            {
                p2 = FindPosition(x2, y2);
                return IsConnected(p1, p2);
            }
            return false;
        }

        public void AddConnection(float x1, float y1, float x2, float y2, float distance)
        {
            IPosition_Connected_Edit p1, p2;
            p1 = FindPosition(x1, y1);
            if (p1 != null)
            {
                p2 = FindPosition(x2, y2);
                if (p2 != null)
                {
                    if (!IsConnected(p1, p2))
                        p1.GetAdjacencyPositionSetEdit().AddAdjacency(p2, distance);
                }
            }
        }

        public void AddDoubleConnection(float x1, float y1, float x2, float y2, float distance)
        {
            AddConnection(x1, y1, x2, y2, distance);
            AddConnection(x2, y2, x1, y1, distance);
        }

        public void RemoveConnection(float x1, float y1, float x2, float y2)
        {
            IPosition_Connected_Edit p1, p2;
            p1 = FindPosition(x1, y1);
            if (p1 != null)
            {
                p2 = FindPosition(x2, y2);
                if (p2 != null)
                {
                    if (IsConnected(p1, p2))
                        p1.GetAdjacencyPositionSetEdit().RemoveAdjacency(p2);
                }
            }
        }

        public void RemoveDoubleConnection(float x1, float y1, float x2, float y2)
        {
            RemoveConnection(x1, y1, x2, y2);
            RemoveConnection(x2, y2, x1, y1);
        }

        public void Clear()
        {
            startPosition = null;
            endPosition = null;
            list.Clear();
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    mapAry[i, j].Clear();
        }

        //清空数组内容再存储，以节省存储空间
        public void ClearArray()
        {
            mapAry = null;
        }

        protected void CreateArray()
        {
            mapAry = new List<IPosition_Connected_Edit>[width, height];
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    mapAry[i, j] = new List<IPosition_Connected_Edit>();
        }

        //从List恢复内容到数组，从List恢复ClearArray清除了的内容
        public void RestoreArrayFromList()
        {
            CreateArray();
            PutAllPositionsInListIntoArray();
        }

        public IPosition_Connected_Edit GetPosition(float x, float y)
        {
            return FindPosition(x, y, mapAry[(int)x, (int)y]);
        }

        public IPositionSet_Connected GetPositionSet_Connected()
        {
            return new PositionSet_Connected(list);
        }

        public IPositionSet_ConnectedEdit GetPositionSet_ConnectedEdit()
        {
            return new PositionSet_ConnectedEdit(list);
        }

        public void SetStartPosition(IPosition_Connected p)
        {
            startPosition = (IPosition_Connected_Edit)p;
        }

        public void SetEndPosition(IPosition_Connected p)
        {
            endPosition = (IPosition_Connected_Edit)p;
        }

        public void SetStartPosition(float x, float y)
        {
            IPosition_Connected_Edit p = FindPosition(x, y);
            if (p != null)
                startPosition = p;
        }

        public void SetEndPosition(float x, float y)
        {
            IPosition_Connected_Edit p = FindPosition(x, y, mapAry[(int)x, (int)y]);
            if (p != null)
                endPosition = p;
        }

        public IPosition_Connected_Edit GetStartPosition()
        {
            return startPosition;
        }

        public IPosition_Connected_Edit GetEndPosition()
        {
            return endPosition;
        }

        public void ClearStartPosition()
        {
            startPosition = null;
        }

        public void ClearEndPosition()
        {
            endPosition = null;
        }

        public void ClearAttachment()
        {
            foreach (IPosition_Connected p in list)
                p.SetAttachment(null);
        }

        public List<IPosition_Connected_Edit> GetPositionList()
        {
            return list;
        }

        public List<IPosition_Connected_Edit> GetPositionList(int x, int y)
        {
            if (Exists(x, y))
                return mapAry[x, y];
            else
                return null;
        }

        public bool PutPositionIntoArray(IPosition_Connected_Edit p)
        {
            float x = p.GetX(), y = p.GetY();
            if (InBound(x, y))
            {
                mapAry[(int)x, (int)y].Add(p);
                return true;
            }
            else
                return false;
        }

        protected void PutAllPositionsInListIntoArray()
        {
            IPosition_Connected_Edit p;
            int i = 0;
            while (i < list.Count)
            {
                p = list[i];
                if (PutPositionIntoArray(p))
                    i++;
                else
                    list.Remove(p);
            }
        }

        protected void GenerateMapFromArray(int[,] maze)
        {
            Clear();

            //根据maze数组创建地图节点
            Position_Connected_Edit p;
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                {
                    if (maze[i, j] != 0)
                    {
                        p = new Position_Connected_Edit(i, j);
                        list.Add(p);
                    }
                }

            PutAllPositionsInListIntoArray();

            int start, end;
            //根据maze数组建立节点间垂直方向的连接
            for (int i = 0; i < width; i++)
            {
                start = 0;
                while (start < height - 1)
                {
                    while (start < height && maze[i, start] == 0)
                    {
                        start++;
                    }
                    end = start;
                    if (start < height - 1)
                    {
                        while (end + 1 < height && maze[i, end + 1] != 0)
                        {
                            end++;
                        }
                        if (end < height && maze[i, end] != 0)
                        {
                            for (int k = start; k < end; k++)
                            {
                                AddDoubleConnection(i, k, i, k + 1, 1);
                            }
                        }
                    }
                    start = end + 1;
                }
            }

            //根据maze数组建立节点间水平方向的连接
            for (int i = 0; i < height; i++)
            {
                start = 0;
                while (start < width - 1)
                {
                    while (start < width && maze[start, i] == 0)
                    {
                        start++;
                    }
                    end = start;
                    if (start < width - 1)
                    {
                        while (end + 1 < width && maze[end + 1, i] != 0)
                        {
                            end++;
                        }
                        if (end < width && maze[end, i] != 0)
                        {
                            for (int k = start; k < end; k++)
                            {
                                AddDoubleConnection(k, i, k + 1, i, 1);
                            }
                        }
                    }
                    start = end + 1;
                }
            }
        }

        public void GenerateFullMap()
        {
            int[,] maze;
            int stepsCount = FullMap.generateFullMap(width, height, out maze);
            GenerateMapFromArray(maze);
        }

        public void GenerateRandomMaze()
        {
            int[,] maze;
            int stepsCount = RandomMaze_duplex.generateMaze(width, height, out maze);
            GenerateMapFromArray(maze);
        }

        public void GenerateRandomMap(int totalAmount, int amount1, int amount2, int amount3, double probability1, double probability2, double probability3)
        {
            List<IPosition_Connected_Edit> newList = RandomPositionList.generateRandomIntPositions(width, height, totalAmount, amount1, amount2, amount3, probability1, probability2, probability3);
            if (newList != null)
            {
                Clear();
                list = newList;
                PutAllPositionsInListIntoArray();
            }
        }

        public void GenerateRandomPositions(int totalAmount, int amount1, int amount2, int amount3, double probability1, double probability2, double probability3)
        {
            List<IPosition_Connected_Edit> newList = RandomPositionList.generateRandomFloatPositions(width, height, totalAmount, amount1, amount2, amount3, probability1, probability2, probability3);
            if (newList != null)
            {
                Clear();
                list = newList;
                PutAllPositionsInListIntoArray();
            }
        }
    }
}
