using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TestConvexHull
{
    public partial class CHTestAndReport : Form
    {
        public string content;

        public CHTestAndReport()
        {
            InitializeComponent();
        }

        private void AnalyzeReport_Load(object sender, EventArgs e)
        {
            textBox1.Text = content;
        }


    }
}