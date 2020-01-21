namespace RTCV.Plugins.ScriptHost
{
    partial class ScriptHost
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

        private void InitializeComponent()
        {
            this.tabControl1 = new Manina.Windows.Forms.TabControl();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = -1;
            this.tabControl1.Size = new System.Drawing.Size(708, 421);
            this.tabControl1.TabIndex = 0;
            // 
            // ScriptHost
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(708, 421);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "ScriptHost";
            this.Text = "ScriptHost";
            this.ResumeLayout(false);

        }

        private Manina.Windows.Forms.TabControl tabControl1;
    }
}
