using System.Windows.Forms;
using RTCV.UI;

namespace RTCV.UI
{
	partial class RTC_SanitizeTool_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RTC_SanitizeTool_Form));
            this.lbOriginalLayerSize = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnYesEffect = new System.Windows.Forms.Button();
            this.lbCurrentLayerSize = new System.Windows.Forms.Label();
            this.pnBlastLayerSanitization = new System.Windows.Forms.Panel();
            this.btnReplayLast = new System.Windows.Forms.Button();
            this.lbSanitizationText = new System.Windows.Forms.Label();
            this.btnNoEffect = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnBackPrevState = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.btnLeaveWithChanges = new System.Windows.Forms.Button();
            this.btnLeaveSubstractChanges = new System.Windows.Forms.Button();
            this.btnLeaveWithoutChanges = new System.Windows.Forms.Button();
            this.btnStartSanitizing = new System.Windows.Forms.Button();
            this.btnReroll = new System.Windows.Forms.Button();
            this.lbSteps = new RTCV.UI.Components.Controls.ListBoxExtended();
            this.pnBlastLayerSanitization.SuspendLayout();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbOriginalLayerSize
            // 
            this.lbOriginalLayerSize.ForeColor = System.Drawing.Color.White;
            this.lbOriginalLayerSize.Location = new System.Drawing.Point(11, 10);
            this.lbOriginalLayerSize.Name = "lbOriginalLayerSize";
            this.lbOriginalLayerSize.Size = new System.Drawing.Size(304, 19);
            this.lbOriginalLayerSize.TabIndex = 132;
            this.lbOriginalLayerSize.Text = "Original Layer size:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(16, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 13);
            this.label3.TabIndex = 135;
            this.label3.Text = "BlastLayer Info";
            // 
            // btnYesEffect
            // 
            this.btnYesEffect.BackColor = System.Drawing.Color.Gray;
            this.btnYesEffect.FlatAppearance.BorderSize = 0;
            this.btnYesEffect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnYesEffect.Font = new System.Drawing.Font("Segoe UI Symbol", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnYesEffect.ForeColor = System.Drawing.Color.White;
            this.btnYesEffect.Location = new System.Drawing.Point(16, 44);
            this.btnYesEffect.Name = "btnYesEffect";
            this.btnYesEffect.Size = new System.Drawing.Size(64, 23);
            this.btnYesEffect.TabIndex = 183;
            this.btnYesEffect.TabStop = false;
            this.btnYesEffect.Tag = "color:light1";
            this.btnYesEffect.Text = "Yes";
            this.btnYesEffect.UseVisualStyleBackColor = false;
            this.btnYesEffect.Click += new System.EventHandler(this.btnYesEffect_Click);
            // 
            // lbCurrentLayerSize
            // 
            this.lbCurrentLayerSize.ForeColor = System.Drawing.Color.White;
            this.lbCurrentLayerSize.Location = new System.Drawing.Point(11, 33);
            this.lbCurrentLayerSize.Name = "lbCurrentLayerSize";
            this.lbCurrentLayerSize.Size = new System.Drawing.Size(307, 19);
            this.lbCurrentLayerSize.TabIndex = 133;
            this.lbCurrentLayerSize.Text = "Current Layer size:";
            // 
            // pnBlastLayerSanitization
            // 
            this.pnBlastLayerSanitization.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnBlastLayerSanitization.Controls.Add(this.btnReroll);
            this.pnBlastLayerSanitization.Controls.Add(this.btnReplayLast);
            this.pnBlastLayerSanitization.Controls.Add(this.lbSanitizationText);
            this.pnBlastLayerSanitization.Controls.Add(this.btnNoEffect);
            this.pnBlastLayerSanitization.Controls.Add(this.btnYesEffect);
            this.pnBlastLayerSanitization.Location = new System.Drawing.Point(19, 124);
            this.pnBlastLayerSanitization.Name = "pnBlastLayerSanitization";
            this.pnBlastLayerSanitization.Size = new System.Drawing.Size(332, 116);
            this.pnBlastLayerSanitization.TabIndex = 185;
            this.pnBlastLayerSanitization.Visible = false;
            // 
            // btnReplayLast
            // 
            this.btnReplayLast.BackColor = System.Drawing.Color.Gray;
            this.btnReplayLast.FlatAppearance.BorderSize = 0;
            this.btnReplayLast.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReplayLast.Font = new System.Drawing.Font("Segoe UI Symbol", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnReplayLast.ForeColor = System.Drawing.Color.White;
            this.btnReplayLast.Location = new System.Drawing.Point(170, 44);
            this.btnReplayLast.Name = "btnReplayLast";
            this.btnReplayLast.Size = new System.Drawing.Size(147, 53);
            this.btnReplayLast.TabIndex = 188;
            this.btnReplayLast.TabStop = false;
            this.btnReplayLast.Tag = "color:light1";
            this.btnReplayLast.Text = "Replay last corruption";
            this.btnReplayLast.UseVisualStyleBackColor = false;
            this.btnReplayLast.Click += new System.EventHandler(this.btnReplayLast_Click);
            // 
            // lbSanitizationText
            // 
            this.lbSanitizationText.Font = new System.Drawing.Font("Segoe UI Symbol", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbSanitizationText.Location = new System.Drawing.Point(16, 6);
            this.lbSanitizationText.Name = "lbSanitizationText";
            this.lbSanitizationText.Size = new System.Drawing.Size(301, 30);
            this.lbSanitizationText.TabIndex = 187;
            this.lbSanitizationText.Text = "Is the effect you are looking for still present?";
            this.lbSanitizationText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnNoEffect
            // 
            this.btnNoEffect.BackColor = System.Drawing.Color.Gray;
            this.btnNoEffect.FlatAppearance.BorderSize = 0;
            this.btnNoEffect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNoEffect.Font = new System.Drawing.Font("Segoe UI Symbol", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnNoEffect.ForeColor = System.Drawing.Color.White;
            this.btnNoEffect.Location = new System.Drawing.Point(93, 44);
            this.btnNoEffect.Name = "btnNoEffect";
            this.btnNoEffect.Size = new System.Drawing.Size(64, 23);
            this.btnNoEffect.TabIndex = 184;
            this.btnNoEffect.TabStop = false;
            this.btnNoEffect.Tag = "color:light1";
            this.btnNoEffect.Text = "No";
            this.btnNoEffect.UseVisualStyleBackColor = false;
            this.btnNoEffect.Click += new System.EventHandler(this.btnNoEffect_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 13);
            this.label2.TabIndex = 186;
            this.label2.Text = "BlastLayer Sanitization";
            // 
            // btnBackPrevState
            // 
            this.btnBackPrevState.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBackPrevState.BackColor = System.Drawing.Color.Gray;
            this.btnBackPrevState.FlatAppearance.BorderSize = 0;
            this.btnBackPrevState.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBackPrevState.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnBackPrevState.ForeColor = System.Drawing.Color.White;
            this.btnBackPrevState.Location = new System.Drawing.Point(372, 287);
            this.btnBackPrevState.Name = "btnBackPrevState";
            this.btnBackPrevState.Size = new System.Drawing.Size(181, 24);
            this.btnBackPrevState.TabIndex = 192;
            this.btnBackPrevState.TabStop = false;
            this.btnBackPrevState.Tag = "color:light1";
            this.btnBackPrevState.Text = "Go back to previous state";
            this.btnBackPrevState.UseVisualStyleBackColor = false;
            this.btnBackPrevState.Click += new System.EventHandler(this.btnBackPrevState_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(369, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(99, 13);
            this.label6.TabIndex = 193;
            this.label6.Text = "Sanitization Steps";
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.lbCurrentLayerSize);
            this.panel6.Controls.Add(this.lbOriginalLayerSize);
            this.panel6.Location = new System.Drawing.Point(19, 35);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(332, 60);
            this.panel6.TabIndex = 194;
            // 
            // btnLeaveWithChanges
            // 
            this.btnLeaveWithChanges.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnLeaveWithChanges.FlatAppearance.BorderSize = 0;
            this.btnLeaveWithChanges.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLeaveWithChanges.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnLeaveWithChanges.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnLeaveWithChanges.Location = new System.Drawing.Point(19, 253);
            this.btnLeaveWithChanges.Name = "btnLeaveWithChanges";
            this.btnLeaveWithChanges.Size = new System.Drawing.Size(145, 25);
            this.btnLeaveWithChanges.TabIndex = 195;
            this.btnLeaveWithChanges.TabStop = false;
            this.btnLeaveWithChanges.Tag = "color:dark2";
            this.btnLeaveWithChanges.Text = "Leave with changes";
            this.btnLeaveWithChanges.UseVisualStyleBackColor = false;
            this.btnLeaveWithChanges.Click += new System.EventHandler(this.btnLeaveWithChanges_Click);
            // 
            // btnLeaveSubstractChanges
            // 
            this.btnLeaveSubstractChanges.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnLeaveSubstractChanges.FlatAppearance.BorderSize = 0;
            this.btnLeaveSubstractChanges.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLeaveSubstractChanges.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnLeaveSubstractChanges.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnLeaveSubstractChanges.Location = new System.Drawing.Point(170, 253);
            this.btnLeaveSubstractChanges.Name = "btnLeaveSubstractChanges";
            this.btnLeaveSubstractChanges.Size = new System.Drawing.Size(181, 25);
            this.btnLeaveSubstractChanges.TabIndex = 196;
            this.btnLeaveSubstractChanges.TabStop = false;
            this.btnLeaveSubstractChanges.Tag = "color:dark2";
            this.btnLeaveSubstractChanges.Text = "Leave and subtract changes";
            this.btnLeaveSubstractChanges.UseVisualStyleBackColor = false;
            this.btnLeaveSubstractChanges.Click += new System.EventHandler(this.btnLeaveSubstractChanges_Click);
            // 
            // btnLeaveWithoutChanges
            // 
            this.btnLeaveWithoutChanges.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnLeaveWithoutChanges.FlatAppearance.BorderSize = 0;
            this.btnLeaveWithoutChanges.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLeaveWithoutChanges.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnLeaveWithoutChanges.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnLeaveWithoutChanges.Location = new System.Drawing.Point(19, 286);
            this.btnLeaveWithoutChanges.Name = "btnLeaveWithoutChanges";
            this.btnLeaveWithoutChanges.Size = new System.Drawing.Size(332, 25);
            this.btnLeaveWithoutChanges.TabIndex = 197;
            this.btnLeaveWithoutChanges.TabStop = false;
            this.btnLeaveWithoutChanges.Tag = "color:dark2";
            this.btnLeaveWithoutChanges.Text = "Leave without changes (Restore BlastLayer)";
            this.btnLeaveWithoutChanges.UseVisualStyleBackColor = false;
            this.btnLeaveWithoutChanges.Click += new System.EventHandler(this.btnLeaveWithoutChanges_Click);
            // 
            // btnStartSanitizing
            // 
            this.btnStartSanitizing.BackColor = System.Drawing.Color.Gray;
            this.btnStartSanitizing.FlatAppearance.BorderSize = 0;
            this.btnStartSanitizing.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartSanitizing.Font = new System.Drawing.Font("Segoe UI Symbol", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnStartSanitizing.ForeColor = System.Drawing.Color.White;
            this.btnStartSanitizing.Location = new System.Drawing.Point(28, 134);
            this.btnStartSanitizing.Name = "btnStartSanitizing";
            this.btnStartSanitizing.Size = new System.Drawing.Size(123, 22);
            this.btnStartSanitizing.TabIndex = 189;
            this.btnStartSanitizing.TabStop = false;
            this.btnStartSanitizing.Tag = "color:light1";
            this.btnStartSanitizing.Text = "Start Sanitizing";
            this.btnStartSanitizing.UseVisualStyleBackColor = false;
            this.btnStartSanitizing.Click += new System.EventHandler(this.btnStartSanitizing_Click);
            // 
            // btnReroll
            // 
            this.btnReroll.BackColor = System.Drawing.Color.Gray;
            this.btnReroll.FlatAppearance.BorderSize = 0;
            this.btnReroll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReroll.Font = new System.Drawing.Font("Segoe UI Symbol", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnReroll.ForeColor = System.Drawing.Color.White;
            this.btnReroll.Location = new System.Drawing.Point(16, 74);
            this.btnReroll.MinimumSize = new System.Drawing.Size(141, 23);
            this.btnReroll.Name = "btnReroll";
            this.btnReroll.Size = new System.Drawing.Size(141, 23);
            this.btnReroll.TabIndex = 189;
            this.btnReroll.TabStop = false;
            this.btnReroll.Tag = "color:light1";
            this.btnReroll.Text = "Reroll sanitize step";
            this.btnReroll.UseVisualStyleBackColor = false;
            this.btnReroll.Click += new System.EventHandler(this.btnReroll_Click);
            // 
            // lbSteps
            // 
            this.lbSteps.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbSteps.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.lbSteps.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbSteps.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lbSteps.ForeColor = System.Drawing.Color.White;
            this.lbSteps.FormattingEnabled = true;
            this.lbSteps.IntegralHeight = false;
            this.lbSteps.Location = new System.Drawing.Point(372, 33);
            this.lbSteps.Margin = new System.Windows.Forms.Padding(5);
            this.lbSteps.Name = "lbSteps";
            this.lbSteps.ScrollAlwaysVisible = true;
            this.lbSteps.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.lbSteps.Size = new System.Drawing.Size(181, 253);
            this.lbSteps.TabIndex = 191;
            this.lbSteps.Tag = "color:dark2";
            // 
            // RTC_SanitizeTool_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(574, 332);
            this.Controls.Add(this.btnStartSanitizing);
            this.Controls.Add(this.btnLeaveWithoutChanges);
            this.Controls.Add(this.btnLeaveSubstractChanges);
            this.Controls.Add(this.btnLeaveWithChanges);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lbSteps);
            this.Controls.Add(this.btnBackPrevState);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pnBlastLayerSanitization);
            this.Controls.Add(this.label3);
            this.Font = new System.Drawing.Font("Segoe UI Symbol", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ForeColor = System.Drawing.Color.White;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(590, 340);
            this.Name = "RTC_SanitizeTool_Form";
            this.Tag = "color:dark1";
            this.Text = "Sanitize Tool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RTC_SanitizeTool_Form_FormClosing);
            this.Load += new System.EventHandler(this.RTC_NewBlastEditorForm_Load);
            this.pnBlastLayerSanitization.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label lbOriginalLayerSize;
        private Label lbCurrentLayerSize;
        private Button btnYesEffect;
        private Panel pnBlastLayerSanitization;
        private Button btnReplayLast;
        private Label lbSanitizationText;
        private Button btnNoEffect;
        private Label label2;
        public Components.Controls.ListBoxExtended lbSteps;
        private Button btnBackPrevState;
        private Label label6;
        private Panel panel6;
        private Button btnLeaveWithChanges;
        private Button btnLeaveSubstractChanges;
        private Button btnLeaveWithoutChanges;
        private Button btnStartSanitizing;
        private Button btnReroll;
    }
}