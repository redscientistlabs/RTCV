namespace RTCV.Launcher
{
    partial class SidebarInfoPanel
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
            this.lbName = new System.Windows.Forms.Label();
            this.lbSubtitle = new System.Windows.Forms.Label();
            this.lbDescription = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbName
            // 
            this.lbName.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbName.ForeColor = System.Drawing.Color.White;
            this.lbName.Location = new System.Drawing.Point(5, 13);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(533, 23);
            this.lbName.TabIndex = 0;
            this.lbName.Text = "_";
            // 
            // lbSubtitle
            // 
            this.lbSubtitle.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lbSubtitle.ForeColor = System.Drawing.Color.White;
            this.lbSubtitle.Location = new System.Drawing.Point(8, 33);
            this.lbSubtitle.Name = "lbSubtitle";
            this.lbSubtitle.Size = new System.Drawing.Size(533, 23);
            this.lbSubtitle.TabIndex = 1;
            this.lbSubtitle.Text = "_";
            // 
            // lbDescription
            // 
            this.lbDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbDescription.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lbDescription.ForeColor = System.Drawing.Color.White;
            this.lbDescription.Location = new System.Drawing.Point(6, 67);
            this.lbDescription.Name = "lbDescription";
            this.lbDescription.Size = new System.Drawing.Size(144, 433);
            this.lbDescription.TabIndex = 2;
            this.lbDescription.Text = "_";
            // 
            // SidebarInfoPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(155, 509);
            this.Controls.Add(this.lbDescription);
            this.Controls.Add(this.lbSubtitle);
            this.Controls.Add(this.lbName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SidebarInfoPanel";
            this.Text = "VesionSelectPanel";
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Label lbName;
        public System.Windows.Forms.Label lbSubtitle;
        public System.Windows.Forms.Label lbDescription;
    }
}