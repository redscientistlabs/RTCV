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
            this.lbConnectionStatus = new System.Windows.Forms.Label();
            this.btnStartEmuhawkDetached = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbRTCver = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lbConnectionStatus
            // 
            this.lbConnectionStatus.AutoSize = true;
            this.lbConnectionStatus.BackColor = System.Drawing.Color.Transparent;
            this.lbConnectionStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lbConnectionStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.lbConnectionStatus.Location = new System.Drawing.Point(65, 74);
            this.lbConnectionStatus.Name = "lbConnectionStatus";
            this.lbConnectionStatus.Size = new System.Drawing.Size(247, 19);
            this.lbConnectionStatus.TabIndex = 1;
            this.lbConnectionStatus.Text = "Connection status: Waiting for Bizhawk";
            // 
            // btnStartEmuhawkDetached
            // 
            this.btnStartEmuhawkDetached.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnStartEmuhawkDetached.FlatAppearance.BorderSize = 0;
            this.btnStartEmuhawkDetached.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartEmuhawkDetached.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.btnStartEmuhawkDetached.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnStartEmuhawkDetached.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnStartEmuhawkDetached.Location = new System.Drawing.Point(31, 407);
            this.btnStartEmuhawkDetached.Name = "btnStartEmuhawkDetached";
            this.btnStartEmuhawkDetached.Size = new System.Drawing.Size(167, 42);
            this.btnStartEmuhawkDetached.TabIndex = 2;
            this.btnStartEmuhawkDetached.Text = "  Start BizHawk";
            this.btnStartEmuhawkDetached.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnStartEmuhawkDetached.UseVisualStyleBackColor = false;
            this.btnStartEmuhawkDetached.Click += new System.EventHandler(this.btnStartEmuhawkDetached_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.Location = new System.Drawing.Point(31, 69);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(32, 32);
            this.panel1.TabIndex = 117;
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
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.Location = new System.Drawing.Point(19, 16);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(421, 50);
            this.pictureBox1.TabIndex = 132;
            this.pictureBox1.TabStop = false;
            // 
            // RTC_ConnectionStatus_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(655, 515);
            this.Controls.Add(this.btnStartEmuhawkDetached);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lbRTCver);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lbConnectionStatus);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "RTC_ConnectionStatus_Form";
            this.Text = "RTC_ConnectionStatus_Form";
            this.Load += new System.EventHandler(this.RTC_ConnectionStatus_Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
		public System.Windows.Forms.Label lbConnectionStatus;
		public System.Windows.Forms.Button btnStartEmuhawkDetached;
		private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbRTCver;
		private System.Windows.Forms.PictureBox pictureBox1;
	}
}