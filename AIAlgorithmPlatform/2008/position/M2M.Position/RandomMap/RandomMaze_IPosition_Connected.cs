using System.Collections.Generic;
using System.Linq;
using System.Text;

using M2M.Position.Implement;
using M2M.Position.Interface;
using Real = System.Double;
using IPosition_ConnectedSet = System.Collections.Generic.ICollection<M2M.Position.Interface.IPosition_Connected>;
using Position_ConnectedSet = System.Collections.Generic.List<M2M.Position.Interface.IPosition_Connected>;

namespace M2M.Position.RandomMap
{
    public class RandomMaze_IPosition_Connected
    {
        protected static void addDoubleConnection(int x1, int y1, int x2, int y2, Real d, Position3D_Connected[,] map)
        {
            IAdjacency a1 = new Adjacency(map[x1, y1], d);
            map[x2, y2].GetAdjacencyOut().Add(a1);

            IAdjacency a2 = new Adjacency(map[x2, y2], d);
            map[x1, y1].GetAdjacencyOut().Add(a2);

            map[x2, y2].GetAdjacencyIn().Add(a1);
            map[x1, y1].GetAdjacencyIn().Add(a2);
        }

        protected static void addDoubleConnection3D(int x1, int y1, int z1, int x2, int y2, int z2, Real d, Position3D_Connected[, ,] map)
        {
            IAdjacency a1 = new Adjacency(map[x1, y1, z1], d);
            map[x2, y2, z2].GetAdjacencyOut().Add(a1);

            IAdjacency a2 = new Adjacency(map[x2, y2, z2], d);
            map[x1, y1, z1].GetAdjacencyOut().Add(a2);

            map[x2, y2, z2].GetAdjacencyIn().Add(a1);
            map[x1, y1, z1].GetAdjacencyIn().Add(a2);
        }

        public static IPosition_ConnectedSet GenerateMap_from_Array(int[,] maze, int width, int height)
        {
            IPosition_ConnectedSet list = new Position_ConnectedSet();
            Position3D_Connected[,] map = new Position3D_Connected[width, height];

            //根据maze数组创建地图节点
            Position3D_Connected p;
            int i, j;

            for (i = 0; i < width; i++)
                for (j = 0; j < height; j++)
                {
                    if (maze[i, j] != 0)
                    {
                        p = new Position3D_Connected(i, j);
                        map[i, j] = p;
                        list.Add(p);
                    }
                }

            int start, end;
            //根据maze数组建立节点间垂直方向的连接
            for (i = 0; i < width; i++)
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
                            for (j = start; j < end; j++)
                            {
                                addDoubleConnection(i, j, i, j + 1, 1, map);
                            }
                        }
                    }
                    start = end + 1;
                }
            }

            //根据maze数组建立节点间水平方向的连接
            for (j = 0; j < height; j++)
            {
                start = 0;
                while (start < width - 1)
                {
                    while (start < width && maze[start, j] == 0)
                    {
                        start++;
                    }
                    end = start;
                    if (start < width - 1)
                    {
                        while (end + 1 < width && maze[end + 1, j] != 0)
                        {
                            end++;
                        }
                        if (end < width && maze[end, j] != 0)
                        {
                            for (i = start; i < end; i++)
                            {
                                addDoubleConnection(i, j, i + 1, j, 1, map);
                            }
                        }
                    }
                    start = end + 1;
                }
            }
            return list;
        }

        public static IPosition_ConnectedSet GenerateMap3D_from_Array(int[, ,] maze, int width, int height, int depth)
        {
            IPosition_ConnectedSet list = new Position_ConnectedSet();
            Position3D_Connected[, ,] map = new Position3D_Connected[width, height, depth];

            //根据maze数组创建地图节点
            Position3D_Connected p;
            int i, j, k;

            for (i = 0; i < width; i++)
                for (j = 0; j < height; j++)
                    for (k = 0; k < depth; k++)
                    {
                        if (maze[i, j, k] != 0)
                        {
                            p = new Position3D_Connected(i, j, k);
                            map[i, j, k] = p;
                            list.Add(p);
                        }
                    }

            int start, end;

            //根据maze数组建立节点间水平方向(i, width)的连接
            for (j = 0; j < height; j++)
                for (k = 0; k < depth; k++)
                {
                    start = 0;
                    while (start < width - 1)
                    {
                        while (start < width && maze[start, j, k] == 0)
                        {
                            start++;
                        }
                        end = start;
                        if (start < width - 1)
                        {
                            while (end + 1 < width && maze[end + 1, j, k] != 0)
                            {
                                end++;
                            }
                            if (end < width && maze[end, j, k] != 0)
                            {
                                for (i = start; i < end; i++)
                                {
                                    addDoubleConnection3D(i, j, k, i + 1, j, k, 1, map);
                                }
                            }
                        }
                        start = end + 1;
                    }
                }

            //根据maze数组建立节点间垂直方向(j, heigth)的连接
            for (i = 0; i < width; i++)
                for (k = 0; k < depth; k++)
                {
                    start = 0;
                    while (start < height - 1)
                    {
                        while (start < height && maze[i, start, k] == 0)
                        {
                            start++;
                        }
                        end = start;
                        if (start < height - 1)
                        {
                            while (end + 1 < height && maze[i, end + 1, k] != 0)
                            {
                                end++;
                            }
                            if (end < height && maze[i, end, k] != 0)
                            {
                                for (j = start; j < end; j++)
                                {
                                    addDoubleConnection3D(i, j, k, i, j + 1, k, 1, map);
                                }
                            }
                        }
                        start = end + 1;
                    }
                }

            //根据maze数组建立节点间纵深方向(k, depth)的连接
            for (i = 0; i < width; i++)
                for (j = 0; j < height; j++)
                {
                    start = 0;
                    while (start < depth - 1)
                    {
                        while (start < depth && maze[i, j, start] == 0)
                        {
                            start++;
                        }
                        end = start;
                        if (start < depth - 1)
                        {
                            while (end + 1 < depth && maze[i, j, end + 1] != 0)
                            {
                                end++;
                            }
                            if (end < depth && maze[i, j, end] != 0)
                            {
                                for (k = start; k < end; k++)
                                {
                                    addDoubleConnection3D(i, j, k, i, j, k + 1, 1, map);
                                }
                            }
                        }
                        start = end + 1;
                    }
                }
            return list;
        }
    }
}
