using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace PositionSetViewer
{
    public delegate void Update();

    /// <summary>
    /// �����õ���չPositionPainter
    /// </summary>
    public class LayersExOptDlg : Layers
    {
        public LayersOptionDialog painterOptionDialog;

        void SetDlgNull(object sender, FormClosedEventArgs e)
        {
            painterOptionDialog = null;
        }

        /// <summary>
        /// ��ʾPainterĬ�ϵ����öԻ���
        /// </summary>
        /// <returns>DialogResult</returns>
        public void ShowOptionDialog(Update updata)
        {
            if(painterOptionDialog == null)
            {
                painterOptionDialog = new LayersOptionDialog(this, updata);
                painterOptionDialog.FormClosed += SetDlgNull;
            }

            painterOptionDialog.Show();
            painterOptionDialog.Activate();
        }

        public void HideOptionDialog()
        {
            painterOptionDialog.Hide();
        }

        public void CloseOptionDialog()
        {
            if (painterOptionDialog != null)
            {
                painterOptionDialog.Close();
                painterOptionDialog = null;
            }
        }
    }
}
