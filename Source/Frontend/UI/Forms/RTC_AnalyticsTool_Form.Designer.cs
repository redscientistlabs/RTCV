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
            this.btnBackPrevState = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.pbActivity = new System.Windows.Forms.PictureBox();
            this.btnCalculateActivity = new System.Windows.Forms.Button();
            this.lbDumps = new RTCV.UI.Components.Controls.ListBoxExtended();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbActivity)).BeginInit();
            this.SuspendLayout();
            // 
            // btnBackPrevState
            // 
            this.btnBackPrevState.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnBackPrevState.BackColor = System.Drawing.Color.Gray;
            this.btnBackPrevState.FlatAppearance.BorderSize = 0;
            this.btnBackPrevState.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBackPrevState.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnBackPrevState.ForeColor = System.Drawing.Color.White;
            this.btnBackPrevState.Location = new System.Drawing.Point(14, 429);
            this.btnBackPrevState.Name = "btnBackPrevState";
            this.btnBackPrevState.Size = new System.Drawing.Size(159, 24);
            this.btnBackPrevState.TabIndex = 192;
            this.btnBackPrevState.TabStop = false;
            this.btnBackPrevState.Tag = "color:light1";
            this.btnBackPrevState.Text = "Select All";
            this.btnBackPrevState.UseVisualStyleBackColor = false;
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
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.BackColor = System.Drawing.Color.Gray;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(14, 459);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(159, 24);
            this.button1.TabIndex = 198;
            this.button1.TabStop = false;
            this.button1.Tag = "color:light1";
            this.button1.Text = "Select None";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.button2.ForeColor = System.Drawing.Color.OrangeRed;
            this.button2.Location = new System.Drawing.Point(15, 8);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(125, 42);
            this.button2.TabIndex = 199;
            this.button2.TabStop = false;
            this.button2.Tag = "color:dark2";
            this.button2.Text = "Generate VMD";
            this.button2.UseVisualStyleBackColor = false;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.button3.ForeColor = System.Drawing.Color.OrangeRed;
            this.button3.Location = new System.Drawing.Point(157, 8);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(154, 42);
            this.button3.TabIndex = 200;
            this.button3.TabStop = false;
            this.button3.Tag = "color:dark2";
            this.button3.Text = "Generate BlastLayer and send to stash";
            this.button3.UseVisualStyleBackColor = false;
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.Gray;
            this.button4.FlatAppearance.BorderSize = 0;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Font = new System.Drawing.Font("Segoe UI Symbol", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.button4.ForeColor = System.Drawing.Color.White;
            this.button4.Location = new System.Drawing.Point(329, 345);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(147, 23);
            this.button4.TabIndex = 201;
            this.button4.TabStop = false;
            this.button4.Tag = "color:light1";
            this.button4.Text = "Replay last corruption";
            this.button4.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Segoe UI Symbol", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label1.Location = new System.Drawing.Point(203, 301);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(301, 30);
            this.label1.TabIndex = 189;
            this.label1.Text = "Is the effect you are looking for still present?";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Location = new System.Drawing.Point(184, 423);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(332, 60);
            this.panel1.TabIndex = 196;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(181, 404);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(137, 13);
            this.label4.TabIndex = 195;
            this.label4.Text = "Export Filtered Addresses";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Segoe UI Symbol", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label5.Location = new System.Drawing.Point(181, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(153, 19);
            this.label5.TabIndex = 203;
            this.label5.Text = "Domain size:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Segoe UI Symbol", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label7.Location = new System.Drawing.Point(342, 10);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(174, 19);
            this.label7.TabIndex = 204;
            this.label7.Text = "Dumps selected:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pbActivity
            // 
            this.pbActivity.Location = new System.Drawing.Point(184, 60);
            this.pbActivity.Name = "pbActivity";
            this.pbActivity.Size = new System.Drawing.Size(332, 35);
            this.pbActivity.TabIndex = 202;
            this.pbActivity.TabStop = false;
            // 
            // button5
            // 
            this.btnCalculateActivity.BackColor = System.Drawing.Color.Gray;
            this.btnCalculateActivity.FlatAppearance.BorderSize = 0;
            this.btnCalculateActivity.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCalculateActivity.Font = new System.Drawing.Font("Segoe UI Symbol", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnCalculateActivity.ForeColor = System.Drawing.Color.White;
            this.btnCalculateActivity.Location = new System.Drawing.Point(184, 32);
            this.btnCalculateActivity.Name = "button5";
            this.btnCalculateActivity.Size = new System.Drawing.Size(332, 22);
            this.btnCalculateActivity.TabIndex = 203;
            this.btnCalculateActivity.TabStop = false;
            this.btnCalculateActivity.Tag = "color:light1";
            this.btnCalculateActivity.Text = "Compute activity";
            this.btnCalculateActivity.UseVisualStyleBackColor = false;
            this.btnCalculateActivity.Click += new System.EventHandler(this.btnCalculateActivity_Click);
            // 
            // lbDumps
            // 
            this.lbDumps.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbDumps.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.lbDumps.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbDumps.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lbDumps.ForeColor = System.Drawing.Color.White;
            this.lbDumps.FormattingEnabled = true;
            this.lbDumps.IntegralHeight = false;
            this.lbDumps.Location = new System.Drawing.Point(16, 31);
            this.lbDumps.Margin = new System.Windows.Forms.Padding(5);
            this.lbDumps.Name = "lbDumps";
            this.lbDumps.ScrollAlwaysVisible = true;
            this.lbDumps.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbDumps.Size = new System.Drawing.Size(157, 390);
            this.lbDumps.TabIndex = 191;
            this.lbDumps.Tag = "color:dark2";
            // 
            // RTC_AnalyticsTool_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(529, 498);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btnCalculateActivity);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.pbActivity);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lbDumps);
            this.Controls.Add(this.btnBackPrevState);
            this.Font = new System.Drawing.Font("Segoe UI Symbol", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ForeColor = System.Drawing.Color.White;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "RTC_AnalyticsTool_Form";
            this.Tag = "color:dark1";
            this.Text = "Analytics Tool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RTC_SanitizeTool_Form_FormClosing);
            this.Load += new System.EventHandler(this.RTC_AnalyticsToolForm_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbActivity)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
        public Components.Controls.ListBoxExtended lbDumps;
        private Button btnBackPrevState;
        private Label label6;
        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Label label1;
        private Label label7;
        private Label label5;
        private Panel panel1;
        private Label label4;
        private PictureBox pbActivity;
        private Button btnCalculateActivity;
    }
}