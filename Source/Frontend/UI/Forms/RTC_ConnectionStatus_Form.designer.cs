namespace RTCV.UI
{
	partial class RTC_ConnectionStatus_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RTC_ConnectionStatus_Form));
            this.btnEmergencySaveAs = new System.Windows.Forms.Button();
            this.btnTriggerKillswitch = new System.Windows.Forms.Button();
            this.lbFlavorText = new System.Windows.Forms.Label();
            this.pbName = new System.Windows.Forms.PictureBox();
            this.lbRTCver = new System.Windows.Forms.Label();
            this.lbConnectionStatus = new System.Windows.Forms.Label();
            this.pbMonster = new System.Windows.Forms.PictureBox();
            this.pnBlockedButtons = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pbName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbMonster)).BeginInit();
            this.pnBlockedButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnEmergencySaveAs
            // 
            this.btnEmergencySaveAs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnEmergencySaveAs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEmergencySaveAs.Location = new System.Drawing.Point(3, 43);
            this.btnEmergencySaveAs.Name = "btnEmergencySaveAs";
            this.btnEmergencySaveAs.Size = new System.Drawing.Size(168, 29);
            this.btnEmergencySaveAs.TabIndex = 136;
            this.btnEmergencySaveAs.Tag = "color:dark1";
            this.btnEmergencySaveAs.Text = "Emergency Save-As Stockpile";
            this.btnEmergencySaveAs.UseVisualStyleBackColor = false;
            this.btnEmergencySaveAs.Click += new System.EventHandler(this.BtnEmergencySaveAs_Click);
            // 
            // btnTriggerKillswitch
            // 
            this.btnTriggerKillswitch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnTriggerKillswitch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTriggerKillswitch.Location = new System.Drawing.Point(3, 3);
            this.btnTriggerKillswitch.Name = "btnTriggerKillswitch";
            this.btnTriggerKillswitch.Size = new System.Drawing.Size(168, 29);
            this.btnTriggerKillswitch.TabIndex = 135;
            this.btnTriggerKillswitch.Tag = "color:dark1";
            this.btnTriggerKillswitch.Text = "Trigger Killswitch";
            this.btnTriggerKillswitch.UseVisualStyleBackColor = false;
            this.btnTriggerKillswitch.MouseClick += (this.BtnTriggerKillswitch_MouseClick);
            // 
            // lbFlavorText
            // 
            this.lbFlavorText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbFlavorText.BackColor = System.Drawing.Color.Transparent;
            this.lbFlavorText.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.lbFlavorText.Location = new System.Drawing.Point(22, 481);
            this.lbFlavorText.Name = "lbFlavorText";
            this.lbFlavorText.Size = new System.Drawing.Size(384, 70);
            this.lbFlavorText.TabIndex = 134;
            this.lbFlavorText.Tag = "color:light2";
            this.lbFlavorText.Text = "Flavor text";
            this.lbFlavorText.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // pbName
            // 
            this.pbName.BackColor = System.Drawing.Color.Transparent;
            this.pbName.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbName.BackgroundImage")));
            this.pbName.Location = new System.Drawing.Point(2, 16);
            this.pbName.Name = "pbName";
            this.pbName.Size = new System.Drawing.Size(421, 50);
            this.pbName.TabIndex = 132;
            this.pbName.TabStop = false;
            // 
            // lbRTCver
            // 
            this.lbRTCver.AutoSize = true;
            this.lbRTCver.BackColor = System.Drawing.Color.Transparent;
            this.lbRTCver.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lbRTCver.ForeColor = System.Drawing.Color.White;
            this.lbRTCver.Location = new System.Drawing.Point(491, 32);
            this.lbRTCver.Name = "lbRTCver";
            this.lbRTCver.Size = new System.Drawing.Size(0, 37);
            this.lbRTCver.TabIndex = 131;
            // 
            // lbConnectionStatus
            // 
            this.lbConnectionStatus.AutoSize = true;
            this.lbConnectionStatus.BackColor = System.Drawing.Color.Transparent;
            this.lbConnectionStatus.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.lbConnectionStatus.ForeColor = System.Drawing.Color.White;
            this.lbConnectionStatus.Location = new System.Drawing.Point(17, 72);
            this.lbConnectionStatus.Name = "lbConnectionStatus";
            this.lbConnectionStatus.Size = new System.Drawing.Size(269, 25);
            this.lbConnectionStatus.TabIndex = 1;
            this.lbConnectionStatus.Text = "Waiting for vanguard to connect";
            // 
            // pbMonster
            // 
            this.pbMonster.BackColor = System.Drawing.Color.Transparent;
            this.pbMonster.Image = ((System.Drawing.Image)(resources.GetObject("pbMonster.Image")));
            this.pbMonster.Location = new System.Drawing.Point(260, 72);
            this.pbMonster.Name = "pbMonster";
            this.pbMonster.Size = new System.Drawing.Size(650, 589);
            this.pbMonster.TabIndex = 133;
            this.pbMonster.TabStop = false;
            // 
            // pnBlockedButtons
            // 
            this.pnBlockedButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pnBlockedButtons.Controls.Add(this.btnTriggerKillswitch);
            this.pnBlockedButtons.Controls.Add(this.btnEmergencySaveAs);
            this.pnBlockedButtons.Location = new System.Drawing.Point(22, 128);
            this.pnBlockedButtons.Name = "pnBlockedButtons";
            this.pnBlockedButtons.Size = new System.Drawing.Size(191, 84);
            this.pnBlockedButtons.TabIndex = 137;
            this.pnBlockedButtons.Visible = false;
            // 
            // RTC_ConnectionStatus_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(704, 560);
            this.Controls.Add(this.lbFlavorText);
            this.Controls.Add(this.pnBlockedButtons);
            this.Controls.Add(this.pbName);
            this.Controls.Add(this.lbRTCver);
            this.Controls.Add(this.lbConnectionStatus);
            this.Controls.Add(this.pbMonster);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RTC_ConnectionStatus_Form";
            this.Tag = "color:dark3";
            this.Text = "RTC_ConnectionStatus_Form";
            this.Load += new System.EventHandler(this.RTC_ConnectionStatus_Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbMonster)).EndInit();
            this.pnBlockedButtons.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
		public System.Windows.Forms.Label lbConnectionStatus;
        private System.Windows.Forms.Label lbRTCver;
		private System.Windows.Forms.PictureBox pbName;
        private System.Windows.Forms.PictureBox pbMonster;
        private System.Windows.Forms.Label lbFlavorText;
        private System.Windows.Forms.Button btnTriggerKillswitch;
        private System.Windows.Forms.Button btnEmergencySaveAs;
        public System.Windows.Forms.Panel pnBlockedButtons;
    }
}