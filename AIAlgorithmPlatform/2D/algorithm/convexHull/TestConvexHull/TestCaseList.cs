using System;
using System.Collections.Generic;
using System.Text;
using Position_Interface;
using Position_Implement;

namespace TestConvexHull
{
    class TestCaseList
    {
        List<IPositionSet> psList = new List<IPositionSet>();
        List<string> nameList = new List<string>();

        public TestCaseList()
        {
            Init();
        }

        public void AddTestCase(string name, IPositionSet ps)
        {
            nameList.Add(name);
            psList.Add(ps);
        }

        public void AddTestCase(string name, double[] data)
        {
            int n = data.Length / 2;
            List<IPosition> pl = new List<IPosition>();
            for (int i = 0; i < n; i++)
            {
                pl.Add(new Position_Point((float)(data[2 * i]), (float)(data[2 * i + 1])));
            }
            PositionSetEdit_ImplementByICollectionTemplate ps = new PositionSetEdit_ImplementByICollectionTemplate(pl);
            AddTestCase(name, ps);
        }

        public int GetCount()
        {
            return psList.Count;
        }

        public string GetName(int i)
        {
            return nameList[i];
        }

        public IPositionSet GetPositionSet(int i)
        {
            return psList[i];
        }

        public List<IPositionSet> GetPositionSetList()
        {
            return psList;
        }

        private void Init()
        {
            double[] data1 = {
                100, 100,
                200, 100,
                200, 200};
            AddTestCase("三角形", data1);

            double[] data2={
                100, 100,
                200, 100,
                100, 100,
                200, 200,
                100, 100
            };
            AddTestCase("有重点的三角形", data2);

            double[] data3 ={
                100, 100,
                150, 100,
                200, 100,
                200, 200,
            };
            AddTestCase("有三点共线", data3);
        }
    }
}
