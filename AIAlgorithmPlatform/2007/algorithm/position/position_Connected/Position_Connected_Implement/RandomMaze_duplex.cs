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
         * �����Թ��ĺ����������˫��������������㷨��
         * ԭ��Ϊ�ֱ��������յ�Ϊ��������������
         * �������������Ľ��紦����һЩ��ȷ������������֮����ͨ��������֮������жദ��ͨ��
         * @author Ark
         * @date 2007-05-05
         */
        ///���ش��Թ����Ͻǵ��Թ����½ǵĲ���
        public static int generateMaze(int width, int height, out int[,] maze)
        {
            const int d = 4;
            int[] dx = { 0, 0, -1, 1 };
            int[] dy = { -1, 1, 0, 0 };
            int[] count = new int[d];//��¼ѡ���������Ĵ���
            int[,] map = new int[width, height];//��¼���ҹ��Ľڵ�
            maze = new int[width, height];//��¼�Թ�·���ڵ�

            Stack<Point> s1 = new Stack<Point>();
            Stack<Point> s2 = new Stack<Point>();
            Point p;

            Random random = new Random();
            int stepsCount;//���Թ����Ͻǵ��Թ����½ǵĲ���
            int n, r, x, y, newX, newY, count1, count2, newConnection;
            int index = 0;
            int mean = width * height / 32;

            //����㿪ʼ��������
            count1 = map[0, 0] = 1;
            s1.Push(new Point(0, 0));
            //���յ㿪ʼ��������
            count2 = map[width - 1, height - 1] = -1;
            s2.Push(new Point(width - 1, height - 1));
            while (s1.Count > 0 || s2.Count > 0)
            {
                //��������
                if (s1.Count > 0)
                {
                    p = s1.Pop();
                    index++;
                    newConnection = 0;
                    n = d;
                    while (n > 0)//ѭ��d�Σ�ÿ�����ѡ��һ������
                    {
                        r = random.Next(d);
                        if (count[r] < index)//��ѡ���������Ĵ������ڵ�ǰ�Ĳ���������ѡ���������Ļ������
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
                                        //�����߽���ߺ͸����ڶԷ����Թ�·����
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
                    //�ж��Ƿ��д����µĽڵ�
                    if (newConnection > 0)
                        maze[p.X, p.Y] = map[p.X, p.Y];
                    else
                    //���û�д����½ڵ㣬�ҵ�ǰ�ڵ��Ƿ���˫��·��֮�䣬�򽫵�ǰ�ڵ����Ϊ�Թ�·���ڵ�
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
                            //�������ӵ���ͨ˫��·���Ľڵ�
                            maze[p.X, p.Y] = mean;
                            //maze[p.x, p.y] = map[p.x, p.y];
                        }
                    }
                }
                //��������
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
                                        //�����߽���ߺ͸����ڶԷ����Թ�·����
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
                    //�ж��Ƿ��д����µĽڵ�
                    if (newConnection > 0)
                        maze[p.X, p.Y] = map[p.X, p.Y];
                    else
                    //���û�д����½ڵ㣬�ҵ�ǰ�ڵ��Ƿ���˫��·��֮�䣬�򽫵�ǰ�ڵ����Ϊ�Թ�·���ڵ�
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
                            //�������ӵ���ͨ˫��·���Ľڵ�
                            maze[p.X, p.Y] = -mean;
                            //maze[p.x, p.y] = map[p.x, p.y];
                        }
                    }
                }
            }

            stepsCount = count1 - count2;

            //�ڴ����ϻ����Թ�
            //bufferGraphics.Clear(Color.Black);
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    //��ֵΪ���ĵڶ������Ľڵ��ֵ��Ϊ������Ϊ��һ��������չ�ڵ�
                    if (maze[i, j] < 0)
                        maze[i, j] += stepsCount + 1;
                }
            }
            return stepsCount;
        }
    }
}
