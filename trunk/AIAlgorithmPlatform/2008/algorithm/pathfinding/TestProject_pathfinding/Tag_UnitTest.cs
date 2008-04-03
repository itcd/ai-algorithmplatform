using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using M2M.Algorithm.Pathfinding.Implement.AStar_Dijkstra_DataStructure;
using M2M.Algorithm.Pathfinding.Interface;
using M2M.Position.Implement;
using M2M.Position.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using M2M.Algorithm.Pathfinding;

namespace TestProject_pathfinding
{
    /// <summary>
    /// Summary description for Tag_UnitTest
    /// </summary>
    [TestClass]
    public class Tag_UnitTest
    {
        public Tag_UnitTest()
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
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestMethod1()
        {
            //
            // TODO: Add test logic	here
            //
            unchecked
            {
                int n = 1000000000;
                //float sum;
                long t1, t2;

                TagClass tc = new TagClass();

                //sum = 0;
                t1 = DateTime.Now.Ticks;
                for (int i = 0; i < n; i++)
                {
                    tc.t.closed = !tc.t.closed;
                    //sum += tc.t.f;
                }

                t2 = DateTime.Now.Ticks;
                //Console.WriteLine(sum);
                Console.WriteLine(t1);
                Console.WriteLine(t2);
                Console.WriteLine(t2 - t1);

                Console.WriteLine();

                //sum = 0;
                t1 = DateTime.Now.Ticks;
                for (int i = 0; i < n; i++)
                {
                    ((Tag) tc.it).closed = !((Tag) tc.it).closed;
                    //sum += ((Tag)tc.it).f;
                }
                t2 = DateTime.Now.Ticks;
                //Console.WriteLine(sum);
                Console.WriteLine(t1);
                Console.WriteLine(t2);
                Console.WriteLine(t2 - t1);

                Console.WriteLine();

                //sum = 0;
                t1 = DateTime.Now.Ticks;
                for (int i = 0; i < n; i++)
                {
                    ((Tag)tc.o).closed = !((Tag)tc.o).closed;
                    //sum += ((Tag)tc.o).f;
                }
                t2 = DateTime.Now.Ticks;
                //Console.WriteLine(sum);
                Console.WriteLine(t1);
                Console.WriteLine(t2);
                Console.WriteLine(t2 - t1);
            }
        }

        //[TestMethod]
        //public void TestMethod2()
        //{
        //    IPosition_Connected_Tag p_tag = new Position3D_Connected_Tag();
        //    Tag t = new Tag();
        //    t.f = 10;
        //    p_tag.Tag = t;
        //    Tag t_new = p_tag.Tag;
        //    Console.WriteLine(t_new.f);
        //}
    }
}
