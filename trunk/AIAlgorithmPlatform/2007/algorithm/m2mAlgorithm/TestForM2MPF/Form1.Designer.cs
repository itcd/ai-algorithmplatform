namespace TestForM2MPF
{
    partial class Form1
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
            this.TestBotton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TestBotton
            // 
            this.TestBotton.Location = new System.Drawing.Point(83, 25);
            this.TestBotton.Name = "TestBotton";
            this.TestBotton.Size = new System.Drawing.Size(75, 23);
            this.TestBotton.TabIndex = 0;
            this.TestBotton.Text = "Test";
            this.TestBotton.UseVisualStyleBackColor = true;
            this.TestBotton.Click += new System.EventHandler(this.TestBotton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(233, 67);
            this.Controls.Add(this.TestBotton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button TestBotton;
    }
}

