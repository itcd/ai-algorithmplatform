using System;
using System.Collections.Generic;
using System.Text;

using Position_Interface;
using Position_Implement;
using QuickHullAlgorithm;


namespace PositionSetUtils
{
    public static class PositionSetAttribute
    {
        //计算多边形的重心
        public static IPosition GetGravityCenter(IPositionSet ps)
        {
            //double xsum = 0, ysum = 0;
            //IPosition[] pa = (IPosition[])ps.ToArray();
            //int n = pa.Length;
            //int nn = (pa[0] == pa[n - 1]) ? n : n + 1;

            //for (int i = 0; i < nn - 1; i++)
            //{
            //    double tvalue = pa[i].GetX() * pa[(i + 1) % n].GetY() - pa[i].GetY() * pa[(i + 1) % n].GetX();
            //    xsum += (pa[i].GetX() + pa[(i + 1) % n].GetX()) * tvalue;
            //    ysum += (pa[i].GetY() + pa[(i + 1) % n].GetY()) * tvalue;
            //}

            //double area = GetArea(ps);
            //double cx = xsum / (6 * area);
            //double cy = ysum / (6 * area);

            //return new Position_Point((float)cx, (float)cy);

            ps.InitToTraverseSet();
            ps.NextPosition();

            float maxX = ps.GetPosition().GetX();
            float minX = ps.GetPosition().GetX();
            float maxY = ps.GetPosition().GetY();
            float minY = ps.GetPosition().GetY();
            
            while(ps.NextPosition())
            {
                float x = ps.GetPosition().GetX();
                float y = ps.GetPosition().GetY();

                if(x > maxX)
                {
                    maxX = x;
                }
                else if(x < minX)
                {
                    minX = x;
                }

                if (y > maxY)
                {
                    maxY = y;
                }
                else if (y < minY)
                {
                    minY = y;
                }
            }

            return new Position_Point((minX + maxX) / 2f, (minY + maxY) / 2f);
        }


        //计算多边形的（有向）面积
        public static double GetArea(IPositionSet ps)
        {
            double sum = 0;
            IPosition[] pa = (IPosition[])ps.ToArray();
            int n=pa.Length;
            int nn;

            //判断是否首尾相接的点序列
            if (pa[0] != pa[n-1])
                nn = n+1;
            else
                nn = n;

            //计算叉积和
            for (int i = 0; i < nn - 1; i++)
                sum += pa[i].GetX() * pa[(i + 1) % n].GetY() - pa[i].GetY() * pa[(i + 1) % n].GetX();

            //计算面积
            return sum / 2;
        }
    }
}
