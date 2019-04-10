namespace RTCV.UI
{
    partial class RTC_VmdPool_Form
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
            this.lbLoadedVmdList = new System.Windows.Forms.ListBox();
            this.btnUnloadVmd = new System.Windows.Forms.Button();
            this.btnLoadVmd = new System.Windows.Forms.Button();
            this.btnSaveVmd = new System.Windows.Forms.Button();
            this.gbVmdSummary = new System.Windows.Forms.GroupBox();
            this.lbRealDomainValue = new System.Windows.Forms.Label();
            this.lbRealDomainLabel = new System.Windows.Forms.Label();
            this.lbVmdSizeValue = new System.Windows.Forms.Label();
            this.lbVmdSizeLabel = new System.Windows.Forms.Label();
            this.btnRenameVMD = new System.Windows.Forms.Button();
            this.gbVmdSummary.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbLoadedVmdList
            // 
            this.lbLoadedVmdList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbLoadedVmdList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lbLoadedVmdList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbLoadedVmdList.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lbLoadedVmdList.ForeColor = System.Drawing.Color.White;
            this.lbLoadedVmdList.FormattingEnabled = true;
            this.lbLoadedVmdList.Location = new System.Drawing.Point(12, 14);
            this.lbLoadedVmdList.Margin = new System.Windows.Forms.Padding(5);
            this.lbLoadedVmdList.Name = "lbLoadedVmdList";
            this.lbLoadedVmdList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbLoadedVmdList.Size = new System.Drawing.Size(173, 223);
            this.lbLoadedVmdList.TabIndex = 12;
            this.lbLoadedVmdList.Tag = "color:dark";
            this.lbLoadedVmdList.SelectedIndexChanged += new System.EventHandler(this.lbLoadedVmdList_SelectedIndexChanged);
            this.lbLoadedVmdList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // btnUnloadVmd
            // 
            this.btnUnloadVmd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUnloadVmd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnUnloadVmd.FlatAppearance.BorderSize = 0;
            this.btnUnloadVmd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUnloadVmd.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnUnloadVmd.ForeColor = System.Drawing.Color.Black;
            this.btnUnloadVmd.Location = new System.Drawing.Point(197, 107);
            this.btnUnloadVmd.Name = "btnUnloadVmd";
            this.btnUnloadVmd.Size = new System.Drawing.Size(182, 25);
            this.btnUnloadVmd.TabIndex = 13;
            this.btnUnloadVmd.TabStop = false;
            this.btnUnloadVmd.Tag = "color:light";
            this.btnUnloadVmd.Text = "Unload Selected VMDs";
            this.btnUnloadVmd.UseVisualStyleBackColor = false;
            this.btnUnloadVmd.Click += new System.EventHandler(this.btnUnloadVMD_Click);
            this.btnUnloadVmd.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // btnLoadVmd
            // 
            this.btnLoadVmd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadVmd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnLoadVmd.FlatAppearance.BorderSize = 0;
            this.btnLoadVmd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadVmd.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnLoadVmd.ForeColor = System.Drawing.Color.Black;
            this.btnLoadVmd.Location = new System.Drawing.Point(197, 14);
            this.btnLoadVmd.Name = "btnLoadVmd";
            this.btnLoadVmd.Size = new System.Drawing.Size(182, 25);
            this.btnLoadVmd.TabIndex = 14;
            this.btnLoadVmd.TabStop = false;
            this.btnLoadVmd.Tag = "color:light";
            this.btnLoadVmd.Text = "Load VMD from File";
            this.btnLoadVmd.UseVisualStyleBackColor = false;
            this.btnLoadVmd.Click += new System.EventHandler(this.btnLoadVmd_Click);
            this.btnLoadVmd.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // btnSaveVmd
            // 
            this.btnSaveVmd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveVmd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnSaveVmd.FlatAppearance.BorderSize = 0;
            this.btnSaveVmd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveVmd.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnSaveVmd.ForeColor = System.Drawing.Color.Black;
            this.btnSaveVmd.Location = new System.Drawing.Point(197, 45);
            this.btnSaveVmd.Name = "btnSaveVmd";
            this.btnSaveVmd.Size = new System.Drawing.Size(182, 25);
            this.btnSaveVmd.TabIndex = 15;
            this.btnSaveVmd.TabStop = false;
            this.btnSaveVmd.Tag = "color:light";
            this.btnSaveVmd.Text = "Save Selected VMD to File";
            this.btnSaveVmd.UseVisualStyleBackColor = false;
            this.btnSaveVmd.Click += new System.EventHandler(this.btnSaveVmd_Click);
            this.btnSaveVmd.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // gbVmdSummary
            // 
            this.gbVmdSummary.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbVmdSummary.Controls.Add(this.lbRealDomainValue);
            this.gbVmdSummary.Controls.Add(this.lbRealDomainLabel);
            this.gbVmdSummary.Controls.Add(this.lbVmdSizeValue);
            this.gbVmdSummary.Controls.Add(this.lbVmdSizeLabel);
            this.gbVmdSummary.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.gbVmdSummary.ForeColor = System.Drawing.Color.White;
            this.gbVmdSummary.Location = new System.Drawing.Point(197, 144);
            this.gbVmdSummary.Name = "gbVmdSummary";
            this.gbVmdSummary.Size = new System.Drawing.Size(182, 91);
            this.gbVmdSummary.TabIndex = 129;
            this.gbVmdSummary.TabStop = false;
            this.gbVmdSummary.Text = "Selected VMD Summary";
            this.gbVmdSummary.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
			// 
			// lbRealDomainValue
			// 
			this.lbRealDomainValue.AutoSize = true;
            this.lbRealDomainValue.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lbRealDomainValue.ForeColor = System.Drawing.Color.White;
            this.lbRealDomainValue.Location = new System.Drawing.Point(8, 72);
            this.lbRealDomainValue.Name = "lbRealDomainValue";
            this.lbRealDomainValue.Size = new System.Drawing.Size(42, 13);
            this.lbRealDomainValue.TabIndex = 95;
            this.lbRealDomainValue.Text = "#####";
            this.lbRealDomainValue.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // lbRealDomainLabel
            // 
            this.lbRealDomainLabel.AutoSize = true;
            this.lbRealDomainLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lbRealDomainLabel.ForeColor = System.Drawing.Color.White;
            this.lbRealDomainLabel.Location = new System.Drawing.Point(8, 53);
            this.lbRealDomainLabel.Name = "lbRealDomainLabel";
            this.lbRealDomainLabel.Size = new System.Drawing.Size(85, 17);
            this.lbRealDomainLabel.TabIndex = 94;
            this.lbRealDomainLabel.Text = "Real Domain:";
            this.lbRealDomainLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // lbVmdSizeValue
            // 
            this.lbVmdSizeValue.AutoSize = true;
            this.lbVmdSizeValue.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lbVmdSizeValue.ForeColor = System.Drawing.Color.White;
            this.lbVmdSizeValue.Location = new System.Drawing.Point(8, 38);
            this.lbVmdSizeValue.Name = "lbVmdSizeValue";
            this.lbVmdSizeValue.Size = new System.Drawing.Size(42, 13);
            this.lbVmdSizeValue.TabIndex = 93;
            this.lbVmdSizeValue.Text = "#####";
            this.lbVmdSizeValue.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // lbVmdSizeLabel
            // 
            this.lbVmdSizeLabel.AutoSize = true;
            this.lbVmdSizeLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lbVmdSizeLabel.ForeColor = System.Drawing.Color.White;
            this.lbVmdSizeLabel.Location = new System.Drawing.Point(8, 19);
            this.lbVmdSizeLabel.Name = "lbVmdSizeLabel";
            this.lbVmdSizeLabel.Size = new System.Drawing.Size(67, 17);
            this.lbVmdSizeLabel.TabIndex = 89;
            this.lbVmdSizeLabel.Text = "VMD Size:";
            this.lbVmdSizeLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // btnRenameVMD
            // 
            this.btnRenameVMD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRenameVMD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnRenameVMD.FlatAppearance.BorderSize = 0;
            this.btnRenameVMD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRenameVMD.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnRenameVMD.ForeColor = System.Drawing.Color.Black;
            this.btnRenameVMD.Location = new System.Drawing.Point(197, 76);
            this.btnRenameVMD.Name = "btnRenameVMD";
            this.btnRenameVMD.Size = new System.Drawing.Size(182, 25);
            this.btnRenameVMD.TabIndex = 130;
            this.btnRenameVMD.TabStop = false;
            this.btnRenameVMD.Tag = "color:light";
            this.btnRenameVMD.Text = "Rename Selected VMD";
            this.btnRenameVMD.UseVisualStyleBackColor = false;
            this.btnRenameVMD.Click += new System.EventHandler(this.btnRenameVMD_Click);
            this.btnRenameVMD.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // RTC_VmdPool_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(390, 250);
            this.Controls.Add(this.btnRenameVMD);
            this.Controls.Add(this.gbVmdSummary);
            this.Controls.Add(this.btnSaveVmd);
            this.Controls.Add(this.btnLoadVmd);
            this.Controls.Add(this.btnUnloadVmd);
            this.Controls.Add(this.lbLoadedVmdList);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "RTC_VmdPool_Form";
            this.Tag = "color:darkerer";
            this.Text = "VMD Pool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HandleFormClosing);
            this.Load += new System.EventHandler(this.RTC_VmdPool_Form_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            this.gbVmdSummary.ResumeLayout(false);
            this.gbVmdSummary.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.ListBox lbLoadedVmdList;
        private System.Windows.Forms.Button btnUnloadVmd;
        private System.Windows.Forms.Button btnLoadVmd;
        private System.Windows.Forms.Button btnSaveVmd;
        private System.Windows.Forms.GroupBox gbVmdSummary;
        public System.Windows.Forms.Label lbRealDomainValue;
        public System.Windows.Forms.Label lbRealDomainLabel;
        public System.Windows.Forms.Label lbVmdSizeValue;
        public System.Windows.Forms.Label lbVmdSizeLabel;
        private System.Windows.Forms.Button btnRenameVMD;
    }
}