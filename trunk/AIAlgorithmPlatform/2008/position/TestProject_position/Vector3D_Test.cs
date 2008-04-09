using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject_position
{
    /// <summary>
    /// Summary description for Vector3D_Test
    /// </summary>
    [TestClass]
    public class Vector3D_Test
    {
        public Vector3D_Test()
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
        public void ProductTest()
        {
            //
            // TODO: Add test logic	here
            //
            // Calculates the angle between two Vector3Ds using the static AngleBetween method. 
            // Returns a Double.

            Vector3D vector1 = new Vector3D(20, 30, 40);
            Vector3D vector2 = new Vector3D(45, 70, 80);
            Double angleBetween;

            angleBetween = Vector3D.AngleBetween(vector1, vector2);
            // angleBetween is approximately equal to 4.15129

            Console.WriteLine(angleBetween);
            double produect = Vector3D.DotProduct(vector1, vector2);
            Console.WriteLine(produect);
            double anglecos = Vector3D.DotProduct(vector1, vector2) / vector1.Length / vector2.Length;
            Console.WriteLine(anglecos);
            double angle = Math.Acos(Vector3D.DotProduct(vector1, vector2) / vector1.Length / vector2.Length);
            Console.WriteLine(angle);
            Console.WriteLine(angle / Math.PI * 180);
        }

        static Vector3D v_up = new Vector3D(0, 1, 0);

        public void diff(Vector3D v1, Vector3D v2)
        {
            Vector3D v_diff = Vector3D.Subtract(v2, v1);
            Console.WriteLine("v1\t" + v1);
            Console.WriteLine("v2\t" + v2);
            Console.WriteLine("v_up\t" + v_up);
            Console.WriteLine("v_diff\t" + v_diff);
            Console.WriteLine("x\t" + Vector3D.CrossProduct(v_up, v_diff));
            Console.WriteLine(".\t" + Vector3D.AngleBetween(v_up, v_diff));
            Console.WriteLine();
        }

        [TestMethod]
        public void RotationTest()
        {
            diff(new Vector3D(0, 1, 0), new Vector3D(0, 0, 0));
            diff(new Vector3D(0, 0, 0), new Vector3D(0, 1, 0));
            //diff(new Vector3D(0, 0, 0), new Vector3D(1, 0, 0));
            //diff(new Vector3D(1, 0, 0), new Vector3D(0, 0, 0));
            //diff(new Vector3D(0, 0, 0), new Vector3D(1, 1, 0));
            //diff(new Vector3D(1, 1, 0), new Vector3D(0, 0, 0));
            //diff(new Vector3D(0, 0, 0), new Vector3D(1, 2, 0));
            //diff(new Vector3D(-1, -2, 0), new Vector3D(0, 0, 0));
        }
    }
}
