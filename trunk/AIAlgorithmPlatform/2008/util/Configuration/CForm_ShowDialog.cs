using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Configuration
{
    public partial class CForm_ShowDialog : Form
    {
        public CForm_ShowDialog()
        {
            InitializeComponent();
        }

        public void SelectConfiguratoinObject(object o)
        {
            propertyGrid.SelectedObject = o;
        }

        private void CForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void CForm_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void propertyGrid_Click(object sender, EventArgs e)
        {
        }

        private void CForm_ShowDialog_Load(object sender, EventArgs e)
        {

        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}