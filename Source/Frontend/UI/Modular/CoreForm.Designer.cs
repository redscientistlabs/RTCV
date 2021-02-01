namespace RTCV.UI
{
    partial class CoreForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CoreForm));
            this.pnSideBar = new System.Windows.Forms.Panel();
            this.pnAddon = new System.Windows.Forms.Panel();
            this.pnGlitchHarvesterOpen = new System.Windows.Forms.Panel();
            this.pnCrashProtection = new System.Windows.Forms.Panel();
            this.btnGpJumpNow = new System.Windows.Forms.Button();
            this.btnGpJumpBack = new System.Windows.Forms.Button();
            this.lbGameProtection = new System.Windows.Forms.Label();
            this.cbUseGameProtection = new System.Windows.Forms.CheckBox();
            this.btnOpenCustomLayout = new System.Windows.Forms.Button();
            this.pnAutoKillSwitch = new System.Windows.Forms.Panel();
            this.cbUseAutoKillSwitch = new System.Windows.Forms.CheckBox();
            this.pbAutoKillSwitchTimeout = new System.Windows.Forms.ProgressBar();
            this.lbAks = new System.Windows.Forms.Label();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnLogo = new System.Windows.Forms.Button();
            this.btnEngineConfig = new System.Windows.Forms.Button();
            this.btnEasyMode = new System.Windows.Forms.Button();
            this.btnStockpilePlayer = new System.Windows.Forms.Button();
            this.btnGlitchHarvester = new System.Windows.Forms.Button();
            this.btnManualBlast = new System.Windows.Forms.Button();
            this.btnAutoCorrupt = new System.Windows.Forms.Button();
            this.pnSideBar.SuspendLayout();
            this.pnCrashProtection.SuspendLayout();
            this.pnAutoKillSwitch.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnSideBar
            // 
            this.pnSideBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.pnSideBar.Controls.Add(this.pnAddon);
            this.pnSideBar.Controls.Add(this.pnGlitchHarvesterOpen);
            this.pnSideBar.Controls.Add(this.pnCrashProtection);
            this.pnSideBar.Controls.Add(this.btnOpenCustomLayout);
            this.pnSideBar.Controls.Add(this.pnAutoKillSwitch);
            this.pnSideBar.Controls.Add(this.btnSettings);
            this.pnSideBar.Controls.Add(this.btnLogo);
            this.pnSideBar.Controls.Add(this.btnEngineConfig);
            this.pnSideBar.Controls.Add(this.btnEasyMode);
            this.pnSideBar.Controls.Add(this.btnStockpilePlayer);
            this.pnSideBar.Controls.Add(this.btnGlitchHarvester);
            this.pnSideBar.Controls.Add(this.btnManualBlast);
            this.pnSideBar.Controls.Add(this.btnAutoCorrupt);
            this.pnSideBar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnSideBar.Location = new System.Drawing.Point(0, 0);
            this.pnSideBar.Name = "pnSideBar";
            this.pnSideBar.Size = new System.Drawing.Size(150, 581);
            this.pnSideBar.TabIndex = 7;
            this.pnSideBar.Tag = "color:dark3";
            // 
            // pnAddon
            // 
            this.pnAddon.Location = new System.Drawing.Point(0, 321);
            this.pnAddon.Name = "pnAddon";
            this.pnAddon.Size = new System.Drawing.Size(150, 118);
            this.pnAddon.TabIndex = 129;
            this.pnAddon.Tag = "color:dark3";
            // 
            // pnGlitchHarvesterOpen
            // 
            this.pnGlitchHarvesterOpen.BackColor = System.Drawing.Color.Gray;
            this.pnGlitchHarvesterOpen.Location = new System.Drawing.Point(-19, 137);
            this.pnGlitchHarvesterOpen.Name = "pnGlitchHarvesterOpen";
            this.pnGlitchHarvesterOpen.Size = new System.Drawing.Size(23, 25);
            this.pnGlitchHarvesterOpen.TabIndex = 8;
            this.pnGlitchHarvesterOpen.Tag = "color:light1";
            this.pnGlitchHarvesterOpen.Visible = false;
            // 
            // pnCrashProtection
            // 
            this.pnCrashProtection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pnCrashProtection.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.pnCrashProtection.Controls.Add(this.btnGpJumpNow);
            this.pnCrashProtection.Controls.Add(this.btnGpJumpBack);
            this.pnCrashProtection.Controls.Add(this.lbGameProtection);
            this.pnCrashProtection.Controls.Add(this.cbUseGameProtection);
            this.pnCrashProtection.Location = new System.Drawing.Point(0, 440);
            this.pnCrashProtection.Name = "pnCrashProtection";
            this.pnCrashProtection.Size = new System.Drawing.Size(150, 60);
            this.pnCrashProtection.TabIndex = 128;
            this.pnCrashProtection.Tag = "color:dark3";
            this.pnCrashProtection.Visible = false;
            this.pnCrashProtection.MouseEnter += new System.EventHandler(this.pnCrashProtection_MouseEnter);
            this.pnCrashProtection.MouseLeave += new System.EventHandler(this.pnCrashProtection_MouseLeave);
            this.pnCrashProtection.MouseHover += new System.EventHandler(this.pnCrashProtection_MouseHover);
            // 
            // btnGpJumpNow
            // 
            this.btnGpJumpNow.BackColor = System.Drawing.Color.Transparent;
            this.btnGpJumpNow.FlatAppearance.BorderSize = 0;
            this.btnGpJumpNow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGpJumpNow.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.btnGpJumpNow.ForeColor = System.Drawing.Color.White;
            this.btnGpJumpNow.Image = global::RTCV.UI.Properties.Resources.playback_ff_icon_16;
            this.btnGpJumpNow.Location = new System.Drawing.Point(80, 30);
            this.btnGpJumpNow.Name = "btnGpJumpNow";
            this.btnGpJumpNow.Size = new System.Drawing.Size(60, 23);
            this.btnGpJumpNow.TabIndex = 117;
            this.btnGpJumpNow.TabStop = false;
            this.btnGpJumpNow.Tag = "color:dark2";
            this.btnGpJumpNow.Text = "Now ";
            this.btnGpJumpNow.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btnGpJumpNow.UseVisualStyleBackColor = false;
            this.btnGpJumpNow.Visible = false;
            this.btnGpJumpNow.Click += new System.EventHandler(this.OnGameProtectionNow);
            this.btnGpJumpNow.MouseEnter += new System.EventHandler(this.pnCrashProtection_MouseHover);
            this.btnGpJumpNow.MouseLeave += new System.EventHandler(this.pnCrashProtection_MouseLeave);
            this.btnGpJumpNow.MouseHover += new System.EventHandler(this.pnCrashProtection_MouseHover);
            // 
            // btnGpJumpBack
            // 
            this.btnGpJumpBack.BackColor = System.Drawing.Color.Transparent;
            this.btnGpJumpBack.FlatAppearance.BorderSize = 0;
            this.btnGpJumpBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGpJumpBack.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.btnGpJumpBack.ForeColor = System.Drawing.Color.White;
            this.btnGpJumpBack.Image = global::RTCV.UI.Properties.Resources.playback_rew_icon_16;
            this.btnGpJumpBack.Location = new System.Drawing.Point(10, 30);
            this.btnGpJumpBack.Name = "btnGpJumpBack";
            this.btnGpJumpBack.Size = new System.Drawing.Size(64, 23);
            this.btnGpJumpBack.TabIndex = 116;
            this.btnGpJumpBack.TabStop = false;
            this.btnGpJumpBack.Tag = "color:dark2";
            this.btnGpJumpBack.Text = " Back";
            this.btnGpJumpBack.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnGpJumpBack.UseVisualStyleBackColor = false;
            this.btnGpJumpBack.Visible = false;
            this.btnGpJumpBack.Click += new System.EventHandler(this.OnGameProtectionBack);
            this.btnGpJumpBack.MouseEnter += new System.EventHandler(this.pnCrashProtection_MouseHover);
            this.btnGpJumpBack.MouseLeave += new System.EventHandler(this.pnCrashProtection_MouseLeave);
            this.btnGpJumpBack.MouseHover += new System.EventHandler(this.pnCrashProtection_MouseHover);
            // 
            // lbGameProtection
            // 
            this.lbGameProtection.AutoSize = true;
            this.lbGameProtection.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lbGameProtection.ForeColor = System.Drawing.Color.White;
            this.lbGameProtection.Location = new System.Drawing.Point(28, 8);
            this.lbGameProtection.Name = "lbGameProtection";
            this.lbGameProtection.Size = new System.Drawing.Size(105, 17);
            this.lbGameProtection.TabIndex = 111;
            this.lbGameProtection.Text = "Game Protection";
            this.lbGameProtection.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ToggleGameProtection);
            this.lbGameProtection.MouseEnter += new System.EventHandler(this.pnCrashProtection_MouseHover);
            this.lbGameProtection.MouseLeave += new System.EventHandler(this.pnCrashProtection_MouseLeave);
            this.lbGameProtection.MouseHover += new System.EventHandler(this.pnCrashProtection_MouseHover);
            // 
            // cbUseGameProtection
            // 
            this.cbUseGameProtection.AutoSize = true;
            this.cbUseGameProtection.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbUseGameProtection.ForeColor = System.Drawing.Color.White;
            this.cbUseGameProtection.Location = new System.Drawing.Point(10, 11);
            this.cbUseGameProtection.Name = "cbUseGameProtection";
            this.cbUseGameProtection.Size = new System.Drawing.Size(15, 14);
            this.cbUseGameProtection.TabIndex = 76;
            this.cbUseGameProtection.UseVisualStyleBackColor = true;
            this.cbUseGameProtection.CheckedChanged += new System.EventHandler(this.OnUseGameProtectionCheckboxChanged);
            this.cbUseGameProtection.MouseEnter += new System.EventHandler(this.pnCrashProtection_MouseHover);
            this.cbUseGameProtection.MouseLeave += new System.EventHandler(this.pnCrashProtection_MouseLeave);
            this.cbUseGameProtection.MouseHover += new System.EventHandler(this.pnCrashProtection_MouseHover);
            // 
            // btnOpenCustomLayout
            // 
            this.btnOpenCustomLayout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.btnOpenCustomLayout.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnOpenCustomLayout.FlatAppearance.BorderSize = 0;
            this.btnOpenCustomLayout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenCustomLayout.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.btnOpenCustomLayout.ForeColor = System.Drawing.Color.White;
            this.btnOpenCustomLayout.Image = global::RTCV.UI.Properties.Resources.Layout;
            this.btnOpenCustomLayout.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOpenCustomLayout.Location = new System.Drawing.Point(0, 208);
            this.btnOpenCustomLayout.Name = "btnOpenCustomLayout";
            this.btnOpenCustomLayout.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.btnOpenCustomLayout.Size = new System.Drawing.Size(150, 33);
            this.btnOpenCustomLayout.TabIndex = 9;
            this.btnOpenCustomLayout.TabStop = false;
            this.btnOpenCustomLayout.Tag = "color:dark3";
            this.btnOpenCustomLayout.Text = " Custom Layout";
            this.btnOpenCustomLayout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOpenCustomLayout.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnOpenCustomLayout.UseVisualStyleBackColor = false;
            this.btnOpenCustomLayout.Visible = false;
            this.btnOpenCustomLayout.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OpenCustomLayout);
            // 
            // pnAutoKillSwitch
            // 
            this.pnAutoKillSwitch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pnAutoKillSwitch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.pnAutoKillSwitch.Controls.Add(this.cbUseAutoKillSwitch);
            this.pnAutoKillSwitch.Controls.Add(this.pbAutoKillSwitchTimeout);
            this.pnAutoKillSwitch.Controls.Add(this.lbAks);
            this.pnAutoKillSwitch.Location = new System.Drawing.Point(0, 500);
            this.pnAutoKillSwitch.Name = "pnAutoKillSwitch";
            this.pnAutoKillSwitch.Size = new System.Drawing.Size(150, 47);
            this.pnAutoKillSwitch.TabIndex = 127;
            this.pnAutoKillSwitch.Tag = "color:dark3";
            this.pnAutoKillSwitch.Visible = false;
            this.pnAutoKillSwitch.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnAutoKillSwitchClick);
            this.pnAutoKillSwitch.MouseEnter += new System.EventHandler(this.OnAutoKillSwitchButtonMouseHover);
            this.pnAutoKillSwitch.MouseLeave += new System.EventHandler(this.OnAutoKillSwitchButtonMouseLeave);
            this.pnAutoKillSwitch.MouseHover += new System.EventHandler(this.OnAutoKillSwitchButtonMouseHover);
            // 
            // cbUseAutoKillSwitch
            // 
            this.cbUseAutoKillSwitch.AutoSize = true;
            this.cbUseAutoKillSwitch.Checked = true;
            this.cbUseAutoKillSwitch.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbUseAutoKillSwitch.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbUseAutoKillSwitch.ForeColor = System.Drawing.Color.White;
            this.cbUseAutoKillSwitch.Location = new System.Drawing.Point(10, 14);
            this.cbUseAutoKillSwitch.Name = "cbUseAutoKillSwitch";
            this.cbUseAutoKillSwitch.Size = new System.Drawing.Size(15, 14);
            this.cbUseAutoKillSwitch.TabIndex = 120;
            this.cbUseAutoKillSwitch.UseVisualStyleBackColor = true;
            this.cbUseAutoKillSwitch.CheckedChanged += new System.EventHandler(this.OnAutoKillSwitchCheckboxChanged);
            this.cbUseAutoKillSwitch.MouseEnter += new System.EventHandler(this.OnAutoKillSwitchButtonMouseHover);
            this.cbUseAutoKillSwitch.MouseLeave += new System.EventHandler(this.OnAutoKillSwitchButtonMouseLeave);
            this.cbUseAutoKillSwitch.MouseHover += new System.EventHandler(this.OnAutoKillSwitchButtonMouseHover);
            // 
            // pbAutoKillSwitchTimeout
            // 
            this.pbAutoKillSwitchTimeout.Location = new System.Drawing.Point(10, 32);
            this.pbAutoKillSwitchTimeout.MarqueeAnimationSpeed = 1;
            this.pbAutoKillSwitchTimeout.Maximum = 13;
            this.pbAutoKillSwitchTimeout.Name = "pbAutoKillSwitchTimeout";
            this.pbAutoKillSwitchTimeout.Size = new System.Drawing.Size(130, 4);
            this.pbAutoKillSwitchTimeout.Step = 1;
            this.pbAutoKillSwitchTimeout.TabIndex = 119;
            this.pbAutoKillSwitchTimeout.Tag = "17";
            this.pbAutoKillSwitchTimeout.Value = 13;
            this.pbAutoKillSwitchTimeout.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnAutoKillSwitchClick);
            this.pbAutoKillSwitchTimeout.MouseEnter += new System.EventHandler(this.OnAutoKillSwitchButtonMouseHover);
            this.pbAutoKillSwitchTimeout.MouseLeave += new System.EventHandler(this.OnAutoKillSwitchButtonMouseLeave);
            this.pbAutoKillSwitchTimeout.MouseHover += new System.EventHandler(this.OnAutoKillSwitchButtonMouseHover);
            // 
            // lbAks
            // 
            this.lbAks.AutoSize = true;
            this.lbAks.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lbAks.ForeColor = System.Drawing.Color.White;
            this.lbAks.Location = new System.Drawing.Point(28, 11);
            this.lbAks.Name = "lbAks";
            this.lbAks.Size = new System.Drawing.Size(93, 17);
            this.lbAks.TabIndex = 111;
            this.lbAks.Text = "Auto-KillSwitch";
            this.lbAks.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnAutoKillSwitchClick);
            this.lbAks.MouseEnter += new System.EventHandler(this.OnAutoKillSwitchButtonMouseHover);
            this.lbAks.MouseLeave += new System.EventHandler(this.OnAutoKillSwitchButtonMouseLeave);
            this.lbAks.MouseHover += new System.EventHandler(this.OnAutoKillSwitchButtonMouseHover);
            // 
            // btnSettings
            // 
            this.btnSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.btnSettings.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnSettings.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnSettings.FlatAppearance.BorderSize = 0;
            this.btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSettings.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.btnSettings.ForeColor = System.Drawing.Color.White;
            this.btnSettings.Image = ((System.Drawing.Image)(resources.GetObject("btnSettings.Image")));
            this.btnSettings.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSettings.Location = new System.Drawing.Point(0, 547);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.btnSettings.Size = new System.Drawing.Size(150, 34);
            this.btnSettings.TabIndex = 126;
            this.btnSettings.TabStop = false;
            this.btnSettings.Tag = "color:dark3";
            this.btnSettings.Text = " Settings and tools";
            this.btnSettings.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSettings.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSettings.UseVisualStyleBackColor = false;
            this.btnSettings.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OpenSettings);
            // 
            // btnLogo
            // 
            this.btnLogo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.btnLogo.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnLogo.FlatAppearance.BorderSize = 0;
            this.btnLogo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogo.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Bold);
            this.btnLogo.ForeColor = System.Drawing.Color.White;
            this.btnLogo.Location = new System.Drawing.Point(0, 0);
            this.btnLogo.Name = "btnLogo";
            this.btnLogo.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.btnLogo.Size = new System.Drawing.Size(150, 53);
            this.btnLogo.TabIndex = 125;
            this.btnLogo.TabStop = false;
            this.btnLogo.Tag = "color:dark3";
            this.btnLogo.Text = "RTCV 0.00";
            this.btnLogo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLogo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLogo.UseVisualStyleBackColor = false;
            this.btnLogo.Click += new System.EventHandler(this.OnLogoClick);
            // 
            // btnEngineConfig
            // 
            this.btnEngineConfig.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.btnEngineConfig.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnEngineConfig.FlatAppearance.BorderSize = 0;
            this.btnEngineConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEngineConfig.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.btnEngineConfig.ForeColor = System.Drawing.Color.White;
            this.btnEngineConfig.Image = ((System.Drawing.Image)(resources.GetObject("btnEngineConfig.Image")));
            this.btnEngineConfig.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEngineConfig.Location = new System.Drawing.Point(0, 94);
            this.btnEngineConfig.Name = "btnEngineConfig";
            this.btnEngineConfig.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.btnEngineConfig.Size = new System.Drawing.Size(150, 34);
            this.btnEngineConfig.TabIndex = 124;
            this.btnEngineConfig.TabStop = false;
            this.btnEngineConfig.Tag = "color:dark3";
            this.btnEngineConfig.Text = " Engine Config";
            this.btnEngineConfig.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEngineConfig.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEngineConfig.UseVisualStyleBackColor = false;
            this.btnEngineConfig.Visible = false;
            this.btnEngineConfig.Click += new System.EventHandler(this.OpenEngineConfig);
            // 
            // btnEasyMode
            // 
            this.btnEasyMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.btnEasyMode.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnEasyMode.FlatAppearance.BorderSize = 0;
            this.btnEasyMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEasyMode.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.btnEasyMode.ForeColor = System.Drawing.Color.White;
            this.btnEasyMode.Image = ((System.Drawing.Image)(resources.GetObject("btnEasyMode.Image")));
            this.btnEasyMode.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEasyMode.Location = new System.Drawing.Point(0, 56);
            this.btnEasyMode.Name = "btnEasyMode";
            this.btnEasyMode.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.btnEasyMode.Size = new System.Drawing.Size(150, 34);
            this.btnEasyMode.TabIndex = 121;
            this.btnEasyMode.TabStop = false;
            this.btnEasyMode.Tag = "color:dark3";
            this.btnEasyMode.Text = " Easy Start";
            this.btnEasyMode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEasyMode.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEasyMode.UseVisualStyleBackColor = false;
            this.btnEasyMode.Visible = false;
            this.btnEasyMode.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnStartEasyModeClick);
            // 
            // btnStockpilePlayer
            // 
            this.btnStockpilePlayer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.btnStockpilePlayer.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnStockpilePlayer.FlatAppearance.BorderSize = 0;
            this.btnStockpilePlayer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStockpilePlayer.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.btnStockpilePlayer.ForeColor = System.Drawing.Color.White;
            this.btnStockpilePlayer.Image = ((System.Drawing.Image)(resources.GetObject("btnStockpilePlayer.Image")));
            this.btnStockpilePlayer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnStockpilePlayer.Location = new System.Drawing.Point(0, 170);
            this.btnStockpilePlayer.Name = "btnStockpilePlayer";
            this.btnStockpilePlayer.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.btnStockpilePlayer.Size = new System.Drawing.Size(150, 34);
            this.btnStockpilePlayer.TabIndex = 123;
            this.btnStockpilePlayer.TabStop = false;
            this.btnStockpilePlayer.Tag = "color:dark3";
            this.btnStockpilePlayer.Text = " Stockpile Player";
            this.btnStockpilePlayer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnStockpilePlayer.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnStockpilePlayer.UseVisualStyleBackColor = false;
            this.btnStockpilePlayer.Visible = false;
            this.btnStockpilePlayer.Click += new System.EventHandler(this.OpenStockpilePlayer);
            // 
            // btnGlitchHarvester
            // 
            this.btnGlitchHarvester.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.btnGlitchHarvester.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnGlitchHarvester.FlatAppearance.BorderSize = 0;
            this.btnGlitchHarvester.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGlitchHarvester.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.btnGlitchHarvester.ForeColor = System.Drawing.Color.White;
            this.btnGlitchHarvester.Image = ((System.Drawing.Image)(resources.GetObject("btnGlitchHarvester.Image")));
            this.btnGlitchHarvester.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGlitchHarvester.Location = new System.Drawing.Point(0, 132);
            this.btnGlitchHarvester.Name = "btnGlitchHarvester";
            this.btnGlitchHarvester.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.btnGlitchHarvester.Size = new System.Drawing.Size(150, 34);
            this.btnGlitchHarvester.TabIndex = 122;
            this.btnGlitchHarvester.TabStop = false;
            this.btnGlitchHarvester.Tag = "color:dark3";
            this.btnGlitchHarvester.Text = " Glitch Harvester";
            this.btnGlitchHarvester.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGlitchHarvester.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnGlitchHarvester.UseVisualStyleBackColor = false;
            this.btnGlitchHarvester.Visible = false;
            this.btnGlitchHarvester.Click += new System.EventHandler(this.OpenGlitchHarvester);
            this.btnGlitchHarvester.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnGlitchHarvesterMouseDown);
            // 
            // btnManualBlast
            // 
            this.btnManualBlast.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.btnManualBlast.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnManualBlast.FlatAppearance.BorderSize = 0;
            this.btnManualBlast.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnManualBlast.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.btnManualBlast.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnManualBlast.Image = ((System.Drawing.Image)(resources.GetObject("btnManualBlast.Image")));
            this.btnManualBlast.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnManualBlast.Location = new System.Drawing.Point(0, 245);
            this.btnManualBlast.Name = "btnManualBlast";
            this.btnManualBlast.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.btnManualBlast.Size = new System.Drawing.Size(150, 34);
            this.btnManualBlast.TabIndex = 119;
            this.btnManualBlast.TabStop = false;
            this.btnManualBlast.Tag = "color:dark3";
            this.btnManualBlast.Text = " Manual Blast";
            this.btnManualBlast.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnManualBlast.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnManualBlast.UseVisualStyleBackColor = false;
            this.btnManualBlast.Visible = false;
            this.btnManualBlast.Click += new System.EventHandler(this.ManualBlast);
            this.btnManualBlast.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnManualBlast_MouseDown);
            // 
            // btnAutoCorrupt
            // 
            this.btnAutoCorrupt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.btnAutoCorrupt.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnAutoCorrupt.FlatAppearance.BorderSize = 0;
            this.btnAutoCorrupt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAutoCorrupt.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.btnAutoCorrupt.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnAutoCorrupt.Image = ((System.Drawing.Image)(resources.GetObject("btnAutoCorrupt.Image")));
            this.btnAutoCorrupt.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAutoCorrupt.Location = new System.Drawing.Point(0, 283);
            this.btnAutoCorrupt.Name = "btnAutoCorrupt";
            this.btnAutoCorrupt.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.btnAutoCorrupt.Size = new System.Drawing.Size(150, 34);
            this.btnAutoCorrupt.TabIndex = 120;
            this.btnAutoCorrupt.TabStop = false;
            this.btnAutoCorrupt.Tag = "color:dark3";
            this.btnAutoCorrupt.Text = " Start Auto-Corrupt";
            this.btnAutoCorrupt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAutoCorrupt.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAutoCorrupt.UseVisualStyleBackColor = false;
            this.btnAutoCorrupt.Visible = false;
            this.btnAutoCorrupt.Click += new System.EventHandler(this.StartAutoCorrupt);
            // 
            // CoreForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(844, 581);
            this.Controls.Add(this.pnSideBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "CoreForm";
            this.Tag = "color:dark2";
            this.Text = "Real-Time Corruptor";
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.ResizeBegin += new System.EventHandler(this.OnResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.OnResizeEnd);
            this.Resize += new System.EventHandler(this.OnResize);
            this.pnSideBar.ResumeLayout(false);
            this.pnCrashProtection.ResumeLayout(false);
            this.pnCrashProtection.PerformLayout();
            this.pnAutoKillSwitch.ResumeLayout(false);
            this.pnAutoKillSwitch.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.Button btnEasyMode;
        public System.Windows.Forms.Button btnStockpilePlayer;
        public System.Windows.Forms.Button btnGlitchHarvester;
        public System.Windows.Forms.Button btnManualBlast;
        public System.Windows.Forms.Button btnAutoCorrupt;
        public System.Windows.Forms.Button btnLogo;
        public System.Windows.Forms.Button btnSettings;
        public System.Windows.Forms.CheckBox cbUseAutoKillSwitch;
        public System.Windows.Forms.ProgressBar pbAutoKillSwitchTimeout;
        private System.Windows.Forms.Label lbAks;
        public System.Windows.Forms.Button btnGpJumpNow;
        public System.Windows.Forms.Button btnGpJumpBack;
        private System.Windows.Forms.Label lbGameProtection;
        public System.Windows.Forms.CheckBox cbUseGameProtection;
        public System.Windows.Forms.Panel pnGlitchHarvesterOpen;
        public System.Windows.Forms.Panel pnCrashProtection;
        public System.Windows.Forms.Panel pnAutoKillSwitch;
        public System.Windows.Forms.Panel pnSideBar;
        public System.Windows.Forms.Button btnOpenCustomLayout;
        public System.Windows.Forms.Button btnEngineConfig;
        public System.Windows.Forms.Panel pnAddon;
    }
}
