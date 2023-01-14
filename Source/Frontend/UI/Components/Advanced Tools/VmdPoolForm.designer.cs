namespace RTCV.UI
{
    partial class VmdPoolForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VmdPoolForm));
            this.lbLoadedVmdList = new System.Windows.Forms.ListBox();
            this.gbVmdSummary = new System.Windows.Forms.GroupBox();
            this.lbRealDomainValue = new System.Windows.Forms.Label();
            this.lbRealDomainLabel = new System.Windows.Forms.Label();
            this.lbVmdSizeValue = new System.Windows.Forms.Label();
            this.lbVmdSizeLabel = new System.Windows.Forms.Label();
            this.tbVmdPrototype = new System.Windows.Forms.TextBox();
            this.cbShowVmdContents = new System.Windows.Forms.CheckBox();
            this.btnReloadItem = new System.Windows.Forms.Button();
            this.btnLoadVmd = new System.Windows.Forms.Button();
            this.btnSaveVmd = new System.Windows.Forms.Button();
            this.btnRenameVmd = new System.Windows.Forms.Button();
            this.btnUnloadVmd = new System.Windows.Forms.Button();
            this.btnSendToMyVMDs = new System.Windows.Forms.Button();
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
            this.lbLoadedVmdList.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lbLoadedVmdList.ForeColor = System.Drawing.Color.White;
            this.lbLoadedVmdList.FormattingEnabled = true;
            this.lbLoadedVmdList.ItemHeight = 17;
            this.lbLoadedVmdList.Location = new System.Drawing.Point(16, 17);
            this.lbLoadedVmdList.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.lbLoadedVmdList.Name = "lbLoadedVmdList";
            this.lbLoadedVmdList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbLoadedVmdList.Size = new System.Drawing.Size(219, 155);
            this.lbLoadedVmdList.TabIndex = 12;
            this.lbLoadedVmdList.Tag = "color:dark1";
            this.lbLoadedVmdList.SelectedIndexChanged += new System.EventHandler(this.HandleLoadedVmdListSelectionChange);
            this.lbLoadedVmdList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // gbVmdSummary
            // 
            this.gbVmdSummary.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbVmdSummary.Controls.Add(this.lbRealDomainValue);
            this.gbVmdSummary.Controls.Add(this.lbRealDomainLabel);
            this.gbVmdSummary.Controls.Add(this.lbVmdSizeValue);
            this.gbVmdSummary.Controls.Add(this.lbVmdSizeLabel);
            this.gbVmdSummary.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.gbVmdSummary.ForeColor = System.Drawing.Color.White;
            this.gbVmdSummary.Location = new System.Drawing.Point(16, 177);
            this.gbVmdSummary.Margin = new System.Windows.Forms.Padding(4);
            this.gbVmdSummary.Name = "gbVmdSummary";
            this.gbVmdSummary.Padding = new System.Windows.Forms.Padding(4);
            this.gbVmdSummary.Size = new System.Drawing.Size(220, 112);
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
            this.lbRealDomainValue.Location = new System.Drawing.Point(11, 89);
            this.lbRealDomainValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbRealDomainValue.Name = "lbRealDomainValue";
            this.lbRealDomainValue.Size = new System.Drawing.Size(49, 19);
            this.lbRealDomainValue.TabIndex = 95;
            this.lbRealDomainValue.Text = "#####";
            this.lbRealDomainValue.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // lbRealDomainLabel
            // 
            this.lbRealDomainLabel.AutoSize = true;
            this.lbRealDomainLabel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lbRealDomainLabel.ForeColor = System.Drawing.Color.White;
            this.lbRealDomainLabel.Location = new System.Drawing.Point(11, 65);
            this.lbRealDomainLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbRealDomainLabel.Name = "lbRealDomainLabel";
            this.lbRealDomainLabel.Size = new System.Drawing.Size(89, 19);
            this.lbRealDomainLabel.TabIndex = 94;
            this.lbRealDomainLabel.Text = "Real Domain:";
            this.lbRealDomainLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // lbVmdSizeValue
            // 
            this.lbVmdSizeValue.AutoSize = true;
            this.lbVmdSizeValue.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lbVmdSizeValue.ForeColor = System.Drawing.Color.White;
            this.lbVmdSizeValue.Location = new System.Drawing.Point(11, 47);
            this.lbVmdSizeValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbVmdSizeValue.Name = "lbVmdSizeValue";
            this.lbVmdSizeValue.Size = new System.Drawing.Size(49, 19);
            this.lbVmdSizeValue.TabIndex = 93;
            this.lbVmdSizeValue.Text = "#####";
            this.lbVmdSizeValue.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // lbVmdSizeLabel
            // 
            this.lbVmdSizeLabel.AutoSize = true;
            this.lbVmdSizeLabel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lbVmdSizeLabel.ForeColor = System.Drawing.Color.White;
            this.lbVmdSizeLabel.Location = new System.Drawing.Point(11, 23);
            this.lbVmdSizeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbVmdSizeLabel.Name = "lbVmdSizeLabel";
            this.lbVmdSizeLabel.Size = new System.Drawing.Size(71, 19);
            this.lbVmdSizeLabel.TabIndex = 89;
            this.lbVmdSizeLabel.Text = "VMD Size:";
            this.lbVmdSizeLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // tbVmdPrototype
            // 
            this.tbVmdPrototype.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbVmdPrototype.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tbVmdPrototype.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbVmdPrototype.ForeColor = System.Drawing.Color.White;
            this.tbVmdPrototype.Location = new System.Drawing.Point(246, 202);
            this.tbVmdPrototype.Margin = new System.Windows.Forms.Padding(4);
            this.tbVmdPrototype.Multiline = true;
            this.tbVmdPrototype.Name = "tbVmdPrototype";
            this.tbVmdPrototype.ReadOnly = true;
            this.tbVmdPrototype.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbVmdPrototype.Size = new System.Drawing.Size(258, 86);
            this.tbVmdPrototype.TabIndex = 131;
            this.tbVmdPrototype.Tag = "color:dark3";
            // 
            // cbShowVmdContents
            // 
            this.cbShowVmdContents.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbShowVmdContents.AutoSize = true;
            this.cbShowVmdContents.BackColor = System.Drawing.Color.Transparent;
            this.cbShowVmdContents.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbShowVmdContents.ForeColor = System.Drawing.Color.White;
            this.cbShowVmdContents.Location = new System.Drawing.Point(263, 175);
            this.cbShowVmdContents.Margin = new System.Windows.Forms.Padding(4);
            this.cbShowVmdContents.Name = "cbShowVmdContents";
            this.cbShowVmdContents.Size = new System.Drawing.Size(160, 23);
            this.cbShowVmdContents.TabIndex = 184;
            this.cbShowVmdContents.Text = "Show VMD Contents";
            this.cbShowVmdContents.UseVisualStyleBackColor = false;
            this.cbShowVmdContents.CheckedChanged += new System.EventHandler(this.cbShowVmdContents_CheckedChanged);
            // 
            // btnReloadItem
            // 
            this.btnReloadItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReloadItem.BackColor = System.Drawing.Color.Gray;
            this.btnReloadItem.FlatAppearance.BorderSize = 0;
            this.btnReloadItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReloadItem.ForeColor = System.Drawing.Color.Black;
            this.btnReloadItem.Image = global::RTCV.UI.Properties.Resources.refresh_icon_16;
            this.btnReloadItem.Location = new System.Drawing.Point(246, 13);
            this.btnReloadItem.Margin = new System.Windows.Forms.Padding(4);
            this.btnReloadItem.Name = "btnReloadItem";
            this.btnReloadItem.Size = new System.Drawing.Size(34, 108);
            this.btnReloadItem.TabIndex = 186;
            this.btnReloadItem.Tag = "color:light1";
            this.btnReloadItem.UseVisualStyleBackColor = false;
            this.btnReloadItem.Click += new System.EventHandler(this.btnReloadItem_Click);
            // 
            // btnLoadVmd
            // 
            this.btnLoadVmd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadVmd.BackColor = System.Drawing.Color.Gray;
            this.btnLoadVmd.FlatAppearance.BorderSize = 0;
            this.btnLoadVmd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadVmd.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnLoadVmd.ForeColor = System.Drawing.Color.White;
            this.btnLoadVmd.Image = ((System.Drawing.Image)(resources.GetObject("btnLoadVmd.Image")));
            this.btnLoadVmd.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLoadVmd.Location = new System.Drawing.Point(288, 13);
            this.btnLoadVmd.Margin = new System.Windows.Forms.Padding(4);
            this.btnLoadVmd.Name = "btnLoadVmd";
            this.btnLoadVmd.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.btnLoadVmd.Size = new System.Drawing.Size(216, 31);
            this.btnLoadVmd.TabIndex = 172;
            this.btnLoadVmd.TabStop = false;
            this.btnLoadVmd.Tag = "color:light1";
            this.btnLoadVmd.Text = "  Load VMD from File";
            this.btnLoadVmd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLoadVmd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLoadVmd.UseVisualStyleBackColor = false;
            this.btnLoadVmd.Click += new System.EventHandler(this.HandleLoadVMDClick);
            this.btnLoadVmd.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // btnSaveVmd
            // 
            this.btnSaveVmd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveVmd.BackColor = System.Drawing.Color.Gray;
            this.btnSaveVmd.Enabled = false;
            this.btnSaveVmd.FlatAppearance.BorderSize = 0;
            this.btnSaveVmd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveVmd.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnSaveVmd.ForeColor = System.Drawing.Color.White;
            this.btnSaveVmd.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveVmd.Image")));
            this.btnSaveVmd.Location = new System.Drawing.Point(288, 51);
            this.btnSaveVmd.Margin = new System.Windows.Forms.Padding(4);
            this.btnSaveVmd.Name = "btnSaveVmd";
            this.btnSaveVmd.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.btnSaveVmd.Size = new System.Drawing.Size(216, 31);
            this.btnSaveVmd.TabIndex = 171;
            this.btnSaveVmd.TabStop = false;
            this.btnSaveVmd.Tag = "color:light1";
            this.btnSaveVmd.Text = "  Save VMD to File";
            this.btnSaveVmd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSaveVmd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSaveVmd.UseVisualStyleBackColor = false;
            this.btnSaveVmd.Click += new System.EventHandler(this.SaveVMD);
            this.btnSaveVmd.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // btnRenameVmd
            // 
            this.btnRenameVmd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRenameVmd.BackColor = System.Drawing.Color.Gray;
            this.btnRenameVmd.Enabled = false;
            this.btnRenameVmd.FlatAppearance.BorderSize = 0;
            this.btnRenameVmd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRenameVmd.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnRenameVmd.ForeColor = System.Drawing.Color.White;
            this.btnRenameVmd.Image = ((System.Drawing.Image)(resources.GetObject("btnRenameVmd.Image")));
            this.btnRenameVmd.Location = new System.Drawing.Point(246, 129);
            this.btnRenameVmd.Margin = new System.Windows.Forms.Padding(4);
            this.btnRenameVmd.Name = "btnRenameVmd";
            this.btnRenameVmd.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.btnRenameVmd.Size = new System.Drawing.Size(119, 31);
            this.btnRenameVmd.TabIndex = 170;
            this.btnRenameVmd.TabStop = false;
            this.btnRenameVmd.Tag = "color:light1";
            this.btnRenameVmd.Text = "  Rename";
            this.btnRenameVmd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRenameVmd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRenameVmd.UseVisualStyleBackColor = false;
            this.btnRenameVmd.Click += new System.EventHandler(this.HandleRenameVMDClick);
            this.btnRenameVmd.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // btnUnloadVmd
            // 
            this.btnUnloadVmd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUnloadVmd.BackColor = System.Drawing.Color.Gray;
            this.btnUnloadVmd.Enabled = false;
            this.btnUnloadVmd.FlatAppearance.BorderSize = 0;
            this.btnUnloadVmd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUnloadVmd.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnUnloadVmd.ForeColor = System.Drawing.Color.White;
            this.btnUnloadVmd.Image = ((System.Drawing.Image)(resources.GetObject("btnUnloadVmd.Image")));
            this.btnUnloadVmd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUnloadVmd.Location = new System.Drawing.Point(373, 129);
            this.btnUnloadVmd.Margin = new System.Windows.Forms.Padding(4);
            this.btnUnloadVmd.Name = "btnUnloadVmd";
            this.btnUnloadVmd.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.btnUnloadVmd.Size = new System.Drawing.Size(131, 31);
            this.btnUnloadVmd.TabIndex = 169;
            this.btnUnloadVmd.TabStop = false;
            this.btnUnloadVmd.Tag = "color:light1";
            this.btnUnloadVmd.Text = "  Unload";
            this.btnUnloadVmd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUnloadVmd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnUnloadVmd.UseVisualStyleBackColor = false;
            this.btnUnloadVmd.Click += new System.EventHandler(this.UnloadVMD);
            this.btnUnloadVmd.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // btnSendToMyVMDs
            // 
            this.btnSendToMyVMDs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSendToMyVMDs.BackColor = System.Drawing.Color.Gray;
            this.btnSendToMyVMDs.Enabled = false;
            this.btnSendToMyVMDs.FlatAppearance.BorderSize = 0;
            this.btnSendToMyVMDs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSendToMyVMDs.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnSendToMyVMDs.ForeColor = System.Drawing.Color.White;
            this.btnSendToMyVMDs.Image = ((System.Drawing.Image)(resources.GetObject("btnSendToMyVMDs.Image")));
            this.btnSendToMyVMDs.Location = new System.Drawing.Point(288, 90);
            this.btnSendToMyVMDs.Margin = new System.Windows.Forms.Padding(4);
            this.btnSendToMyVMDs.Name = "btnSendToMyVMDs";
            this.btnSendToMyVMDs.Size = new System.Drawing.Size(216, 31);
            this.btnSendToMyVMDs.TabIndex = 151;
            this.btnSendToMyVMDs.TabStop = false;
            this.btnSendToMyVMDs.Tag = "color:light1";
            this.btnSendToMyVMDs.Text = " Send to My VMDs";
            this.btnSendToMyVMDs.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSendToMyVMDs.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSendToMyVMDs.UseVisualStyleBackColor = false;
            this.btnSendToMyVMDs.Click += new System.EventHandler(this.SendToMyVMDs);
            // 
            // VmdPoolForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(520, 308);
            this.Controls.Add(this.btnReloadItem);
            this.Controls.Add(this.cbShowVmdContents);
            this.Controls.Add(this.btnLoadVmd);
            this.Controls.Add(this.btnSaveVmd);
            this.Controls.Add(this.btnRenameVmd);
            this.Controls.Add(this.btnUnloadVmd);
            this.Controls.Add(this.btnSendToMyVMDs);
            this.Controls.Add(this.tbVmdPrototype);
            this.Controls.Add(this.gbVmdSummary);
            this.Controls.Add(this.lbLoadedVmdList);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(520, 308);
            this.Name = "VmdPoolForm";
            this.Tag = "color:dark3";
            this.Text = "VMD Pool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HandleFormClosing);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.HandleDragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            this.gbVmdSummary.ResumeLayout(false);
            this.gbVmdSummary.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ListBox lbLoadedVmdList;
        private System.Windows.Forms.GroupBox gbVmdSummary;
        public System.Windows.Forms.Label lbRealDomainValue;
        public System.Windows.Forms.Label lbRealDomainLabel;
        public System.Windows.Forms.Label lbVmdSizeValue;
        public System.Windows.Forms.Label lbVmdSizeLabel;
        private System.Windows.Forms.TextBox tbVmdPrototype;
        public System.Windows.Forms.Button btnSendToMyVMDs;
        public System.Windows.Forms.Button btnRenameVmd;
        public System.Windows.Forms.Button btnUnloadVmd;
        public System.Windows.Forms.Button btnSaveVmd;
        private System.Windows.Forms.Button btnLoadVmd;
        public System.Windows.Forms.CheckBox cbShowVmdContents;
        private System.Windows.Forms.Button btnReloadItem;
    }
}
