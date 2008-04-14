using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Position_Interface;
using PositionSetDrawer;
using System.ComponentModel;
using M2M;
using DrawingLib;
using System.Reflection;

namespace PositionSetViewer
{
    public abstract class Layer_PositionSet : Layer
    {
        private bool editAble;
        [BrowsableAttribute(false)]
        public bool EditAble
        {
            get { return editAble; }
            set { editAble = value; }
        }

        protected IPositionSet positionSet;
        [BrowsableAttribute(false)]
        public IPositionSet PositionSet
        {
            get { return positionSet; }
            set { positionSet = value; SpringLayerMaxRectChangedEvent(); }
        }

        protected Layer_PositionSet()
        {
        }

        public Layer_PositionSet(IPositionSet ptSet)
        {
            positionSet = ptSet;
        }

        bool positionSetIsChanged = true;

        [BrowsableAttribute(false)]
        public override RectangleF MaxRect
        {
            get
            {
                if (positionSetIsChanged)
                {
                    maxRect = GetMaxRect();
                    positionSetIsChanged = false;
                }
                return maxRect;
            }
        }

        protected RectangleF GetMaxRect()
        {
            RectangleF rect = new RectangleF(0f, 0f, 0f, 0f);

            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;

            positionSet.InitToTraverseSet();
            while (positionSet.NextPosition())
            {
                IPosition point = positionSet.GetPosition();

                float x = point.GetX() * this.PositionSetScaleX + this.PositionSetTranslationX;
                if (minX > x)
                {
                    minX = x;
                }
                else if (maxX < x)
                {
                    maxX = x;
                }

                float y = point.GetY() * this.PositionSetScaleY + this.PositionSetTranslationY;
                if (minY > y)
                {
                    minY = y;
                }
                else if (maxY < y)
                {
                    maxY = y;
                }
            }

            return new RectangleF(minX - 10, minY - 10, maxX - minX + 20, maxY - minY + 20);
        }

        public override void SpringLayerMaxRectChangedEvent()
        {
            positionSetIsChanged = true;
            base.SpringLayerMaxRectChangedEvent();
        }

        private float positionSetScaleX = 1;
        [CategoryAttribute("Layer")]
        public float PositionSetScaleX
        {
            get { return positionSetScaleX; }
            set
            {
                if (positionSetScaleX != value)
                {
                    positionSetScaleX = value;
                    positionSetIsChanged = true;
                    transformIsChanged = true;
                    SpringLayerRepresentationChangedEvent(this);
                }
            }
        }

        private float positionSetScaleY = 1;
        [CategoryAttribute("Layer")]
        public float PositionSetScaleY
        {
            get { return positionSetScaleY; }
            set
            {
                if (positionSetScaleY != value)
                {
                    positionSetScaleY = value;
                    positionSetIsChanged = true;
                    transformIsChanged = true;
                    SpringLayerRepresentationChangedEvent(this);
                }
            }
        }

        private float positionSetTranslationX = 0;
        [CategoryAttribute("Layer")]
        public float PositionSetTranslationX
        {
            get { return positionSetTranslationX; }
            set
            {
                if (positionSetTranslationX != value)
                {
                    positionSetTranslationX = value;
                    positionSetIsChanged = true;
                    transformIsChanged = true;
                    SpringLayerRepresentationChangedEvent(this);
                }
            }
        }

        private float positionSetTranslationY = 0;
        [CategoryAttribute("Layer")]
        public float PositionSetTranslationY
        {
            get { return positionSetTranslationY; }
            set
            {
                if (positionSetTranslationY != value)
                {
                    positionSetTranslationY = value;
                    positionSetIsChanged = true;
                    transformIsChanged = true;
                    SpringLayerRepresentationChangedEvent(this);
                }
            }
        }

        public void SetPositionSetTransformByM2MLevel(ILevel level)
        {
            this.PositionSetScaleX = level.GetGridWidth();
            this.PositionSetScaleY = level.GetGridHeight();
            this.PositionSetTranslationX = -(level.ConvertRealValueToRelativeValueX(0) - level.GetGridWidth() / 2);
            this.PositionSetTranslationY = -(level.ConvertRealValueToRelativeValueY(0) - level.GetGridHeight() / 2);
        }

