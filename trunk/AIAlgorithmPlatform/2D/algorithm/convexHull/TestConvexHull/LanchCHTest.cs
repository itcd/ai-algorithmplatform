using System;
using System.Collections.Generic;
using System.Text;

using ConvexHullEngine;
using GrahamScanAlgorithm;
using QuickHullAlgorithm;
using JarvisMatchAlgorithm;
using Position_Interface;
using PositionSetViewer;

namespace TestConvexHull
{
    class LanchCHTest
    {
        static CHTestAndReport report;

        private static void PrintPosSet(IPositionSet ps)
        {
            ps.InitToTraverseSet();
            while (ps.NextPosition())
            {
                IPosition p = ps.GetPosition();
                System.Console.Out.WriteLine(p.GetX() + "," + p.GetY() + ",");
            }
        }

        public static void lanch(IConvexHullEngine chEngine, IPositionSet ps)
        {
            PainterDialog painterDialog = new PainterDialog();

            //求解凸包并作图
            IPositionSet cps = chEngine.ConvexHull(ps);
            painterDialog.FillPolygon = true;
            painterDialog.Clear();
            painterDialog.DrawPositionSet(ps);
            painterDialog.DrawConvexHull(cps);
            painterDialog.Show();

            //测试凸包正确性
            report = new CHTestAndReport();
            ConvexHullCompare CHCompare = new ConvexHullCompare(ps, cps, (new QuickHull()).ConvexHull(ps));
            CHCompare.Compare();
            report.content = CHCompare.GetReport();
            report.Show();
        }

        public static void lanch(IConvexHullEngine chEngine, List<IPositionSet> psList)
        {
            ConvexHullCompare CHCompare;
            report = new CHTestAndReport();
            bool allPass = true;

            for (int i = 0; i < psList.Count; i++)
            {
                IPositionSet ps = psList[i];
                IPositionSet cps = chEngine.ConvexHull(ps);
                IPositionSet cps_ref = (new QuickHull()).ConvexHull(ps);

                CHCompare = new ConvexHullCompare(ps, cps, cps_ref);
                if (!CHCompare.Compare())
                    allPass = false;
                report.content += CHCompare.GetReport();
            }

            if (allPass)
                report.content += "测试全部通过！";
            report.Show();
        }
    }
}
