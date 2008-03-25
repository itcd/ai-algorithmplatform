using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Position;
using PositionSetViewer;
using ConvexHullEngine;
using GrahamScanAlgorithm;
using QuickHullAlgorithm;
using JarvisMatchAlgorithm;
using M2M;

namespace TestConvexHull
{
    public partial class TestCHForm : Form
    {
        public TestCHForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LanchConvexHull.lanch(new M2M_CH(), int.Parse(textForPointNumber.Text), 0, 500);
            //LanchConvexHull.lanch(new QuickHull(), int.Parse(textForPointNumber.Text), 50, 500);
            //LanchConvexHull.lanch(new JarvisMatch(), int.Parse(textForPointNumber.Text), 50, 500);
            //LanchConvexHull.lanch(new GrahamScan(), 100, 50, 500);
        }
    }
}