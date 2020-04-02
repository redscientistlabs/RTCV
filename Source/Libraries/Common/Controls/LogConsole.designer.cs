namespace RTCV.Common.Forms
{
    partial class LogConsole
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
            this.tbLog = new System.Windows.Forms.RichTextBox();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.SuspendLayout();
            // 
            // tbLog
            // 
            this.tbLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbLog.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.tbLog.ForeColor = System.Drawing.Color.White;
            this.tbLog.Location = new System.Drawing.Point(0, 0);
            this.tbLog.Name = "tbLog";
            this.tbLog.Size = new System.Drawing.Size(484, 321);
            this.tbLog.TabIndex = 0;
            this.tbLog.Text = "";
            // 
            // LogConsole
            // 
            this.Controls.Add(this.tbLog);
            this.Name = "LogConsole";
            this.Size = new System.Drawing.Size(484, 321);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox tbLog;
        private System.Windows.Forms.ColorDialog colorDialog1;
    }
}
