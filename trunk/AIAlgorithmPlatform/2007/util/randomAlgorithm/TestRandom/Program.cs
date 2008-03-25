using System;
using System.Collections.Generic;
using System.Text;
using Troschuetz.Random;

namespace TestRandom
{
    class Program
    {
        static void Main(string[] args)
        {
            LevyDistribution distribution = new LevyDistribution();
            distribution.Alpha = 2;
            
            //NormalDistribution distribution = new NormalDistribution();

            for (int i = 0; i < 1000; i++)
            {
                Console.WriteLine(distribution.NextDouble()); 
            }

            Console.WriteLine();
            Console.WriteLine("Mean:" + distribution.Mean.ToString());
            Console.WriteLine("Median:" + distribution.Median.ToString());
            Console.WriteLine("Maximum:" + distribution.Maximum.ToString());
            Console.WriteLine("Minimum:" + distribution.Minimum.ToString());
            Console.WriteLine("Mode:" + distribution.Mode[0].ToString());

            Console.Read();
        }
    }
}
