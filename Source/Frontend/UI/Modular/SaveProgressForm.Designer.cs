namespace RTCV.UI
{
    partial class SaveProgressForm
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbCurrentAction
            // 
            this.lbCurrentAction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lbCurrentAction.AutoSize = true;
            this.lbCurrentAction.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lbCurrentAction.ForeColor = System.Drawing.Color.White;
            this.lbCurrentAction.Location = new System.Drawing.Point(12, 57);
            this.lbCurrentAction.Name = "lbCurrentAction";
            this.lbCurrentAction.Size = new System.Drawing.Size(99, 19);
            this.lbCurrentAction.TabIndex = 1;
            this.lbCurrentAction.Text = "Current Action";
            // 
            // pbSave
            // 
            this.pbSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.pbSave.Location = new System.Drawing.Point(12, 86);
            this.pbSave.Name = "pbSave";
            this.pbSave.Size = new System.Drawing.Size(446, 55);
            this.pbSave.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.btnCancel.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnCancel.Location = new System.Drawing.Point(379, 165);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(79, 37);
            this.btnCancel.TabIndex = 115;
            this.btnCancel.TabStop = false;
            this.btnCancel.Tag = "color:dark2";
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // SaveProgressForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(470, 214);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lbCurrentAction);
            this.Controls.Add(this.pbSave);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimumSize = new System.Drawing.Size(470, 120);
            this.Name = "SaveProgressForm";
            this.Tag = "color:dark1";
            this.Text = "Save Progress";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HandleFormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar pbSave;
        private System.Windows.Forms.Label lbCurrentAction;
        private System.Windows.Forms.Button btnCancel;
    }
}
