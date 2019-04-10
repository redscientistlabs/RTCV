namespace RTCV.UI
{
    partial class RTC_VmdGen_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RTC_VmdGen_Form));
            this.cbSelectedMemoryDomain = new System.Windows.Forms.ComboBox();
            this.btnLoadDomains = new System.Windows.Forms.Button();
            this.label17 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbEndianTypeValue = new System.Windows.Forms.Label();
            this.lbWordSizeValue = new System.Windows.Forms.Label();
            this.lbDomainSizeValue = new System.Windows.Forms.Label();
            this.lbEndianTypeLabel = new System.Windows.Forms.Label();
            this.lbWordSizeLabel = new System.Windows.Forms.Label();
            this.lbDomainSizeLabel = new System.Windows.Forms.Label();
            this.btnGenerateVMD = new System.Windows.Forms.Button();
            this.cbUsePointerSpacer = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbCustomAddresses = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbVmdName = new System.Windows.Forms.TextBox();
            this.btnHelp = new System.Windows.Forms.Button();
            this.nmPointerSpacer = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.nmPadding = new System.Windows.Forms.NumericUpDown();
            this.cbUsePadding = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmPointerSpacer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmPadding)).BeginInit();
            this.SuspendLayout();
            // 
            // cbSelectedMemoryDomain
            // 
            this.cbSelectedMemoryDomain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cbSelectedMemoryDomain.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSelectedMemoryDomain.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbSelectedMemoryDomain.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.cbSelectedMemoryDomain.ForeColor = System.Drawing.Color.White;
            this.cbSelectedMemoryDomain.FormattingEnabled = true;
            this.cbSelectedMemoryDomain.Location = new System.Drawing.Point(89, 26);
            this.cbSelectedMemoryDomain.Name = "cbSelectedMemoryDomain";
            this.cbSelectedMemoryDomain.Size = new System.Drawing.Size(130, 25);
            this.cbSelectedMemoryDomain.TabIndex = 16;
            this.cbSelectedMemoryDomain.Tag = "color:dark";
            this.cbSelectedMemoryDomain.SelectedIndexChanged += new System.EventHandler(this.cbSelectedMemoryDomain_SelectedIndexChanged);
            this.cbSelectedMemoryDomain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // btnLoadDomains
            // 
            this.btnLoadDomains.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnLoadDomains.FlatAppearance.BorderSize = 0;
            this.btnLoadDomains.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadDomains.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnLoadDomains.ForeColor = System.Drawing.Color.Black;
            this.btnLoadDomains.Location = new System.Drawing.Point(4, 5);
            this.btnLoadDomains.Name = "btnLoadDomains";
            this.btnLoadDomains.Size = new System.Drawing.Size(80, 47);
            this.btnLoadDomains.TabIndex = 17;
            this.btnLoadDomains.TabStop = false;
            this.btnLoadDomains.Tag = "color:light";
            this.btnLoadDomains.Text = "Load Domains";
            this.btnLoadDomains.UseVisualStyleBackColor = false;
            this.btnLoadDomains.Click += new System.EventHandler(this.btnSelectAll_Click);
            this.btnLoadDomains.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.label17.ForeColor = System.Drawing.Color.White;
            this.label17.Location = new System.Drawing.Point(85, 4);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(115, 19);
            this.label17.TabIndex = 117;
            this.label17.Text = "Memory Domain";
            this.label17.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbEndianTypeValue);
            this.groupBox1.Controls.Add(this.lbWordSizeValue);
            this.groupBox1.Controls.Add(this.lbDomainSizeValue);
            this.groupBox1.Controls.Add(this.lbEndianTypeLabel);
            this.groupBox1.Controls.Add(this.lbWordSizeLabel);
            this.groupBox1.Controls.Add(this.lbDomainSizeLabel);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(4, 54);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(215, 81);
            this.groupBox1.TabIndex = 123;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Domain summary";
            this.groupBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // lbEndianTypeValue
            // 
            this.lbEndianTypeValue.AutoSize = true;
            this.lbEndianTypeValue.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lbEndianTypeValue.ForeColor = System.Drawing.Color.White;
            this.lbEndianTypeValue.Location = new System.Drawing.Point(82, 58);
            this.lbEndianTypeValue.Name = "lbEndianTypeValue";
            this.lbEndianTypeValue.Size = new System.Drawing.Size(42, 13);
            this.lbEndianTypeValue.TabIndex = 92;
            this.lbEndianTypeValue.Text = "#####";
            this.lbEndianTypeValue.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // lbWordSizeValue
            // 
            this.lbWordSizeValue.AutoSize = true;
            this.lbWordSizeValue.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lbWordSizeValue.ForeColor = System.Drawing.Color.White;
            this.lbWordSizeValue.Location = new System.Drawing.Point(82, 39);
            this.lbWordSizeValue.Name = "lbWordSizeValue";
            this.lbWordSizeValue.Size = new System.Drawing.Size(42, 13);
            this.lbWordSizeValue.TabIndex = 91;
            this.lbWordSizeValue.Text = "#####";
            this.lbWordSizeValue.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // lbDomainSizeValue
            // 
            this.lbDomainSizeValue.AutoSize = true;
            this.lbDomainSizeValue.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lbDomainSizeValue.ForeColor = System.Drawing.Color.White;
            this.lbDomainSizeValue.Location = new System.Drawing.Point(82, 22);
            this.lbDomainSizeValue.Name = "lbDomainSizeValue";
            this.lbDomainSizeValue.Size = new System.Drawing.Size(42, 13);
            this.lbDomainSizeValue.TabIndex = 90;
            this.lbDomainSizeValue.Text = "#####";
            this.lbDomainSizeValue.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // lbEndianTypeLabel
            // 
            this.lbEndianTypeLabel.AutoSize = true;
            this.lbEndianTypeLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lbEndianTypeLabel.ForeColor = System.Drawing.Color.White;
            this.lbEndianTypeLabel.Location = new System.Drawing.Point(2, 54);
            this.lbEndianTypeLabel.Name = "lbEndianTypeLabel";
            this.lbEndianTypeLabel.Size = new System.Drawing.Size(81, 17);
            this.lbEndianTypeLabel.TabIndex = 88;
            this.lbEndianTypeLabel.Text = "Endian Type:";
            this.lbEndianTypeLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // lbWordSizeLabel
            // 
            this.lbWordSizeLabel.AutoSize = true;
            this.lbWordSizeLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lbWordSizeLabel.ForeColor = System.Drawing.Color.White;
            this.lbWordSizeLabel.Location = new System.Drawing.Point(2, 36);
            this.lbWordSizeLabel.Name = "lbWordSizeLabel";
            this.lbWordSizeLabel.Size = new System.Drawing.Size(70, 17);
            this.lbWordSizeLabel.TabIndex = 87;
            this.lbWordSizeLabel.Text = "Word Size:";
            this.lbWordSizeLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // lbDomainSizeLabel
            // 
            this.lbDomainSizeLabel.AutoSize = true;
            this.lbDomainSizeLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lbDomainSizeLabel.ForeColor = System.Drawing.Color.White;
            this.lbDomainSizeLabel.Location = new System.Drawing.Point(2, 18);
            this.lbDomainSizeLabel.Name = "lbDomainSizeLabel";
            this.lbDomainSizeLabel.Size = new System.Drawing.Size(83, 17);
            this.lbDomainSizeLabel.TabIndex = 86;
            this.lbDomainSizeLabel.Text = "Domain Size:";
            this.lbDomainSizeLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // btnGenerateVMD
            // 
            this.btnGenerateVMD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnGenerateVMD.FlatAppearance.BorderSize = 0;
            this.btnGenerateVMD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerateVMD.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnGenerateVMD.ForeColor = System.Drawing.Color.Black;
            this.btnGenerateVMD.Location = new System.Drawing.Point(4, 216);
            this.btnGenerateVMD.Name = "btnGenerateVMD";
            this.btnGenerateVMD.Size = new System.Drawing.Size(215, 30);
            this.btnGenerateVMD.TabIndex = 124;
            this.btnGenerateVMD.TabStop = false;
            this.btnGenerateVMD.Tag = "color:light";
            this.btnGenerateVMD.Text = "Generate VMD";
            this.btnGenerateVMD.UseVisualStyleBackColor = false;
            this.btnGenerateVMD.Click += new System.EventHandler(this.btnGenerateVMD_Click);
            this.btnGenerateVMD.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // cbUsePointerSpacer
            // 
            this.cbUsePointerSpacer.AutoSize = true;
            this.cbUsePointerSpacer.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbUsePointerSpacer.ForeColor = System.Drawing.Color.White;
            this.cbUsePointerSpacer.Location = new System.Drawing.Point(4, 143);
            this.cbUsePointerSpacer.Name = "cbUsePointerSpacer";
            this.cbUsePointerSpacer.Size = new System.Drawing.Size(112, 17);
            this.cbUsePointerSpacer.TabIndex = 125;
            this.cbUsePointerSpacer.Text = "Set pointer every";
            this.cbUsePointerSpacer.UseVisualStyleBackColor = true;
            this.cbUsePointerSpacer.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(163, 144);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 127;
            this.label4.Text = "addresses";
            this.label4.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // tbCustomAddresses
            // 
            this.tbCustomAddresses.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbCustomAddresses.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tbCustomAddresses.Font = new System.Drawing.Font("Consolas", 8F);
            this.tbCustomAddresses.ForeColor = System.Drawing.Color.White;
            this.tbCustomAddresses.Location = new System.Drawing.Point(225, 24);
            this.tbCustomAddresses.Multiline = true;
            this.tbCustomAddresses.Name = "tbCustomAddresses";
            this.tbCustomAddresses.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbCustomAddresses.Size = new System.Drawing.Size(161, 222);
            this.tbCustomAddresses.TabIndex = 128;
            this.tbCustomAddresses.Tag = "color:dark";
            this.tbCustomAddresses.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(222, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(131, 15);
            this.label5.TabIndex = 129;
            this.label5.Text = "Remove/Add addresses";
            this.label5.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(4, 197);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 131;
            this.label2.Text = "VMD Name:";
            this.label2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // tbVmdName
            // 
            this.tbVmdName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tbVmdName.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.tbVmdName.ForeColor = System.Drawing.Color.White;
            this.tbVmdName.Location = new System.Drawing.Point(72, 191);
            this.tbVmdName.Name = "tbVmdName";
            this.tbVmdName.Size = new System.Drawing.Size(147, 22);
            this.tbVmdName.TabIndex = 132;
            this.tbVmdName.Tag = "color:dark";
            this.tbVmdName.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // btnHelp
            // 
            this.btnHelp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnHelp.FlatAppearance.BorderSize = 0;
            this.btnHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHelp.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnHelp.ForeColor = System.Drawing.Color.Black;
            this.btnHelp.Image = ((System.Drawing.Image)(resources.GetObject("btnHelp.Image")));
            this.btnHelp.Location = new System.Drawing.Point(367, 4);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(19, 19);
            this.btnHelp.TabIndex = 134;
            this.btnHelp.TabStop = false;
            this.btnHelp.Tag = "color:light";
            this.btnHelp.UseVisualStyleBackColor = false;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            this.btnHelp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // nmPointerSpacer
            // 
            this.nmPointerSpacer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nmPointerSpacer.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.nmPointerSpacer.ForeColor = System.Drawing.Color.White;
            this.nmPointerSpacer.Location = new System.Drawing.Point(114, 140);
            this.nmPointerSpacer.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nmPointerSpacer.Name = "nmPointerSpacer";
            this.nmPointerSpacer.Size = new System.Drawing.Size(47, 23);
            this.nmPointerSpacer.TabIndex = 126;
            this.nmPointerSpacer.Tag = "color:dark";
            this.nmPointerSpacer.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nmPointerSpacer.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            this.nmPointerSpacer.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(98, 168);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 137;
            this.label1.Text = "bytes of padding";
            this.label1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // nmPadding
            // 
            this.nmPadding.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nmPadding.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.nmPadding.ForeColor = System.Drawing.Color.White;
            this.nmPadding.Location = new System.Drawing.Point(49, 164);
            this.nmPadding.Maximum = new decimal(new int[] {
            8192,
            0,
            0,
            0});
            this.nmPadding.Name = "nmPadding";
            this.nmPadding.Size = new System.Drawing.Size(47, 22);
            this.nmPadding.TabIndex = 136;
            this.nmPadding.Tag = "color:dark";
            this.nmPadding.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmPadding.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // cbUsePadding
            // 
            this.cbUsePadding.AutoSize = true;
            this.cbUsePadding.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbUsePadding.ForeColor = System.Drawing.Color.White;
            this.cbUsePadding.Location = new System.Drawing.Point(4, 167);
            this.cbUsePadding.Name = "cbUsePadding";
            this.cbUsePadding.Size = new System.Drawing.Size(47, 17);
            this.cbUsePadding.TabIndex = 135;
            this.cbUsePadding.Text = "Add";
            this.cbUsePadding.UseVisualStyleBackColor = true;
            this.cbUsePadding.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // RTC_VmdGen_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(390, 250);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nmPadding);
            this.Controls.Add(this.cbUsePadding);
            this.Controls.Add(this.btnHelp);
            this.Controls.Add(this.tbVmdName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbCustomAddresses);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.nmPointerSpacer);
            this.Controls.Add(this.cbUsePointerSpacer);
            this.Controls.Add(this.btnGenerateVMD);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.btnLoadDomains);
            this.Controls.Add(this.cbSelectedMemoryDomain);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "RTC_VmdGen_Form";
            this.Tag = "color:darkerer";
            this.Text = "VMD Generator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RTC_VmdGen_Form_FormClosing);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.RTC_VmdGen_Form_MouseDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmPointerSpacer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmPadding)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ComboBox cbSelectedMemoryDomain;
        private System.Windows.Forms.Button btnLoadDomains;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.Label lbWordSizeLabel;
        public System.Windows.Forms.Label lbDomainSizeLabel;
        public System.Windows.Forms.Label lbEndianTypeLabel;
        private System.Windows.Forms.Button btnGenerateVMD;
        public System.Windows.Forms.CheckBox cbUsePointerSpacer;
        public System.Windows.Forms.NumericUpDown nmPointerSpacer;
        public System.Windows.Forms.Label lbDomainSizeValue;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.Label lbEndianTypeValue;
        public System.Windows.Forms.Label lbWordSizeValue;
        private System.Windows.Forms.TextBox tbCustomAddresses;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbVmdName;
        private System.Windows.Forms.Button btnHelp;
		private System.Windows.Forms.Label label1;
		public System.Windows.Forms.NumericUpDown nmPadding;
		public System.Windows.Forms.CheckBox cbUsePadding;
	}
}