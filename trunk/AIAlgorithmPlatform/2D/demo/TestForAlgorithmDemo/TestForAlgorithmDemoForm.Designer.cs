namespace TestForAlgorithmDemo
{
    partial class TestForAlgorithmDemoForm
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
            this.demoBotton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // demoBotton
            // 
            this.demoBotton.Location = new System.Drawing.Point(69, 17);
            this.demoBotton.Name = "demoBotton";
            this.demoBotton.Size = new System.Drawing.Size(75, 23);
            this.demoBotton.TabIndex = 0;
            this.demoBotton.Text = "Demo";
            this.demoBotton.UseVisualStyleBackColor = true;
            this.demoBotton.Click += new System.EventHandler(this.TestForAlgorithmDemo);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(215, 57);
            this.Controls.Add(this.demoBotton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button demoBotton;
    }
}

