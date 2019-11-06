namespace RTCV.Launcher
{
    partial class LaunchPanelV2
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
            this.lbSelectedVersion = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // lbSelectedVersion
            // 
            this.lbSelectedVersion.AutoSize = true;
            this.lbSelectedVersion.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbSelectedVersion.ForeColor = System.Drawing.Color.White;
            this.lbSelectedVersion.Location = new System.Drawing.Point(8, 12);
            this.lbSelectedVersion.Name = "lbSelectedVersion";
            this.lbSelectedVersion.Size = new System.Drawing.Size(117, 19);
            this.lbSelectedVersion.TabIndex = 133;
            this.lbSelectedVersion.Text = "Program Selector";
            this.lbSelectedVersion.Visible = false;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 35);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(8);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(4);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(986, 500);
            this.flowLayoutPanel1.TabIndex = 134;
            // 
            // LaunchPanelV3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(990, 548);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.lbSelectedVersion);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LaunchPanelV2";
            this.Text = "LaunchPanelV2";
            this.Load += new System.EventHandler(this.NewLaunchPanel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label lbSelectedVersion;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}