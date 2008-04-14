using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Position_Interface;
using System.ComponentModel;
using DrawingLib;

namespace PositionSetDrawer
{
    [TypeConverter(typeof(Converter))]
    [CategoryAttribute("Drawer")]
    public class ConnectionDrawer : ADraw
    {
        Pen penLine = new Pen(GenerateColor.GetNextDifferentColor());

        Pen borderPenLine = new Pen(Color.Black);

        CustomLineCap HookCap = null;

        bool showBorder = true;

        public ConnectionDrawer()
        {
            penLine.EndCap = LineCap.ArrowAnchor;

            GraphicsPath hPath = new GraphicsPath();

            // Create the outline for our custom end cap.

            //hPath.AddLine(new Point(1, -8), new Point(0,-4));

            //hPath.AddLine(new Point(0,-4), new Point(-1,-8));

            //hPath.AddLine(new Point(-1,-8), new Point(1,-8));

            int bias = 14;
            int halfWidth = 2;
            int length = 4;

            hPath.AddLine(new Point(halfWidth, -bias), new Point(0, -bias + length));

            hPath.AddLine(new Point(0, -bias + length), new Point(-halfWidth, -bias));

            hPath.AddLine(new Point(-halfWidth, -bias), new Point(halfWidth, -bias));

            //hPath.AddLine(new Point(0, 0), new Point(0, 10));


            // Construct the hook-shaped end cap.

            HookCap = new CustomLineCap(null, hPath);

            // Set the start cap and end cap of the HookCap to be rounded.

            HookCap.SetStrokeCaps(LineCap.Round, LineCap.Round);

            penLine.CustomEndCap = HookCap;

            LineWidth = 1;
        }

        public LineCap EndCap
        {
            get { return penLine.EndCap; }
            set
            {
                penLine.EndCap = value;
                SpringDrawerPropertyChangedEvent();
            }
        }

        public LineCap StartCap
        {
            get { return penLine.StartCap; }
            set
            {
                penLine.StartCap = value;
                SpringDrawerPropertyChangedEvent();
            }
        }

        public Color LineColor
        {
            get
            {
                return penLine.Color;
            }
            set
            {
                penLine.Color = value;
                SpringDrawerPropertyChangedEvent();
            }
        }

        public Color BorderLineColor
        {
            get
            {
                return borderPenLine.Color;
            }
            set
            {
                borderPenLine.Color = value;
                SpringDrawerPropertyChangedEvent();
            }
        }

        public bool ShowBorderLine
        {
            get { return showBorder; }
            set
            {
                showBorder = value;
                SpringDrawerPropertyChangedEvent();
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
                borderPenLine.Width = value + 1.4f;
                penLine.Width = value;
                for (int i = 0; i < p.Length; i++)
                    p[i].Width = value;
                SpringDrawerPropertyChangedEvent();
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
                penLine.DashStyle = value;
                SpringDrawerPropertyChangedEvent();
            }
        }

        public void Draw(float x1, float y1, float x2, float y2)
        {
            if (Visible == false)
            {
                return;
            }

            int min = 28 * 28;

            if ((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1) > min)
            {
                penLine.CustomEndCap = HookCap;
            }
            else
            {
                penLine.EndCap = LineCap.NoAnchor;
            }

            if (showBorder)
            {
                if (triple)
                {
                    drawTripleLine(graphics, p, x1, y1, x2, y2);
                }
                else
                {
                    graphics.DrawLine(borderPenLine, x1, y1, x2, y2);
                }
            }
            graphics.DrawLine(penLine, x1, y1, x2, y2);

        }

        protected bool triple = true;
        Pen[] p = { new Pen(Color.White), new Pen(GenerateColor.GetNextDifferentColor()), new Pen(GenerateColor.GetNextDifferentColor()) };

        public bool TripleColorsBorder
        {
            get { return triple; }
            set
            {
                triple = value;
                SpringDrawerPropertyChangedEvent();
            }
        }

        public Pen[] TriplePens
        {
            get { return p; }
            set
            {
                p = value;
                SpringDrawerPropertyChangedEvent();
            }
        }

        protected void drawTripleLine(Graphics g, Pen[] p, float x1, float y1, float x2, float y2)
        {
            float[] xa = new float[3], ya = new float[3];
            xa[0] = x1;
            ya[0] = y1;
            float[] xb = new float[3], yb = new float[3];
            xb[0] = x2;
            yb[0] = y2;
            float width = p[0].Width;
            float w = width * 0.8f;

            if (Math.Abs(xb[0] - xa[0]) > 0.1)
            {
                int dx = (int)((xb[0] - xa[0]) / Math.Abs(xb[0] - xa[0]));
                xa[1] = xa[0] + dx * width;
                xa[2] = xa[0];
                xb[1] = xb[0];
                xb[2] = xb[0] - dx * width;
            }
            else
            {
                xa[1] = xa[0] + w;
                xa[2] = xa[0] - w;
                xb[1] = xb[0] + w;
                xb[2] = xb[0] - w;
            }

            if (Math.Abs(yb[0] - ya[0]) > 0.1)
            {
                int dy = (int)((yb[0] - ya[0]) / Math.Abs(yb[0] - ya[0]));
                ya[1] = ya[0];
                ya[2] = ya[0] + dy * width;
                yb[1] = yb[0] - dy * width;
                yb[2] = yb[0];
            }
            else
            {
                ya[1] = ya[0] - w;
                ya[2] = ya[0] + w;
                yb[1] = yb[0] - w;
                yb[2] = yb[0] + w;
            }

            for (int i = 1; i < p.Length; i++)
            {
                g.DrawLine(p[i], xa[i], ya[i], xb[i], yb[i]);
            }
        }
    }
}