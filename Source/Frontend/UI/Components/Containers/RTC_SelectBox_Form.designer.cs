namespace RTCV.UI
{
    partial class RTC_SelectBox_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RTC_SelectBox_Form));
            this.pnComponentForm = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.cbSelectBox = new System.Windows.Forms.ComboBox();
            this.pnComponentForm.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnComponentForm
            // 
            this.pnComponentForm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnComponentForm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.pnComponentForm.Controls.Add(this.label8);
            this.pnComponentForm.Location = new System.Drawing.Point(11, 31);
            this.pnComponentForm.Name = "pnComponentForm";
            this.pnComponentForm.Size = new System.Drawing.Size(390, 250);
            this.pnComponentForm.TabIndex = 117;
            this.pnComponentForm.Tag = "color:normal";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label8.ForeColor = System.Drawing.Color.Silver;
            this.label8.Location = new System.Drawing.Point(8, 8);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(178, 13);
            this.label8.TabIndex = 120;
            this.label8.Tag = "color:normal";
            this.label8.Text = "Component is detached/unavailable";
            // 
            // cbSelectBox
            // 
            this.cbSelectBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbSelectBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.cbSelectBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSelectBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbSelectBox.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbSelectBox.ForeColor = System.Drawing.Color.White;
            this.cbSelectBox.FormattingEnabled = true;
            this.cbSelectBox.Location = new System.Drawing.Point(11, 10);
            this.cbSelectBox.Name = "cbSelectBox";
            this.cbSelectBox.Size = new System.Drawing.Size(390, 21);
            this.cbSelectBox.TabIndex = 116;
            this.cbSelectBox.TabStop = false;
            this.cbSelectBox.Tag = "color:normal";
            this.cbSelectBox.SelectedIndexChanged += new System.EventHandler(this.cbSelectBox_SelectedIndexChanged);
            // 
            // RTC_SelectBox_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(413, 291);
            this.Controls.Add(this.pnComponentForm);
            this.Controls.Add(this.cbSelectBox);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RTC_SelectBox_Form";
            this.Tag = "color:dark1";
            this.Text = "SelectBox Component";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HandleFormClosing);
            this.Load += new System.EventHandler(this.RTC_SelectBox_Form_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            this.Resize += new System.EventHandler(this.RTC_SelectBox_Form_Resize);
            this.pnComponentForm.ResumeLayout(false);
            this.pnComponentForm.PerformLayout();
            this.ResumeLayout(false);

        }

		#endregion

		private System.Windows.Forms.Panel pnComponentForm;
		private System.Windows.Forms.Label label8;
		public System.Windows.Forms.ComboBox cbSelectBox;
	}
}