namespace RTCV.UI
{
    partial class UI_SaveProgress_Form
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
            this.lbCurrentAction = new System.Windows.Forms.Label();
            this.pbSave = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // lbCurrentAction
            // 
            this.lbCurrentAction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lbCurrentAction.AutoSize = true;
            this.lbCurrentAction.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lbCurrentAction.ForeColor = System.Drawing.Color.White;
            this.lbCurrentAction.Location = new System.Drawing.Point(12, 10);
            this.lbCurrentAction.Name = "lbCurrentAction";
            this.lbCurrentAction.Size = new System.Drawing.Size(99, 19);
            this.lbCurrentAction.TabIndex = 1;
            this.lbCurrentAction.Text = "Current Action";
            // 
            // pbSave
            // 
            this.pbSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.pbSave.Location = new System.Drawing.Point(12, 39);
            this.pbSave.Name = "pbSave";
            this.pbSave.Size = new System.Drawing.Size(446, 55);
            this.pbSave.TabIndex = 0;
            // 
            // UI_SaveProgress_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(470, 120);
            this.Controls.Add(this.lbCurrentAction);
            this.Controls.Add(this.pbSave);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimumSize = new System.Drawing.Size(470, 120);
            this.Name = "UI_SaveProgress_Form";
            this.Tag = "color:dark1";
            this.Text = "Save Progress";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar pbSave;
        private System.Windows.Forms.Label lbCurrentAction;
    }
}