        public float ConvertPositionSetXToRealX(float positionSetX)
        {
            return positionSetX * positionSetScaleX + positionSetTranslationX;
        }

        public float ConvertPositionSetYToRealY(float positionSetY)
        {
            return positionSetY * positionSetScaleY + positionSetTranslationY;
        }

        float totalScaleX;
        float totalScaleY;
        float totalTranslationX;
        float totalTranslationY;

        public void SetTotalTransform()
        {
            totalScaleX = positionSetScaleX * ScreenScaleX;
            totalScaleY = positionSetScaleY * ScreenScaleY;
            totalTranslationX = positionSetTranslationX * ScreenScaleX + ScreenTranslationX;
            totalTranslationY = positionSetTranslationY * ScreenScaleY + ScreenTranslationY;
        }

        /// <summary>
        /// 可以直接地完成positionset的坐标和屏幕坐标的转换
        /// </summary>
        /// <param name="positionSetX"></param>
        /// <returns></returns>
        public float ConvertPositionSetXToScreenX(float positionSetX)
        {
            if (transformIsChanged)
            {
                SetTotalTransform();
                transformIsChanged = false;
            }
            return positionSetX * totalScaleX + totalTranslationX;
        }

        /// <summary>
        /// 可以直接地完成positionset的坐标和屏幕坐标的转换
        /// </summary>
        /// <param name="positionSetY"></param>
        /// <returns></returns>
        public float ConvertPositionSetYToScreenY(float positionSetY)
        {
            if (transformIsChanged)
            {
                SetTotalTransform();
                transformIsChanged = false;
            }
            return positionSetY * totalScaleY + totalTranslationY;
        }
    }

    /// <summary>
    /// Add new position set layer style below
    /// </summary>
    #region custom position set layer

    public class Layer_PositionSet_ConvexHull : Layer_PositionSet
    {
        PositionSetDrawerPump positionSetDrawerPump;        

        public Layer_PositionSet_ConvexHull(IPositionSet positionSet)
            : base(positionSet)
        {
            positionSetDrawerPump = new PositionSetDrawerPump(this);
            HullCoordinate.Visible = false;
        }

        public override Color MainColor
        {
            get { return hullLineDrawer.LineColor; }
            set { hullLineDrawer.LineColor = value; SpringLayerRepresentationChangedEvent(this); }
        }

        public bool fillColor;
        public bool FillColor
        {
            get { return fillColor; }
            set { fillColor = value; SpringLayerRepresentationChangedEvent(this); }
        }

        PointDrawer hullPointDrawer = new PointDrawer();
        public PointDrawer HullPoint
        {
            get { return hullPointDrawer; }
            set { hullPointDrawer = value; SpringLayerRepresentationChangedEvent(this); }
        }

        CoordinateDrawer hullPointCoordinateDrawer = new CoordinateDrawer();
        public CoordinateDrawer HullCoordinate
        {
            get { return hullPointCoordinateDrawer; }
            set { hullPointCoordinateDrawer = value; SpringLayerRepresentationChangedEvent(this); }
        }

        PolygonDrawer polygonDrawer = new PolygonDrawer();
        public PolygonDrawer Ploygon
        {
            get { return polygonDrawer; }
            set { polygonDrawer = value; SpringLayerRepresentationChangedEvent(this); }
        }

        CloseLineDrawer hullLineDrawer = new CloseLineDrawer();
        public CloseLineDrawer ConvexHull
        {
            get { return hullLineDrawer; }
            set { hullLineDrawer = value; SpringLayerRepresentationChangedEvent(this); }
        }



        public override void Draw(Graphics graphics)
        {
            if (Visible)
            {
                SetDrawerGraphic(graphics);

                positionSetDrawerPump.ResetPump();
                positionSetDrawerPump.DrawPoint(hullPointDrawer);
                positionSetDrawerPump.DrawCoordinate(hullPointCoordinateDrawer);
                positionSetDrawerPump.DrawLineClose(hullLineDrawer);
                if (fillColor)
                    positionSetDrawerPump.FillPloygon(polygonDrawer);
                positionSetDrawerPump.Run();
            }
        }
    }

