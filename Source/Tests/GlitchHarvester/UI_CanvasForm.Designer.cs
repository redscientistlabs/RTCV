namespace RTCV.UI
{
    partial class UI_CanvasForm
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
            this.pnScale = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnScale
            // 
            this.pnScale.BackColor = System.Drawing.Color.Gray;
            this.pnScale.Location = new System.Drawing.Point(15, 15);
            this.pnScale.Name = "pnScale";
            this.pnScale.Size = new System.Drawing.Size(50, 50);
            this.pnScale.TabIndex = 5;
            this.pnScale.Tag = "color:normal";
            // 
            // UI_CanvasForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(591, 425);
            this.Controls.Add(this.pnScale);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "UI_CanvasForm";
            this.Tag = "color:dark";
            this.Text = "UI_CanvasForm";
            this.Load += new System.EventHandler(this.UI_CanvasForm_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pnScale;
    }
}