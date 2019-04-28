namespace RTCV.UI
{
    partial class RTC_SettingsHotkeyConfig_Form
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
            this.components = new System.ComponentModel.Container();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.HotkeyTabControl = new System.Windows.Forms.TabControl();
            this.HotkeyTabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(380, 302);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Tag = "color:normal";
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // HotkeyTabControl
            // 
            this.HotkeyTabControl.Controls.Add(this.tabPage1);
            this.HotkeyTabControl.Location = new System.Drawing.Point(12, 12);
            this.HotkeyTabControl.Name = "HotkeyTabControl";
            this.HotkeyTabControl.SelectedIndex = 0;
            this.HotkeyTabControl.Size = new System.Drawing.Size(388, 328);
            this.HotkeyTabControl.TabIndex = 0;
            this.HotkeyTabControl.Tag = "color:normal";
            // 
            // RTC_SettingsHotkeyConfig_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(412, 352);
            this.Controls.Add(this.HotkeyTabControl);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "RTC_SettingsHotkeyConfig_Form";
            this.Tag = "color:dark2";
            this.Text = "Hotkey Config";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HandleFormClosing);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            this.HotkeyTabControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabControl HotkeyTabControl;
    }
}