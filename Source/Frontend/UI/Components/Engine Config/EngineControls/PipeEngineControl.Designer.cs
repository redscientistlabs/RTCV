namespace RTCV.UI.Components.EngineConfig.EngineControls
{
    partial class PipeEngineControl
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
            this.label4 = new System.Windows.Forms.Label();
            this.updownMaxPipes = new RTCV.UI.Components.Controls.MultiUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.cbClearPipesOnRewind = new System.Windows.Forms.CheckBox();
            this.cbLockPipes = new System.Windows.Forms.CheckBox();
            this.btnClearPipes = new System.Windows.Forms.Button();
            this.engineGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // engineGroupBox
            // 
            this.engineGroupBox.Controls.Add(this.label4);
            this.engineGroupBox.Controls.Add(this.updownMaxPipes);
            this.engineGroupBox.Controls.Add(this.label14);
            this.engineGroupBox.Controls.Add(this.cbClearPipesOnRewind);
            this.engineGroupBox.Controls.Add(this.cbLockPipes);
            this.engineGroupBox.Controls.Add(this.btnClearPipes);
            this.engineGroupBox.Controls.SetChildIndex(this.placeholderComboBox, 0);
            this.engineGroupBox.Controls.SetChildIndex(this.btnClearPipes, 0);
            this.engineGroupBox.Controls.SetChildIndex(this.cbLockPipes, 0);
            this.engineGroupBox.Controls.SetChildIndex(this.cbClearPipesOnRewind, 0);
            this.engineGroupBox.Controls.SetChildIndex(this.label14, 0);
            this.engineGroupBox.Controls.SetChildIndex(this.updownMaxPipes, 0);
            this.engineGroupBox.Controls.SetChildIndex(this.label4, 0);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(3, 38);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 13);
            this.label4.TabIndex = 153;
            this.label4.Text = "Max ∞ Units";
            // 
            // updownMaxPipes
            // 
            this.updownMaxPipes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.updownMaxPipes.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.updownMaxPipes.ForeColor = System.Drawing.Color.White;
            this.updownMaxPipes.Hexadecimal = false;
            this.updownMaxPipes.Location = new System.Drawing.Point(94, 35);
            this.updownMaxPipes.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.updownMaxPipes.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.updownMaxPipes.Name = "updownMaxPipes";
            this.updownMaxPipes.Size = new System.Drawing.Size(70, 22);
            this.updownMaxPipes.TabIndex = 152;
            this.updownMaxPipes.Tag = "color:normal";
            this.updownMaxPipes.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic);
            this.label14.ForeColor = System.Drawing.Color.White;
            this.label14.Location = new System.Drawing.Point(168, 12);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(201, 13);
            this.label14.TabIndex = 151;
            this.label14.Text = "Copies values from an address to another";
            // 
            // cbClearPipesOnRewind
            // 
            this.cbClearPipesOnRewind.AutoSize = true;
            this.cbClearPipesOnRewind.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbClearPipesOnRewind.ForeColor = System.Drawing.Color.White;
            this.cbClearPipesOnRewind.Location = new System.Drawing.Point(182, 52);
            this.cbClearPipesOnRewind.Name = "cbClearPipesOnRewind";
            this.cbClearPipesOnRewind.Size = new System.Drawing.Size(165, 17);
            this.cbClearPipesOnRewind.TabIndex = 150;
            this.cbClearPipesOnRewind.Text = "Clear step units on Rewind";
            this.cbClearPipesOnRewind.UseVisualStyleBackColor = true;
            // 
            // cbLockPipes
            // 
            this.cbLockPipes.AutoSize = true;
            this.cbLockPipes.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbLockPipes.ForeColor = System.Drawing.Color.White;
            this.cbLockPipes.Location = new System.Drawing.Point(182, 32);
            this.cbLockPipes.Name = "cbLockPipes";
            this.cbLockPipes.Size = new System.Drawing.Size(103, 17);
            this.cbLockPipes.TabIndex = 149;
            this.cbLockPipes.Text = "Lock step units";
            this.cbLockPipes.UseVisualStyleBackColor = true;
            this.cbLockPipes.CheckedChanged += new System.EventHandler(this.OnLockPipesToggle);
            // 
            // btnClearPipes
            // 
            this.btnClearPipes.BackColor = System.Drawing.Color.Gray;
            this.btnClearPipes.FlatAppearance.BorderSize = 0;
            this.btnClearPipes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearPipes.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnClearPipes.ForeColor = System.Drawing.Color.White;
            this.btnClearPipes.Location = new System.Drawing.Point(5, 116);
            this.btnClearPipes.Name = "btnClearPipes";
            this.btnClearPipes.Size = new System.Drawing.Size(159, 24);
            this.btnClearPipes.TabIndex = 148;
            this.btnClearPipes.TabStop = false;
            this.btnClearPipes.Tag = "color:light1";
            this.btnClearPipes.Text = "Clear Pipes";
            this.btnClearPipes.UseVisualStyleBackColor = false;
            this.btnClearPipes.Click += new System.EventHandler(this.ClearPipes);
            // 
            // PipeEngineControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "PipeEngineControl";
            this.engineGroupBox.ResumeLayout(false);
            this.engineGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label4;
        public Controls.MultiUpDown updownMaxPipes;
        private System.Windows.Forms.Label label14;
        public System.Windows.Forms.CheckBox cbClearPipesOnRewind;
        public System.Windows.Forms.CheckBox cbLockPipes;
        private System.Windows.Forms.Button btnClearPipes;
    }
}
