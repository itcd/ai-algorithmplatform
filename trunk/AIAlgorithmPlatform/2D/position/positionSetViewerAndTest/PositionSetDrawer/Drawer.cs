using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Position_Interface;
using System.ComponentModel;
using DrawingLib;

namespace PositionSetDrawer
{
    internal class Converter : ExpandableObjectConverter
    { }

    [TypeConverter(typeof(Converter))]
    [CategoryAttribute("Drawer")]
    public class PointDrawer : ADraw
    {
        //Pen penLine = new Pen(Color.Black);

        Brush BorderBrush = new SolidBrush(Color.Black);

        public PointDrawer()
        {
            pointColor = GenerateColor.GetNextDifferentColor();
            pointBrush = new SolidBrush(pointColor);
        }

        Brush pointBrush;

        Color pointColor;        
        public Color PointColor
        {
            get
            {
                return pointColor;
            }
            set 
            {
                pointColor = value;
                pointBrush = new SolidBrush(pointColor);
                SpringDrawerPropertyChangedEvent();
            }
        }

        float pointRadius = 1;
        public float PointRadius
        {
            get
            {
                return pointRadius;
            }
            set
            {
                pointRadius = value;
                SpringDrawerPropertyChangedEvent();
            }
        }

        bool isDrawPointBorder = false;
        public bool IsDrawPointBorder
        {
            get { return isDrawPointBorder; }
            set { isDrawPointBorder = value; SpringDrawerPropertyChangedEvent(); }
        }
        
        public void Draw(float x, float y)
        {
            if (Visible == false)
            {
                return;
            }

            if (isDrawPointBorder)
            {
                graphics.FillEllipse(BorderBrush, x - pointRadius - 1, y - pointRadius - 1, (pointRadius + 1) * 2, (pointRadius + 1) * 2);
            }
            graphics.FillEllipse(pointBrush, x - pointRadius, y - pointRadius, pointRadius * 2, pointRadius * 2);
        }
    }

    [TypeConverter(typeof(Converter))]
    [CategoryAttribute("Drawer")]
    public class CoordinateDrawer : ADraw
    {
        Font coordinateFont = new Font("ËÎÌו", 9);

        public Font CoordinateFont
        {
            get { return coordinateFont; }
            set { coordinateFont = value; SpringDrawerPropertyChangedEvent(); }
        }

        Brush fontBrush =  SystemBrushes.MenuText;

        float realCoordinateX = 0;
        float realCoordinateY = 0;

        public void RecordRealCoordinate(float x, float y)
        {
            realCoordinateX = x;
            realCoordinateY = y;
        }

        public void Draw(float x, float y)
        {
            if (Visible == false)
            {
                return;
            }

            //graphics.DrawString(string.Format("({0:0},{1:0})", realCoordinateX, realCoordinateY), CoordinateFont, fontBrush, x, y - 5);
            graphics.DrawString("( " + realCoordinateX +", " + realCoordinateY + " )", CoordinateFont, fontBrush, x, y - 5);
        }
    }

    [TypeConverter(typeof(Converter))]
    [CategoryAttribute("Drawer")]
    public class OpenLineDrawer : ADraw
    {
        Pen penLine = new Pen(GenerateColor.GetNextDifferentColor());
                
        public Color LineColor
        {
            get
            {
                return penLine.Color; 
            }
            set 
            {
                penLine.Color = value; SpringDrawerPropertyChangedEvent();
            }
        }

        public float LineWidth
        {
            get
            {
                return penLine.Width;
            }
            set
            {
                penLine.Width = value; SpringDrawerPropertyChangedEvent();
            }
        }

        public System.Drawing.Drawing2D.DashStyle LineStyle
        {
            get
            {
                return penLine.DashStyle;
            }
            set
            {
                penLine.DashStyle = value; SpringDrawerPropertyChangedEvent();
            }
        }

        float xOld = 0;
        float yOld = 0;

        public void DrawFirstPoint(float x, float y)
        {
            if (Visible == false)
            {
                return;
            }

            xOld = x;
            yOld = y;
        }

