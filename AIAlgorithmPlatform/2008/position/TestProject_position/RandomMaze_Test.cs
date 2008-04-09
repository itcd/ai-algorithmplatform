using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using M2M.Position.RandomMap;
using Configuration;

namespace TestProject_position
{
    /// <summary>
    /// Summary description for RandomMaze_Test
    /// </summary>
    [TestClass]
    public class RandomMaze_Test
    {
        public RandomMaze_Test()
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

        /// <summary>
        /// 生成随机迷宫的测试
        /// </summary>
        [TestMethod]
        public void TestMethod_RandomMaze()
        {
            //
            // TODO: Add test logic	here
            //
            RandomMaze_Config config = new RandomMaze_Config();
            new ConfiguratedByForm(config);
            config.Produce();
        }

        /// <summary>
        /// 测试：struct的复制是值复制，一个struct变量加到容器里之后，改变这个struct变量的值，对容器里的struct变量没有影响
        /// </summary>
        [TestMethod]
        public void TestMethod2()
        {
            List<Point3D_int> l = new List<Point3D_int>();
            Point3D_int p;
            p.x = p.y = p.z = 1;
            l.Add(p);
            p.x++;
            p.z++;
            l.Add(p);
            foreach (Point3D_int t in l)
            {
                Console.WriteLine("{0}\t{1}\t{2}", t.x, t.y, t.z);
            }

            Console.WriteLine();

            Point_int p2, tt;
            p2.x = p2.y = 0;
            Stack<Point_int> s = new Stack<Point_int>();
            s.Push(p2);
            p2.y++;
            s.Push(p2);
            while (s.Count > 0)
            { 
                tt = s.Pop();
                Console.WriteLine("{0}\t{1}", tt.x, tt.y);
            }
        }
    }
}
