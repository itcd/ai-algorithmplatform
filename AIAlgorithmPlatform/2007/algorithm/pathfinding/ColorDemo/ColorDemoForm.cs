using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using DrawingLib;

namespace ColorDemo
{
    public partial class ColorDemoForm : Form
    {
        public ColorDemoForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Graphics g = this.CreateGraphics();
            g.FillEllipse(Brushes.DarkBlue, this.ClientRectangle);
            g.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Graphics g = this.CreateGraphics();
            g.FillEllipse(SystemBrushes.Control, this.ClientRectangle);
            g.Dispose();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Graphics g = this.CreateGraphics();
            int x = 0;
            int y = 0;
            int width = this.ClientRectangle.Width;
            int height = this.ClientRectangle.Height / 5;
            Brush whiteBrush = System.Drawing.Brushes.White;
            Brush blackBrush = System.Drawing.Brushes.Black;
            using (Brush brush = new SolidBrush(Color.DarkBlue))
            {
                g.FillRectangle(brush, x, y, width, height);
                g.DrawString(brush.ToString(), this.Font, whiteBrush, x, y);
                y += height;
            }
            string file = @"E:\document\My Pictures\mine\俄罗斯方块_图标_16.bmp";
            using (Brush brush =
            new TextureBrush(new Bitmap(file)))
            {
                g.FillRectangle(brush, x, y, width, height);
                g.DrawString(brush.ToString(), this.Font, whiteBrush, x, y);
                y += height;
            }
            using (Brush brush =
            new HatchBrush(
            HatchStyle.Divot, Color.DarkBlue, Color.White))
            {
                g.FillRectangle(brush, x, y, width, height);
                g.DrawString(brush.ToString(), this.Font, blackBrush, x, y);
                y += height;
            }
            using (Brush brush =
            new LinearGradientBrush(
            new Rectangle(x, y, width, height),
            Color.DarkBlue,
            Color.White,
            45.0f))
            {
                g.FillRectangle(brush, x, y, width, height);
                g.DrawString(brush.ToString(), this.Font, blackBrush, x, y);
                y += height;
            }
            Point[] points = new Point[] { new Point(x, y),
new Point(x + width, y),
new Point(x + width, y + height),
new Point(x, y + height) };
            using (Brush brush = new PathGradientBrush(points))
            {
                g.FillRectangle(brush, x, y, width, height);
                g.DrawString(brush.ToString(), this.Font, blackBrush, x, y);
                y += height;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int x = 0;
            int y = 0;
            int width = 1;
            int height = 1;
            int r = 0;
            int g = 0;
            int b = 0;
            int a = 255;
            int max = 256;
            Graphics g1 = null;
            Graphics g2 = null;
            Graphics g3 = null;
            Graphics gp1 = null;
            Graphics gp2 = null;
            Graphics gp3 = null;
            Brush brush = null;
            Bitmap b1 = new Bitmap(256, 256);
            Bitmap b2 = new Bitmap(256, 256);
            Bitmap b3 = new Bitmap(256, 256);
          
            try
            {
                g1 = Graphics.FromImage(b1);
                g2 = Graphics.FromImage(b2);
                g3 = Graphics.FromImage(b3);
                for (r = 0; r < max; r++)
                {
                    for (g = 0; g < max; g++)
                    {
                        x = g;
                        y = r;
                        brush = new SolidBrush(Color.FromArgb(a, r, g, b));
                        g1.FillRectangle(brush, x, y, width, height);
                    }
                }
                gp1 = panel1.CreateGraphics();
                gp1.DrawImage(b1, new Point(0, 0));

                r = 0;
                for (g = 0; g < max; g++)
                {
                    for (b = 0; b < max; b++)
                    {
                        x = b;
                        y = g;
                        brush = new SolidBrush(Color.FromArgb(a, r, g, b));
                        g2.FillRectangle(brush, x, y, width, height);
                    }
                }
                gp2 = panel2.CreateGraphics();
                gp2.DrawImage(b2, new Point(0, 0));

                g = 0;
                for (b = 0; b < max; b++)
                {
                    for (r = 0; r < max; r++)
                    {
                        x = r;
                        y = b;
                        brush = new SolidBrush(Color.FromArgb(a, r, g, b));
                        g3.FillRectangle(brush, x, y, width, height);
                    }
                }
                gp3 = panel3.CreateGraphics();
                gp3.DrawImage(b3, new Point(0, 0));
            }
            finally
            {
                gp1.Dispose();
                gp2.Dispose();
                gp3.Dispose();
                g1.Dispose();
                g2.Dispose();
                g3.Dispose();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Graphics gb = null;
            Graphics g = null;
            Rainbow rb = new Rainbow();
            Brush brush = null;
            try
            {
                Bitmap b = new Bitmap(780, 64);
                gb = Graphics.FromImage(b);
                //rb.setMaxValue(780);

                int i;
                int x = 0;
                for (i = -1; i <= rb.getHalfMax(); i++)
                {
                    brush = new SolidBrush(rb.getColor(i));
                    gb.FillRectangle(brush, x, 0, 1, 32);
                    x++;
                }
                
                x = 2;
                for (i = 766; i <= rb.getMax(); i++)
                {
                    brush = new SolidBrush(rb.getColor(i));
                    gb.FillRectangle(brush, x, 32, 1, 32);
                    x++;
                }

                g = panel4.CreateGraphics();
                g.DrawImage(b, new Point(0, 0));
            }
            finally
            {
                g.Dispose();
                gb.Dispose();
            }
        }
    }
}