    public class Layer_PositionSet_Path : Layer_PositionSet
    {
        PositionSetDrawerPump positionSetDrawerPump;

        public Layer_PositionSet_Path(IPositionSet positionSet)
            : base(positionSet)
        {
            positionSetDrawerPump = new PositionSetDrawerPump(this);

            pathPointCoordinateDrawer.Visible = false;
        }

        public override Color MainColor
        {
            get { return pathLineDrawer.LineColor; }
            set { pathLineDrawer.LineColor = value; SpringLayerRepresentationChangedEvent(this); }
        }

        PointDrawer pathPointDrawer = new PointDrawer();
        public PointDrawer PathPoint
        {
            get { return pathPointDrawer; }
            set { pathPointDrawer = value; SpringLayerRepresentationChangedEvent(this); }
        }

        CoordinateDrawer pathPointCoordinateDrawer = new CoordinateDrawer();
        public CoordinateDrawer PathPointCoordinate
        {
            get { return pathPointCoordinateDrawer; }
            set { pathPointCoordinateDrawer = value; SpringLayerRepresentationChangedEvent(this); }
        }

        OpenLineDrawer pathLineDrawer = new OpenLineDrawer();
        public OpenLineDrawer PathLine
        {
            get { return pathLineDrawer; }
            set { pathLineDrawer = value; SpringLayerRepresentationChangedEvent(this); }
        }

        public override void Draw(Graphics graphics)
        {
            if (Visible)
            {
                SetDrawerGraphic(graphics);

                positionSetDrawerPump.ResetPump();
                positionSetDrawerPump.DrawCoordinate(pathPointCoordinateDrawer);
                positionSetDrawerPump.DrawLineOpen(pathLineDrawer);
                positionSetDrawerPump.DrawPoint(pathPointDrawer);
                positionSetDrawerPump.Run();
            }
        }
    }

    public class Layer_PositionSet_Point : Layer_PositionSet
    {
        PositionSetDrawerPump positionSetDrawerPump;

        public Layer_PositionSet_Point(IPositionSet positionSet)
            : base(positionSet)
        {
            positionSetDrawerPump = new PositionSetDrawerPump(this);

            pointCoordinateDrawer.Visible = false;
        }

        public override Color MainColor
        {
            get { return pointDrawer.PointColor; }
            set { pointDrawer.PointColor = value; SpringLayerRepresentationChangedEvent(this); }
        }

        PointDrawer pointDrawer = new PointDrawer();
        public PointDrawer Point
        {
            get { return pointDrawer; }
            set { pointDrawer = value; SpringLayerRepresentationChangedEvent(this); }
        }

        CoordinateDrawer pointCoordinateDrawer = new CoordinateDrawer();
        public CoordinateDrawer PointCoordinate
        {
            get { return pointCoordinateDrawer; }
            set { pointCoordinateDrawer = value; SpringLayerRepresentationChangedEvent(this); }
        }

        public override void Draw(Graphics graphics)
        {
            if (Visible)
            {
                SetDrawerGraphic(graphics);

                positionSetDrawerPump.ResetPump();
                positionSetDrawerPump.DrawCoordinate(pointCoordinateDrawer);
                positionSetDrawerPump.DrawPoint(pointDrawer);
                positionSetDrawerPump.Run();
            }
        }
    }

    public class Layer_PositionSet_Square : Layer_PositionSet
    {
        PositionSetDrawerPump positionSetDrawerPump;

        public Layer_PositionSet_Square(IPositionSet positionSet)
            : base(positionSet)
        {
            positionSetDrawerPump = new PositionSetDrawerPump(this);

            pointCoordinateDrawer.Visible = false;
        }

        public override Color MainColor
        {
            get { return squareFrameDrawer.LineColor; }
            set { squareFrameDrawer.LineColor = value; SpringLayerRepresentationChangedEvent(this); }
        }

