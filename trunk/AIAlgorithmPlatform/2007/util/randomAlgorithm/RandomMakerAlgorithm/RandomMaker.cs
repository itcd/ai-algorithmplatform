using System;
using System.Security.Cryptography;

namespace RandomMakerAlgorithm
{
    public class RandomMaker
    {
        static Random ra = new Random();

        // This method simulates a roll of the dice. The input parameter is the 
        // number of sides of the dice.
        private static int RollDice(int NumSides)
        {
            // Create a byte array to hold the random value.
            byte[] randomNumber = new byte[1];

            // Create a new instance of the RNGCryptoServiceProvider. 
            RNGCryptoServiceProvider Gen = new RNGCryptoServiceProvider();

            // Fill the array with a random value.
            Gen.GetBytes(randomNumber);

            // Convert the byte to an integer value to make the modulus operation easier.
            int rand = Convert.ToInt32(randomNumber[0]);

            // Return the random number mod the number
            // of sides.  The possible values are zero-
            // based, so we add one.
            return rand % NumSides + 1;
        }

        public static float Between(float min, float max)
        {
            //Random ra = new Random(seed);

            //Random temp = new Random(9);

            //seed += temp.Next();

            int ra = RollDice(255);

            return min + (float)ra / 255 * (max - min);

            //return min + (float)ra.NextDouble() * (max - min);
            //Random ra = new Random(unchecked((int)DateTime.Now.Ticks));
        }

        /// <summary>
        /// 快速生成min和max之间的浮点数
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float RapidBetween(float min, float max)
        {
            //Random temp = new Random(9);

            //seed += temp.Next();

            //int ra = RollDice(100);

            //return min + (float)ra / 100 * (max - min);

            return min + (float)(ra.NextDouble() * (max - min));
            //Random ra = new Random(unchecked((int)DateTime.Now.Ticks));
        }

        //static int seed = 0;

        public static int RapidBetween(int min, int max)
        {
            //seed++;
            //Random ra = new Random(seed);

            return ra.Next(min, max);
            //float ra = RollDice(255);

            //return (int)(min + ra / 255.0 * (max - min));
        }

        /// <summary>
        /// 快速生成0和1之间的浮点数
        /// </summary>
        /// <returns></returns>
        public static double RapidBetween01()
        {
            //seed++;
            //Random ra = new Random(seed);

            return ra.NextDouble();
        }

        /// <summary>
        /// 快速生成-1和1之间的浮点数
        /// </summary>
        /// <returns></returns>
        public static double RapidBetween11()
        {
            //seed++;
            //Random ra = new Random(seed);

            return (ra.NextDouble() - 0.5) * 2;
        }
    }
}