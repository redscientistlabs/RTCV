namespace RTCV.UI
{
    partial class RTC_SettingsAbout_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RTC_SettingsAbout_Form));
            this.lbVersion = new System.Windows.Forms.Label();
            this.lbProcess = new System.Windows.Forms.Label();
            this.lbConnectedTo = new System.Windows.Forms.Label();
            this.lbSourceCode = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lbVersion
            // 
            this.lbVersion.AutoSize = true;
            this.lbVersion.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.lbVersion.ForeColor = System.Drawing.Color.White;
            this.lbVersion.Location = new System.Drawing.Point(14, 53);
            this.lbVersion.Name = "lbVersion";
            this.lbVersion.Size = new System.Drawing.Size(72, 21);
            this.lbVersion.TabIndex = 1;
            this.lbVersion.Text = "Version: ";
            // 
            // lbProcess
            // 
            this.lbProcess.AutoSize = true;
            this.lbProcess.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.lbProcess.ForeColor = System.Drawing.Color.White;
            this.lbProcess.Location = new System.Drawing.Point(14, 77);
            this.lbProcess.Name = "lbProcess";
            this.lbProcess.Size = new System.Drawing.Size(121, 21);
            this.lbProcess.TabIndex = 2;
            this.lbProcess.Text = "Process mode: ";
            // 
            // lbConnectedTo
            // 
            this.lbConnectedTo.AutoSize = true;
            this.lbConnectedTo.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.lbConnectedTo.ForeColor = System.Drawing.Color.White;
            this.lbConnectedTo.Location = new System.Drawing.Point(14, 101);
            this.lbConnectedTo.Name = "lbConnectedTo";
            this.lbConnectedTo.Size = new System.Drawing.Size(118, 21);
            this.lbConnectedTo.TabIndex = 3;
            this.lbConnectedTo.Text = "Connected to: ";
            // 
            // lbSourceCode
            // 
            this.lbSourceCode.AutoSize = true;
            this.lbSourceCode.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lbSourceCode.LinkColor = System.Drawing.Color.White;
            this.lbSourceCode.Location = new System.Drawing.Point(131, 326);
            this.lbSourceCode.Name = "lbSourceCode";
            this.lbSourceCode.Size = new System.Drawing.Size(230, 20);
            this.lbSourceCode.TabIndex = 4;
            this.lbSourceCode.TabStop = true;
            this.lbSourceCode.Text = "https://github.com/ircluzar/RTC3/";
            this.lbSourceCode.VisitedLinkColor = System.Drawing.Color.Gray;
            this.lbSourceCode.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LbSourceCode_LinkClicked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(14, 325);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 21);
            this.label1.TabIndex = 5;
            this.label1.Text = "Source Code:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(14, 234);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 21);
            this.label2.TabIndex = 6;
            this.label2.Text = "Made By:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(14, 257);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(140, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "Phil Girard (Ircluzar)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(14, 276);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(279, 20);
            this.label4.TabIndex = 8;
            this.label4.Text = "Dan Barreiro (Narry/SmellyFeetYouHave)";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(-7, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(426, 50);
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(14, 305);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(111, 21);
            this.label5.TabIndex = 11;
            this.label5.Text = "Official Home";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.linkLabel1.LinkColor = System.Drawing.Color.White;
            this.linkLabel1.Location = new System.Drawing.Point(131, 304);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(187, 20);
            this.linkLabel1.TabIndex = 10;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "https://redscientist.com/rtc";
            this.linkLabel1.VisitedLinkColor = System.Drawing.Color.Gray;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LbRTCHome_Clicked);
            // 
            // RTC_SettingsAbout_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(412, 352);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbSourceCode);
            this.Controls.Add(this.lbConnectedTo);
            this.Controls.Add(this.lbProcess);
            this.Controls.Add(this.lbVersion);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "RTC_SettingsAbout_Form";
            this.Tag = "color:dark";
            this.Text = "About";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HandleFormClosing);
            this.Load += new System.EventHandler(this.RTC_SettingsAbout_Form_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

		#endregion
		private System.Windows.Forms.Label lbVersion;
		private System.Windows.Forms.Label lbProcess;
		private System.Windows.Forms.Label lbConnectedTo;
		private System.Windows.Forms.LinkLabel lbSourceCode;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.LinkLabel linkLabel1;
	}
}