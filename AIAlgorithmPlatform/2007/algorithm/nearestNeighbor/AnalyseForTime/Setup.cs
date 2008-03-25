using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AnalyseForTime
{
    public partial class Setup : Form
    {
        private static int b=0, p=10, n=30, sn=1000;
        private static double a=1.3;

        public Setup()
        {
            InitializeComponent();
        }

        private void Setup_Load(object sender, EventArgs e)
        {
            txtA.Text = a.ToString();
            txtB.Text = b.ToString();
            txtP.Text = p.ToString();
            txtN.Text = n.ToString();
            txtSn.Text = sn.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            a = Double.Parse(txtA.Text);
            b = Int32.Parse(txtB.Text);
            p = Int32.Parse(txtP.Text);
            n = Int32.Parse(txtN.Text);
            sn = Int32.Parse(txtSn.Text);
            this.Close();
        }

        public static double getA() { return a; }
        public static int getB() { return b; }
        public static int getP() { return p; }
        public static int getN() { return n; }
        public static int getSN() { return sn; }


    }
}