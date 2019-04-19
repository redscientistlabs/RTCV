namespace RTC
{
    partial class AutoKillSwitch
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoKillSwitch));
			this.btnKill = new System.Windows.Forms.Button();
			this.btnKillAndRestart = new System.Windows.Forms.Button();
			this.pbTimeout = new System.Windows.Forms.ProgressBar();
			this.btnKillResetAndRestart = new System.Windows.Forms.Button();
			this.cbEnabled = new System.Windows.Forms.CheckBox();
			this.cbDetection = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnKill
			// 
			this.btnKill.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.btnKill.FlatAppearance.BorderSize = 0;
			this.btnKill.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnKill.ForeColor = System.Drawing.Color.Black;
			this.btnKill.Location = new System.Drawing.Point(13, 12);
			this.btnKill.Name = "btnKill";
			this.btnKill.Size = new System.Drawing.Size(163, 23);
			this.btnKill.TabIndex = 0;
			this.btnKill.Text = "Kill";
			this.btnKill.UseVisualStyleBackColor = false;
			this.btnKill.Click += new System.EventHandler(this.btnKill_Click);
			// 
			// btnKillAndRestart
			// 
			this.btnKillAndRestart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.btnKillAndRestart.FlatAppearance.BorderSize = 0;
			this.btnKillAndRestart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnKillAndRestart.ForeColor = System.Drawing.Color.Black;
			this.btnKillAndRestart.Location = new System.Drawing.Point(13, 38);
			this.btnKillAndRestart.Name = "btnKillAndRestart";
			this.btnKillAndRestart.Size = new System.Drawing.Size(163, 23);
			this.btnKillAndRestart.TabIndex = 1;
			this.btnKillAndRestart.Text = "Kill Restart";
			this.btnKillAndRestart.UseVisualStyleBackColor = false;
			this.btnKillAndRestart.Click += new System.EventHandler(this.btnKillAndRestart_Click);
			// 
			// pbTimeout
			// 
			this.pbTimeout.Location = new System.Drawing.Point(13, 119);
			this.pbTimeout.MarqueeAnimationSpeed = 1;
			this.pbTimeout.Maximum = 4;
			this.pbTimeout.Name = "pbTimeout";
			this.pbTimeout.Size = new System.Drawing.Size(163, 23);
			this.pbTimeout.Step = 1;
			this.pbTimeout.TabIndex = 2;
			this.pbTimeout.Value = 4;
			// 
			// btnKillResetAndRestart
			// 
			this.btnKillResetAndRestart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.btnKillResetAndRestart.FlatAppearance.BorderSize = 0;
			this.btnKillResetAndRestart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnKillResetAndRestart.ForeColor = System.Drawing.Color.Black;
			this.btnKillResetAndRestart.Location = new System.Drawing.Point(13, 64);
			this.btnKillResetAndRestart.Name = "btnKillResetAndRestart";
			this.btnKillResetAndRestart.Size = new System.Drawing.Size(163, 23);
			this.btnKillResetAndRestart.TabIndex = 3;
			this.btnKillResetAndRestart.Text = "Kill Reset Restart";
			this.btnKillResetAndRestart.UseVisualStyleBackColor = false;
			this.btnKillResetAndRestart.Click += new System.EventHandler(this.btnKillResetAndRestart_Click);
			// 
			// cbEnabled
			// 
			this.cbEnabled.AutoSize = true;
			this.cbEnabled.Checked = true;
			this.cbEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbEnabled.ForeColor = System.Drawing.Color.White;
			this.cbEnabled.Location = new System.Drawing.Point(13, 96);
			this.cbEnabled.Name = "cbEnabled";
			this.cbEnabled.Size = new System.Drawing.Size(163, 17);
			this.cbEnabled.TabIndex = 4;
			this.cbEnabled.Text = "Automatically restart Bizhawk";
			this.cbEnabled.UseVisualStyleBackColor = true;
			this.cbEnabled.CheckedChanged += new System.EventHandler(this.cbEnabled_CheckedChanged);
			// 
			// cbDetection
			// 
			this.cbDetection.BackColor = System.Drawing.Color.Black;
			this.cbDetection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbDetection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.cbDetection.ForeColor = System.Drawing.Color.White;
			this.cbDetection.FormattingEnabled = true;
			this.cbDetection.Items.AddRange(new object[] {
            "VIOLENT",
            "HEAVY",
            "MILD",
            "SLOPPY",
            "COMATOSE"});
			this.cbDetection.Location = new System.Drawing.Point(75, 153);
			this.cbDetection.Name = "cbDetection";
			this.cbDetection.Size = new System.Drawing.Size(101, 21);
			this.cbDetection.TabIndex = 5;
			this.cbDetection.SelectedIndexChanged += new System.EventHandler(this.cbDetection_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.ForeColor = System.Drawing.Color.White;
			this.label1.Location = new System.Drawing.Point(13, 156);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 13);
			this.label1.TabIndex = 6;
			this.label1.Text = "Detection:";
			// 
			// AutoKillSwitch
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.ClientSize = new System.Drawing.Size(189, 187);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cbDetection);
			this.Controls.Add(this.cbEnabled);
			this.Controls.Add(this.btnKillResetAndRestart);
			this.Controls.Add(this.pbTimeout);
			this.Controls.Add(this.btnKillAndRestart);
			this.Controls.Add(this.btnKill);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "AutoKillSwitch";
			this.Text = "KS";
			this.Load += new System.EventHandler(this.AutoKillSwitch_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnKill;
        private System.Windows.Forms.Button btnKillAndRestart;
        private System.Windows.Forms.ProgressBar pbTimeout;
        private System.Windows.Forms.Button btnKillResetAndRestart;
        private System.Windows.Forms.CheckBox cbEnabled;
        private System.Windows.Forms.ComboBox cbDetection;
        private System.Windows.Forms.Label label1;
    }
}

