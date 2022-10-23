namespace RTCV.UI
{
    partial class SettingsCorruptForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsCorruptForm));
            this.label3 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.GroupBox();
            this.nmMaxInfiniteStepUnits = new RTCV.UI.Components.Controls.MultiUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.GroupBox();
            this.cbClearStepUnitsOnRewind = new System.Windows.Forms.CheckBox();
            this.cbLockUnits = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.GroupBox();
            this.cbIgnoreUnitOrigin = new System.Windows.Forms.CheckBox();
            this.cbRerollFollowsCustom = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.GroupBox();
            this.cbRerollDomain = new System.Windows.Forms.CheckBox();
            this.cbRerollAddress = new System.Windows.Forms.CheckBox();
            this.cbRerollSourceAddress = new System.Windows.Forms.CheckBox();
            this.cbRerollSourceDomain = new System.Windows.Forms.CheckBox();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(17, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 15);
            this.label3.TabIndex = 146;
            this.label3.Text = "Corruption Settings";
            // 
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel4.BackColor = System.Drawing.Color.Gray;
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Controls.Add(this.panel6);
            this.panel4.Location = new System.Drawing.Point(20, 34);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(276, 137);
            this.panel4.TabIndex = 145;
            this.panel4.Tag = "color:normal";
            // 
            // panel5
            // 
            this.panel5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel5.BackColor = System.Drawing.Color.Transparent;
            this.panel5.Controls.Add(this.nmMaxInfiniteStepUnits);
            this.panel5.Controls.Add(this.label5);
            this.panel5.ForeColor = System.Drawing.Color.White;
            this.panel5.Location = new System.Drawing.Point(14, 11);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(247, 51);
            this.panel5.TabIndex = 152;
            this.panel5.TabStop = false;
            this.panel5.Tag = "";
            this.panel5.Text = "AutoCorrupt Settings";
            // 
            // nmMaxInfiniteStepUnits
            // 
            this.nmMaxInfiniteStepUnits.AutoSize = true;
            this.nmMaxInfiniteStepUnits.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nmMaxInfiniteStepUnits.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.nmMaxInfiniteStepUnits.ForeColor = System.Drawing.Color.White;
            this.nmMaxInfiniteStepUnits.Hexadecimal = false;
            this.nmMaxInfiniteStepUnits.Location = new System.Drawing.Point(111, 17);
            this.nmMaxInfiniteStepUnits.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.nmMaxInfiniteStepUnits.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmMaxInfiniteStepUnits.Name = "nmMaxInfiniteStepUnits";
            this.nmMaxInfiniteStepUnits.Size = new System.Drawing.Size(106, 22);
            this.nmMaxInfiniteStepUnits.TabIndex = 151;
            this.nmMaxInfiniteStepUnits.Tag = "color:dark1";
            this.nmMaxInfiniteStepUnits.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(4, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 13);
            this.label5.TabIndex = 150;
            this.label5.Text = "Max Infinite Units:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // panel6
            // 
            this.panel6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel6.BackColor = System.Drawing.Color.Transparent;
            this.panel6.Controls.Add(this.cbClearStepUnitsOnRewind);
            this.panel6.Controls.Add(this.cbLockUnits);
            this.panel6.ForeColor = System.Drawing.Color.White;
            this.panel6.Location = new System.Drawing.Point(14, 66);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(247, 61);
            this.panel6.TabIndex = 156;
            this.panel6.TabStop = false;
            this.panel6.Tag = "";
            this.panel6.Text = "StepActions Settings";
            // 
            // cbClearStepUnitsOnRewind
            // 
            this.cbClearStepUnitsOnRewind.AutoSize = true;
            this.cbClearStepUnitsOnRewind.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbClearStepUnitsOnRewind.ForeColor = System.Drawing.Color.White;
            this.cbClearStepUnitsOnRewind.Location = new System.Drawing.Point(11, 33);
            this.cbClearStepUnitsOnRewind.Name = "cbClearStepUnitsOnRewind";
            this.cbClearStepUnitsOnRewind.Size = new System.Drawing.Size(167, 17);
            this.cbClearStepUnitsOnRewind.TabIndex = 1;
            this.cbClearStepUnitsOnRewind.Text = "Clear Step Units on Rewind";
            this.cbClearStepUnitsOnRewind.UseVisualStyleBackColor = true;
            this.cbClearStepUnitsOnRewind.CheckedChanged += new System.EventHandler(this.UpdateClearStepUnitsOnRewind);
            // 
            // cbLockUnits
            // 
            this.cbLockUnits.AutoSize = true;
            this.cbLockUnits.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbLockUnits.ForeColor = System.Drawing.Color.White;
            this.cbLockUnits.Location = new System.Drawing.Point(11, 17);
            this.cbLockUnits.Name = "cbLockUnits";
            this.cbLockUnits.Size = new System.Drawing.Size(79, 17);
            this.cbLockUnits.TabIndex = 0;
            this.cbLockUnits.Text = "Lock Units";
            this.cbLockUnits.UseVisualStyleBackColor = true;
            this.cbLockUnits.CheckedChanged += new System.EventHandler(this.UpdateLockUnits);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(17, 184);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 15);
            this.label2.TabIndex = 144;
            this.label2.Text = "Reroll Settings";
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.BackColor = System.Drawing.Color.Gray;
            this.panel3.Controls.Add(this.panel2);
            this.panel3.Controls.Add(this.panel1);
            this.panel3.Location = new System.Drawing.Point(20, 202);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(276, 180);
            this.panel3.TabIndex = 141;
            this.panel3.Tag = "color:normal";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.Controls.Add(this.cbIgnoreUnitOrigin);
            this.panel2.Controls.Add(this.cbRerollFollowsCustom);
            this.panel2.ForeColor = System.Drawing.Color.White;
            this.panel2.Location = new System.Drawing.Point(14, 8);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(247, 53);
            this.panel2.TabIndex = 142;
            this.panel2.TabStop = false;
            this.panel2.Tag = "";
            this.panel2.Text = "Value Settings";
            // 
            // cbIgnoreUnitOrigin
            // 
            this.cbIgnoreUnitOrigin.AutoSize = true;
            this.cbIgnoreUnitOrigin.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbIgnoreUnitOrigin.ForeColor = System.Drawing.Color.White;
            this.cbIgnoreUnitOrigin.Location = new System.Drawing.Point(11, 30);
            this.cbIgnoreUnitOrigin.Name = "cbIgnoreUnitOrigin";
            this.cbIgnoreUnitOrigin.Size = new System.Drawing.Size(154, 17);
            this.cbIgnoreUnitOrigin.TabIndex = 1;
            this.cbIgnoreUnitOrigin.Text = "Ignore Unit Origin Mode";
            this.cbIgnoreUnitOrigin.UseVisualStyleBackColor = true;
            this.cbIgnoreUnitOrigin.CheckedChanged += new System.EventHandler(this.CBRerollIgnoresOriginalSource);
            // 
            // cbRerollFollowsCustom
            // 
            this.cbRerollFollowsCustom.AutoSize = true;
            this.cbRerollFollowsCustom.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbRerollFollowsCustom.ForeColor = System.Drawing.Color.White;
            this.cbRerollFollowsCustom.Location = new System.Drawing.Point(11, 15);
            this.cbRerollFollowsCustom.Name = "cbRerollFollowsCustom";
            this.cbRerollFollowsCustom.Size = new System.Drawing.Size(180, 17);
            this.cbRerollFollowsCustom.TabIndex = 0;
            this.cbRerollFollowsCustom.Text = "Reroll Follows Custom Engine";
            this.cbRerollFollowsCustom.UseVisualStyleBackColor = true;
            this.cbRerollFollowsCustom.CheckedChanged += new System.EventHandler(this.UpdateRerollFollowsCustom);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.cbRerollDomain);
            this.panel1.Controls.Add(this.cbRerollAddress);
            this.panel1.Controls.Add(this.cbRerollSourceAddress);
            this.panel1.Controls.Add(this.cbRerollSourceDomain);
            this.panel1.ForeColor = System.Drawing.Color.White;
            this.panel1.Location = new System.Drawing.Point(14, 67);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(247, 100);
            this.panel1.TabIndex = 140;
            this.panel1.TabStop = false;
            this.panel1.Tag = "";
            this.panel1.Text = "Store Settings";
            // 
            // cbRerollDomain
            // 
            this.cbRerollDomain.AutoSize = true;
            this.cbRerollDomain.Enabled = false;
            this.cbRerollDomain.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbRerollDomain.ForeColor = System.Drawing.Color.White;
            this.cbRerollDomain.Location = new System.Drawing.Point(10, 68);
            this.cbRerollDomain.Name = "cbRerollDomain";
            this.cbRerollDomain.Size = new System.Drawing.Size(99, 17);
            this.cbRerollDomain.TabIndex = 3;
            this.cbRerollDomain.Text = "Reroll Domain";
            this.cbRerollDomain.UseVisualStyleBackColor = true;
            this.cbRerollDomain.CheckedChanged += new System.EventHandler(this.UpdateRerollDomain);
            // 
            // cbRerollAddress
            // 
            this.cbRerollAddress.AutoSize = true;
            this.cbRerollAddress.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbRerollAddress.ForeColor = System.Drawing.Color.White;
            this.cbRerollAddress.Location = new System.Drawing.Point(10, 53);
            this.cbRerollAddress.Name = "cbRerollAddress";
            this.cbRerollAddress.Size = new System.Drawing.Size(100, 17);
            this.cbRerollAddress.TabIndex = 1;
            this.cbRerollAddress.Text = "Reroll Address";
            this.cbRerollAddress.UseVisualStyleBackColor = true;
            this.cbRerollAddress.CheckedChanged += new System.EventHandler(this.UpdateRerollAddress);
            // 
            // cbRerollSourceAddress
            // 
            this.cbRerollSourceAddress.AutoSize = true;
            this.cbRerollSourceAddress.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbRerollSourceAddress.ForeColor = System.Drawing.Color.White;
            this.cbRerollSourceAddress.Location = new System.Drawing.Point(10, 17);
            this.cbRerollSourceAddress.Name = "cbRerollSourceAddress";
            this.cbRerollSourceAddress.Size = new System.Drawing.Size(138, 17);
            this.cbRerollSourceAddress.TabIndex = 0;
            this.cbRerollSourceAddress.Text = "Reroll Source Address";
            this.cbRerollSourceAddress.UseVisualStyleBackColor = true;
            this.cbRerollSourceAddress.CheckedChanged += new System.EventHandler(this.UpdateRerollSourceAddress);
            // 
            // cbRerollSourceDomain
            // 
            this.cbRerollSourceDomain.AutoSize = true;
            this.cbRerollSourceDomain.Enabled = false;
            this.cbRerollSourceDomain.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbRerollSourceDomain.ForeColor = System.Drawing.Color.White;
            this.cbRerollSourceDomain.Location = new System.Drawing.Point(10, 32);
            this.cbRerollSourceDomain.Name = "cbRerollSourceDomain";
            this.cbRerollSourceDomain.Size = new System.Drawing.Size(137, 17);
            this.cbRerollSourceDomain.TabIndex = 2;
            this.cbRerollSourceDomain.Text = "Reroll Source Domain";
            this.cbRerollSourceDomain.UseVisualStyleBackColor = true;
            this.cbRerollSourceDomain.CheckedChanged += new System.EventHandler(this.UpdateRerollSourceDomain);
            // 
            // SettingsCorruptForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.ClientSize = new System.Drawing.Size(320, 394);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel3);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingsCorruptForm";
            this.ShowInTaskbar = false;
            this.Tag = "color:dark1";
            this.Text = "Corruption Settings";
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

		#endregion

		private System.Windows.Forms.GroupBox panel1;
		public System.Windows.Forms.CheckBox cbRerollAddress;
		public System.Windows.Forms.CheckBox cbRerollSourceAddress;
		private System.Windows.Forms.GroupBox panel2;
		public System.Windows.Forms.CheckBox cbIgnoreUnitOrigin;
		public System.Windows.Forms.CheckBox cbRerollFollowsCustom;
		public System.Windows.Forms.CheckBox cbRerollSourceDomain;
		public System.Windows.Forms.CheckBox cbRerollDomain;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Panel panel4;
		public Components.Controls.MultiUpDown nmMaxInfiniteStepUnits;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.GroupBox panel5;
		private System.Windows.Forms.GroupBox panel6;
		public System.Windows.Forms.CheckBox cbClearStepUnitsOnRewind;
		public System.Windows.Forms.CheckBox cbLockUnits;
	}
}
