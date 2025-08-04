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
            this.label5 = new System.Windows.Forms.Label();
            this.nmSwapEmuTimeout = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.nmMaxAutosaveSize = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.nmAutosaveSeconds = new System.Windows.Forms.NumericUpDown();
            this.lbSeconds = new System.Windows.Forms.Label();
            this.nmAutosaveMinutes = new System.Windows.Forms.NumericUpDown();
            this.cbAutosave = new System.Windows.Forms.CheckBox();
            this.cbRasterizeUponStockpiling = new System.Windows.Forms.CheckBox();
            this.cbAutoUncorrupt = new System.Windows.Forms.CheckBox();
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
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmSwapEmuTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmMaxAutosaveSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmAutosaveSeconds)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmAutosaveMinutes)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gray;
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.nmSwapEmuTimeout);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.nmMaxAutosaveSize);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.nmAutosaveSeconds);
            this.panel1.Controls.Add(this.lbSeconds);
            this.panel1.Controls.Add(this.nmAutosaveMinutes);
            this.panel1.Controls.Add(this.cbAutosave);
            this.panel1.Controls.Add(this.cbRasterizeUponStockpiling);
            this.panel1.Controls.Add(this.cbAutoUncorrupt);
            this.panel1.Controls.Add(this.cbUncapIntensity);
            this.panel1.Controls.Add(this.cbDontCleanAtQuit);
            this.panel1.Controls.Add(this.cbAllowCrossCoreCorruption);
            this.panel1.Controls.Add(this.cbDisableEmulatorOSD);
            this.panel1.Location = new System.Drawing.Point(18, 197);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(323, 196);
            this.panel1.TabIndex = 138;
            this.panel1.Tag = "color:normal";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.label5.Location = new System.Drawing.Point(55, 170);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(218, 13);
            this.label5.TabIndex = 123;
            this.label5.Text = "seconds before swapping emulators fails";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // nmSwapEmuTimeout
            // 
            this.nmSwapEmuTimeout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nmSwapEmuTimeout.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.nmSwapEmuTimeout.ForeColor = System.Drawing.Color.White;
            this.nmSwapEmuTimeout.Location = new System.Drawing.Point(11, 168);
            this.nmSwapEmuTimeout.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.nmSwapEmuTimeout.Name = "nmSwapEmuTimeout";
            this.nmSwapEmuTimeout.Size = new System.Drawing.Size(44, 22);
            this.nmSwapEmuTimeout.TabIndex = 122;
            this.nmSwapEmuTimeout.Tag = "color:dark1";
            this.nmSwapEmuTimeout.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nmSwapEmuTimeout.ValueChanged += new System.EventHandler(this.nmSwapEmuTimeout_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.label3.Location = new System.Drawing.Point(223, 152);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(24, 13);
            this.label3.TabIndex = 121;
            this.label3.Text = "GiB";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // nmMaxAutosaveSize
            // 
            this.nmMaxAutosaveSize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nmMaxAutosaveSize.DecimalPlaces = 2;
            this.nmMaxAutosaveSize.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.nmMaxAutosaveSize.ForeColor = System.Drawing.Color.White;
            this.nmMaxAutosaveSize.Location = new System.Drawing.Point(172, 148);
            this.nmMaxAutosaveSize.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nmMaxAutosaveSize.Name = "nmMaxAutosaveSize";
            this.nmMaxAutosaveSize.Size = new System.Drawing.Size(51, 22);
            this.nmMaxAutosaveSize.TabIndex = 120;
            this.nmMaxAutosaveSize.Tag = "color:dark1";
            this.nmMaxAutosaveSize.Value = new decimal(new int[] {
            25,
            0,
            0,
            65536});
            this.nmMaxAutosaveSize.ValueChanged += new System.EventHandler(this.MaxAutosaveSizeChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.label2.Location = new System.Drawing.Point(8, 152);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(166, 13);
            this.label2.TabIndex = 119;
            this.label2.Text = "Max size per autosave file type:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.label1.Location = new System.Drawing.Point(259, 131);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 118;
            this.label1.Text = "seconds";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // nmAutosaveSeconds
            // 
            this.nmAutosaveSeconds.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nmAutosaveSeconds.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.nmAutosaveSeconds.ForeColor = System.Drawing.Color.White;
            this.nmAutosaveSeconds.Location = new System.Drawing.Point(226, 129);
            this.nmAutosaveSeconds.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.nmAutosaveSeconds.Name = "nmAutosaveSeconds";
            this.nmAutosaveSeconds.Size = new System.Drawing.Size(33, 22);
            this.nmAutosaveSeconds.TabIndex = 117;
            this.nmAutosaveSeconds.Tag = "color:dark1";
            this.nmAutosaveSeconds.ValueChanged += new System.EventHandler(this.AutosaveTimeChanged);
            // 
            // lbSeconds
            // 
            this.lbSeconds.AutoSize = true;
            this.lbSeconds.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lbSeconds.ForeColor = System.Drawing.Color.White;
            this.lbSeconds.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.lbSeconds.Location = new System.Drawing.Point(156, 131);
            this.lbSeconds.Name = "lbSeconds";
            this.lbSeconds.Size = new System.Drawing.Size(71, 13);
            this.lbSeconds.TabIndex = 116;
            this.lbSeconds.Text = "minutes and";
            this.lbSeconds.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // nmAutosaveMinutes
            // 
            this.nmAutosaveMinutes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nmAutosaveMinutes.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.nmAutosaveMinutes.ForeColor = System.Drawing.Color.White;
            this.nmAutosaveMinutes.Location = new System.Drawing.Point(113, 129);
            this.nmAutosaveMinutes.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nmAutosaveMinutes.Name = "nmAutosaveMinutes";
            this.nmAutosaveMinutes.Size = new System.Drawing.Size(43, 22);
            this.nmAutosaveMinutes.TabIndex = 115;
            this.nmAutosaveMinutes.Tag = "color:dark1";
            this.nmAutosaveMinutes.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nmAutosaveMinutes.ValueChanged += new System.EventHandler(this.AutosaveTimeChanged);
            // 
            // cbAutosave
            // 
            this.cbAutosave.AutoSize = true;
            this.cbAutosave.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbAutosave.ForeColor = System.Drawing.Color.White;
            this.cbAutosave.Location = new System.Drawing.Point(11, 130);
            this.cbAutosave.Name = "cbAutosave";
            this.cbAutosave.Size = new System.Drawing.Size(106, 17);
            this.cbAutosave.TabIndex = 6;
            this.cbAutosave.Text = "Auto-save every";
            this.cbAutosave.UseVisualStyleBackColor = true;
            this.cbAutosave.CheckedChanged += new System.EventHandler(this.cbAutosave_CheckedChanged);
            // 
            // cbRasterizeUponStockpiling
            // 
            this.cbRasterizeUponStockpiling.AutoSize = true;
            this.cbRasterizeUponStockpiling.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbRasterizeUponStockpiling.ForeColor = System.Drawing.Color.White;
            this.cbRasterizeUponStockpiling.Location = new System.Drawing.Point(11, 110);
            this.cbRasterizeUponStockpiling.Name = "cbRasterizeUponStockpiling";
            this.cbRasterizeUponStockpiling.Size = new System.Drawing.Size(250, 17);
            this.cbRasterizeUponStockpiling.TabIndex = 5;
            this.cbRasterizeUponStockpiling.Text = "Rasterize VMDs before sending to stockpile";
            this.cbRasterizeUponStockpiling.UseVisualStyleBackColor = true;
            this.cbRasterizeUponStockpiling.CheckedChanged += new System.EventHandler(this.HandleRasterizeUponStockpilingChange);
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
            this.label4.Location = new System.Drawing.Point(15, 179);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(322, 15);
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
            this.btnRefreshInputDevices.Location = new System.Drawing.Point(18, 120);
            this.btnRefreshInputDevices.Name = "btnRefreshInputDevices";
            this.btnRefreshInputDevices.Size = new System.Drawing.Size(220, 45);
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
            this.btnChangeRTCColor.Location = new System.Drawing.Point(18, 69);
            this.btnChangeRTCColor.Name = "btnChangeRTCColor";
            this.btnChangeRTCColor.Size = new System.Drawing.Size(220, 45);
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
            this.btnOpenOnlineWiki.Location = new System.Drawing.Point(18, 18);
            this.btnOpenOnlineWiki.Name = "btnOpenOnlineWiki";
            this.btnOpenOnlineWiki.Size = new System.Drawing.Size(220, 45);
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
            this.btnWatchTutorialVideo.Location = new System.Drawing.Point(244, 18);
            this.btnWatchTutorialVideo.Name = "btnWatchTutorialVideo";
            this.btnWatchTutorialVideo.Size = new System.Drawing.Size(219, 45);
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
            this.btnResetRandomSeed.Location = new System.Drawing.Point(244, 69);
            this.btnResetRandomSeed.Name = "btnResetRandomSeed";
            this.btnResetRandomSeed.Size = new System.Drawing.Size(219, 45);
            this.btnResetRandomSeed.TabIndex = 142;
            this.btnResetRandomSeed.Tag = "color:light1";
            this.btnResetRandomSeed.Text = "   Reset random seed";
            this.btnResetRandomSeed.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnResetRandomSeed.UseVisualStyleBackColor = false;
            this.btnResetRandomSeed.Click += new System.EventHandler(this.btnResetRandomSeed_Click);
            // 
            // SettingsGeneralForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.ClientSize = new System.Drawing.Size(481, 405);
            this.Controls.Add(this.btnResetRandomSeed);
            this.Controls.Add(this.btnWatchTutorialVideo);
            this.Controls.Add(this.btnRefreshInputDevices);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnChangeRTCColor);
            this.Controls.Add(this.btnOpenOnlineWiki);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimumSize = new System.Drawing.Size(481, 392);
            this.Name = "SettingsGeneralForm";
            this.Tag = "color:dark1";
            this.Text = "General";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HandleFormClosing);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmSwapEmuTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmMaxAutosaveSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmAutosaveSeconds)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmAutosaveMinutes)).EndInit();
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
        public System.Windows.Forms.CheckBox cbRasterizeUponStockpiling;
        public System.Windows.Forms.CheckBox cbAutosave;
        public System.Windows.Forms.NumericUpDown nmAutosaveMinutes;
        private System.Windows.Forms.Label lbSeconds;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.NumericUpDown nmAutosaveSeconds;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.NumericUpDown nmMaxAutosaveSize;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.NumericUpDown nmSwapEmuTimeout;
    }
}
