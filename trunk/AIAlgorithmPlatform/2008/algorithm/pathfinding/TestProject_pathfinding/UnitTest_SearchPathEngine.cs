using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Configuration;
using M2M.Media3D;
using M2M.Position.RandomMap;
using M2M.Algorithm.Pathfinding;
using M2M.Position;
using IPosition_ConnectedSet = System.Collections.Generic.ICollection<M2M.Position.IPosition_Connected>;

namespace TestProject_pathfinding
{
    /// <summary>
    /// Summary description for UnitTest_SearchPathEngine
    /// </summary>
    [TestClass]
    public class UnitTest_SearchPathEngine
    {
        public UnitTest_SearchPathEngine()
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
            RandomMaze_IPosition_Connected_Config config = new RandomMaze_IPosition_Connected_Config();
            config.Width = 3;
            config.Height = 3;
            config.Depth = 3;
            IPosition_ConnectedSet set = config.Produce();

            IPosition_Connected a = null, b = null;
            foreach (IPosition_Connected p in set)
            {
                if(a == null)
                    a = p;
                b = p;
            }

            AStar search = new AStar();
            search.InitEngineForMap(set);
            IPosition_ConnectedSet path = search.SearchPath(a, b);
            if (path != null)
                foreach (IPosition_Connected p in path)
                    Console.WriteLine("{0},{1}", p.GetValue(0), p.GetValue(1));
        }
    }
}
