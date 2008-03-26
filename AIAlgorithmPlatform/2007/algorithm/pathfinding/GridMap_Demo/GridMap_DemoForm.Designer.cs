namespace GridMap_Demo
{
    partial class GridMapForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.BFSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AStarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.RandomMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.randomMap2ToolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.RenewMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ClearMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowPathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TimeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.AboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(1, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1000, 600);
            this.panel1.TabIndex = 1;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1002, 24);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BFSToolStripMenuItem,
            this.AStarToolStripMenuItem,
            this.ExitToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(57, 20);
            this.toolStripMenuItem1.Text = "功能(&F)";
            // 
            // BFSToolStripMenuItem
            // 
            this.BFSToolStripMenuItem.Name = "BFSToolStripMenuItem";
            this.BFSToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.BFSToolStripMenuItem.Text = "广度优先搜索(&B)";
            this.BFSToolStripMenuItem.Click += new System.EventHandler(this.BFSToolStripMenuItem_Click);
            // 
            // AStarToolStripMenuItem
            // 
            this.AStarToolStripMenuItem.Name = "AStarToolStripMenuItem";
            this.AStarToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.AStarToolStripMenuItem.Text = "A*搜索(A)";
            this.AStarToolStripMenuItem.Click += new System.EventHandler(this.AStarToolStripMenuItem_Click);
            // 
            // ExitToolStripMenuItem
            // 
            this.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
            this.ExitToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.ExitToolStripMenuItem.Text = "退出(&X)";
            this.ExitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RandomMapToolStripMenuItem,
            this.randomMap2ToolStripMenuItem5,
            this.RenewMapToolStripMenuItem,
            this.ClearMapToolStripMenuItem});
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(59, 20);
            this.toolStripMenuItem2.Text = "地图(&M)";
            // 
            // RandomMapToolStripMenuItem
            // 
            this.RandomMapToolStripMenuItem.Name = "RandomMapToolStripMenuItem";
            this.RandomMapToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.RandomMapToolStripMenuItem.Text = "生成随机地图(&R)";
            this.RandomMapToolStripMenuItem.Click += new System.EventHandler(this.RandomMapToolStripMenuItem_Click);
            // 
            // randomMap2ToolStripMenuItem5
            // 
            this.randomMap2ToolStripMenuItem5.Name = "randomMap2ToolStripMenuItem5";
            this.randomMap2ToolStripMenuItem5.Size = new System.Drawing.Size(161, 22);
            this.randomMap2ToolStripMenuItem5.Text = "生成随机地图2";
            this.randomMap2ToolStripMenuItem5.Click += new System.EventHandler(this.randomMap2ToolStripMenuItem5_Click);
            // 
            // RenewMapToolStripMenuItem
            // 
            this.RenewMapToolStripMenuItem.Name = "RenewMapToolStripMenuItem";
            this.RenewMapToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.RenewMapToolStripMenuItem.Text = "清除地图色彩(&C)";
            this.RenewMapToolStripMenuItem.Click += new System.EventHandler(this.RenewMapToolStripMenuItem_Click);
            // 
            // ClearMapToolStripMenuItem
            // 
            this.ClearMapToolStripMenuItem.Name = "ClearMapToolStripMenuItem";
            this.ClearMapToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.ClearMapToolStripMenuItem.Text = "清空地图(&E)";
            this.ClearMapToolStripMenuItem.Click += new System.EventHandler(this.ClearMapToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ShowPathToolStripMenuItem,
            this.TimeToolStripMenuItem});
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(59, 20);
            this.toolStripMenuItem3.Text = "选项(&O)";
            // 
            // ShowPathToolStripMenuItem
            // 
            this.ShowPathToolStripMenuItem.Checked = true;
            this.ShowPathToolStripMenuItem.CheckOnClick = true;
            this.ShowPathToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ShowPathToolStripMenuItem.Name = "ShowPathToolStripMenuItem";
            this.ShowPathToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.ShowPathToolStripMenuItem.Text = "显示路径(&P)";
            // 
            // TimeToolStripMenuItem
            // 
            this.TimeToolStripMenuItem.Checked = true;
            this.TimeToolStripMenuItem.CheckOnClick = true;
            this.TimeToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.TimeToolStripMenuItem.Name = "TimeToolStripMenuItem";
            this.TimeToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.TimeToolStripMenuItem.Text = "计算时间";
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AboutToolStripMenuItem});
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(58, 20);
            this.toolStripMenuItem4.Text = "帮助(&H)";
            // 
            // AboutToolStripMenuItem
            // 
            this.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem";
            this.AboutToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.AboutToolStripMenuItem.Text = "关于(&A)";
            this.AboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
            // 
            // GridMapForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1002, 626);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "算法演示平台";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem BFSToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AStarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem RandomMapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem RenewMapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ClearMapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ShowPathToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem TimeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem randomMap2ToolStripMenuItem5;
    }
}

