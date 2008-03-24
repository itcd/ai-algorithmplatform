namespace AnalyseForTime
{
    partial class CurvePlot
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.zg = new ZedGraph.ZedGraphControl();
            this.SuspendLayout();
            // 
            // zg
            // 
            this.zg.BackColor = System.Drawing.SystemColors.Highlight;
            this.zg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zg.EditButtons = System.Windows.Forms.MouseButtons.Left;
            this.zg.EditModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.None)));
            this.zg.ForeColor = System.Drawing.Color.Coral;
            this.zg.IsAutoScrollRange = false;
            this.zg.IsEnableHEdit = false;
            this.zg.IsEnableHPan = true;
            this.zg.IsEnableHZoom = true;
            this.zg.IsEnableVEdit = false;
            this.zg.IsEnableVPan = true;
            this.zg.IsEnableVZoom = true;
            this.zg.IsPrintFillPage = true;
            this.zg.IsPrintKeepAspectRatio = true;
            this.zg.IsScrollY2 = false;
            this.zg.IsShowContextMenu = true;
            this.zg.IsShowCopyMessage = true;
            this.zg.IsShowCursorValues = false;
            this.zg.IsShowHScrollBar = false;
            this.zg.IsShowPointValues = false;
            this.zg.IsShowVScrollBar = false;
            this.zg.IsSynchronizeXAxes = false;
            this.zg.IsSynchronizeYAxes = false;
            this.zg.IsZoomOnMouseCenter = false;
            this.zg.LinkButtons = System.Windows.Forms.MouseButtons.Left;
            this.zg.LinkModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.None)));
            this.zg.Location = new System.Drawing.Point(0, 0);
            this.zg.Name = "zg";
            this.zg.PanButtons = System.Windows.Forms.MouseButtons.Left;
            this.zg.PanButtons2 = System.Windows.Forms.MouseButtons.Middle;
            this.zg.PanModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)));
            this.zg.PanModifierKeys2 = System.Windows.Forms.Keys.None;
            this.zg.PointDateFormat = "g";
            this.zg.PointValueFormat = "G";
            this.zg.ScrollMaxX = 0;
            this.zg.ScrollMaxY = 0;
            this.zg.ScrollMaxY2 = 0;
            this.zg.ScrollMinX = 0;
            this.zg.ScrollMinY = 0;
            this.zg.ScrollMinY2 = 0;
            this.zg.Size = new System.Drawing.Size(399, 326);
            this.zg.TabIndex = 2;
            this.zg.ZoomButtons = System.Windows.Forms.MouseButtons.Left;
            this.zg.ZoomButtons2 = System.Windows.Forms.MouseButtons.None;
            this.zg.ZoomModifierKeys = System.Windows.Forms.Keys.None;
            this.zg.ZoomModifierKeys2 = System.Windows.Forms.Keys.None;
            this.zg.ZoomStepFraction = 0.1;
            // 
            // CurvePlot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(399, 326);
            this.Controls.Add(this.zg);
            this.Name = "CurvePlot";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.CurvePlot_Paint);
            this.Resize += new System.EventHandler(this.CurvePlot_Resize);
            this.Shown += new System.EventHandler(this.CurvePlot_Shown);
            this.Load += new System.EventHandler(this.PlotForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ZedGraph.ZedGraphControl zg;

    }
}