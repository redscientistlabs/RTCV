namespace RTCV.Launcher
{
    partial class SidebarVersionsPanel
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
            this.lbVersions = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // lbVersions
            // 
            this.lbVersions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbVersions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.lbVersions.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbVersions.Font = new System.Drawing.Font("Segoe UI Light", 12F, System.Drawing.FontStyle.Bold);
            this.lbVersions.ForeColor = System.Drawing.Color.White;
            this.lbVersions.FormattingEnabled = true;
            this.lbVersions.IntegralHeight = false;
            this.lbVersions.ItemHeight = 21;
            this.lbVersions.Location = new System.Drawing.Point(9, 8);
            this.lbVersions.Margin = new System.Windows.Forms.Padding(0);
            this.lbVersions.Name = "lbVersions";
            this.lbVersions.Size = new System.Drawing.Size(137, 492);
            this.lbVersions.TabIndex = 83;
            this.lbVersions.Tag = "";
            this.lbVersions.SelectedIndexChanged += new System.EventHandler(this.lbVersions_SelectedIndexChanged);
            this.lbVersions.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbVersions_MouseDown);
            // 
            // SidebarVersionsPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(155, 509);
            this.Controls.Add(this.lbVersions);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SidebarVersionsPanel";
            this.Text = "VesionSelectPanel";
            this.Load += new System.EventHandler(this.SidebarVersionsPanel_Load);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.ListBox lbVersions;
    }
}
