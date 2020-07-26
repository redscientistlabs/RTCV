using System.Windows.Forms;
using RTCV.UI;

namespace RTCV.UI
{
	partial class RTC_AnalyticsTool_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RTC_AnalyticsTool_Form));
            this.btnSelectAllDumps = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.btnSelectNoDumps = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.lbDomainSize = new System.Windows.Forms.Label();
            this.lbDumpsSelected = new System.Windows.Forms.Label();
            this.pbActivity = new System.Windows.Forms.PictureBox();
            this.btnComputeActivity = new System.Windows.Forms.Button();
            this.cbWordSize = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lbDumps = new RTCV.UI.Components.Controls.ListBoxExtended();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbActivity)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSelectAllDumps
            // 
            this.btnSelectAllDumps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSelectAllDumps.BackColor = System.Drawing.Color.Gray;
            this.btnSelectAllDumps.FlatAppearance.BorderSize = 0;
            this.btnSelectAllDumps.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectAllDumps.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnSelectAllDumps.ForeColor = System.Drawing.Color.White;
            this.btnSelectAllDumps.Location = new System.Drawing.Point(14, 429);
            this.btnSelectAllDumps.Name = "btnSelectAllDumps";
            this.btnSelectAllDumps.Size = new System.Drawing.Size(134, 24);
            this.btnSelectAllDumps.TabIndex = 192;
            this.btnSelectAllDumps.TabStop = false;
            this.btnSelectAllDumps.Tag = "color:light1";
            this.btnSelectAllDumps.Text = "Select All";
            this.btnSelectAllDumps.UseVisualStyleBackColor = false;
            this.btnSelectAllDumps.Click += new System.EventHandler(this.btnSelectAllDumps_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(87, 13);
            this.label6.TabIndex = 193;
            this.label6.Text = "Memory Dumps";
            // 
            // btnSelectNoDumps
            // 
            this.btnSelectNoDumps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSelectNoDumps.BackColor = System.Drawing.Color.Gray;
            this.btnSelectNoDumps.FlatAppearance.BorderSize = 0;
            this.btnSelectNoDumps.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectNoDumps.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnSelectNoDumps.ForeColor = System.Drawing.Color.White;
            this.btnSelectNoDumps.Location = new System.Drawing.Point(14, 459);
            this.btnSelectNoDumps.Name = "btnSelectNoDumps";
            this.btnSelectNoDumps.Size = new System.Drawing.Size(134, 24);
            this.btnSelectNoDumps.TabIndex = 198;
            this.btnSelectNoDumps.TabStop = false;
            this.btnSelectNoDumps.Tag = "color:light1";
            this.btnSelectNoDumps.Text = "Select None";
            this.btnSelectNoDumps.UseVisualStyleBackColor = false;
            this.btnSelectNoDumps.Click += new System.EventHandler(this.btnSelectNoDumps_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.button2.ForeColor = System.Drawing.Color.OrangeRed;
            this.button2.Location = new System.Drawing.Point(168, 459);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(138, 24);
            this.button2.TabIndex = 199;
            this.button2.TabStop = false;
            this.button2.Tag = "color:dark2";
            this.button2.Text = "Generate VMD";
            this.button2.UseVisualStyleBackColor = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(169, 440);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(137, 13);
            this.label4.TabIndex = 195;
            this.label4.Text = "Export Filtered Addresses";
            // 
            // lbDomainSize
            // 
            this.lbDomainSize.Font = new System.Drawing.Font("Segoe UI Symbol", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lbDomainSize.Location = new System.Drawing.Point(165, 9);
            this.lbDomainSize.Name = "lbDomainSize";
            this.lbDomainSize.Size = new System.Drawing.Size(137, 19);
            this.lbDomainSize.TabIndex = 203;
            this.lbDomainSize.Text = "Domain size:";
            this.lbDomainSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbDumpsSelected
            // 
            this.lbDumpsSelected.Font = new System.Drawing.Font("Segoe UI Symbol", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lbDumpsSelected.Location = new System.Drawing.Point(303, 9);
            this.lbDumpsSelected.Name = "lbDumpsSelected";
            this.lbDumpsSelected.Size = new System.Drawing.Size(130, 19);
            this.lbDumpsSelected.TabIndex = 204;
            this.lbDumpsSelected.Text = "Dumps selected:";
            this.lbDumpsSelected.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pbActivity
            // 
            this.pbActivity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbActivity.Location = new System.Drawing.Point(168, 173);
            this.pbActivity.Name = "pbActivity";
            this.pbActivity.Size = new System.Drawing.Size(486, 104);
            this.pbActivity.TabIndex = 202;
            this.pbActivity.TabStop = false;
            // 
            // btnComputeActivity
            // 
            this.btnComputeActivity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnComputeActivity.BackColor = System.Drawing.Color.Gray;
            this.btnComputeActivity.FlatAppearance.BorderSize = 0;
            this.btnComputeActivity.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnComputeActivity.Font = new System.Drawing.Font("Segoe UI Symbol", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnComputeActivity.ForeColor = System.Drawing.Color.White;
            this.btnComputeActivity.Location = new System.Drawing.Point(168, 145);
            this.btnComputeActivity.Name = "btnComputeActivity";
            this.btnComputeActivity.Size = new System.Drawing.Size(486, 22);
            this.btnComputeActivity.TabIndex = 203;
            this.btnComputeActivity.TabStop = false;
            this.btnComputeActivity.Tag = "color:light1";
            this.btnComputeActivity.Text = "Compute activity";
            this.btnComputeActivity.UseVisualStyleBackColor = false;
            this.btnComputeActivity.Click += new System.EventHandler(this.btnComputeActivity_Click);
            // 
            // cbWordSize
            // 
            this.cbWordSize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.cbWordSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWordSize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbWordSize.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbWordSize.ForeColor = System.Drawing.Color.White;
            this.cbWordSize.FormattingEnabled = true;
            this.cbWordSize.Items.AddRange(new object[] {
            "8-bit",
            "16-bit",
            "32-bit",
            "64-bit"});
            this.cbWordSize.Location = new System.Drawing.Point(286, 36);
            this.cbWordSize.Name = "cbWordSize";
            this.cbWordSize.Size = new System.Drawing.Size(151, 21);
            this.cbWordSize.TabIndex = 205;
            this.cbWordSize.Tag = "color:normal";
            this.cbWordSize.SelectedIndexChanged += new System.EventHandler(this.cbWordSize_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F);
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(165, 40);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(117, 13);
            this.label5.TabIndex = 206;
            this.label5.Text = "Word Size (Precision):";
            // 
            // lbDumps
            // 
            this.lbDumps.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbDumps.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.lbDumps.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbDumps.DisplayMember = "key";
            this.lbDumps.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lbDumps.ForeColor = System.Drawing.Color.White;
            this.lbDumps.FormattingEnabled = true;
            this.lbDumps.IntegralHeight = false;
            this.lbDumps.Location = new System.Drawing.Point(15, 31);
            this.lbDumps.Margin = new System.Windows.Forms.Padding(5);
            this.lbDumps.Name = "lbDumps";
            this.lbDumps.ScrollAlwaysVisible = true;
            this.lbDumps.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbDumps.Size = new System.Drawing.Size(133, 390);
            this.lbDumps.TabIndex = 191;
            this.lbDumps.Tag = "color:dark2";
            this.lbDumps.ValueMember = "value";
            this.lbDumps.SelectedIndexChanged += new System.EventHandler(this.lbDumps_SelectedIndexChanged);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.radioButton2);
            this.panel2.Controls.Add(this.radioButton1);
            this.panel2.Controls.Add(this.comboBox1);
            this.panel2.Location = new System.Drawing.Point(460, 35);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(194, 94);
            this.panel2.TabIndex = 207;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(8, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 211;
            this.label2.Text = "List:";
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(8, 28);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(82, 17);
            this.radioButton2.TabIndex = 210;
            this.radioButton2.Text = "Against list";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(8, 6);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(141, 17);
            this.radioButton1.TabIndex = 209;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Against previous value";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox1.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.comboBox1.ForeColor = System.Drawing.Color.White;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "8-bit",
            "16-bit",
            "32-bit",
            "64-bit"});
            this.comboBox1.Location = new System.Drawing.Point(41, 50);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(136, 21);
            this.comboBox1.TabIndex = 208;
            this.comboBox1.Tag = "color:normal";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(457, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 13);
            this.label1.TabIndex = 208;
            this.label1.Text = "Definition of activity";
            // 
            // RTC_AnalyticsTool_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(677, 498);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cbWordSize);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.lbDumpsSelected);
            this.Controls.Add(this.btnComputeActivity);
            this.Controls.Add(this.lbDomainSize);
            this.Controls.Add(this.pbActivity);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnSelectNoDumps);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lbDumps);
            this.Controls.Add(this.btnSelectAllDumps);
            this.Font = new System.Drawing.Font("Segoe UI Symbol", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ForeColor = System.Drawing.Color.White;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "RTC_AnalyticsTool_Form";
            this.Tag = "color:dark1";
            this.Text = "Analytics Tool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RTC_SanitizeTool_Form_FormClosing);
            this.Load += new System.EventHandler(this.RTC_AnalyticsToolForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbActivity)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
        public Components.Controls.ListBoxExtended lbDumps;
        private Button btnSelectAllDumps;
        private Label label6;
        private Button btnSelectNoDumps;
        private Button button2;
        private Label lbDumpsSelected;
        private Label lbDomainSize;
        private Label label4;
        private PictureBox pbActivity;
        private Button btnComputeActivity;
        public ComboBox cbWordSize;
        private Label label5;
        private Panel panel2;
        private Label label2;
        private RadioButton radioButton2;
        private RadioButton radioButton1;
        public ComboBox comboBox1;
        private Label label1;
    }
}
