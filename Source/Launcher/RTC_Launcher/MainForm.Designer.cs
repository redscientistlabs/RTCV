namespace RTCV.Launcher
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.pnBottomPanel = new System.Windows.Forms.Panel();
            this.lbMOTD = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lbVersions = new System.Windows.Forms.ListBox();
            this.pnLeftSide = new System.Windows.Forms.Panel();
            this.pbNewVersionNotification = new System.Windows.Forms.PictureBox();
            this.btnVersionDownloader = new System.Windows.Forms.Button();
            this.pnTopPanel = new System.Windows.Forms.Panel();
            this.btnDiscord = new System.Windows.Forms.Button();
            this.btnMinimize = new System.Windows.Forms.Button();
            this.btnQuit = new System.Windows.Forms.Button();
            this.btnOnlineGuide = new System.Windows.Forms.Button();
            this.pnAnchorRight = new System.Windows.Forms.Panel();
            this.pnBottomPanel.SuspendLayout();
            this.pnLeftSide.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbNewVersionNotification)).BeginInit();
            this.pnTopPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnBottomPanel
            // 
            this.pnBottomPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.pnBottomPanel.Controls.Add(this.lbMOTD);
            this.pnBottomPanel.Controls.Add(this.label5);
            this.pnBottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnBottomPanel.Location = new System.Drawing.Point(139, 505);
            this.pnBottomPanel.Name = "pnBottomPanel";
            this.pnBottomPanel.Size = new System.Drawing.Size(1011, 45);
            this.pnBottomPanel.TabIndex = 0;
            // 
            // lbMOTD
            // 
            this.lbMOTD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbMOTD.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.lbMOTD.ForeColor = System.Drawing.Color.White;
            this.lbMOTD.Location = new System.Drawing.Point(2, 21);
            this.lbMOTD.Name = "lbMOTD";
            this.lbMOTD.Size = new System.Drawing.Size(1003, 18);
            this.lbMOTD.TabIndex = 125;
            this.lbMOTD.Text = "...";
            this.lbMOTD.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbMOTD.Visible = false;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.Font = new System.Drawing.Font("Segoe UI", 7F);
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(2, 3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(1003, 18);
            this.label5.TabIndex = 132;
            this.label5.Text = "RTC, emulator mods and stubs are developed by Redscientist Media, consult redscie" +
    "ntist.com for more details";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbVersions
            // 
            this.lbVersions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbVersions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.lbVersions.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbVersions.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Bold);
            this.lbVersions.ForeColor = System.Drawing.Color.White;
            this.lbVersions.FormattingEnabled = true;
            this.lbVersions.IntegralHeight = false;
            this.lbVersions.ItemHeight = 23;
            this.lbVersions.Location = new System.Drawing.Point(10, 7);
            this.lbVersions.Margin = new System.Windows.Forms.Padding(14, 3, 3, 3);
            this.lbVersions.Name = "lbVersions";
            this.lbVersions.Size = new System.Drawing.Size(120, 452);
            this.lbVersions.TabIndex = 82;
            this.lbVersions.Tag = "";
            this.lbVersions.SelectedIndexChanged += new System.EventHandler(this.lbVersions_SelectedIndexChanged);
            this.lbVersions.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbVersions_MouseDown);
            // 
            // pnLeftSide
            // 
            this.pnLeftSide.AutoSize = true;
            this.pnLeftSide.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.pnLeftSide.Controls.Add(this.pbNewVersionNotification);
            this.pnLeftSide.Controls.Add(this.btnVersionDownloader);
            this.pnLeftSide.Controls.Add(this.lbVersions);
            this.pnLeftSide.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnLeftSide.Location = new System.Drawing.Point(0, 41);
            this.pnLeftSide.Name = "pnLeftSide";
            this.pnLeftSide.Size = new System.Drawing.Size(139, 509);
            this.pnLeftSide.TabIndex = 134;
            // 
            // pbNewVersionNotification
            // 
            this.pbNewVersionNotification.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbNewVersionNotification.Image = global::RTCV.Launcher.Properties.Resources.notificationBadge;
            this.pbNewVersionNotification.InitialImage = global::RTCV.Launcher.Properties.Resources.notificationBadge;
            this.pbNewVersionNotification.Location = new System.Drawing.Point(109, 467);
            this.pbNewVersionNotification.Name = "pbNewVersionNotification";
            this.pbNewVersionNotification.Size = new System.Drawing.Size(14, 14);
            this.pbNewVersionNotification.TabIndex = 131;
            this.pbNewVersionNotification.TabStop = false;
            this.pbNewVersionNotification.Visible = false;
            // 
            // btnVersionDownloader
            // 
            this.btnVersionDownloader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnVersionDownloader.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnVersionDownloader.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnVersionDownloader.FlatAppearance.BorderSize = 0;
            this.btnVersionDownloader.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVersionDownloader.Font = new System.Drawing.Font("Segoe UI Light", 12.25F);
            this.btnVersionDownloader.ForeColor = System.Drawing.Color.White;
            this.btnVersionDownloader.Location = new System.Drawing.Point(0, 464);
            this.btnVersionDownloader.Name = "btnVersionDownloader";
            this.btnVersionDownloader.Size = new System.Drawing.Size(139, 45);
            this.btnVersionDownloader.TabIndex = 132;
            this.btnVersionDownloader.TabStop = false;
            this.btnVersionDownloader.Tag = "";
            this.btnVersionDownloader.Text = "Downloader";
            this.btnVersionDownloader.UseVisualStyleBackColor = false;
            this.btnVersionDownloader.Click += new System.EventHandler(this.btnVersionDownloader_Click);
            // 
            // pnTopPanel
            // 
            this.pnTopPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.pnTopPanel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnTopPanel.BackgroundImage")));
            this.pnTopPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pnTopPanel.Controls.Add(this.btnDiscord);
            this.pnTopPanel.Controls.Add(this.btnMinimize);
            this.pnTopPanel.Controls.Add(this.btnQuit);
            this.pnTopPanel.Controls.Add(this.btnOnlineGuide);
            this.pnTopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnTopPanel.Location = new System.Drawing.Point(0, 0);
            this.pnTopPanel.Name = "pnTopPanel";
            this.pnTopPanel.Size = new System.Drawing.Size(1150, 41);
            this.pnTopPanel.TabIndex = 131;
            // 
            // btnDiscord
            // 
            this.btnDiscord.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDiscord.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnDiscord.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnDiscord.FlatAppearance.BorderSize = 0;
            this.btnDiscord.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDiscord.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.btnDiscord.ForeColor = System.Drawing.Color.White;
            this.btnDiscord.Image = global::RTCV.Launcher.Properties.Resources.discord;
            this.btnDiscord.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDiscord.Location = new System.Drawing.Point(908, 8);
            this.btnDiscord.Name = "btnDiscord";
            this.btnDiscord.Size = new System.Drawing.Size(73, 24);
            this.btnDiscord.TabIndex = 131;
            this.btnDiscord.TabStop = false;
            this.btnDiscord.Tag = "";
            this.btnDiscord.Text = " Discord";
            this.btnDiscord.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDiscord.UseVisualStyleBackColor = false;
            this.btnDiscord.Click += new System.EventHandler(this.btnDiscord_Click);
            // 
            // btnMinimize
            // 
            this.btnMinimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMinimize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnMinimize.FlatAppearance.BorderSize = 0;
            this.btnMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            this.btnMinimize.ForeColor = System.Drawing.Color.White;
            this.btnMinimize.Location = new System.Drawing.Point(1092, 8);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(22, 24);
            this.btnMinimize.TabIndex = 131;
            this.btnMinimize.TabStop = false;
            this.btnMinimize.Tag = "";
            this.btnMinimize.Text = "_";
            this.btnMinimize.UseVisualStyleBackColor = false;
            this.btnMinimize.Click += new System.EventHandler(this.btnMinimize_Click);
            // 
            // btnQuit
            // 
            this.btnQuit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnQuit.FlatAppearance.BorderSize = 0;
            this.btnQuit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQuit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            this.btnQuit.ForeColor = System.Drawing.Color.White;
            this.btnQuit.Location = new System.Drawing.Point(1120, 8);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(22, 24);
            this.btnQuit.TabIndex = 130;
            this.btnQuit.TabStop = false;
            this.btnQuit.Tag = "";
            this.btnQuit.Text = "X";
            this.btnQuit.UseVisualStyleBackColor = false;
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            this.btnQuit.MouseEnter += new System.EventHandler(this.btnQuit_MouseEnter);
            this.btnQuit.MouseLeave += new System.EventHandler(this.btnQuit_MouseLeave);
            // 
            // btnOnlineGuide
            // 
            this.btnOnlineGuide.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOnlineGuide.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnOnlineGuide.FlatAppearance.BorderSize = 0;
            this.btnOnlineGuide.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOnlineGuide.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.btnOnlineGuide.ForeColor = System.Drawing.Color.White;
            this.btnOnlineGuide.Image = global::RTCV.Launcher.Properties.Resources.corruptwiki;
            this.btnOnlineGuide.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOnlineGuide.Location = new System.Drawing.Point(987, 8);
            this.btnOnlineGuide.Name = "btnOnlineGuide";
            this.btnOnlineGuide.Size = new System.Drawing.Size(99, 24);
            this.btnOnlineGuide.TabIndex = 129;
            this.btnOnlineGuide.TabStop = false;
            this.btnOnlineGuide.Tag = "";
            this.btnOnlineGuide.Text = " Online guide";
            this.btnOnlineGuide.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOnlineGuide.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnOnlineGuide.UseVisualStyleBackColor = false;
            this.btnOnlineGuide.Click += new System.EventHandler(this.btnOnlineGuide_Click);
            // 
            // pnAnchorRight
            // 
            this.pnAnchorRight.AutoSize = true;
            this.pnAnchorRight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.pnAnchorRight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pnAnchorRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnAnchorRight.Location = new System.Drawing.Point(139, 41);
            this.pnAnchorRight.Name = "pnAnchorRight";
            this.pnAnchorRight.Size = new System.Drawing.Size(1011, 464);
            this.pnAnchorRight.TabIndex = 133;
            this.pnAnchorRight.Paint += new System.Windows.Forms.PaintEventHandler(this.pnAnchorRight_Paint);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(1150, 550);
            this.Controls.Add(this.pnAnchorRight);
            this.Controls.Add(this.pnBottomPanel);
            this.Controls.Add(this.pnLeftSide);
            this.Controls.Add(this.pnTopPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(680, 550);
            this.Name = "MainForm";
            this.Tag = "";
            this.Text = "RTC Launcher";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.pnBottomPanel.ResumeLayout(false);
            this.pnLeftSide.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbNewVersionNotification)).EndInit();
            this.pnTopPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnBottomPanel;
        private System.Windows.Forms.Label lbMOTD;
        public System.Windows.Forms.ListBox lbVersions;
        private System.Windows.Forms.Panel pnTopPanel;
        private System.Windows.Forms.Button btnOnlineGuide;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.Panel pnLeftSide;
        private System.Windows.Forms.Button btnQuit;
        private System.Windows.Forms.Button btnMinimize;
        private System.Windows.Forms.Button btnDiscord;
        private System.Windows.Forms.Panel pnAnchorRight;
        private System.Windows.Forms.PictureBox pbNewVersionNotification;
        public System.Windows.Forms.Button btnVersionDownloader;
    }
}

