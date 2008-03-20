using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Position_Interface;
using System.ComponentModel;
using PositionSetDrawer;

namespace PositionSetViewer
{
    /// <summary>
    /// PositionSetPainter对同一点集，同一变换进行绘制（graphics可以不同）
    /// </summary>
    public partial class PositionSetDrawerPump
    {
        public delegate void GetPoint(float x, float y);

        #region event definition

        //当获得点集里每一个点的时候触发，返回当前点的真实坐标，先发生于OnGetPoint
        event GetPoint OnGetRealCoordinate;

        //当获得点集里每一个点的时候触发，包括第一个和最后一个点！
        event GetPoint OnGetPoint;
        event GetPoint OnGetFirstPoint;
        event GetPoint OnGetMiddlePoint;
        event GetPoint OnGetLastPoint;

        #endregion

        public PositionSetDrawerPump(Layer_PositionSet layer)
        {
            this.layer = layer;
        }

        Layer_PositionSet layer;        
                
        /// <summary>
        /// 遍历positionSet并产生相应的事件（如果有新的时间请在里面添加，并在ResetPainter函数里面设为null）
        /// </summary>
        /// <param name="ps"></param>
        public void TraversePositionSetAndSpringEvent(IPositionSet ps)
        {
            ps.InitToTraverseSet();
            IPosition[] positionSet_ary = (IPosition[])ps.ToArray();

            for (int i = 0; i < positionSet_ary.Length; i++)
            {
                if (OnGetRealCoordinate != null)
                {
                    this.OnGetRealCoordinate(layer.ConvertPositionSetXToRealX(positionSet_ary[i].GetX()),
                        layer.ConvertPositionSetYToRealY(positionSet_ary[i].GetY()));
                }                

                //DrawPointX和DrawPointY是屏幕上显示的坐标
                float DrawPointX = layer.ConvertPositionSetXToScreenX(positionSet_ary[i].GetX());
                float DrawPointY = layer.ConvertPositionSetYToScreenY(positionSet_ary[i].GetY());

                if (i == 0)
                {
                    if (OnGetFirstPoint != null)
                    {
                        this.OnGetFirstPoint(DrawPointX, DrawPointY);
                    }
                }
                else if (i == positionSet_ary.Length - 1)
                {
                    if (OnGetLastPoint != null)
                    {
                        this.OnGetLastPoint(DrawPointX, DrawPointY);
                    }
                }
                else
                {
                    if (OnGetMiddlePoint != null)
                    {
                        this.OnGetMiddlePoint(DrawPointX, DrawPointY);
                    }
                }

                if (OnGetPoint != null)
                {
                    this.OnGetPoint(DrawPointX, DrawPointY);
                }
            }

            #region old implement

            //if (OnGetLastPoint != null)
            //{
            //    this.OnGetLastPoint(x, y);
            //}

            //if (ps.NextPosition())
            //{
            //    if (OnGetRealPoint != null)
            //    {
            //        this.OnGetRealPoint(ps.GetPosition().GetX(), ps.GetPosition().GetY())
            //    }

            //    x = ps.GetPosition().GetX() * scaleX + translationX;
            //    y = -(ps.GetPosition().GetY() * scaleY + translationY);

            //    if (OnGetFirstPoint != null)
            //    {
            //        this.OnGetFirstPoint(x, y);
            //    }

            //    if (OnGetPoint != null)
            //    {
            //        this.OnGetPoint(x, y);
            //    }
            //}
            //while (ps.NextPosition())
            //{
            //    if (OnGetRealPoint != null)
            //    {
            //        this.OnGetRealPoint(ps.GetPosition().GetX(), ps.GetPosition().GetY())
            //    }

            //    x = ps.GetPosition().GetX() * scaleX + translationX;
            //    y = ps.GetPosition().GetY() * scaleY + translationY;

            //    if (OnGetMiddlePoint != null)
            //    {
            //        if(ps.NextPosition())
            //        {
            //            this.OnGetMiddlePoint(x, y);
            //        }
            //    }

            //    if (OnGetPoint != null)
            //    {
            //        this.OnGetPoint(x, y);
            //    }
            //}

            //if (OnGetLastPoint != null)
            //{
            //    this.OnGetLastPoint(x, y);
            //}

            #endregion
        }

        //public void ResetPump()
        //{
        //    OnGetPoint = null;
        //    OnGetFirstPoint = null;
        //    OnGetMiddlePoint = null;
        //    OnGetLastPoint = null;
        //}

        ///// <summary>
        ///// startup event pump
        ///// </summary>
        //public void Run()
        //{
        //    TraversePositionSetAndSpringEvent(layer.PositionSet);
        //}

       
        #region add new event binding

        public void DrawPoint(PointDrawer pd)
        {
            OnGetPoint += pd.Draw;
        }
        public void DrawCoordinate(CoordinateDrawer cd)
        {
            OnGetRealCoordinate += cd.RecordRealCoordinate;
            OnGetPoint += cd.Draw;
        }
        public void DrawLineOpen(OpenLineDrawer od)
        {
            OnGetFirstPoint += od.DrawFirstPoint;
            OnGetMiddlePoint += od.Draw;
            OnGetLastPoint += od.Draw;
        }
        public void DrawLineClose(CloseLineDrawer cd)
        {
            OnGetFirstPoint += cd.DrawFirstPoint;
            OnGetMiddlePoint += cd.DrawMiddlePoint;
            OnGetLastPoint += cd.DrawLastPoint;
        }
        public void DrawSquareFrame(SquareFrameDrawer cd)
        {
            OnGetPoint += cd.Draw;
        }

        public void FillPloygon(PolygonDrawer cd)
        {
            OnGetFirstPoint += cd.MeetFirstPoint;
            OnGetMiddlePoint += cd.MeetMiddlePoint;
            OnGetLastPoint += cd.MeetLastPoint;
        }

        #endregion
    }
}
