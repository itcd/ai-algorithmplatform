using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TestConvexHull
{
    public partial class AnalyzeReport : Form
    {
        public string content;

        public AnalyzeReport()
        {
            InitializeComponent();
        }

        private void AnalyzeReport_Load(object sender, EventArgs e)
        {
            textBox1.Text = content;
        }


    }
}