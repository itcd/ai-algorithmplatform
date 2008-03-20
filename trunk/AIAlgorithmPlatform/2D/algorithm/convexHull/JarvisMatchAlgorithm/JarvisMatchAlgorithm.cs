using System;
using System.Collections.Generic;
using System.Text;
using Position_Interface;
using ConvexHullEngine;
using Position_Implement;

namespace JarvisMatchAlgorithm
{
    public class JarvisMatch : IConvexHullEngine
    {

        public IPositionSet ConvexHull(IPositionSet ps)
        {
            List<IPosition> result = new List<IPosition>();
            List<IPosition> posList = new List<IPosition>();
            IPosition p0 = null, p1 = null, p3;
            int index = 0;

            //先转换成List
            ps.InitToTraverseSet();
            while (ps.NextPosition())
            {
                posList.Add(ps.GetPosition());
            }

            //找出第1个点(最低点)
            index = 0;
            for (int i = 1; i < posList.Count; i++)
            {
                p0 = posList[index];
                p1 = posList[i];
                if (p1.GetY() < p0.GetY() || (p1.GetY() == p0.GetY() && p1.GetX() < p0.GetX()))
                    index = i;
            }
            p0 = p1 = posList[index];
            removePosition(posList, result, index); 

            //找到第2个点
            /*
            index = 0;
            preAngle = getAngle(p1, posList[index]);
            for (int i = 1; i < posList.Count; i++)
            {
                p2 = posList[i];
                curAngle = getAngle(p1, p2);
                if (curAngle < preAngle || (curAngle == preAngle && getDistance(p1, p2) > getDistance(p1, posList[index])))
                {
                    index = i;
                    preAngle = curAngle;
                }
            }
            p2 = posList[index];
            removePosition(posList, result, index);
            */
            posList.Add(p0);    //重新加入起点，好让能构成环让算法结束
            
            //找到其他点
            while (true)
            {
                index = 0;
                //preAngle = getAngle(p2, posList[index], p1);
                if (posList.Count <= 1)
                    break;
                for (int i = 1; i < posList.Count; i++)
                {
                    p3 = posList[i];
                    //curAngle = getAngle(p2, p3, p1);
                    double dir = getDirection(p1, posList[index], p3);
                    if (dir<0 || (dir==0 && getDistance(p1, p3) > getDistance(p1, posList[index])))
                    //if (curAngle > preAngle || (curAngle == preAngle && getDistance(p2, p3) > getDistance(p2, posList[index])))
                    {
                        index = i;
                        //preAngle = curAngle;
                    }
                }
                p3 = posList[index];
                if (p3 == p0)
                    break;
                removePosition(posList, result, index);
                p1 = p3;
            }

            return new PositionSetEdit_ImplementByICollectionTemplate(result);
            
        }

        /*
        //<213
        private double getAngle(IPosition p1, IPosition p2, IPosition p3)
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
        private double getAngle(IPosition p1, IPosition p2)
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
        */
        private double getDistance(IPosition p1, IPosition p3)
        {
            return Math.Sqrt((p1.GetX() - p3.GetX()) * (p1.GetX() - p3.GetX()) + (p1.GetY() - p3.GetY()) * (p1.GetY() - p3.GetY()));
        }

        private IPosition removePosition(List<IPosition> oriList, List<IPosition> desList, int index)
        {
            IPosition p = oriList[index];
            desList.Add(p);
            oriList.RemoveAt(index);
            for (int i = 0; i < oriList.Count; i++)
            {
                IPosition p1 = oriList[i];
                if (p1.GetX() == p.GetX() && p1.GetY() == p.GetY())
                {
                    desList.Add(p1);
                    oriList.RemoveAt(i);
                    i--;
                }
            }
            return p;
        }

        private double getDirection(IPosition p0, IPosition p1, IPosition p2)
        {
            double cc = (p1.GetX() - p0.GetX()) * (p2.GetY() - p0.GetY()) - (p1.GetY() - p0.GetY()) * (p2.GetX() - p0.GetX());
            return cc;
        }
    }
}
