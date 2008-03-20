using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Configuration
{
    public partial class CForm_Show : Form
    {
        dUpdate update;

        public CForm_Show(dUpdate update)
        {
            InitializeComponent();
            this.update = update;
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            update();
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

        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            update();
        }
    }
}