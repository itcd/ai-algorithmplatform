using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CurvePlotDrawer
{
    public partial class SequenceSetupForm : Form
    {
        public SequenceSetupForm()
        {
            InitializeComponent();
        }

        private void SequenceSetupForm_Load(object sender, EventArgs e)
        {

        }

        public void SelectSequenceGener(SequenceGener sequenceGener)
        {
            propertyGrid.SelectedObject = sequenceGener;
        }

        private void propertyGrid1_Click(object sender, EventArgs e)
        {

        }
    }
}