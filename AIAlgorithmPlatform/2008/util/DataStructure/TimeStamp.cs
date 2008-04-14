using System;
using System.Collections.Generic;
using System.Text;

namespace M2M.Util.DataStructure
{
    /// <summary>
    /// 生成寻径节点的Time Stamp
    /// </summary>
    [Serializable]
    public class TimeStamp
    {
        //protected static ushort[] d ={ 2, 4, 6, 10, 12, 16};
        //protected static Random r = new Random();

        /// <summary>
        /// 根据当前TimeStamp值生成下一个TimeStamp值
        /// </summary>
        /// <param name="timestamp_now"></param>
        /// <returns></returns>
        public static ushort getNextTimeStamp(ushort timestamp_now)
        {
            return unchecked((ushort)(timestamp_now + 1));
        }

        //public static ushort getNextRandomTimeStamp(ushort timestamp)
        //{
        //    return (ushort)((timestamp + d[r.Next(d.Length)]) % ushort.MaxValue + 1);
        //}

        /// <summary>
        /// 随机生成一个TimeStamp值
        /// </summary>
        /// <returns></returns>
        public static ushort getRandomTimeStamp()
        {
            return unchecked((ushort)(DateTime.Now.Ticks));
        }
    }
}
