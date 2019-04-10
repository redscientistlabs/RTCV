namespace RTCV.UI
{
    partial class RTC_ListBox_Form
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
            this.lbComponentForms = new System.Windows.Forms.ListBox();
            this.pnTargetComponentForm = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.pnTargetComponentForm.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbComponentForms
            // 
            this.lbComponentForms.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbComponentForms.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lbComponentForms.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbComponentForms.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.lbComponentForms.ForeColor = System.Drawing.Color.White;
            this.lbComponentForms.FormattingEnabled = true;
            this.lbComponentForms.IntegralHeight = false;
            this.lbComponentForms.ItemHeight = 25;
            this.lbComponentForms.Location = new System.Drawing.Point(14, 14);
            this.lbComponentForms.Margin = new System.Windows.Forms.Padding(5);
            this.lbComponentForms.Name = "lbComponentForms";
            this.lbComponentForms.Size = new System.Drawing.Size(172, 207);
            this.lbComponentForms.TabIndex = 16;
            this.lbComponentForms.Tag = "color:dark";
            this.lbComponentForms.SelectedIndexChanged += new System.EventHandler(this.lbComponentForms_SelectedIndexChanged);
            // 
            // pnTargetComponentForm
            // 
            this.pnTargetComponentForm.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnTargetComponentForm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pnTargetComponentForm.Controls.Add(this.label8);
            this.pnTargetComponentForm.Location = new System.Drawing.Point(198, 14);
            this.pnTargetComponentForm.Name = "pnTargetComponentForm";
            this.pnTargetComponentForm.Size = new System.Drawing.Size(218, 209);
            this.pnTargetComponentForm.TabIndex = 17;
            this.pnTargetComponentForm.Tag = "color:dark";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label8.ForeColor = System.Drawing.Color.Gray;
            this.label8.Location = new System.Drawing.Point(13, 15);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(170, 13);
            this.label8.TabIndex = 121;
            this.label8.Tag = "color:normal";
            this.label8.Text = "Component is detached to window";
            // 
            // RTC_ListBox_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(428, 235);
            this.Controls.Add(this.pnTargetComponentForm);
            this.Controls.Add(this.lbComponentForms);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "RTC_ListBox_Form";
            this.Tag = "color:normal";
            this.Text = "Listbox Form";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HandleFormClosing);
            this.Load += new System.EventHandler(this.RTC_ListBox_Form_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            this.pnTargetComponentForm.ResumeLayout(false);
            this.pnTargetComponentForm.PerformLayout();
            this.ResumeLayout(false);

        }

		#endregion

		public System.Windows.Forms.ListBox lbComponentForms;
		private System.Windows.Forms.Panel pnTargetComponentForm;
		private System.Windows.Forms.Label label8;
	}
}