namespace TestForPositionSetEditer
{
    partial class TestForm
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
            this.TestBotton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TestBotton
            // 
            this.TestBotton.Location = new System.Drawing.Point(104, 32);
            this.TestBotton.Name = "TestBotton";
            this.TestBotton.Size = new System.Drawing.Size(75, 23);
            this.TestBotton.TabIndex = 0;
            this.TestBotton.Text = "Test";
            this.TestBotton.UseVisualStyleBackColor = true;
            this.TestBotton.Click += new System.EventHandler(this.TestBotton_Click);
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 87);
            this.Controls.Add(this.TestBotton);
            this.Name = "TestForm";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button TestBotton;
    }
}

