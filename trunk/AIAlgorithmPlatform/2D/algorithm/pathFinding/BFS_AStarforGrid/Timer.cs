using System;
using System.Collections.Generic;
using System.Text;

namespace BFS_AStarforGrid
{
    //计时器
    public class TickTimer
    {
        int startTime;
        int endTime;

        public void start()
        {
            startTime = Environment.TickCount;
        }

        public int getElapsedTime()
        {
            endTime = Environment.TickCount;
            return unchecked(endTime - startTime);
        }

        public int time()
        {
            int now = Environment.TickCount;
            //运行算法多次
            int then = Environment.TickCount;
            int elapsed = unchecked(then - now);
            return elapsed;
        }
    }

    public class TicksTimer
    {
        long startTime;
        long endTime;
        DateTime dt = new DateTime();

        public void start()
        {
            startTime = dt.Ticks;
        }

        public long getElapsedTime()
        {
            endTime = dt.Ticks;
            return unchecked(endTime - startTime);
        }
    }
}
