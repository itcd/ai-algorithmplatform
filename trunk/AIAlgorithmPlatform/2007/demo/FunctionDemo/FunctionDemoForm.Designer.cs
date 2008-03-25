namespace FunctionDemo
{
    partial class FunctionDemoForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.positionSetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.emptyPositionSetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createRandomSetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getPositionSetFormDBFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.currentPositionSetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.algorithmToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.demoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m2MCHToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m2MNNToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pathFindingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.preProcessForPositionSetConnectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createRandomSetConnectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.positionSetToolStripMenuItem,
            this.algorithmToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(538, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // positionSetToolStripMenuItem
            // 
            this.positionSetToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.emptyPositionSetToolStripMenuItem,
            this.createRandomSetToolStripMenuItem,
            this.createRandomSetConnectedToolStripMenuItem,
            this.getPositionSetFormDBFToolStripMenuItem,
            this.toolStripSeparator1,
            this.currentPositionSetToolStripMenuItem});
            this.positionSetToolStripMenuItem.Name = "positionSetToolStripMenuItem";
            this.positionSetToolStripMenuItem.Size = new System.Drawing.Size(83, 20);
            this.positionSetToolStripMenuItem.Text = "PositionSet";
            // 
            // emptyPositionSetToolStripMenuItem
            // 
            this.emptyPositionSetToolStripMenuItem.Name = "emptyPositionSetToolStripMenuItem";
            this.emptyPositionSetToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.emptyPositionSetToolStripMenuItem.Text = "EmptyPositionSet";
            this.emptyPositionSetToolStripMenuItem.Click += new System.EventHandler(this.emptyPositionSetToolStripMenuItem_Click);
            // 
            // createRandomSetToolStripMenuItem
            // 
            this.createRandomSetToolStripMenuItem.Name = "createRandomSetToolStripMenuItem";
            this.createRandomSetToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.createRandomSetToolStripMenuItem.Text = "CreateRandomSet";
            this.createRandomSetToolStripMenuItem.Click += new System.EventHandler(this.createRandomSetToolStripMenuItem_Click);
            // 
            // getPositionSetFormDBFToolStripMenuItem
            // 
            this.getPositionSetFormDBFToolStripMenuItem.Name = "getPositionSetFormDBFToolStripMenuItem";
            this.getPositionSetFormDBFToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.getPositionSetFormDBFToolStripMenuItem.Text = "GetPositionSetFormDBF";
            this.getPositionSetFormDBFToolStripMenuItem.Click += new System.EventHandler(this.getPositionSetFormDBFToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(211, 6);
            // 
            // currentPositionSetToolStripMenuItem
            // 
            this.currentPositionSetToolStripMenuItem.Name = "currentPositionSetToolStripMenuItem";
            this.currentPositionSetToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.currentPositionSetToolStripMenuItem.Text = "CurrentPositionSet";
            this.currentPositionSetToolStripMenuItem.Click += new System.EventHandler(this.currentPositionSetToolStripMenuItem_Click);
            // 
            // algorithmToolStripMenuItem
            // 
            this.algorithmToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.demoToolStripMenuItem,
            this.resultToolStripMenuItem});
            this.algorithmToolStripMenuItem.Name = "algorithmToolStripMenuItem";
            this.algorithmToolStripMenuItem.Size = new System.Drawing.Size(71, 20);
            this.algorithmToolStripMenuItem.Text = "Algorithm";
            // 
            // demoToolStripMenuItem
            // 
            this.demoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m2MCHToolStripMenuItem,
            this.m2MNNToolStripMenuItem});
            this.demoToolStripMenuItem.Name = "demoToolStripMenuItem";
            this.demoToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.demoToolStripMenuItem.Text = "Demo";
            // 
            // m2MCHToolStripMenuItem
            // 
            this.m2MCHToolStripMenuItem.Name = "m2MCHToolStripMenuItem";
            this.m2MCHToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.m2MCHToolStripMenuItem.Text = "M2M_CH";
            this.m2MCHToolStripMenuItem.Click += new System.EventHandler(this.m2MCHToolStripMenuItem_Click);
            // 
            // m2MNNToolStripMenuItem
            // 
            this.m2MNNToolStripMenuItem.Name = "m2MNNToolStripMenuItem";
            this.m2MNNToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.m2MNNToolStripMenuItem.Text = "M2M_NN";
            this.m2MNNToolStripMenuItem.Click += new System.EventHandler(this.m2MNNToolStripMenuItem_Click);
            // 
            // resultToolStripMenuItem
            // 
            this.resultToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pathFindingToolStripMenuItem,
            this.preProcessForPositionSetConnectedToolStripMenuItem});
            this.resultToolStripMenuItem.Name = "resultToolStripMenuItem";
            this.resultToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.resultToolStripMenuItem.Text = "Result";
            // 
            // pathFindingToolStripMenuItem
            // 
            this.pathFindingToolStripMenuItem.Name = "pathFindingToolStripMenuItem";
            this.pathFindingToolStripMenuItem.Size = new System.Drawing.Size(274, 22);
            this.pathFindingToolStripMenuItem.Text = "PathFinding";
            this.pathFindingToolStripMenuItem.Click += new System.EventHandler(this.pathFindingToolStripMenuItem_Click);
            // 
            // preProcessForPositionSetConnectedToolStripMenuItem
            // 
            this.preProcessForPositionSetConnectedToolStripMenuItem.Name = "preProcessForPositionSetConnectedToolStripMenuItem";
            this.preProcessForPositionSetConnectedToolStripMenuItem.Size = new System.Drawing.Size(274, 22);
            this.preProcessForPositionSetConnectedToolStripMenuItem.Text = "PreProcessForPositionSet_Connected";
            this.preProcessForPositionSetConnectedToolStripMenuItem.Click += new System.EventHandler(this.preProcessForPositionSetConnectedToolStripMenuItem_Click);
            // 
            // createRandomSetConnectedToolStripMenuItem
            // 
            this.createRandomSetConnectedToolStripMenuItem.Name = "createRandomSetConnectedToolStripMenuItem";
            this.createRandomSetConnectedToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.createRandomSetConnectedToolStripMenuItem.Text = "CreateRandomSetConnected";
            this.createRandomSetConnectedToolStripMenuItem.Click += new System.EventHandler(this.createRandomSetConnectedToolStripMenuItem_Click);
            // 
            // FunctionDemoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(538, 408);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FunctionDemoForm";
            this.Text = "FunctionDemoForm";
            this.Load += new System.EventHandler(this.FunctionDemoForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem positionSetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem emptyPositionSetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createRandomSetToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem currentPositionSetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem algorithmToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem demoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem m2MCHToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem m2MNNToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getPositionSetFormDBFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resultToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pathFindingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem preProcessForPositionSetConnectedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createRandomSetConnectedToolStripMenuItem;
    }
}

