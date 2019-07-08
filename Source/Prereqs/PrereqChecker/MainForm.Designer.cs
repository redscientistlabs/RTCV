namespace RTCV.Prereqs
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
            this.lbStatus = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lbDownloadProgress = new System.Windows.Forms.Label();
            this.btnMinimize = new System.Windows.Forms.Button();
            this.btnQuit = new System.Windows.Forms.Button();
            this.pnTopPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.pnTopPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbStatus
            // 
            this.lbStatus.AutoSize = true;
            this.lbStatus.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold);
            this.lbStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.lbStatus.Location = new System.Drawing.Point(11, 49);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(147, 30);
            this.lbStatus.TabIndex = 4;
            this.lbStatus.Text = "Downloading";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(9, 108);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(638, 35);
            this.progressBar.TabIndex = 3;
            // 
            // lbDownloadProgress
            // 
            this.lbDownloadProgress.AutoSize = true;
            this.lbDownloadProgress.Font = new System.Drawing.Font("Consolas", 12F);
            this.lbDownloadProgress.ForeColor = System.Drawing.Color.White;
            this.lbDownloadProgress.Location = new System.Drawing.Point(513, 80);
            this.lbDownloadProgress.Name = "lbDownloadProgress";
            this.lbDownloadProgress.Size = new System.Drawing.Size(36, 19);
            this.lbDownloadProgress.TabIndex = 5;
            this.lbDownloadProgress.Text = "...";
            // 
            // btnMinimize
            // 
            this.btnMinimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMinimize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnMinimize.FlatAppearance.BorderSize = 0;
            this.btnMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            this.btnMinimize.ForeColor = System.Drawing.Color.White;
            this.btnMinimize.Location = new System.Drawing.Point(597, 8);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(22, 24);
            this.btnMinimize.TabIndex = 131;
            this.btnMinimize.TabStop = false;
            this.btnMinimize.Tag = "";
            this.btnMinimize.Text = "_";
            this.btnMinimize.UseVisualStyleBackColor = false;
            this.btnMinimize.Click += new System.EventHandler(this.BtnMinimize_Click);
            // 
            // btnQuit
            // 
            this.btnQuit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnQuit.FlatAppearance.BorderSize = 0;
            this.btnQuit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQuit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            this.btnQuit.ForeColor = System.Drawing.Color.White;
            this.btnQuit.Location = new System.Drawing.Point(625, 8);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(22, 24);
            this.btnQuit.TabIndex = 130;
            this.btnQuit.TabStop = false;
            this.btnQuit.Tag = "";
            this.btnQuit.Text = "X";
            this.btnQuit.UseVisualStyleBackColor = false;
            this.btnQuit.Click += new System.EventHandler(this.BtnQuit_Click);
            // 
            // pnTopPanel
            // 
            this.pnTopPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.pnTopPanel.Controls.Add(this.label1);
            this.pnTopPanel.Controls.Add(this.btnMinimize);
            this.pnTopPanel.Controls.Add(this.btnQuit);
            this.pnTopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnTopPanel.Location = new System.Drawing.Point(0, 0);
            this.pnTopPanel.Name = "pnTopPanel";
            this.pnTopPanel.Size = new System.Drawing.Size(655, 41);
            this.pnTopPanel.TabIndex = 132;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 16F);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(103)))), ((int)(((byte)(240)))));
            this.label1.Location = new System.Drawing.Point(4, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(414, 30);
            this.label1.TabIndex = 132;
            this.label1.Text = "Real-Time Corruptor Prerequisite Installer";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(655, 156);
            this.ControlBox = false;
            this.Controls.Add(this.pnTopPanel);
            this.Controls.Add(this.lbStatus);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lbDownloadProgress);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(655, 151);
            this.Name = "MainForm";
            this.Text = "Prereq Downloader";
            this.pnTopPanel.ResumeLayout(false);
            this.pnTopPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label lbStatus;
        public System.Windows.Forms.ProgressBar progressBar;
        public System.Windows.Forms.Label lbDownloadProgress;
        private System.Windows.Forms.Button btnMinimize;
        private System.Windows.Forms.Button btnQuit;
        private System.Windows.Forms.Panel pnTopPanel;
        private System.Windows.Forms.Label label1;
    }
}