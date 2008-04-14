using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using mydouble = System.Double;
using M2M.Position;

namespace TestProject_position
{
    /// <summary>
    /// Summary description for CountTime_Test
    /// </summary>
    [TestClass]
    public class CountTime_Test
    {
        public CountTime_Test()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        public void MyTestInitialize()
        {
            a[0] = -1;
            a[1] = 0;
            a[2] = 1;
        }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion     
        
        public int switchTest(int i)
        {
            switch(i)
            {
                case 0:
                    return -1;
                case 1:
                    return 0;
                case 2:
                    return 1;
                default:
                    return -10;
            }
        }

        int[] a = new int[3];

        public int arrayTest(int i)
        {
            //if (i >= 0 && i < a.Length)
                return a[i];
            //else
            //    return -10;
        }

        [TestMethod]
        public void TestMethod_ReadSpeed()
        {
            //
            // TODO: Add test logic	here
            //

            unchecked 
            {
                int n = 100000000;
                int sum;
                long t1, t2;

                sum = 0;
                t1 = DateTime.Now.Ticks;
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < 3; j++)
                        sum += switchTest(j);
                t2 = DateTime.Now.Ticks;
                Console.WriteLine(sum);
                Console.WriteLine(t1);
                Console.WriteLine(t2);
                Console.WriteLine(t2 - t1);

                Console.WriteLine();

                sum = 0;
                t1 = DateTime.Now.Ticks;
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < 3; j++)
                        sum += arrayTest(j);
                t2 = DateTime.Now.Ticks;
                Console.WriteLine(sum);
                Console.WriteLine(t1);
                Console.WriteLine(t2);
                Console.WriteLine(t2 - t1);
            }
        }
    }
}
