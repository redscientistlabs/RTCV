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
            this.btnReroll = new System.Windows.Forms.Button();
            this.btnReplayLast = new System.Windows.Forms.Button();
            this.lbSanitizationText = new System.Windows.Forms.Label();
            this.btnNoEffect = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnBackPrevState = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.pbProgress = new System.Windows.Forms.ProgressBar();
            this.btnLeaveWithChanges = new System.Windows.Forms.Button();
            this.btnLeaveSubstractChanges = new System.Windows.Forms.Button();
            this.btnLeaveWithoutChanges = new System.Windows.Forms.Button();
            this.btnStartSanitizing = new System.Windows.Forms.Button();
            this.lbSteps = new RTCV.UI.Components.Controls.ListBoxExtended();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAddToStockpile = new System.Windows.Forms.Button();
            this.btnLeaveNoChanges = new System.Windows.Forms.Button();
            this.lbWorkingPleaseWait = new System.Windows.Forms.Label();
            this.btnAddToStash = new System.Windows.Forms.Button();
            this.pnBlastLayerSanitization.SuspendLayout();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbOriginalLayerSize
            // 
            this.lbOriginalLayerSize.ForeColor = System.Drawing.Color.White;
            this.lbOriginalLayerSize.Location = new System.Drawing.Point(10, 4);
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
            this.label3.Size = new System.Drawing.Size(95, 13);
            this.label3.TabIndex = 135;
            this.label3.Text = "Sanitize progress";
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
            this.lbCurrentLayerSize.Location = new System.Drawing.Point(11, 22);
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
            this.btnReroll.Text = "Try different attempt";
            this.btnReroll.UseVisualStyleBackColor = false;
            this.btnReroll.Click += new System.EventHandler(this.btnReroll_Click);
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
            this.btnReplayLast.Text = "Reload current attempt (Replay corruption)";
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
            this.label2.Location = new System.Drawing.Point(16, 106);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 186;
            this.label2.Text = "Guided steps";
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
            this.btnBackPrevState.Location = new System.Drawing.Point(372, 339);
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
            this.panel6.Controls.Add(this.pbProgress);
            this.panel6.Controls.Add(this.lbCurrentLayerSize);
            this.panel6.Controls.Add(this.lbOriginalLayerSize);
            this.panel6.Location = new System.Drawing.Point(19, 35);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(332, 60);
            this.panel6.TabIndex = 194;
            // 
            // pbProgress
            // 
            this.pbProgress.Location = new System.Drawing.Point(16, 43);
            this.pbProgress.Name = "pbProgress";
            this.pbProgress.Size = new System.Drawing.Size(302, 7);
            this.pbProgress.TabIndex = 134;
            // 
            // btnLeaveWithChanges
            // 
            this.btnLeaveWithChanges.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLeaveWithChanges.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnLeaveWithChanges.FlatAppearance.BorderSize = 0;
            this.btnLeaveWithChanges.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLeaveWithChanges.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnLeaveWithChanges.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnLeaveWithChanges.Location = new System.Drawing.Point(18, 276);
            this.btnLeaveWithChanges.Name = "btnLeaveWithChanges";
            this.btnLeaveWithChanges.Size = new System.Drawing.Size(172, 25);
            this.btnLeaveWithChanges.TabIndex = 195;
            this.btnLeaveWithChanges.TabStop = false;
            this.btnLeaveWithChanges.Tag = "color:dark2";
            this.btnLeaveWithChanges.Text = "Keep changes";
            this.btnLeaveWithChanges.UseVisualStyleBackColor = false;
            this.btnLeaveWithChanges.Click += new System.EventHandler(this.btnLeaveWithChanges_Click);
            // 
            // btnLeaveSubstractChanges
            // 
            this.btnLeaveSubstractChanges.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLeaveSubstractChanges.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnLeaveSubstractChanges.FlatAppearance.BorderSize = 0;
            this.btnLeaveSubstractChanges.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLeaveSubstractChanges.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnLeaveSubstractChanges.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnLeaveSubstractChanges.Location = new System.Drawing.Point(18, 307);
            this.btnLeaveSubstractChanges.Name = "btnLeaveSubstractChanges";
            this.btnLeaveSubstractChanges.Size = new System.Drawing.Size(172, 25);
            this.btnLeaveSubstractChanges.TabIndex = 196;
            this.btnLeaveSubstractChanges.TabStop = false;
            this.btnLeaveSubstractChanges.Tag = "color:dark2";
            this.btnLeaveSubstractChanges.Text = "Subtract result from original";
            this.btnLeaveSubstractChanges.UseVisualStyleBackColor = false;
            this.btnLeaveSubstractChanges.Click += new System.EventHandler(this.btnLeaveSubstractChanges_Click);
            // 
            // btnLeaveWithoutChanges
            // 
            this.btnLeaveWithoutChanges.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLeaveWithoutChanges.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnLeaveWithoutChanges.FlatAppearance.BorderSize = 0;
            this.btnLeaveWithoutChanges.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLeaveWithoutChanges.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnLeaveWithoutChanges.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnLeaveWithoutChanges.Location = new System.Drawing.Point(18, 338);
            this.btnLeaveWithoutChanges.Name = "btnLeaveWithoutChanges";
            this.btnLeaveWithoutChanges.Size = new System.Drawing.Size(172, 25);
            this.btnLeaveWithoutChanges.TabIndex = 197;
            this.btnLeaveWithoutChanges.TabStop = false;
            this.btnLeaveWithoutChanges.Tag = "color:dark2";
            this.btnLeaveWithoutChanges.Text = "Reload original Blast Layer";
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
            this.btnStartSanitizing.Location = new System.Drawing.Point(37, 136);
            this.btnStartSanitizing.Name = "btnStartSanitizing";
            this.btnStartSanitizing.Size = new System.Drawing.Size(300, 22);
            this.btnStartSanitizing.TabIndex = 189;
            this.btnStartSanitizing.TabStop = false;
            this.btnStartSanitizing.Tag = "color:light1";
            this.btnStartSanitizing.Text = "Start Sanitizing";
            this.btnStartSanitizing.UseVisualStyleBackColor = false;
            this.btnStartSanitizing.Click += new System.EventHandler(this.btnStartSanitizing_Click);
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
            this.lbSteps.Size = new System.Drawing.Size(181, 305);
            this.lbSteps.TabIndex = 191;
            this.lbSteps.Tag = "color:dark2";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.Font = new System.Drawing.Font("Segoe UI Symbol", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(15, 243);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(178, 30);
            this.label1.TabIndex = 198;
            this.label1.Text = "Send results to Blast Editor";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnAddToStockpile
            // 
            this.btnAddToStockpile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddToStockpile.BackColor = System.Drawing.Color.Gray;
            this.btnAddToStockpile.FlatAppearance.BorderSize = 0;
            this.btnAddToStockpile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddToStockpile.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnAddToStockpile.ForeColor = System.Drawing.Color.White;
            this.btnAddToStockpile.Image = ((System.Drawing.Image)(resources.GetObject("btnAddToStockpile.Image")));
            this.btnAddToStockpile.Location = new System.Drawing.Point(219, 292);
            this.btnAddToStockpile.Name = "btnAddToStockpile";
            this.btnAddToStockpile.Size = new System.Drawing.Size(132, 25);
            this.btnAddToStockpile.TabIndex = 199;
            this.btnAddToStockpile.TabStop = false;
            this.btnAddToStockpile.Tag = "color:light1";
            this.btnAddToStockpile.Text = "  To Stockpile";
            this.btnAddToStockpile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAddToStockpile.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAddToStockpile.UseVisualStyleBackColor = false;
            this.btnAddToStockpile.Click += new System.EventHandler(this.btnAddToStockpile_Click);
            // 
            // btnLeaveNoChanges
            // 
            this.btnLeaveNoChanges.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLeaveNoChanges.BackColor = System.Drawing.Color.Gray;
            this.btnLeaveNoChanges.FlatAppearance.BorderSize = 0;
            this.btnLeaveNoChanges.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLeaveNoChanges.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnLeaveNoChanges.ForeColor = System.Drawing.Color.White;
            this.btnLeaveNoChanges.Location = new System.Drawing.Point(219, 323);
            this.btnLeaveNoChanges.Name = "btnLeaveNoChanges";
            this.btnLeaveNoChanges.Size = new System.Drawing.Size(132, 40);
            this.btnLeaveNoChanges.TabIndex = 201;
            this.btnLeaveNoChanges.TabStop = false;
            this.btnLeaveNoChanges.Tag = "color:light1";
            this.btnLeaveNoChanges.Text = "Leave with no changes";
            this.btnLeaveNoChanges.UseVisualStyleBackColor = false;
            this.btnLeaveNoChanges.Click += new System.EventHandler(this.btnLeaveNoChanges_Click);
            // 
            // lbWorkingPleaseWait
            // 
            this.lbWorkingPleaseWait.Font = new System.Drawing.Font("Segoe UI Symbol", 12F);
            this.lbWorkingPleaseWait.Location = new System.Drawing.Point(95, 165);
            this.lbWorkingPleaseWait.Name = "lbWorkingPleaseWait";
            this.lbWorkingPleaseWait.Size = new System.Drawing.Size(178, 30);
            this.lbWorkingPleaseWait.TabIndex = 202;
            this.lbWorkingPleaseWait.Text = "Working... Please Wait...";
            this.lbWorkingPleaseWait.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbWorkingPleaseWait.Visible = false;
            // 
            // btnAddToStash
            // 
            this.btnAddToStash.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddToStash.BackColor = System.Drawing.Color.Gray;
            this.btnAddToStash.FlatAppearance.BorderSize = 0;
            this.btnAddToStash.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddToStash.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnAddToStash.ForeColor = System.Drawing.Color.White;
            this.btnAddToStash.Image = ((System.Drawing.Image)(resources.GetObject("btnAddToStash.Image")));
            this.btnAddToStash.Location = new System.Drawing.Point(219, 261);
            this.btnAddToStash.Name = "btnAddToStash";
            this.btnAddToStash.Size = new System.Drawing.Size(132, 25);
            this.btnAddToStash.TabIndex = 203;
            this.btnAddToStash.TabStop = false;
            this.btnAddToStash.Tag = "color:light1";
            this.btnAddToStash.Text = "  To Stash";
            this.btnAddToStash.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAddToStash.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAddToStash.UseVisualStyleBackColor = false;
            this.btnAddToStash.Click += new System.EventHandler(this.btnAddToStash_Click);
            // 
            // RTC_SanitizeTool_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(574, 384);
            this.Controls.Add(this.btnAddToStash);
            this.Controls.Add(this.btnLeaveNoChanges);
            this.Controls.Add(this.btnAddToStockpile);
            this.Controls.Add(this.label1);
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
            this.Controls.Add(this.lbWorkingPleaseWait);
            this.Font = new System.Drawing.Font("Segoe UI Symbol", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(590, 401);
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
        private Label label1;
        private ProgressBar pbProgress;
        public Button btnAddToStockpile;
        private Button btnLeaveNoChanges;
        private Label lbWorkingPleaseWait;
        public Button btnAddToStash;
    }
}
