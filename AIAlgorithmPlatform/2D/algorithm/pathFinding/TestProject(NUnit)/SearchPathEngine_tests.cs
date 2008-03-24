using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using DataStructure;
using SearchEngineLibrary;
using NodesMapEditor;

using NUnit.Framework;

namespace TestProject
{
    [TestFixture]
    public class SearchPathEngine_tests
    {
        IComparer<IPosition_Connected> com = new PositionComparer();
        EditForm f = new EditForm();
        List<IPosition_Connected> start, end;
        List<List<IPosition_Connected>> path;
        List<PositionSet_Connected> set;

        [SetUp]
        public void SetUp()
        {
            start = new List<IPosition_Connected>();
            end = new List<IPosition_Connected>();
            path = new List<List<IPosition_Connected>>();
            set = new List<PositionSet_Connected>();

            loadMapAndPath(@"../../../_maps/testCase/30_20.map", @"../../../_maps/testCase/30_20.path");
            //loadMapAndPath(@"../../../_maps/testCase/60_40.map", @"../../../_maps/testCase/60_40.path");
            //loadMapAndPath(@"../../../_maps/testCase/120_80.map", @"../../../_maps/testCase/120_80.path");
            //loadMapAndPath(@"../../../_maps/testCase/240_160.map", @"../../../_maps/testCase/240_160.path");
        }

        protected void loadMapAndPath(string mapfile, string pathfile)
        {
            Console.WriteLine("map:\t" + mapfile);
            Console.WriteLine("path:\t" + pathfile);
            //Â·¾¶:11,5  9,5  7,7  5,7  3,9
            f.LoadPathFromFile(pathfile);
            path.Add(f.GetPath());
            f.LoadMapFromFile(mapfile);
            set.Add(f.GetPositionMap().GetPositionSet());
            start.Add(f.GetPath()[0]);
            end.Add(f.GetPath()[path.Count - 1]);
            Console.WriteLine("start:" + f.GetPath()[0].ToString() + "\tend:" + f.GetPath()[f.GetPath().Count - 1].ToString());
        }

        [Test]
        public void Dijkstra_Test()
        {
            ISearchPathEngine searchEngine = new Dijkstra();
            //((Dijkstra)searchEngine).Debug = true;
            performTest(searchEngine);
        }

        [Test]
        public void AStar_Test()
        {
            ISearchPathEngine searchEngine = new AStar();
            //((AStar_old)searchEngine).Debug = true;
            performTest(searchEngine);
        }

        protected void performTest(ISearchPathEngine searchEngine)
        {
            List<IPosition_Connected> newPath;
            for (int i = 0; i < path.Count; i++)
            {
                searchEngine.InitEngineForMap(set[i]);
                newPath = searchEngine.SearchPath(start[i], end[i]);
                Console.WriteLine("There are " + newPath.Count.ToString() + " nodes in the path.");
                foreach (IPosition_Connected p in newPath)
                    Console.WriteLine(p.ToString() + "\t" + p.GetAttachment().ToString());
                Assert.AreEqual(newPath.Count, path[i].Count);
                for (int j = 0; j < newPath.Count; j++)
                    Assert.AreEqual(com.Compare(newPath[j], path[i][j]), 0);            
            }
        }
    }
}
