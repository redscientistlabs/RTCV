namespace RTC_Launcher
{
    partial class DownloadForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DownloadForm));
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lbStatus = new System.Windows.Forms.Label();
            this.lbDownloadProgress = new System.Windows.Forms.Label();
            this.pnDownloadBar = new System.Windows.Forms.Panel();
            this.pnDownloadBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(22, 84);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(454, 13);
            this.progressBar.TabIndex = 0;
            // 
            // lbStatus
            // 
            this.lbStatus.AutoSize = true;
            this.lbStatus.Font = new System.Drawing.Font("Segoe UI Semibold", 24F, System.Drawing.FontStyle.Bold);
            this.lbStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.lbStatus.Location = new System.Drawing.Point(14, 12);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(214, 45);
            this.lbStatus.TabIndex = 1;
            this.lbStatus.Text = "Downloading";
            // 
            // lbDownloadProgress
            // 
            this.lbDownloadProgress.AutoSize = true;
            this.lbDownloadProgress.Font = new System.Drawing.Font("Consolas", 12F);
            this.lbDownloadProgress.ForeColor = System.Drawing.Color.White;
            this.lbDownloadProgress.Location = new System.Drawing.Point(231, 34);
            this.lbDownloadProgress.Name = "lbDownloadProgress";
            this.lbDownloadProgress.Size = new System.Drawing.Size(36, 19);
            this.lbDownloadProgress.TabIndex = 2;
            this.lbDownloadProgress.Text = "...";
            // 
            // pnDownloadBar
            // 
            this.pnDownloadBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pnDownloadBar.Controls.Add(this.lbStatus);
            this.pnDownloadBar.Controls.Add(this.progressBar);
            this.pnDownloadBar.Controls.Add(this.lbDownloadProgress);
            this.pnDownloadBar.Location = new System.Drawing.Point(24, 22);
            this.pnDownloadBar.Name = "pnDownloadBar";
            this.pnDownloadBar.Size = new System.Drawing.Size(500, 120);
            this.pnDownloadBar.TabIndex = 3;
            this.pnDownloadBar.Visible = false;
            // 
            // DownloadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(1136, 452);
            this.Controls.Add(this.pnDownloadBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DownloadForm";
            this.Text = "DownloadForm";
            this.Load += new System.EventHandler(this.DownloadForm_Load);
            this.pnDownloadBar.ResumeLayout(false);
            this.pnDownloadBar.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.ProgressBar progressBar;
        public System.Windows.Forms.Label lbStatus;
        public System.Windows.Forms.Label lbDownloadProgress;
        private System.Windows.Forms.Panel pnDownloadBar;
    }
}