using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Position_Interface;

namespace FunctionDemo
{
    public partial class CurrentPositionSetDlg : Form
    {
        public CurrentPositionSetDlg()
        {
            InitializeComponent();
        }

        private void CurrentPositionSetDlg_Load(object sender, EventArgs e)
        {
        }

        private IPositionSet selectedPositionSet = null;
        public IPositionSet SelectedPositionSet
        {
            get { return selectedPositionSet; }
        }

        private void GetPositionSetBotton_Click(object sender, EventArgs e)
        {
            if (this.PositionSetListView.SelectedItems.Count == 0)
            {
                selectedPositionSet = null;
            }
            else
            {
                selectedPositionSet = (IPositionSet)(this.PositionSetListView.SelectedItems[0].Tag);
            }
            this.Close();
        }
    }
}