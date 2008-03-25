namespace AlgorithmDemo
{
    partial class FlowControlerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FlowControlerForm));
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.AutoStepBotton = new System.Windows.Forms.Button();
            this.BreakAllBotton = new System.Windows.Forms.Button();
            this.NextStepBotton = new System.Windows.Forms.Button();
            this.DropDownBotton = new System.Windows.Forms.Button();
            this.DemoRateTrackBar = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.DemoRateTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // propertyGrid
            // 
            this.propertyGrid.HelpVisible = false;
            this.propertyGrid.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.propertyGrid.Location = new System.Drawing.Point(1, 36);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.propertyGrid.Size = new System.Drawing.Size(310, 118);
            this.propertyGrid.TabIndex = 0;
            this.propertyGrid.ToolbarVisible = false;
            this.propertyGrid.Click += new System.EventHandler(this.propertyGrid_Click);
            this.propertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid_PropertyValueChanged);
            // 
            // AutoStepBotton
            // 
            this.AutoStepBotton.Enabled = false;
            this.AutoStepBotton.Location = new System.Drawing.Point(63, 6);
            this.AutoStepBotton.Name = "AutoStepBotton";
            this.AutoStepBotton.Size = new System.Drawing.Size(37, 23);
            this.AutoStepBotton.TabIndex = 5;
            this.AutoStepBotton.Text = ">>";
            this.AutoStepBotton.UseVisualStyleBackColor = true;
            this.AutoStepBotton.Click += new System.EventHandler(this.AutoStepBotton_Click);
            // 
            // BreakAllBotton
            // 
            this.BreakAllBotton.Enabled = false;
            this.BreakAllBotton.Location = new System.Drawing.Point(122, 6);
            this.BreakAllBotton.Name = "BreakAllBotton";
            this.BreakAllBotton.Size = new System.Drawing.Size(37, 23);
            this.BreakAllBotton.TabIndex = 4;
            this.BreakAllBotton.Text = "¡ö";
            this.BreakAllBotton.UseVisualStyleBackColor = true;
            this.BreakAllBotton.Click += new System.EventHandler(this.BreakAllBotton_Click);
            // 
            // NextStepBotton
            // 
            this.NextStepBotton.Enabled = false;
            this.NextStepBotton.Location = new System.Drawing.Point(7, 6);
            this.NextStepBotton.Name = "NextStepBotton";
            this.NextStepBotton.Size = new System.Drawing.Size(37, 23);
            this.NextStepBotton.TabIndex = 3;
            this.NextStepBotton.Text = ">";
            this.NextStepBotton.UseVisualStyleBackColor = true;
            this.NextStepBotton.Click += new System.EventHandler(this.NextStepBotton_Click);
            // 
            // DropDownBotton
            // 
            this.DropDownBotton.FlatAppearance.BorderSize = 0;
            this.DropDownBotton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.MintCream;
            this.DropDownBotton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.DropDownBotton.Image = ((System.Drawing.Image)(resources.GetObject("DropDownBotton.Image")));
            this.DropDownBotton.Location = new System.Drawing.Point(296, 18);
            this.DropDownBotton.Name = "DropDownBotton";
            this.DropDownBotton.Size = new System.Drawing.Size(15, 15);
            this.DropDownBotton.TabIndex = 6;
            this.DropDownBotton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.DropDownBotton.UseVisualStyleBackColor = true;
            this.DropDownBotton.Click += new System.EventHandler(this.DropDownBotton_Click);
            // 
            // DemoRateTrackBar
            // 
            this.DemoRateTrackBar.LargeChange = 20;
            this.DemoRateTrackBar.Location = new System.Drawing.Point(169, 3);
            this.DemoRateTrackBar.Maximum = 80;
            this.DemoRateTrackBar.Minimum = 1;
            this.DemoRateTrackBar.Name = "DemoRateTrackBar";
            this.DemoRateTrackBar.Size = new System.Drawing.Size(123, 42);
            this.DemoRateTrackBar.TabIndex = 7;
            this.DemoRateTrackBar.TickFrequency = 5;
            this.DemoRateTrackBar.Value = 1;
            this.DemoRateTrackBar.Scroll += new System.EventHandler(this.DemoRateTrackBar_Scroll);
            // 
            // FlowControlerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(312, 155);
            this.Controls.Add(this.DropDownBotton);
            this.Controls.Add(this.AutoStepBotton);
            this.Controls.Add(this.BreakAllBotton);
            this.Controls.Add(this.NextStepBotton);
            this.Controls.Add(this.propertyGrid);
            this.Controls.Add(this.DemoRateTrackBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FlowControlerForm";
            this.Text = "FlowControl";
            this.Load += new System.EventHandler(this.FlowControler_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DemoRateTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.Button AutoStepBotton;
        private System.Windows.Forms.Button BreakAllBotton;
        private System.Windows.Forms.Button NextStepBotton;
        private System.Windows.Forms.Button DropDownBotton;
        private System.Windows.Forms.TrackBar DemoRateTrackBar;
    }
}