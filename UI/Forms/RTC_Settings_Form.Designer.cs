namespace RTCV.UI
{
    partial class RTC_Settings_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RTC_Settings_Form));
            this.lbSettingsAndTools = new System.Windows.Forms.Label();
            this.btnRtcFactoryClean = new System.Windows.Forms.Button();
            this.btnCloseSettings = new System.Windows.Forms.Button();
            this.pnListBoxForm = new System.Windows.Forms.Panel();
            this.btnOpenConsole = new System.Windows.Forms.Button();
            this.btnDebugInfo = new System.Windows.Forms.Button();
            this.btnTestForm = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbSettingsAndTools
            // 
            this.lbSettingsAndTools.AutoSize = true;
            this.lbSettingsAndTools.Font = new System.Drawing.Font("Segoe UI Semibold", 26.25F, System.Drawing.FontStyle.Bold);
            this.lbSettingsAndTools.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lbSettingsAndTools.Location = new System.Drawing.Point(11, 9);
            this.lbSettingsAndTools.Name = "lbSettingsAndTools";
            this.lbSettingsAndTools.Size = new System.Drawing.Size(307, 47);
            this.lbSettingsAndTools.TabIndex = 118;
            this.lbSettingsAndTools.Text = "Settings and tools";
            // 
            // btnRtcFactoryClean
            // 
            this.btnRtcFactoryClean.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnRtcFactoryClean.FlatAppearance.BorderSize = 0;
            this.btnRtcFactoryClean.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRtcFactoryClean.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnRtcFactoryClean.ForeColor = System.Drawing.Color.Black;
            this.btnRtcFactoryClean.Image = ((System.Drawing.Image)(resources.GetObject("btnRtcFactoryClean.Image")));
            this.btnRtcFactoryClean.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnRtcFactoryClean.Location = new System.Drawing.Point(409, 465);
            this.btnRtcFactoryClean.Name = "btnRtcFactoryClean";
            this.btnRtcFactoryClean.Size = new System.Drawing.Size(232, 29);
            this.btnRtcFactoryClean.TabIndex = 127;
            this.btnRtcFactoryClean.Tag = "color:light";
            this.btnRtcFactoryClean.Text = "  RTC Factory Clean";
            this.btnRtcFactoryClean.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRtcFactoryClean.UseVisualStyleBackColor = false;
            this.btnRtcFactoryClean.Click += new System.EventHandler(this.btnRtcFactoryClean_Click);
            // 
            // btnCloseSettings
            // 
            this.btnCloseSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnCloseSettings.FlatAppearance.BorderSize = 0;
            this.btnCloseSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseSettings.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnCloseSettings.ForeColor = System.Drawing.Color.Black;
            this.btnCloseSettings.Image = ((System.Drawing.Image)(resources.GetObject("btnCloseSettings.Image")));
            this.btnCloseSettings.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCloseSettings.Location = new System.Drawing.Point(19, 465);
            this.btnCloseSettings.Name = "btnCloseSettings";
            this.btnCloseSettings.Size = new System.Drawing.Size(215, 29);
            this.btnCloseSettings.TabIndex = 127;
            this.btnCloseSettings.Tag = "color:light";
            this.btnCloseSettings.Text = " Close Settings";
            this.btnCloseSettings.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCloseSettings.UseVisualStyleBackColor = false;
            this.btnCloseSettings.Click += new System.EventHandler(this.btnCloseSettings_Click);
            // 
            // pnListBoxForm
            // 
            this.pnListBoxForm.BackColor = System.Drawing.Color.Gray;
            this.pnListBoxForm.Location = new System.Drawing.Point(19, 66);
            this.pnListBoxForm.Name = "pnListBoxForm";
            this.pnListBoxForm.Size = new System.Drawing.Size(622, 378);
            this.pnListBoxForm.TabIndex = 137;
            this.pnListBoxForm.Tag = "color:normal";
            // 
            // btnOpenConsole
            // 
            this.btnOpenConsole.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnOpenConsole.FlatAppearance.BorderSize = 0;
            this.btnOpenConsole.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenConsole.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnOpenConsole.ForeColor = System.Drawing.Color.Black;
            this.btnOpenConsole.Image = ((System.Drawing.Image)(resources.GetObject("btnOpenConsole.Image")));
            this.btnOpenConsole.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOpenConsole.Location = new System.Drawing.Point(515, 27);
            this.btnOpenConsole.Name = "btnOpenConsole";
            this.btnOpenConsole.Size = new System.Drawing.Size(126, 29);
            this.btnOpenConsole.TabIndex = 138;
            this.btnOpenConsole.Tag = "color:light";
            this.btnOpenConsole.Text = " Toggle Console";
            this.btnOpenConsole.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnOpenConsole.UseVisualStyleBackColor = false;
            this.btnOpenConsole.Click += new System.EventHandler(this.btnToggleConsole_Click);
            // 
            // btnDebugInfo
            // 
            this.btnDebugInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnDebugInfo.FlatAppearance.BorderSize = 0;
            this.btnDebugInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDebugInfo.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnDebugInfo.ForeColor = System.Drawing.Color.Black;
            this.btnDebugInfo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDebugInfo.Location = new System.Drawing.Point(380, 27);
            this.btnDebugInfo.Name = "btnDebugInfo";
            this.btnDebugInfo.Size = new System.Drawing.Size(126, 29);
            this.btnDebugInfo.TabIndex = 139;
            this.btnDebugInfo.Tag = "color:light";
            this.btnDebugInfo.Text = "🙃 Show Debug Info";
            this.btnDebugInfo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDebugInfo.UseVisualStyleBackColor = false;
            this.btnDebugInfo.Click += new System.EventHandler(this.btnDebugInfo_Click);
            // 
            // btnTestForm
            // 
            this.btnTestForm.Location = new System.Drawing.Point(317, 9);
            this.btnTestForm.Name = "btnTestForm";
            this.btnTestForm.Size = new System.Drawing.Size(57, 51);
            this.btnTestForm.TabIndex = 140;
            this.btnTestForm.Text = "Open Test Form";
            this.btnTestForm.UseVisualStyleBackColor = true;
            this.btnTestForm.Visible = false;
            this.btnTestForm.Click += new System.EventHandler(this.BtnTestForm_Click);
            // 
            // RTC_Settings_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(655, 515);
            this.Controls.Add(this.btnTestForm);
            this.Controls.Add(this.btnDebugInfo);
            this.Controls.Add(this.btnOpenConsole);
            this.Controls.Add(this.pnListBoxForm);
            this.Controls.Add(this.btnCloseSettings);
            this.Controls.Add(this.btnRtcFactoryClean);
            this.Controls.Add(this.lbSettingsAndTools);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "RTC_Settings_Form";
            this.Tag = "color:dark";
            this.Text = "RTC : Settings";
            this.Load += new System.EventHandler(this.RTC_Settings_Form_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lbSettingsAndTools;
        public System.Windows.Forms.Button btnRtcFactoryClean;
        public System.Windows.Forms.Button btnCloseSettings;
		private System.Windows.Forms.Panel pnListBoxForm;
		public System.Windows.Forms.Button btnOpenConsole;
		public System.Windows.Forms.Button btnDebugInfo;
		private System.Windows.Forms.Button btnTestForm;
	}
}