namespace RTCV.GlitchHarvester.TileForms
{
    partial class GH_Engine_MemoryDomains
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
            this.lbMemoryDomains = new System.Windows.Forms.ListBox();
            this.btnAutoSelectDomains = new System.Windows.Forms.Button();
            this.btnRefreshDomains = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbMemoryDomains
            // 
            this.lbMemoryDomains.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.lbMemoryDomains.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbMemoryDomains.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lbMemoryDomains.ForeColor = System.Drawing.Color.White;
            this.lbMemoryDomains.FormattingEnabled = true;
            this.lbMemoryDomains.IntegralHeight = false;
            this.lbMemoryDomains.Location = new System.Drawing.Point(12, 32);
            this.lbMemoryDomains.Margin = new System.Windows.Forms.Padding(5);
            this.lbMemoryDomains.Name = "lbMemoryDomains";
            this.lbMemoryDomains.ScrollAlwaysVisible = true;
            this.lbMemoryDomains.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lbMemoryDomains.Size = new System.Drawing.Size(254, 204);
            this.lbMemoryDomains.TabIndex = 15;
            this.lbMemoryDomains.Tag = "color:dark";
            // 
            // btnAutoSelectDomains
            // 
            this.btnAutoSelectDomains.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnAutoSelectDomains.FlatAppearance.BorderSize = 0;
            this.btnAutoSelectDomains.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAutoSelectDomains.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnAutoSelectDomains.ForeColor = System.Drawing.Color.Black;
            this.btnAutoSelectDomains.Location = new System.Drawing.Point(12, 244);
            this.btnAutoSelectDomains.Name = "btnAutoSelectDomains";
            this.btnAutoSelectDomains.Size = new System.Drawing.Size(99, 24);
            this.btnAutoSelectDomains.TabIndex = 18;
            this.btnAutoSelectDomains.TabStop = false;
            this.btnAutoSelectDomains.Tag = "color:light";
            this.btnAutoSelectDomains.Text = "Auto-select";
            this.btnAutoSelectDomains.UseVisualStyleBackColor = false;
            // 
            // btnRefreshDomains
            // 
            this.btnRefreshDomains.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnRefreshDomains.FlatAppearance.BorderSize = 0;
            this.btnRefreshDomains.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefreshDomains.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnRefreshDomains.ForeColor = System.Drawing.Color.Black;
            this.btnRefreshDomains.Location = new System.Drawing.Point(188, 244);
            this.btnRefreshDomains.Name = "btnRefreshDomains";
            this.btnRefreshDomains.Size = new System.Drawing.Size(80, 24);
            this.btnRefreshDomains.TabIndex = 16;
            this.btnRefreshDomains.TabStop = false;
            this.btnRefreshDomains.Tag = "color:light";
            this.btnRefreshDomains.Text = "Unselect all";
            this.btnRefreshDomains.UseVisualStyleBackColor = false;
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnSelectAll.FlatAppearance.BorderSize = 0;
            this.btnSelectAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectAll.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnSelectAll.ForeColor = System.Drawing.Color.Black;
            this.btnSelectAll.Location = new System.Drawing.Point(117, 244);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(65, 24);
            this.btnSelectAll.TabIndex = 17;
            this.btnSelectAll.TabStop = false;
            this.btnSelectAll.Tag = "color:light";
            this.btnSelectAll.Text = "Select all";
            this.btnSelectAll.UseVisualStyleBackColor = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(8, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 19);
            this.label3.TabIndex = 19;
            this.label3.Text = "Memory Domains";
            // 
            // GH_Engine_MemoryDomains
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(280, 280);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lbMemoryDomains);
            this.Controls.Add(this.btnAutoSelectDomains);
            this.Controls.Add(this.btnRefreshDomains);
            this.Controls.Add(this.btnSelectAll);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "GH_Engine_MemoryDomains";
            this.Text = "GH_DummyTileForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ListBox lbMemoryDomains;
        private System.Windows.Forms.Button btnAutoSelectDomains;
        private System.Windows.Forms.Button btnRefreshDomains;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Label label3;
    }
}