namespace RTCV.UI.Components.Controls
{
    partial class MultiUpDown
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
            this.updown = new RTCV.UI.NumericUpDownHexFix();
            ((System.ComponentModel.ISupportInitialize)(this.updown)).BeginInit();
            this.SuspendLayout();
            // 
            // updown
            // 
            this.updown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.updown.Location = new System.Drawing.Point(0, 0);
            this.updown.Name = "updown";
            this.updown.Size = new System.Drawing.Size(120, 20);
            this.updown.TabIndex = 0;
            // 
            // MultiUpDown
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.updown);
            this.Name = "MultiUpDown";
            this.Size = new System.Drawing.Size(120, 20);
            ((System.ComponentModel.ISupportInitialize)(this.updown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private NumericUpDownHexFix updown;
    }
}
