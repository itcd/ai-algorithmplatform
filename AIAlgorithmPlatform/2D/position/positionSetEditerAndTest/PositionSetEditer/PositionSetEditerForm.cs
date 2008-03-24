using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PositionSetViewer;
using Position_Interface;
using Configuration;

namespace PositionSetEditer
{
    public partial class LayersEditerForm : Form
    {
        public LayersEditerForm(LayersExOptDlg layers)
            //: base(layers)
        {
            this.layers = layers;
            InitializeComponent();

            //IsMdiContainer = true;

            ////Create ToolStripPanel controls.
            //ToolStripPanel tspTop = new ToolStripPanel();
            //ToolStripPanel tspBottom = new ToolStripPanel();
            //ToolStripPanel tspLeft = new ToolStripPanel();
            //ToolStripPanel tspRight = new ToolStripPanel();

            //// Dock the ToolStripPanel controls to the edges of the form.
            //tspTop.Dock = DockStyle.Top;
            //tspBottom.Dock = DockStyle.Bottom;
            //tspLeft.Dock = DockStyle.Left;
            //tspRight.Dock = DockStyle.Right;

            ////Create ToolStrip
            //ToolStrip tsTop = new ToolStrip();
            //tsTop.Items.Add("TestToolBar");

            ////add ToolStrip to ToolStripPanel
            ////tspTop.Join(tsTop);
            //tspTop.Join(toolStrip1);

            ////add ToolStripPanel to the form
            //this.Controls.Add(tspRight);
            //this.Controls.Add(tspLeft);
            //this.Controls.Add(tspBottom);
            //this.Controls.Add(tspTop);

            ////Create StatusStrip
            //StatusStrip ssBottom = new StatusStrip();
            //ssBottom.Items.Add(DateTime.Now.ToShortDateString());
            //ssBottom.Items.Add(" ");
            //ssBottom.Items.Add("User:TestUser");
            //this.Controls.Add(ssBottom);

        }

        LayersExOptDlg layers;
        LayersPainterForm layersPainterForm;

        delegate void dGetPosition(IPosition position);
        event dGetPosition MouseCheckRealPosition;

        private void PositionSetEditerForm_Load(object sender, EventArgs e)
        {
            layersPainterForm = new LayersPainterForm(layers);
            layersPainterForm.MdiParent = this;
            layersPainterForm.WindowState = FormWindowState.Maximized;
            layersPainterForm.Show();
            //layersPainterForm

            //this.PositionSetEditedControl.LayersPainterForm = layersPainterForm;

            //layersPainterForm.MouseMove += new MouseEventHandler(OnLayersPainterFormMouseMove);
            //layersPainterForm.MouseDoubleClick += OnLayersPainterFormMouseDown;
            //this.Invalidated += delegate { layersPainterForm.Invalidate(); };
            //PainterForm painterForm2 = new PainterForm(layersPainter);
            //painterForm2.MdiParent = this;
            //painterForm2.Show();
        }

        public void InvalidateCurrentPainterForm()
        {
            layersPainterForm.Invalidate();
        }

        //public void OnLayersPainterFormMouseMove(object sender, MouseEventArgs e)
        //{
        //    this.PositionCoordinateLabel.Text = "(" + layersPainterForm.ConvertMouseXToRealX(e.x).ToString()
        //        + ", " + layersPainterForm.ConvertMouseYToRealY(e.y).ToString() + ")";
        //}
        
        //public void OnLayersPainterFormMouseDown(object sender, MouseEventArgs e)
        //{
        //    if (MouseCheckRealPosition != null)
        //    {
        //        Position_Point mouseChickedRealPosition = new Position_Point();
        //        mouseChickedRealPosition.SetX(layersPainterForm.ConvertMouseXToRealX(e.x));
        //        mouseChickedRealPosition.SetY(layersPainterForm.ConvertMouseYToRealY(e.y));
        //        if (MouseCheckRealPosition != null)
        //        {
        //            MouseCheckRealPosition(mouseChickedRealPosition);
        //        }
        //    }
        //}

        delegate IPosition dGetMouseChickedRealPosition();
        public IPosition GetMouseChickedRealPosition()
        {
            if (this.InvokeRequired)
            {                
                IPosition chickedPosition = null;
                System.Threading.Thread currentThread = System.Threading.Thread.CurrentThread;
                MouseCheckRealPosition += delegate(IPosition position)
                {
                    chickedPosition = position;
                    if ((currentThread.ThreadState & System.Threading.ThreadState.Suspended) == System.Threading.ThreadState.Suspended)
                    {
                        currentThread.Resume();
                    }
                };
                currentThread.Suspend();
                MouseCheckRealPosition = null;
                return chickedPosition;
            }
            else
            {
                IAsyncResult result = new dGetMouseChickedRealPosition(GetMouseChickedRealPosition).BeginInvoke(null, null);
                return (IPosition)this.EndInvoke(result);
            }
        }
    }
}