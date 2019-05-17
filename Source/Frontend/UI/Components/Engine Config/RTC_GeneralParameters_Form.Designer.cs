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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RTC_GeneralParameters_Form));
            this.cbBlastRadius = new System.Windows.Forms.ComboBox();
            this.labelBlastRadius = new System.Windows.Forms.Label();
            this.multiTB_ErrorDelay = new RTCV.UI.Components.Controls.MultiTrackBar();
            this.multiTB_Intensity = new RTCV.UI.Components.Controls.MultiTrackBar();
            this.SuspendLayout();
            // 
            // cbBlastRadius
            // 
            this.cbBlastRadius.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbBlastRadius.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
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
            this.cbBlastRadius.Location = new System.Drawing.Point(94, 159);
            this.cbBlastRadius.Name = "cbBlastRadius";
            this.cbBlastRadius.Size = new System.Drawing.Size(83, 21);
            this.cbBlastRadius.TabIndex = 21;
            this.cbBlastRadius.Tag = "color:normal";
            this.cbBlastRadius.SelectedIndexChanged += new System.EventHandler(this.cbBlastRadius_SelectedIndexChanged);
            this.cbBlastRadius.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // labelBlastRadius
            // 
            this.labelBlastRadius.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelBlastRadius.AutoSize = true;
            this.labelBlastRadius.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.labelBlastRadius.ForeColor = System.Drawing.Color.White;
            this.labelBlastRadius.Location = new System.Drawing.Point(14, 161);
            this.labelBlastRadius.Name = "labelBlastRadius";
            this.labelBlastRadius.Size = new System.Drawing.Size(81, 17);
            this.labelBlastRadius.TabIndex = 20;
            this.labelBlastRadius.Text = "Blast Radius:";
            this.labelBlastRadius.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // multiTB_ErrorDelay
            // 
            this.multiTB_ErrorDelay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.multiTB_ErrorDelay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.multiTB_ErrorDelay.DisplayCheckbox = false;
            this.multiTB_ErrorDelay.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.multiTB_ErrorDelay.Label = "Error Delay";
            this.multiTB_ErrorDelay.Location = new System.Drawing.Point(9, 81);
            this.multiTB_ErrorDelay.Maximum = ((long)(65535));
            this.multiTB_ErrorDelay.Minimum = ((long)(1));
            this.multiTB_ErrorDelay.Name = "multiTB_ErrorDelay";
            this.multiTB_ErrorDelay.Size = new System.Drawing.Size(182, 60);
            this.multiTB_ErrorDelay.TabIndex = 23;
            this.multiTB_ErrorDelay.Tag = "color:dark1";
            this.multiTB_ErrorDelay.UncapNumericBox = false;
            this.multiTB_ErrorDelay.Value = ((long)(1));
            // 
            // multiTB_Intensity
            // 
            this.multiTB_Intensity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.multiTB_Intensity.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.multiTB_Intensity.DisplayCheckbox = false;
            this.multiTB_Intensity.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.multiTB_Intensity.Label = "Intensity";
            this.multiTB_Intensity.Location = new System.Drawing.Point(9, 9);
            this.multiTB_Intensity.Maximum = ((long)(65535));
            this.multiTB_Intensity.Minimum = ((long)(1));
            this.multiTB_Intensity.Name = "multiTB_Intensity";
            this.multiTB_Intensity.Size = new System.Drawing.Size(181, 60);
            this.multiTB_Intensity.TabIndex = 22;
            this.multiTB_Intensity.Tag = "color:dark1";
            this.multiTB_Intensity.UncapNumericBox = false;
            this.multiTB_Intensity.Value = ((long)(1));
            // 
            // RTC_GeneralParameters_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(200, 192);
            this.Controls.Add(this.multiTB_ErrorDelay);
            this.Controls.Add(this.multiTB_Intensity);
            this.Controls.Add(this.cbBlastRadius);
            this.Controls.Add(this.labelBlastRadius);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RTC_GeneralParameters_Form";
            this.Tag = "color:dark1";
            this.Text = "General Parameters";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HandleFormClosing);
            this.Load += new System.EventHandler(this.RTC_GeneralParameters_Form_Load);
            this.Shown += new System.EventHandler(this.RTC_GeneralParameters_Form_Shown);
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