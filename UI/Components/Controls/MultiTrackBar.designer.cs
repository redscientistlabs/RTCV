namespace RTCV.UI.Components.Controls
{
    partial class MultiTrackBar
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbControlName = new System.Windows.Forms.Label();
            this.nmControlValue = new System.Windows.Forms.NumericUpDown();
            this.tbControlValue = new RTCV.UI.Components.Controls.NoFocusTrackBar();
            this.cbControlName = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.nmControlValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbControlValue)).BeginInit();
            this.SuspendLayout();
            // 
            // lbControlName
            // 
            this.lbControlName.AutoSize = true;
            this.lbControlName.Font = new System.Drawing.Font("Segoe UI", 9.25F);
            this.lbControlName.ForeColor = System.Drawing.Color.White;
            this.lbControlName.Location = new System.Drawing.Point(5, 8);
            this.lbControlName.Name = "lbControlName";
            this.lbControlName.Size = new System.Drawing.Size(43, 17);
            this.lbControlName.TabIndex = 0;
            this.lbControlName.Text = "Name";
            // 
            // nmControlValue
            // 
            this.nmControlValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nmControlValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nmControlValue.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.nmControlValue.ForeColor = System.Drawing.Color.White;
            this.nmControlValue.Location = new System.Drawing.Point(148, 7);
            this.nmControlValue.Maximum = new decimal(new int[] {
            -1,
            2147483647,
            0,
            0});
            this.nmControlValue.Name = "nmControlValue";
            this.nmControlValue.Size = new System.Drawing.Size(72, 22);
            this.nmControlValue.TabIndex = 2;
            this.nmControlValue.Tag = "color:dark";
            this.nmControlValue.ValueChanged += new System.EventHandler(this.nmControlValue_ValueChanged);
            // 
            // tbControlValue
            // 
            this.tbControlValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbControlValue.Location = new System.Drawing.Point(0, 29);
            this.tbControlValue.Maximum = 65536;
            this.tbControlValue.Name = "tbControlValue";
            this.tbControlValue.Size = new System.Drawing.Size(227, 45);
            this.tbControlValue.TabIndex = 3;
            this.tbControlValue.TabStop = false;
            this.tbControlValue.TickFrequency = 6553;
            this.tbControlValue.ValueChanged += new System.EventHandler(this.tbControlValue_ValueChanged);
            // 
            // cbControlName
            // 
            this.cbControlName.AutoSize = true;
            this.cbControlName.Font = new System.Drawing.Font("Segoe UI", 9.25F);
            this.cbControlName.ForeColor = System.Drawing.Color.White;
            this.cbControlName.Location = new System.Drawing.Point(8, 7);
            this.cbControlName.Name = "cbControlName";
            this.cbControlName.Size = new System.Drawing.Size(62, 21);
            this.cbControlName.TabIndex = 4;
            this.cbControlName.Text = "Name";
            this.cbControlName.UseVisualStyleBackColor = true;
            this.cbControlName.Visible = false;
            this.cbControlName.CheckedChanged += new System.EventHandler(this.cbControlName_CheckedChanged);
            // 
            // MultiTrackBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(94)))), ((int)(((byte)(94)))));
            this.Controls.Add(this.cbControlName);
            this.Controls.Add(this.nmControlValue);
            this.Controls.Add(this.lbControlName);
            this.Controls.Add(this.tbControlValue);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.Name = "MultiTrackBar";
            this.Size = new System.Drawing.Size(227, 60);
            this.Tag = "color:normal";
            this.Load += new System.EventHandler(this.MultiTrackBar_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nmControlValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbControlValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbControlName;
        private System.Windows.Forms.NumericUpDown nmControlValue;
        private NoFocusTrackBar tbControlValue;
		private System.Windows.Forms.CheckBox cbControlName;
	}
}
