namespace RTCV.UI
{
	partial class RTC_GeneralParameters_Form
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
            this.cbBlastRadius = new System.Windows.Forms.ComboBox();
            this.labelBlastRadius = new System.Windows.Forms.Label();
            this.multiTB_ErrorDelay = new RTCV.UI.Components.Controls.MultiTrackBar();
            this.multiTB_Intensity = new RTCV.UI.Components.Controls.MultiTrackBar();
            this.SuspendLayout();
            // 
            // cbBlastRadius
            // 
            this.cbBlastRadius.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cbBlastRadius.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBlastRadius.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbBlastRadius.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbBlastRadius.ForeColor = System.Drawing.Color.White;
            this.cbBlastRadius.FormattingEnabled = true;
            this.cbBlastRadius.Items.AddRange(new object[] {
            "SPREAD",
            "CHUNK",
            "BURST",
            "EVEN",
            "PROPORTIONAL",
            "NORMALIZED"});
            this.cbBlastRadius.Location = new System.Drawing.Point(91, 134);
            this.cbBlastRadius.Name = "cbBlastRadius";
            this.cbBlastRadius.Size = new System.Drawing.Size(100, 21);
            this.cbBlastRadius.TabIndex = 21;
            this.cbBlastRadius.Tag = "color:dark";
            this.cbBlastRadius.SelectedIndexChanged += new System.EventHandler(this.cbBlastRadius_SelectedIndexChanged);
            this.cbBlastRadius.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // labelBlastRadius
            // 
            this.labelBlastRadius.AutoSize = true;
            this.labelBlastRadius.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.labelBlastRadius.ForeColor = System.Drawing.Color.White;
            this.labelBlastRadius.Location = new System.Drawing.Point(7, 136);
            this.labelBlastRadius.Name = "labelBlastRadius";
            this.labelBlastRadius.Size = new System.Drawing.Size(81, 17);
            this.labelBlastRadius.TabIndex = 20;
            this.labelBlastRadius.Text = "Blast Radius:";
            this.labelBlastRadius.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // multiTB_ErrorDelay
            // 
            this.multiTB_ErrorDelay.BackColor = System.Drawing.Color.Gray;
            this.multiTB_ErrorDelay.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.multiTB_ErrorDelay.LabelText = "Error Delay";
            this.multiTB_ErrorDelay.Location = new System.Drawing.Point(9, 65);
            this.multiTB_ErrorDelay.Maximum = ((long)(65535));
            this.multiTB_ErrorDelay.Minimum = ((long)(1));
            this.multiTB_ErrorDelay.Name = "multiTB_ErrorDelay";
            this.multiTB_ErrorDelay.Size = new System.Drawing.Size(182, 60);
            this.multiTB_ErrorDelay.TabIndex = 23;
            this.multiTB_ErrorDelay.Tag = "color:normal";
            this.multiTB_ErrorDelay.UncapNumericBox = false;
            this.multiTB_ErrorDelay.Value = ((long)(1));
            // 
            // multiTB_Intensity
            // 
            this.multiTB_Intensity.BackColor = System.Drawing.Color.Gray;
            this.multiTB_Intensity.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.multiTB_Intensity.LabelText = "Intensity";
            this.multiTB_Intensity.Location = new System.Drawing.Point(9, 2);
            this.multiTB_Intensity.Maximum = ((long)(65535));
            this.multiTB_Intensity.Minimum = ((long)(1));
            this.multiTB_Intensity.Name = "multiTB_Intensity";
            this.multiTB_Intensity.Size = new System.Drawing.Size(181, 60);
            this.multiTB_Intensity.TabIndex = 22;
            this.multiTB_Intensity.Tag = "color:normal";
            this.multiTB_Intensity.UncapNumericBox = false;
            this.multiTB_Intensity.Value = ((long)(1));
            // 
            // RTC_GeneralParameters_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(200, 167);
            this.Controls.Add(this.multiTB_ErrorDelay);
            this.Controls.Add(this.multiTB_Intensity);
            this.Controls.Add(this.cbBlastRadius);
            this.Controls.Add(this.labelBlastRadius);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "RTC_GeneralParameters_Form";
            this.Tag = "color:normal";
            this.Text = "General Parameters";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HandleFormClosing);
            this.Load += new System.EventHandler(this.RTC_GeneralParameters_Form_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
		public System.Windows.Forms.ComboBox cbBlastRadius;
		public System.Windows.Forms.Label labelBlastRadius;
		public UI.Components.Controls.MultiTrackBar multiTB_Intensity;
		public UI.Components.Controls.MultiTrackBar multiTB_ErrorDelay;
	}
}