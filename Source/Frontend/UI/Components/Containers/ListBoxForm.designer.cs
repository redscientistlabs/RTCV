namespace RTCV.UI
{
    partial class ListBoxForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ListBoxForm));
            this.pnTargetComponentForm = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.lbComponentForms = new System.Windows.Forms.ListBox();
            this.pnTargetComponentForm.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnTargetComponentForm
            // 
            this.pnTargetComponentForm.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnTargetComponentForm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pnTargetComponentForm.Controls.Add(this.label8);
            this.pnTargetComponentForm.Location = new System.Drawing.Point(177, 14);
            this.pnTargetComponentForm.Name = "pnTargetComponentForm";
            this.pnTargetComponentForm.Size = new System.Drawing.Size(239, 218);
            this.pnTargetComponentForm.TabIndex = 17;
            this.pnTargetComponentForm.Tag = "color:dark1";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label8.ForeColor = System.Drawing.Color.Gray;
            this.label8.Location = new System.Drawing.Point(13, 15);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(178, 13);
            this.label8.TabIndex = 121;
            this.label8.Tag = "color:normal";
            this.label8.Text = "Component is detached/unavailable";
            // 
            // lbComponentForms
            // 
            this.lbComponentForms.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbComponentForms.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lbComponentForms.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbComponentForms.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lbComponentForms.ForeColor = System.Drawing.Color.White;
            this.lbComponentForms.FormattingEnabled = true;
            this.lbComponentForms.IntegralHeight = false;
            this.lbComponentForms.ItemHeight = 21;
            this.lbComponentForms.Location = new System.Drawing.Point(14, 14);
            this.lbComponentForms.Margin = new System.Windows.Forms.Padding(5);
            this.lbComponentForms.Name = "lbComponentForms";
            this.lbComponentForms.Size = new System.Drawing.Size(154, 218);
            this.lbComponentForms.TabIndex = 16;
            this.lbComponentForms.Tag = "color:dark1";
            this.lbComponentForms.SelectedIndexChanged += new System.EventHandler(this.OnComponentFormsSelectedIndexChanged);
            // 
            // ListBoxForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(428, 235);
            this.Controls.Add(this.pnTargetComponentForm);
            this.Controls.Add(this.lbComponentForms);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ListBoxForm";
            this.Tag = "color:normal";
            this.Text = "Listbox Form";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HandleFormClosing);
            this.Load += new System.EventHandler(this.OnFormLoad);
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
