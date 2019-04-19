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
            this.lbRTCver = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.lbFlavorText = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // lbConnectionStatus
            // 
            this.lbConnectionStatus.AutoSize = true;
            this.lbConnectionStatus.BackColor = System.Drawing.Color.Transparent;
            this.lbConnectionStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lbConnectionStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.lbConnectionStatus.Location = new System.Drawing.Point(17, 72);
            this.lbConnectionStatus.Name = "lbConnectionStatus";
            this.lbConnectionStatus.Size = new System.Drawing.Size(253, 19);
            this.lbConnectionStatus.TabIndex = 1;
            this.lbConnectionStatus.Text = "Connection status: Waiting for emulator";
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
            this.pictureBox1.Location = new System.Drawing.Point(2, 16);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(421, 50);
            this.pictureBox1.TabIndex = 132;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(240, 72);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(650, 589);
            this.pictureBox2.TabIndex = 133;
            this.pictureBox2.TabStop = false;
            // 
            // lbFlavorText
            // 
            this.lbFlavorText.AutoSize = true;
            this.lbFlavorText.Font = new System.Drawing.Font("Segoe UI Symbol", 16F);
            this.lbFlavorText.Location = new System.Drawing.Point(16, 509);
            this.lbFlavorText.Name = "lbFlavorText";
            this.lbFlavorText.Size = new System.Drawing.Size(114, 30);
            this.lbFlavorText.TabIndex = 134;
            this.lbFlavorText.Text = "Flavor text";
            // 
            // RTC_ConnectionStatus_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(655, 560);
            this.Controls.Add(this.lbFlavorText);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lbRTCver);
            this.Controls.Add(this.lbConnectionStatus);
            this.Controls.Add(this.pictureBox2);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "RTC_ConnectionStatus_Form";
            this.Tag = "color:dark3";
            this.Text = "RTC_ConnectionStatus_Form";
            this.Load += new System.EventHandler(this.RTC_ConnectionStatus_Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
		public System.Windows.Forms.Label lbConnectionStatus;
        private System.Windows.Forms.Label lbRTCver;
		private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label lbFlavorText;
    }
}