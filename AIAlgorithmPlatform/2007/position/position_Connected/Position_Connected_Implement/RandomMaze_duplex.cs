using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Position_Connected_Implement;

namespace Position_Connected_Implement
{
    public class RandomMaze_duplex
    {
        /**
         * 生成迷宫的函数，随机的双向深度优先搜索算法。
         * 原理为分别以起点和终点为跟生成两棵树。
         * 另外在两棵树的交界处补上一些点确保两个两棵树之间连通，两棵树之间可能有多处连通。
         * @author Ark
         * @date 2007-05-05
         */
        ///返回从迷宫左上角到迷宫右下角的步数
        public static int generateMaze(int width, int height, out int[,] maze)
        {
            const int d = 4;
            int[] dx = { 0, 0, -1, 1 };
            int[] dy = { -1, 1, 0, 0 };
            int[] count = new int[d];//记录选择各个方向的次数
            int[,] map = new int[width, height];//记录查找过的节点
            maze = new int[width, height];//记录迷宫路径节点

            Stack<Point> s1 = new Stack<Point>();
            Stack<Point> s2 = new Stack<Point>();
            Point p;

            Random random = new Random();
            int stepsCount;//从迷宫左上角到迷宫右下角的步数
            int n, r, x, y, newX, newY, count1, count2, newConnection;
            int index = 0;
            int mean = width * height / 32;

            //从起点开始正向搜索
            count1 = map[0, 0] = 1;
            s1.Push(new Point(0, 0));
            //从终点开始逆向搜索
            count2 = map[width - 1, height - 1] = -1;
            s2.Push(new Point(width - 1, height - 1));
            while (s1.Count > 0 || s2.Count > 0)
            {
                //正向搜索
                if (s1.Count > 0)
                {
                    p = s1.Pop();
                    index++;
                    newConnection = 0;
                    n = d;
                    while (n > 0)//循环d次，每次随机选择一个方向
                    {
                        r = random.Next(d);
                        if (count[r] < index)//令选择各个方向的次数等于当前的步数，即令选择各个方向的机会均等
                        {
                            count[r]++;
                            x = p.X + dx[r];
                            y = p.Y + dy[r];
                            //if (x >= 0 && x < width && y >= 0 && y < height && map[x, y] <= 0 && maze[x, y] == 0)
                            if (x >= 0 && x < width && y >= 0 && y < height)
                            {
                                if (map[x, y] == 0)
                                {
                                    stepsCount = 0;
                                    for (int i = 0; i < d; i++)
                                    {
                                        newX = x + dx[i];
                                        newY = y + dy[i];
                                        //超出边界或者和格子在对方的迷宫路径上
                                        if (newX < 0 || newX >= width || newY < 0 || newY >= height || maze[newX, newY] <= 0)
                                        {
                                            stepsCount++;
                                        }
                                    }
                                    if (stepsCount >= d - 1)
                                    {
                                        map[x, y] = map[p.X, p.Y] + 1;
                                        if (map[x, y] > count1)
                                            count1 = map[x, y];
                                        s1.Push(new Point(x, y));
                                        newConnection++;
                                    }
                                }
                            }
                            n--;
                        }
                    }
                    //判断是否有创建新的节点
                    if (newConnection > 0)
                        maze[p.X, p.Y] = map[p.X, p.Y];
                    else
                    //如果没有创建新节点，且当前节点是否在双方路径之间，则将当前节点添加为迷宫路径节点
                    {
                        stepsCount = 0;
                        r = 0;
                        for (int i = 0; i < d; i++)
                        {
                            x = p.X + dx[i];
                            y = p.Y + dy[i];
                            if (x >= 0 && x < width && y >= 0 && y < height)
                            {
                                if (maze[x, y] > 0)
                                    stepsCount++;
                                else
                                    if (maze[x, y] < 0)
                                        r++;
                            }
                        }
                        if (stepsCount == 1 && r == 1)
                        {
                            //标记新添加的连通双方路径的节点
                            maze[p.X, p.Y] = mean;
                            //maze[p.x, p.y] = map[p.x, p.y];
                        }
                    }
                }
                //逆向搜索
                if (s2.Count > 0)
                {
                    p = s2.Pop();
                    index++;
                    newConnection = 0;
                    n = d;
                    while (n > 0)
                    {
                        r = random.Next(d);
                        if (count[r] < index)
                        {
                            count[r]++;
                            x = p.X + dx[r];
                            y = p.Y + dy[r];
                            //if (x >= 0 && x < width && y >= 0 && y < height && map[x, y] >= 0 && maze[x, y] == 0)
                            if (x >= 0 && x < width && y >= 0 && y < height)
                            {
                                if (map[x, y] == 0)
                                {
                                    stepsCount = 0;
                                    for (int i = 0; i < d; i++)
                                    {
                                        newX = x + dx[i];
                                        newY = y + dy[i];
                                        //超出边界或者和格子在对方的迷宫路径上
                                        if (newX < 0 || newX >= width || newY < 0 || newY >= height || maze[newX, newY] >= 0)
                                        {
                                            stepsCount++;
                                        }
                                    }
                                    if (stepsCount >= d - 1)
                                    {
                                        map[x, y] = map[p.X, p.Y] - 1;
                                        if (map[x, y] < count2)
                                            count2 = map[x, y];
                                        s2.Push(new Point(x, y));
                                        newConnection++;
                                    }
                                }
                            }
                            n--;
                        }
                    }
                    //判断是否有创建新的节点
                    if (newConnection > 0)
                        maze[p.X, p.Y] = map[p.X, p.Y];
                    else
                    //如果没有创建新节点，且当前节点是否在双方路径之间，则将当前节点添加为迷宫路径节点
                    {
                        stepsCount = 0;
                        r = 0;
                        for (int i = 0; i < d; i++)
                        {
                            x = p.X + dx[i];
                            y = p.Y + dy[i];
                            if (x >= 0 && x < width && y >= 0 && y < height)
                            {
                                if (maze[x, y] > 0)
                                    stepsCount++;
                                else
                                    if (maze[x, y] < 0)
                                        r++;
                            }
                        }
                        if (stepsCount == 1 && r == 1)
                        {
                            //标记新添加的连通双方路径的节点
                            maze[p.X, p.Y] = -mean;
                            //maze[p.x, p.y] = map[p.x, p.y];
                        }
                    }
                }
            }

            stepsCount = count1 - count2;

            //在窗口上画出迷宫
            //bufferGraphics.Clear(Color.Black);
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    //将值为负的第二棵树的节点的值变为正，作为第一棵树的扩展节点
                    if (maze[i, j] < 0)
                        maze[i, j] += stepsCount + 1;
                }
            }
            return stepsCount;
        }
    }
}
