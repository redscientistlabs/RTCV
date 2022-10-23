namespace RTCV.UI
{
    partial class SettingsHotkeyConfigForm
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
            this.lbClearBindHint = new System.Windows.Forms.Label();
            this.HotkeyTabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.HotkeyTabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbClearBindHint
            // 
            this.lbClearBindHint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbClearBindHint.AutoSize = true;
            this.lbClearBindHint.ForeColor = System.Drawing.Color.White;
            this.lbClearBindHint.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.lbClearBindHint.Location = new System.Drawing.Point(265, 15);
            this.lbClearBindHint.Name = "lbClearBindHint";
            this.lbClearBindHint.Size = new System.Drawing.Size(137, 13);
            this.lbClearBindHint.TabIndex = 1;
            this.lbClearBindHint.Text = "Press \"Esc\" to clear a bind";
            this.lbClearBindHint.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // HotkeyTabControl
            // 
            this.HotkeyTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.HotkeyTabControl.Controls.Add(this.tabPage1);
            this.HotkeyTabControl.Location = new System.Drawing.Point(12, 11);
            this.HotkeyTabControl.Name = "HotkeyTabControl";
            this.HotkeyTabControl.SelectedIndex = 0;
            this.HotkeyTabControl.Size = new System.Drawing.Size(388, 329);
            this.HotkeyTabControl.TabIndex = 0;
            this.HotkeyTabControl.Tag = "color:normal";
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(380, 303);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Tag = "color:normal";
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // SettingsHotkeyConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.ClientSize = new System.Drawing.Size(412, 352);
            this.Controls.Add(this.lbClearBindHint);
            this.Controls.Add(this.HotkeyTabControl);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SettingsHotkeyConfigForm";
            this.Tag = "color:dark1";
            this.Text = "Hotkey Config";
            this.undockedSizable = false;
            this.Activated += new System.EventHandler(this.OnFormGotFocus);
            this.Deactivate += new System.EventHandler(this.OnFormLostFocus);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HandleFormClosing);
            this.Leave += new System.EventHandler(this.OnFormLostFocus);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            this.HotkeyTabControl.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabControl HotkeyTabControl;
        private System.Windows.Forms.Label lbClearBindHint;
    }
}
