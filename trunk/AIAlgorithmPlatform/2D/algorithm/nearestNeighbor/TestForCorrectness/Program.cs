using System;
using System.Collections.Generic;
using System.Text;
using M2M;
using KDT;
using System.Collections;
using System.Diagnostics;

using Position_Interface;
using NearestNeighbor;

namespace TestForCorrectness
{
    class Program
    {
        //static void Main()
        //{
        //    Application.EnableVisualStyles();
        //    Application.SetCompatibleTextRenderingDefault(false);
        //    Application.Run(new Form1());
        //}

        static void Main(string[] args)
        {
            //bool b = true;
            //for (int i = 0; i < 100; i++)
            //{

            //    if (!NearestNeighbourTest())
            //    {
            //        b = false;
            //        Console.WriteLine("两者结构不一样！");
            //    }

            //}
            //if (b)
            //    Console.WriteLine("两者结构一样");

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new M2M.Form1());

            NearestNeighbourTest();

            Console.Read();
        }

        static public bool NearestNeighbourTest()
        {
            int range = 4000;
            int Pnum = 100000;
            int Tnum = 1000;
            Random r = new Random();
            Stopwatch sWatch = new Stopwatch();
            List<IPosition> pl = new List<IPosition>();
            for (int i = 0; i < Pnum; i++)
            {
               // KD2DPoint pp = new KD2DPoint((float)r.NextDouble() * (range - 1), (float)r.NextDouble() * (range - 1));

                float x = (float)r.NextDouble() * r.Next((range - 1));
                KD2DPoint pp = new KD2DPoint(x, ((range - 1) - x));//(float)(200+Math.Sin(x)* 200));
               
      
                pl.Add(pp);
            }


            INearestNeighbor SearchNearestNeighborEngine = new M2M_NN();

            //SearchNearestNeighborEngine.Init(range, range, 8000, 5, 4);

            SearchNearestNeighborEngine.PreProcess(pl);

            INearestNeighbor KDTSearchNN = new KDT_NN();
            KDTSearchNN.PreProcess(pl);
            KD2DPoint expected, actual;
            long timeKD, timeMM;
            timeKD = 0;
            timeMM = 0;

            List<IPosition> testPointList = new List<IPosition>();

            for (int i = 0; i < Tnum; i++)
            {



                KD2DPoint NearestNeighbour_target2 = new KD2DPoint((float)r.NextDouble() * (range - 1), (float)r.NextDouble() * (range - 1)); // TODO: 初始化为适当的值

                testPointList.Add(NearestNeighbour_target2);

              
            }

                //Console.WriteLine("x: " + NearestNeighbour_target2.GetX().ToString() + "   y: " + NearestNeighbour_target2.GetY().ToString());

            sWatch.Reset();
            sWatch.Start();
            KD2DPoint p1, p2;
            int err = 0;
            foreach(IPosition testPoint in testPointList)
            {
               p1=(KD2DPoint)
                SearchNearestNeighborEngine.NearestNeighbor(testPoint);
           //   double b= Math.Sqrt(Math.Pow(p1.X - testPoint.GetX(), 2) + Math.Pow(p1.X - testPoint.GetX(), 2));
           //}
           //sWatch.Stop();

           //Console.WriteLine("M2M:" + sWatch.ElapsedMilliseconds);

           //sWatch.Reset();
           //sWatch.Start();
           //foreach (Position testPoint in testPointList)
           //{
               p2 = (KD2DPoint)
               KDTSearchNN.NearestNeighbor(testPoint);
               if (!p2.Equals(p1))
               {
                   Console.WriteLine("不符！");

                   float dist1 = (float)Math.Sqrt((double)((testPoint.GetX() - p1.GetX()) * (testPoint.GetX() - p1.GetX()) + (testPoint.GetY() - p1.GetY()) * (testPoint.GetY() - p1.GetY())));

                   float dist2 = (float)Math.Sqrt((double)((testPoint.GetX() - p2.GetX()) * (testPoint.GetX() - p2.GetX()) + (testPoint.GetY() - p2.GetY()) * (testPoint.GetY() - p2.GetY())));

                   Console.WriteLine("m2mnn: " + dist1.ToString());
                   Console.WriteLine("kdnn: " + dist2.ToString());

                   for (int i = 0; i < 100; i++)
                   {
                       SearchNearestNeighborEngine.NearestNeighbor(testPoint);
                       KDTSearchNN.NearestNeighbor(testPoint);
                   }

                   err++;
               }
            }
            sWatch.Stop();
            Console.WriteLine("测试点数:"+Tnum.ToString()+"；不准确数："+err.ToString());
            Console.WriteLine("KD:" + sWatch.ElapsedMilliseconds);




                //timeMM+=sWatch.ElapsedMilliseconds;
                //sWatch.Reset();
                //sWatch.Start();
                //expected = (KD2DPoint)KDTSearchNN.NearestNeighbor((Position)NearestNeighbour_target2);
                //sWatch.Stop();
                //timeKD += sWatch.ElapsedMilliseconds;
                //sWatch.Reset();
                //if (!expected.Equals(actual))
                //    Console.WriteLine("不符合！");            
            

            //Console.WriteLine("测试完毕！");
            //Console.WriteLine("kd:" + timeKD.ToString());
            //Console.WriteLine("M2M:"+timeMM.ToString());

          
            return true;
        }      
    }
}