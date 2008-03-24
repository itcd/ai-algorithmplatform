using System;
using System.Collections.Generic;
using System.Text;
using Position_Interface;
using ConvexHullEngine;
using Position_Implement;

namespace QuickHullAlgorithm
{
    public class QuickHull : IConvexHullEngine
    {
        //求散点集的凸包
        public IPositionSet ConvexHull(IPositionSet ps)
        {
            IPosition p1 = null;
            IPosition p2 = null;
            PositionSetEdit_ImplementByICollectionTemplate rest=new PositionSetEdit_ImplementByICollectionTemplate();

            //找到最低点
            /*
            p1 = pts[0];
            for (int i = 1; i < pts.Length; i++)
            {
                if (pts[i].Y < p1.Y || ( pts[i].Y == p1.Y && pts[i].X < p1.X) )
                {
                    rest.Add(p1);
                    p1 = pts[i];
                }
                else
                    rest.Add(pts[i]);
            }
             * */
            ps.InitToTraverseSet();
            if (ps.NextPosition())
            {
                p1 = ps.GetPosition();
            }
            while (ps.NextPosition())
            {
                p2 = ps.GetPosition();
                if (p2.GetY() < p1.GetY() || (p2.GetY() == p1.GetY() && p2.GetX() < p1.GetX()))
                {
                    rest.AddPosition(p1);
                    p1 = p2;
                }
                else
                    rest.AddPosition(p2);
            }            

            rest.InitToTraverseSet();

            //找到与最低点成角最小的点
            /*
            int p2_i = 0;
            double p2_angle = getAngle(rest[0], p1);
            for (int i = 1; i < rest.Count; i++)
            {
                double angle_t = getAngle(rest[i], p1);
                if (angle_t < p2_angle || (angle_t == p2_angle && getDistance(p1, rest[i]) > getDistance(p1, rest[p2_i])) )
                {
                    p2_i = i;
                    p2_angle = angle_t;
                }
            }
            p2 = rest[p2_i];
            rest.RemoveAt(p2_i);
             * */
            ps = rest;
            rest = new PositionSetEdit_ImplementByICollectionTemplate();
            ps.InitToTraverseSet();
            if (ps.NextPosition())
            {
                p2 = ps.GetPosition();
            }
            
            double p2_angle = getAngle(p2, p1);
            while (ps.NextPosition())
            {
                IPosition pt = ps.GetPosition();
                double angle_t = getAngle(pt, p1);
                if (angle_t < p2_angle || (angle_t == p2_angle && getDistance(p1, pt) > getDistance(p1, p2)))
                {
                    rest.AddPosition(p2);
                    p2 = pt;
                    p2_angle = angle_t;
                }
                else
                    rest.AddPosition(pt);
            }

            //求解剩余点
            IPositionSet subResult = solveHelp(p1, p2, rest);

            //组合成凸包
            PositionSetEdit_ImplementByICollectionTemplate result = new PositionSetEdit_ImplementByICollectionTemplate();
            result.AddPosition(p1);
            /*
            foreach (Point p in subResult)
                result.Add(p);
             * */
            subResult.InitToTraverseSet();
            while (subResult.NextPosition())
                result.AddPosition( subResult.GetPosition() );
            result.AddPosition(p2);

            return result;
        }

