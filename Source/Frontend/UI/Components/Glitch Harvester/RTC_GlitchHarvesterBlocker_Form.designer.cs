namespace RTCV.UI
{
    partial class RTC_GlitchHarvesterBlocker_Form
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
            this.btnEmergencySave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnEmergencySave
            // 
            this.btnEmergencySave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEmergencySave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnEmergencySave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEmergencySave.ForeColor = System.Drawing.Color.White;
            this.btnEmergencySave.Location = new System.Drawing.Point(12, 315);
            this.btnEmergencySave.Name = "btnEmergencySave";
            this.btnEmergencySave.Size = new System.Drawing.Size(166, 25);
            this.btnEmergencySave.TabIndex = 0;
            this.btnEmergencySave.Tag = "color:normal";
            this.btnEmergencySave.Text = "Emergency Stockpile Save-As";
            this.btnEmergencySave.UseVisualStyleBackColor = false;
            this.btnEmergencySave.Click += new System.EventHandler(this.BtnEmergencySave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(7, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(205, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Waiting for reconnect...";
            // 
            // RTC_GlithHarvesterBlocker_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(412, 352);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnEmergencySave);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "RTC_Blocker_Form";
            this.Tag = "color:dark2";
            this.Text = "Blocker Form";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnEmergencySave;
        private System.Windows.Forms.Label label1;
    }
}