        public void Draw(float x, float y)
        {
            if (Visible == false)
            {
                return;
            }

            graphics.DrawLine(penLine, new PointF(xOld, yOld), new PointF(x, y));

            xOld = x;
            yOld = y;
        }
    }

    [TypeConverter(typeof(Converter))]
    [CategoryAttribute("Drawer")]
    public class PolygonDrawer : ADraw
    {
        SolidBrush brush = new SolidBrush(GenerateColor.GetNextDifferentColor());
        List<Point> polygonPoints = new List<Point>();

        public Color BrushColor
        {
            get
            {
                return brush.Color;
            }
            set
            {
                brush.Color = value; SpringDrawerPropertyChangedEvent();
            }
        }

        public void MeetFirstPoint(float x, float y)
        {
            polygonPoints.Clear();
            polygonPoints.Add(new Point((int)x, (int)y));
        }

        public void MeetMiddlePoint(float x, float y)
        {
            polygonPoints.Add(new Point((int)x, (int)y));
        }

        public void MeetLastPoint(float x, float y)
        {
            polygonPoints.Add(new Point((int)x, (int)y));
            graphics.FillPolygon(brush, polygonPoints.ToArray());
        }
    }

    [TypeConverter(typeof(Converter))]
    [CategoryAttribute("Drawer")]
    public class CloseLineDrawer : ADraw
    {
        Pen penLine = new Pen(GenerateColor.GetNextDifferentColor());

        public Color LineColor
        {
            get
            {
                return penLine.Color;
            }
            set
            {
                penLine.Color = value; SpringDrawerPropertyChangedEvent();
            }
        }

        public float LineWidth
        {
            get
            {
                return penLine.Width;
            }
            set
            {
                penLine.Width = value; SpringDrawerPropertyChangedEvent();
            }
        }

        public System.Drawing.Drawing2D.DashStyle LineStyle
        {
            get
            {
                return penLine.DashStyle;
            }
            set
            {
                penLine.DashStyle = value; SpringDrawerPropertyChangedEvent();
            }
        }

        float xStart = 0;
        float yStart = 0;

        float xOld = 0;
        float yOld = 0;

        public void DrawFirstPoint(float x, float y)
        {
            if (Visible == false)
            {
                return;
            }

            xStart = x;
            yStart = y;

            xOld = x;
            yOld = y;
        }

        public void DrawMiddlePoint(float x, float y)
        {
            if (Visible == false)
            {
                return;
            }

            graphics.DrawLine(penLine, new PointF(xOld, yOld), new PointF(x, y));

            xOld = x;
            yOld = y;
        }

        public void DrawLastPoint(float x, float y)
        {
            if (Visible == false)
            {
                return;
            }

            graphics.DrawLine(penLine, new PointF(xOld, yOld), new PointF(x, y));
            graphics.DrawLine(penLine, new PointF(x, y), new PointF(xStart, yStart));
        }
    }

    [TypeConverter(typeof(Converter))]
    [CategoryAttribute("Drawer")]
    public class SquareFrameDrawer : ADraw
    {
        Pen penLine = new Pen(GenerateColor.GetNextDifferentColor());

        public Color LineColor
        {
            get
            {
                return penLine.Color;
            }
            set
            {
                penLine.Color = value; SpringDrawerPropertyChangedEvent();
            }
        }

        public float LineWidth
        {
            get
            {
                return penLine.Width;
            }
            set
            {
                penLine.Width = value; SpringDrawerPropertyChangedEvent();
            }
        }

        private float rectangleRadiusWidth = 20;

        public float RectangleRadiusWidth
        {
            get { return rectangleRadiusWidth; }
            set { rectangleRadiusWidth = value; SpringDrawerPropertyChangedEvent(); }
        }

        public void Draw(float x, float y)
        {
            if (Visible == false)
            {
                return;
            }

            graphics.DrawRectangle(penLine, x - rectangleRadiusWidth / 2, y - rectangleRadiusWidth / 2, rectangleRadiusWidth, rectangleRadiusWidth);
        }
    }
}
