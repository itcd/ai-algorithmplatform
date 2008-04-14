using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PositionSetViewer
{
    public partial class LayersPainterForm : Form
    {
        public LayersPainterForm()
        {
            InitializeComponent();
        }

        LayersPaintedControl layersPaintedControl = null;
        public LayersPaintedControl LayersPaintedControl
        {
            get { return layersPaintedControl; }
            set { layersPaintedControl = value; }
        }

        public LayersPainterForm(LayersExOptDlg layersExOptDlg)
        {
            layersPaintedControl = new LayersPaintedControl();
            layersPaintedControl.Layers = layersExOptDlg;
            layersPaintedControl.Dock = DockStyle.Fill;
            this.Controls.Add(layersPaintedControl);
            InitializeComponent();
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {            
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        private void PainterForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}