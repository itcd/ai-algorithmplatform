using System;
using System.Collections.Generic;
using System.Text;
using BFS_AStarforGrid;

namespace BFS_AStar
{
    class Program
    {
        public static int compare(int i1, int i2)
        {
            if (i1 < i2)
                return -1;
            if (i1 > i2)
                return 1;
            return 0;
        }

        static void Main(string[] args)
        {
            TickTimer t = new TickTimer();
            TicksTimer tt = new TicksTimer();

            //Program program = new Program();
            ArrayMap map = new ArrayMap(5, 10);
            //ArrayMap map1 = new ArrayMap(5, 10);
            MapMark mapMark = null;
            BFS bfs = new BFS();
            AStar astar = new AStar();
            //List<MyPoint> pathList;

            int total = 0;
            int total2 = 0;

            do
            {               
                total = 0;
                total2 = 0;
                map.clear();
                map.setObstacle();

                astar.init(map);
                t.start();         
                astar.search(map);
                total += t.getElapsedTime();

                bfs.init(map);
                t.start();
                bfs.search(map);
                total2 += t.getElapsedTime();

                Console.WriteLine("AStar:\t" + total);
                mapMark = astar.getMapMark();
                if (mapMark != null)
                    mapMark.show();

                Console.WriteLine("BFS:\t" + total2);
                mapMark = bfs.getMapMark();
                if (mapMark != null)
                    mapMark.show();
            } while (Console.ReadLine() != "x");

            //链表测试
            //SortedLinkedList<int> h = new SortedLinkedList<int>();
            //Random r = new Random();
            //int temp;
            //for (int i = 0; i < 10; i++)
            //{ 
            //    temp = r.Next(100);
            //    Console.Write(temp);
            //    Console.Write("\t");
            //    h.sortedInsert(temp, compare);
            //}
            //Console.WriteLine();
            //Console.WriteLine("in the list:");
            //h.show();
            //for (int i = 0; i < 10; i++)
            //{
            //    Console.Write(h.getFirst());
            //    Console.Write(" is removeed. remain: ");
            //    h.removeFirst();
            //    Console.WriteLine(h.getCount());
            //    h.show();           
            //}

            //队列测试
            //ArrayQueue<int> q = new ArrayQueue<int>(5);
            //Console.Write("count:\t");
            //Console.Write(q.getCount());
            //Console.Write("\t");
            //Console.WriteLine(q.getSize());
            //q.show();
            //for (int i = 0; i < 12; i++)
            //{
            //    q.enqueue(i);
            //    Console.Write("count:\t");
            //    Console.Write(q.getCount());
            //    Console.Write("\t");
            //    Console.WriteLine(q.getSize());
            //    q.show();
            //}

            Console.ReadLine();
        }
    }
}
