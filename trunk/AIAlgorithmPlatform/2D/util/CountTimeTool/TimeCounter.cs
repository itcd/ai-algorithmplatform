using System;
using System.Threading;
using System.Diagnostics;

namespace CountTimeTool
{
    /*
     * 计时器
     */

    //想办法避免嵌套调用同一个计时器所带来的问题。
    //提供对代码段的计时方式。

    public class TimeCounter
    {
        public const int ms = 1000000;
        public const int us = 10;

        private int precision = 100*ms;     //计时精度
        private int min_round = 1;          //最少重复次数

        //设置计时精度
        public void setPrecision(int value)
        {
            precision = value;
        }

        //设置最少重复次数
        public void setMinRound(int value)
        {
            min_round = value;
        }

        //要计时的事件
        public delegate void Action();
        Action actions;

        /// <summary>
        /// 快速计算一个函数的用时，要求待计算时间的函数可重复调用，且用时固定。
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public double CountTimeForRepeatableMethod(Action action)
        {
            this.actions = action;

            return ElapsedTime();
        }
        
        /// <summary>
        /// 快速计算一个函数的用时，要求待计算时间的函数可重复调用，且用时固定。并在控制台上面显示该函数所用的时间。
        /// </summary>
        /// <param name="action"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public double CountTimeAndDisplayForRepeatableMethod(Action action, string methodName)
        {
            this.actions = action;

            double temp = ElapsedTime();

            Console.WriteLine("\'" + methodName + "\'" + " ConsumeTime: " + temp.ToString() + "ms");

            return temp;
        }

        /*
         * 计时并返回结果
         */
        double GetTime()
        {
            //double d = 100, at = 0, dt = 0;
            //double t1, t2, t3;
            //while (d > 1)
            //{
            //    t1 = getTimeHelp();
            //    t2 = getTimeHelp();
            //    t3 = getTimeHelp();
            //    at = (t1 + t2 + t3) / 3.0f;
            //    dt = Math.Sqrt(((t1 - at) * (t1 - at) + (t2 - at) * (t2 - at) + (t3 - at) * (t3 - at)) / 3.0f);
            //    d = dt / at;
            //}
            //return at;
            return getTimeHelp();
        }

        double ElapsedTime2()
        {
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            int set_watchTimes = 10;

            double minTime;

            TimeSpan sw;
            DateTime start = new DateTime();
 
            start = DateTime.Now;
            actions();
            sw = DateTime.Now - start;


            minTime = sw.TotalMilliseconds;

            if (minTime == 0)
            {
            }
            else if (minTime > 50)
            {
                set_watchTimes = 0;
            }
            else
            {
                set_watchTimes = (int)(50 / minTime) - 1;
            }

            if (set_watchTimes > 10)
            {
                set_watchTimes = 10;
            }

            for (int i = 0; i < set_watchTimes; i++)
            {
                start = DateTime.Now;
                actions();
                sw = DateTime.Now - start;

                if (sw.TotalMilliseconds < minTime)
                {
                    minTime = sw.TotalMilliseconds;
                }
            }

            Thread.CurrentThread.Priority = ThreadPriority.Normal;

            return minTime;
        }

        double ElapsedTime()
        {
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            int set_watchTimes = 10;

            long minTime = long.MaxValue;

            Stopwatch sw = new Stopwatch();

            sw.Start();
            actions();
            sw.Stop();

            minTime = sw.ElapsedMilliseconds;

            if (minTime == 0)
            {
                int times = 100;

                sw.Reset();

                sw.Start();
                for (int i = 0; i < times; i++)
                {
                    actions();
                }
                sw.Stop();

                Thread.CurrentThread.Priority = ThreadPriority.Normal;

                return (double)sw.ElapsedMilliseconds / times;
            }
            else if (minTime < 5)
            {
                int times = (int)(100 / minTime);

                sw.Reset();

                sw.Start();
                for (int i = 0; i < times; i++)
                {
                    actions();
                }
                sw.Stop();

                Thread.CurrentThread.Priority = ThreadPriority.Normal;

                return (double)sw.ElapsedMilliseconds / times;
            }
            if (minTime > 50)
            {
                set_watchTimes = 0;
            }
            else
            {
                set_watchTimes = (int)(50 / minTime) - 1;
            }

            if (set_watchTimes > 10)
            {
                set_watchTimes = 10;
            }

            for (int i = 0; i < set_watchTimes; i++)
            {
                sw.Reset();

                sw.Start();
                actions();
                sw.Stop();

                if (sw.ElapsedMilliseconds < minTime)
                {
                    minTime = sw.ElapsedMilliseconds;
                }
            }

            Thread.CurrentThread.Priority = ThreadPriority.Normal;

            return minTime;
        }

        double ElapsedTime3()
        {
            int set_watchTimes = 10;

            long minTime = long.MaxValue;

            Stopwatch sw = new Stopwatch();

            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            sw.Start();
            actions();
            sw.Stop();
            Thread.CurrentThread.Priority = ThreadPriority.Lowest;

            minTime = sw.ElapsedTicks;

            if (minTime == 0)
            {
            }
            else if (minTime > 50 * ms)
            {
                set_watchTimes = 0;
            }
            else
            {
                set_watchTimes = (int)(50 * ms / minTime) - 1;
            }

            if (set_watchTimes > 5)
            {
                set_watchTimes = 5;
            }

            for (int i = 0; i < set_watchTimes; i++)
            {
                sw.Reset();

                Thread.CurrentThread.Priority = ThreadPriority.Highest;
                sw.Start();
                actions();
                sw.Stop();
                Thread.CurrentThread.Priority = ThreadPriority.Lowest;

                if (sw.ElapsedTicks < minTime)
                {
                    minTime = sw.ElapsedTicks;
                }
            }

            Thread.CurrentThread.Priority = ThreadPriority.Normal;

            return minTime / (double)ms;
        }

        double getTimeHelp()
        {
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            long startTick;     //开始时间
            long interval=0;    //经历时间
            long n=0;           //执行次数

            
            //执行第一次
            startTick = DateTime.Now.Ticks;
            interval = perform(min_round) - startTick;
            n = min_round;

            //根据需要再执行若干次
            while (interval < precision)
            {
                long add_n = 0;
                if (interval == 0)
                    add_n = 10;
                else
                    add_n = precision / interval;
                n += add_n;
                interval = perform(add_n) - startTick;

                //if (interval == 0)
                //{
                //    n = n * 10;
                //}
                //else
                //{
                //    n = (int)(n * Math.Round(((double)precision / (double)interval)) + n / 5 + 1);
                //}

                //startTick = DateTime.Now.Ticks;
                //interval = perform(n) - startTick;
            }

            double result = interval / n / us;

            
            /*
            if (result < 50)
            {
                n = 1000000/result;
                startTick = DateTime.Now.Ticks;
                result = (perform(n) - startTick)/n/us;
            }
            */
            Thread.CurrentThread.Priority = ThreadPriority.Lowest;

            return result*us/ms;
        }


        /*
         * 执行指定次数
         */
        private long perform(long n)
        {
            for (int i = 0; i < n; i++)
                actions();
            long ticks = DateTime.Now.Ticks;
            return ticks;
        }

    }
}
