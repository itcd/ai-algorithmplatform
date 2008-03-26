using System;
using System.Collections.Generic;
using System.Text;
using DataStructure;
using NUnit.Framework;

namespace TestProject
{
    [TestFixture]
    public class RandomMap_tests
    {
        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public void testRandomMap()
        {
            List<IPosition_Connected_Edit> list = RandomIntPositions.generateRandomIntPositions(10, 10, 50, 2, 4, 16, 0.6, 0.3, 0.1);
            IPosition_Connected_Edit adj;
            StringBuilder sb = new StringBuilder();
            //Console.WriteLine(open.Count);
            sb.AppendLine(list.Count.ToString());
            foreach (IPosition_Connected_Edit p in list)
            {
                //Console.Write(p.ToString()+"\t");
                sb.Append(p.ToString() + "\t");
                IPositionSet_Connected_AdjacencyEdit adjSet = p.GetAdjacencyPositionSetEdit();
                adjSet.InitToTraverseSet();
                while (adjSet.NextPosition())
                {
                    adj = adjSet.GetPosition_Connected_Edit();
                    //Console.Write(adj.ToString() + ":" + p.GetDistanceToAdjacency().ToString() + "\t");
                    sb.Append(adj.ToString() + ":" + adjSet.GetDistanceToAdjacency().ToString() + "\t");
                }
                //Console.WriteLine();
                sb.AppendLine();
            }
            Console.WriteLine(sb.ToString());
        }
    }
}
