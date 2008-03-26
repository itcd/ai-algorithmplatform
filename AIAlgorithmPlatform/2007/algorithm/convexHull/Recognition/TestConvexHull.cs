using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Position_Interface;
using ConvexHullEngine;
using QuickHullAlgorithm;
using GrahamScanAlgorithm;
using M2M;
using Position_Implement;

namespace Recognition
{
    public partial class TestConvexHull : Form
    {
        public TestConvexHull()
        {
            InitializeComponent();
        }

        private PositionSetEdit_ImplementByICollectionTemplate testData()
        {
            int[] data =
            {
100, 100,
100, 200,
200, 100,
200, 200,
200, 200
            };
            int n = data.Length / 2;
            PositionSetEdit_ImplementByICollectionTemplate ps = new PositionSetEdit_ImplementByICollectionTemplate();
            for (int i = 0; i < n; i++)
            {
                ps.AddPosition( new Position_Point(data[2 * i], data[2 * i + 1]) );
            }
            return ps;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IConvexHullEngine convexhull;
            int n;
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    convexhull = new GrahamScan();
                    break;
                case 1:
                    //convexhull = new QuickHull();
                    convexhull = new M2M_CH();
                    break;
                default:
                    return;
            }
            try
            {
                n = Int32.Parse( textBox1.Text );
            }
            catch (Exception)
            {
                return;
            }
            
            //Point[] ps = new Point[n];
            PositionSetEdit_ImplementByICollectionTemplate ps = new PositionSetEdit_ImplementByICollectionTemplate();
            Random r = new Random(DateTime.Now.Millisecond);

            //if (true)
            {
                int minx = (int)(pb.Width * 0.05);
                int miny = (int)(pb.Height * 0.05);
                for (int i = 0; i < n; i++)
                {
                    int x = r.Next(minx, pb.Width - minx);
                    int y = r.Next(miny, pb.Height - miny);
                    //ps[i] = new Point(x, y);
                    ps.AddPosition(new Position_Point(x, y));
                    System.Console.Write(x);
                    System.Console.Write(",");
                    System.Console.Write(y);
                    System.Console.WriteLine(",");
                }
                System.Console.WriteLine();
            }
            //else
            //{
            //    ps = testData();
            //}

            IPositionSet convexPoints = convexhull.ConvexHull(ps);
            pb.Image = new Bitmap(pb.Width, pb.Height);
            Graphics g = Graphics.FromImage(pb.Image);
            convexPoints.InitToTraverseSet();

            g.Clear(Color.White);

            /*
            for (int i = 0; i < ps.Length; i++)
            {
                g.FillEllipse(Brushes.Black, ps[i].X - 2, pb.Height - ps[i].Y - 2, 4, 4);
                g.DrawString("(" + ps[i].X + "," + ps[i].Y + ")", Font, Brushes.Blue, ps[i].X, pb.Height - ps[i].Y);
            }
            for (int i = 0; i < convexPoints.Length; i++)
            {
                if (i != convexPoints.Length - 1)
                    g.DrawLine(new Pen(Color.Green), new PointF(convexPoints[i].X, pb.Height - convexPoints[i].Y), new PointF(convexPoints[i + 1].X, pb.Height - convexPoints[i + 1].Y));
                else
                {
                    g.DrawLine(new Pen(Color.Green), new PointF(convexPoints[i].X, pb.Height - convexPoints[i].Y), new PointF(convexPoints[0].X, pb.Height - convexPoints[0].Y));
                }
            }
             * */
            ps.InitToTraverseSet();
            IPosition p = ps.GetPosition();
            while (p != null)
            {
                g.FillEllipse(Brushes.Black, p.GetX() - 2, pb.Height - p.GetY() - 2, 4, 4);
                g.DrawString("(" + p.GetX() + "," + p.GetY() + ")", Font, Brushes.Blue, p.GetX(), pb.Height - p.GetY());
                p = ps.GetPosition();
            }
            IPosition p1 = convexPoints.GetPosition();
            IPosition p2 = convexPoints.GetPosition();
            while (p2 != null)
            {
                g.DrawLine(new Pen(Color.Green), new PointF(p1.GetX(), pb.Height - p1.GetY()), new PointF(p2.GetX(), pb.Height - p2.GetY()));
                p1 = p2;
                p2 = convexPoints.GetPosition();
            }
            convexPoints.InitToTraverseSet();
            p2 = convexPoints.GetPosition();
            g.DrawLine(new Pen(Color.Green), new PointF(p1.GetX(), pb.Height - p1.GetY()), new PointF(p2.GetX(), pb.Height - p2.GetY()));

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Form1_Resize(sender, e);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            pb.Left = 0;
            pb.Top = 30;
            pb.Width = this.ClientRectangle.Width;
            pb.Height = this.ClientRectangle.Height - 20;
        }
    }
}