        PointDrawer pointDrawer = new PointDrawer();
        public PointDrawer CenterPoint
        {
            get { return pointDrawer; }
            set { pointDrawer = value; SpringLayerRepresentationChangedEvent(this); }
        }

        CoordinateDrawer pointCoordinateDrawer = new CoordinateDrawer();
        public CoordinateDrawer CenterPointCoordinate
        {
            get { return pointCoordinateDrawer; }
            set { pointCoordinateDrawer = value; SpringLayerRepresentationChangedEvent(this); }
        }

        SquareFrameDrawer squareFrameDrawer = new SquareFrameDrawer();
        public SquareFrameDrawer SquareFrameDrawer
        {
            get { return squareFrameDrawer; }
            set { squareFrameDrawer = value; SpringLayerRepresentationChangedEvent(this); }
        }

        public override void Draw(Graphics graphics)
        {
            if (Visible)
            {
                SetDrawerGraphic(graphics);

                positionSetDrawerPump.ResetPump();
                positionSetDrawerPump.DrawSquareFrame(squareFrameDrawer);
                positionSetDrawerPump.DrawCoordinate(pointCoordinateDrawer);
                positionSetDrawerPump.DrawPoint(pointDrawer);
                positionSetDrawerPump.Run();
            }
        }
    }

    public class Layer_M2MPartSetInSpecificLevel : Layer_PositionSet
    {
        ILevel level;

        public Layer_M2MPartSetInSpecificLevel(ILevel level, IPositionSet partSet)
            : base(partSet)
        {
            this.level = level;
        }

        Pen penLine = new Pen(GenerateColor.GetNextDifferentColor());

        [BrowsableAttribute(false)]
        public override RectangleF MaxRect
        {
            get
            {                
                return new RectangleF();
            }
        }

        [CategoryAttribute("Drawer")]
        public Color LineColor
        {
            get
            {
                return penLine.Color;
            }
            set
            {
                penLine.Color = value; SpringLayerRepresentationChangedEvent(this);
            }
        }

        [CategoryAttribute("Drawer")]
        public float LineWidth
        {
            get
            {
                return penLine.Width;
            }
            set
            {
                penLine.Width = value; SpringLayerRepresentationChangedEvent(this);
            }
        }

        [CategoryAttribute("Drawer")]
        public System.Drawing.Drawing2D.DashStyle LineStyle
        {
            get
            {
                return penLine.DashStyle;
            }
            set
            {
                penLine.DashStyle = value; SpringLayerRepresentationChangedEvent(this);
            }
        }

        Color partColor = GenerateColor.GetNextDifferentColor();
        [CategoryAttribute("Drawer")]
        public Color PartColor
        {
            get { return partColor; }
            set { partColor = value; }
        }

        public override Color MainColor
        {
            get { return partColor; }
            set
            {
                partColor = value; penLine.Color = value;
                SpringLayerRepresentationChangedEvent(this);
            }
        }

        int alpha = 30;
        [CategoryAttribute("Drawer")]
        public int Alpha
        {
            get { return alpha; }
            set
            {
                if (value > 255)
                {
                    alpha = 255;
                }
                else if (value < 0)
                {
                    alpha = 0;
                }
                else
                {
                    alpha = value;
                }

                SpringLayerRepresentationChangedEvent(this);
            }
        }

        public override void Draw(Graphics graphics)
        {
            if (Visible)
            {
                Brush brush = new SolidBrush(Color.FromArgb(Alpha, partColor));
                penLine.Color = penLine.Color;

                positionSet.InitToTraverseSet();
                while (positionSet.NextPosition())
                {
                    graphics.FillRectangle(brush, GetPartScreenRectangleInSpecificLevel(level,
                        (int)positionSet.GetPosition().GetX(), (int)positionSet.GetPosition().GetY()));
                    graphics.DrawRectangle(penLine, GetPartScreenRectangleInSpecificLevel(level,
                        (int)positionSet.GetPosition().GetX(), (int)positionSet.GetPosition().GetY()));
                }
            }
        }
    }
    #endregion
}
