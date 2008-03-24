using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
using KDT;
using M2M;
using Position_Interface;
using NearestNeighbor;

namespace TestInGUI
{
    public partial class Form1 : Form
    {
        private int height;
        private int width;
        private int oldx = 0;
        private int oldy = 0;
        bool selecting = false;
        private List<IPosition> pointList;

        public Form1()
        {
            InitializeComponent();
            pointList = new List<IPosition>();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(pictureBox1.Image);
            g.Clear(Color.White);
            ScatterPoints(1000, pictureBox1);
            pictureBox1.Refresh();
            label3.Text = "Xmas:" + pictureBox1.Width.ToString() + ",Ymax:" + pictureBox1.Height.ToString();

        }

        private void ScatterPoints(int n, PictureBox pb)
        {

            Rectangle rt = new Rectangle(0, 0, pb.Width, pb.Height);
            Bitmap bmp = new Bitmap(rt.Width, rt.Height);
            for (int i = 0; i < rt.Width; i++)
            {
                for (int j = 0; j < rt.Height; j++)
                {
                    bmp.SetPixel(i, j, Color.White);
                }
            }
            
            pointList.Clear();
            Random r = new Random(DateTime.Now.Millisecond);
            for (int k = 0; k < n; k++)
            {
                int x, y, minx, miny;
                minx = (int)(rt.Width * .2);
                miny = (int)(rt.Height * .2);
                x = r.Next(minx, rt.Width - minx);
                y = r.Next(miny, rt.Height - miny);
                KD2DPoint pp = new KD2DPoint(x, y);
                pointList.Add(pp);
                bmp.SetPixel(x, y, Color.Black);
            }

            //   pb.DrawToBitmap(bmp, rt);

            pb.Image = (Image)bmp;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Point[] nPoints = getPoints((Bitmap)pictureBox1.Image);
            List<IPosition> al = new List<IPosition>();
            al = pointList;
            //for (int i = 0; i < nPoints.Length; i++)
            //{
            //   al.Add(new KD2DPoint(nPoints[i]));
            //   // all.Add(new KD2DPoint(nPoints[i]));
            //}
           // KDTree Ktree = KDTree.CreateKDTree(all);
            INearestNeighbor M2Mnn = new M2M_NN();
            INearestNeighbor KDTnn = new KDT_NN();
            //M2Mnn.Init(400, 400, 100, 5, 5);
            int runTimes = 10000;
            M2Mnn.PreProcess(al);
            
            KDTnn.PreProcess(al);
            Graphics g = Graphics.FromImage(pictureBox1.Image);

            int px = int.Parse(textBox1.Text);
            int py = int.Parse(textBox2.Text);
            KD2DPoint AskPoint = new KD2DPoint(px, py);
            long timeMM, timeKD;
            Stopwatch sW = new Stopwatch();
             KD2DPoint nearest=null;
             KD2DPoint nearest2 = null;
             sW.Start();
             for (int i = 0; i < runTimes; i++)
            {
                nearest = (KD2DPoint)M2Mnn.NearestNeighbor(AskPoint);
            }
            sW.Stop();
            timeMM = sW.ElapsedTicks / runTimes;
            sW.Reset();
            sW.Start();
            for (int i = 0; i < runTimes; i++)
            {
                nearest2 = (KD2DPoint)KDTnn.NearestNeighbor(AskPoint);
            }
            if (!nearest.Equals(nearest2))
            {
                MessageBox.Show("结果不符！");
            }
            sW.Stop();
            timeKD = sW.ElapsedTicks / runTimes;
            sW.Reset();
            M2Mlabel.Text = timeMM.ToString();
            KDTlabel.Text = timeKD.ToString();

             //   label4.Text = "最近距离：" + dis.ToString();
               g.DrawLine(new Pen(Color.SteelBlue), new PointF(AskPoint.X, AskPoint.Y), new PointF(nearest.X, nearest.Y));

           
            pictureBox1.Refresh();

        }
        private Point[] getPoints(Bitmap bmp)
        {
            List<Point> psList = new List<Point>();
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    if (bmp.GetPixel(i, j).Name != "ffffffff")
                    {
                        Point pp = new Point(i, j);
                        psList.Add(pp);
                    }
                }
            }
            return psList.ToArray();

        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (selecting)
            {
                textBox1.Text = e.X.ToString();
                textBox2.Text = e.Y.ToString();
            }

        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            selecting = false;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            selecting = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int n=int.Parse(textBox3.Text);
            Graphics g = Graphics.FromImage(pictureBox1.Image);
            g.Clear(Color.White);
            pictureBox1.Refresh();
            ScatterPoints(n, pictureBox1);
        }
    }
}