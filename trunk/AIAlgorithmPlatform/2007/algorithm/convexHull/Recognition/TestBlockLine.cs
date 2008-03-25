using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Position_Interface;
using BlockLineAlgorithm;

namespace Recognition
{
    public partial class TestBlockLine : Form
    {
        public TestBlockLine()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            

        }

        private void TestBlockLine_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //取得测试数据
            float rx = float.Parse(txtRX.Text);
            float ry = float.Parse(txtRY.Text);
            float sx = float.Parse(txtSX.Text);
            float sy = float.Parse(txtSY.Text);
            float ex = float.Parse(txtEX.Text);
            float ey = float.Parse(txtEY.Text);
            float itv = float.Parse(txtItv.Text);

            //求解
            Position_Point rp = new Position_Point(rx, ry);
            Position_Point sp = new Position_Point(sx, sy);
            Position_Point ep = new Position_Point(ex, ey);
            IPositionSet ps = BlockLine.getBlockLine(rp, itv, sp, ep);

            //作图
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(pictureBox1.Image);
            g.Clear(Color.White);
            //网格线
            for (float x = rx - itv; x >= 0; x -= itv)
                g.DrawLine(Pens.Black, x, 0, x, pictureBox1.Height);
            for (float y = ry - itv; y >= 0; y -= itv)
                g.DrawLine(Pens.Black, 0, y, pictureBox1.Width, y);
            for (float x = rx; x < pictureBox1.Width; x += itv)
                g.DrawLine(Pens.Black, x, 0, x, pictureBox1.Height);
            for (float y = ry; y < pictureBox1.Width; y += itv)
                g.DrawLine(Pens.Black, 0, y, pictureBox1.Width, y);
            //经过的网格
            ps.InitToTraverseSet();
            while (ps.NextPosition())
            {
                IPosition p = ps.GetPosition();
                float x = p.GetX();
                float y = p.GetY();
                g.FillRectangle(Brushes.Gray, x - itv / 2 + 1, y - itv / 2 + 1, itv - 1, itv - 1);
            }
            //起点和终点
            g.FillEllipse(Brushes.Black, sx - 2, sy - 2, 4, 4);
            g.FillEllipse(Brushes.Black, ex - 2, ey - 2, 4, 4);
            g.DrawLine(Pens.Red, sx, sy, ex, ey);
        }
    }
}