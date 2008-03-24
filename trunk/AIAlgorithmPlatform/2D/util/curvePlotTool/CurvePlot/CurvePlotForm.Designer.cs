namespace CurvePlotDrawer
{
    partial class CurvePlotForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.graph = new ZedGraph.ZedGraphControl();
            this.SuspendLayout();
            // 
            // graph
            // 
            this.graph.BackColor = System.Drawing.SystemColors.Highlight;
            this.graph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.graph.EditButtons = System.Windows.Forms.MouseButtons.Left;
            this.graph.EditModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.None)));
            this.graph.ForeColor = System.Drawing.Color.Coral;
            this.graph.IsAutoScrollRange = true;
            this.graph.IsEnableHEdit = false;
            this.graph.IsEnableHPan = true;
            this.graph.IsEnableHZoom = true;
            this.graph.IsEnableVEdit = true;
            this.graph.IsEnableVPan = true;
            this.graph.IsEnableVZoom = true;
            this.graph.IsPrintFillPage = true;
            this.graph.IsPrintKeepAspectRatio = true;
            this.graph.IsScrollY2 = false;
            this.graph.IsShowContextMenu = true;
            this.graph.IsShowCopyMessage = true;
            this.graph.IsShowCursorValues = false;
            this.graph.IsShowHScrollBar = false;
            this.graph.IsShowPointValues = false;
            this.graph.IsShowVScrollBar = false;
            this.graph.IsSynchronizeXAxes = false;
            this.graph.IsSynchronizeYAxes = false;
            this.graph.IsZoomOnMouseCenter = false;
            this.graph.LinkButtons = System.Windows.Forms.MouseButtons.Left;
            this.graph.LinkModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.None)));
            this.graph.Location = new System.Drawing.Point(0, 0);
            this.graph.Name = "graph";
            this.graph.PanButtons = System.Windows.Forms.MouseButtons.Left;
            this.graph.PanButtons2 = System.Windows.Forms.MouseButtons.Middle;
            this.graph.PanModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)));
            this.graph.PanModifierKeys2 = System.Windows.Forms.Keys.None;
            this.graph.PointDateFormat = "g";
            this.graph.PointValueFormat = "G";
            this.graph.ScrollMaxX = 0;
            this.graph.ScrollMaxY = 0;
            this.graph.ScrollMaxY2 = 0;
            this.graph.ScrollMinX = 0;
            this.graph.ScrollMinY = 0;
            this.graph.ScrollMinY2 = 0;
            this.graph.Size = new System.Drawing.Size(666, 263);
            this.graph.TabIndex = 3;
            this.graph.ZoomButtons = System.Windows.Forms.MouseButtons.Left;
            this.graph.ZoomButtons2 = System.Windows.Forms.MouseButtons.None;
            this.graph.ZoomModifierKeys = System.Windows.Forms.Keys.None;
            this.graph.ZoomModifierKeys2 = System.Windows.Forms.Keys.None;
            this.graph.ZoomStepFraction = 0.1;
            this.graph.Load += new System.EventHandler(this.graph_Load);
            // 
            // CurvePlotForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(666, 263);
            this.Controls.Add(this.graph);
            this.Name = "CurvePlotForm";
            this.Text = "Form1";
            this.SizeChanged += new System.EventHandler(this.CurvePlot_Resize);
            this.Load += new System.EventHandler(this.PlotForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ZedGraph.ZedGraphControl graph;
    }
}