        //求散点子集的凸包
        private IPositionSet solveHelp(IPosition p1, IPosition p2, IPositionSet ps)
        {

            //求离p1，p2最远的点
            ps.InitToTraverseSet();
            if (!ps.NextPosition())
                return ps;
            IPosition p3 = ps.GetPosition();
            double p3_h = getH(p1, p2, p3);
            PositionSetEdit_ImplementByICollectionTemplate rest = new PositionSetEdit_ImplementByICollectionTemplate();
            while (ps.NextPosition())
            {
                IPosition pt = ps.GetPosition();
                double pt_h = getH(p1, p2, pt);
                if (pt_h > p3_h)
                {
                    rest.AddPosition(p3);
                    p3 = pt;
                    p3_h = pt_h;
                }
                else
                    rest.AddPosition(pt);
            }

            //把散点分成几部分
            PositionSetEdit_ImplementByICollectionTemplate leftpart = new PositionSetEdit_ImplementByICollectionTemplate();
            PositionSetEdit_ImplementByICollectionTemplate rightpart = new PositionSetEdit_ImplementByICollectionTemplate();
            PositionSetEdit_ImplementByICollectionTemplate p1set = new PositionSetEdit_ImplementByICollectionTemplate();
            PositionSetEdit_ImplementByICollectionTemplate p2set = new PositionSetEdit_ImplementByICollectionTemplate();

            ps = rest;
            IPosition p11 = new Position_Point(2 * p2.GetX() - p1.GetX(), 2 * p2.GetY() - p1.GetY());  //p1关于p2的对称点
            double leftangle = getAngle(p1, p2, p3);
            double rightangle = getAngle(p2, p11, p3);

            ps.InitToTraverseSet();
            while (ps.NextPosition())
            {
                IPosition p = ps.GetPosition();
                if (p.GetX() == p1.GetX() && p.GetY() == p1.GetY())
                    p1set.AddPosition(p);
                else if (p.GetX() == p2.GetX() && p.GetY() == p2.GetY())
                    p2set.AddPosition(p);
                else if (p.GetX() == p3.GetX() && p.GetY() == p3.GetY())
                    leftpart.AddPosition(p);
                else 
                {
                    double a = getAngle(p1, p2, p);
                    if (a > leftangle)
                        leftpart.AddPosition(p);
                    a = getAngle(p2, p11, p);
                    if (a < rightangle)
                        rightpart.AddPosition(p);
                }
            }

            //对子集求解
            IPositionSet subResult1 = solveHelp(p1, p3, leftpart);
            IPositionSet subResult2 = solveHelp(p3, p2, rightpart);

            //合并各部分解
            PositionSetEdit_ImplementByICollectionTemplate result = new PositionSetEdit_ImplementByICollectionTemplate();

            p1set.InitToTraverseSet();
            while (p1set.NextPosition())
                result.AddPosition(p1set.GetPosition());

            subResult1.InitToTraverseSet();
            while (subResult1.NextPosition())
                result.AddPosition(subResult1.GetPosition());

            result.AddPosition(p3);

            subResult2.InitToTraverseSet();
            while (subResult2.NextPosition())
                result.AddPosition(subResult2.GetPosition());

            p2set.InitToTraverseSet();
            while (p2set.NextPosition())
                result.AddPosition(p2set.GetPosition());

            return result;
        }

        //求点（p3）到直线（p1-p2）的距离
        private static double getH(IPosition p1, IPosition p2, IPosition p3)
        {
            double a = getAngle(p1, p2, p3);
            double d = getDistance(p1, p3);
            return d * Math.Sin(a);
        }

        private static double getDistance(IPosition p1, IPosition p3)
        {
            return Math.Sqrt((p1.GetX() - p3.GetX()) * (p1.GetX() - p3.GetX()) + (p1.GetY() - p3.GetY()) * (p1.GetY() - p3.GetY()));
        }

        //求角<p3p1p2
        private static double getAngle(IPosition p1, IPosition p2, IPosition p3)
        {
            //if ((p2.X - p1.X) * (p3.Y - p1.Y) + (p3.X - p1.X) * (p2.Y - p1.Y) == 0)
            //    return Math.PI;
            //else
            //{
                double a = getAngle(p3, p1) - getAngle(p2, p1);
                if (a < 0)
                    a += Math.PI;
                else if (   //区分0度和180度
                    a == 0 &&
                    ((p3.GetX() - p1.GetX()) * (p3.GetX() - p2.GetX()) > 0 || (p3.GetY() - p1.GetY()) * (p3.GetY() - p2.GetY()) > 0)
                    )
                    a = Math.PI;
                return a;
            //}

        }
        private static double getAngle(IPosition p1, IPosition p2)
        {
            if (p1.GetX() == p2.GetX())
                return Math.PI / 2; //判断是否垂直x轴
            else
            {
                double k = ((double)p2.GetY() - (double)p1.GetY()) / ((double)p2.GetX() - (double)p1.GetX());
                double a = Math.Atan(k);
                if (a < 0)
                    a += Math.PI;
                return a;
            }
        }

        /*
        #region ConvexHullEngine Members

        public SimplePositionSet ConvexHull(SimplePositionSet ps)
        {
            int n = ps.GetCount();
            Point[] pts = new Point[n];
            for (int i = 0; i < n; i++)
            {
                SimplePosition pos = ps.GetNextPosition();
                pts[i] = new Point(pos.GetX(), pos.GetY());
            }
            Point[] resPts = solve(pts);
            SimplePositionSet result = new SimplePositionSet();
            for (int i = 0; i < resPts.Length; i++)
                result.AddPosition(new SimplePosition(resPts[i].X, resPts[i].Y));
            return result;
        }

        #endregion
         * */
    }
}
