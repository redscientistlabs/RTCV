namespace RTCV.GlitchHarvester
{
    partial class GH_CanvasForm
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
            this.pnScale.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pnScale.Location = new System.Drawing.Point(10, 10);
            this.pnScale.Name = "pnScale";
            this.pnScale.Size = new System.Drawing.Size(135, 135);
            this.pnScale.TabIndex = 5;
            // 
            // GH_CanvasForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(591, 425);
            this.Controls.Add(this.pnScale);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "GH_CanvasForm";
            this.Text = "GH_CanvasForm";
            this.Load += new System.EventHandler(this.GH_CanvasForm_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pnScale;
    }
}