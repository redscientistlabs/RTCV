namespace RTCV.UI
{
    partial class RTC_SettingsNetCore_Form
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbCrashSoundEffect = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.nmGameProtectionDelay = new System.Windows.Forms.NumericUpDown();
            this.lbDetachedModeSettings = new System.Windows.Forms.Label();
            this.pnDetachedModeSettings.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmGameProtectionDelay)).BeginInit();
            this.SuspendLayout();
            // 
            // pnDetachedModeSettings
            // 
            this.pnDetachedModeSettings.BackColor = System.Drawing.Color.Gray;
            this.pnDetachedModeSettings.Controls.Add(this.groupBox2);
            this.pnDetachedModeSettings.Controls.Add(this.groupBox1);
            this.pnDetachedModeSettings.Location = new System.Drawing.Point(80, 40);
            this.pnDetachedModeSettings.Name = "pnDetachedModeSettings";
            this.pnDetachedModeSettings.Size = new System.Drawing.Size(250, 190);
            this.pnDetachedModeSettings.TabIndex = 132;
            this.pnDetachedModeSettings.Tag = "color:normal";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbCrashSoundEffect);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(10, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(230, 88);
            this.groupBox2.TabIndex = 125;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "NetCore";
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
            this.cbCrashSoundEffect.Location = new System.Drawing.Point(18, 34);
            this.cbCrashSoundEffect.Name = "cbCrashSoundEffect";
            this.cbCrashSoundEffect.Size = new System.Drawing.Size(163, 21);
            this.cbCrashSoundEffect.TabIndex = 127;
            this.cbCrashSoundEffect.Tag = "color:dark";
            this.cbCrashSoundEffect.SelectedIndexChanged += new System.EventHandler(this.cbCrashSoundEffect_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label1.Location = new System.Drawing.Point(15, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 126;
            this.label1.Text = "Crash sound effect:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label20);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label21);
            this.groupBox1.Controls.Add(this.nmGameProtectionDelay);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(10, 97);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(230, 88);
            this.groupBox1.TabIndex = 124;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Game Protection";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label20.ForeColor = System.Drawing.Color.White;
            this.label20.Location = new System.Drawing.Point(15, 24);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(129, 13);
            this.label20.TabIndex = 116;
            this.label20.Text = "Backups the game state";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(95, 45);
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
            this.label21.Location = new System.Drawing.Point(14, 45);
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
            this.nmGameProtectionDelay.Location = new System.Drawing.Point(47, 41);
            this.nmGameProtectionDelay.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.nmGameProtectionDelay.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nmGameProtectionDelay.Name = "nmGameProtectionDelay";
            this.nmGameProtectionDelay.Size = new System.Drawing.Size(45, 25);
            this.nmGameProtectionDelay.TabIndex = 114;
            this.nmGameProtectionDelay.Tag = "color:dark";
            this.nmGameProtectionDelay.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.nmGameProtectionDelay.ValueChanged += new System.EventHandler(this.nmGameProtectionDelay_ValueChanged);
            this.nmGameProtectionDelay.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.nmGameProtectionDelay_ValueChanged);
            this.nmGameProtectionDelay.KeyUp += new System.Windows.Forms.KeyEventHandler(this.nmGameProtectionDelay_ValueChanged);
            // 
            // lbDetachedModeSettings
            // 
            this.lbDetachedModeSettings.AutoSize = true;
            this.lbDetachedModeSettings.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.lbDetachedModeSettings.ForeColor = System.Drawing.Color.White;
            this.lbDetachedModeSettings.Location = new System.Drawing.Point(81, 22);
            this.lbDetachedModeSettings.Name = "lbDetachedModeSettings";
            this.lbDetachedModeSettings.Size = new System.Drawing.Size(138, 15);
            this.lbDetachedModeSettings.TabIndex = 133;
            this.lbDetachedModeSettings.Text = "Detached Mode Settings";
            // 
            // RTC_SettingsNetCore_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(412, 352);
            this.Controls.Add(this.pnDetachedModeSettings);
            this.Controls.Add(this.lbDetachedModeSettings);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "RTC_SettingsNetCore_Form";
            this.Tag = "color:dark";
            this.Text = "NetCore";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HandleFormClosing);
            this.Load += new System.EventHandler(this.RTC_SettingsNetCore_Form_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            this.pnDetachedModeSettings.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmGameProtectionDelay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

		#endregion
		private System.Windows.Forms.Panel pnDetachedModeSettings;
		private System.Windows.Forms.GroupBox groupBox2;
		public System.Windows.Forms.ComboBox cbCrashSoundEffect;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label21;
		public System.Windows.Forms.NumericUpDown nmGameProtectionDelay;
		private System.Windows.Forms.Label lbDetachedModeSettings;
	}
}