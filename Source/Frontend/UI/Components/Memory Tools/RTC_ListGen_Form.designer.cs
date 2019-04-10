namespace RTCV.UI
{
    partial class RTC_ListGen_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RTC_ListGen_Form));
            this.btnGenerateList = new System.Windows.Forms.Button();
            this.tbListValues = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbListName = new System.Windows.Forms.TextBox();
            this.cbSaveFile = new System.Windows.Forms.CheckBox();
            this.btnRefreshListsFromFile = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnHelp = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnGenerateList
            // 
            this.btnGenerateList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnGenerateList.FlatAppearance.BorderSize = 0;
            this.btnGenerateList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerateList.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnGenerateList.ForeColor = System.Drawing.Color.Black;
            this.btnGenerateList.Location = new System.Drawing.Point(6, 83);
            this.btnGenerateList.Name = "btnGenerateList";
            this.btnGenerateList.Size = new System.Drawing.Size(203, 30);
            this.btnGenerateList.TabIndex = 124;
            this.btnGenerateList.TabStop = false;
            this.btnGenerateList.Tag = "color:light";
            this.btnGenerateList.Text = "Generate New List";
            this.btnGenerateList.UseVisualStyleBackColor = false;
            this.btnGenerateList.Click += new System.EventHandler(this.btnGenerateList_Click);
            this.btnGenerateList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // tbListValues
            // 
            this.tbListValues.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbListValues.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tbListValues.ForeColor = System.Drawing.Color.White;
            this.tbListValues.Location = new System.Drawing.Point(225, 12);
            this.tbListValues.Multiline = true;
            this.tbListValues.Name = "tbListValues";
            this.tbListValues.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbListValues.Size = new System.Drawing.Size(161, 226);
            this.tbListValues.TabIndex = 128;
            this.tbListValues.Tag = "color:dark";
            this.tbListValues.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(8, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 131;
            this.label2.Text = "List Name:";
            this.label2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // tbListName
            // 
            this.tbListName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tbListName.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.tbListName.ForeColor = System.Drawing.Color.White;
            this.tbListName.Location = new System.Drawing.Point(71, 44);
            this.tbListName.Name = "tbListName";
            this.tbListName.Size = new System.Drawing.Size(136, 22);
            this.tbListName.TabIndex = 132;
            this.tbListName.Tag = "color:dark";
            this.tbListName.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // cbSaveFile
            // 
            this.cbSaveFile.Location = new System.Drawing.Point(11, 21);
            this.cbSaveFile.Name = "cbSaveFile";
            this.cbSaveFile.Size = new System.Drawing.Size(104, 24);
            this.cbSaveFile.TabIndex = 0;
            this.cbSaveFile.Text = "Save List to File";
            // 
            // btnRefreshListsFromFile
            // 
            this.btnRefreshListsFromFile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnRefreshListsFromFile.FlatAppearance.BorderSize = 0;
            this.btnRefreshListsFromFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefreshListsFromFile.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnRefreshListsFromFile.ForeColor = System.Drawing.Color.Black;
            this.btnRefreshListsFromFile.Location = new System.Drawing.Point(4, 208);
            this.btnRefreshListsFromFile.Name = "btnRefreshListsFromFile";
            this.btnRefreshListsFromFile.Size = new System.Drawing.Size(215, 30);
            this.btnRefreshListsFromFile.TabIndex = 136;
            this.btnRefreshListsFromFile.TabStop = false;
            this.btnRefreshListsFromFile.Tag = "color:light";
            this.btnRefreshListsFromFile.Text = "Refresh Lists from Files";
            this.btnRefreshListsFromFile.UseVisualStyleBackColor = false;
            this.btnRefreshListsFromFile.Click += new System.EventHandler(this.btnRefreshListsFromFile_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnHelp);
            this.groupBox1.Controls.Add(this.cbSaveFile);
            this.groupBox1.Controls.Add(this.btnGenerateList);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tbListName);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(4, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(215, 128);
            this.groupBox1.TabIndex = 137;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "List Generator";
            // 
            // btnHelp
            // 
            this.btnHelp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnHelp.FlatAppearance.BorderSize = 0;
            this.btnHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHelp.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnHelp.ForeColor = System.Drawing.Color.Black;
            this.btnHelp.Image = ((System.Drawing.Image)(resources.GetObject("btnHelp.Image")));
            this.btnHelp.Location = new System.Drawing.Point(188, 13);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(19, 19);
            this.btnHelp.TabIndex = 138;
            this.btnHelp.TabStop = false;
            this.btnHelp.Tag = "color:light";
            this.btnHelp.UseVisualStyleBackColor = false;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // RTC_ListGen_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(390, 250);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnRefreshListsFromFile);
            this.Controls.Add(this.tbListValues);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "RTC_ListGen_Form";
            this.Tag = "color:darkerer";
            this.Text = "List Generator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RTC_ListGen_Form_FormClosing);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.RTC_ListGen_Form_MouseDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnGenerateList;
        private System.Windows.Forms.TextBox tbListValues;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbListName;
		public System.Windows.Forms.CheckBox cbSaveFile;
		private System.Windows.Forms.Button btnRefreshListsFromFile;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnHelp;
	}
}