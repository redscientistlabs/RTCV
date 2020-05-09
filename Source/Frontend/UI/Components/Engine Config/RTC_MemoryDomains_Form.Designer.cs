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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RTC_MemoryDomains_Form));
            this.btnAutoSelectDomains = new System.Windows.Forms.Button();
            this.btnRefreshDomains = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.lbMemoryDomains = new RTCV.UI.Components.Controls.ListBoxExtended();
            this.SuspendLayout();
            // 
            // btnAutoSelectDomains
            // 
            this.btnAutoSelectDomains.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAutoSelectDomains.BackColor = System.Drawing.Color.Gray;
            this.btnAutoSelectDomains.FlatAppearance.BorderSize = 0;
            this.btnAutoSelectDomains.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAutoSelectDomains.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnAutoSelectDomains.ForeColor = System.Drawing.Color.White;
            this.btnAutoSelectDomains.Location = new System.Drawing.Point(10, 12);
            this.btnAutoSelectDomains.Name = "btnAutoSelectDomains";
            this.btnAutoSelectDomains.Size = new System.Drawing.Size(181, 24);
            this.btnAutoSelectDomains.TabIndex = 18;
            this.btnAutoSelectDomains.TabStop = false;
            this.btnAutoSelectDomains.Tag = "color:light1";
            this.btnAutoSelectDomains.Text = "Auto-select domains";
            this.btnAutoSelectDomains.UseVisualStyleBackColor = false;
            this.btnAutoSelectDomains.Click += new System.EventHandler(this.btnAutoSelectDomains_Click);
            this.btnAutoSelectDomains.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // btnRefreshDomains
            // 
            this.btnRefreshDomains.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefreshDomains.BackColor = System.Drawing.Color.Gray;
            this.btnRefreshDomains.FlatAppearance.BorderSize = 0;
            this.btnRefreshDomains.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefreshDomains.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnRefreshDomains.ForeColor = System.Drawing.Color.White;
            this.btnRefreshDomains.Location = new System.Drawing.Point(103, 255);
            this.btnRefreshDomains.Name = "btnRefreshDomains";
            this.btnRefreshDomains.Size = new System.Drawing.Size(88, 24);
            this.btnRefreshDomains.TabIndex = 16;
            this.btnRefreshDomains.TabStop = false;
            this.btnRefreshDomains.Tag = "color:light1";
            this.btnRefreshDomains.Text = "Unselect all";
            this.btnRefreshDomains.UseVisualStyleBackColor = false;
            this.btnRefreshDomains.Click += new System.EventHandler(this.btnRefreshDomains_Click);
            this.btnRefreshDomains.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSelectAll.BackColor = System.Drawing.Color.Gray;
            this.btnSelectAll.FlatAppearance.BorderSize = 0;
            this.btnSelectAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectAll.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnSelectAll.ForeColor = System.Drawing.Color.White;
            this.btnSelectAll.Location = new System.Drawing.Point(10, 255);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(88, 24);
            this.btnSelectAll.TabIndex = 17;
            this.btnSelectAll.TabStop = false;
            this.btnSelectAll.Tag = "color:light1";
            this.btnSelectAll.Text = "Select all";
            this.btnSelectAll.UseVisualStyleBackColor = false;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            this.btnSelectAll.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
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
            this.lbMemoryDomains.Tag = "color:dark2";
            this.lbMemoryDomains.SelectedIndexChanged += new System.EventHandler(this.lbMemoryDomains_SelectedIndexChanged);
            this.lbMemoryDomains.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbMemoryDomains_MouseDown);
            // 
            // RTC_MemoryDomains_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(200, 291);
            this.Controls.Add(this.lbMemoryDomains);
            this.Controls.Add(this.btnAutoSelectDomains);
            this.Controls.Add(this.btnRefreshDomains);
            this.Controls.Add(this.btnSelectAll);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RTC_MemoryDomains_Form";
            this.Tag = "color:dark1";
            this.Text = "Memory Domains";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HandleFormClosing);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            this.ResumeLayout(false);

		}

		#endregion

		public RTCV.UI.Components.Controls.ListBoxExtended lbMemoryDomains;
		private System.Windows.Forms.Button btnAutoSelectDomains;
		private System.Windows.Forms.Button btnRefreshDomains;
		private System.Windows.Forms.Button btnSelectAll;
	}
}