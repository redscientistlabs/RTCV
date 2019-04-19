namespace RTC_Launcher
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
            this.label5 = new System.Windows.Forms.Label();
            this.lbMOTD = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbVersions = new System.Windows.Forms.ListBox();
            this.pnLeftSide = new System.Windows.Forms.Panel();
            this.btnVersionDownloader = new System.Windows.Forms.Button();
            this.pbNewVersionNotification = new System.Windows.Forms.PictureBox();
            this.pnAnchorRight = new System.Windows.Forms.Panel();
            this.pnTopPanel = new System.Windows.Forms.Panel();
            this.btnDiscord = new System.Windows.Forms.Button();
            this.btnMinimize = new System.Windows.Forms.Button();
            this.btnQuit = new System.Windows.Forms.Button();
            this.btnOnlineGuide = new System.Windows.Forms.Button();
            this.pnBottomPanel.SuspendLayout();
            this.pnLeftSide.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbNewVersionNotification)).BeginInit();
            this.pnTopPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnBottomPanel
            // 
            this.pnBottomPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.pnBottomPanel.Controls.Add(this.label5);
            this.pnBottomPanel.Controls.Add(this.lbMOTD);
            this.pnBottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnBottomPanel.Location = new System.Drawing.Point(0, 492);
            this.pnBottomPanel.Name = "pnBottomPanel";
            this.pnBottomPanel.Size = new System.Drawing.Size(1138, 60);
            this.pnBottomPanel.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(15, 5);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(1111, 23);
            this.label5.TabIndex = 132;
            this.label5.Text = "RTC and WGH are developed by Redscientist Media, consult redscientist.com for mor" +
    "e details";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbMOTD
            // 
            this.lbMOTD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbMOTD.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbMOTD.ForeColor = System.Drawing.Color.White;
            this.lbMOTD.Location = new System.Drawing.Point(9, 30);
            this.lbMOTD.Name = "lbMOTD";
            this.lbMOTD.Size = new System.Drawing.Size(1119, 21);
            this.lbMOTD.TabIndex = 125;
            this.lbMOTD.Text = "...";
            this.lbMOTD.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbMOTD.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(10, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 19);
            this.label2.TabIndex = 83;
            this.label2.Text = "Version Selector";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // lbVersions
            // 
            this.lbVersions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbVersions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.lbVersions.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbVersions.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbVersions.ForeColor = System.Drawing.Color.White;
            this.lbVersions.FormattingEnabled = true;
            this.lbVersions.IntegralHeight = false;
            this.lbVersions.ItemHeight = 21;
            this.lbVersions.Location = new System.Drawing.Point(12, 34);
            this.lbVersions.Name = "lbVersions";
            this.lbVersions.Size = new System.Drawing.Size(123, 375);
            this.lbVersions.TabIndex = 82;
            this.lbVersions.Tag = "";
            this.lbVersions.SelectedIndexChanged += new System.EventHandler(this.lbVersions_SelectedIndexChanged);
            this.lbVersions.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbVersions_MouseDown);
            // 
            // pnLeftSide
            // 
            this.pnLeftSide.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.pnLeftSide.Controls.Add(this.pbNewVersionNotification);
            this.pnLeftSide.Controls.Add(this.btnVersionDownloader);
            this.pnLeftSide.Controls.Add(this.lbVersions);
            this.pnLeftSide.Controls.Add(this.label2);
            this.pnLeftSide.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnLeftSide.Location = new System.Drawing.Point(0, 41);
            this.pnLeftSide.Name = "pnLeftSide";
            this.pnLeftSide.Size = new System.Drawing.Size(138, 451);
            this.pnLeftSide.TabIndex = 134;
            // 
            // btnVersionDownloader
            // 
            this.btnVersionDownloader.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnVersionDownloader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnVersionDownloader.FlatAppearance.BorderSize = 0;
            this.btnVersionDownloader.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVersionDownloader.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.btnVersionDownloader.ForeColor = System.Drawing.Color.White;
            this.btnVersionDownloader.Location = new System.Drawing.Point(13, 417);
            this.btnVersionDownloader.Name = "btnVersionDownloader";
            this.btnVersionDownloader.Size = new System.Drawing.Size(122, 24);
            this.btnVersionDownloader.TabIndex = 130;
            this.btnVersionDownloader.TabStop = false;
            this.btnVersionDownloader.Tag = "";
            this.btnVersionDownloader.Text = "Version downloader";
            this.btnVersionDownloader.UseVisualStyleBackColor = false;
            this.btnVersionDownloader.Click += new System.EventHandler(this.btnVersionDownloader_Click);
            // 
            // pbNewVersionNotification
            // 
            this.pbNewVersionNotification.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pbNewVersionNotification.Image = global::RTC_Launcher.Properties.Resources.notificationBadge;
            this.pbNewVersionNotification.InitialImage = global::RTC_Launcher.Properties.Resources.notificationBadge;
            this.pbNewVersionNotification.Location = new System.Drawing.Point(118, 409);
            this.pbNewVersionNotification.Name = "pbNewVersionNotification";
            this.pbNewVersionNotification.Size = new System.Drawing.Size(14, 14);
            this.pbNewVersionNotification.TabIndex = 0;
            this.pbNewVersionNotification.TabStop = false;
            // 
            // pnAnchorRight
            // 
            this.pnAnchorRight.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnAnchorRight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.pnAnchorRight.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnAnchorRight.BackgroundImage")));
            this.pnAnchorRight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pnAnchorRight.Location = new System.Drawing.Point(138, 41);
            this.pnAnchorRight.Name = "pnAnchorRight";
            this.pnAnchorRight.Size = new System.Drawing.Size(1000, 451);
            this.pnAnchorRight.TabIndex = 133;
            this.pnAnchorRight.Paint += new System.Windows.Forms.PaintEventHandler(this.pnAnchorRight_Paint);
            // 
            // pnTopPanel
            // 
            this.pnTopPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.pnTopPanel.BackgroundImage = global::RTC_Launcher.Properties.Resources.LauncherBack;
            this.pnTopPanel.Controls.Add(this.btnDiscord);
            this.pnTopPanel.Controls.Add(this.btnMinimize);
            this.pnTopPanel.Controls.Add(this.btnQuit);
            this.pnTopPanel.Controls.Add(this.btnOnlineGuide);
            this.pnTopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnTopPanel.Location = new System.Drawing.Point(0, 0);
            this.pnTopPanel.Name = "pnTopPanel";
            this.pnTopPanel.Size = new System.Drawing.Size(1138, 41);
            this.pnTopPanel.TabIndex = 131;
            this.pnTopPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel2_MouseMove);
            // 
            // btnDiscord
            // 
            this.btnDiscord.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDiscord.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnDiscord.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnDiscord.FlatAppearance.BorderSize = 0;
            this.btnDiscord.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDiscord.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.btnDiscord.ForeColor = System.Drawing.Color.White;
            this.btnDiscord.Image = global::RTC_Launcher.Properties.Resources.discord;
            this.btnDiscord.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDiscord.Location = new System.Drawing.Point(896, 8);
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
            this.btnMinimize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnMinimize.FlatAppearance.BorderSize = 0;
            this.btnMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            this.btnMinimize.ForeColor = System.Drawing.Color.White;
            this.btnMinimize.Location = new System.Drawing.Point(1080, 8);
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
            this.btnQuit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnQuit.FlatAppearance.BorderSize = 0;
            this.btnQuit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQuit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            this.btnQuit.ForeColor = System.Drawing.Color.White;
            this.btnQuit.Location = new System.Drawing.Point(1108, 8);
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
            this.btnOnlineGuide.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnOnlineGuide.FlatAppearance.BorderSize = 0;
            this.btnOnlineGuide.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOnlineGuide.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.btnOnlineGuide.ForeColor = System.Drawing.Color.White;
            this.btnOnlineGuide.Image = global::RTC_Launcher.Properties.Resources.corruptwiki;
            this.btnOnlineGuide.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOnlineGuide.Location = new System.Drawing.Point(975, 8);
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
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(1138, 552);
            this.Controls.Add(this.pnLeftSide);
            this.Controls.Add(this.pnAnchorRight);
            this.Controls.Add(this.pnTopPanel);
            this.Controls.Add(this.pnBottomPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(535, 551);
            this.Name = "MainForm";
            this.Tag = "";
            this.Text = "RTC Launcher";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.pnBottomPanel.ResumeLayout(false);
            this.pnLeftSide.ResumeLayout(false);
            this.pnLeftSide.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbNewVersionNotification)).EndInit();
            this.pnTopPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnBottomPanel;
        private System.Windows.Forms.Label lbMOTD;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.ListBox lbVersions;
        private System.Windows.Forms.Panel pnTopPanel;
        private System.Windows.Forms.Button btnOnlineGuide;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel pnAnchorRight;
        private System.Windows.Forms.Button btnVersionDownloader;
        public System.Windows.Forms.Panel pnLeftSide;
        private System.Windows.Forms.PictureBox pbNewVersionNotification;
        private System.Windows.Forms.Button btnQuit;
        private System.Windows.Forms.Button btnMinimize;
        private System.Windows.Forms.Button btnDiscord;
    }
}

