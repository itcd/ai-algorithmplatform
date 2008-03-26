using System;
using System.Collections.Generic;
using System.Text;

using DataStructure;
using SearchEngineLibrary;

using NUnit.Framework;

namespace TestProject
{
    [TestFixture]
    public class TimeStamp_Test
    {
        [Test]
        public void getTimeStamp_Test()
        {
            Console.WriteLine("{0}\t{1}", ushort.MinValue, ushort.MaxValue);
            ushort t = 0, i = 0;
            while (i < 99)
            {
                t = TimeStamp.getTimeStamp(t);
                if (t < 10 || t > 65526)
                {
                    Console.Write(t);
                    i++;
                    if (i % 9 == 0)
                        Console.WriteLine();
                    else
                        Console.Write("\t");
                }
                Assert.Greater(t, 0);
                Assert.LessOrEqual(t, ushort.MaxValue);
            }
        }

        [Test]
        public void getRandomTimeStamp_Test()
        {
            Console.WriteLine("{0}\t{1}", ushort.MinValue, ushort.MaxValue);
            Random r = new Random();
            ushort t = 0, i = 0, n;
            int count, total = 0;
            while (i < 99)
            {
                n = (ushort)(r.Next(ushort.MaxValue) + 1);
                t = TimeStamp.getRandomTimeStamp(t);
                count = 1;
                while (t != n)
                {
                    t = TimeStamp.getRandomTimeStamp(t);
                    count++;
                }
                Assert.AreEqual(t, n);
                Console.Write("{0},{1}\t", t, count);
                total += count;
                i++;
                if (i % 9 == 0)
                    Console.WriteLine();
                else
                    Console.Write("\t");
            }
            Console.WriteLine(total/99.0);
        }
    }
}
