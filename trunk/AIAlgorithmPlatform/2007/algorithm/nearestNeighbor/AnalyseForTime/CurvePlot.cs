using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZedGraph;

namespace AnalyseForTime
{
    public partial class CurvePlot : Form
    {
        private List<double[]> xData = new List<double[]>();
        private List<double[]> yData = new List<double[]>();
        String xLabel, yLabel, title;

        //设置标题
        public void setTitle(String title)
        {
            this.title = title;
            this.Text = title;
        }

        //设置X坐标标签
        public void setXLabel(String xl)
        {
            xLabel = xl;
        }

        //设置Y坐标标签
        public void setYLabel(String yl)
        {
            yLabel = yl;
        }

        //增加一条曲线
        public void addCurve(double[] xd, double[] yd)
        {
            xData.Add(xd);
            yData.Add(yd);
        }

        //清除曲线列表
        public void clearCurve()
        {
            xData.Clear();
            yData.Clear();
        }

        //绘图

        public CurvePlot()
        {
            InitializeComponent();
        }

        private void PlotForm_Load(object sender, EventArgs e)
        {
            CreateGraph(zg);
            SetSize();
        }

        private void CurvePlot_Shown(object sender, EventArgs e)
        {

        }

        private void CurvePlot_Paint(object sender, PaintEventArgs e)
        {

        }

        private void CurvePlot_Resize(object sender, EventArgs e)
        {
            SetSize();
        }

        private void SetSize()
        {
            zg.Location = new Point(10, 10);
            // Leave a small margin around the outside of the control
            zg.Size = new Size(ClientRectangle.Width - 20,
                                    ClientRectangle.Height - 20);
        }

        private void CreateGraph(ZedGraphControl zgc)
        {
            // get a reference to the GraphPane
            GraphPane myPane = zgc.GraphPane;

            // Set the Titles
            myPane.Title.Text = title;
            myPane.XAxis.Title.Text = xLabel;
            myPane.YAxis.Title.Text = yLabel;

            myPane.CurveList.Clear();

            LineItem myCurve = myPane.AddCurve("M2M",
                  xData[0], yData[0], Color.Red, SymbolType.Circle);

            // Generate a blue curve with circle
            // symbols, and "Piper" in the legend
            LineItem myCurve2 = myPane.AddCurve("KDT",
                  xData[1], yData[1], Color.Blue, SymbolType.Triangle);

            zgc.AxisChange();
        }

        public void refresh()
        {
            CreateGraph(zg);
        }

    }
}