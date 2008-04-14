namespace PositionSetEditer
{
    partial class LayersEditedControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LayersEditedControl));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.editedBotton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.newButton = new System.Windows.Forms.ToolStripButton();
            this.openButton = new System.Windows.Forms.ToolStripButton();
            this.saveButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.daggleButton = new System.Windows.Forms.ToolStripButton();
            this.selectBotton = new System.Windows.Forms.ToolStripButton();
            this.addNodeButton = new System.Windows.Forms.ToolStripButton();
            this.removeNodeButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.addConnectionButton = new System.Windows.Forms.ToolStripButton();
            this.addDoubleConnectionButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.positionCoordinateLabel = new System.Windows.Forms.ToolStripLabel();
            this.saveMapDialog = new System.Windows.Forms.SaveFileDialog();
            this.loadMapDialog = new System.Windows.Forms.OpenFileDialog();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editedBotton,
            this.toolStripSeparator,
            this.newButton,
            this.openButton,
            this.saveButton,
            this.toolStripSeparator1,
            this.daggleButton,
            this.selectBotton,
            this.addNodeButton,
            this.removeNodeButton,
            this.toolStripSeparator2,
            this.addConnectionButton,
            this.addDoubleConnectionButton,
            this.toolStripSeparator3,
            this.positionCoordinateLabel});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(500, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip";
            // 
            // editedBotton
            // 
            this.editedBotton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.editedBotton.Image = ((System.Drawing.Image)(resources.GetObject("editedBotton.Image")));
            this.editedBotton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.editedBotton.Name = "editedBotton";
            this.editedBotton.Size = new System.Drawing.Size(32, 22);
            this.editedBotton.Text = "Edit";
            this.editedBotton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.editedBotton.Click += new System.EventHandler(this.EditedBotton_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // newButton
            // 
            this.newButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.newButton.Image = ((System.Drawing.Image)(resources.GetObject("newButton.Image")));
            this.newButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newButton.Name = "newButton";
            this.newButton.Size = new System.Drawing.Size(23, 22);
            this.newButton.Text = "&New";
            this.newButton.Click += new System.EventHandler(this.newButton_Click);
            // 
            // openButton
            // 
            this.openButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.openButton.Image = ((System.Drawing.Image)(resources.GetObject("openButton.Image")));
            this.openButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(23, 22);
            this.openButton.Text = "&Open";
            this.openButton.Click += new System.EventHandler(this.openButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveButton.Image = ((System.Drawing.Image)(resources.GetObject("saveButton.Image")));
            this.saveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(23, 22);
            this.saveButton.Text = "&Save";
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // daggleButton
            // 
            this.daggleButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.daggleButton.Image = ((System.Drawing.Image)(resources.GetObject("daggleButton.Image")));
            this.daggleButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.daggleButton.Name = "daggleButton";
            this.daggleButton.Size = new System.Drawing.Size(23, 22);
            this.daggleButton.Text = "Daggle Screen";
            this.daggleButton.Click += new System.EventHandler(this.DaggleButton_Click);
            // 
            // selectBotton
            // 
            this.selectBotton.BackColor = System.Drawing.Color.Transparent;
            this.selectBotton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.selectBotton.Image = ((System.Drawing.Image)(resources.GetObject("selectBotton.Image")));
            this.selectBotton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.selectBotton.Name = "selectBotton";
            this.selectBotton.Size = new System.Drawing.Size(23, 22);
            this.selectBotton.Text = "Select";
            this.selectBotton.Click += new System.EventHandler(this.SelectBotton_Click);
            // 
            // addNodeButton
            // 
            this.addNodeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.addNodeButton.Image = ((System.Drawing.Image)(resources.GetObject("addNodeButton.Image")));
            this.addNodeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addNodeButton.Name = "addNodeButton";
            this.addNodeButton.Size = new System.Drawing.Size(23, 22);
            this.addNodeButton.Text = "Add Node";
            this.addNodeButton.Click += new System.EventHandler(this.AddNodeButton_Click);
            // 
            // removeNodeButton
            // 
            this.removeNodeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.removeNodeButton.Image = ((System.Drawing.Image)(resources.GetObject("removeNodeButton.Image")));
            this.removeNodeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.removeNodeButton.Name = "removeNodeButton";
            this.removeNodeButton.Size = new System.Drawing.Size(23, 22);
            this.removeNodeButton.Text = "Remove Node";
            this.removeNodeButton.Click += new System.EventHandler(this.RemoveNodeButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // addConnectionButton
            // 
            this.addConnectionButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.addConnectionButton.Image = ((System.Drawing.Image)(resources.GetObject("addConnectionButton.Image")));
            this.addConnectionButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addConnectionButton.Name = "addConnectionButton";
            this.addConnectionButton.Size = new System.Drawing.Size(23, 22);
            this.addConnectionButton.Text = "toolStripButton9";
            this.addConnectionButton.ToolTipText = "Add Single Connection";
            this.addConnectionButton.Click += new System.EventHandler(this.AddConnectionButton_Click);
            // 
            // addDoubleConnectionButton
            // 
            this.addDoubleConnectionButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.addDoubleConnectionButton.Image = ((System.Drawing.Image)(resources.GetObject("addDoubleConnectionButton.Image")));
            this.addDoubleConnectionButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addDoubleConnectionButton.Name = "addDoubleConnectionButton";
            this.addDoubleConnectionButton.Size = new System.Drawing.Size(23, 22);
            this.addDoubleConnectionButton.Text = "toolStripButton10";
            this.addDoubleConnectionButton.ToolTipText = "Add Double Connection";
            this.addDoubleConnectionButton.Click += new System.EventHandler(this.AddDoubleConnectionButton_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // positionCoordinateLabel
            // 
            this.positionCoordinateLabel.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold);
            this.positionCoordinateLabel.Name = "positionCoordinateLabel";
            this.positionCoordinateLabel.Size = new System.Drawing.Size(75, 22);
            this.positionCoordinateLabel.Text = "Coordinate";
            // 
            // saveMapDialog
            // 
            this.saveMapDialog.Filter = "map|*.map|all|*.*";
            this.saveMapDialog.Title = "Save map to file";
            // 
            // loadMapDialog
            // 
            this.loadMapDialog.DefaultExt = "map";
            this.loadMapDialog.Filter = "map|*.map|all|*.*";
            this.loadMapDialog.Title = "Load map from file";
            // 
            // LayersEditedControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStrip1);
            this.Name = "LayersEditedControl";
            this.Size = new System.Drawing.Size(500, 25);
            this.Load += new System.EventHandler(this.PositionSetEditedControl_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton daggleButton;
        private System.Windows.Forms.ToolStripButton selectBotton;
        private System.Windows.Forms.ToolStripButton addNodeButton;
        private System.Windows.Forms.ToolStripLabel positionCoordinateLabel;
        private System.Windows.Forms.ToolStripButton editedBotton;
        private System.Windows.Forms.ToolStripButton removeNodeButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton addConnectionButton;
        private System.Windows.Forms.ToolStripButton addDoubleConnectionButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton newButton;
        private System.Windows.Forms.ToolStripButton openButton;
        private System.Windows.Forms.ToolStripButton saveButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.SaveFileDialog saveMapDialog;
        private System.Windows.Forms.OpenFileDialog loadMapDialog;

    }
}
