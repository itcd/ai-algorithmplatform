using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NodesMapEditor
{
    public partial class PositionInfoForm : Form
    {
        public PositionInfoForm()
        {
            InitializeComponent();
        }

        public void SetText(string s)
        {
            textBox1.Text = s;
        }

        private void PositionInfoForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}