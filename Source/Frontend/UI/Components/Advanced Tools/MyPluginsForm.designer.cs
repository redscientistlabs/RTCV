namespace RTCV.UI
{
    partial class MyPluginsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MyPluginsForm));
            this.lbKnownPlugins = new System.Windows.Forms.ListBox();
            this.btnRefreshListFiles = new System.Windows.Forms.Button();
            this.btnRemoveList = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbKnownPlugins
            // 
            this.lbKnownPlugins.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbKnownPlugins.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lbKnownPlugins.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbKnownPlugins.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lbKnownPlugins.ForeColor = System.Drawing.Color.White;
            this.lbKnownPlugins.FormattingEnabled = true;
            this.lbKnownPlugins.IntegralHeight = false;
            this.lbKnownPlugins.Location = new System.Drawing.Point(12, 14);
            this.lbKnownPlugins.Margin = new System.Windows.Forms.Padding(5);
            this.lbKnownPlugins.Name = "lbKnownPlugins";
            this.lbKnownPlugins.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbKnownPlugins.Size = new System.Drawing.Size(173, 223);
            this.lbKnownPlugins.TabIndex = 12;
            this.lbKnownPlugins.Tag = "color:dark2";
            this.lbKnownPlugins.SelectedIndexChanged += new System.EventHandler(this.OnKnownListSelectedIndexChanged);
            this.lbKnownPlugins.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
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
            this.btnRefreshListFiles.Text = "Refresh Plugin Files";
            this.btnRefreshListFiles.UseVisualStyleBackColor = false;
            this.btnRefreshListFiles.Click += new System.EventHandler(this.RefreshVMDFiles);
            // 
            // btnRemoveList
            // 
            this.btnRemoveList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveList.BackColor = System.Drawing.Color.Gray;
            this.btnRemoveList.Enabled = false;
            this.btnRemoveList.FlatAppearance.BorderSize = 0;
            this.btnRemoveList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveList.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnRemoveList.ForeColor = System.Drawing.Color.White;
            this.btnRemoveList.Image = ((System.Drawing.Image)(resources.GetObject("btnRemoveList.Image")));
            this.btnRemoveList.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnRemoveList.Location = new System.Drawing.Point(197, 14);
            this.btnRemoveList.Name = "btnRemoveList";
            this.btnRemoveList.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.btnRemoveList.Size = new System.Drawing.Size(182, 35);
            this.btnRemoveList.TabIndex = 175;
            this.btnRemoveList.TabStop = false;
            this.btnRemoveList.Tag = "color:light1";
            this.btnRemoveList.Text = "  Delete Plugin";
            this.btnRemoveList.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRemoveList.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRemoveList.UseVisualStyleBackColor = false;
            this.btnRemoveList.Click += new System.EventHandler(this.DeletePlugin);
            this.btnRemoveList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // MyPluginsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.ClientSize = new System.Drawing.Size(390, 250);
            this.Controls.Add(this.btnRemoveList);
            this.Controls.Add(this.btnRefreshListFiles);
            this.Controls.Add(this.lbKnownPlugins);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(390, 250);
            this.Name = "MyPluginsForm";
            this.Tag = "color:dark1";
            this.Text = "My Plugins";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HandleFormClosing);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.ListBox lbKnownPlugins;
        private System.Windows.Forms.Button btnRefreshListFiles;
        public System.Windows.Forms.Button btnRemoveList;
    }
}
