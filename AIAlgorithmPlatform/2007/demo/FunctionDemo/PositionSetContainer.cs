using System;
using System.Collections.Generic;
using System.Text;
using Position_Interface;
using System.Windows.Forms;

namespace FunctionDemo
{
    class PositionSetContainer
    {
        CurrentPositionSetDlg currentPositionSetDlg = new CurrentPositionSetDlg();
        Form parentForm = null;

        public PositionSetContainer(Form parentForm)
        {
            this.parentForm = parentForm;
        }
        
        public void AddPositionSet(IPositionSet positionSet)
        {
            ListViewItem item = currentPositionSetDlg.PositionSetListView.Items.Add("default");
            item.Tag = positionSet;
            item.SubItems.Add(positionSet.GetType().ToString());
        }

        delegate DialogResult dShowDialog();
        public IPositionSet GetPositionSet()
        {
            //if (currentPositionSetDlg.InvokeRequired)
            //{
            IAsyncResult result = parentForm.BeginInvoke(new dShowDialog(currentPositionSetDlg.ShowDialog), new object[] { });
            parentForm.EndInvoke(result);
                return currentPositionSetDlg.SelectedPositionSet;
            //}
            //else
            //{
            //    currentPositionSetDlg.ShowDialog();
            //    return currentPositionSetDlg.SelectedPositionSet;
            //}
        }

        public void ShowCurrentPositionSetDlg()
        {
            currentPositionSetDlg.ShowDialog();
        }
    }
}
