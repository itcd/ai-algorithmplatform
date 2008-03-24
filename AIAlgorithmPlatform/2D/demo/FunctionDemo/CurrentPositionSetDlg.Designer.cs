namespace FunctionDemo
{
    partial class CurrentPositionSetDlg
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
            this.PositionSetListView = new System.Windows.Forms.ListView();
            this.Name = new System.Windows.Forms.ColumnHeader();
            this.Type = new System.Windows.Forms.ColumnHeader();
            this.GetPositionSetBotton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // PositionSetListView
            // 
            this.PositionSetListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Name,
            this.Type});
            this.PositionSetListView.Location = new System.Drawing.Point(12, 12);
            this.PositionSetListView.Name = "PositionSetListView";
            this.PositionSetListView.Size = new System.Drawing.Size(297, 163);
            this.PositionSetListView.TabIndex = 0;
            this.PositionSetListView.UseCompatibleStateImageBehavior = false;
            this.PositionSetListView.View = System.Windows.Forms.View.Details;
            // 
            // Name
            // 
            this.Name.Text = "Name";
            this.Name.Width = 100;
            // 
            // Type
            // 
            this.Type.Text = "Type";
            this.Type.Width = 165;
            // 
            // GetPositionSetBotton
            // 
            this.GetPositionSetBotton.Location = new System.Drawing.Point(122, 193);
            this.GetPositionSetBotton.Name = "GetPositionSetBotton";
            this.GetPositionSetBotton.Size = new System.Drawing.Size(75, 23);
            this.GetPositionSetBotton.TabIndex = 1;
            this.GetPositionSetBotton.Text = "GetSelect";
            this.GetPositionSetBotton.UseVisualStyleBackColor = true;
            this.GetPositionSetBotton.Click += new System.EventHandler(this.GetPositionSetBotton_Click);
            // 
            // CurrentPositionSetDlg
            // 
            this.AllowDrop = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(321, 232);
            this.Controls.Add(this.GetPositionSetBotton);
            this.Controls.Add(this.PositionSetListView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            //this.Name = "CurrentPositionSetDlg";
            this.Text = "CurrentPositionSetDlg";
            this.Load += new System.EventHandler(this.CurrentPositionSetDlg_Load);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.ListView PositionSetListView;
        private System.Windows.Forms.ColumnHeader Name;
        private System.Windows.Forms.ColumnHeader Type;
        private System.Windows.Forms.Button GetPositionSetBotton;
    }
}