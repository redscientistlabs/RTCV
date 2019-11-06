using System.Windows.Forms;
using RTCV.UI;

namespace RTCV.UI
{
	partial class RTC_Intro_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RTC_Intro_Form));
            this.lbWelcome = new System.Windows.Forms.Label();
            this.btnSimpleMode = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.tbDisclaimerText = new System.Windows.Forms.RichTextBox();
            this.btnNormalMode = new System.Windows.Forms.Button();
            this.cbAgree = new System.Windows.Forms.CheckBox();
            this.lbStartupMode = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbWelcome
            // 
            this.lbWelcome.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbWelcome.Font = new System.Drawing.Font("Segoe UI Symbol", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbWelcome.Location = new System.Drawing.Point(12, 5);
            this.lbWelcome.Name = "lbWelcome";
            this.lbWelcome.Size = new System.Drawing.Size(442, 30);
            this.lbWelcome.TabIndex = 187;
            this.lbWelcome.Text = "Please take a bit of time to read the following:";
            this.lbWelcome.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSimpleMode
            // 
            this.btnSimpleMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSimpleMode.BackColor = System.Drawing.Color.Gray;
            this.btnSimpleMode.FlatAppearance.BorderSize = 0;
            this.btnSimpleMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSimpleMode.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnSimpleMode.ForeColor = System.Drawing.Color.White;
            this.btnSimpleMode.Location = new System.Drawing.Point(12, 421);
            this.btnSimpleMode.Name = "btnSimpleMode";
            this.btnSimpleMode.Size = new System.Drawing.Size(168, 32);
            this.btnSimpleMode.TabIndex = 196;
            this.btnSimpleMode.TabStop = false;
            this.btnSimpleMode.Tag = "color:light1";
            this.btnSimpleMode.Text = "Simple Mode";
            this.btnSimpleMode.UseVisualStyleBackColor = false;
            this.btnSimpleMode.Visible = false;
            this.btnSimpleMode.Click += new System.EventHandler(this.btnSimpleMode_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnExit.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnExit.Location = new System.Drawing.Point(392, 421);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(78, 32);
            this.btnExit.TabIndex = 197;
            this.btnExit.TabStop = false;
            this.btnExit.Tag = "color:dark2";
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // tbDisclaimerText
            // 
            this.tbDisclaimerText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDisclaimerText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.tbDisclaimerText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbDisclaimerText.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbDisclaimerText.ForeColor = System.Drawing.Color.White;
            this.tbDisclaimerText.Location = new System.Drawing.Point(12, 38);
            this.tbDisclaimerText.Name = "tbDisclaimerText";
            this.tbDisclaimerText.ReadOnly = true;
            this.tbDisclaimerText.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.tbDisclaimerText.Size = new System.Drawing.Size(458, 308);
            this.tbDisclaimerText.TabIndex = 198;
            this.tbDisclaimerText.Tag = "color:dark2";
            this.tbDisclaimerText.Text = "disclaimer text";
            // 
            // btnNormalMode
            // 
            this.btnNormalMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNormalMode.BackColor = System.Drawing.Color.Gray;
            this.btnNormalMode.FlatAppearance.BorderSize = 0;
            this.btnNormalMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNormalMode.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnNormalMode.ForeColor = System.Drawing.Color.White;
            this.btnNormalMode.Location = new System.Drawing.Point(194, 421);
            this.btnNormalMode.Name = "btnNormalMode";
            this.btnNormalMode.Size = new System.Drawing.Size(186, 32);
            this.btnNormalMode.TabIndex = 199;
            this.btnNormalMode.TabStop = false;
            this.btnNormalMode.Tag = "color:light1";
            this.btnNormalMode.Text = "Normal Mode";
            this.btnNormalMode.UseVisualStyleBackColor = false;
            this.btnNormalMode.Visible = false;
            this.btnNormalMode.Click += new System.EventHandler(this.btnNormalMode_Click);
            // 
            // cbAgree
            // 
            this.cbAgree.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbAgree.AutoSize = true;
            this.cbAgree.Font = new System.Drawing.Font("Segoe UI Symbol", 9.75F);
            this.cbAgree.Location = new System.Drawing.Point(15, 355);
            this.cbAgree.Name = "cbAgree";
            this.cbAgree.Size = new System.Drawing.Size(261, 21);
            this.cbAgree.TabIndex = 200;
            this.cbAgree.Text = "I have read the above warning, let me in";
            this.cbAgree.UseVisualStyleBackColor = true;
            this.cbAgree.CheckedChanged += new System.EventHandler(this.cbAgree_CheckedChanged);
            // 
            // lbStartupMode
            // 
            this.lbStartupMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbStartupMode.Font = new System.Drawing.Font("Segoe UI Symbol", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbStartupMode.Location = new System.Drawing.Point(9, 393);
            this.lbStartupMode.Name = "lbStartupMode";
            this.lbStartupMode.Size = new System.Drawing.Size(370, 25);
            this.lbStartupMode.TabIndex = 201;
            this.lbStartupMode.Text = "Select the startup mode of RTC";
            this.lbStartupMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbStartupMode.Visible = false;
            // 
            // RTC_Intro_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(481, 464);
            this.Controls.Add(this.lbStartupMode);
            this.Controls.Add(this.cbAgree);
            this.Controls.Add(this.btnSimpleMode);
            this.Controls.Add(this.btnNormalMode);
            this.Controls.Add(this.tbDisclaimerText);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.lbWelcome);
            this.Font = new System.Drawing.Font("Segoe UI Symbol", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "RTC_Intro_Form";
            this.Tag = "color:dark1";
            this.Text = "Welcome to RTCV";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RTC_Intro_Form_FormClosing);
            this.Load += new System.EventHandler(this.RTC_Intro_Form_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
        private Label lbWelcome;
        private Button btnSimpleMode;
        private Button btnExit;
        private RichTextBox tbDisclaimerText;
        private Button btnNormalMode;
        private CheckBox cbAgree;
        private Label lbStartupMode;
    }
}