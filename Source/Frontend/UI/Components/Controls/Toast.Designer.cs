namespace RTCV.UI.Components.Controls
{
    partial class Toast
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnEntriesContainer = new System.Windows.Forms.FlowLayoutPanel();
            this.toastEntry1 = new RTCV.UI.Components.Controls.ToastEntry();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbChevron = new System.Windows.Forms.Label();
            this.lbTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnEntriesContainer.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnEntriesContainer
            // 
            this.pnEntriesContainer.AutoSize = true;
            this.pnEntriesContainer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnEntriesContainer.Controls.Add(this.toastEntry1);
            this.pnEntriesContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnEntriesContainer.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnEntriesContainer.Location = new System.Drawing.Point(0, 20);
            this.pnEntriesContainer.Margin = new System.Windows.Forms.Padding(0);
            this.pnEntriesContainer.Name = "pnEntriesContainer";
            this.pnEntriesContainer.Size = new System.Drawing.Size(215, 41);
            this.pnEntriesContainer.TabIndex = 1;
            this.pnEntriesContainer.Tag = "color:dark1";
            // 
            // toastEntry1
            // 
            this.toastEntry1.AutoSize = true;
            this.toastEntry1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.toastEntry1.Location = new System.Drawing.Point(1, 1);
            this.toastEntry1.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.toastEntry1.Name = "toastEntry1";
            this.toastEntry1.Size = new System.Drawing.Size(213, 39);
            this.toastEntry1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.panel2.Controls.Add(this.lbChevron);
            this.panel2.Controls.Add(this.lbTitle);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.MinimumSize = new System.Drawing.Size(191, 20);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(215, 20);
            this.panel2.TabIndex = 0;
            this.panel2.Tag = "color:dark3";
            // 
            // lbChevron
            // 
            this.lbChevron.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbChevron.Font = new System.Drawing.Font("Segoe UI Symbol", 11F);
            this.lbChevron.ForeColor = System.Drawing.Color.White;
            this.lbChevron.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.lbChevron.Location = new System.Drawing.Point(195, -1);
            this.lbChevron.Name = "lbChevron";
            this.lbChevron.Size = new System.Drawing.Size(21, 20);
            this.lbChevron.TabIndex = 119;
            this.lbChevron.Text = "v";
            this.lbChevron.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lbChevron.Click += new System.EventHandler(this.lbChevron_Click);
            // 
            // lbTitle
            // 
            this.lbTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbTitle.AutoSize = true;
            this.lbTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.lbTitle.ForeColor = System.Drawing.Color.White;
            this.lbTitle.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.lbTitle.Location = new System.Drawing.Point(-1, 1);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(102, 15);
            this.lbTitle.TabIndex = 118;
            this.lbTitle.Text = "Background Tasks";
            this.lbTitle.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel1.Controls.Add(this.pnEntriesContainer);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Location = new System.Drawing.Point(1, 1);
            this.panel1.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(215, 61);
            this.panel1.TabIndex = 0;
            this.panel1.Tag = "color:dark1";
            // 
            // Toast
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Gray;
            this.Controls.Add(this.panel1);
            this.Name = "Toast";
            this.Size = new System.Drawing.Size(217, 63);
            this.Tag = "color:light1";
            this.Load += new System.EventHandler(this.Toast_Load);
            this.pnEntriesContainer.ResumeLayout(false);
            this.pnEntriesContainer.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.FlowLayoutPanel pnEntriesContainer;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lbChevron;
        private System.Windows.Forms.Label lbTitle;
        private System.Windows.Forms.Panel panel1;
        private ToastEntry toastEntry1;
    }
}
