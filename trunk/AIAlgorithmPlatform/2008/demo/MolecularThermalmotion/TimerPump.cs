using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;
using System.Windows;

namespace MolecularThermalmotion
{
    class TimerPump
    {
        public delegate void InvokeByTimerDelegate(double time);

        InvokeByTimerDelegate invokeByTimerMethod = null;
        public InvokeByTimerDelegate InvokedMethod
        {
            set { invokeByTimerMethod = value; }
        }

        Thread thread = null;
        double timeInterval = 0;

        ~TimerPump()
        {
            if (thread != null && thread.IsAlive)
            {
                thread.Abort();
            }
        }

        void Run()
        {
            while (true)
            {
                TimeSpan sw = new TimeSpan();

                DateTime start = DateTime.Now;
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Send, invokeByTimerMethod, timeInterval);
                sw = DateTime.Now - start;

                double elapsedTime = sw.TotalMilliseconds;

                if (elapsedTime < timeInterval)
                {
                    Thread.Sleep((int)(timeInterval - elapsedTime));
                }
            }
        }

        /// <summary>
        /// BeginPumpWithTimeInterval
        /// </summary>
        /// <param name="intervalTime">Millisecond</param>
        public void BeginPumpWithTimeInterval(double intervalTime)
        {
            if (thread != null && thread.IsAlive)
            {
                thread.Abort();
            }

            this.timeInterval = intervalTime;
            thread = new Thread(new ThreadStart(Run));
            thread.IsBackground = true;
            thread.Start();
        }

        public void StopPump()
        {
            if (thread != null && thread.IsAlive)
            {
                thread.Suspend();
            }
        }

        public void Continue()
        {
            if (thread != null && thread.IsAlive)
            {
                thread.Resume();
            }
        }

        public void AbortPump()
        {
            if (thread != null && thread.IsAlive)
            {
                thread.Abort();
            }
        }
    }
}
