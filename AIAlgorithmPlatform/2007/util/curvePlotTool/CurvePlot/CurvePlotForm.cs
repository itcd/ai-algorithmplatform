using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZedGraph;

namespace CurvePlotDrawer
{
    public partial class CurvePlotForm : Form
    {
        String xLabel, yLabel, title;

        List<Curve> curves = new List<Curve>();

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
        public void addCurve(String label, double[] xd, double[] yd)
        {
            curves.Add(new Curve(label, xd, yd));
        }

        public CurvePlotForm()
        {
            InitializeComponent();
        }

        private void PlotForm_Load(object sender, EventArgs e)
        {
            CreateGraph(graph);
        }

        private void CurvePlot_Resize(object sender, EventArgs e)
        {
            SetSize();
        }

        private void SetSize()
        {
            graph.Location = new Point(10, 10);
            graph.Size = new Size(ClientRectangle.Width - 20,
                                    ClientRectangle.Height - 20);
        }

        private void CreateGraph(ZedGraphControl zgc)
        {
            GraphPane myPane = zgc.GraphPane;

            // Set the Titles
            myPane.Title.Text = title;
            myPane.XAxis.Title.Text = xLabel;
            myPane.YAxis.Title.Text = yLabel;

            Color[] colors = { Color.Red, Color.Blue, Color.Black, Color.Green, Color.Purple, Color.YellowGreen };
            SymbolType[] symbols = { SymbolType.Circle, SymbolType.Diamond, SymbolType.Star, SymbolType.Triangle, SymbolType.Square, SymbolType.XCross };
            myPane.CurveList.Clear();
            for (int i = 0; i < curves.Count; i++)
            {
                Curve curve = curves[i];
                myPane.AddCurve(curve.label, curve.xData, curve.yData, colors[i % colors.Length], symbols[i % symbols.Length]);
            }
            SetSize();
            zgc.AxisChange();
        }

        private void graph_Load(object sender, EventArgs e)
        {

        }

    }

    public class Curve
    {
        public String label;
        public double[] xData, yData;
        public Curve(String label, double[] xData, double[] yData)
        {
            this.label = label;
            this.xData = xData;
            this.yData = yData;
        }
    }
}