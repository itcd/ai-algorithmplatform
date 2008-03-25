using System;
using System.Collections.Generic;
using System.Text;

namespace TestForTime
{
    class Program
    {
        static string[,] ary_s = new string[10000, 10000];

        public static String castObjectToString(Object o)
        {
            //double temp = 12321312.0 / 213244234.0;
            //double a = 12321312.0;
            //double b = 213244234.0;

            //temp = a / b;
            //temp = a / b;
            //temp = a / b;
            //temp = a / b;
            //temp = a / b;
            //temp = a / b;
            //temp = a / b;
            //temp = a / b;
            //temp = a / b;
            //temp = a / b;

            string s = (string)o;

            //temp = ary_s[100, 100];
            //temp = ary_s[23, 434];
            //temp = ary_s[234, 34];
            //temp = ary_s[234, 100];
            //temp = ary_s[333, 555];

            return null;
        }

        public static String objectString(Object o)
        {
            //double temp = 12321312.0 / 213244234.0;
            //double a = 12321312.0;
            //double b = 213244234.0;

            

            //string temp;
            return null;
        }

        static void Main(string[] args)
        {            
            long loop = 1000;
            int step = 30000;
            long startTime;
            long endTime;
            long sumCastTime = 0;
            long sumToStringTime = 0;

            System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.Highest;
           
            for (int i = 0; i < loop; i++)
            {
                String s = i.ToString();
                startTime = System.DateTime.Now.Ticks;
                for (int j = 0; j < step; j++)
                {
                    castObjectToString(s);
                }
                endTime = System.DateTime.Now.Ticks;
                sumCastTime = sumCastTime + (endTime - startTime);
                                
                startTime = System.DateTime.Now.Ticks;
                for (int j = 0; j < step; j++)
                {
                    objectString(s);
                }
                endTime = System.DateTime.Now.Ticks;
                sumToStringTime = sumToStringTime + (endTime - startTime);
            }

            Console.WriteLine(sumCastTime);
            Console.WriteLine(sumToStringTime);
            Console.WriteLine(sumCastTime - sumToStringTime);
            Console.Read();

            //.println("Cast time(ms): "+sumCastTime);
            //System.out.println("toString() time(ms): "+sumToStringTime);
        }
    }
}
