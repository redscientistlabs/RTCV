namespace RTCV.UI
{
	partial class CorruptionEngineForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CorruptionEngineForm));
            this.pnCustomPrecision = new System.Windows.Forms.Panel();
            this.cbCustomPrecision = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.nmAlignment = new RTCV.UI.Components.Controls.MultiUpDown();
            this.cbSelectedEngine = new System.Windows.Forms.ComboBox();
            this.gbSelectedEngine = new System.Windows.Forms.GroupBox();
            this.pnCustomPrecision.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnCustomPrecision
            // 
            this.pnCustomPrecision.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.pnCustomPrecision.Controls.Add(this.cbCustomPrecision);
            this.pnCustomPrecision.Controls.Add(this.label5);
            this.pnCustomPrecision.Controls.Add(this.label8);
            this.pnCustomPrecision.Controls.Add(this.nmAlignment);
            this.pnCustomPrecision.Location = new System.Drawing.Point(19, 157);
            this.pnCustomPrecision.Name = "pnCustomPrecision";
            this.pnCustomPrecision.Size = new System.Drawing.Size(421, 32);
            this.pnCustomPrecision.TabIndex = 139;
            this.pnCustomPrecision.Tag = "color:dark2";
            this.pnCustomPrecision.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // cbCustomPrecision
            // 
            this.cbCustomPrecision.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.cbCustomPrecision.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCustomPrecision.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbCustomPrecision.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbCustomPrecision.ForeColor = System.Drawing.Color.White;
            this.cbCustomPrecision.FormattingEnabled = true;
            this.cbCustomPrecision.Items.AddRange(new object[] {
            "8-bit",
            "16-bit",
            "32-bit",
            "64-bit"});
            this.cbCustomPrecision.Location = new System.Drawing.Point(294, 5);
            this.cbCustomPrecision.Name = "cbCustomPrecision";
            this.cbCustomPrecision.Size = new System.Drawing.Size(121, 21);
            this.cbCustomPrecision.TabIndex = 81;
            this.cbCustomPrecision.Tag = "color:normal";
            this.cbCustomPrecision.SelectedIndexChanged += new System.EventHandler(this.UpdateCustomPrecision);
            this.cbCustomPrecision.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F);
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(195, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 13);
            this.label5.TabIndex = 82;
            this.label5.Text = "Engine Precision:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F);
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(86, 8);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 13);
            this.label8.TabIndex = 149;
            this.label8.Text = "Alignment:";
            // 
            // nmAlignment
            // 
            this.nmAlignment.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.nmAlignment.Font = new System.Drawing.Font("Segoe UI", 7F);
            this.nmAlignment.ForeColor = System.Drawing.Color.White;
            this.nmAlignment.Hexadecimal = false;
            this.nmAlignment.Location = new System.Drawing.Point(152, 5);
            this.nmAlignment.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.nmAlignment.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nmAlignment.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nmAlignment.Name = "nmAlignment";
            this.nmAlignment.Size = new System.Drawing.Size(37, 21);
            this.nmAlignment.TabIndex = 148;
            this.nmAlignment.Tag = "color:normal";
            this.nmAlignment.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nmAlignment.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // cbSelectedEngine
            // 
            this.cbSelectedEngine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.cbSelectedEngine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSelectedEngine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbSelectedEngine.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbSelectedEngine.ForeColor = System.Drawing.Color.White;
            this.cbSelectedEngine.FormattingEnabled = true;
            this.cbSelectedEngine.Items.AddRange(new object[] {
            "Nightmare Engine",
            "Hellgenie Engine",
            "Distortion Engine",
            "Freeze Engine",
            "Pipe Engine",
            "Vector Engine",
            "Cluster Engine",
            "Custom Engine",
            "Blast Generator"});
            this.cbSelectedEngine.Location = new System.Drawing.Point(19, 16);
            this.cbSelectedEngine.Name = "cbSelectedEngine";
            this.cbSelectedEngine.Size = new System.Drawing.Size(165, 21);
            this.cbSelectedEngine.TabIndex = 138;
            this.cbSelectedEngine.Tag = "color:normal";
            this.cbSelectedEngine.SelectedIndexChanged += new System.EventHandler(this.UpdateEngine);
            this.cbSelectedEngine.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // gbSelectedEngine
            // 
            this.gbSelectedEngine.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.gbSelectedEngine.ForeColor = System.Drawing.Color.White;
            this.gbSelectedEngine.Location = new System.Drawing.Point(19, 7);
            this.gbSelectedEngine.Name = "gbSelectedEngine";
            this.gbSelectedEngine.Size = new System.Drawing.Size(420, 151);
            this.gbSelectedEngine.TabIndex = 137;
            this.gbSelectedEngine.TabStop = false;
            this.gbSelectedEngine.Visible = false;
            this.gbSelectedEngine.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // CorruptionEngineForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(1448, 1097);
            this.Controls.Add(this.pnCustomPrecision);
            this.Controls.Add(this.cbSelectedEngine);
            this.Controls.Add(this.gbSelectedEngine);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CorruptionEngineForm";
            this.Tag = "color:dark1";
            this.Text = "Corruption Engine";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HandleFormClosing);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            this.pnCustomPrecision.ResumeLayout(false);
            this.pnCustomPrecision.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel pnCustomPrecision;
		public System.Windows.Forms.ComboBox cbCustomPrecision;
		public System.Windows.Forms.ComboBox cbSelectedEngine;
		internal System.Windows.Forms.GroupBox gbSelectedEngine;
		private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
        public Components.Controls.MultiUpDown nmAlignment;
    }
}
