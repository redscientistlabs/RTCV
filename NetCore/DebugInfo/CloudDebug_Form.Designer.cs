namespace RTCV.NetCore
{
	partial class CloudDebug
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CloudDebug));
            this.lbError = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnContinue = new System.Windows.Forms.Button();
            this.btnSendDebug = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tbKey = new System.Windows.Forms.TextBox();
            this.pnBottom = new System.Windows.Forms.Panel();
            this.btnDebugInfo = new System.Windows.Forms.Button();
            this.lbException = new System.Windows.Forms.Label();
            this.tbStackTrace = new System.Windows.Forms.TextBox();
            this.pnBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbError
            // 
            this.lbError.AutoSize = true;
            this.lbError.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold);
            this.lbError.ForeColor = System.Drawing.Color.White;
            this.lbError.Location = new System.Drawing.Point(8, 5);
            this.lbError.Name = "lbError";
            this.lbError.Size = new System.Drawing.Size(227, 30);
            this.lbError.TabIndex = 1;
            this.lbError.Text = "An Error has occurred";
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Location = new System.Drawing.Point(427, 37);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(82, 23);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnContinue
            // 
            this.btnContinue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnContinue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnContinue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnContinue.ForeColor = System.Drawing.Color.White;
            this.btnContinue.Location = new System.Drawing.Point(427, 8);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(82, 23);
            this.btnContinue.TabIndex = 3;
            this.btnContinue.Text = "Continue";
            this.btnContinue.UseVisualStyleBackColor = false;
            this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
            // 
            // btnSendDebug
            // 
            this.btnSendDebug.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnSendDebug.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSendDebug.ForeColor = System.Drawing.Color.White;
            this.btnSendDebug.Location = new System.Drawing.Point(12, 37);
            this.btnSendDebug.Name = "btnSendDebug";
            this.btnSendDebug.Size = new System.Drawing.Size(135, 23);
            this.btnSendDebug.TabIndex = 4;
            this.btnSendDebug.Text = "Send debug info to devs";
            this.btnSendDebug.UseVisualStyleBackColor = false;
            this.btnSendDebug.Click += new System.EventHandler(this.btnSendDebug_Click);
            this.btnSendDebug.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnSendDebug_MouseDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(152, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(243, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Encryption key (Give this key to devs for analysis):";
            // 
            // tbKey
            // 
            this.tbKey.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tbKey.ForeColor = System.Drawing.Color.White;
            this.tbKey.Location = new System.Drawing.Point(153, 24);
            this.tbKey.Multiline = true;
            this.tbKey.Name = "tbKey";
            this.tbKey.ReadOnly = true;
            this.tbKey.Size = new System.Drawing.Size(242, 37);
            this.tbKey.TabIndex = 6;
            // 
            // pnBottom
            // 
            this.pnBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.pnBottom.Controls.Add(this.btnDebugInfo);
            this.pnBottom.Controls.Add(this.tbKey);
            this.pnBottom.Controls.Add(this.btnExit);
            this.pnBottom.Controls.Add(this.btnSendDebug);
            this.pnBottom.Controls.Add(this.btnContinue);
            this.pnBottom.Controls.Add(this.label2);
            this.pnBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnBottom.Location = new System.Drawing.Point(0, 258);
            this.pnBottom.Name = "pnBottom";
            this.pnBottom.Size = new System.Drawing.Size(519, 68);
            this.pnBottom.TabIndex = 7;
            // 
            // button1
            // 
            this.btnDebugInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnDebugInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDebugInfo.ForeColor = System.Drawing.Color.White;
            this.btnDebugInfo.Location = new System.Drawing.Point(13, 8);
            this.btnDebugInfo.Name = "button1";
            this.btnDebugInfo.Size = new System.Drawing.Size(135, 23);
            this.btnDebugInfo.TabIndex = 7;
            this.btnDebugInfo.Text = "View debug info";
            this.btnDebugInfo.UseVisualStyleBackColor = false;
            this.btnDebugInfo.Click += new System.EventHandler(this.btnDebugInfo_Click);
            // 
            // lbException
            // 
            this.lbException.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbException.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.lbException.ForeColor = System.Drawing.Color.White;
            this.lbException.Location = new System.Drawing.Point(20, 32);
            this.lbException.Name = "lbException";
            this.lbException.Size = new System.Drawing.Size(495, 26);
            this.lbException.TabIndex = 8;
            this.lbException.Text = "Exception";
            // 
            // tbStackTrace
            // 
            this.tbStackTrace.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbStackTrace.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tbStackTrace.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbStackTrace.ForeColor = System.Drawing.Color.White;
            this.tbStackTrace.Location = new System.Drawing.Point(12, 58);
            this.tbStackTrace.Multiline = true;
            this.tbStackTrace.Name = "tbStackTrace";
            this.tbStackTrace.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbStackTrace.Size = new System.Drawing.Size(495, 189);
            this.tbStackTrace.TabIndex = 9;
            // 
            // CloudDebug
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(519, 326);
            this.Controls.Add(this.tbStackTrace);
            this.Controls.Add(this.lbException);
            this.Controls.Add(this.pnBottom);
            this.Controls.Add(this.lbError);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(535, 365);
            this.Name = "CloudDebug";
            this.Text = "Cloud Debug";
            this.Load += new System.EventHandler(this.CloudDebug_Load);
            this.pnBottom.ResumeLayout(false);
            this.pnBottom.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Label lbError;
		private System.Windows.Forms.Button btnExit;
		private System.Windows.Forms.Button btnContinue;
		private System.Windows.Forms.Button btnSendDebug;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox tbKey;
		private System.Windows.Forms.Panel pnBottom;
		private System.Windows.Forms.Label lbException;
		private System.Windows.Forms.TextBox tbStackTrace;
		private System.Windows.Forms.Button btnDebugInfo;
	}
}