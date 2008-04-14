using System;
using System.Collections.Generic;
using System.Text;

namespace M2M.Util.DataStructure
{
    /// <summary>
    /// ����Ѱ���ڵ��Time Stamp
    /// </summary>
    [Serializable]
    public class TimeStamp
    {
        //protected static ushort[] d ={ 2, 4, 6, 10, 12, 16};
        //protected static Random r = new Random();

        /// <summary>
        /// ���ݵ�ǰTimeStampֵ������һ��TimeStampֵ
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
        /// �������һ��TimeStampֵ
        /// </summary>
        /// <returns></returns>
        public static ushort getRandomTimeStamp()
        {
            return unchecked((ushort)(DateTime.Now.Ticks));
        }
    }
}
