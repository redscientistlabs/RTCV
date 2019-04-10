namespace RTCV.UI
{
	partial class RTC_MemoryDomains_Form
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
            this.lbMemoryDomains = new System.Windows.Forms.ListBox();
            this.btnAutoSelectDomains = new System.Windows.Forms.Button();
            this.btnRefreshDomains = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbMemoryDomains
            // 
            this.lbMemoryDomains.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbMemoryDomains.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lbMemoryDomains.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbMemoryDomains.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lbMemoryDomains.ForeColor = System.Drawing.Color.White;
            this.lbMemoryDomains.FormattingEnabled = true;
            this.lbMemoryDomains.IntegralHeight = false;
            this.lbMemoryDomains.Location = new System.Drawing.Point(10, 41);
            this.lbMemoryDomains.Margin = new System.Windows.Forms.Padding(5);
            this.lbMemoryDomains.Name = "lbMemoryDomains";
            this.lbMemoryDomains.ScrollAlwaysVisible = true;
            this.lbMemoryDomains.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lbMemoryDomains.Size = new System.Drawing.Size(181, 208);
            this.lbMemoryDomains.TabIndex = 15;
            this.lbMemoryDomains.Tag = "color:dark";
            this.lbMemoryDomains.SelectedIndexChanged += new System.EventHandler(this.lbMemoryDomains_SelectedIndexChanged);
            this.lbMemoryDomains.MouseDown += new System.Windows.Forms.MouseEventHandler(base.HandleMouseDown);
            // 
            // btnAutoSelectDomains
            // 
            this.btnAutoSelectDomains.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAutoSelectDomains.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnAutoSelectDomains.FlatAppearance.BorderSize = 0;
            this.btnAutoSelectDomains.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAutoSelectDomains.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnAutoSelectDomains.ForeColor = System.Drawing.Color.Black;
            this.btnAutoSelectDomains.Location = new System.Drawing.Point(10, 12);
            this.btnAutoSelectDomains.Name = "btnAutoSelectDomains";
            this.btnAutoSelectDomains.Size = new System.Drawing.Size(181, 24);
            this.btnAutoSelectDomains.TabIndex = 18;
            this.btnAutoSelectDomains.TabStop = false;
            this.btnAutoSelectDomains.Tag = "color:light";
            this.btnAutoSelectDomains.Text = "Auto-select domains";
            this.btnAutoSelectDomains.UseVisualStyleBackColor = false;
            this.btnAutoSelectDomains.Click += new System.EventHandler(this.btnAutoSelectDomains_Click);
            this.btnAutoSelectDomains.MouseDown += new System.Windows.Forms.MouseEventHandler(base.HandleMouseDown);
            // 
            // btnRefreshDomains
            // 
            this.btnRefreshDomains.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefreshDomains.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnRefreshDomains.FlatAppearance.BorderSize = 0;
            this.btnRefreshDomains.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefreshDomains.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnRefreshDomains.ForeColor = System.Drawing.Color.Black;
            this.btnRefreshDomains.Location = new System.Drawing.Point(103, 255);
            this.btnRefreshDomains.Name = "btnRefreshDomains";
            this.btnRefreshDomains.Size = new System.Drawing.Size(88, 24);
            this.btnRefreshDomains.TabIndex = 16;
            this.btnRefreshDomains.TabStop = false;
            this.btnRefreshDomains.Tag = "color:light";
            this.btnRefreshDomains.Text = "Unselect all";
            this.btnRefreshDomains.UseVisualStyleBackColor = false;
            this.btnRefreshDomains.Click += new System.EventHandler(this.btnRefreshDomains_Click);
            this.btnRefreshDomains.MouseDown += new System.Windows.Forms.MouseEventHandler(base.HandleMouseDown);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSelectAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnSelectAll.FlatAppearance.BorderSize = 0;
            this.btnSelectAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectAll.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnSelectAll.ForeColor = System.Drawing.Color.Black;
            this.btnSelectAll.Location = new System.Drawing.Point(10, 255);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(88, 24);
            this.btnSelectAll.TabIndex = 17;
            this.btnSelectAll.TabStop = false;
            this.btnSelectAll.Tag = "color:light";
            this.btnSelectAll.Text = "Select all";
            this.btnSelectAll.UseVisualStyleBackColor = false;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            this.btnSelectAll.MouseDown += new System.Windows.Forms.MouseEventHandler(base.HandleMouseDown);
            // 
            // RTC_MemoryDomains_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(200, 291);
            this.Controls.Add(this.lbMemoryDomains);
            this.Controls.Add(this.btnAutoSelectDomains);
            this.Controls.Add(this.btnRefreshDomains);
            this.Controls.Add(this.btnSelectAll);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "RTC_MemoryDomains_Form";
            this.Tag = "color:normal";
            this.Text = "Memory Domains";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(base.HandleFormClosing);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(base.HandleMouseDown);
            this.ResumeLayout(false);

		}

		#endregion

		public System.Windows.Forms.ListBox lbMemoryDomains;
		private System.Windows.Forms.Button btnAutoSelectDomains;
		private System.Windows.Forms.Button btnRefreshDomains;
		private System.Windows.Forms.Button btnSelectAll;
	}
}