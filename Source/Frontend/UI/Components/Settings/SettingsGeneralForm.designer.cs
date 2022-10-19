namespace RTCV.UI
{
    partial class SettingsGeneralForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsGeneralForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbUncapIntensity = new System.Windows.Forms.CheckBox();
            this.cbDontCleanAtQuit = new System.Windows.Forms.CheckBox();
            this.cbAllowCrossCoreCorruption = new System.Windows.Forms.CheckBox();
            this.cbDisableEmulatorOSD = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnRefreshInputDevices = new System.Windows.Forms.Button();
            this.btnChangeRTCColor = new System.Windows.Forms.Button();
            this.btnOpenOnlineWiki = new System.Windows.Forms.Button();
            this.btnWatchTutorialVideo = new System.Windows.Forms.Button();
            this.btnResetRandomSeed = new System.Windows.Forms.Button();
            this.cbAutoUncorrupt = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gray;
            this.panel1.Controls.Add(this.cbAutoUncorrupt);
            this.panel1.Controls.Add(this.cbUncapIntensity);
            this.panel1.Controls.Add(this.cbDontCleanAtQuit);
            this.panel1.Controls.Add(this.cbAllowCrossCoreCorruption);
            this.panel1.Controls.Add(this.cbDisableEmulatorOSD);
            this.panel1.Location = new System.Drawing.Point(19, 228);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(323, 119);
            this.panel1.TabIndex = 138;
            this.panel1.Tag = "color:normal";
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
            this.cbUncapIntensity.CheckedChanged += new System.EventHandler(this.HandleUncapIntensityChange);
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
            this.cbDontCleanAtQuit.CheckedChanged += new System.EventHandler(this.HandleDontCleanAtQuitChange);
            // 
            // cbAllowCrossCoreCorruption
            // 
            this.cbAllowCrossCoreCorruption.AutoSize = true;
            this.cbAllowCrossCoreCorruption.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbAllowCrossCoreCorruption.ForeColor = System.Drawing.Color.White;
            this.cbAllowCrossCoreCorruption.Location = new System.Drawing.Point(11, 30);
            this.cbAllowCrossCoreCorruption.Name = "cbAllowCrossCoreCorruption";
            this.cbAllowCrossCoreCorruption.Size = new System.Drawing.Size(243, 17);
            this.cbAllowCrossCoreCorruption.TabIndex = 1;
            this.cbAllowCrossCoreCorruption.Text = "Allow Cross-Core / Cross-Game corruption";
            this.cbAllowCrossCoreCorruption.UseVisualStyleBackColor = true;
            this.cbAllowCrossCoreCorruption.CheckedChanged += new System.EventHandler(this.HandleAllowCrossCoreCorruptionChange);
            // 
            // cbDisableEmulatorOSD
            // 
            this.cbDisableEmulatorOSD.AutoSize = true;
            this.cbDisableEmulatorOSD.Checked = true;
            this.cbDisableEmulatorOSD.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbDisableEmulatorOSD.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbDisableEmulatorOSD.ForeColor = System.Drawing.Color.White;
            this.cbDisableEmulatorOSD.Location = new System.Drawing.Point(11, 10);
            this.cbDisableEmulatorOSD.Name = "cbDisableEmulatorOSD";
            this.cbDisableEmulatorOSD.Size = new System.Drawing.Size(196, 17);
            this.cbDisableEmulatorOSD.TabIndex = 0;
            this.cbDisableEmulatorOSD.Text = "Disable the emulator OSD system";
            this.cbDisableEmulatorOSD.UseVisualStyleBackColor = true;
            this.cbDisableEmulatorOSD.CheckedChanged += new System.EventHandler(this.HandleDisableBizhawkOSDChange);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(16, 210);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(326, 15);
            this.label4.TabIndex = 139;
            this.label4.Text = "General RTC Settings";
            // 
            // btnRefreshInputDevices
            // 
            this.btnRefreshInputDevices.BackColor = System.Drawing.Color.Gray;
            this.btnRefreshInputDevices.FlatAppearance.BorderSize = 0;
            this.btnRefreshInputDevices.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefreshInputDevices.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnRefreshInputDevices.ForeColor = System.Drawing.Color.White;
            this.btnRefreshInputDevices.Image = ((System.Drawing.Image)(resources.GetObject("btnRefreshInputDevices.Image")));
            this.btnRefreshInputDevices.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnRefreshInputDevices.Location = new System.Drawing.Point(19, 142);
            this.btnRefreshInputDevices.Name = "btnRefreshInputDevices";
            this.btnRefreshInputDevices.Size = new System.Drawing.Size(215, 45);
            this.btnRefreshInputDevices.TabIndex = 140;
            this.btnRefreshInputDevices.Tag = "color:light1";
            this.btnRefreshInputDevices.Text = "   Refresh Input Devices";
            this.btnRefreshInputDevices.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRefreshInputDevices.UseVisualStyleBackColor = false;
            this.btnRefreshInputDevices.Click += new System.EventHandler(this.RefreshInputDevices);
            // 
            // btnChangeRTCColor
            // 
            this.btnChangeRTCColor.BackColor = System.Drawing.Color.Gray;
            this.btnChangeRTCColor.FlatAppearance.BorderSize = 0;
            this.btnChangeRTCColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChangeRTCColor.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnChangeRTCColor.ForeColor = System.Drawing.Color.White;
            this.btnChangeRTCColor.Image = ((System.Drawing.Image)(resources.GetObject("btnChangeRTCColor.Image")));
            this.btnChangeRTCColor.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnChangeRTCColor.Location = new System.Drawing.Point(19, 91);
            this.btnChangeRTCColor.Name = "btnChangeRTCColor";
            this.btnChangeRTCColor.Size = new System.Drawing.Size(215, 45);
            this.btnChangeRTCColor.TabIndex = 136;
            this.btnChangeRTCColor.Tag = "color:light1";
            this.btnChangeRTCColor.Text = "   Change color theme";
            this.btnChangeRTCColor.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnChangeRTCColor.UseVisualStyleBackColor = false;
            this.btnChangeRTCColor.Click += new System.EventHandler(this.ChangeRTCColor);
            // 
            // btnOpenOnlineWiki
            // 
            this.btnOpenOnlineWiki.BackColor = System.Drawing.Color.Gray;
            this.btnOpenOnlineWiki.FlatAppearance.BorderSize = 0;
            this.btnOpenOnlineWiki.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenOnlineWiki.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnOpenOnlineWiki.ForeColor = System.Drawing.Color.White;
            this.btnOpenOnlineWiki.Image = ((System.Drawing.Image)(resources.GetObject("btnOpenOnlineWiki.Image")));
            this.btnOpenOnlineWiki.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOpenOnlineWiki.Location = new System.Drawing.Point(19, 40);
            this.btnOpenOnlineWiki.Name = "btnOpenOnlineWiki";
            this.btnOpenOnlineWiki.Size = new System.Drawing.Size(215, 45);
            this.btnOpenOnlineWiki.TabIndex = 135;
            this.btnOpenOnlineWiki.Tag = "color:light1";
            this.btnOpenOnlineWiki.Text = "    Open the online wiki";
            this.btnOpenOnlineWiki.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnOpenOnlineWiki.UseVisualStyleBackColor = false;
            this.btnOpenOnlineWiki.Click += new System.EventHandler(this.OpenOnlineWiki);
            // 
            // btnWatchTutorialVideo
            // 
            this.btnWatchTutorialVideo.BackColor = System.Drawing.Color.Gray;
            this.btnWatchTutorialVideo.FlatAppearance.BorderSize = 0;
            this.btnWatchTutorialVideo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWatchTutorialVideo.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnWatchTutorialVideo.ForeColor = System.Drawing.Color.White;
            this.btnWatchTutorialVideo.Image = ((System.Drawing.Image)(resources.GetObject("btnWatchTutorialVideo.Image")));
            this.btnWatchTutorialVideo.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnWatchTutorialVideo.Location = new System.Drawing.Point(240, 40);
            this.btnWatchTutorialVideo.Name = "btnWatchTutorialVideo";
            this.btnWatchTutorialVideo.Size = new System.Drawing.Size(215, 45);
            this.btnWatchTutorialVideo.TabIndex = 141;
            this.btnWatchTutorialVideo.Tag = "color:light1";
            this.btnWatchTutorialVideo.Text = "    Watch a tutorial video";
            this.btnWatchTutorialVideo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnWatchTutorialVideo.UseVisualStyleBackColor = false;
            this.btnWatchTutorialVideo.Click += new System.EventHandler(this.btnWatchTutorialVideo_Click);
            // 
            // btnResetRandomSeed
            // 
            this.btnResetRandomSeed.BackColor = System.Drawing.Color.Gray;
            this.btnResetRandomSeed.FlatAppearance.BorderSize = 0;
            this.btnResetRandomSeed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResetRandomSeed.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnResetRandomSeed.ForeColor = System.Drawing.Color.White;
            this.btnResetRandomSeed.Image = ((System.Drawing.Image)(resources.GetObject("btnResetRandomSeed.Image")));
            this.btnResetRandomSeed.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnResetRandomSeed.Location = new System.Drawing.Point(240, 91);
            this.btnResetRandomSeed.Name = "btnResetRandomSeed";
            this.btnResetRandomSeed.Size = new System.Drawing.Size(215, 45);
            this.btnResetRandomSeed.TabIndex = 142;
            this.btnResetRandomSeed.Tag = "color:light1";
            this.btnResetRandomSeed.Text = "   Reset random seed";
            this.btnResetRandomSeed.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnResetRandomSeed.UseVisualStyleBackColor = false;
            this.btnResetRandomSeed.Click += new System.EventHandler(this.btnResetRandomSeed_Click);
            // 
            // cbAutoUncorrupt
            // 
            this.cbAutoUncorrupt.AutoSize = true;
            this.cbAutoUncorrupt.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbAutoUncorrupt.ForeColor = System.Drawing.Color.White;
            this.cbAutoUncorrupt.Location = new System.Drawing.Point(11, 90);
            this.cbAutoUncorrupt.Name = "cbAutoUncorrupt";
            this.cbAutoUncorrupt.Size = new System.Drawing.Size(221, 17);
            this.cbAutoUncorrupt.TabIndex = 4;
            this.cbAutoUncorrupt.Text = "Enable Auto-Uncorrupt (Experimental)";
            this.cbAutoUncorrupt.UseVisualStyleBackColor = true;
            this.cbAutoUncorrupt.CheckedChanged += new System.EventHandler(this.cbAutoUncorrupt_CheckedChanged);
            // 
            // SettingsGeneralForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.ClientSize = new System.Drawing.Size(485, 387);
            this.Controls.Add(this.btnResetRandomSeed);
            this.Controls.Add(this.btnWatchTutorialVideo);
            this.Controls.Add(this.btnRefreshInputDevices);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnChangeRTCColor);
            this.Controls.Add(this.btnOpenOnlineWiki);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SettingsGeneralForm";
            this.Tag = "color:dark1";
            this.Text = "General";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HandleFormClosing);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

		#endregion
		public System.Windows.Forms.Button btnChangeRTCColor;
		public System.Windows.Forms.Button btnOpenOnlineWiki;
		private System.Windows.Forms.Panel panel1;
		public System.Windows.Forms.CheckBox cbDontCleanAtQuit;
		public System.Windows.Forms.CheckBox cbAllowCrossCoreCorruption;
		public System.Windows.Forms.CheckBox cbDisableEmulatorOSD;
		private System.Windows.Forms.Label label4;
		public System.Windows.Forms.CheckBox cbUncapIntensity;
        public System.Windows.Forms.Button btnRefreshInputDevices;
        public System.Windows.Forms.Button btnWatchTutorialVideo;
        public System.Windows.Forms.Button btnResetRandomSeed;
        public System.Windows.Forms.CheckBox cbAutoUncorrupt;
    }
}
