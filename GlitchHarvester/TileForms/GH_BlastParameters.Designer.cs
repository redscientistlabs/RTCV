namespace RTCV.GlitchHarvester.TileForms
{
    partial class GH_BlastParameters
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
            this.nmIntensity = new System.Windows.Forms.NumericUpDown();
            this.labelIntensity = new System.Windows.Forms.Label();
            this.track_Intensity = new System.Windows.Forms.TrackBar();
            this.cbLoadOnSelect = new System.Windows.Forms.CheckBox();
            this.cbStashCorrupted = new System.Windows.Forms.CheckBox();
            this.cbAutoLoadState = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.nmIntensity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.track_Intensity)).BeginInit();
            this.SuspendLayout();
            // 
            // nmIntensity
            // 
            this.nmIntensity.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nmIntensity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nmIntensity.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.nmIntensity.ForeColor = System.Drawing.Color.White;
            this.nmIntensity.Location = new System.Drawing.Point(63, 12);
            this.nmIntensity.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.nmIntensity.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmIntensity.Name = "nmIntensity";
            this.nmIntensity.Size = new System.Drawing.Size(64, 22);
            this.nmIntensity.TabIndex = 148;
            this.nmIntensity.Tag = "color:dark";
            this.nmIntensity.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // labelIntensity
            // 
            this.labelIntensity.AutoSize = true;
            this.labelIntensity.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.labelIntensity.ForeColor = System.Drawing.Color.White;
            this.labelIntensity.Location = new System.Drawing.Point(6, 15);
            this.labelIntensity.Name = "labelIntensity";
            this.labelIntensity.Size = new System.Drawing.Size(57, 13);
            this.labelIntensity.TabIndex = 149;
            this.labelIntensity.Text = "Intensity :";
            // 
            // track_Intensity
            // 
            this.track_Intensity.Location = new System.Drawing.Point(10, 36);
            this.track_Intensity.Maximum = 512000;
            this.track_Intensity.Minimum = 2000;
            this.track_Intensity.Name = "track_Intensity";
            this.track_Intensity.Size = new System.Drawing.Size(119, 45);
            this.track_Intensity.TabIndex = 150;
            this.track_Intensity.TickFrequency = 64000;
            this.track_Intensity.Value = 2000;
            // 
            // cbLoadOnSelect
            // 
            this.cbLoadOnSelect.AutoSize = true;
            this.cbLoadOnSelect.Checked = true;
            this.cbLoadOnSelect.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbLoadOnSelect.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbLoadOnSelect.ForeColor = System.Drawing.Color.White;
            this.cbLoadOnSelect.Location = new System.Drawing.Point(11, 91);
            this.cbLoadOnSelect.Name = "cbLoadOnSelect";
            this.cbLoadOnSelect.Size = new System.Drawing.Size(100, 17);
            this.cbLoadOnSelect.TabIndex = 153;
            this.cbLoadOnSelect.TabStop = false;
            this.cbLoadOnSelect.Text = "Load on select";
            this.cbLoadOnSelect.UseVisualStyleBackColor = true;
            // 
            // cbStashCorrupted
            // 
            this.cbStashCorrupted.AutoSize = true;
            this.cbStashCorrupted.Checked = true;
            this.cbStashCorrupted.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbStashCorrupted.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbStashCorrupted.ForeColor = System.Drawing.Color.White;
            this.cbStashCorrupted.Location = new System.Drawing.Point(11, 106);
            this.cbStashCorrupted.Name = "cbStashCorrupted";
            this.cbStashCorrupted.Size = new System.Drawing.Size(94, 17);
            this.cbStashCorrupted.TabIndex = 151;
            this.cbStashCorrupted.TabStop = false;
            this.cbStashCorrupted.Text = "Stash Results";
            this.cbStashCorrupted.UseVisualStyleBackColor = true;
            // 
            // cbAutoLoadState
            // 
            this.cbAutoLoadState.AutoSize = true;
            this.cbAutoLoadState.Checked = true;
            this.cbAutoLoadState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAutoLoadState.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbAutoLoadState.ForeColor = System.Drawing.Color.White;
            this.cbAutoLoadState.Location = new System.Drawing.Point(11, 76);
            this.cbAutoLoadState.Name = "cbAutoLoadState";
            this.cbAutoLoadState.Size = new System.Drawing.Size(109, 17);
            this.cbAutoLoadState.TabIndex = 152;
            this.cbAutoLoadState.TabStop = false;
            this.cbAutoLoadState.Text = "Auto-Load State";
            this.cbAutoLoadState.UseVisualStyleBackColor = true;
            // 
            // GH_BlastManipulator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(135, 135);
            this.Controls.Add(this.cbLoadOnSelect);
            this.Controls.Add(this.cbStashCorrupted);
            this.Controls.Add(this.cbAutoLoadState);
            this.Controls.Add(this.nmIntensity);
            this.Controls.Add(this.labelIntensity);
            this.Controls.Add(this.track_Intensity);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "GH_BlastManipulator";
            this.Text = "GH_DummyTileForm";
            ((System.ComponentModel.ISupportInitialize)(this.nmIntensity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.track_Intensity)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.NumericUpDown nmIntensity;
        private System.Windows.Forms.Label labelIntensity;
        public System.Windows.Forms.TrackBar track_Intensity;
        public System.Windows.Forms.CheckBox cbLoadOnSelect;
        public System.Windows.Forms.CheckBox cbStashCorrupted;
        public System.Windows.Forms.CheckBox cbAutoLoadState;
    }
}