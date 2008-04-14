using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace PositionSetViewer
{
    public partial class ColorSelect : UserControl
    {
        public ColorSelect()
        {
            InitializeComponent();
        }

        private void ColorSelect_MouseDown(object sender, MouseEventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = this.BackColor;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                this.BackColor = dlg.Color;
            }
        }
    }
}
