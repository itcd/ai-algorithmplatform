﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using M2M.Util.DataStructure.PriorityQueue;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject_position
{
    /// <summary>
    /// Summary description for HeapTest
    /// </summary>
    [TestClass]
    public class HeapTest
    {
        public HeapTest()
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
        public void TestMethod_HeapTest()
        {
            //
            // TODO: Add test logic	here
            //
            Random r = new Random();
            IPriorityQueue<int> q = new IntervalHeap_C5<int>();
            //DataStructure.PriorityQueue.IPriorityQueue q = new DataStructure.PriorityQueue.IntervalHeap_C5<int>();
            for (int i = 0; i < 10; i++)
                q.add(r.Next(short.MaxValue));

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(q.removeFirst());
            }
        }
    }
}
