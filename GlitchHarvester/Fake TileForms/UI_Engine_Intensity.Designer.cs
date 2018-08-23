namespace RTCV.UI.TileForms
{
    partial class UI_Engine_Intensity
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.nmIntensity = new System.Windows.Forms.NumericUpDown();
            this.labelIntensityTimes = new System.Windows.Forms.Label();
            this.labelIntensity = new System.Windows.Forms.Label();
            this.track_Intensity = new System.Windows.Forms.TrackBar();
            this.nmErrorDelay = new System.Windows.Forms.NumericUpDown();
            this.labelErrorDelay = new System.Windows.Forms.Label();
            this.labelErrorDelaySteps = new System.Windows.Forms.Label();
            this.track_ErrorDelay = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.nmIntensity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.track_Intensity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmErrorDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.track_ErrorDelay)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel1.Location = new System.Drawing.Point(0, 55);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(280, 10);
            this.panel1.TabIndex = 0;
            // 
            // nmIntensity
            // 
            this.nmIntensity.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nmIntensity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nmIntensity.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.nmIntensity.ForeColor = System.Drawing.Color.White;
            this.nmIntensity.Location = new System.Drawing.Point(103, 6);
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
            this.nmIntensity.Size = new System.Drawing.Size(60, 22);
            this.nmIntensity.TabIndex = 15;
            this.nmIntensity.Tag = "color:dark";
            this.nmIntensity.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // labelIntensityTimes
            // 
            this.labelIntensityTimes.AutoSize = true;
            this.labelIntensityTimes.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.labelIntensityTimes.ForeColor = System.Drawing.Color.White;
            this.labelIntensityTimes.Location = new System.Drawing.Point(166, 6);
            this.labelIntensityTimes.Name = "labelIntensityTimes";
            this.labelIntensityTimes.Size = new System.Drawing.Size(42, 19);
            this.labelIntensityTimes.TabIndex = 17;
            this.labelIntensityTimes.Text = "times";
            // 
            // labelIntensity
            // 
            this.labelIntensity.AutoSize = true;
            this.labelIntensity.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.labelIntensity.ForeColor = System.Drawing.Color.White;
            this.labelIntensity.Location = new System.Drawing.Point(11, 5);
            this.labelIntensity.Name = "labelIntensity";
            this.labelIntensity.Size = new System.Drawing.Size(69, 19);
            this.labelIntensity.TabIndex = 16;
            this.labelIntensity.Text = "Intensity :";
            // 
            // track_Intensity
            // 
            this.track_Intensity.Location = new System.Drawing.Point(2, 24);
            this.track_Intensity.Maximum = 512000;
            this.track_Intensity.Minimum = 2000;
            this.track_Intensity.Name = "track_Intensity";
            this.track_Intensity.Size = new System.Drawing.Size(233, 45);
            this.track_Intensity.TabIndex = 18;
            this.track_Intensity.TickFrequency = 32000;
            this.track_Intensity.Value = 2000;
            // 
            // nmErrorDelay
            // 
            this.nmErrorDelay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nmErrorDelay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nmErrorDelay.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.nmErrorDelay.ForeColor = System.Drawing.Color.White;
            this.nmErrorDelay.Location = new System.Drawing.Point(101, 66);
            this.nmErrorDelay.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.nmErrorDelay.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmErrorDelay.Name = "nmErrorDelay";
            this.nmErrorDelay.Size = new System.Drawing.Size(60, 22);
            this.nmErrorDelay.TabIndex = 20;
            this.nmErrorDelay.Tag = "color:dark";
            this.nmErrorDelay.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // labelErrorDelay
            // 
            this.labelErrorDelay.AutoSize = true;
            this.labelErrorDelay.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.labelErrorDelay.ForeColor = System.Drawing.Color.White;
            this.labelErrorDelay.Location = new System.Drawing.Point(9, 66);
            this.labelErrorDelay.Name = "labelErrorDelay";
            this.labelErrorDelay.Size = new System.Drawing.Size(82, 19);
            this.labelErrorDelay.TabIndex = 19;
            this.labelErrorDelay.Text = "Error delay :";
            // 
            // labelErrorDelaySteps
            // 
            this.labelErrorDelaySteps.AutoSize = true;
            this.labelErrorDelaySteps.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.labelErrorDelaySteps.ForeColor = System.Drawing.Color.White;
            this.labelErrorDelaySteps.Location = new System.Drawing.Point(164, 66);
            this.labelErrorDelaySteps.Name = "labelErrorDelaySteps";
            this.labelErrorDelaySteps.Size = new System.Drawing.Size(41, 19);
            this.labelErrorDelaySteps.TabIndex = 21;
            this.labelErrorDelaySteps.Text = "steps";
            // 
            // track_ErrorDelay
            // 
            this.track_ErrorDelay.Location = new System.Drawing.Point(0, 84);
            this.track_ErrorDelay.Maximum = 512000;
            this.track_ErrorDelay.Minimum = 2000;
            this.track_ErrorDelay.Name = "track_ErrorDelay";
            this.track_ErrorDelay.Size = new System.Drawing.Size(233, 45);
            this.track_ErrorDelay.TabIndex = 22;
            this.track_ErrorDelay.TickFrequency = 32000;
            this.track_ErrorDelay.Value = 2000;
            // 
            // UI_Engine_Intensity
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(245, 115);
            this.Controls.Add(this.nmErrorDelay);
            this.Controls.Add(this.labelErrorDelay);
            this.Controls.Add(this.labelErrorDelaySteps);
            this.Controls.Add(this.track_ErrorDelay);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.nmIntensity);
            this.Controls.Add(this.labelIntensityTimes);
            this.Controls.Add(this.labelIntensity);
            this.Controls.Add(this.track_Intensity);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "UI_Engine_Intensity";
            this.Tag = "color:normal";
            this.Text = "UI_DummyTileForm";
            ((System.ComponentModel.ISupportInitialize)(this.nmIntensity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.track_Intensity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmErrorDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.track_ErrorDelay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.NumericUpDown nmIntensity;
        private System.Windows.Forms.Label labelIntensityTimes;
        private System.Windows.Forms.Label labelIntensity;
        public System.Windows.Forms.TrackBar track_Intensity;
        public System.Windows.Forms.NumericUpDown nmErrorDelay;
        private System.Windows.Forms.Label labelErrorDelay;
        private System.Windows.Forms.Label labelErrorDelaySteps;
        public System.Windows.Forms.TrackBar track_ErrorDelay;
    }
}