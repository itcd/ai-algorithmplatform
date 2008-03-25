using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PositionSetViewer;

namespace PositionSetEditer
{
    public partial class LayersEditedForm : PositionSetViewer.LayersPainterForm
    {
        public LayersEditedForm(LayersExOptDlg layersExOptDlg)
            : base(layersExOptDlg)
        {
            InitializeComponent();
        }
    }
}

