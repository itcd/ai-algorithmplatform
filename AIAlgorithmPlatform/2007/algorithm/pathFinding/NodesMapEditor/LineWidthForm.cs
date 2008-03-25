using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NodesMapEditor
{
    public partial class LineWidthForm : Form
    {
        public LineWidthForm()
        {
            InitializeComponent();
        }

        public float NodeLineWidth
        {
            get { return (float)numericUpDown1.Value; }
            set { numericUpDown1.Value = (decimal)value; }
        }

        public float PathNodeLineWidth
        {
            get { return (float)numericUpDown2.Value; }
            set { numericUpDown2.Value = (decimal)value; }
        }

        public float SelectedNodeLineWidth
        {
            get { return (float)numericUpDown3.Value; }
            set { numericUpDown3.Value = (decimal)value; }
        }

        public float ConnectionLineWidth
        {
            get { return (float)numericUpDown4.Value; }
            set { numericUpDown4.Value = (decimal)value; }
        }

        public float PathConnectionLineWidth
        {
            get { return (float)numericUpDown5.Value; }
            set { numericUpDown5.Value = (decimal)value; }
        }
    }
}