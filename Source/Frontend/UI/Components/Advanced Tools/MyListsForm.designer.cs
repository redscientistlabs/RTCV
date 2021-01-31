namespace RTCV.UI
{
    partial class MyListsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MyListsForm));
            this.lbKnownLists = new System.Windows.Forms.ListBox();
            this.btnRefreshListFiles = new System.Windows.Forms.Button();
            this.btnImportList = new System.Windows.Forms.Button();
            this.btnSaveList = new System.Windows.Forms.Button();
            this.btnRenameList = new System.Windows.Forms.Button();
            this.btnRemoveList = new System.Windows.Forms.Button();
            this.btnEnableDisableList = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbKnownLists
            // 
            this.lbKnownLists.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbKnownLists.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lbKnownLists.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbKnownLists.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lbKnownLists.ForeColor = System.Drawing.Color.White;
            this.lbKnownLists.FormattingEnabled = true;
            this.lbKnownLists.IntegralHeight = false;
            this.lbKnownLists.Location = new System.Drawing.Point(12, 14);
            this.lbKnownLists.Margin = new System.Windows.Forms.Padding(5);
            this.lbKnownLists.Name = "lbKnownLists";
            this.lbKnownLists.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbKnownLists.Size = new System.Drawing.Size(173, 223);
            this.lbKnownLists.TabIndex = 12;
            this.lbKnownLists.Tag = "color:dark2";
            this.lbKnownLists.SelectedIndexChanged += new System.EventHandler(this.OnKnownListSelectedIndexChanged);
            this.lbKnownLists.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // btnRefreshListFiles
            // 
            this.btnRefreshListFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefreshListFiles.BackColor = System.Drawing.Color.Gray;
            this.btnRefreshListFiles.FlatAppearance.BorderSize = 0;
            this.btnRefreshListFiles.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefreshListFiles.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnRefreshListFiles.ForeColor = System.Drawing.Color.White;
            this.btnRefreshListFiles.Location = new System.Drawing.Point(197, 212);
            this.btnRefreshListFiles.Name = "btnRefreshListFiles";
            this.btnRefreshListFiles.Size = new System.Drawing.Size(182, 25);
            this.btnRefreshListFiles.TabIndex = 131;
            this.btnRefreshListFiles.TabStop = false;
            this.btnRefreshListFiles.Tag = "color:light1";
            this.btnRefreshListFiles.Text = "Refresh List Files";
            this.btnRefreshListFiles.UseVisualStyleBackColor = false;
            this.btnRefreshListFiles.Click += new System.EventHandler(this.RefreshVMDFiles);
            // 
            // btnImportList
            // 
            this.btnImportList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImportList.BackColor = System.Drawing.Color.Gray;
            this.btnImportList.FlatAppearance.BorderSize = 0;
            this.btnImportList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImportList.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnImportList.ForeColor = System.Drawing.Color.White;
            this.btnImportList.Image = ((System.Drawing.Image)(resources.GetObject("btnImportList.Image")));
            this.btnImportList.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnImportList.Location = new System.Drawing.Point(197, 61);
            this.btnImportList.Name = "btnImportList";
            this.btnImportList.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.btnImportList.Size = new System.Drawing.Size(182, 25);
            this.btnImportList.TabIndex = 173;
            this.btnImportList.TabStop = false;
            this.btnImportList.Tag = "color:light1";
            this.btnImportList.Text = "  Import List File";
            this.btnImportList.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnImportList.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnImportList.UseVisualStyleBackColor = false;
            this.btnImportList.Click += new System.EventHandler(this.ImportVMD);
            this.btnImportList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // btnSaveList
            // 
            this.btnSaveList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveList.BackColor = System.Drawing.Color.Gray;
            this.btnSaveList.Enabled = false;
            this.btnSaveList.FlatAppearance.BorderSize = 0;
            this.btnSaveList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveList.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnSaveList.ForeColor = System.Drawing.Color.White;
            this.btnSaveList.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveList.Image")));
            this.btnSaveList.Location = new System.Drawing.Point(197, 92);
            this.btnSaveList.Name = "btnSaveList";
            this.btnSaveList.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.btnSaveList.Size = new System.Drawing.Size(182, 25);
            this.btnSaveList.TabIndex = 174;
            this.btnSaveList.TabStop = false;
            this.btnSaveList.Tag = "color:light1";
            this.btnSaveList.Text = "  Save List as..";
            this.btnSaveList.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSaveList.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSaveList.UseVisualStyleBackColor = false;
            this.btnSaveList.Click += new System.EventHandler(this.SaveSelectedList);
            this.btnSaveList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // btnRenameList
            // 
            this.btnRenameList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRenameList.BackColor = System.Drawing.Color.Gray;
            this.btnRenameList.Enabled = false;
            this.btnRenameList.FlatAppearance.BorderSize = 0;
            this.btnRenameList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRenameList.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnRenameList.ForeColor = System.Drawing.Color.White;
            this.btnRenameList.Image = ((System.Drawing.Image)(resources.GetObject("btnRenameList.Image")));
            this.btnRenameList.Location = new System.Drawing.Point(197, 123);
            this.btnRenameList.Name = "btnRenameList";
            this.btnRenameList.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.btnRenameList.Size = new System.Drawing.Size(89, 25);
            this.btnRenameList.TabIndex = 176;
            this.btnRenameList.TabStop = false;
            this.btnRenameList.Tag = "color:light1";
            this.btnRenameList.Text = "  Rename";
            this.btnRenameList.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRenameList.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRenameList.UseVisualStyleBackColor = false;
            this.btnRenameList.Click += new System.EventHandler(this.RenameSelectedList);
            this.btnRenameList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // btnRemoveList
            // 
            this.btnRemoveList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveList.BackColor = System.Drawing.Color.Gray;
            this.btnRemoveList.Enabled = false;
            this.btnRemoveList.FlatAppearance.BorderSize = 0;
            this.btnRemoveList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveList.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnRemoveList.ForeColor = System.Drawing.Color.White;
            this.btnRemoveList.Image = ((System.Drawing.Image)(resources.GetObject("btnRemoveList.Image")));
            this.btnRemoveList.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRemoveList.Location = new System.Drawing.Point(292, 123);
            this.btnRemoveList.Name = "btnRemoveList";
            this.btnRemoveList.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.btnRemoveList.Size = new System.Drawing.Size(87, 25);
            this.btnRemoveList.TabIndex = 175;
            this.btnRemoveList.TabStop = false;
            this.btnRemoveList.Tag = "color:light1";
            this.btnRemoveList.Text = "  Remove";
            this.btnRemoveList.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRemoveList.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRemoveList.UseVisualStyleBackColor = false;
            this.btnRemoveList.Click += new System.EventHandler(this.RemoveSelectedList);
            this.btnRemoveList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // btnEnableDisableList
            // 
            this.btnEnableDisableList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEnableDisableList.BackColor = System.Drawing.Color.Gray;
            this.btnEnableDisableList.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnEnableDisableList.FlatAppearance.BorderSize = 0;
            this.btnEnableDisableList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnableDisableList.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnEnableDisableList.ForeColor = System.Drawing.Color.White;
            this.btnEnableDisableList.Image = ((System.Drawing.Image)(resources.GetObject("btnEnableDisableList.Image")));
            this.btnEnableDisableList.Location = new System.Drawing.Point(197, 14);
            this.btnEnableDisableList.Name = "btnEnableDisableList";
            this.btnEnableDisableList.Size = new System.Drawing.Size(182, 41);
            this.btnEnableDisableList.TabIndex = 177;
            this.btnEnableDisableList.TabStop = false;
            this.btnEnableDisableList.Tag = "color:light1";
            this.btnEnableDisableList.Text = "  Disable List";
            this.btnEnableDisableList.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnEnableDisableList.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEnableDisableList.UseVisualStyleBackColor = false;
            this.btnEnableDisableList.Click += new System.EventHandler(this.LoadSelectedList);
            this.btnEnableDisableList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // MyListsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.ClientSize = new System.Drawing.Size(390, 250);
            this.Controls.Add(this.btnEnableDisableList);
            this.Controls.Add(this.btnRenameList);
            this.Controls.Add(this.btnRemoveList);
            this.Controls.Add(this.btnSaveList);
            this.Controls.Add(this.btnImportList);
            this.Controls.Add(this.btnRefreshListFiles);
            this.Controls.Add(this.lbKnownLists);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(390, 250);
            this.Name = "MyListsForm";
            this.Tag = "color:dark1";
            this.Text = "My Lists";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HandleFormClosing);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.ListBox lbKnownLists;
        private System.Windows.Forms.Button btnRefreshListFiles;
        private System.Windows.Forms.Button btnImportList;
        public System.Windows.Forms.Button btnSaveList;
        public System.Windows.Forms.Button btnRenameList;
        public System.Windows.Forms.Button btnRemoveList;
        public System.Windows.Forms.Button btnEnableDisableList;
    }
}
