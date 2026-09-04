namespace RTCV.UI
{
    partial class SettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.lbSettingsAndTools = new System.Windows.Forms.Label();
            this.btnEmuFactoryClean = new System.Windows.Forms.Button();
            this.pnListBoxForm = new System.Windows.Forms.Panel();
            this.btnOpenConsole = new System.Windows.Forms.Button();
            this.btnDebugInfo = new System.Windows.Forms.Button();
            this.btnTestForm = new System.Windows.Forms.Button();
            this.btnRtcFactoryClean = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbSettingsAndTools
            // 
            this.lbSettingsAndTools.AutoSize = true;
            this.lbSettingsAndTools.Font = new System.Drawing.Font("Segoe UI Semibold", 22F, System.Drawing.FontStyle.Bold);
            this.lbSettingsAndTools.ForeColor = System.Drawing.Color.White;
            this.lbSettingsAndTools.Location = new System.Drawing.Point(18, 26);
            this.lbSettingsAndTools.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbSettingsAndTools.Name = "lbSettingsAndTools";
            this.lbSettingsAndTools.Size = new System.Drawing.Size(391, 60);
            this.lbSettingsAndTools.TabIndex = 118;
            this.lbSettingsAndTools.Text = "Settings and Tools";
            // 
            // btnEmuFactoryClean
            // 
            this.btnEmuFactoryClean.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEmuFactoryClean.BackColor = System.Drawing.Color.Gray;
            this.btnEmuFactoryClean.FlatAppearance.BorderSize = 0;
            this.btnEmuFactoryClean.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEmuFactoryClean.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnEmuFactoryClean.ForeColor = System.Drawing.Color.White;
            this.btnEmuFactoryClean.Image = ((System.Drawing.Image)(resources.GetObject("btnEmuFactoryClean.Image")));
            this.btnEmuFactoryClean.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnEmuFactoryClean.Location = new System.Drawing.Point(610, 768);
            this.btnEmuFactoryClean.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnEmuFactoryClean.Name = "btnEmuFactoryClean";
            this.btnEmuFactoryClean.Size = new System.Drawing.Size(420, 45);
            this.btnEmuFactoryClean.TabIndex = 127;
            this.btnEmuFactoryClean.Tag = "color:light1";
            this.btnEmuFactoryClean.Text = "  Emulator Factory Clean";
            this.btnEmuFactoryClean.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEmuFactoryClean.UseVisualStyleBackColor = false;
            this.btnEmuFactoryClean.Click += new System.EventHandler(this.OnEmuFactoryCleanSelect);
            // 
            // pnListBoxForm
            // 
            this.pnListBoxForm.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnListBoxForm.BackColor = System.Drawing.Color.Gray;
            this.pnListBoxForm.Location = new System.Drawing.Point(24, 102);
            this.pnListBoxForm.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnListBoxForm.Name = "pnListBoxForm";
            this.pnListBoxForm.Size = new System.Drawing.Size(1006, 649);
            this.pnListBoxForm.TabIndex = 137;
            this.pnListBoxForm.Tag = "color:normal";
            // 
            // btnOpenConsole
            // 
            this.btnOpenConsole.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenConsole.BackColor = System.Drawing.Color.Gray;
            this.btnOpenConsole.FlatAppearance.BorderSize = 0;
            this.btnOpenConsole.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenConsole.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnOpenConsole.ForeColor = System.Drawing.Color.White;
            this.btnOpenConsole.Image = ((System.Drawing.Image)(resources.GetObject("btnOpenConsole.Image")));
            this.btnOpenConsole.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOpenConsole.Location = new System.Drawing.Point(842, 42);
            this.btnOpenConsole.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnOpenConsole.Name = "btnOpenConsole";
            this.btnOpenConsole.Size = new System.Drawing.Size(189, 45);
            this.btnOpenConsole.TabIndex = 138;
            this.btnOpenConsole.Tag = "color:light1";
            this.btnOpenConsole.Text = " Toggle Console";
            this.btnOpenConsole.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnOpenConsole.UseVisualStyleBackColor = false;
            this.btnOpenConsole.Click += new System.EventHandler(this.ToggleConsole);
            // 
            // btnDebugInfo
            // 
            this.btnDebugInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDebugInfo.BackColor = System.Drawing.Color.Gray;
            this.btnDebugInfo.FlatAppearance.BorderSize = 0;
            this.btnDebugInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDebugInfo.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnDebugInfo.ForeColor = System.Drawing.Color.White;
            this.btnDebugInfo.Image = ((System.Drawing.Image)(resources.GetObject("btnDebugInfo.Image")));
            this.btnDebugInfo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDebugInfo.Location = new System.Drawing.Point(639, 42);
            this.btnDebugInfo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnDebugInfo.Name = "btnDebugInfo";
            this.btnDebugInfo.Size = new System.Drawing.Size(189, 45);
            this.btnDebugInfo.TabIndex = 139;
            this.btnDebugInfo.Tag = "color:light1";
            this.btnDebugInfo.Text = " Show Debug Info";
            this.btnDebugInfo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDebugInfo.UseVisualStyleBackColor = false;
            this.btnDebugInfo.Click += new System.EventHandler(this.ShowDebugInfo);
            // 
            // btnTestForm
            // 
            this.btnTestForm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTestForm.BackColor = System.Drawing.Color.Gray;
            this.btnTestForm.FlatAppearance.BorderSize = 0;
            this.btnTestForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTestForm.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnTestForm.ForeColor = System.Drawing.Color.White;
            this.btnTestForm.Location = new System.Drawing.Point(540, 42);
            this.btnTestForm.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnTestForm.Name = "btnTestForm";
            this.btnTestForm.Size = new System.Drawing.Size(86, 45);
            this.btnTestForm.TabIndex = 140;
            this.btnTestForm.Tag = "color:light1";
            this.btnTestForm.Text = "Test";
            this.btnTestForm.UseVisualStyleBackColor = false;
            this.btnTestForm.Visible = false;
            this.btnTestForm.Click += new System.EventHandler(this.ShowTestForm);
            // 
            // btnRtcFactoryClean
            // 
            this.btnRtcFactoryClean.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRtcFactoryClean.BackColor = System.Drawing.Color.Gray;
            this.btnRtcFactoryClean.FlatAppearance.BorderSize = 0;
            this.btnRtcFactoryClean.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRtcFactoryClean.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnRtcFactoryClean.ForeColor = System.Drawing.Color.White;
            this.btnRtcFactoryClean.Image = ((System.Drawing.Image)(resources.GetObject("btnRtcFactoryClean.Image")));
            this.btnRtcFactoryClean.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnRtcFactoryClean.Location = new System.Drawing.Point(182, 768);
            this.btnRtcFactoryClean.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnRtcFactoryClean.Name = "btnRtcFactoryClean";
            this.btnRtcFactoryClean.Size = new System.Drawing.Size(420, 45);
            this.btnRtcFactoryClean.TabIndex = 141;
            this.btnRtcFactoryClean.Tag = "color:light1";
            this.btnRtcFactoryClean.Text = "  RTC Factory Clean";
            this.btnRtcFactoryClean.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRtcFactoryClean.UseVisualStyleBackColor = false;
            this.btnRtcFactoryClean.Click += new System.EventHandler(this.OnFactoryCleanSelect);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(1056, 862);
            this.Controls.Add(this.btnRtcFactoryClean);
            this.Controls.Add(this.btnTestForm);
            this.Controls.Add(this.btnDebugInfo);
            this.Controls.Add(this.btnOpenConsole);
            this.Controls.Add(this.pnListBoxForm);
            this.Controls.Add(this.btnEmuFactoryClean);
            this.Controls.Add(this.lbSettingsAndTools);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(1056, 854);
            this.Name = "SettingsForm";
            this.Tag = "color:dark1";
            this.Text = "RTC : Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HandleFormClosing);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lbSettingsAndTools;
        public System.Windows.Forms.Button btnEmuFactoryClean;
		private System.Windows.Forms.Panel pnListBoxForm;
		public System.Windows.Forms.Button btnOpenConsole;
		public System.Windows.Forms.Button btnDebugInfo;
		private System.Windows.Forms.Button btnTestForm;
        public System.Windows.Forms.Button btnRtcFactoryClean;
    }
}
