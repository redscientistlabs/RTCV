namespace RTCV.UI
{
    partial class RTC_StashHistory_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RTC_StashHistory_Form));
            this.btnAddStashToStockpile = new System.Windows.Forms.Button();
            this.btnClearStashHistory = new System.Windows.Forms.Button();
            this.btnStashDOWN = new System.Windows.Forms.Button();
            this.btnStashUP = new System.Windows.Forms.Button();
            this.lbStashHistory = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // btnAddStashToStockpile
            // 
            this.btnAddStashToStockpile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddStashToStockpile.BackColor = System.Drawing.Color.Gray;
            this.btnAddStashToStockpile.FlatAppearance.BorderSize = 0;
            this.btnAddStashToStockpile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddStashToStockpile.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnAddStashToStockpile.ForeColor = System.Drawing.Color.White;
            this.btnAddStashToStockpile.Image = ((System.Drawing.Image)(resources.GetObject("btnAddStashToStockpile.Image")));
            this.btnAddStashToStockpile.Location = new System.Drawing.Point(14, 355);
            this.btnAddStashToStockpile.Name = "btnAddStashToStockpile";
            this.btnAddStashToStockpile.Size = new System.Drawing.Size(256, 32);
            this.btnAddStashToStockpile.TabIndex = 112;
            this.btnAddStashToStockpile.TabStop = false;
            this.btnAddStashToStockpile.Tag = "color:light1";
            this.btnAddStashToStockpile.Text = " To Stockpile";
            this.btnAddStashToStockpile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAddStashToStockpile.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAddStashToStockpile.UseVisualStyleBackColor = false;
            this.btnAddStashToStockpile.Click += new System.EventHandler(this.btnAddStashToStockpile_Click);
            // 
            // btnClearStashHistory
            // 
            this.btnClearStashHistory.BackColor = System.Drawing.Color.Gray;
            this.btnClearStashHistory.FlatAppearance.BorderSize = 0;
            this.btnClearStashHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearStashHistory.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnClearStashHistory.ForeColor = System.Drawing.Color.White;
            this.btnClearStashHistory.Image = ((System.Drawing.Image)(resources.GetObject("btnClearStashHistory.Image")));
            this.btnClearStashHistory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClearStashHistory.Location = new System.Drawing.Point(14, 13);
            this.btnClearStashHistory.Name = "btnClearStashHistory";
            this.btnClearStashHistory.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.btnClearStashHistory.Size = new System.Drawing.Size(105, 32);
            this.btnClearStashHistory.TabIndex = 111;
            this.btnClearStashHistory.TabStop = false;
            this.btnClearStashHistory.Tag = "color:light1";
            this.btnClearStashHistory.Text = "  Clear stash";
            this.btnClearStashHistory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClearStashHistory.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClearStashHistory.UseVisualStyleBackColor = false;
            this.btnClearStashHistory.Click += new System.EventHandler(this.btnClearStashHistory_Click);
            // 
            // btnStashDOWN
            // 
            this.btnStashDOWN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStashDOWN.BackColor = System.Drawing.Color.Gray;
            this.btnStashDOWN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnStashDOWN.FlatAppearance.BorderSize = 0;
            this.btnStashDOWN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStashDOWN.Font = new System.Drawing.Font("Segoe UI Symbol", 12F);
            this.btnStashDOWN.ForeColor = System.Drawing.Color.White;
            this.btnStashDOWN.Location = new System.Drawing.Point(238, 13);
            this.btnStashDOWN.Name = "btnStashDOWN";
            this.btnStashDOWN.Size = new System.Drawing.Size(32, 32);
            this.btnStashDOWN.TabIndex = 110;
            this.btnStashDOWN.TabStop = false;
            this.btnStashDOWN.Tag = "color:light1";
            this.btnStashDOWN.Text = "▼";
            this.btnStashDOWN.UseVisualStyleBackColor = false;
            this.btnStashDOWN.Click += new System.EventHandler(this.btnStashDOWN_Click);
            // 
            // btnStashUP
            // 
            this.btnStashUP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStashUP.BackColor = System.Drawing.Color.Gray;
            this.btnStashUP.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnStashUP.FlatAppearance.BorderSize = 0;
            this.btnStashUP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStashUP.Font = new System.Drawing.Font("Segoe UI Symbol", 12F);
            this.btnStashUP.ForeColor = System.Drawing.Color.White;
            this.btnStashUP.Location = new System.Drawing.Point(203, 13);
            this.btnStashUP.Name = "btnStashUP";
            this.btnStashUP.Size = new System.Drawing.Size(32, 32);
            this.btnStashUP.TabIndex = 109;
            this.btnStashUP.TabStop = false;
            this.btnStashUP.Tag = "color:light1";
            this.btnStashUP.Text = "▲";
            this.btnStashUP.UseVisualStyleBackColor = false;
            this.btnStashUP.Click += new System.EventHandler(this.btnStashUP_Click);
            // 
            // lbStashHistory
            // 
            this.lbStashHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbStashHistory.BackColor = System.Drawing.Color.Gray;
            this.lbStashHistory.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbStashHistory.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lbStashHistory.ForeColor = System.Drawing.Color.White;
            this.lbStashHistory.FormattingEnabled = true;
            this.lbStashHistory.IntegralHeight = false;
            this.lbStashHistory.Location = new System.Drawing.Point(14, 48);
            this.lbStashHistory.Name = "lbStashHistory";
            this.lbStashHistory.ScrollAlwaysVisible = true;
            this.lbStashHistory.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbStashHistory.Size = new System.Drawing.Size(256, 304);
            this.lbStashHistory.TabIndex = 108;
            this.lbStashHistory.Tag = "color:normal";
            this.lbStashHistory.SelectedIndexChanged += new System.EventHandler(this.lbStashHistory_SelectedIndexChanged);
            this.lbStashHistory.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbStashHistory_MouseDown);
            // 
            // RTC_StashHistory_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(284, 400);
            this.Controls.Add(this.btnAddStashToStockpile);
            this.Controls.Add(this.btnClearStashHistory);
            this.Controls.Add(this.btnStashDOWN);
            this.Controls.Add(this.btnStashUP);
            this.Controls.Add(this.lbStashHistory);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(203, 200);
            this.Name = "RTC_StashHistory_Form";
            this.Tag = "color:dark1";
            this.Text = "Stash History";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HandleFormClosing);
            this.Load += new System.EventHandler(this.RTC_StashHistory_Form_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Button btnAddStashToStockpile;
        private System.Windows.Forms.Button btnClearStashHistory;
        private System.Windows.Forms.Button btnStashDOWN;
        private System.Windows.Forms.Button btnStashUP;
        public System.Windows.Forms.ListBox lbStashHistory;
    }
}