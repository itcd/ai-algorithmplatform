using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NodesMapEditor
{
    public partial class InputForm : Form
    {
        public InputForm()
        {
            InitializeComponent();
        }

        public int TotalAmount
        {
            get { return (int)numericUpDown1.Value; }
            set { numericUpDown1.Value = value; }
        }

        public int Amount1
        {
            get { return (int)numericUpDown5.Value; }
            set { numericUpDown5.Value = value; }
        }

        public int Amount2
        {
            get { return (int)numericUpDown6.Value; }
            set { numericUpDown6.Value = value; }
        }

        public int Amount3
        {
            get { return (int)numericUpDown7.Value; }
            set { numericUpDown7.Value = value; }
        }

        public double Probability1
        {
            get { return (double)numericUpDown2.Value; }
            set { numericUpDown2.Value = (decimal)value; }
        }

        public double Probability2
        {
            get { return (double)numericUpDown3.Value; }
            set { numericUpDown3.Value = (decimal)value; }
        }

        public double Probability3
        {
            get { return (double)numericUpDown4.Value; }
            set { numericUpDown4.Value = (decimal)value; }
        }
    }
}