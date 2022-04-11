namespace RTCV.UI
{
    partial class CodeCaveSettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CodeCaveSettingsForm));
            this.btnLoadDomains = new System.Windows.Forms.Button();
            this.lbDomains = new System.Windows.Forms.Label();
            this.lbMemoryDomains = new RTCV.UI.Components.Controls.ListBoxExtended();
            this.SuspendLayout();
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
            // lbDomains
            // 
            this.lbDomains.AutoSize = true;
            this.lbDomains.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lbDomains.ForeColor = System.Drawing.Color.White;
            this.lbDomains.Location = new System.Drawing.Point(19, 132);
            this.lbDomains.Name = "lbDomains";
            this.lbDomains.Size = new System.Drawing.Size(199, 19);
            this.lbDomains.TabIndex = 86;
            this.lbDomains.Text = "Domains to search for caves in:";
            this.lbDomains.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
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
            this.lbMemoryDomains.Location = new System.Drawing.Point(246, 89);
            this.lbMemoryDomains.Margin = new System.Windows.Forms.Padding(5);
            this.lbMemoryDomains.Name = "lbMemoryDomains";
            this.lbMemoryDomains.ScrollAlwaysVisible = true;
            this.lbMemoryDomains.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lbMemoryDomains.Size = new System.Drawing.Size(174, 99);
            this.lbMemoryDomains.TabIndex = 128;
            this.lbMemoryDomains.Tag = "color:dark2";
            this.lbMemoryDomains.SelectedIndexChanged += new System.EventHandler(this.lbMemoryDomains_SelectedIndexChanged);
            // 
            // CodeCaveSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(434, 250);
            this.Controls.Add(this.lbMemoryDomains);
            this.Controls.Add(this.btnLoadDomains);
            this.Controls.Add(this.lbDomains);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CodeCaveSettingsForm";
            this.Tag = "color:dark2";
            this.Text = "Code Cave Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HandleFormClosing);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.Label lbDomains;
		private System.Windows.Forms.Button btnLoadDomains;
        public Components.Controls.ListBoxExtended lbMemoryDomains;
    }
}
