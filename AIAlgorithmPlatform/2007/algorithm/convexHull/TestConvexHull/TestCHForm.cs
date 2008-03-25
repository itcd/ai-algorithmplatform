using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Position_Interface;
using PositionSetViewer;
using ConvexHullEngine;
using GrahamScanAlgorithm;
using QuickHullAlgorithm;
using JarvisMatchAlgorithm;
using M2M;
using Position_Implement;

namespace TestConvexHull
{
    public partial class TestCHForm : Form
    {
        TestCaseList testCaseList = new TestCaseList();
        IConvexHullEngine chEngine = new JarvisMatch();

        public TestCHForm()
        {
            InitializeComponent();
            listBox1.Items.Clear();
            for (int i = 0; i < testCaseList.GetCount(); i++)
                listBox1.Items.Add(testCaseList.GetName(i));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IPositionSet ps = new RandomPositionSet_Square(int.Parse(textForPointNumber.Text), 50, 500);
            LanchCHTest.lanch(chEngine, ps);
            //LanchConvexHull.lanch(new GrahamScan(), 100, 50, 500);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int i = listBox1.SelectedIndex;
            if (i >= 0)
                LanchCHTest.lanch(chEngine, testCaseList.GetPositionSet(i));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LanchCHTest.lanch(chEngine, testCaseList.GetPositionSetList());
        }
    }
}