namespace RTCV.UI
{
    partial class RTC_SettingsGeneral_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RTC_SettingsGeneral_Form));
            this.btnImportKeyBindings = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbDontCleanAtQuit = new System.Windows.Forms.CheckBox();
            this.cbAllowCrossCoreCorruption = new System.Windows.Forms.CheckBox();
            this.cbDisableBizhawkOSD = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnChangeRTCColor = new System.Windows.Forms.Button();
            this.btnOpenOnlineWiki = new System.Windows.Forms.Button();
            this.cbUncapIntensity = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnImportKeyBindings
            // 
            this.btnImportKeyBindings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnImportKeyBindings.FlatAppearance.BorderSize = 0;
            this.btnImportKeyBindings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImportKeyBindings.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnImportKeyBindings.ForeColor = System.Drawing.Color.Black;
            this.btnImportKeyBindings.Image = ((System.Drawing.Image)(resources.GetObject("btnImportKeyBindings.Image")));
            this.btnImportKeyBindings.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnImportKeyBindings.Location = new System.Drawing.Point(80, 21);
            this.btnImportKeyBindings.Name = "btnImportKeyBindings";
            this.btnImportKeyBindings.Size = new System.Drawing.Size(250, 45);
            this.btnImportKeyBindings.TabIndex = 137;
            this.btnImportKeyBindings.Tag = "color:light";
            this.btnImportKeyBindings.Text = "   Import key bindings";
            this.btnImportKeyBindings.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnImportKeyBindings.UseVisualStyleBackColor = false;
            this.btnImportKeyBindings.Click += new System.EventHandler(this.btnImportKeyBindings_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gray;
            this.panel1.Controls.Add(this.cbUncapIntensity);
            this.panel1.Controls.Add(this.cbDontCleanAtQuit);
            this.panel1.Controls.Add(this.cbAllowCrossCoreCorruption);
            this.panel1.Controls.Add(this.cbDisableBizhawkOSD);
            this.panel1.Location = new System.Drawing.Point(80, 198);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(250, 96);
            this.panel1.TabIndex = 138;
            this.panel1.Tag = "color:normal";
            // 
            // cbDontCleanAtQuit
            // 
            this.cbDontCleanAtQuit.AutoSize = true;
            this.cbDontCleanAtQuit.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbDontCleanAtQuit.ForeColor = System.Drawing.Color.White;
            this.cbDontCleanAtQuit.Location = new System.Drawing.Point(11, 70);
            this.cbDontCleanAtQuit.Name = "cbDontCleanAtQuit";
            this.cbDontCleanAtQuit.Size = new System.Drawing.Size(177, 17);
            this.cbDontCleanAtQuit.TabIndex = 2;
            this.cbDontCleanAtQuit.Text = "Don\'t clean savestates at quit";
            this.cbDontCleanAtQuit.UseVisualStyleBackColor = true;
            this.cbDontCleanAtQuit.CheckedChanged += new System.EventHandler(this.cbDontCleanAtQuit_CheckedChanged);
            // 
            // cbAllowCrossCoreCorruption
            // 
            this.cbAllowCrossCoreCorruption.AutoSize = true;
            this.cbAllowCrossCoreCorruption.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbAllowCrossCoreCorruption.ForeColor = System.Drawing.Color.White;
            this.cbAllowCrossCoreCorruption.Location = new System.Drawing.Point(11, 30);
            this.cbAllowCrossCoreCorruption.Name = "cbAllowCrossCoreCorruption";
            this.cbAllowCrossCoreCorruption.Size = new System.Drawing.Size(172, 17);
            this.cbAllowCrossCoreCorruption.TabIndex = 1;
            this.cbAllowCrossCoreCorruption.Text = "Allow Cross-Core corruption";
            this.cbAllowCrossCoreCorruption.UseVisualStyleBackColor = true;
            this.cbAllowCrossCoreCorruption.CheckedChanged += new System.EventHandler(this.cbAllowCrossCoreCorruption_CheckedChanged);
            // 
            // cbDisableBizhawkOSD
            // 
            this.cbDisableBizhawkOSD.AutoSize = true;
            this.cbDisableBizhawkOSD.Checked = true;
            this.cbDisableBizhawkOSD.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbDisableBizhawkOSD.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbDisableBizhawkOSD.ForeColor = System.Drawing.Color.White;
            this.cbDisableBizhawkOSD.Location = new System.Drawing.Point(11, 10);
            this.cbDisableBizhawkOSD.Name = "cbDisableBizhawkOSD";
            this.cbDisableBizhawkOSD.Size = new System.Drawing.Size(194, 17);
            this.cbDisableBizhawkOSD.TabIndex = 0;
            this.cbDisableBizhawkOSD.Text = "Disable the BizHawk OSD system";
            this.cbDisableBizhawkOSD.UseVisualStyleBackColor = true;
            this.cbDisableBizhawkOSD.CheckedChanged += new System.EventHandler(this.cbDisableBizhawkOSD_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(77, 180);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(117, 15);
            this.label4.TabIndex = 139;
            this.label4.Text = "General RTC Settings";
            // 
            // btnChangeRTCColor
            // 
            this.btnChangeRTCColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnChangeRTCColor.FlatAppearance.BorderSize = 0;
            this.btnChangeRTCColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChangeRTCColor.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnChangeRTCColor.ForeColor = System.Drawing.Color.Black;
            this.btnChangeRTCColor.Image = ((System.Drawing.Image)(resources.GetObject("btnChangeRTCColor.Image")));
            this.btnChangeRTCColor.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnChangeRTCColor.Location = new System.Drawing.Point(80, 123);
            this.btnChangeRTCColor.Name = "btnChangeRTCColor";
            this.btnChangeRTCColor.Size = new System.Drawing.Size(250, 45);
            this.btnChangeRTCColor.TabIndex = 136;
            this.btnChangeRTCColor.Tag = "color:light";
            this.btnChangeRTCColor.Text = "   Change color theme";
            this.btnChangeRTCColor.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnChangeRTCColor.UseVisualStyleBackColor = false;
            this.btnChangeRTCColor.Click += new System.EventHandler(this.btnChangeRTCColor_Click);
            // 
            // btnOpenOnlineWiki
            // 
            this.btnOpenOnlineWiki.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnOpenOnlineWiki.FlatAppearance.BorderSize = 0;
            this.btnOpenOnlineWiki.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenOnlineWiki.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnOpenOnlineWiki.ForeColor = System.Drawing.Color.Black;
            this.btnOpenOnlineWiki.Image = ((System.Drawing.Image)(resources.GetObject("btnOpenOnlineWiki.Image")));
            this.btnOpenOnlineWiki.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOpenOnlineWiki.Location = new System.Drawing.Point(80, 72);
            this.btnOpenOnlineWiki.Name = "btnOpenOnlineWiki";
            this.btnOpenOnlineWiki.Size = new System.Drawing.Size(250, 45);
            this.btnOpenOnlineWiki.TabIndex = 135;
            this.btnOpenOnlineWiki.Tag = "color:light";
            this.btnOpenOnlineWiki.Text = "    Open the online wiki";
            this.btnOpenOnlineWiki.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnOpenOnlineWiki.UseVisualStyleBackColor = false;
            this.btnOpenOnlineWiki.Click += new System.EventHandler(this.btnOpenOnlineWiki_Click);
            // 
            // cbUncapIntensity
            // 
            this.cbUncapIntensity.AutoSize = true;
            this.cbUncapIntensity.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbUncapIntensity.ForeColor = System.Drawing.Color.White;
            this.cbUncapIntensity.Location = new System.Drawing.Point(11, 50);
            this.cbUncapIntensity.Name = "cbUncapIntensity";
            this.cbUncapIntensity.Size = new System.Drawing.Size(158, 17);
            this.cbUncapIntensity.TabIndex = 3;
            this.cbUncapIntensity.Text = "Uncap intensity box value";
            this.cbUncapIntensity.UseVisualStyleBackColor = true;
            this.cbUncapIntensity.CheckedChanged += new System.EventHandler(this.CbUncapIntensity_CheckedChanged);
            // 
            // RTC_SettingsGeneral_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(412, 352);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnImportKeyBindings);
            this.Controls.Add(this.btnChangeRTCColor);
            this.Controls.Add(this.btnOpenOnlineWiki);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "RTC_SettingsGeneral_Form";
            this.Tag = "color:dark";
            this.Text = "General";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HandleFormClosing);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

		#endregion

		public System.Windows.Forms.Button btnImportKeyBindings;
		public System.Windows.Forms.Button btnChangeRTCColor;
		public System.Windows.Forms.Button btnOpenOnlineWiki;
		private System.Windows.Forms.Panel panel1;
		public System.Windows.Forms.CheckBox cbDontCleanAtQuit;
		public System.Windows.Forms.CheckBox cbAllowCrossCoreCorruption;
		public System.Windows.Forms.CheckBox cbDisableBizhawkOSD;
		private System.Windows.Forms.Label label4;
		public System.Windows.Forms.CheckBox cbUncapIntensity;
	}
}