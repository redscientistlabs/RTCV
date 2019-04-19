namespace RTC_Launcher
{
    partial class VersionDownloadPanel
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
            this.lbOnlineVersions = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnDownloadVersion = new System.Windows.Forms.Button();
            this.cbDevBuids = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lbOnlineVersions
            // 
            this.lbOnlineVersions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbOnlineVersions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.lbOnlineVersions.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbOnlineVersions.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbOnlineVersions.ForeColor = System.Drawing.Color.White;
            this.lbOnlineVersions.FormattingEnabled = true;
            this.lbOnlineVersions.IntegralHeight = false;
            this.lbOnlineVersions.ItemHeight = 30;
            this.lbOnlineVersions.Location = new System.Drawing.Point(12, 34);
            this.lbOnlineVersions.Name = "lbOnlineVersions";
            this.lbOnlineVersions.Size = new System.Drawing.Size(463, 192);
            this.lbOnlineVersions.TabIndex = 129;
            this.lbOnlineVersions.Tag = "color:normal";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(10, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 19);
            this.label3.TabIndex = 130;
            this.label3.Text = "Online Downloader";
            // 
            // btnDownloadVersion
            // 
            this.btnDownloadVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDownloadVersion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnDownloadVersion.FlatAppearance.BorderSize = 0;
            this.btnDownloadVersion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDownloadVersion.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.btnDownloadVersion.ForeColor = System.Drawing.Color.White;
            this.btnDownloadVersion.Location = new System.Drawing.Point(12, 234);
            this.btnDownloadVersion.Name = "btnDownloadVersion";
            this.btnDownloadVersion.Size = new System.Drawing.Size(463, 24);
            this.btnDownloadVersion.TabIndex = 131;
            this.btnDownloadVersion.TabStop = false;
            this.btnDownloadVersion.Tag = "color:light";
            this.btnDownloadVersion.Text = "Download";
            this.btnDownloadVersion.UseVisualStyleBackColor = false;
            this.btnDownloadVersion.Click += new System.EventHandler(this.btnDownloadVersion_Click);
            // 
            // cbDevBuids
            // 
            this.cbDevBuids.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbDevBuids.AutoSize = true;
            this.cbDevBuids.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.cbDevBuids.ForeColor = System.Drawing.Color.White;
            this.cbDevBuids.Location = new System.Drawing.Point(398, 14);
            this.cbDevBuids.Name = "cbDevBuids";
            this.cbDevBuids.Size = new System.Drawing.Size(80, 17);
            this.cbDevBuids.TabIndex = 132;
            this.cbDevBuids.Text = "Dev builds";
            this.cbDevBuids.UseVisualStyleBackColor = true;
            this.cbDevBuids.CheckedChanged += new System.EventHandler(this.cbDevBuids_CheckedChanged);
            // 
            // VersionDownloadPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(490, 268);
            this.Controls.Add(this.cbDevBuids);
            this.Controls.Add(this.lbOnlineVersions);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnDownloadVersion);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "VersionDownloadPanel";
            this.Text = "VersionDownloadPanel";
            this.Load += new System.EventHandler(this.VersionDownloadPanel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ListBox lbOnlineVersions;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.Button btnDownloadVersion;
        private System.Windows.Forms.CheckBox cbDevBuids;
    }
}