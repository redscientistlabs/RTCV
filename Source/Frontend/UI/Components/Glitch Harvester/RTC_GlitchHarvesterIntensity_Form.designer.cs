namespace RTCV.UI
{
    partial class RTC_GlitchHarvesterIntensity_Form
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
            this.multiTB_Intensity = new RTCV.UI.Components.Controls.MultiTrackBar();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // multiTB_Intensity
            // 
            this.multiTB_Intensity.BackColor = System.Drawing.Color.Gray;
            this.multiTB_Intensity.DisplayCheckbox = false;
            this.multiTB_Intensity.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.multiTB_Intensity.Label = "Intensity";
            this.multiTB_Intensity.Location = new System.Drawing.Point(12, 12);
            this.multiTB_Intensity.Maximum = ((long)(65535));
            this.multiTB_Intensity.Minimum = ((long)(1));
            this.multiTB_Intensity.Name = "multiTB_Intensity";
            this.multiTB_Intensity.Size = new System.Drawing.Size(232, 51);
            this.multiTB_Intensity.TabIndex = 1;
            this.multiTB_Intensity.Tag = "color:normal";
            this.multiTB_Intensity.UncapNumericBox = false;
            this.multiTB_Intensity.Value = ((long)(1));
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(53, 24);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(139, 26);
            this.label6.TabIndex = 124;
            this.label6.Text = "Parameters unavailable with\ncurrent engine";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // RTC_VmdNoTool_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(257, 78);
            this.Controls.Add(this.multiTB_Intensity);
            this.Controls.Add(this.label6);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "RTC_VmdNoTool_Form";
            this.Tag = "color:dark1";
            this.Text = "No Tool Selected";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public Components.Controls.MultiTrackBar multiTB_Intensity;
        private System.Windows.Forms.Label label6;
    }
}