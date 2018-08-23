namespace RTCV.UI
{
    partial class UI_CoreForm
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
            this.pnTopBar = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.pnTopBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnTopBar
            // 
            this.pnTopBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.pnTopBar.Controls.Add(this.button1);
            this.pnTopBar.Controls.Add(this.button4);
            this.pnTopBar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnTopBar.Location = new System.Drawing.Point(0, 0);
            this.pnTopBar.Name = "pnTopBar";
            this.pnTopBar.Size = new System.Drawing.Size(150, 561);
            this.pnTopBar.TabIndex = 7;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(122, 33);
            this.button1.TabIndex = 10;
            this.button1.Text = "Load Test CanvasGrid";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(12, 58);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(122, 33);
            this.button4.TabIndex = 9;
            this.button4.Text = "Show SubForm Test";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // UI_CoreForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(1009, 561);
            this.Controls.Add(this.pnTopBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(600, 550);
            this.Name = "UI_CoreForm";
            this.Text = "RTC";
            this.Load += new System.EventHandler(this.UI_CoreForm_Load);
            this.ResizeBegin += new System.EventHandler(this.UI_CoreForm_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.UI_CoreForm_ResizeEnd);
            this.Resize += new System.EventHandler(this.UI_CoreForm_Resize);
            this.pnTopBar.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnTopBar;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button4;
    }
}

