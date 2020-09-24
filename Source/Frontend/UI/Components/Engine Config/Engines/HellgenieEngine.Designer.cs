namespace RTCV.UI.Components.EngineConfig.Engines
{
    partial class HellgenieEngine
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
            this.label1 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.updownMaxCheats = new RTCV.UI.Components.Controls.MultiUpDown();
            this.label27 = new System.Windows.Forms.Label();
            this.nmMaxValueHellgenie = new RTCV.UI.Components.Controls.MultiUpDown();
            this.label16 = new System.Windows.Forms.Label();
            this.nmMinValueHellgenie = new RTCV.UI.Components.Controls.MultiUpDown();
            this.cbClearCheatsOnRewind = new System.Windows.Forms.CheckBox();
            this.btnClearCheats = new System.Windows.Forms.Button();
            this.engineGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // engineGroupBox
            // 
            this.engineGroupBox.Controls.Add(this.label1);
            this.engineGroupBox.Controls.Add(this.label26);
            this.engineGroupBox.Controls.Add(this.updownMaxCheats);
            this.engineGroupBox.Controls.Add(this.label27);
            this.engineGroupBox.Controls.Add(this.nmMaxValueHellgenie);
            this.engineGroupBox.Controls.Add(this.label16);
            this.engineGroupBox.Controls.Add(this.nmMinValueHellgenie);
            this.engineGroupBox.Controls.Add(this.cbClearCheatsOnRewind);
            this.engineGroupBox.Controls.Add(this.btnClearCheats);
            this.engineGroupBox.Controls.SetChildIndex(this.placeholderComboBox, 0);
            this.engineGroupBox.Controls.SetChildIndex(this.btnClearCheats, 0);
            this.engineGroupBox.Controls.SetChildIndex(this.cbClearCheatsOnRewind, 0);
            this.engineGroupBox.Controls.SetChildIndex(this.nmMinValueHellgenie, 0);
            this.engineGroupBox.Controls.SetChildIndex(this.label16, 0);
            this.engineGroupBox.Controls.SetChildIndex(this.nmMaxValueHellgenie, 0);
            this.engineGroupBox.Controls.SetChildIndex(this.label27, 0);
            this.engineGroupBox.Controls.SetChildIndex(this.updownMaxCheats, 0);
            this.engineGroupBox.Controls.SetChildIndex(this.label26, 0);
            this.engineGroupBox.Controls.SetChildIndex(this.label1, 0);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(6, 86);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 152;
            this.label1.Text = "Max ∞ Units";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label26.ForeColor = System.Drawing.Color.White;
            this.label26.Location = new System.Drawing.Point(6, 61);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(87, 13);
            this.label26.TabIndex = 156;
            this.label26.Text = "Maximum Value";
            // 
            // updownMaxCheats
            // 
            this.updownMaxCheats.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.updownMaxCheats.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.updownMaxCheats.ForeColor = System.Drawing.Color.White;
            this.updownMaxCheats.Hexadecimal = false;
            this.updownMaxCheats.Location = new System.Drawing.Point(97, 84);
            this.updownMaxCheats.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.updownMaxCheats.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.updownMaxCheats.Name = "updownMaxCheats";
            this.updownMaxCheats.Size = new System.Drawing.Size(70, 22);
            this.updownMaxCheats.TabIndex = 151;
            this.updownMaxCheats.Tag = "color:normal";
            this.updownMaxCheats.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label27.ForeColor = System.Drawing.Color.White;
            this.label27.Location = new System.Drawing.Point(6, 36);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(86, 13);
            this.label27.TabIndex = 153;
            this.label27.Text = "Minimum Value";
            // 
            // nmMaxValueHellgenie
            // 
            this.nmMaxValueHellgenie.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.nmMaxValueHellgenie.Font = new System.Drawing.Font("Consolas", 8.25F);
            this.nmMaxValueHellgenie.ForeColor = System.Drawing.Color.White;
            this.nmMaxValueHellgenie.Hexadecimal = true;
            this.nmMaxValueHellgenie.Location = new System.Drawing.Point(97, 59);
            this.nmMaxValueHellgenie.Maximum = new decimal(new int[] {
            -1,
            -1,
            0,
            0});
            this.nmMaxValueHellgenie.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nmMaxValueHellgenie.Name = "nmMaxValueHellgenie";
            this.nmMaxValueHellgenie.Size = new System.Drawing.Size(70, 20);
            this.nmMaxValueHellgenie.TabIndex = 155;
            this.nmMaxValueHellgenie.Tag = "color:normal";
            this.nmMaxValueHellgenie.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic);
            this.label16.ForeColor = System.Drawing.Color.White;
            this.label16.Location = new System.Drawing.Point(171, 10);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(218, 13);
            this.label16.TabIndex = 150;
            this.label16.Text = "Edits values and makes them keep their value";
            // 
            // nmMinValueHellgenie
            // 
            this.nmMinValueHellgenie.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.nmMinValueHellgenie.Font = new System.Drawing.Font("Consolas", 8.25F);
            this.nmMinValueHellgenie.ForeColor = System.Drawing.Color.White;
            this.nmMinValueHellgenie.Hexadecimal = true;
            this.nmMinValueHellgenie.Location = new System.Drawing.Point(97, 34);
            this.nmMinValueHellgenie.Maximum = new decimal(new int[] {
            -1,
            -1,
            0,
            0});
            this.nmMinValueHellgenie.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nmMinValueHellgenie.Name = "nmMinValueHellgenie";
            this.nmMinValueHellgenie.Size = new System.Drawing.Size(70, 20);
            this.nmMinValueHellgenie.TabIndex = 154;
            this.nmMinValueHellgenie.Tag = "color:normal";
            this.nmMinValueHellgenie.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // cbClearCheatsOnRewind
            // 
            this.cbClearCheatsOnRewind.AutoSize = true;
            this.cbClearCheatsOnRewind.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbClearCheatsOnRewind.ForeColor = System.Drawing.Color.White;
            this.cbClearCheatsOnRewind.Location = new System.Drawing.Point(185, 35);
            this.cbClearCheatsOnRewind.Name = "cbClearCheatsOnRewind";
            this.cbClearCheatsOnRewind.Size = new System.Drawing.Size(165, 17);
            this.cbClearCheatsOnRewind.TabIndex = 149;
            this.cbClearCheatsOnRewind.Text = "Clear step units on Rewind";
            this.cbClearCheatsOnRewind.UseVisualStyleBackColor = true;
            // 
            // btnClearCheats
            // 
            this.btnClearCheats.BackColor = System.Drawing.Color.Gray;
            this.btnClearCheats.FlatAppearance.BorderSize = 0;
            this.btnClearCheats.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearCheats.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnClearCheats.ForeColor = System.Drawing.Color.White;
            this.btnClearCheats.Location = new System.Drawing.Point(8, 114);
            this.btnClearCheats.Name = "btnClearCheats";
            this.btnClearCheats.Size = new System.Drawing.Size(159, 24);
            this.btnClearCheats.TabIndex = 148;
            this.btnClearCheats.TabStop = false;
            this.btnClearCheats.Tag = "color:light1";
            this.btnClearCheats.Text = "Clear all cheats";
            this.btnClearCheats.UseVisualStyleBackColor = false;
            // 
            // HellgenieEngine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "HellgenieEngine";
            this.engineGroupBox.ResumeLayout(false);
            this.engineGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label26;
        public Controls.MultiUpDown updownMaxCheats;
        private System.Windows.Forms.Label label27;
        public Controls.MultiUpDown nmMaxValueHellgenie;
        private System.Windows.Forms.Label label16;
        public Controls.MultiUpDown nmMinValueHellgenie;
        public System.Windows.Forms.CheckBox cbClearCheatsOnRewind;
        private System.Windows.Forms.Button btnClearCheats;
    }
}
