namespace RTCV.UI.Components.EngineConfig.EngineControls
{
    partial class NightmareEngine
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
            this.label24 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.nmMaxValueNightmare = new RTCV.UI.Components.Controls.MultiUpDown();
            this.nmMinValueNightmare = new RTCV.UI.Components.Controls.MultiUpDown();
            this.label15 = new System.Windows.Forms.Label();
            this.cbBlastType = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.engineGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // engineGroupBox
            // 
            this.engineGroupBox.Controls.Add(this.label23);
            this.engineGroupBox.Controls.Add(this.label24);
            this.engineGroupBox.Controls.Add(this.nmMaxValueNightmare);
            this.engineGroupBox.Controls.Add(this.nmMinValueNightmare);
            this.engineGroupBox.Controls.Add(this.label15);
            this.engineGroupBox.Controls.Add(this.cbBlastType);
            this.engineGroupBox.Controls.Add(this.label9);
            this.engineGroupBox.Controls.SetChildIndex(this.label9, 0);
            this.engineGroupBox.Controls.SetChildIndex(this.cbBlastType, 0);
            this.engineGroupBox.Controls.SetChildIndex(this.label15, 0);
            this.engineGroupBox.Controls.SetChildIndex(this.nmMinValueNightmare, 0);
            this.engineGroupBox.Controls.SetChildIndex(this.nmMaxValueNightmare, 0);
            this.engineGroupBox.Controls.SetChildIndex(this.label24, 0);
            this.engineGroupBox.Controls.SetChildIndex(this.label23, 0);
            this.engineGroupBox.Controls.SetChildIndex(this.placeholderComboBox, 0);
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label24.ForeColor = System.Drawing.Color.White;
            this.label24.Location = new System.Drawing.Point(5, 88);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(87, 13);
            this.label24.TabIndex = 151;
            this.label24.Text = "Maximum Value";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label23.ForeColor = System.Drawing.Color.White;
            this.label23.Location = new System.Drawing.Point(5, 63);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(86, 13);
            this.label23.TabIndex = 148;
            this.label23.Text = "Minimum Value";
            // 
            // nmMaxValueNightmare
            // 
            this.nmMaxValueNightmare.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.nmMaxValueNightmare.Font = new System.Drawing.Font("Consolas", 8.25F);
            this.nmMaxValueNightmare.ForeColor = System.Drawing.Color.White;
            this.nmMaxValueNightmare.Hexadecimal = true;
            this.nmMaxValueNightmare.Location = new System.Drawing.Point(96, 86);
            this.nmMaxValueNightmare.Maximum = new decimal(new int[] {
            -1,
            -1,
            0,
            0});
            this.nmMaxValueNightmare.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nmMaxValueNightmare.Name = "nmMaxValueNightmare";
            this.nmMaxValueNightmare.Size = new System.Drawing.Size(70, 20);
            this.nmMaxValueNightmare.TabIndex = 150;
            this.nmMaxValueNightmare.Tag = "color:normal";
            this.nmMaxValueNightmare.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            // 
            // nmMinValueNightmare
            // 
            this.nmMinValueNightmare.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.nmMinValueNightmare.Font = new System.Drawing.Font("Consolas", 8.25F);
            this.nmMinValueNightmare.ForeColor = System.Drawing.Color.White;
            this.nmMinValueNightmare.Hexadecimal = true;
            this.nmMinValueNightmare.Location = new System.Drawing.Point(96, 61);
            this.nmMinValueNightmare.Maximum = new decimal(new int[] {
            -1,
            -1,
            0,
            0});
            this.nmMinValueNightmare.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nmMinValueNightmare.Name = "nmMinValueNightmare";
            this.nmMinValueNightmare.Size = new System.Drawing.Size(70, 20);
            this.nmMinValueNightmare.TabIndex = 149;
            this.nmMinValueNightmare.Tag = "color:normal";
            this.nmMinValueNightmare.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic);
            this.label15.ForeColor = System.Drawing.Color.White;
            this.label15.Location = new System.Drawing.Point(170, 13);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(117, 13);
            this.label15.TabIndex = 147;
            this.label15.Text = "Replaces or edits values";
            // 
            // cbBlastType
            // 
            this.cbBlastType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.cbBlastType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBlastType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbBlastType.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbBlastType.ForeColor = System.Drawing.Color.White;
            this.cbBlastType.FormattingEnabled = true;
            this.cbBlastType.Items.AddRange(new object[] {
            "RANDOM",
            "RANDOMTILT",
            "TILT"});
            this.cbBlastType.Location = new System.Drawing.Point(74, 34);
            this.cbBlastType.Name = "cbBlastType";
            this.cbBlastType.Size = new System.Drawing.Size(92, 21);
            this.cbBlastType.TabIndex = 145;
            this.cbBlastType.Tag = "color:normal";
            this.cbBlastType.SelectedIndexChanged += new System.EventHandler(this.UpdateBlastType);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(5, 38);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 13);
            this.label9.TabIndex = 144;
            this.label9.Text = "Blast type:";
            // 
            // NightmareEngine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "NightmareEngine";
            this.engineGroupBox.ResumeLayout(false);
            this.engineGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label23;
        public Controls.MultiUpDown nmMaxValueNightmare;
        public Controls.MultiUpDown nmMinValueNightmare;
        private System.Windows.Forms.Label label15;
        public System.Windows.Forms.ComboBox cbBlastType;
        private System.Windows.Forms.Label label9;
    }
}
