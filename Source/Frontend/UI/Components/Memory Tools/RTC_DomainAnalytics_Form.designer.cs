namespace RTCV.UI
{
    partial class RTC_DomainAnalytics_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RTC_DomainAnalytics_Form));
            this.btnSendToAnalytics = new System.Windows.Forms.Button();
            this.btnLoadDomains = new System.Windows.Forms.Button();
            this.cbSelectedMemoryDomain = new System.Windows.Forms.ComboBox();
            this.btnActiveTableDumpsReset = new System.Windows.Forms.Button();
            this.lbNbMemoryDumps = new System.Windows.Forms.Label();
            this.nmAutoAddSec = new System.Windows.Forms.NumericUpDown();
            this.cbAutoAddDump = new System.Windows.Forms.CheckBox();
            this.lbDomainAddressSize = new System.Windows.Forms.Label();
            this.btnActiveTableAddDump = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nmAutoAddSec)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSendToAnalytics
            // 
            this.btnSendToAnalytics.BackColor = System.Drawing.Color.Gray;
            this.btnSendToAnalytics.Enabled = false;
            this.btnSendToAnalytics.FlatAppearance.BorderSize = 0;
            this.btnSendToAnalytics.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSendToAnalytics.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnSendToAnalytics.ForeColor = System.Drawing.Color.White;
            this.btnSendToAnalytics.Location = new System.Drawing.Point(23, 196);
            this.btnSendToAnalytics.Name = "btnSendToAnalytics";
            this.btnSendToAnalytics.Size = new System.Drawing.Size(399, 35);
            this.btnSendToAnalytics.TabIndex = 127;
            this.btnSendToAnalytics.Tag = "color:light1";
            this.btnSendToAnalytics.Text = "Send dumps to analytics";
            this.btnSendToAnalytics.UseVisualStyleBackColor = false;
            this.btnSendToAnalytics.Click += new System.EventHandler(this.btnSendToAnalytics_Click);
            // 
            // btnLoadDomains
            // 
            this.btnLoadDomains.BackColor = System.Drawing.Color.Gray;
            this.btnLoadDomains.FlatAppearance.BorderSize = 0;
            this.btnLoadDomains.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadDomains.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnLoadDomains.ForeColor = System.Drawing.Color.White;
            this.btnLoadDomains.Location = new System.Drawing.Point(16, 20);
            this.btnLoadDomains.Name = "btnLoadDomains";
            this.btnLoadDomains.Size = new System.Drawing.Size(182, 25);
            this.btnLoadDomains.TabIndex = 126;
            this.btnLoadDomains.TabStop = false;
            this.btnLoadDomains.Tag = "color:light1";
            this.btnLoadDomains.Text = "Load Domains";
            this.btnLoadDomains.UseVisualStyleBackColor = false;
            this.btnLoadDomains.Click += new System.EventHandler(this.btnLoadDomains_Click);
            this.btnLoadDomains.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // cbSelectedMemoryDomain
            // 
            this.cbSelectedMemoryDomain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cbSelectedMemoryDomain.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSelectedMemoryDomain.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbSelectedMemoryDomain.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cbSelectedMemoryDomain.ForeColor = System.Drawing.Color.White;
            this.cbSelectedMemoryDomain.FormattingEnabled = true;
            this.cbSelectedMemoryDomain.Location = new System.Drawing.Point(16, 56);
            this.cbSelectedMemoryDomain.Name = "cbSelectedMemoryDomain";
            this.cbSelectedMemoryDomain.Size = new System.Drawing.Size(182, 25);
            this.cbSelectedMemoryDomain.TabIndex = 125;
            this.cbSelectedMemoryDomain.Tag = "color:dark1";
            this.cbSelectedMemoryDomain.SelectedIndexChanged += new System.EventHandler(this.cbSelectedMemoryDomain_SelectedIndexChanged);
            this.cbSelectedMemoryDomain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // btnActiveTableDumpsReset
            // 
            this.btnActiveTableDumpsReset.BackColor = System.Drawing.Color.Gray;
            this.btnActiveTableDumpsReset.Enabled = false;
            this.btnActiveTableDumpsReset.FlatAppearance.BorderSize = 0;
            this.btnActiveTableDumpsReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActiveTableDumpsReset.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnActiveTableDumpsReset.ForeColor = System.Drawing.Color.White;
            this.btnActiveTableDumpsReset.Location = new System.Drawing.Point(209, 20);
            this.btnActiveTableDumpsReset.Name = "btnActiveTableDumpsReset";
            this.btnActiveTableDumpsReset.Size = new System.Drawing.Size(213, 25);
            this.btnActiveTableDumpsReset.TabIndex = 83;
            this.btnActiveTableDumpsReset.Tag = "color:light1";
            this.btnActiveTableDumpsReset.Text = "Initialize Dump Collection";
            this.btnActiveTableDumpsReset.UseVisualStyleBackColor = false;
            this.btnActiveTableDumpsReset.Click += new System.EventHandler(this.btnActiveTableDumpsReset_Click);
            this.btnActiveTableDumpsReset.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // lbNbMemoryDumps
            // 
            this.lbNbMemoryDumps.AutoSize = true;
            this.lbNbMemoryDumps.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lbNbMemoryDumps.ForeColor = System.Drawing.Color.White;
            this.lbNbMemoryDumps.Location = new System.Drawing.Point(19, 132);
            this.lbNbMemoryDumps.Name = "lbNbMemoryDumps";
            this.lbNbMemoryDumps.Size = new System.Drawing.Size(179, 19);
            this.lbNbMemoryDumps.TabIndex = 86;
            this.lbNbMemoryDumps.Text = "Memory dumps collected: #";
            this.lbNbMemoryDumps.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // nmAutoAddSec
            // 
            this.nmAutoAddSec.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nmAutoAddSec.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.nmAutoAddSec.ForeColor = System.Drawing.Color.White;
            this.nmAutoAddSec.Location = new System.Drawing.Point(159, 100);
            this.nmAutoAddSec.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmAutoAddSec.Name = "nmAutoAddSec";
            this.nmAutoAddSec.Size = new System.Drawing.Size(48, 25);
            this.nmAutoAddSec.TabIndex = 122;
            this.nmAutoAddSec.Tag = "color:dark1";
            this.nmAutoAddSec.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmAutoAddSec.ValueChanged += new System.EventHandler(this.nmAutoAddSec_ValueChanged);
            this.nmAutoAddSec.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // cbAutoAddDump
            // 
            this.cbAutoAddDump.AutoSize = true;
            this.cbAutoAddDump.Enabled = false;
            this.cbAutoAddDump.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cbAutoAddDump.ForeColor = System.Drawing.Color.White;
            this.cbAutoAddDump.Location = new System.Drawing.Point(22, 101);
            this.cbAutoAddDump.Name = "cbAutoAddDump";
            this.cbAutoAddDump.Size = new System.Drawing.Size(137, 23);
            this.cbAutoAddDump.TabIndex = 122;
            this.cbAutoAddDump.Text = "Auto-dump every";
            this.cbAutoAddDump.UseVisualStyleBackColor = true;
            this.cbAutoAddDump.CheckedChanged += new System.EventHandler(this.cbAutoAddDump_CheckedChanged);
            this.cbAutoAddDump.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // lbDomainAddressSize
            // 
            this.lbDomainAddressSize.AutoSize = true;
            this.lbDomainAddressSize.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lbDomainAddressSize.ForeColor = System.Drawing.Color.White;
            this.lbDomainAddressSize.Location = new System.Drawing.Point(19, 160);
            this.lbDomainAddressSize.Name = "lbDomainAddressSize";
            this.lbDomainAddressSize.Size = new System.Drawing.Size(189, 19);
            this.lbDomainAddressSize.TabIndex = 82;
            this.lbDomainAddressSize.Text = "Domain address size: ######";
            this.lbDomainAddressSize.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // btnActiveTableAddDump
            // 
            this.btnActiveTableAddDump.BackColor = System.Drawing.Color.Gray;
            this.btnActiveTableAddDump.Enabled = false;
            this.btnActiveTableAddDump.FlatAppearance.BorderSize = 0;
            this.btnActiveTableAddDump.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActiveTableAddDump.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnActiveTableAddDump.ForeColor = System.Drawing.Color.White;
            this.btnActiveTableAddDump.Location = new System.Drawing.Point(209, 56);
            this.btnActiveTableAddDump.Name = "btnActiveTableAddDump";
            this.btnActiveTableAddDump.Size = new System.Drawing.Size(213, 25);
            this.btnActiveTableAddDump.TabIndex = 121;
            this.btnActiveTableAddDump.Tag = "color:light1";
            this.btnActiveTableAddDump.Text = "Add domain dump";
            this.btnActiveTableAddDump.UseVisualStyleBackColor = false;
            this.btnActiveTableAddDump.Click += new System.EventHandler(this.btnActiveTableAddDump_Click);
            this.btnActiveTableAddDump.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label16.ForeColor = System.Drawing.Color.White;
            this.label16.Location = new System.Drawing.Point(210, 101);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(28, 19);
            this.label16.TabIndex = 122;
            this.label16.Text = "sec";
            this.label16.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // RTC_DomainAnalytics_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(434, 250);
            this.Controls.Add(this.btnSendToAnalytics);
            this.Controls.Add(this.btnLoadDomains);
            this.Controls.Add(this.cbSelectedMemoryDomain);
            this.Controls.Add(this.btnActiveTableDumpsReset);
            this.Controls.Add(this.lbNbMemoryDumps);
            this.Controls.Add(this.nmAutoAddSec);
            this.Controls.Add(this.cbAutoAddDump);
            this.Controls.Add(this.lbDomainAddressSize);
            this.Controls.Add(this.btnActiveTableAddDump);
            this.Controls.Add(this.label16);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RTC_DomainAnalytics_Form";
            this.Tag = "color:dark2";
            this.Text = "Domain Analytics";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HandleFormClosing);
            this.Load += new System.EventHandler(this.RTC_VmdAct_Form_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            ((System.ComponentModel.ISupportInitialize)(this.nmAutoAddSec)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.Label lbNbMemoryDumps;
        public System.Windows.Forms.CheckBox cbAutoAddDump;
        public System.Windows.Forms.Label lbDomainAddressSize;
        public System.Windows.Forms.Button btnActiveTableAddDump;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button btnActiveTableDumpsReset;
        public System.Windows.Forms.NumericUpDown nmAutoAddSec;
		private System.Windows.Forms.Button btnLoadDomains;
		public System.Windows.Forms.ComboBox cbSelectedMemoryDomain;
        public System.Windows.Forms.Button btnSendToAnalytics;
    }
}