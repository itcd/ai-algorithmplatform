using System;
using System.Collections.Generic;
using System.Text;

namespace M2M.Position.RandomMap
{
    public struct Point_int
    {
        public int x, y;
    };

    public struct Point3D_int
    {
        public int x, y, z;
    };

    /**
    * 生成迷宫地图的类，采用随机的深度优先搜索算法
    * PS:RandomMaze_duplex为双向随机深度优先搜索，而此算法为单向搜索，并且此算法改进了随机选择方向的方式
    * @author Ark
    * @date 2008-03-12
    */
    
    public class RandomMaze
    {
        static int[] dx = { -1, 1, 0, 0, 0, 0 };
        static int[] dy = { 0, 0, -1, 1, 0, 0 };
        static int[] dz = { 0, 0, 0, 0, -1, 1 };

        //--------2D
        static int[,] map;
        static int D2 = 4;

        //--------3D
        static int[, ,] map3D;
        static int D3 = 6;

        static int xmax = 50, ymax = 40, zmax = 30;

        static bool inBound(Point_int p)
        {
            return p.x >= 0 && p.x < xmax && p.y >= 0 && p.y < ymax;
        }

        static bool isEmpty(Point_int p)
        {
            return map[p.x, p.y] == 0;
        }

        static bool countNeighbor(Point_int p, int branch_max)
        {
            int i;
            int count = 0;
            Point_int p2;
            for (i = 0; i < D2; i++)
            {
                p2.x = p.x + dx[i];
                p2.y = p.y + dy[i];
                if (inBound(p2) && !isEmpty(p2))
                    count++;
            }
            return count < branch_max;
        }

        //branch_max表示路径的交叉点处的最多连接多少个节点，至少为2，数值越大，分支越多
        static void search(int branch_max)
        {
            int i, j;
            int[] r = new int[D2];
            Random rand = new Random();
            Stack<Point_int> s = new Stack<Point_int>();
            Point_int p, p2;
            p.x = 0;
            p.y = 0;
            s.Push(p);

            while (s.Count > 0)
            {
                p = s.Pop();
                if (isEmpty(p) && countNeighbor(p, branch_max))
                {
                    map[p.x, p.y] = 1;
                    for (j = 0; j < D2; j++)//重置随机方向数组，令到数组的值为从0至D3-1
                        r[j] = j;
                    for (i = D2; i > 0; i--)//随机方向数组的大小从D3开始递减至0
                    {
                        j = rand.Next() % i;//每次从随机方向数组取一个方向索引j (j>=0&&j<=i-1)
                        p2.x = p.x + dx[r[j]];
                        p2.y = p.y + dy[r[j]];
                        if (j != i - 1)//如果j不是随机方向数组的最后一个元素，则将最后一个元素i-1保存到位置j
                            r[j] = r[i - 1];
                        if (inBound(p2) && isEmpty(p2) && countNeighbor(p2, branch_max))
                        {
                            s.Push(p2);
                        }
                    }
                }
            }
        }

        /**
        * 生成迷宫的函数，随机的深度优先搜索算法，以起点为根生成一棵路径树(2D版本)
        * @author Ark
        * @date 2008-03-12
        */
        public static int[,] generateMaze(int width, int height)
        {
            return generateMaze(width, height, 2);
        }

        public static int[,] generateMaze(int width, int height, int branch_max)
        {
            xmax = width;
            ymax = height;
            map = new int[xmax, ymax];
            search(branch_max);
            return map;
        }

        static bool inBound3D(Point3D_int p)
        {
            return p.x >= 0 && p.x < xmax && p.y >= 0 && p.y < ymax && p.z >= 0 && p.z < zmax;
        }

        static bool isEmpty3D(Point3D_int p)
        {
            return map3D[p.x, p.y, p.z] == 0;
        }

        static bool countNeighbor3D(Point3D_int p, int branch_max)
        {
            int i;
            int count = 0;
            Point3D_int p3;
            for (i = 0; i < D3; i++)
            {
                p3.x = p.x + dx[i];
                p3.y = p.y + dy[i];
                p3.z = p.z + dz[i];
                if (inBound3D(p3) && !isEmpty3D(p3))
                    count++;
            }
            return count < branch_max;
        }

        //branch_max表示路径的交叉点处的最多连接多少个节点，至少为2，数值越大，分支越多
        static void search3D(int branch_max)
        {
            int i, j;
            int[] r = new int[D3];
            Random rand = new Random();
            Stack<Point3D_int> s = new Stack<Point3D_int>();
            Point3D_int p, p3;
            p.x = 0;
            p.y = 0;
            p.z = 0;
            s.Push(p);

            while (s.Count > 0)
            {
                p = s.Pop();
                if (isEmpty3D(p) && countNeighbor3D(p, branch_max))
                {
                    map3D[p.x, p.y, p.z] = 1;
                    for (j = 0; j < D3; j++)//重置随机方向数组，令到数组的值为从0至D3-1
                        r[j] = j;
                    for (i = D3; i > 0; i--)//随机方向数组的大小从D3开始递减至0
                    {
                        j = rand.Next() % i;//每次从随机方向数组取一个方向索引j (j>=0&&j<=i-1)
                        p3 = new Point3D_int();
                        p3.x = p.x + dx[r[j]];
                        p3.y = p.y + dy[r[j]];
                        p3.z = p.z + dz[r[j]];
                        if (j != i - 1)//如果j不是随机方向数组的最后一个元素，则将最后一个元素i-1保存到位置j
                            r[j] = r[i - 1];                       
                        if (inBound3D(p3) && isEmpty3D(p3) && countNeighbor3D(p3, branch_max))
                        {                        
                            s.Push(p3);
                        }
                    }
                }
            }
        }

        /**
        * 生成迷宫的函数，随机的深度优先搜索算法，以起点为根生成一棵路径树(3D版本)
        * @author Ark
        * @date 2008-03-12
        */
        public static int[, ,] generateMaze3D(int width, int height, int depth)
        {
            return generateMaze3D(width, height, depth, 2);
        }

        public static int[, ,] generateMaze3D(int width, int height, int depth, int branch_max)
        {
            xmax = width;
            ymax = height;
            zmax = depth;
            map3D = new int[xmax, ymax, zmax];
            search3D(branch_max);
            return map3D;
        }
    }
}
