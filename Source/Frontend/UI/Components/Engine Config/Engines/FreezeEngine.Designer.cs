namespace RTCV.UI.Components.Engine_Config.Engines
{
    partial class FreezeEngine
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.comboBox4 = new System.Windows.Forms.ComboBox();
            this.cbClearFreezesOnRewind = new System.Windows.Forms.CheckBox();
            this.btnClearAllFreezes = new System.Windows.Forms.Button();
            this.updownMaxFreeze = new RTCV.UI.Components.Controls.MultiUpDown();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.updownMaxFreeze);
            this.groupBox1.Controls.Add(this.label20);
            this.groupBox1.Controls.Add(this.comboBox4);
            this.groupBox1.Controls.Add(this.cbClearFreezesOnRewind);
            this.groupBox1.Controls.Add(this.btnClearAllFreezes);
            this.groupBox1.Location = new System.Drawing.Point(0, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(420, 148);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Tag = "color:dark1";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(5, 39);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 13);
            this.label6.TabIndex = 149;
            this.label6.Text = "Max ∞ Units";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic);
            this.label20.ForeColor = System.Drawing.Color.White;
            this.label20.Location = new System.Drawing.Point(170, 13);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(112, 13);
            this.label20.TabIndex = 147;
            this.label20.Text = "Freezes values in place";
            // 
            // comboBox4
            // 
            this.comboBox4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.comboBox4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox4.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.comboBox4.ForeColor = System.Drawing.Color.White;
            this.comboBox4.FormattingEnabled = true;
            this.comboBox4.Location = new System.Drawing.Point(0, 6);
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new System.Drawing.Size(165, 21);
            this.comboBox4.TabIndex = 146;
            this.comboBox4.Tag = "color:normal";
            this.comboBox4.Visible = false;
            // 
            // cbClearFreezesOnRewind
            // 
            this.cbClearFreezesOnRewind.AutoSize = true;
            this.cbClearFreezesOnRewind.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbClearFreezesOnRewind.ForeColor = System.Drawing.Color.White;
            this.cbClearFreezesOnRewind.Location = new System.Drawing.Point(184, 37);
            this.cbClearFreezesOnRewind.Name = "cbClearFreezesOnRewind";
            this.cbClearFreezesOnRewind.Size = new System.Drawing.Size(165, 17);
            this.cbClearFreezesOnRewind.TabIndex = 145;
            this.cbClearFreezesOnRewind.Text = "Clear step units on Rewind";
            this.cbClearFreezesOnRewind.UseVisualStyleBackColor = true;
            // 
            // btnClearAllFreezes
            // 
            this.btnClearAllFreezes.BackColor = System.Drawing.Color.Gray;
            this.btnClearAllFreezes.FlatAppearance.BorderSize = 0;
            this.btnClearAllFreezes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearAllFreezes.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnClearAllFreezes.ForeColor = System.Drawing.Color.White;
            this.btnClearAllFreezes.Location = new System.Drawing.Point(7, 117);
            this.btnClearAllFreezes.Name = "btnClearAllFreezes";
            this.btnClearAllFreezes.Size = new System.Drawing.Size(159, 24);
            this.btnClearAllFreezes.TabIndex = 144;
            this.btnClearAllFreezes.TabStop = false;
            this.btnClearAllFreezes.Tag = "color:light1";
            this.btnClearAllFreezes.Text = "Clear all freezes";
            this.btnClearAllFreezes.UseVisualStyleBackColor = false;
            // 
            // updownMaxFreeze
            // 
            this.updownMaxFreeze.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.updownMaxFreeze.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.updownMaxFreeze.ForeColor = System.Drawing.Color.White;
            this.updownMaxFreeze.Hexadecimal = false;
            this.updownMaxFreeze.Location = new System.Drawing.Point(96, 36);
            this.updownMaxFreeze.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.updownMaxFreeze.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.updownMaxFreeze.Name = "updownMaxFreeze";
            this.updownMaxFreeze.Size = new System.Drawing.Size(70, 22);
            this.updownMaxFreeze.TabIndex = 148;
            this.updownMaxFreeze.Tag = "color:normal";
            this.updownMaxFreeze.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // FreezeEngine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "FreezeEngine";
            this.Size = new System.Drawing.Size(420, 151);
            this.Tag = "color:dark1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label6;
        public Controls.MultiUpDown updownMaxFreeze;
        private System.Windows.Forms.Label label20;
        public System.Windows.Forms.ComboBox comboBox4;
        public System.Windows.Forms.CheckBox cbClearFreezesOnRewind;
        private System.Windows.Forms.Button btnClearAllFreezes;
    }
}
