namespace RTCV.UI
{
    partial class MyVMDsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MyVMDsForm));
            this.lbLoadedVmdList = new System.Windows.Forms.ListBox();
            this.btnRefreshVmdFiles = new System.Windows.Forms.Button();
            this.btnLoadVmd = new System.Windows.Forms.Button();
            this.btnImportVmd = new System.Windows.Forms.Button();
            this.btnSaveVmd = new System.Windows.Forms.Button();
            this.btnRenameVMD = new System.Windows.Forms.Button();
            this.btnUnloadVmd = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbLoadedVmdList
            // 
            this.lbLoadedVmdList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbLoadedVmdList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lbLoadedVmdList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbLoadedVmdList.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lbLoadedVmdList.ForeColor = System.Drawing.Color.White;
            this.lbLoadedVmdList.FormattingEnabled = true;
            this.lbLoadedVmdList.IntegralHeight = false;
            this.lbLoadedVmdList.Location = new System.Drawing.Point(12, 14);
            this.lbLoadedVmdList.Margin = new System.Windows.Forms.Padding(5);
            this.lbLoadedVmdList.Name = "lbLoadedVmdList";
            this.lbLoadedVmdList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbLoadedVmdList.Size = new System.Drawing.Size(173, 223);
            this.lbLoadedVmdList.TabIndex = 12;
            this.lbLoadedVmdList.Tag = "color:dark2";
            this.lbLoadedVmdList.SelectedIndexChanged += new System.EventHandler(this.HandleLoadedVmdListSelectionChange);
            this.lbLoadedVmdList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // btnRefreshVmdFiles
            // 
            this.btnRefreshVmdFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefreshVmdFiles.BackColor = System.Drawing.Color.Gray;
            this.btnRefreshVmdFiles.FlatAppearance.BorderSize = 0;
            this.btnRefreshVmdFiles.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefreshVmdFiles.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnRefreshVmdFiles.ForeColor = System.Drawing.Color.White;
            this.btnRefreshVmdFiles.Location = new System.Drawing.Point(197, 212);
            this.btnRefreshVmdFiles.Name = "btnRefreshVmdFiles";
            this.btnRefreshVmdFiles.Size = new System.Drawing.Size(182, 25);
            this.btnRefreshVmdFiles.TabIndex = 131;
            this.btnRefreshVmdFiles.TabStop = false;
            this.btnRefreshVmdFiles.Tag = "color:light1";
            this.btnRefreshVmdFiles.Text = "Refresh VMD Files";
            this.btnRefreshVmdFiles.UseVisualStyleBackColor = false;
            this.btnRefreshVmdFiles.Click += new System.EventHandler(this.RefreshVMDFiles);
            // 
            // btnLoadVmd
            // 
            this.btnLoadVmd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadVmd.BackColor = System.Drawing.Color.Gray;
            this.btnLoadVmd.Enabled = false;
            this.btnLoadVmd.FlatAppearance.BorderSize = 0;
            this.btnLoadVmd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadVmd.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnLoadVmd.ForeColor = System.Drawing.Color.White;
            this.btnLoadVmd.Image = ((System.Drawing.Image)(resources.GetObject("btnLoadVmd.Image")));
            this.btnLoadVmd.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLoadVmd.Location = new System.Drawing.Point(197, 14);
            this.btnLoadVmd.Name = "btnLoadVmd";
            this.btnLoadVmd.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.btnLoadVmd.Size = new System.Drawing.Size(182, 41);
            this.btnLoadVmd.TabIndex = 161;
            this.btnLoadVmd.TabStop = false;
            this.btnLoadVmd.Tag = "color:light1";
            this.btnLoadVmd.Text = "  Load Selected Item";
            this.btnLoadVmd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLoadVmd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLoadVmd.UseVisualStyleBackColor = false;
            this.btnLoadVmd.Click += new System.EventHandler(this.LoadVMD);
            this.btnLoadVmd.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // btnImportVmd
            // 
            this.btnImportVmd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImportVmd.BackColor = System.Drawing.Color.Gray;
            this.btnImportVmd.FlatAppearance.BorderSize = 0;
            this.btnImportVmd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImportVmd.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnImportVmd.ForeColor = System.Drawing.Color.White;
            this.btnImportVmd.Image = ((System.Drawing.Image)(resources.GetObject("btnImportVmd.Image")));
            this.btnImportVmd.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnImportVmd.Location = new System.Drawing.Point(197, 61);
            this.btnImportVmd.Name = "btnImportVmd";
            this.btnImportVmd.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.btnImportVmd.Size = new System.Drawing.Size(182, 25);
            this.btnImportVmd.TabIndex = 173;
            this.btnImportVmd.TabStop = false;
            this.btnImportVmd.Tag = "color:light1";
            this.btnImportVmd.Text = "  Import VMD File";
            this.btnImportVmd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnImportVmd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnImportVmd.UseVisualStyleBackColor = false;
            this.btnImportVmd.Click += new System.EventHandler(this.ImportVMD);
            this.btnImportVmd.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
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
            this.btnSaveVmd.Location = new System.Drawing.Point(197, 92);
            this.btnSaveVmd.Name = "btnSaveVmd";
            this.btnSaveVmd.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.btnSaveVmd.Size = new System.Drawing.Size(182, 25);
            this.btnSaveVmd.TabIndex = 174;
            this.btnSaveVmd.TabStop = false;
            this.btnSaveVmd.Tag = "color:light1";
            this.btnSaveVmd.Text = "  Save VMD as..";
            this.btnSaveVmd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSaveVmd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSaveVmd.UseVisualStyleBackColor = false;
            this.btnSaveVmd.Click += new System.EventHandler(this.SaveVMD);
            this.btnSaveVmd.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // btnRenameVMD
            // 
            this.btnRenameVMD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRenameVMD.BackColor = System.Drawing.Color.Gray;
            this.btnRenameVMD.Enabled = false;
            this.btnRenameVMD.FlatAppearance.BorderSize = 0;
            this.btnRenameVMD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRenameVMD.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnRenameVMD.ForeColor = System.Drawing.Color.White;
            this.btnRenameVMD.Image = ((System.Drawing.Image)(resources.GetObject("btnRenameVMD.Image")));
            this.btnRenameVMD.Location = new System.Drawing.Point(197, 123);
            this.btnRenameVMD.Name = "btnRenameVMD";
            this.btnRenameVMD.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.btnRenameVMD.Size = new System.Drawing.Size(89, 25);
            this.btnRenameVMD.TabIndex = 176;
            this.btnRenameVMD.TabStop = false;
            this.btnRenameVMD.Tag = "color:light1";
            this.btnRenameVMD.Text = "  Rename";
            this.btnRenameVMD.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRenameVMD.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRenameVMD.UseVisualStyleBackColor = false;
            this.btnRenameVMD.Click += new System.EventHandler(this.RenameVMD);
            this.btnRenameVMD.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
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
            this.btnUnloadVmd.Location = new System.Drawing.Point(292, 123);
            this.btnUnloadVmd.Name = "btnUnloadVmd";
            this.btnUnloadVmd.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.btnUnloadVmd.Size = new System.Drawing.Size(87, 25);
            this.btnUnloadVmd.TabIndex = 175;
            this.btnUnloadVmd.TabStop = false;
            this.btnUnloadVmd.Tag = "color:light1";
            this.btnUnloadVmd.Text = "  Remove";
            this.btnUnloadVmd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUnloadVmd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnUnloadVmd.UseVisualStyleBackColor = false;
            this.btnUnloadVmd.Click += new System.EventHandler(this.UnloadVMD);
            this.btnUnloadVmd.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // MyVMDsForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.ClientSize = new System.Drawing.Size(390, 250);
            this.Controls.Add(this.btnRenameVMD);
            this.Controls.Add(this.btnUnloadVmd);
            this.Controls.Add(this.btnSaveVmd);
            this.Controls.Add(this.btnImportVmd);
            this.Controls.Add(this.btnLoadVmd);
            this.Controls.Add(this.btnRefreshVmdFiles);
            this.Controls.Add(this.lbLoadedVmdList);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(390, 250);
            this.Name = "MyVMDsForm";
            this.Tag = "color:dark1";
            this.Text = "My VMDs";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HandleFormClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.HandleDragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.ListBox lbLoadedVmdList;
        private System.Windows.Forms.Button btnRefreshVmdFiles;
        private System.Windows.Forms.Button btnLoadVmd;
        private System.Windows.Forms.Button btnImportVmd;
        public System.Windows.Forms.Button btnSaveVmd;
        public System.Windows.Forms.Button btnRenameVMD;
        public System.Windows.Forms.Button btnUnloadVmd;
    }
}
