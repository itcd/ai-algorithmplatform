using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using CurvePlotDrawer;

namespace TestForTimeAnalyze
{
    public partial class LanchTest : Form
    {
        public LanchTest()
        {
            InitializeComponent();
        }

        double f1(double x) { return x; }
        double f2(double x) { return 2 * x; }
        void proc1(double x)
        {
            for (int i = 0; i < x * x; i++);
        }
        void proc2(double x)
        {
            for (int i = 0; i < 100 * x; i++);
        }

        private void LanchTest_Load(object sender, EventArgs e)
        {
            test2();
            test3();
        }


        //测试画函数图
        void test2()
        {
            CurvePlot_Function curvePlot = new CurvePlot_Function("hello", "x", "y", false);
            curvePlot.add("func1", f1);
            curvePlot.add("func2", f2);
            curvePlot.draw(SequenceMaker.GenerateData_Linear(0, 10000, 10));
        }

        //测试计时并作图
        void test3()
        {
            CurvePlot_Function_Time curvePlot = new CurvePlot_Function_Time("hello", "x", "y", false);
            curvePlot.addProc("proc1", proc1);
            curvePlot.addProc("proc2", proc2);
            //curvePlot.draw(SequenceMaker.genXData_Exp(2, 0, 10));
            curvePlot.draw(SequenceMaker.GenerateData_Multiple());
        }
    }
}