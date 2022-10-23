namespace RTCV.UI
{
    partial class SettingsNetCoreForm
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
            this.pnDetachedModeSettings = new System.Windows.Forms.Panel();
            this.cbCrashSoundEffect = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.nmGameProtectionDelay = new System.Windows.Forms.NumericUpDown();
            this.lbDetachedModeSettings = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.pnDetachedModeSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmGameProtectionDelay)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnDetachedModeSettings
            // 
            this.pnDetachedModeSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnDetachedModeSettings.BackColor = System.Drawing.Color.Gray;
            this.pnDetachedModeSettings.Controls.Add(this.cbCrashSoundEffect);
            this.pnDetachedModeSettings.Controls.Add(this.label1);
            this.pnDetachedModeSettings.Location = new System.Drawing.Point(23, 36);
            this.pnDetachedModeSettings.Name = "pnDetachedModeSettings";
            this.pnDetachedModeSettings.Size = new System.Drawing.Size(360, 74);
            this.pnDetachedModeSettings.TabIndex = 132;
            this.pnDetachedModeSettings.Tag = "color:normal";
            // 
            // cbCrashSoundEffect
            // 
            this.cbCrashSoundEffect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cbCrashSoundEffect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCrashSoundEffect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbCrashSoundEffect.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbCrashSoundEffect.ForeColor = System.Drawing.Color.White;
            this.cbCrashSoundEffect.FormattingEnabled = true;
            this.cbCrashSoundEffect.Items.AddRange(new object[] {
            "Breaking plates HD",
            "Breaking plates Classic",
            "None",
            "CRASHSOUNDS folder"});
            this.cbCrashSoundEffect.Location = new System.Drawing.Point(18, 33);
            this.cbCrashSoundEffect.Name = "cbCrashSoundEffect";
            this.cbCrashSoundEffect.Size = new System.Drawing.Size(163, 21);
            this.cbCrashSoundEffect.TabIndex = 127;
            this.cbCrashSoundEffect.Tag = "color:dark1";
            this.cbCrashSoundEffect.SelectedIndexChanged += new System.EventHandler(this.OnCrashSoundeffectChange);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(15, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 126;
            this.label1.Text = "Crash sound effect:";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label20.ForeColor = System.Drawing.Color.White;
            this.label20.Location = new System.Drawing.Point(16, 18);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(128, 13);
            this.label20.TabIndex = 116;
            this.label20.Text = "Backups the game state";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(96, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 115;
            this.label3.Text = "seconds";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label21.ForeColor = System.Drawing.Color.White;
            this.label21.Location = new System.Drawing.Point(15, 39);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(33, 13);
            this.label21.TabIndex = 117;
            this.label21.Text = "every";
            // 
            // nmGameProtectionDelay
            // 
            this.nmGameProtectionDelay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nmGameProtectionDelay.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.nmGameProtectionDelay.ForeColor = System.Drawing.Color.White;
            this.nmGameProtectionDelay.Location = new System.Drawing.Point(48, 35);
            this.nmGameProtectionDelay.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.nmGameProtectionDelay.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmGameProtectionDelay.Name = "nmGameProtectionDelay";
            this.nmGameProtectionDelay.Size = new System.Drawing.Size(45, 25);
            this.nmGameProtectionDelay.TabIndex = 114;
            this.nmGameProtectionDelay.Tag = "color:dark1";
            this.nmGameProtectionDelay.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nmGameProtectionDelay.ValueChanged += new System.EventHandler(this.OnGameProtectionDelayChange);
            this.nmGameProtectionDelay.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnGameProtectionDelayChange);
            this.nmGameProtectionDelay.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnGameProtectionDelayChange);
            // 
            // lbDetachedModeSettings
            // 
            this.lbDetachedModeSettings.AutoSize = true;
            this.lbDetachedModeSettings.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.lbDetachedModeSettings.ForeColor = System.Drawing.Color.White;
            this.lbDetachedModeSettings.Location = new System.Drawing.Point(27, 18);
            this.lbDetachedModeSettings.Name = "lbDetachedModeSettings";
            this.lbDetachedModeSettings.Size = new System.Drawing.Size(50, 15);
            this.lbDetachedModeSettings.TabIndex = 133;
            this.lbDetachedModeSettings.Text = "NetCore";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.Gray;
            this.panel1.Controls.Add(this.label20);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.nmGameProtectionDelay);
            this.panel1.Controls.Add(this.label21);
            this.panel1.Location = new System.Drawing.Point(23, 141);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(360, 77);
            this.panel1.TabIndex = 133;
            this.panel1.Tag = "color:normal";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(30, 123);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 15);
            this.label2.TabIndex = 134;
            this.label2.Text = "Game Protection";
            // 
            // SettingsNetCoreForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.ClientSize = new System.Drawing.Size(412, 241);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnDetachedModeSettings);
            this.Controls.Add(this.lbDetachedModeSettings);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SettingsNetCoreForm";
            this.Tag = "color:dark1";
            this.Text = "NetCore";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HandleFormClosing);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            this.pnDetachedModeSettings.ResumeLayout(false);
            this.pnDetachedModeSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmGameProtectionDelay)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

		#endregion
		private System.Windows.Forms.Panel pnDetachedModeSettings;
		public System.Windows.Forms.ComboBox cbCrashSoundEffect;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label21;
		public System.Windows.Forms.NumericUpDown nmGameProtectionDelay;
		private System.Windows.Forms.Label lbDetachedModeSettings;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
    }
}
