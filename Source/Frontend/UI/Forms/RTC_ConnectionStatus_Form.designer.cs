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
            this.label1 = new System.Windows.Forms.Label();
            this.lbFlavorText = new System.Windows.Forms.Label();
            this.pnBlockedButtons = new System.Windows.Forms.Panel();
            this.btnTriggerKillswitch = new System.Windows.Forms.Button();
            this.btnEmergencySaveAs = new System.Windows.Forms.Button();
            this.lbRTCver = new System.Windows.Forms.Label();
            this.lbConnectionStatus = new System.Windows.Forms.Label();
            this.pbMonster = new System.Windows.Forms.PictureBox();
            this.btnBreakCrashLoop = new System.Windows.Forms.Button();
            this.pnBlockedButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbMonster)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 30F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(8, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(393, 54);
            this.label1.TabIndex = 138;
            this.label1.Text = "Real-Time Corruptor";
            // 
            // lbFlavorText
            // 
            this.lbFlavorText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbFlavorText.BackColor = System.Drawing.Color.Transparent;
            this.lbFlavorText.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.lbFlavorText.Location = new System.Drawing.Point(17, 481);
            this.lbFlavorText.Name = "lbFlavorText";
            this.lbFlavorText.Size = new System.Drawing.Size(384, 70);
            this.lbFlavorText.TabIndex = 134;
            this.lbFlavorText.Tag = "color:light2";
            this.lbFlavorText.Text = "Flavor text";
            this.lbFlavorText.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // pnBlockedButtons
            // 
            this.pnBlockedButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pnBlockedButtons.Controls.Add(this.btnBreakCrashLoop);
            this.pnBlockedButtons.Controls.Add(this.btnTriggerKillswitch);
            this.pnBlockedButtons.Controls.Add(this.btnEmergencySaveAs);
            this.pnBlockedButtons.Location = new System.Drawing.Point(22, 128);
            this.pnBlockedButtons.Name = "pnBlockedButtons";
            this.pnBlockedButtons.Size = new System.Drawing.Size(191, 128);
            this.pnBlockedButtons.TabIndex = 137;
            this.pnBlockedButtons.Visible = false;
            // 
            // btnTriggerKillswitch
            // 
            this.btnTriggerKillswitch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnTriggerKillswitch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTriggerKillswitch.Location = new System.Drawing.Point(0, 3);
            this.btnTriggerKillswitch.Name = "btnTriggerKillswitch";
            this.btnTriggerKillswitch.Size = new System.Drawing.Size(168, 29);
            this.btnTriggerKillswitch.TabIndex = 135;
            this.btnTriggerKillswitch.Tag = "color:dark1";
            this.btnTriggerKillswitch.Text = "Trigger Killswitch";
            this.btnTriggerKillswitch.UseVisualStyleBackColor = false;
            // 
            // btnEmergencySaveAs
            // 
            this.btnEmergencySaveAs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnEmergencySaveAs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEmergencySaveAs.Location = new System.Drawing.Point(0, 73);
            this.btnEmergencySaveAs.Name = "btnEmergencySaveAs";
            this.btnEmergencySaveAs.Size = new System.Drawing.Size(168, 29);
            this.btnEmergencySaveAs.TabIndex = 136;
            this.btnEmergencySaveAs.Tag = "color:dark1";
            this.btnEmergencySaveAs.Text = "Emergency Save-As Stockpile";
            this.btnEmergencySaveAs.UseVisualStyleBackColor = false;
            this.btnEmergencySaveAs.Click += new System.EventHandler(this.BtnEmergencySaveAs_Click);
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
            this.lbConnectionStatus.Location = new System.Drawing.Point(18, 69);
            this.lbConnectionStatus.Name = "lbConnectionStatus";
            this.lbConnectionStatus.Size = new System.Drawing.Size(316, 25);
            this.lbConnectionStatus.TabIndex = 1;
            this.lbConnectionStatus.Text = "Waiting for Vanguard client to connect";
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
            // btnBreakCrashLoop
            // 
            this.btnBreakCrashLoop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnBreakCrashLoop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBreakCrashLoop.Location = new System.Drawing.Point(0, 38);
            this.btnBreakCrashLoop.Name = "btnBreakCrashLoop";
            this.btnBreakCrashLoop.Size = new System.Drawing.Size(168, 29);
            this.btnBreakCrashLoop.TabIndex = 137;
            this.btnBreakCrashLoop.Tag = "color:dark1";
            this.btnBreakCrashLoop.Text = "Emergency Break Crash-Loop";
            this.btnBreakCrashLoop.UseVisualStyleBackColor = false;
            this.btnBreakCrashLoop.Click += new System.EventHandler(this.btnBreakCrashLoop_Click);
            // 
            // RTC_ConnectionStatus_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(704, 560);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbFlavorText);
            this.Controls.Add(this.pnBlockedButtons);
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
            this.pnBlockedButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbMonster)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
		public System.Windows.Forms.Label lbConnectionStatus;
        private System.Windows.Forms.Label lbRTCver;
        private System.Windows.Forms.PictureBox pbMonster;
        private System.Windows.Forms.Label lbFlavorText;
        private System.Windows.Forms.Button btnTriggerKillswitch;
        private System.Windows.Forms.Button btnEmergencySaveAs;
        public System.Windows.Forms.Panel pnBlockedButtons;
        public System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBreakCrashLoop;
    }
}