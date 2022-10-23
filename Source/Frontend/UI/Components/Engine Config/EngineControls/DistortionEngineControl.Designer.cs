namespace RTCV.UI.Components.EngineConfig.EngineControls
{
    partial class DistortionEngineControl
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
            this.label17 = new System.Windows.Forms.Label();
            this.btnResyncDistortionEngine = new System.Windows.Forms.Button();
            this.nmDistortionDelay = new RTCV.UI.Components.Controls.MultiUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.engineGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // engineGroupBox
            // 
            this.engineGroupBox.Controls.Add(this.label17);
            this.engineGroupBox.Controls.Add(this.btnResyncDistortionEngine);
            this.engineGroupBox.Controls.Add(this.nmDistortionDelay);
            this.engineGroupBox.Controls.Add(this.label7);
            this.engineGroupBox.Controls.SetChildIndex(this.placeholderComboBox, 0);
            this.engineGroupBox.Controls.SetChildIndex(this.label7, 0);
            this.engineGroupBox.Controls.SetChildIndex(this.nmDistortionDelay, 0);
            this.engineGroupBox.Controls.SetChildIndex(this.btnResyncDistortionEngine, 0);
            this.engineGroupBox.Controls.SetChildIndex(this.label17, 0);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic);
            this.label17.ForeColor = System.Drawing.Color.White;
            this.label17.Location = new System.Drawing.Point(169, 13);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(189, 13);
            this.label17.TabIndex = 151;
            this.label17.Text = "Backups values and restores them later";
            // 
            // btnResyncDistortionEngine
            // 
            this.btnResyncDistortionEngine.BackColor = System.Drawing.Color.Gray;
            this.btnResyncDistortionEngine.FlatAppearance.BorderSize = 0;
            this.btnResyncDistortionEngine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResyncDistortionEngine.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnResyncDistortionEngine.ForeColor = System.Drawing.Color.White;
            this.btnResyncDistortionEngine.Location = new System.Drawing.Point(6, 117);
            this.btnResyncDistortionEngine.Name = "btnResyncDistortionEngine";
            this.btnResyncDistortionEngine.Size = new System.Drawing.Size(159, 24);
            this.btnResyncDistortionEngine.TabIndex = 150;
            this.btnResyncDistortionEngine.TabStop = false;
            this.btnResyncDistortionEngine.Tag = "color:light1";
            this.btnResyncDistortionEngine.Text = "Resync Distortion Engine";
            this.btnResyncDistortionEngine.UseVisualStyleBackColor = false;
            this.btnResyncDistortionEngine.Click += new System.EventHandler(this.ResyncDistortionEngine);
            // 
            // nmDistortionDelay
            // 
            this.nmDistortionDelay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.nmDistortionDelay.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.nmDistortionDelay.ForeColor = System.Drawing.Color.White;
            this.nmDistortionDelay.Hexadecimal = false;
            this.nmDistortionDelay.Location = new System.Drawing.Point(95, 36);
            this.nmDistortionDelay.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.nmDistortionDelay.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmDistortionDelay.Name = "nmDistortionDelay";
            this.nmDistortionDelay.Size = new System.Drawing.Size(70, 22);
            this.nmDistortionDelay.TabIndex = 149;
            this.nmDistortionDelay.Tag = "color:normal";
            this.nmDistortionDelay.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(4, 39);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(92, 13);
            this.label7.TabIndex = 148;
            this.label7.Text = "Distortion delay:";
            // 
            // DistortionEngineControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "DistortionEngineControl";
            this.engineGroupBox.ResumeLayout(false);
            this.engineGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Button btnResyncDistortionEngine;
        public Controls.MultiUpDown nmDistortionDelay;
        private System.Windows.Forms.Label label7;
    }
}
