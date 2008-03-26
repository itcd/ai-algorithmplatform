using System;
using System.Collections.Generic;
using System.Text;

namespace SearchEngineLibrary
{
    /// <summary>
    /// 生成寻径节点的Time Stamp
    /// </summary>
    [Serializable]
    public class TimeStamp
    {
        protected static ushort[] d ={ 2, 4, 6, 10, 12, 16};
        protected static Random r = new Random();

        public static ushort getTimeStamp(ushort timestamp)
        {
            return (ushort)(timestamp % ushort.MaxValue + 1);
        }

        public static ushort getRandomTimeStamp(ushort timestamp)
        {
            return (ushort)((timestamp + d[r.Next(d.Length)]) % ushort.MaxValue + 1);
        }
    }
}
