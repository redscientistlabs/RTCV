namespace RTC_Launcher
{
    partial class NewLaunchPanel
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
            this.btnDefaultSize = new System.Windows.Forms.Button();
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
            // btnDefaultSize
            // 
            this.btnDefaultSize.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btnDefaultSize.FlatAppearance.BorderSize = 0;
            this.btnDefaultSize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDefaultSize.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.btnDefaultSize.ForeColor = System.Drawing.Color.Black;
            this.btnDefaultSize.Location = new System.Drawing.Point(12, 33);
            this.btnDefaultSize.Name = "btnDefaultSize";
            this.btnDefaultSize.Size = new System.Drawing.Size(92, 65);
            this.btnDefaultSize.TabIndex = 134;
            this.btnDefaultSize.TabStop = false;
            this.btnDefaultSize.Tag = "color:light";
            this.btnDefaultSize.Text = "...";
            this.btnDefaultSize.UseVisualStyleBackColor = false;
            this.btnDefaultSize.Visible = false;
            // 
            // NewLaunchPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(990, 548);
            this.Controls.Add(this.btnDefaultSize);
            this.Controls.Add(this.lbSelectedVersion);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "NewLaunchPanel";
            this.Text = "NewLaunchPanel";
            this.Load += new System.EventHandler(this.NewLaunchPanel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label lbSelectedVersion;
        private System.Windows.Forms.Button btnDefaultSize;
    }
}