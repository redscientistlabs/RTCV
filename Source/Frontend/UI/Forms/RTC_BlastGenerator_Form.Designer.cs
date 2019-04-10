namespace RTCV.UI
{
	partial class RTC_BlastGenerator_Form
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RTC_BlastGenerator_Form));
			this.panelSidebar = new System.Windows.Forms.Panel();
			this.cbUnitsShareNote = new System.Windows.Forms.CheckBox();
			this.btnHelp = new System.Windows.Forms.Button();
			this.btnLoadCorrupt = new System.Windows.Forms.Button();
			this.btnJustCorrupt = new System.Windows.Forms.Button();
			this.btnRefreshDomains = new System.Windows.Forms.Button();
			this.btnSendTo = new System.Windows.Forms.Button();
			this.btnAddRow = new System.Windows.Forms.Button();
			this.panel2 = new System.Windows.Forms.Panel();
			this.btnNudgeParam2Up = new System.Windows.Forms.Button();
			this.btnNudgeParam2Down = new System.Windows.Forms.Button();
			this.updownNudgeParam2 = new RTCV.UI.NumericUpDownHexFix();
			this.btnNudgeParam1Up = new System.Windows.Forms.Button();
			this.btnNudgeParam1Down = new System.Windows.Forms.Button();
			this.updownNudgeParam1 = new RTCV.UI.NumericUpDownHexFix();
			this.btnNudgeEndAddressUp = new System.Windows.Forms.Button();
			this.btnNudgeEndAddressDown = new System.Windows.Forms.Button();
			this.updownNudgeEndAddress = new RTCV.UI.NumericUpDownHexFix();
			this.btnNudgeStartAddressUp = new System.Windows.Forms.Button();
			this.btnNudgeStartAddressDown = new System.Windows.Forms.Button();
			this.updownNudgeStartAddress = new RTCV.UI.NumericUpDownHexFix();
			this.label5 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.dgvBlastGenerator = new System.Windows.Forms.DataGridView();
			this.dgvBlastProtoReference = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dgvRowDirty = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dgvNoteText = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dgvEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.dgvDomain = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.dgvPrecision = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.dgvType = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.dgvMode = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.dgvInterval = new RTCV.UI.DataGridViewNumericUpDownColumn();
			this.dgvStartAddress = new RTCV.UI.DataGridViewNumericUpDownColumn();
			this.dgvEndAddress = new RTCV.UI.DataGridViewNumericUpDownColumn();
			this.dgvParam1 = new RTCV.UI.DataGridViewNumericUpDownColumn();
			this.dgvParam2 = new RTCV.UI.DataGridViewNumericUpDownColumn();
			this.dgvLifetime = new RTCV.UI.DataGridViewNumericUpDownColumn();
			this.dgvExecuteFrame = new RTCV.UI.DataGridViewNumericUpDownColumn();
			this.dgvLoop = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.dgvSeed = new RTCV.UI.DataGridViewNumericUpDownColumn();
			this.dgvNoteButton = new System.Windows.Forms.DataGridViewButtonColumn();
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewNumericUpDownColumn1 = new RTCV.UI.DataGridViewNumericUpDownColumn();
			this.dataGridViewNumericUpDownColumn2 = new RTCV.UI.DataGridViewNumericUpDownColumn();
			this.dataGridViewNumericUpDownColumn3 = new RTCV.UI.DataGridViewNumericUpDownColumn();
			this.dataGridViewNumericUpDownColumn4 = new RTCV.UI.DataGridViewNumericUpDownColumn();
			this.dataGridViewNumericUpDownColumn5 = new RTCV.UI.DataGridViewNumericUpDownColumn();
			this.menuStripEx1 = new System.Windows.Forms.MenuStrip();
			this.blastLayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAsToFileblToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.loadFromFileblToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.importBlastlayerblToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.btnHideSidebar = new System.Windows.Forms.Button();
			this.panelSidebar.SuspendLayout();
			this.panel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.updownNudgeParam2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.updownNudgeParam1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.updownNudgeEndAddress)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.updownNudgeStartAddress)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dgvBlastGenerator)).BeginInit();
			this.menuStripEx1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panelSidebar
			// 
			this.panelSidebar.Controls.Add(this.cbUnitsShareNote);
			this.panelSidebar.Controls.Add(this.btnHelp);
			this.panelSidebar.Controls.Add(this.btnLoadCorrupt);
			this.panelSidebar.Controls.Add(this.btnJustCorrupt);
			this.panelSidebar.Controls.Add(this.btnRefreshDomains);
			this.panelSidebar.Controls.Add(this.btnSendTo);
			this.panelSidebar.Controls.Add(this.btnAddRow);
			this.panelSidebar.Controls.Add(this.panel2);
			this.panelSidebar.Controls.Add(this.label4);
			this.panelSidebar.Dock = System.Windows.Forms.DockStyle.Right;
			this.panelSidebar.Location = new System.Drawing.Point(872, 24);
			this.panelSidebar.Name = "panelSidebar";
			this.panelSidebar.Size = new System.Drawing.Size(159, 365);
			this.panelSidebar.TabIndex = 166;
			this.panelSidebar.Tag = "color:dark";
			// 
			// cbUnitsShareNote
			// 
			this.cbUnitsShareNote.AutoSize = true;
			this.cbUnitsShareNote.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
			this.cbUnitsShareNote.ForeColor = System.Drawing.Color.White;
			this.cbUnitsShareNote.Location = new System.Drawing.Point(5, 196);
			this.cbUnitsShareNote.Name = "cbUnitsShareNote";
			this.cbUnitsShareNote.Size = new System.Drawing.Size(116, 17);
			this.cbUnitsShareNote.TabIndex = 175;
			this.cbUnitsShareNote.Tag = "";
			this.cbUnitsShareNote.Text = "Units inheret note";
			this.cbUnitsShareNote.UseVisualStyleBackColor = true;
			this.cbUnitsShareNote.CheckedChanged += new System.EventHandler(this.CbUnitsShareNote_CheckedChanged);
			// 
			// btnHelp
			// 
			this.btnHelp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.btnHelp.FlatAppearance.BorderSize = 0;
			this.btnHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnHelp.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
			this.btnHelp.ForeColor = System.Drawing.Color.Black;
			this.btnHelp.Location = new System.Drawing.Point(133, 1);
			this.btnHelp.Name = "btnHelp";
			this.btnHelp.Size = new System.Drawing.Size(27, 22);
			this.btnHelp.TabIndex = 174;
			this.btnHelp.TabStop = false;
			this.btnHelp.Tag = "color:dark";
			this.btnHelp.UseVisualStyleBackColor = false;
			this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
			// 
			// btnLoadCorrupt
			// 
			this.btnLoadCorrupt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnLoadCorrupt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
			this.btnLoadCorrupt.FlatAppearance.BorderSize = 0;
			this.btnLoadCorrupt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnLoadCorrupt.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.btnLoadCorrupt.ForeColor = System.Drawing.Color.OrangeRed;
			this.btnLoadCorrupt.Location = new System.Drawing.Point(5, 284);
			this.btnLoadCorrupt.Name = "btnLoadCorrupt";
			this.btnLoadCorrupt.Size = new System.Drawing.Size(149, 23);
			this.btnLoadCorrupt.TabIndex = 173;
			this.btnLoadCorrupt.TabStop = false;
			this.btnLoadCorrupt.Tag = "color:darker";
			this.btnLoadCorrupt.Text = "Load + Corrupt";
			this.btnLoadCorrupt.UseVisualStyleBackColor = false;
			this.btnLoadCorrupt.Click += new System.EventHandler(this.btnLoadCorrupt_Click);
			// 
			// btnJustCorrupt
			// 
			this.btnJustCorrupt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnJustCorrupt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
			this.btnJustCorrupt.FlatAppearance.BorderSize = 0;
			this.btnJustCorrupt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnJustCorrupt.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.btnJustCorrupt.ForeColor = System.Drawing.Color.OrangeRed;
			this.btnJustCorrupt.Location = new System.Drawing.Point(5, 310);
			this.btnJustCorrupt.Name = "btnJustCorrupt";
			this.btnJustCorrupt.Size = new System.Drawing.Size(149, 23);
			this.btnJustCorrupt.TabIndex = 172;
			this.btnJustCorrupt.TabStop = false;
			this.btnJustCorrupt.Tag = "color:darker";
			this.btnJustCorrupt.Text = "Apply Corruption";
			this.btnJustCorrupt.UseVisualStyleBackColor = false;
			this.btnJustCorrupt.Click += new System.EventHandler(this.btnJustCorrupt_Click);
			// 
			// btnRefreshDomains
			// 
			this.btnRefreshDomains.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.btnRefreshDomains.FlatAppearance.BorderSize = 0;
			this.btnRefreshDomains.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnRefreshDomains.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.btnRefreshDomains.ForeColor = System.Drawing.Color.Black;
			this.btnRefreshDomains.Location = new System.Drawing.Point(5, 248);
			this.btnRefreshDomains.Name = "btnRefreshDomains";
			this.btnRefreshDomains.Size = new System.Drawing.Size(149, 25);
			this.btnRefreshDomains.TabIndex = 161;
			this.btnRefreshDomains.TabStop = false;
			this.btnRefreshDomains.Tag = "color:light";
			this.btnRefreshDomains.Text = "Refresh Domains";
			this.btnRefreshDomains.UseVisualStyleBackColor = false;
			this.btnRefreshDomains.Click += new System.EventHandler(this.btnRefreshDomains_Click);
			// 
			// btnSendTo
			// 
			this.btnSendTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSendTo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
			this.btnSendTo.FlatAppearance.BorderSize = 0;
			this.btnSendTo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSendTo.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.btnSendTo.ForeColor = System.Drawing.Color.OrangeRed;
			this.btnSendTo.Location = new System.Drawing.Point(5, 336);
			this.btnSendTo.Name = "btnSendTo";
			this.btnSendTo.Size = new System.Drawing.Size(149, 23);
			this.btnSendTo.TabIndex = 171;
			this.btnSendTo.TabStop = false;
			this.btnSendTo.Tag = "color:darker";
			this.btnSendTo.Text = "Send To Stash";
			this.btnSendTo.UseVisualStyleBackColor = false;
			this.btnSendTo.Click += new System.EventHandler(this.btnSendTo_Click);
			// 
			// btnAddRow
			// 
			this.btnAddRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.btnAddRow.FlatAppearance.BorderSize = 0;
			this.btnAddRow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnAddRow.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.btnAddRow.ForeColor = System.Drawing.Color.Black;
			this.btnAddRow.Location = new System.Drawing.Point(5, 218);
			this.btnAddRow.Name = "btnAddRow";
			this.btnAddRow.Size = new System.Drawing.Size(149, 25);
			this.btnAddRow.TabIndex = 160;
			this.btnAddRow.TabStop = false;
			this.btnAddRow.Tag = "color:light";
			this.btnAddRow.Text = "Add Row";
			this.btnAddRow.UseVisualStyleBackColor = false;
			this.btnAddRow.Click += new System.EventHandler(this.btnAddRow_Click);
			// 
			// panel2
			// 
			this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.panel2.BackColor = System.Drawing.Color.Gray;
			this.panel2.Controls.Add(this.btnNudgeParam2Up);
			this.panel2.Controls.Add(this.btnNudgeParam2Down);
			this.panel2.Controls.Add(this.updownNudgeParam2);
			this.panel2.Controls.Add(this.btnNudgeParam1Up);
			this.panel2.Controls.Add(this.btnNudgeParam1Down);
			this.panel2.Controls.Add(this.updownNudgeParam1);
			this.panel2.Controls.Add(this.btnNudgeEndAddressUp);
			this.panel2.Controls.Add(this.btnNudgeEndAddressDown);
			this.panel2.Controls.Add(this.updownNudgeEndAddress);
			this.panel2.Controls.Add(this.btnNudgeStartAddressUp);
			this.panel2.Controls.Add(this.btnNudgeStartAddressDown);
			this.panel2.Controls.Add(this.updownNudgeStartAddress);
			this.panel2.Controls.Add(this.label5);
			this.panel2.Controls.Add(this.label3);
			this.panel2.Controls.Add(this.label2);
			this.panel2.Controls.Add(this.label1);
			this.panel2.Location = new System.Drawing.Point(5, 25);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(147, 167);
			this.panel2.TabIndex = 159;
			this.panel2.Tag = "color:normal";
			// 
			// btnNudgeParam2Up
			// 
			this.btnNudgeParam2Up.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnNudgeParam2Up.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.btnNudgeParam2Up.FlatAppearance.BorderSize = 0;
			this.btnNudgeParam2Up.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnNudgeParam2Up.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.btnNudgeParam2Up.ForeColor = System.Drawing.Color.Black;
			this.btnNudgeParam2Up.Location = new System.Drawing.Point(110, 138);
			this.btnNudgeParam2Up.Name = "btnNudgeParam2Up";
			this.btnNudgeParam2Up.Size = new System.Drawing.Size(21, 20);
			this.btnNudgeParam2Up.TabIndex = 167;
			this.btnNudgeParam2Up.TabStop = false;
			this.btnNudgeParam2Up.Tag = "color:light";
			this.btnNudgeParam2Up.Text = "▶";
			this.btnNudgeParam2Up.UseVisualStyleBackColor = false;
			this.btnNudgeParam2Up.Click += new System.EventHandler(this.btnNudgeParam2Up_Click);
			// 
			// btnNudgeParam2Down
			// 
			this.btnNudgeParam2Down.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnNudgeParam2Down.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.btnNudgeParam2Down.FlatAppearance.BorderSize = 0;
			this.btnNudgeParam2Down.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnNudgeParam2Down.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.btnNudgeParam2Down.ForeColor = System.Drawing.Color.Black;
			this.btnNudgeParam2Down.Location = new System.Drawing.Point(18, 140);
			this.btnNudgeParam2Down.Name = "btnNudgeParam2Down";
			this.btnNudgeParam2Down.Size = new System.Drawing.Size(21, 19);
			this.btnNudgeParam2Down.TabIndex = 168;
			this.btnNudgeParam2Down.TabStop = false;
			this.btnNudgeParam2Down.Tag = "color:light";
			this.btnNudgeParam2Down.Text = "◀";
			this.btnNudgeParam2Down.UseVisualStyleBackColor = false;
			this.btnNudgeParam2Down.Click += new System.EventHandler(this.btnNudgeParam2Down_Click);
			// 
			// updownNudgeParam2
			// 
			this.updownNudgeParam2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.updownNudgeParam2.Font = new System.Drawing.Font("Consolas", 8F);
			this.updownNudgeParam2.ForeColor = System.Drawing.Color.White;
			this.updownNudgeParam2.Hexadecimal = true;
			this.updownNudgeParam2.Location = new System.Drawing.Point(45, 138);
			this.updownNudgeParam2.Minimum = new decimal(new int[] {
			1,
			0,
			0,
			0});
			this.updownNudgeParam2.Name = "updownNudgeParam2";
			this.updownNudgeParam2.Size = new System.Drawing.Size(59, 20);
			this.updownNudgeParam2.TabIndex = 166;
			this.updownNudgeParam2.Tag = "color:dark hex";
			this.updownNudgeParam2.Value = new decimal(new int[] {
			1,
			0,
			0,
			0});
			// 
			// btnNudgeParam1Up
			// 
			this.btnNudgeParam1Up.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnNudgeParam1Up.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.btnNudgeParam1Up.FlatAppearance.BorderSize = 0;
			this.btnNudgeParam1Up.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnNudgeParam1Up.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.btnNudgeParam1Up.ForeColor = System.Drawing.Color.Black;
			this.btnNudgeParam1Up.Location = new System.Drawing.Point(110, 99);
			this.btnNudgeParam1Up.Name = "btnNudgeParam1Up";
			this.btnNudgeParam1Up.Size = new System.Drawing.Size(21, 20);
			this.btnNudgeParam1Up.TabIndex = 164;
			this.btnNudgeParam1Up.TabStop = false;
			this.btnNudgeParam1Up.Tag = "color:light";
			this.btnNudgeParam1Up.Text = "▶";
			this.btnNudgeParam1Up.UseVisualStyleBackColor = false;
			this.btnNudgeParam1Up.Click += new System.EventHandler(this.btnNudgeParam1Up_Click);
			// 
			// btnNudgeParam1Down
			// 
			this.btnNudgeParam1Down.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnNudgeParam1Down.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.btnNudgeParam1Down.FlatAppearance.BorderSize = 0;
			this.btnNudgeParam1Down.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnNudgeParam1Down.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.btnNudgeParam1Down.ForeColor = System.Drawing.Color.Black;
			this.btnNudgeParam1Down.Location = new System.Drawing.Point(18, 99);
			this.btnNudgeParam1Down.Name = "btnNudgeParam1Down";
			this.btnNudgeParam1Down.Size = new System.Drawing.Size(21, 20);
			this.btnNudgeParam1Down.TabIndex = 165;
			this.btnNudgeParam1Down.TabStop = false;
			this.btnNudgeParam1Down.Tag = "color:light";
			this.btnNudgeParam1Down.Text = "◀";
			this.btnNudgeParam1Down.UseVisualStyleBackColor = false;
			this.btnNudgeParam1Down.Click += new System.EventHandler(this.btnNudgeParam1Down_Click);
			// 
			// updownNudgeParam1
			// 
			this.updownNudgeParam1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.updownNudgeParam1.Font = new System.Drawing.Font("Consolas", 8F);
			this.updownNudgeParam1.ForeColor = System.Drawing.Color.White;
			this.updownNudgeParam1.Hexadecimal = true;
			this.updownNudgeParam1.Location = new System.Drawing.Point(45, 99);
			this.updownNudgeParam1.Minimum = new decimal(new int[] {
			1,
			0,
			0,
			0});
			this.updownNudgeParam1.Name = "updownNudgeParam1";
			this.updownNudgeParam1.Size = new System.Drawing.Size(59, 20);
			this.updownNudgeParam1.TabIndex = 163;
			this.updownNudgeParam1.Tag = "color:dark hex";
			this.updownNudgeParam1.Value = new decimal(new int[] {
			1,
			0,
			0,
			0});
			// 
			// btnNudgeEndAddressUp
			// 
			this.btnNudgeEndAddressUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnNudgeEndAddressUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.btnNudgeEndAddressUp.FlatAppearance.BorderSize = 0;
			this.btnNudgeEndAddressUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnNudgeEndAddressUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.btnNudgeEndAddressUp.ForeColor = System.Drawing.Color.Black;
			this.btnNudgeEndAddressUp.Location = new System.Drawing.Point(110, 59);
			this.btnNudgeEndAddressUp.Name = "btnNudgeEndAddressUp";
			this.btnNudgeEndAddressUp.Size = new System.Drawing.Size(21, 20);
			this.btnNudgeEndAddressUp.TabIndex = 161;
			this.btnNudgeEndAddressUp.TabStop = false;
			this.btnNudgeEndAddressUp.Tag = "color:light";
			this.btnNudgeEndAddressUp.Text = "▶";
			this.btnNudgeEndAddressUp.UseVisualStyleBackColor = false;
			this.btnNudgeEndAddressUp.Click += new System.EventHandler(this.btnNudgeEndAddressUp_Click);
			// 
			// btnNudgeEndAddressDown
			// 
			this.btnNudgeEndAddressDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnNudgeEndAddressDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.btnNudgeEndAddressDown.FlatAppearance.BorderSize = 0;
			this.btnNudgeEndAddressDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnNudgeEndAddressDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.btnNudgeEndAddressDown.ForeColor = System.Drawing.Color.Black;
			this.btnNudgeEndAddressDown.Location = new System.Drawing.Point(18, 59);
			this.btnNudgeEndAddressDown.Name = "btnNudgeEndAddressDown";
			this.btnNudgeEndAddressDown.Size = new System.Drawing.Size(21, 20);
			this.btnNudgeEndAddressDown.TabIndex = 162;
			this.btnNudgeEndAddressDown.TabStop = false;
			this.btnNudgeEndAddressDown.Tag = "color:light";
			this.btnNudgeEndAddressDown.Text = "◀";
			this.btnNudgeEndAddressDown.UseVisualStyleBackColor = false;
			this.btnNudgeEndAddressDown.Click += new System.EventHandler(this.btnNudgeEndAddressDown_Click);
			// 
			// updownNudgeEndAddress
			// 
			this.updownNudgeEndAddress.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.updownNudgeEndAddress.Font = new System.Drawing.Font("Consolas", 8F);
			this.updownNudgeEndAddress.ForeColor = System.Drawing.Color.White;
			this.updownNudgeEndAddress.Hexadecimal = true;
			this.updownNudgeEndAddress.Location = new System.Drawing.Point(45, 59);
			this.updownNudgeEndAddress.Minimum = new decimal(new int[] {
			1,
			0,
			0,
			0});
			this.updownNudgeEndAddress.Name = "updownNudgeEndAddress";
			this.updownNudgeEndAddress.Size = new System.Drawing.Size(59, 20);
			this.updownNudgeEndAddress.TabIndex = 160;
			this.updownNudgeEndAddress.Tag = "color:dark hex";
			this.updownNudgeEndAddress.Value = new decimal(new int[] {
			1,
			0,
			0,
			0});
			// 
			// btnNudgeStartAddressUp
			// 
			this.btnNudgeStartAddressUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnNudgeStartAddressUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.btnNudgeStartAddressUp.FlatAppearance.BorderSize = 0;
			this.btnNudgeStartAddressUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnNudgeStartAddressUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.btnNudgeStartAddressUp.ForeColor = System.Drawing.Color.Black;
			this.btnNudgeStartAddressUp.Location = new System.Drawing.Point(110, 20);
			this.btnNudgeStartAddressUp.Name = "btnNudgeStartAddressUp";
			this.btnNudgeStartAddressUp.Size = new System.Drawing.Size(21, 20);
			this.btnNudgeStartAddressUp.TabIndex = 158;
			this.btnNudgeStartAddressUp.TabStop = false;
			this.btnNudgeStartAddressUp.Tag = "color:light";
			this.btnNudgeStartAddressUp.Text = "▶";
			this.btnNudgeStartAddressUp.UseVisualStyleBackColor = false;
			this.btnNudgeStartAddressUp.Click += new System.EventHandler(this.btnNudgeStartAddressUp_Click);
			// 
			// btnNudgeStartAddressDown
			// 
			this.btnNudgeStartAddressDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnNudgeStartAddressDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.btnNudgeStartAddressDown.FlatAppearance.BorderSize = 0;
			this.btnNudgeStartAddressDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnNudgeStartAddressDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.btnNudgeStartAddressDown.ForeColor = System.Drawing.Color.Black;
			this.btnNudgeStartAddressDown.Location = new System.Drawing.Point(18, 20);
			this.btnNudgeStartAddressDown.Name = "btnNudgeStartAddressDown";
			this.btnNudgeStartAddressDown.Size = new System.Drawing.Size(21, 20);
			this.btnNudgeStartAddressDown.TabIndex = 159;
			this.btnNudgeStartAddressDown.TabStop = false;
			this.btnNudgeStartAddressDown.Tag = "color:light";
			this.btnNudgeStartAddressDown.Text = "◀";
			this.btnNudgeStartAddressDown.UseVisualStyleBackColor = false;
			this.btnNudgeStartAddressDown.Click += new System.EventHandler(this.btnNudgeStartAddressDown_Click);
			// 
			// updownNudgeStartAddress
			// 
			this.updownNudgeStartAddress.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.updownNudgeStartAddress.Font = new System.Drawing.Font("Consolas", 8F);
			this.updownNudgeStartAddress.ForeColor = System.Drawing.Color.White;
			this.updownNudgeStartAddress.Hexadecimal = true;
			this.updownNudgeStartAddress.Location = new System.Drawing.Point(45, 20);
			this.updownNudgeStartAddress.Minimum = new decimal(new int[] {
			1,
			0,
			0,
			0});
			this.updownNudgeStartAddress.Name = "updownNudgeStartAddress";
			this.updownNudgeStartAddress.Size = new System.Drawing.Size(59, 20);
			this.updownNudgeStartAddress.TabIndex = 157;
			this.updownNudgeStartAddress.Tag = "color:dark hex";
			this.updownNudgeStartAddress.Value = new decimal(new int[] {
			1,
			0,
			0,
			0});
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label5.ForeColor = System.Drawing.Color.White;
			this.label5.Location = new System.Drawing.Point(14, 125);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(47, 13);
			this.label5.TabIndex = 172;
			this.label5.Text = "Param 2";
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label3.ForeColor = System.Drawing.Color.White;
			this.label3.Location = new System.Drawing.Point(14, 85);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(47, 13);
			this.label3.TabIndex = 171;
			this.label3.Text = "Param 1";
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label2.ForeColor = System.Drawing.Color.White;
			this.label2.Location = new System.Drawing.Point(14, 45);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(71, 13);
			this.label2.TabIndex = 170;
			this.label2.Text = "End Address";
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.label1.ForeColor = System.Drawing.Color.White;
			this.label1.Location = new System.Drawing.Point(15, 6);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(75, 13);
			this.label1.TabIndex = 169;
			this.label1.Text = "Start Address";
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Segoe UI", 8F);
			this.label4.ForeColor = System.Drawing.Color.White;
			this.label4.Location = new System.Drawing.Point(2, 4);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(108, 13);
			this.label4.TabIndex = 158;
			this.label4.Text = "Shift Selected Rows";
			// 
			// dgvBlastGenerator
			// 
			this.dgvBlastGenerator.AllowUserToAddRows = false;
			this.dgvBlastGenerator.AllowUserToResizeRows = false;
			dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
			this.dgvBlastGenerator.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
			this.dgvBlastGenerator.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.dgvBlastGenerator.BackgroundColor = System.Drawing.Color.Gray;
			this.dgvBlastGenerator.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
			this.dgvBlastGenerator.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvBlastGenerator.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
			this.dgvBlastProtoReference,
			this.dgvRowDirty,
			this.dgvNoteText,
			this.dgvEnabled,
			this.dgvDomain,
			this.dgvPrecision,
			this.dgvType,
			this.dgvMode,
			this.dgvInterval,
			this.dgvStartAddress,
			this.dgvEndAddress,
			this.dgvParam1,
			this.dgvParam2,
			this.dgvLifetime,
			this.dgvExecuteFrame,
			this.dgvLoop,
			this.dgvSeed,
			this.dgvNoteButton});
			this.dgvBlastGenerator.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgvBlastGenerator.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
			this.dgvBlastGenerator.Location = new System.Drawing.Point(0, 24);
			this.dgvBlastGenerator.Margin = new System.Windows.Forms.Padding(2);
			this.dgvBlastGenerator.Name = "dgvBlastGenerator";
			this.dgvBlastGenerator.RowHeadersVisible = false;
			this.dgvBlastGenerator.RowTemplate.Height = 24;
			this.dgvBlastGenerator.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvBlastGenerator.Size = new System.Drawing.Size(872, 365);
			this.dgvBlastGenerator.TabIndex = 167;
			this.dgvBlastGenerator.Tag = "color:normal hex";
			// 
			// dgvBlastProtoReference
			// 
			this.dgvBlastProtoReference.HeaderText = "dgvBlastProtoReference";
			this.dgvBlastProtoReference.Name = "dgvBlastProtoReference";
			this.dgvBlastProtoReference.Visible = false;
			// 
			// dgvRowDirty
			// 
			this.dgvRowDirty.HeaderText = "dgvRowDirty";
			this.dgvRowDirty.Name = "dgvRowDirty";
			this.dgvRowDirty.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.dgvRowDirty.Visible = false;
			// 
			// dgvNoteText
			// 
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.dgvNoteText.DefaultCellStyle = dataGridViewCellStyle2;
			this.dgvNoteText.HeaderText = "dgvNoteText";
			this.dgvNoteText.Name = "dgvNoteText";
			this.dgvNoteText.Visible = false;
			// 
			// dgvEnabled
			// 
			this.dgvEnabled.FillWeight = 17F;
			this.dgvEnabled.HeaderText = "✔";
			this.dgvEnabled.Name = "dgvEnabled";
			this.dgvEnabled.ToolTipText = "Row enabled";
			this.dgvEnabled.TrueValue = "true";
			// 
			// dgvDomain
			// 
			this.dgvDomain.DropDownWidth = 150;
			this.dgvDomain.FillWeight = 60F;
			this.dgvDomain.HeaderText = "Domain";
			this.dgvDomain.Items.AddRange(new object[] {
			"IF_YOU_SEE_THIS_OPEN_A_BUG_REPORT"});
			this.dgvDomain.MaxDropDownItems = 100;
			this.dgvDomain.Name = "dgvDomain";
			this.dgvDomain.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.dgvDomain.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.dgvDomain.ToolTipText = "The domain to generate your corruption for";
			// 
			// dgvPrecision
			// 
			this.dgvPrecision.FillWeight = 45F;
			this.dgvPrecision.HeaderText = "Precision";
			this.dgvPrecision.Items.AddRange(new object[] {
			"8-bit",
			"16-bit",
			"32-bit"});
			this.dgvPrecision.Name = "dgvPrecision";
			this.dgvPrecision.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.dgvPrecision.ToolTipText = "The size of the generated blast";
			// 
			// dgvType
			// 
			this.dgvType.FillWeight = 55F;
			this.dgvType.HeaderText = "Type";
			this.dgvType.Items.AddRange(new object[] {
			"Value",
			"Store"});
			this.dgvType.Name = "dgvType";
			this.dgvType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.dgvType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.dgvType.ToolTipText = "The BlastUnit type";
			// 
			// dgvMode
			// 
			this.dgvMode.DropDownWidth = 150;
			this.dgvMode.FillWeight = 60F;
			this.dgvMode.HeaderText = "Mode";
			this.dgvMode.Items.AddRange(new object[] {
			"IF_YOU_SEE_THIS_OPEN_A_BUG_REPORT"});
			this.dgvMode.MaxDropDownItems = 100;
			this.dgvMode.Name = "dgvMode";
			this.dgvMode.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.dgvMode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.dgvMode.ToolTipText = "The generation mode. See the ? button on the sidebar for more info.";
			// 
			// dgvInterval
			// 
			this.dgvInterval.FillWeight = 45F;
			this.dgvInterval.HeaderText = "Interval";
			this.dgvInterval.Maximum = new decimal(new int[] {
			-1,
			0,
			0,
			0});
			this.dgvInterval.Minimum = new decimal(new int[] {
			1,
			0,
			0,
			0});
			this.dgvInterval.Name = "dgvInterval";
			this.dgvInterval.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.dgvInterval.ToolTipText = "How many bytes to step in-between generated BlastUnits";
			// 
			// dgvStartAddress
			// 
			dataGridViewCellStyle3.Font = new System.Drawing.Font("Consolas", 9F);
			this.dgvStartAddress.DefaultCellStyle = dataGridViewCellStyle3;
			this.dgvStartAddress.FillWeight = 50F;
			this.dgvStartAddress.HeaderText = "Start Address";
			this.dgvStartAddress.Hexadecimal = true;
			this.dgvStartAddress.Maximum = new decimal(new int[] {
			-1,
			0,
			0,
			0});
			this.dgvStartAddress.Name = "dgvStartAddress";
			this.dgvStartAddress.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.dgvStartAddress.ToolTipText = "The address to start generating from";
			// 
			// dgvEndAddress
			// 
			dataGridViewCellStyle4.Font = new System.Drawing.Font("Consolas", 9F);
			this.dgvEndAddress.DefaultCellStyle = dataGridViewCellStyle4;
			this.dgvEndAddress.FillWeight = 50F;
			this.dgvEndAddress.HeaderText = "End Address";
			this.dgvEndAddress.Hexadecimal = true;
			this.dgvEndAddress.Maximum = new decimal(new int[] {
			-1,
			0,
			0,
			0});
			this.dgvEndAddress.Name = "dgvEndAddress";
			this.dgvEndAddress.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.dgvEndAddress.ToolTipText = "The address to end generation at (exclusive)";
			// 
			// dgvParam1
			// 
			dataGridViewCellStyle5.Font = new System.Drawing.Font("Consolas", 9F);
			this.dgvParam1.DefaultCellStyle = dataGridViewCellStyle5;
			this.dgvParam1.FillWeight = 50F;
			this.dgvParam1.HeaderText = "Param 1";
			this.dgvParam1.Hexadecimal = true;
			this.dgvParam1.Maximum = new decimal(new int[] {
			-1,
			0,
			0,
			0});
			this.dgvParam1.Name = "dgvParam1";
			this.dgvParam1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.dgvParam1.ToolTipText = "See the ? button on the right for more details.";
			// 
			// dgvParam2
			// 
			dataGridViewCellStyle6.Font = new System.Drawing.Font("Consolas", 9F);
			this.dgvParam2.DefaultCellStyle = dataGridViewCellStyle6;
			this.dgvParam2.FillWeight = 50F;
			this.dgvParam2.HeaderText = "Param 2";
			this.dgvParam2.Hexadecimal = true;
			this.dgvParam2.Maximum = new decimal(new int[] {
			-1,
			0,
			0,
			0});
			this.dgvParam2.Name = "dgvParam2";
			this.dgvParam2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.dgvParam2.ToolTipText = "See the ? button on the right for more details.";
			// 
			// dgvLifetime
			// 
			this.dgvLifetime.FillWeight = 45F;
			this.dgvLifetime.HeaderText = "Lifetime";
			this.dgvLifetime.Name = "dgvLifetime";
			// 
			// dgvExecuteFrame
			// 
			this.dgvExecuteFrame.FillWeight = 45F;
			this.dgvExecuteFrame.HeaderText = "Execute Frame";
			this.dgvExecuteFrame.Name = "dgvExecuteFrame";
			// 
			// dgvLoop
			// 
			this.dgvLoop.FillWeight = 30F;
			this.dgvLoop.HeaderText = "Loop";
			this.dgvLoop.Name = "dgvLoop";
			// 
			// dgvSeed
			// 
			this.dgvSeed.FillWeight = 50F;
			this.dgvSeed.HeaderText = "Seed";
			this.dgvSeed.Maximum = new decimal(new int[] {
			2147483647,
			0,
			0,
			0});
			this.dgvSeed.Minimum = new decimal(new int[] {
			-2147483648,
			0,
			0,
			-2147483648});
			this.dgvSeed.Name = "dgvSeed";
			// 
			// dgvNoteButton
			// 
			this.dgvNoteButton.FillWeight = 15F;
			this.dgvNoteButton.HeaderText = "📝";
			this.dgvNoteButton.MinimumWidth = 15;
			this.dgvNoteButton.Name = "dgvNoteButton";
			this.dgvNoteButton.ToolTipText = "Tells you if the row has a note.";
			// 
			// dataGridViewTextBoxColumn1
			// 
			this.dataGridViewTextBoxColumn1.FillWeight = 55F;
			this.dataGridViewTextBoxColumn1.HeaderText = "Mode";
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn1.Width = 92;
			// 
			// dataGridViewNumericUpDownColumn1
			// 
			this.dataGridViewNumericUpDownColumn1.FillWeight = 50F;
			this.dataGridViewNumericUpDownColumn1.HeaderText = "Step Size";
			this.dataGridViewNumericUpDownColumn1.Maximum = new decimal(new int[] {
			-1,
			0,
			0,
			0});
			this.dataGridViewNumericUpDownColumn1.Minimum = new decimal(new int[] {
			1,
			0,
			0,
			0});
			this.dataGridViewNumericUpDownColumn1.Name = "dataGridViewNumericUpDownColumn1";
			this.dataGridViewNumericUpDownColumn1.Width = 82;
			// 
			// dataGridViewNumericUpDownColumn2
			// 
			this.dataGridViewNumericUpDownColumn2.FillWeight = 50F;
			this.dataGridViewNumericUpDownColumn2.HeaderText = "Start Address";
			this.dataGridViewNumericUpDownColumn2.Hexadecimal = true;
			this.dataGridViewNumericUpDownColumn2.Maximum = new decimal(new int[] {
			-1,
			0,
			0,
			0});
			this.dataGridViewNumericUpDownColumn2.Name = "dataGridViewNumericUpDownColumn2";
			this.dataGridViewNumericUpDownColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.dataGridViewNumericUpDownColumn2.Width = 83;
			// 
			// dataGridViewNumericUpDownColumn3
			// 
			this.dataGridViewNumericUpDownColumn3.FillWeight = 50F;
			this.dataGridViewNumericUpDownColumn3.HeaderText = "End Address";
			this.dataGridViewNumericUpDownColumn3.Hexadecimal = true;
			this.dataGridViewNumericUpDownColumn3.Maximum = new decimal(new int[] {
			-1,
			0,
			0,
			0});
			this.dataGridViewNumericUpDownColumn3.Name = "dataGridViewNumericUpDownColumn3";
			this.dataGridViewNumericUpDownColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.dataGridViewNumericUpDownColumn3.Width = 83;
			// 
			// dataGridViewNumericUpDownColumn4
			// 
			this.dataGridViewNumericUpDownColumn4.FillWeight = 50F;
			this.dataGridViewNumericUpDownColumn4.HeaderText = "Param 1";
			this.dataGridViewNumericUpDownColumn4.Hexadecimal = true;
			this.dataGridViewNumericUpDownColumn4.Maximum = new decimal(new int[] {
			-1,
			0,
			0,
			0});
			this.dataGridViewNumericUpDownColumn4.Name = "dataGridViewNumericUpDownColumn4";
			this.dataGridViewNumericUpDownColumn4.Width = 83;
			// 
			// dataGridViewNumericUpDownColumn5
			// 
			this.dataGridViewNumericUpDownColumn5.FillWeight = 50F;
			this.dataGridViewNumericUpDownColumn5.HeaderText = "Param 2";
			this.dataGridViewNumericUpDownColumn5.Hexadecimal = true;
			this.dataGridViewNumericUpDownColumn5.Maximum = new decimal(new int[] {
			-1,
			0,
			0,
			0});
			this.dataGridViewNumericUpDownColumn5.Name = "dataGridViewNumericUpDownColumn5";
			this.dataGridViewNumericUpDownColumn5.Width = 83;
			// 
			// menuStripEx1
			// 
			this.menuStripEx1.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.menuStripEx1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.blastLayerToolStripMenuItem});
			this.menuStripEx1.Location = new System.Drawing.Point(0, 0);
			this.menuStripEx1.Name = "menuStripEx1";
			this.menuStripEx1.Size = new System.Drawing.Size(1031, 24);
			this.menuStripEx1.TabIndex = 168;
			this.menuStripEx1.Tag = "";
			this.menuStripEx1.Text = "menuStripEx1";
			// 
			// blastLayerToolStripMenuItem
			// 
			this.blastLayerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.saveAsToFileblToolStripMenuItem,
			this.loadFromFileblToolStripMenuItem,
			this.importBlastlayerblToolStripMenuItem});
			this.blastLayerToolStripMenuItem.Name = "blastLayerToolStripMenuItem";
			this.blastLayerToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.blastLayerToolStripMenuItem.Tag = "";
			this.blastLayerToolStripMenuItem.Text = "File";
			// 
			// saveAsToFileblToolStripMenuItem
			// 
			this.saveAsToFileblToolStripMenuItem.Name = "saveAsToFileblToolStripMenuItem";
			this.saveAsToFileblToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
			this.saveAsToFileblToolStripMenuItem.Text = "&Save As to File (.bg)";
			this.saveAsToFileblToolStripMenuItem.Click += new System.EventHandler(this.saveAsToFileblToolStripMenuItem_Click);
			// 
			// loadFromFileblToolStripMenuItem
			// 
			this.loadFromFileblToolStripMenuItem.Name = "loadFromFileblToolStripMenuItem";
			this.loadFromFileblToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
			this.loadFromFileblToolStripMenuItem.Text = "&Load From File (.bg)";
			this.loadFromFileblToolStripMenuItem.Click += new System.EventHandler(this.loadFromFileblToolStripMenuItem_Click);
			// 
			// importBlastlayerblToolStripMenuItem
			// 
			this.importBlastlayerblToolStripMenuItem.Name = "importBlastlayerblToolStripMenuItem";
			this.importBlastlayerblToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
			this.importBlastlayerblToolStripMenuItem.Text = "&Import Generation Params (.bg)";
			this.importBlastlayerblToolStripMenuItem.Click += new System.EventHandler(this.importBlastlayerblToolStripMenuItem_Click);
			// 
			// btnHideSidebar
			// 
			this.btnHideSidebar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnHideSidebar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.btnHideSidebar.Location = new System.Drawing.Point(1004, 0);
			this.btnHideSidebar.Name = "btnHideSidebar";
			this.btnHideSidebar.Size = new System.Drawing.Size(27, 23);
			this.btnHideSidebar.TabIndex = 170;
			this.btnHideSidebar.Text = "▶";
			this.btnHideSidebar.UseVisualStyleBackColor = true;
			this.btnHideSidebar.Click += new System.EventHandler(this.btnHideSidebar_Click);
			// 
			// RTC_BlastGenerator_Form
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.ClientSize = new System.Drawing.Size(1031, 389);
			this.Controls.Add(this.btnHideSidebar);
			this.Controls.Add(this.dgvBlastGenerator);
			this.Controls.Add(this.panelSidebar);
			this.Controls.Add(this.menuStripEx1);
			this.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStripEx1;
			this.MinimumSize = new System.Drawing.Size(18, 428);
			this.Name = "RTC_BlastGenerator_Form";
			this.Tag = "color:dark";
			this.Text = "Blast Generator";
			this.Load += new System.EventHandler(this.RTC_BlastGeneratorForm_Load);
			this.panelSidebar.ResumeLayout(false);
			this.panelSidebar.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.updownNudgeParam2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.updownNudgeParam1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.updownNudgeEndAddress)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.updownNudgeStartAddress)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dgvBlastGenerator)).EndInit();
			this.menuStripEx1.ResumeLayout(false);
			this.menuStripEx1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Panel panelSidebar;
		public System.Windows.Forms.DataGridView dgvBlastGenerator;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button btnNudgeParam2Up;
		private System.Windows.Forms.Button btnNudgeParam2Down;
		private NumericUpDownHexFix updownNudgeParam2;
		private System.Windows.Forms.Button btnNudgeParam1Up;
		private System.Windows.Forms.Button btnNudgeParam1Down;
		private NumericUpDownHexFix updownNudgeParam1;
		private System.Windows.Forms.Button btnNudgeEndAddressUp;
		private System.Windows.Forms.Button btnNudgeEndAddressDown;
		private NumericUpDownHexFix updownNudgeEndAddress;
		private System.Windows.Forms.Button btnNudgeStartAddressUp;
		private System.Windows.Forms.Button btnNudgeStartAddressDown;
		private NumericUpDownHexFix updownNudgeStartAddress;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ToolStripMenuItem blastLayerToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem loadFromFileblToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveAsToFileblToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem importBlastlayerblToolStripMenuItem;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private DataGridViewNumericUpDownColumn dataGridViewNumericUpDownColumn1;
		private DataGridViewNumericUpDownColumn dataGridViewNumericUpDownColumn2;
		private DataGridViewNumericUpDownColumn dataGridViewNumericUpDownColumn3;
		private DataGridViewNumericUpDownColumn dataGridViewNumericUpDownColumn4;
		private DataGridViewNumericUpDownColumn dataGridViewNumericUpDownColumn5;
		private System.Windows.Forms.Button btnAddRow;
		private System.Windows.Forms.Button btnHideSidebar;
		private System.Windows.Forms.MenuStrip menuStripEx1;
		private System.Windows.Forms.Button btnRefreshDomains;
		private System.Windows.Forms.Button btnLoadCorrupt;
		private System.Windows.Forms.Button btnJustCorrupt;
		private System.Windows.Forms.Button btnSendTo;
		private System.Windows.Forms.Button btnHelp;
		private System.Windows.Forms.CheckBox cbUnitsShareNote;
		private System.Windows.Forms.DataGridViewTextBoxColumn dgvBlastProtoReference;
		private System.Windows.Forms.DataGridViewTextBoxColumn dgvRowDirty;
		private System.Windows.Forms.DataGridViewTextBoxColumn dgvNoteText;
		private System.Windows.Forms.DataGridViewCheckBoxColumn dgvEnabled;
		private System.Windows.Forms.DataGridViewComboBoxColumn dgvDomain;
		private System.Windows.Forms.DataGridViewComboBoxColumn dgvPrecision;
		private System.Windows.Forms.DataGridViewComboBoxColumn dgvType;
		private System.Windows.Forms.DataGridViewComboBoxColumn dgvMode;
		private DataGridViewNumericUpDownColumn dgvInterval;
		private DataGridViewNumericUpDownColumn dgvStartAddress;
		private DataGridViewNumericUpDownColumn dgvEndAddress;
		private DataGridViewNumericUpDownColumn dgvParam1;
		private DataGridViewNumericUpDownColumn dgvParam2;
		private DataGridViewNumericUpDownColumn dgvLifetime;
		private DataGridViewNumericUpDownColumn dgvExecuteFrame;
		private System.Windows.Forms.DataGridViewCheckBoxColumn dgvLoop;
		private DataGridViewNumericUpDownColumn dgvSeed;
		private System.Windows.Forms.DataGridViewButtonColumn dgvNoteButton;
	}
}