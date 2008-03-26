using System;
using System.Collections.Generic;
using System.Text;

using DataStructure;

using NUnit.Framework;

namespace TestProject
{
    [TestFixture]
    public class Dictionary_tests
    {
        Dictionary<IPosition_Connected, Tag> dict;

        [SetUp]
        public void SetUp()
        {
            dict = new Dictionary<IPosition_Connected, Tag>();
        }

        [Test]
        public void testDictionary()
        {
            float value = 3.14f;  
            IPosition_Connected p = new Position_Connected_Edit(2, 5);
            Tag l = new Tag(p, value), temp;
            dict.Add(p, l);
            dict.TryGetValue(p, out temp);
            Assert.AreEqual(l, temp);
            Console.WriteLine(l.GetHashCode());
            Console.WriteLine(temp.GetHashCode());
            temp.g *= 2;
            dict.TryGetValue(p, out temp);
            Assert.AreEqual(l, temp);
            Console.WriteLine(l.GetHashCode());
            Console.WriteLine(temp.GetHashCode());
        }
    }
}
