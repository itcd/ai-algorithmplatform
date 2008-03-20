using System;
using System.Collections.Generic;
using System.Text;
using Position_Interface;
using WriteLineInGridEngine;
using Position_Implement;

namespace BlockLineAlgorithm
{
    public class BlockLine : IWriteLineInGridEngine
    {
        private const float MIN_ITV = 1;
        private static float rX, rY;
        private static float itv;

        public IPositionSet WriteLineInGrid2(float gridWidth, float gridHeight, IPosition startPosition, IPosition endPosition)
        {
            List<IPosition> line = new List<IPosition>();

            double dx = startPosition.GetX() - endPosition.GetX();
            double dy = startPosition.GetY() - endPosition.GetY();

            double initiativeStartY;
            double passivityStartX;

            double initiativeEndY;
            double passivityEndX;

            double initiativeY;
            double passivityX;

            float initiativeLength;
            float passivityLength;

            if (Math.Abs(dx) < Math.Abs(dy))
            {
                initiativeStartY = startPosition.GetY();
                passivityStartX = startPosition.GetX();

                initiativeEndY = endPosition.GetY();
                passivityEndX = endPosition.GetX();

                initiativeY = dy;
                passivityX = dx;

                initiativeLength = gridHeight;
                passivityLength = gridWidth;
            }
            else
            {
                initiativeStartY = startPosition.GetX();
                passivityStartX = startPosition.GetY();

                initiativeEndY = endPosition.GetX();
                passivityEndX = endPosition.GetY();

                initiativeY = dx;
                passivityX = dy;

                initiativeLength = gridWidth;
                passivityLength = gridHeight;
            }

            int startGridSequenceInitiative = (int)Math.Floor(initiativeStartY / initiativeLength);
            int startGridSequencePassivity = (int)Math.Floor(passivityStartX / passivityLength);

            int endGridSequenceInitiative = (int)Math.Floor(initiativeEndY / initiativeLength);
            int endGridSequencePassivity = (int)Math.Floor(passivityEndX / passivityLength);

                double e = Math.Abs(passivityX * initiativeLength / initiativeY);

                double StartIntersectionPointPassivity;

                int initiativeIncreaseNum = 0;

                if(initiativeStartY > initiativeEndY)
                {
                    StartIntersectionPointPassivity = startGridSequenceInitiative * passivityX / initiativeY;

                    initiativeIncreaseNum = -1;
                }
                else
                {
                    StartIntersectionPointPassivity = (startGridSequenceInitiative + 1) * passivityX / initiativeY;

                    initiativeIncreaseNum = 1;
                }

                double StartIntersectionPointInitiative = startGridSequenceInitiative;

                double bound = 0;

                if (passivityStartX > passivityEndX)
                {
                    double currentRemainLength = 0;

                    int currentGridSequenceInitiative = 0;
                    int currentGridSequencePassivity = 0;

                    bound = Math.Floor(passivityStartX / passivityLength) * passivityLength;
                    if (StartIntersectionPointPassivity < bound)
                    {
                        currentGridSequencePassivity = startGridSequencePassivity - 1;

                        addPositionToLine(line, dx, dy, startGridSequenceInitiative, startGridSequencePassivity - 1);

                        currentRemainLength = StartIntersectionPointPassivity - (bound - passivityLength);
                    }
                    else
                    {
                        currentGridSequencePassivity = startGridSequencePassivity;

                        currentRemainLength = StartIntersectionPointPassivity - bound;
                    }
                    
                    currentGridSequenceInitiative = startGridSequenceInitiative;                    

                    while ((currentGridSequenceInitiative != endGridSequenceInitiative) || (currentGridSequencePassivity != endGridSequencePassivity))
                    {
                        currentGridSequenceInitiative += initiativeIncreaseNum;

                        addPositionToLine(line, dx, dy, currentGridSequenceInitiative, currentGridSequencePassivity);

                        currentRemainLength -= e;

                        if (currentRemainLength < 0)
                        {
                            currentGridSequencePassivity -= 1;

                            currentRemainLength += passivityLength;

                            if ((currentGridSequenceInitiative != endGridSequenceInitiative) || (currentGridSequencePassivity != endGridSequencePassivity))
                            {
                                addPositionToLine(line, dx, dy, currentGridSequenceInitiative, currentGridSequencePassivity);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                    ///////////////////////////////////////
                else
                {
                    double currentRemainLength = 0;

                    int currentGridSequenceInitiative = 0;
                    int currentGridSequencePassivity = 0;

                    bound = Math.Floor(passivityStartX / passivityLength + 1) * passivityLength;
                    if (StartIntersectionPointPassivity > bound)
                    {
                        currentGridSequencePassivity = startGridSequencePassivity + 1;

                        addPositionToLine(line, dx, dy, startGridSequenceInitiative, startGridSequencePassivity + 1);

                        currentRemainLength = StartIntersectionPointPassivity - bound;
                    }
                    else
                    {
                        currentGridSequencePassivity = startGridSequencePassivity;

                        currentRemainLength = StartIntersectionPointPassivity - (bound - passivityLength);
                    }

                    currentGridSequenceInitiative = startGridSequenceInitiative;

                    while ((currentGridSequenceInitiative != endGridSequenceInitiative) || (currentGridSequencePassivity != endGridSequencePassivity))
                    {
                        currentGridSequenceInitiative += initiativeIncreaseNum;

                        addPositionToLine(line, dx, dy, currentGridSequenceInitiative, currentGridSequencePassivity);

                        currentRemainLength -= e;

                        if (currentRemainLength < 0)
                        {
                            currentGridSequencePassivity -= 1;

                            currentRemainLength += passivityLength;

                            if ((currentGridSequenceInitiative != endGridSequenceInitiative) || (currentGridSequencePassivity != endGridSequencePassivity))
                            {
                                addPositionToLine(line, dx, dy, currentGridSequenceInitiative, currentGridSequencePassivity);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            

            return new PositionSetEdit_ImplementByICollectionTemplate(line);
        }

        private static void addPositionToLine(List<IPosition> line, double dx, double dy, int gridSequenceInitiative, int gridSequencePassivity)
        {
            if (Math.Abs(dx) < Math.Abs(dy))
            {
                line.Add(new Position_Point(gridSequencePassivity, gridSequenceInitiative));
            }
            else
            {
                line.Add(new Position_Point(gridSequenceInitiative, gridSequencePassivity));
            }
        }

        public IPositionSet WriteLineInGrid(float gridWidth, float gridHeight, IPosition startPosition, IPosition endPosition)
        {
            Position_Point refPos = new Position_Point();
            refPos.SetX(0);
            refPos.SetY(0);
            return getBlockLine(refPos, gridWidth, startPosition, endPosition);
        }

        public static IPositionSet getBlockLine(IPosition refPos, float itv0, IPosition startPos, IPosition endPos)
        {
            rX = refPos.GetX();
            rY = refPos.GetY();
            itv = itv0;
            float sX = startPos.GetX();
            float sY = startPos.GetY();
            float eX = endPos.GetX();
            float eY = endPos.GetY();

            PositionSetEdit_ImplementByICollectionTemplate result = new PositionSetEdit_ImplementByICollectionTemplate();

            //判断起点终点是否相同
            if ((sX == eX) && (sY == eY))
            {
                return result;
            }

            //判断起点终点横坐标是否相同

            //判断起点终点纵坐标是否相同

            int xDir = (eX > sX) ? 1 : (eX == sX) ? 0 : -1;
            int yDir = (eY > sY) ? 1 : (eY == sY) ? 0 : -1;

            //最简单的情况：一条线
            if (getM(sX) == getM(eX) && getN(sY) == getN(eY))
            {
                float px = getM(sX) * itv + itv / 2 + rX;
                float py = getN(sY) * itv + itv / 2 + rY;
                result.AddPosition(new Position_Point(px, py));
            }
            else if (getM(sX) == getM(eX))
            {
                for (int i = getN(sY); (getN(eY) - i) * yDir >= 0; i += yDir)
                {
                    float px = getM(sX) * itv + itv / 2 + rX;
                    float py = getN(sY) * itv + itv / 2 + rY;
                    result.AddPosition(new Position_Point(px, py));
                }
            }
            else if (getN(sY) == getN(eY))
            {
                for (int i = getM(sX); (getM(eX) - i) * xDir >= 0; i += xDir)
                {
                    float px = getM(sX) * itv + itv / 2 + rX;
                    float py = getN(sY) * itv + itv / 2 + rY;
                    result.AddPosition(new Position_Point(px, py));
                }
            }
            //一般情况：斜线
            float x = sX;
            float y = sY;
            float k = (eY - sY) / (eX - sX);
            float nextX, nextY;

            int offsetX = 0, offsetY = 0;

            if (xDir > 0)
                nextX = (getM(sX) + 1) * itv + rX;
            else
                nextX = getM(sX) * itv + rX;

            if (yDir > 0)
                nextY = (getN(sY) + 1) * itv + rY;
            else
                nextY = getN(sY) * itv + rY;

            while ((eX - x) * xDir >= 0 || (eY - y) * yDir >= 0)
            {
                float px = (getM(x) + offsetX) * itv + itv / 2 + rX;
                float py = (getN(y) + offsetY) * itv + itv / 2 + rY;
                offsetX = offsetY = 0;

                result.AddPosition(new Position_Point(px, py));

                float dx = Math.Abs(nextX - x);
                float dy = Math.Abs(nextY - y);
                double dk = dy / dx;
                if (dk > Math.Abs(k))
                {
                    x = nextX;
                    if (xDir < 0) offsetX = -1;
                    nextX += itv * xDir;
                    y = sY + (x - sX) * k;
                }
                else if (dk < Math.Abs(k))
                {
                    y = nextY;
                    if (yDir < 0) offsetY = -1;
                    nextY += itv * yDir;
                    x = sX + (y - sY) / k;
                }
                else
                {
                    x = nextX;
                    y = nextY;
                    if (xDir < 0) offsetX = -1;
                    if (yDir < 0) offsetY = -1;
                    nextX += itv * xDir;
                    nextY += itv * yDir;
                }
            }


            IList<IPosition> poslist = result.ToIlist();
            //去掉重复点
            for (int i = 0; i < poslist.Count - 1; )
            {
                if (poslist[i].GetX() == poslist[i + 1].GetX() && poslist[i].GetY() == poslist[i + 1].GetY())
                    poslist.RemoveAt(i + 1);
                else
                    i++;
            }
            //去掉两个端点
            poslist.RemoveAt(0);
            poslist.RemoveAt(poslist.Count - 1);

            return result;
        }

        private static int getM(float x)
        {
            double k;
            if (x < rX)
                k = ((x - rX + 1) / itv) - 1;
            else
                k = ((x - rX) / itv);

            if (Math.Abs(k / Math.Round(k) - 1) < 0.00001)
                return (int)Math.Round(k);
            else
                return (int)k;
        }

        private static int getN(float y)
        {
            double k;
            if (y < rY)
                k = ((y - rY + 1) / itv) - 1;
            else
                k = ((y - rY) / itv);

            if (Math.Abs(k / Math.Round(k) - 1) < 0.00001)
                return (int)Math.Round(k);
            else
                return (int)k;
        }

        public static IPositionSet getBlockLine3(IPosition refPos, float itv0, IPosition startPos, IPosition endPos)
        {
            rX = refPos.GetX();
            rY = refPos.GetY();
            itv = itv0;
            float sX = startPos.GetX();
            float sY = startPos.GetY();
            float eX = endPos.GetX();
            float eY = endPos.GetY();

            PositionSetEdit_ImplementByICollectionTemplate result = new PositionSetEdit_ImplementByICollectionTemplate();

            if ((sX == eX) && (sY == eY))
            {
                return result;
            }

            /*
            if (getM(sX) == getM(eX) && getN(sY) == getN(eY))
            {
                x = getM(sX) * itv + itv / 2 + rX;
                y = getN(sY) * itv + itv / 2 + rY;
                result.AddPosition(new SimplePosition(x, y));
                return result;
            }

            //斜率为0或无限大的情况
            else if (xDir == 0 || yDir == 0)
            {
                eX = getM(eX) * itv + itv / 2 + rX;
                eY = getN(eY) * itv + itv / 2 + rY;
                while ((eX - x) * xDir >= 0 && (eY - y) * yDir >= 0)
                {
                    x = getM(x) * itv + itv / 2 + rX;
                    y = getN(y) * itv + itv / 2 + rY;
                    result.AddPosition(new SimplePosition(x, y));
                    x += itv * xDir;
                    y += itv * yDir;
                }
                return result;
            }
            */
            int xDir = (eX > sX) ? 1 : (eX == sX) ? 0 : -1;
            int yDir = (eY > sY) ? 1 : (eY == sY) ? 0 : -1;

            //最简单的情况：一条线
            if (getM(sX) == getM(eX) && getN(sY) == getN(eY))
            {
                float px = getM(sX) * itv + itv / 2 + rX;
                float py = getN(sY) * itv + itv / 2 + rY;
                result.AddPosition(new Position_Point(px, py));
            }
            else if (getM(sX) == getM(eX))
            {
                for (int i = getN(sY); (getN(eY) - i) * yDir >= 0; i += yDir)
                {
                    float px = getM(sX) * itv + itv / 2 + rX;
                    float py = getN(sY) * itv + itv / 2 + rY;
                    result.AddPosition(new Position_Point(px, py));
                }
            }
            else if (getN(sY) == getN(eY))
            {
                for (int i = getM(sX); (getM(eX) - i) * xDir >= 0; i += xDir)
                {
                    float px = getM(sX) * itv + itv / 2 + rX;
                    float py = getN(sY) * itv + itv / 2 + rY;
                    result.AddPosition(new Position_Point(px, py));
                }
            }
            //一般情况：斜线
            float x = sX;
            float y = sY;
            float k = (eY - sY) / (eX - sX);
            float nextX, nextY;

            int offsetX = 0, offsetY = 0;

            if (xDir > 0)
                nextX = (getM(sX) + 1) * itv + rX;
            else
                nextX = getM(sX) * itv + rX;

            if (yDir > 0)
                nextY = (getN(sY) + 1) * itv + rY;
            else
                nextY = getN(sY) * itv + rY;

            while ((eX - x) * xDir >= 0 || (eY - y) * yDir >= 0)
            {
                float px = (getM(x) + offsetX) * itv + itv / 2 + rX;
                float py = (getN(y) + offsetY) * itv + itv / 2 + rY;
                offsetX = offsetY = 0;

                result.AddPosition(new Position_Point(px, py));

                float dx = Math.Abs(nextX - x);
                float dy = Math.Abs(nextY - y);
                double dk = dy / dx;
                if (dk > Math.Abs(k))
                {
                    x = nextX;
                    if (xDir < 0) offsetX = -1;
                    nextX += itv * xDir;
                    y = sY + (x - sX) * k;
                }
                else if (dk < Math.Abs(k))
                {
                    y = nextY;
                    if (yDir < 0) offsetY = -1;
                    nextY += itv * yDir;
                    x = sX + (y - sY) / k;
                }
                else
                {
                    x = nextX;
                    y = nextY;
                    if (xDir < 0) offsetX = -1;
                    if (yDir < 0) offsetY = -1;
                    nextX += itv * xDir;
                    nextY += itv * yDir;
                }
            }

            return result;
        }

        public static IPositionSet getBlockLine2(IPosition refPos, float itv0, IPosition startPos, IPosition endPos)
        {
            rX = refPos.GetX();
            rY = refPos.GetY();
            itv = itv0;
            float sX = startPos.GetX();
            float sY = startPos.GetY();
            float eX = endPos.GetX();
            float eY = endPos.GetY();

            PositionSetEdit_ImplementByICollectionTemplate result = new PositionSetEdit_ImplementByICollectionTemplate();
            int xDir = (eX > sX) ? 1 : (eX == sX) ? 0 : -1;
            int yDir = (eY > sY) ? 1 : (eY == sY) ? 0 : -1;
            float x = sX;
            float y = sY;

            //结果只包含一个格
            if (getM(sX) == getM(eX) && getN(sY) == getN(eY))
            {
                x = getM(sX) * itv + itv / 2 + rX;
                y = getN(sY) * itv + itv / 2 + rY;
                result.AddPosition(new Position_Point(x, y));
                return result;
            }

            //斜率为0或无限大的情况
            else if (xDir == 0 || yDir == 0)
            {
                //eX = getM(eX) * itv + itv / 2 + rX;
                //eY = getN(eY) * itv + itv / 2 + rY;
                //while ((eX - x) * xDir >= 0 && (eY - y) * yDir >= 0)
                //{
                //    x = getM(x) * itv + itv / 2 + rX;
                //    y = getN(y) * itv + itv / 2 + rY;
                //    result.AddPosition(new SimplePosition(x, y));
                //    x += itv * xDir;
                //    y += itv * yDir;
                //}

                while ((eX - x) * xDir >= 0 && (eY - y) * yDir >= 0)
                {
                    result.AddPosition(new Position_Point(getM(x) * itv + itv / 2 + rX, getN(y) * itv + itv / 2 + rY));
                    if (eX % itv == 0 && xDir == 0)
                    {
                        result.AddPosition(new Position_Point(getM(x - itv / 2) * itv + itv / 2 + rX, getN(y) * itv + itv / 2 + rY));
                    }
                    if (eY % itv == 0 && yDir == 0)
                    {
                        result.AddPosition(new Position_Point(getM(x) * itv + itv / 2 + rX, getN(y - itv / 2) * itv + itv / 2 + rY));
                    }
                    x += itv * xDir;
                    y += itv * yDir;
                }
                return result;
            }

            //斜率不为0且不为无限大的情况
            float k = (eY - sY) / (eX - sX);
            //float nextX, nextY;

            //if (xDir > 0)
            //    nextX = (getM(sX) + 1) * itv + rX; 
            //else
            //    nextX = getM(sX) * itv + rX - MIN_ITV;

            //if (yDir > 0)
            //    nextY = (getN(sY) + 1) * itv + rY;
            //else
            //    nextY = getN(sY) * itv + rY - MIN_ITV;

            //while ((eX - x) * xDir >= 0 || (eY - y) * yDir >= 0) 
            //{
            //    float px = getM(x) * itv + itv / 2 + rX;
            //    float py = getN(y) * itv + itv / 2 + rY;
            //    result.AddPosition(new SimplePosition(px, py));

            //    float dx = Math.Abs(nextX - x);
            //    float dy = Math.Abs(nextY - y);
            //    double dk = dy / dx;
            //    if (dk > Math.Abs(k))
            //    {
            //        x = nextX;
            //        nextX += itv * xDir;
            //        y = sY + (x - sX) * k;
            //    }
            //    else if (dk < Math.Abs(k))
            //    {
            //        y = nextY;
            //        nextY += itv * yDir;
            //        x = sX + (y - sY) / k;
            //    }
            //    else
            //    {
            //        x = nextX;
            //        y = nextY;
            //        nextX += itv * xDir;
            //        nextY += itv * yDir;
            //    }
            //} 

            //xDir>0 yDir>0
            ///找出所有边境交点

            int sxx = getM(sX);
            int exx = getM(eX);
            int syy = getN(sY);
            int eyy = getN(eY);
            List<Position_Point> pointList = new List<Position_Point>();
            if (xDir > 0)
            {
                for (int i = 0; i + sxx < exx + 1; i++)
                {
                    float xx = (i + sxx) * itv + rX;
                    float yy = k * xx + (eY - k * eX);
                    pointList.Add(new Position_Point(xx, yy));
                }
            }
            else
            {
                for (int i = 0; i + exx < sxx + 1; i++)
                {
                    float xx = (i + exx) * itv + rX;
                    float yy = k * xx + (eY - k * eX);
                    pointList.Add(new Position_Point(xx, yy));
                }

            }



            if (yDir > 0)
            {
                for (int i = 0; i + syy < eyy + 1; i++)
                {
                    float yy = (i + syy) * itv + rY;
                    float xx = (yy - (eY - k * eX)) / k;

                    pointList.Add(new Position_Point(xx, yy));
                }
            }
            else
            {
                for (int i = 0; i + eyy < syy + 1; i++)
                {
                    float yy = (i + eyy) * itv + rY;
                    float xx = (yy - (eY - k * eX)) / k;
                    pointList.Add(new Position_Point(xx, yy));
                }

            }

            pointList.Sort(new pointXcomparer());

            for (int i = 1; i < pointList.Count; i++)
            {
                Position_Point p1 = pointList[i - 1];
                Position_Point p2 = pointList[i];
                //把两个交点的中点添加进去
                float px = getM((p1.GetX() + p2.GetX()) / 2) * itv + itv / 2 + rX;
                float py = getN((p1.GetY() + p2.GetY()) / 2) * itv + itv / 2 + rY;
                result.AddPosition(new Position_Point(px, py));

                if (i == pointList.Count - 1)
                {

                    if (eX % itv == 0)
                    {
                        px = getM(eX + (itv / 2)) * itv + itv / 2 + rX;
                        py = getN(eY) * itv + itv / 2 + rY;
                        result.AddPosition(new Position_Point(px, py));
                        px = getM(eX - (itv / 2)) * itv + itv / 2 + rX;
                        result.AddPosition(new Position_Point(px, py));
                    }
                    if (eY % itv == 0)
                    {
                        px = getM(eX) * itv + itv / 2 + rX;
                        py = getN(eY + (itv / 2)) * itv + itv / 2 + rY;
                        result.AddPosition(new Position_Point(px, py));
                        py = getN(eY - (itv / 2)) * itv + itv / 2 + rY;
                        result.AddPosition(new Position_Point(px, py));
                    }
                }
                else if (i == 0)
                {
                    if (sX % itv == 0)
                    {
                        px = getM(sX + (itv / 2)) * itv + itv / 2 + rX;
                        py = getN(sY) * itv + itv / 2 + rY; ;
                        result.AddPosition(new Position_Point(px, py));
                        px = getM(sX - (itv / 2)) * itv + itv / 2 + rX;
                        result.AddPosition(new Position_Point(px, py));
                    }
                    if (sY % itv == 0)
                    {
                        px = getM(sX) * itv + itv / 2 + rX;
                        py = getN(sY + (itv / 2)) * itv + itv / 2 + rY;
                        result.AddPosition(new Position_Point(px, py));
                        py = getN(sY - (itv / 2)) * itv + itv / 2 + rY;
                        result.AddPosition(new Position_Point(px, py));
                    }
                }

                if (p1.GetX() == p2.GetX() && p1.GetY() == p2.GetY())
                {
                    px = p1.GetX();
                    py = p2.GetY();
                    result.AddPosition(new Position_Point(px - itv / 2, py + itv / 2));
                    result.AddPosition(new Position_Point(px - itv / 2, py - itv / 2));
                    result.AddPosition(new Position_Point(px + itv / 2, py - itv / 2));
                    result.AddPosition(new Position_Point(px + itv / 2, py + itv / 2));
                }
            }

            return result;
        }

        class pointXcomparer : IComparer<Position_Point>
        {
            #region IComparer<SimplePosition> Members

            public int Compare(Position_Point x, Position_Point y)
            {
                return x.GetX().CompareTo(y.GetX());
            }

            #endregion
        }

        //private static int getM(float x)
        //{
        //    if (x < rX)
        //        return (int)((x - rX + 1) / itv) - 1;
        //    else
        //        return (int)((x - rX) / itv);
        //}

        //private static int getN(float y)
        //{
        //    if (y < rY)
        //        return (int)((y - rY + 1) / itv) - 1;
        //    else
        //        return (int)((y - rY) / itv);
        //}      
    }
}
