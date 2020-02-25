namespace RTCV.UI
{
    partial class RTC_OpenTools_Form
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
            this.flpTools = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // flpTools
            // 
            this.flpTools.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpTools.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpTools.Location = new System.Drawing.Point(0, 0);
            this.flpTools.Name = "flpTools";
            this.flpTools.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.flpTools.Size = new System.Drawing.Size(390, 250);
            this.flpTools.TabIndex = 138;
            // 
            // RTC_OpenTools_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(390, 250);
            this.Controls.Add(this.flpTools);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "RTC_OpenTools_Form";
            this.Tag = "color:dark3";
            this.Text = "Extra Tools Form";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HandleFormClosing);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.FlowLayoutPanel flpTools;
    }
}
