namespace RTCV.NetCore
{
	partial class DebugInfo_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DebugInfo_Form));
            this.btnGetDebugRTC = new System.Windows.Forms.Button();
            this.tbRTC = new System.Windows.Forms.RichTextBox();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.btnGetDebugEmu = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnGetDebugRTC
            // 
            this.btnGetDebugRTC.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.btnGetDebugRTC.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGetDebugRTC.Location = new System.Drawing.Point(12, 12);
            this.btnGetDebugRTC.Name = "btnGetDebugRTC";
            this.btnGetDebugRTC.Size = new System.Drawing.Size(213, 29);
            this.btnGetDebugRTC.TabIndex = 0;
            this.btnGetDebugRTC.Tag = "color:lighter";
            this.btnGetDebugRTC.Text = "Get Debug Info (RTC)";
            this.btnGetDebugRTC.UseVisualStyleBackColor = false;
            this.btnGetDebugRTC.Click += new System.EventHandler(this.btnGetDebugRTC_Click);
            // 
            // tbRTC
            // 
            this.tbRTC.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.tbRTC.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbRTC.ForeColor = System.Drawing.Color.White;
            this.tbRTC.Location = new System.Drawing.Point(12, 47);
            this.tbRTC.Name = "tbRTC";
            this.tbRTC.Size = new System.Drawing.Size(212, 259);
            this.tbRTC.TabIndex = 1;
            this.tbRTC.Tag = "color:dark";
            this.tbRTC.Text = "";
            // 
            // richTextBox2
            // 
            this.richTextBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.richTextBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox2.ForeColor = System.Drawing.Color.White;
            this.richTextBox2.Location = new System.Drawing.Point(258, 47);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(212, 259);
            this.richTextBox2.TabIndex = 3;
            this.richTextBox2.Tag = "color:dark";
            this.richTextBox2.Text = "";
            // 
            // btnGetDebugEmu
            // 
            this.btnGetDebugEmu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGetDebugEmu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.btnGetDebugEmu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGetDebugEmu.Location = new System.Drawing.Point(258, 12);
            this.btnGetDebugEmu.Name = "btnGetDebugEmu";
            this.btnGetDebugEmu.Size = new System.Drawing.Size(213, 29);
            this.btnGetDebugEmu.TabIndex = 2;
            this.btnGetDebugEmu.Tag = "color:lighter";
            this.btnGetDebugEmu.Text = "Get Debug Info (Emu)";
            this.btnGetDebugEmu.UseVisualStyleBackColor = false;
            this.btnGetDebugEmu.Click += new System.EventHandler(this.btnGetDebugEmu_Click);
            // 
            // DebugInfo_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(485, 314);
            this.Controls.Add(this.richTextBox2);
            this.Controls.Add(this.btnGetDebugEmu);
            this.Controls.Add(this.tbRTC);
            this.Controls.Add(this.btnGetDebugRTC);
            this.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F);
            this.ForeColor = System.Drawing.Color.White;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DebugInfo_Form";
            this.Tag = "color:normal";
            this.Text = "Debug Info";
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnGetDebugRTC;
		private System.Windows.Forms.RichTextBox tbRTC;
		private System.Windows.Forms.RichTextBox richTextBox2;
		private System.Windows.Forms.Button btnGetDebugEmu;
	}
}