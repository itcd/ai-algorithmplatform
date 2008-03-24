using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using Position_Interface;
using ConvexHullEngine;
using QuickHullAlgorithm;
using GrahamScanAlgorithm;
using Position_Implement;

namespace Recognition
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DSUnit
    {
        public int i;
        public int j;
        public byte d;
    }

    public partial class MainForm : Form
    {
        private int height;
        private int width;
        private int oldx = 0;
        private int oldy = 0;
        public MainForm()
        {
            InitializeComponent();
            height = this.Height;
            width = this.Width;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                this.Height = height;
                this.Width = width;
            }
            catch
            {
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Load(openFileDialog1.FileName);
                pictureBox2.Load(openFileDialog1.FileName);
            }
        }

        
        private void button2_Click(object sender, EventArgs e)
        {
            //ProcessEx(pictureBox2);

            Point[] ps = getPoints((Bitmap)pictureBox2.Image);

            double sigmaX, sigmaY, sigmaXY, mX, mY;
            sigmaX = 0;
            sigmaXY = 0;
            sigmaY = 0;
            mY = 0;
            mX = 0;
            for (int i = 0; i < ps.Length; i++)
            {
                mX += ps[i].X;
                mY += ps[i].Y;
            }
            mX = mX / ps.Length;
            mY = mY / ps.Length;
            float mmX = (float)mX;
            float mmY = (float)mY;

            

//            Bitmap image = new Bitmap(pictureBox2.Image);
            //pictureBox1.Image = image;
            //pictureBox1.Load(openFileDialog1.FileName);
            Bitmap image = new Bitmap(pictureBox2.Image);
            Graphics graphics =pictureBox1.CreateGraphics();
            //graphics.Clear(Color.White);

            //装入图片

            

            //获取当前窗口的中心点
            Rectangle rect = new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height);
            PointF center = new PointF(rect.Width / 2, rect.Height / 2);

            float offsetX = 0;
            float offsetY = 0;
            offsetX = center.X - image.Width / 2;
            offsetY = center.Y - image.Height / 2;
            //构造图片显示区域:让图片的中心点与窗口的中心点一致
            RectangleF picRect = new RectangleF(0, 0, image.Width, image.Height);
            PointF Pcenter = new PointF(mmX,
                mmY);
            for (int i = 0; i < ps.Length; i++)
            {
                sigmaX += (ps[i].X - mX) * (ps[i].X - mX);
                sigmaY += (ps[i].Y - mY) * (ps[i].Y - mY);
                sigmaXY += (ps[i].Y - mY) * (ps[i].X - mX);
            }
            EigenvalueTool eTool = new EigenvalueTool(2);
            double[][] a = new double[][] { new double[] { sigmaX, sigmaXY }, new double[] { sigmaXY, sigmaY } };
            double[] vector = eTool.MaxEigenVector(a);
            double rad = Math.Atan(vector[0] / vector[1]);
            rad = 180 * rad / Math.PI;
     
            float XX = mmX + 100 * (float)vector[0];
            float YY = mmY + 100 * (float)vector[1];
            
                //让图片绕中心旋转一周
                // 绘图平面以图片的中心点旋转
                graphics.TranslateTransform(Pcenter.X, Pcenter.Y);
                graphics.RotateTransform((float)rad);
                //恢复绘图平面在水平和垂直方向的平移
                graphics.TranslateTransform(-Pcenter.X, -Pcenter.Y);
                //绘制图片并延时
                graphics.DrawImage(image, picRect);
                graphics.DrawLine(new Pen(Brushes.Coral, 2), new PointF(mmX, mmY), new PointF(XX, YY));
                Graphics g = pictureBox2.CreateGraphics();
                g.DrawLine(new Pen(Brushes.Coral, 2), new PointF(mmX, mmY), new PointF(XX, YY));
                //graphics.Save();
               // Thread.Sleep(1000);
                //重置绘图平面的所有变换
                //pictureBox1.Image = image;
                //pictureBox1.Refresh();
              //  graphics.ResetTransform();
            


            //Matrix myMatrix = new Matrix();
            //myMatrix.RotateAt(45, Pcenter, MatrixOrder.Append);
            //graphics.Transform = myMatrix;
            //graphics.DrawRectangle(new Pen(Color.Beige), 150, 50, 200, 100);
            //pictureBox1.Image = image;
            //pictureBox1.Refresh();









       //     Bitmap image = new Bitmap(Application.StartupPath+"//聚类流程.bmp"); //new Bitmap(pictureBox1.Image);
       //     Rectangle rect = new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height);
       //     PointF center = new PointF(rect.Width / 2, rect.Height / 2);

       //     float offsetX = 0;
       //     float offsetY = 0;
       //     offsetX = center.X - pictureBox1.Image.Width / 2;
       //     offsetY = center.Y - pictureBox1.Image.Height / 2;
       //     //构造图片显示区域:让图片的中心点与窗口的中心点一致
       //     RectangleF picRect = new RectangleF(0, 0, pictureBox1.Image.Width, pictureBox1.Image.Height);





       //     Point[] ps = getPoints((Bitmap)pictureBox1.Image);
       //     //  ps = Convexhull.convexhull(ps);
       //     double sigmaX, sigmaY, sigmaXY, mX, mY;
       //     sigmaX = 0;
       //     sigmaXY = 0;
       //     sigmaY = 0;
       //     mY = 0;
       //     mX = 0;
       //     for (int i = 0; i < ps.Length; i++)
       //     {
       //         mX += ps[i].X;
       //         mY += ps[i].Y;
       //     }
       //     mX = mX / ps.Length;
       //     mY = mY / ps.Length;




       //     for (int i = 0; i < ps.Length; i++)
       //     {
       //         sigmaX += (ps[i].X - mX) * (ps[i].X - mX);
       //         sigmaY += (ps[i].Y - mY) * (ps[i].Y - mY);
       //         sigmaXY += (ps[i].Y - mY) * (ps[i].X - mX);
       //     }
       //     EigenvalueTool eTool = new EigenvalueTool(2);
       //     double[][] a = new double[][] { new double[] { sigmaX, sigmaXY }, new double[] { sigmaXY, sigmaY } };
       //     double[] vector = eTool.MaxEigenVector(a);
       //     double rad = Math.Atan(vector[0] / vector[1]);
       //     rad = 180 * rad / Math.PI;
       //     Graphics g = Graphics.FromImage(image);
       //     //g.Clear(Color.White);
       //     float mmX = (float)mX;
       //     float mmY = (float)mY;
       //     float XX = mmX + 100 * (float)vector[0];
       //     float YY = mmY + 100 * (float)vector[1];

       //     PointF Pcenter = new PointF(mmX,
       //mmY);
       //     g.TranslateTransform(Pcenter.X, Pcenter.Y);

       //     g.RotateTransform((float)30);

       //     g.TranslateTransform(-Pcenter.X, -Pcenter.Y);
       //     //g.DrawLine(new Pen(Brushes.Coral, 2), new PointF(mmX, mmY), new PointF(XX, YY));

       //     //g.Save();

       //     //   g.ResetTransform();
       //     //g.DrawImage(image, rect);
       //     pictureBox1.Image = image;
       //     pictureBox1.Refresh();
           
        }
 
        private void ScatterPoints(int n,PictureBox pb)
        {
            
            Rectangle rt = new Rectangle(0, 0, pb.Width, pb.Height);
            Bitmap bmp = new Bitmap( rt.Width, rt.Height);
            for (int i = 0; i < rt.Width; i++)
            {
                for (int j = 0; j <rt.Height; j++)
                {
                    bmp.SetPixel(i,j,Color.White);
                }
            }

            Random r = new Random(DateTime.Now.Millisecond);
            for (int k = 0; k < n; k++)
            {
                int x, y,minx,miny;
               minx=(int)(rt.Width * .2);
                miny=(int)(rt.Height*.2);
                x = r.Next(minx, rt.Width -minx);
               y = r.Next(miny, rt.Height -miny);
                bmp.SetPixel(x, y, Color.Black);
            }
            
         //   pb.DrawToBitmap(bmp, rt);
            
            pb.Image = (Image)bmp;
          
        }

        public void GrayScale(PictureBox pb)
        {
            Rectangle rt = new Rectangle(0, 0, pb.Image.Width, pb.Image.Height);
            Bitmap bmp = new Bitmap(pb.Image, rt.Width, rt.Height);
            byte[, ,] rgb = new byte[rt.Height, rt.Width, 3];
        }

        public void BinaryScale(PictureBox pb)
        {
            Rectangle rt = new Rectangle(0, 0, pb.Image.Width, pb.Image.Height);
            Bitmap bmp = new Bitmap(pb.Image, rt.Width, rt.Height);
            byte[, ,] rgb = new byte[rt.Height, rt.Width, 3];
        }

        public void Thin(PictureBox pb)
        {
            Rectangle rt = new Rectangle(0, 0, pb.Image.Width, pb.Image.Height);
            Bitmap bmp = new Bitmap(pb.Image, rt.Width, rt.Height);
            byte[, ,] rgb = new byte[rt.Height, rt.Width, 3];
        }

        public void NormalScale(PictureBox pb)
        {
            Rectangle rt = new Rectangle(0, 0, pb.Image.Width, pb.Image.Height);
            Bitmap bmp = new Bitmap(pb.Image, rt.Width, rt.Height);
            byte[, ,] rgb = new byte[rt.Height, rt.Width, 3];
        }


        public void Gradation(PictureBox pb)
        {
            Rectangle rt = new Rectangle(0, 0, pb.Image.Width, pb.Image.Height);
            Bitmap bmp = new Bitmap(pb.Image, rt.Width, rt.Height);
            byte[, ,] rgb = new byte[rt.Height, rt.Width, 3];
        }

        public void ProcessEx(PictureBox pb)
        {
            Rectangle rt = new Rectangle(0, 0, pb.Image.Width, pb.Image.Height);
            Bitmap bmp = new Bitmap(pb.Image, rt.Width, rt.Height);
            byte[, ,] rgb = new byte[rt.Height, rt.Width, 3];
            int i = 0;
            for (int j = 0; j < rt.Width; j++)
            {
                for (i = 0; i < rt.Height; i++)
                {
                    rgb[i, j, 0] = bmp.GetPixel(j, i).R;
                    rgb[i, j, 1] = bmp.GetPixel(j, i).G;
                    rgb[i, j, 2] = bmp.GetPixel(j, i).B;
                    rgb[i, j, 1] = (byte)(rgb[i, j, 0] * .3 + rgb[i, j, 1] * .6 + rgb[i, j, 2] * .1);//灰度化
                }
            }
            byte[] kk = new byte[9];
            byte tt = 0;
            for (int j = 0; j < rt.Width; j++)
            {
                for (i = 0; i < rt.Height; i++)
                {
                    for (int ii = -1; ii < 2; ii++)
                    {
                        for (int jj = -1; jj < 2; jj++)
                        {
                            if (i + ii < 0 || j + jj < 0 || i + ii > rt.Height - 1 || j + jj > rt.Width - 1)
                                kk[(ii + 1) * 3 + jj + 1] = 255;
                            else
                                kk[(ii + 1) * 3 + jj + 1] = rgb[i + ii, j + jj, 1];
                        }
                    }
                    for (int ii = 0; ii < 8; ii++)
                    {
                        for (int jj = ii + 1; jj < 8; jj++)
                        {
                            if (kk[ii] < kk[jj])
                            {
                                tt = kk[ii];
                                kk[ii] = kk[jj];
                                kk[jj] = tt;
                            }
                        }
                    }
                    rgb[i, j, 0] = kk[4];
                }
            }
            int st = 0;
            int tmp = 0;
            int sum = 0;
            int k = 0;
            int num = 0;
            for (int j = 0; j < rt.Width; j++)
            {
                for (i = 0; i < rt.Height; i++)
                {
                    st = 0;
                    if (i > 0 && j > 0)
                    {
                        tmp = rgb[i - 1, j - 1, k] - rgb[i, j, k];
                        st += Math.Abs(tmp);
                    }
                    if (i > 0)
                    {
                        tmp = rgb[i - 1, j, k] - rgb[i, j, k];
                        st += Math.Abs(tmp);
                    }
                    if (i > 0 && j < rt.Width - 1)
                    {
                        tmp = rgb[i - 1, j + 1, k] - rgb[i, j, k];
                        st += Math.Abs(tmp);
                    }
                    if (j > 0)
                    {
                        tmp = rgb[i, j - 1, k] - rgb[i, j, k];
                        st += Math.Abs(tmp);
                    }
                    if (j < rt.Width - 1)
                    {
                        tmp = rgb[i, j + 1, k] - rgb[i, j, k];
                        st += Math.Abs(tmp);
                    }
                    if (i < rt.Height - 1 && j > 0)
                    {
                        tmp = rgb[i + 1, j - 1, k] - rgb[i, j, k];
                        st += Math.Abs(tmp);
                    }
                    if (i < rt.Height - 1)
                    {
                        tmp = rgb[i + 1, j, k] - rgb[i, j, k];
                        st += Math.Abs(tmp);
                    }
                    if (i < rt.Height - 1 && j < rt.Width - 1)
                    {
                        tmp = rgb[i + 1, j + 1, k] - rgb[i, j, k];
                        st += Math.Abs(tmp);
                    }
                    if (st > 7)
                    {
                        sum += st;
                        num++;
                    }
                    rgb[i, j, 1] = (byte)(st > 255 ? 255 : st);
                }
            }
            sum /= num;
            double mul=0;
            for (int j = 0; j < rt.Width; j++)
            {
                for (i = 0; i < rt.Height; i++)
                {
                    mul = (double)(rgb[i, j, 1] / sum);
                    rgb[i, j, 0] = (byte)(mul * (255 - rgb[i, j, 0]) > 255 ? 0 : 255 - mul * (255 - rgb[i, j, 0]));
                }
            }
            for (int j = 0; j < rt.Width; j++)
            {
                for (i = 0; i < rt.Height; i++)
                {
                    for (int ii = -1; ii < 2; ii++)
                    {
                        for (int jj = -1; jj < 2; jj++)
                        {
                            if (i + ii < 0 || j + jj < 0 || i + ii > rt.Height - 1 || j + jj > rt.Width - 1)
                                kk[(ii + 1) * 3 + jj + 1] = 255;
                            else
                                kk[(ii + 1) * 3 + jj + 1] = rgb[i + ii, j + jj, 0];
                        }
                    }
                    for (int ii = 0; ii < 8; ii++)
                    {
                        for (int jj = ii + 1; jj < 8; jj++)
                        {
                            if (kk[ii] < kk[jj])
                            {
                                tt = kk[ii];
                                kk[ii] = kk[jj];
                                kk[jj] = tt;
                            }
                        }
                    }
                    rgb[i, j, 1] = kk[4];
                    bmp.SetPixel(j, i, Color.FromArgb(rgb[i, j, 1], rgb[i, j, 1], rgb[i, j, 1]));
                }
            }
            byte[,] blk = new byte[rt.Height / 4, rt.Width / 4];
            for (int j = 0; j < rt.Width; j++)
            {
                for (i = 0; i < rt.Height; i++)
                {
                    if (rgb[i, j, 1] < 255)
                        blk[i / 4, j / 4]++;
                }
            }
            int area = 0;
            int n = 0;
            Rectangle rtg;
            List<Rectangle> lr = new List<Rectangle>();
            Rectangle mrt=new Rectangle();
            Graphics gg = Graphics.FromImage(bmp);
            
            Bitmap bck = new Bitmap((Image)bmp);
            for (int j = 0; j < rt.Width / 4; j++)
            {
                for (i = 0; i < rt.Height / 4; i++)
                {
                    rtg = Path(ref blk, i, j, out n);
                    if (rtg.Width * rtg.Height > area && rtg.Width * rtg.Height < n * 4 && rtg.Width < rtg.Height * 4 && rtg.Width * 4 > rtg.Height)
                    {
                        area = rtg.Width * rtg.Height;
                        mrt = rtg;
                    }
                    if (n > 0)
                        lr.Add(rtg);
                    //gg.DrawRectangle(new Pen(Color.Red),new Rectangle(rtg.Left * 4, rtg.Top * 4, rtg.Width * 4, rtg.Height * 4));
                }
            }
            int i1 = 0;
            int i2 = 0;
            int j1 = 0;
            int j2 = 0;
            for (int ii = 0; ii < lr.Count; ii++)
            {
                if (lr[ii].IntersectsWith(mrt))
                {
                    j1 = mrt.Left < lr[ii].Left ? mrt.Left : lr[ii].Left;
                    i1 = mrt.Top < lr[ii].Top ? mrt.Top : lr[ii].Top;
                    j2 = mrt.Left + mrt.Width > lr[ii].Left + lr[ii].Width ? mrt.Left + mrt.Width : lr[ii].Left + lr[ii].Width;
                    i2 = mrt.Top + mrt.Height > lr[ii].Top + lr[ii].Height ? mrt.Top + mrt.Height : lr[ii].Top + lr[ii].Height;
                    mrt = new Rectangle(j1, i1, j2 - j1, i2 - i1);
                }
            }
            mrt = new Rectangle(mrt.Left * 4, mrt.Top * 4, mrt.Width * 4, mrt.Height * 4);
            //gg.DrawRectangle(new Pen(Color.Green), mrt);
            //int grh = 40;
            //int grw = 30;
            //byte[,] dg = new byte[grh, grw];
            //Bitmap sn = new Bitmap(mrt.Width, mrt.Height);
            //bmp = new Bitmap(mrt.Width, mrt.Height);
            //Graphics g = Graphics.FromImage(sn);
            //g.DrawImage(bmp, new Rectangle(0, 0, mrt.Width, mrt.Height), mrt, GraphicsUnit.Pixel);
            pb.Image = (Image)bmp;
           
        }

        private Rectangle Path(ref byte[,] dg, int ii, int jj, out int n)
        {
            n = 0;
            if (dg[ii, jj] == 0)
                return new Rectangle(jj, ii, 0, 0);
            int grh = dg.GetUpperBound(0) + 1;
            int grw = dg.GetUpperBound(1) + 1;
            List<DSUnit> t = new List<DSUnit>();
            int i1 = grh - 1;
            int i2 = 0;
            int j1 = grw - 1;
            int j2 = 0;
            DSUnit d = new DSUnit();
            d.i = ii;
            d.j = jj;
            d.d = 0;
            t.Add(d);
            if (d.i < i1)
                i1 = d.i;
            if (d.j < j1)
                j1 = d.j;
            if (d.i > i2)
                i2 = d.i;
            if (d.j > j2)
                j2 = d.j;
            dg[d.i, d.j] = 0;
            n++;
            int p = 0;
            bool sg = false;
            while (true)
            {
                switch (t[p].d)
                {
                    case 0:
                        if (t[p].j < grw - 1 && dg[t[p].i, t[p].j + 1] > 0)
                        {
                            d = new DSUnit();
                            d.i = t[p].i;
                            d.j = t[p].j + 1;
                            d.d = 0;
                            sg = false;
                            for (int i = t.Count - 1; i >= 0; i--)
                            {
                                if (d.i == t[i].i && d.j == t[i].j)
                                {
                                    sg = true;
                                    break;
                                }
                            }
                            if (!sg)
                            {
                                if (d.i < i1)
                                    i1 = d.i;
                                if (d.j < j1)
                                    j1 = d.j;
                                if (d.i > i2)
                                    i2 = d.i;
                                if (d.j > j2)
                                    j2 = d.j;
                                dg[d.i, d.j] = 0;
                                t.Add(d);
                                n++;
                                p++;
                            }
                            else
                                goto a;
                            continue;
                        }
                        break;
                    case 1:
                        if (t[p].i < grh - 1 && dg[t[p].i + 1, t[p].j] > 0)
                        {
                            d = new DSUnit();
                            d.i = t[p].i + 1;
                            d.j = t[p].j;
                            d.d = 0;
                            sg = false;
                            for (int i = t.Count - 1; i >= 0; i--)
                            {
                                if (d.i == t[i].i && d.j == t[i].j)
                                {
                                    sg = true;
                                    break;
                                }
                            }
                            if (!sg)
                            {
                                if (d.i < i1)
                                    i1 = d.i;
                                if (d.j < j1)
                                    j1 = d.j;
                                if (d.i > i2)
                                    i2 = d.i;
                                if (d.j > j2)
                                    j2 = d.j;
                                dg[d.i, d.j] = 0;
                                t.Add(d);
                                n++;
                                p++;
                            }
                            else
                                goto a;
                            continue;
                        }
                        break;
                    case 2:
                        if (t[p].i > 0 && dg[t[p].i - 1, t[p].j] > 0)
                        {
                            d = new DSUnit();
                            d.i = t[p].i - 1;
                            d.j = t[p].j;
                            d.d = 0;
                            sg = false;
                            for (int i = t.Count - 1; i >= 0; i--)
                            {
                                if (d.i == t[i].i && d.j == t[i].j)
                                {
                                    sg = true;
                                    break;
                                }
                            }
                            if (!sg)
                            {
                                if (d.i < i1)
                                    i1 = d.i;
                                if (d.j < j1)
                                    j1 = d.j;
                                if (d.i > i2)
                                    i2 = d.i;
                                if (d.j > j2)
                                    j2 = d.j;
                                dg[d.i, d.j] = 0;
                                t.Add(d);
                                n++;
                                p++;
                            }
                            else
                                goto a;
                            continue;
                        }
                        break;
                    case 3:
                        if (t[p].j > 0 && dg[t[p].i, t[p].j - 1] > 0)
                        {
                            d = new DSUnit();
                            d.i = t[p].i;
                            d.j = t[p].j - 1;
                            d.d = 0;
                            sg = false;
                            for (int i = t.Count - 1; i >= 0; i--)
                            {
                                if (d.i == t[i].i && d.j == t[i].j)
                                {
                                    sg = true;
                                    break;
                                }
                            }
                            if (!sg)
                            {
                                if (d.i < i1)
                                    i1 = d.i;
                                if (d.j < j1)
                                    j1 = d.j;
                                if (d.i > i2)
                                    i2 = d.i;
                                if (d.j > j2)
                                    j2 = d.j;
                                dg[d.i, d.j] = 0;
                                t.Add(d);
                                n++;
                                p++;
                            }
                            else
                                goto a;
                            continue;
                        }
                        break;
                }
                a: if (t[p].d >= 3)
                {
                    t.RemoveAt(p);
                    p--;
                    if (p < 0)
                        return new Rectangle(j1, i1, j2 - j1 + 1 < 0 ? 0 : j2 - j1 + 1, i2 - i1 + 1 < 0 ? 0 : i2 - i1 + 1);
                }
                d = new DSUnit();
                d.i = t[p].i;
                d.j = t[p].j;
                d.d = (byte)(t[p].d + 1);
                t.RemoveAt(p);
                t.Add(d);
            }
        }

        private bool Check(byte[,] dg, byte[,] bdg, int i, int j)
        {
            int grh = dg.GetUpperBound(0) + 1;
            int grw = dg.GetUpperBound(1) + 1;
            bool fs = false, sc = false, rd = false, cs = false;
            if (i > 0 && j > 0 && bdg[i - 1, j - 1] == 1)
            {
                if (fs)
                    sc = true;
                if ((fs && sc && rd) || (!fs && !sc && !rd))
                    cs = true;
            }
            else
            {
                fs = true;
                if (sc)
                    rd = true;
            }
            if (i > 0 && bdg[i - 1, j] == 1)
            {
                if (fs)
                    sc = true;
                if ((fs && sc && rd) || (!fs && !sc && !rd))
                    cs = true;
            }
            else
            {
                fs = true;
                if (sc)
                    rd = true;
            }
            if (i > 0 && j < grw - 1 && bdg[i - 1, j + 1] == 1)
            {
                if (fs)
                    sc = true;
                if ((fs && sc && rd) || (!fs && !sc && !rd))
                    cs = true;
            }
            else
            {
                fs = true;
                if (sc)
                    rd = true;
            }
            if (j < grw - 1 && bdg[i, j + 1] == 1)
            {
                if (fs)
                    sc = true;
                if ((fs && sc && rd) || (!fs && !sc && !rd))
                    cs = true;
            }
            else
            {
                fs = true;
                if (sc)
                    rd = true;
            }
            if (i < grh - 1 && j < grw - 1 && bdg[i + 1, j + 1] == 1)
            {
                if (fs)
                    sc = true;
                if ((fs && sc && rd) || (!fs && !sc && !rd))
                    cs = true;
            }
            else
            {
                fs = true;
                if (sc)
                    rd = true;
            }
            if (i < grh - 1 && bdg[i + 1, j] == 1)
            {
                if (fs)
                    sc = true;
                if ((fs && sc && rd) || (!fs && !sc && !rd))
                    cs = true;
            }
            else
            {
                fs = true;
                if (sc)
                    rd = true;
            }
            if (i < grh - 1 && j > 0 && bdg[i + 1, j - 1] == 1)
            {
                if (fs)
                    sc = true;
                if ((fs && sc && rd) || (!fs && !sc && !rd))
                    cs = true;
            }
            else
            {
                fs = true;
                if (sc)
                    rd = true;
            }
            if (j > 0 && bdg[i, j - 1] == 1)
            {
                if (fs)
                    sc = true;
                if ((fs && sc && rd) || (!fs && !sc && !rd))
                    cs = true;
            }
            else
            {
                fs = true;
                if (sc)
                    rd = true;
            }
            byte tt = 0;
            if (i > 0 && dg[i - 1, j] == 1)
                tt++;
            if (j > 0 && dg[i, j - 1] == 1)
                tt++;
            if (j < grw - 1 && dg[i, j + 1] == 1)
                tt++;
            if (i < grh - 1 && dg[i + 1, j] == 1)
                tt++;
            byte btt = 0;
            if (i > 1 && bdg[i - 2, j] == 0 && dg[i - 2, j] == 0)
                btt++;
            if (i < grh - 1 && bdg[i + 1, j] == 0 && bdg[i + 1, j] == 0)
                btt++;
            byte st = 0, bg = 0, mx = 0;
            bool ch = false;
            if (i > 0 && j > 0 && dg[i - 1, j - 1] == 1)
            {
                st++;
                if (!ch)
                    bg++;
            }
            else
            {
                if (mx < st)
                    mx = st;
                st = 0;
                ch = true;
            }
            if (i > 0 && dg[i - 1, j] == 1)
            {
                st++;
                if (!ch)
                    bg++;
            }
            else
            {
                if (mx < st)
                    mx = st;
                st = 0;
                ch = true;
            }
            if (i > 0 && j < grw - 1 && dg[i - 1, j + 1] == 1)
            {
                st++;
                if (!ch)
                    bg++;
            }
            else
            {
                if (mx < st)
                    mx = st;
                st = 0;
                ch = true;
            }
            if (j < grw - 1 && dg[i, j + 1] == 1)
            {
                st++;
                if (!ch)
                    bg++;
            }
            else
            {
                if (mx < st)
                    mx = st;
                st = 0;
                ch = true;
            }
            if (i < grh - 1 && j < grw - 1 && dg[i + 1, j + 1] == 1)
            {
                st++;
                if (!ch)
                    bg++;
            }
            else
            {
                if (mx < st)
                    mx = st;
                st = 0;
                ch = true;
            }
            if (i < grh - 1 && dg[i + 1, j] == 1)
            {
                st++;
                if (!ch)
                    bg++;
            }
            else
            {
                if (mx < st)
                    mx = st;
                st = 0;
                ch = true;
            }
            if (i < grh - 1 && j > 0 && dg[i + 1, j - 1] == 1)
            {
                st++;
                if (!ch)
                    bg++;
            }
            else
            {
                if (mx < st)
                    mx = st;
                st = 0;
                ch = true;
            }
            if (j > 0 && dg[i, j - 1] == 1)
            {
                st++;
                if (!ch)
                    bg++;
            }
            else
            {
                if (mx < st)
                    mx = st;
                st = 0;
                ch = true;
            }
            if (mx < st + bg)
                mx = (byte)(st + bg);
            switch (tt)
            {
                case 0:
                    return false;
                case 1:
                    return false;
                case 2:
                    if (mx >= 3)
                        return !(rd && cs) && btt < 2;
                    else
                        return false;
                case 3:
                    if (mx >= 5)
                        return !(rd && cs) && btt < 2;
                    else
                        return false;
                case 4:
                    return false;
            }
            return false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            GrahamScan ch = new GrahamScan();
            IPositionSet ps = ch.ConvexHull(getPoints2((Bitmap)pictureBox1.Image));
            Graphics g = Graphics.FromImage(pictureBox1.Image);
            IPosition p1 = ps.GetPosition();
            IPosition p2 = ps.GetPosition();
            while (p2 != null)
            {
                g.DrawLine(new Pen(Color.Green), new PointF(p1.GetX(), p1.GetY()), new PointF(p2.GetX(), p2.GetY()));
                p1 = p2;
                p2 = ps.GetPosition();
            }
            ps.InitToTraverseSet();
            p2 = ps.GetPosition();
            g.DrawLine(new Pen(Color.Green), new PointF(p1.GetX(), p1.GetY()), new PointF(p2.GetX(), p2.GetY()));

            pictureBox1.Refresh();
        }

        private PositionSetEdit_ImplementByICollectionTemplate getPoints2(Bitmap bmp)
        {
            PositionSetEdit_ImplementByICollectionTemplate ps = new PositionSetEdit_ImplementByICollectionTemplate();
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    if (bmp.GetPixel(i, j).Name != "ffffffff")
                        ps.AddPosition(new Position_Point(i, j));
                }
            }
            return ps;
        }

        private Point[] getPoints(Bitmap bmp)
        {
            List<Point> ps = new List<Point>();
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    if (bmp.GetPixel(i, j).Name != "ffffffff")
                        ps.Add(new Point(i, j));
                }
            }
            return ps.ToArray();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ScatterPoints(50, pictureBox1);
        }
       

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Graphics g = Graphics.FromImage(pictureBox1.Image);
                g.DrawLine(new Pen(Color.Black), oldx, oldy, e.X, e.Y);
                pictureBox1.Refresh();
                oldx = e.X;
                oldy = e.Y;
               
            }

        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Graphics g = Graphics.FromImage(pictureBox1.Image);
                g.DrawLine(new Pen(Color.Black), oldx, oldy, e.X, e.Y);
                pictureBox1.Refresh();
                oldx = e.X;
                oldy = e.Y;
               
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                oldx = e.X;
                oldy = e.Y;
              
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            
            Graphics g = Graphics.FromImage(pictureBox1.Image);
            g.Clear(Color.White);
            pictureBox1.Refresh();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            QuickHull qn = new QuickHull();
            IPositionSet ps = qn.ConvexHull(getPoints2((Bitmap)pictureBox1.Image));
            Graphics g = Graphics.FromImage(pictureBox1.Image);
            IPosition p1 = ps.GetPosition();
            IPosition p2 = ps.GetPosition();
            while (p2 != null)
            {
                g.DrawLine(new Pen(Color.Green), new PointF(p1.GetX(), p1.GetY()), new PointF(p2.GetX(), p2.GetY()));
                p1 = p2;
                p2 = ps.GetPosition();
            }
            ps.InitToTraverseSet();
            p2 = ps.GetPosition();
            g.DrawLine(new Pen(Color.Green), new PointF(p1.GetX(), p1.GetY()), new PointF(p2.GetX(), p2.GetY()));

            pictureBox1.Refresh();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            TestConvexHull test = new TestConvexHull();
            test.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            TestBlockLine test = new TestBlockLine();
            test.ShowDialog();
        }
    }
}