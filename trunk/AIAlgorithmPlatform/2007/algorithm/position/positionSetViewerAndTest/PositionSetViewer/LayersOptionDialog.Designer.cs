namespace PositionSetViewer
{
    partial class LayersOptionDialog
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
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem("凸包3");
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem("凸包2");
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem("凸包1");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LayersOptionDialog));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listView = new System.Windows.Forms.ListView();
            this.LayerName = new System.Windows.Forms.ColumnHeader();
            this.Type = new System.Windows.Forms.ColumnHeader();
            this.Visible = new System.Windows.Forms.ColumnHeader();
            this.Active = new System.Windows.Forms.ColumnHeader();
            this.toolLayer = new System.Windows.Forms.ToolStrip();
            this.toolLayerUp = new System.Windows.Forms.ToolStripButton();
            this.toolLayerDown = new System.Windows.Forms.ToolStripButton();
            this.toolLayerDelete = new System.Windows.Forms.ToolStripButton();
            this.layerPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.ScaleTrackBar = new System.Windows.Forms.TrackBar();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.VisibleCheckBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.ActiveCheckBox = new System.Windows.Forms.CheckBox();
            this.LayerColorSelect = new PositionSetViewer.ColorSelect();
            this.DropDownBotton = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.toolLayer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleTrackBar)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listView);
            this.groupBox1.Controls.Add(this.toolLayer);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Location = new System.Drawing.Point(9, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(361, 198);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "LayerList";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // listView
            // 
            this.listView.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.LayerName,
            this.Type,
            this.Visible,
            this.Active});
            this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView.Font = new System.Drawing.Font("NSimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listView.ForeColor = System.Drawing.SystemColors.WindowText;
            this.listView.FullRowSelect = true;
            this.listView.GridLines = true;
            this.listView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView.HideSelection = false;
            this.listView.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            listViewItem4.StateImageIndex = 0;
            listViewItem5.StateImageIndex = 0;
            listViewItem6.Checked = true;
            listViewItem6.StateImageIndex = 1;
            this.listView.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem4,
            listViewItem5,
            listViewItem6});
            this.listView.LabelEdit = true;
            this.listView.LabelWrap = false;
            this.listView.Location = new System.Drawing.Point(3, 17);
            this.listView.Name = "listView";
            this.listView.ShowGroups = false;
            this.listView.Size = new System.Drawing.Size(355, 153);
            this.listView.TabIndex = 1;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            this.listView.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // LayerName
            // 
            this.LayerName.Text = "LayerName";
            this.LayerName.Width = 130;
            // 
            // Type
            // 
            this.Type.Text = "Type";
            this.Type.Width = 153;
            // 
            // Visible
            // 
            this.Visible.Text = "Visible";
            this.Visible.Width = 24;
            // 
            // Active
            // 
            this.Active.Text = "Active";
            this.Active.Width = 24;
            // 
            // toolLayer
            // 
            this.toolLayer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolLayer.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolLayer.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolLayerUp,
            this.toolLayerDown,
            this.toolLayerDelete});
            this.toolLayer.Location = new System.Drawing.Point(3, 170);
            this.toolLayer.Name = "toolLayer";
            this.toolLayer.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolLayer.Size = new System.Drawing.Size(355, 25);
            this.toolLayer.TabIndex = 2;
            this.toolLayer.Text = "toolStrip1";
            this.toolLayer.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolLayer_ItemClicked);
            // 
            // toolLayerUp
            // 
            this.toolLayerUp.Image = ((System.Drawing.Image)(resources.GetObject("toolLayerUp.Image")));
            this.toolLayerUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolLayerUp.Name = "toolLayerUp";
            this.toolLayerUp.Size = new System.Drawing.Size(67, 22);
            this.toolLayerUp.Text = "上移(&U)";
            this.toolLayerUp.Click += new System.EventHandler(this.toolLayerUp_Click);
            // 
            // toolLayerDown
            // 
            this.toolLayerDown.Image = ((System.Drawing.Image)(resources.GetObject("toolLayerDown.Image")));
            this.toolLayerDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolLayerDown.Name = "toolLayerDown";
            this.toolLayerDown.Size = new System.Drawing.Size(67, 22);
            this.toolLayerDown.Text = "下移(&D)";
            this.toolLayerDown.Click += new System.EventHandler(this.toolLayerDown_Click);
            // 
            // toolLayerDelete
            // 
            this.toolLayerDelete.Image = ((System.Drawing.Image)(resources.GetObject("toolLayerDelete.Image")));
            this.toolLayerDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolLayerDelete.Name = "toolLayerDelete";
            this.toolLayerDelete.Size = new System.Drawing.Size(67, 22);
            this.toolLayerDelete.Text = "移除(&D)";
            this.toolLayerDelete.Click += new System.EventHandler(this.toolLayerDelete_Click);
            // 
            // layerPropertyGrid
            // 
            this.layerPropertyGrid.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.layerPropertyGrid.HelpVisible = false;
            this.layerPropertyGrid.Location = new System.Drawing.Point(17, 228);
            this.layerPropertyGrid.Name = "layerPropertyGrid";
            this.layerPropertyGrid.SelectedObject = this.listView;
            this.layerPropertyGrid.Size = new System.Drawing.Size(502, 170);
            this.layerPropertyGrid.TabIndex = 5;
            this.layerPropertyGrid.ToolbarVisible = false;
            this.layerPropertyGrid.Click += new System.EventHandler(this.layerpPropertyGrid_Click);
            this.layerPropertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.layerpPropertyGrid_PropertyValueChanged);
            // 
            // ScaleTrackBar
            // 
            this.ScaleTrackBar.Location = new System.Drawing.Point(377, 25);
            this.ScaleTrackBar.Maximum = 20;
            this.ScaleTrackBar.Minimum = -5;
            this.ScaleTrackBar.Name = "ScaleTrackBar";
            this.ScaleTrackBar.Size = new System.Drawing.Size(147, 42);
            this.ScaleTrackBar.TabIndex = 6;
            this.ScaleTrackBar.Scroll += new System.EventHandler(this.ScaleTrackBar_Scroll);
            // 
            // groupBox2
            // 
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Location = new System.Drawing.Point(375, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(151, 68);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "ScreenScale";
            // 
            // VisibleCheckBox
            // 
            this.VisibleCheckBox.AutoSize = true;
            this.VisibleCheckBox.Location = new System.Drawing.Point(417, 133);
            this.VisibleCheckBox.Name = "VisibleCheckBox";
            this.VisibleCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.VisibleCheckBox.Size = new System.Drawing.Size(72, 16);
            this.VisibleCheckBox.TabIndex = 9;
            this.VisibleCheckBox.Text = ":Visible";
            this.VisibleCheckBox.UseVisualStyleBackColor = true;
            this.VisibleCheckBox.Enter += new System.EventHandler(this.VisibleCheckBox_Enter);
            this.VisibleCheckBox.CheckedChanged += new System.EventHandler(this.VisibleCheckBox_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(409, 157);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 11;
            this.label1.Text = "MainColor:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.ActiveCheckBox);
            this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.groupBox3.Location = new System.Drawing.Point(375, 86);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(151, 98);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "LayerCommonProperty";
            // 
            // ActiveCheckBox
            // 
            this.ActiveCheckBox.AutoSize = true;
            this.ActiveCheckBox.Location = new System.Drawing.Point(48, 24);
            this.ActiveCheckBox.Name = "ActiveCheckBox";
            this.ActiveCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ActiveCheckBox.Size = new System.Drawing.Size(66, 16);
            this.ActiveCheckBox.TabIndex = 9;
            this.ActiveCheckBox.Text = ":Active";
            this.ActiveCheckBox.UseVisualStyleBackColor = true;
            this.ActiveCheckBox.Enter += new System.EventHandler(this.ActiveCheckBox_Enter);
            this.ActiveCheckBox.CheckedChanged += new System.EventHandler(this.ActiveCheckBox_CheckedChanged);
            // 
            // LayerColorSelect
            // 
            this.LayerColorSelect.BackColor = System.Drawing.Color.Crimson;
            this.LayerColorSelect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LayerColorSelect.Location = new System.Drawing.Point(475, 157);
            this.LayerColorSelect.Name = "LayerColorSelect";
            this.LayerColorSelect.Size = new System.Drawing.Size(14, 13);
            this.LayerColorSelect.TabIndex = 10;
            this.LayerColorSelect.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LayerColorSelect_MouseDown);
            // 
            // DropDownBotton
            // 
            this.DropDownBotton.FlatAppearance.BorderSize = 0;
            this.DropDownBotton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.MintCream;
            this.DropDownBotton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.DropDownBotton.Image = ((System.Drawing.Image)(resources.GetObject("DropDownBotton.Image")));
            this.DropDownBotton.Location = new System.Drawing.Point(510, 189);
            this.DropDownBotton.Name = "DropDownBotton";
            this.DropDownBotton.Size = new System.Drawing.Size(15, 15);
            this.DropDownBotton.TabIndex = 12;
            this.DropDownBotton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.DropDownBotton.UseVisualStyleBackColor = true;
            this.DropDownBotton.Click += new System.EventHandler(this.DropDownBotton_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox4.Location = new System.Drawing.Point(9, 208);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(517, 196);
            this.groupBox4.TabIndex = 13;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "LayerProperty";
            // 
            // LayersOptionDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 409);
            this.Controls.Add(this.DropDownBotton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LayerColorSelect);
            this.Controls.Add(this.VisibleCheckBox);
            this.Controls.Add(this.ScaleTrackBar);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.layerPropertyGrid);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LayersOptionDialog";
            this.Opacity = 0.9;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configuration";
            this.Load += new System.EventHandler(this.PainterOptionDialog_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.toolLayer.ResumeLayout(false);
            this.toolLayer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleTrackBar)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ColumnHeader LayerName;
        private new System.Windows.Forms.ColumnHeader Visible;
        private System.Windows.Forms.PropertyGrid layerPropertyGrid;
        private System.Windows.Forms.TrackBar ScaleTrackBar;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox VisibleCheckBox;
        private System.Windows.Forms.ToolStrip toolLayer;
        private System.Windows.Forms.ToolStripButton toolLayerUp;
        private System.Windows.Forms.ToolStripButton toolLayerDown;
        private System.Windows.Forms.ToolStripButton toolLayerDelete;
        private ColorSelect LayerColorSelect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ColumnHeader Active;
        private System.Windows.Forms.CheckBox ActiveCheckBox;
        private System.Windows.Forms.ColumnHeader Type;
        private System.Windows.Forms.Button DropDownBotton;
        private System.Windows.Forms.GroupBox groupBox4;
    }
}