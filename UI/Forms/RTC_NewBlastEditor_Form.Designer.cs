using System.Windows.Forms;
using RTCV.UI;

namespace RTCV.UI
{
	partial class RTC_NewBlastEditor_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RTC_NewBlastEditor_Form));
            this.dgvBlastEditor = new System.Windows.Forms.DataGridView();
            this.panelSidebar = new System.Windows.Forms.Panel();
            this.btnAddRow = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbShiftBlastlayer = new System.Windows.Forms.ComboBox();
            this.btnShiftBlastLayerDown = new System.Windows.Forms.Button();
            this.btnShiftBlastLayerUp = new System.Windows.Forms.Button();
            this.updownShiftBlastLayerAmount = new RTCV.UI.NumericUpDownHexFix();
            this.pnMemoryTargetting = new System.Windows.Forms.Panel();
            this.lbBlastLayerSize = new System.Windows.Forms.Label();
            this.btnHelp = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnLoadCorrupt = new System.Windows.Forms.Button();
            this.btnRemoveSelected = new System.Windows.Forms.Button();
            this.btnCorrupt = new System.Windows.Forms.Button();
            this.btnSendToStash = new System.Windows.Forms.Button();
            this.btnRemoveDisabled = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.btnDisable50 = new System.Windows.Forms.Button();
            this.btnInvertDisabled = new System.Windows.Forms.Button();
            this.btnDisableEverything = new System.Windows.Forms.Button();
            this.btnEnableEverything = new System.Windows.Forms.Button();
            this.btnDuplicateSelected = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cbFilterColumn = new System.Windows.Forms.ComboBox();
            this.tbFilter = new System.Windows.Forms.TextBox();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.panel9 = new System.Windows.Forms.Panel();
            this.label16 = new System.Windows.Forms.Label();
            this.upDownLifetime = new RTCV.UI.NumericUpDownHexFix();
            this.label1 = new System.Windows.Forms.Label();
            this.upDownExecuteFrame = new RTCV.UI.NumericUpDownHexFix();
            this.cbLoop = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btnNote = new System.Windows.Forms.Button();
            this.panel8 = new System.Windows.Forms.Panel();
            this.cbStoreLimiterSource = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.cbInvertLimiter = new System.Windows.Forms.CheckBox();
            this.cbLimiterTime = new System.Windows.Forms.ComboBox();
            this.label18 = new System.Windows.Forms.Label();
            this.cbLimiterList = new System.Windows.Forms.ComboBox();
            this.label19 = new System.Windows.Forms.Label();
            this.tbTiltValue = new RTCV.UI.UI_Extensions.NumericTextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.cbSource = new System.Windows.Forms.ComboBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.tbValue = new RTCV.UI.UI_Extensions.HexTextBox();
            this.panel7 = new System.Windows.Forms.Panel();
            this.cbSourceDomain = new System.Windows.Forms.ComboBox();
            this.upDownSourceAddress = new RTCV.UI.NumericUpDownHexFix();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.cbStoreTime = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cbStoreType = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.upDownPrecision = new RTCV.UI.NumericUpDownHexFix();
            this.label9 = new System.Windows.Forms.Label();
            this.upDownAddress = new RTCV.UI.NumericUpDownHexFix();
            this.label8 = new System.Windows.Forms.Label();
            this.cbDomain = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.cbBigEndian = new System.Windows.Forms.CheckBox();
            this.cbLocked = new System.Windows.Forms.CheckBox();
            this.cbEnabled = new System.Windows.Forms.CheckBox();
            this.menuStripEx1 = new System.Windows.Forms.MenuStrip();
            this.blastLayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadFromFileblToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToFileblToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToFileblToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importBlastlayerblToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToCSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveStateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runOriginalSavestateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceSavestateFromGHToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceSavestateFromFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveSavestateToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rOMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runRomWithoutBlastlayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceRomFromGHToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceRomFromFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bakeROMBlastunitsToFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sanitizeDuplicatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rasterizeVMDsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bakeBlastunitsToVALUEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openBlastGeneratorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBlastEditor)).BeginInit();
            this.panelSidebar.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updownShiftBlastLayerAmount)).BeginInit();
            this.pnMemoryTargetting.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.panel9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.upDownLifetime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.upDownExecuteFrame)).BeginInit();
            this.panel5.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.upDownSourceAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.upDownPrecision)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.upDownAddress)).BeginInit();
            this.panel4.SuspendLayout();
            this.menuStripEx1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvBlastEditor
            // 
            this.dgvBlastEditor.AllowUserToAddRows = false;
            this.dgvBlastEditor.AllowUserToResizeRows = false;
            this.dgvBlastEditor.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvBlastEditor.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvBlastEditor.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dgvBlastEditor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBlastEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBlastEditor.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.dgvBlastEditor.Location = new System.Drawing.Point(0, 24);
            this.dgvBlastEditor.Margin = new System.Windows.Forms.Padding(2);
            this.dgvBlastEditor.Name = "dgvBlastEditor";
            this.dgvBlastEditor.RowHeadersVisible = false;
            this.dgvBlastEditor.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
            this.dgvBlastEditor.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBlastEditor.Size = new System.Drawing.Size(662, 221);
            this.dgvBlastEditor.TabIndex = 0;
            this.dgvBlastEditor.Tag = "color:normal";
            // 
            // panelSidebar
            // 
            this.panelSidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panelSidebar.Controls.Add(this.btnAddRow);
            this.panelSidebar.Controls.Add(this.panel1);
            this.panelSidebar.Controls.Add(this.pnMemoryTargetting);
            this.panelSidebar.Controls.Add(this.btnHelp);
            this.panelSidebar.Controls.Add(this.label3);
            this.panelSidebar.Controls.Add(this.btnLoadCorrupt);
            this.panelSidebar.Controls.Add(this.btnRemoveSelected);
            this.panelSidebar.Controls.Add(this.btnCorrupt);
            this.panelSidebar.Controls.Add(this.btnSendToStash);
            this.panelSidebar.Controls.Add(this.btnRemoveDisabled);
            this.panelSidebar.Controls.Add(this.label4);
            this.panelSidebar.Controls.Add(this.btnDisable50);
            this.panelSidebar.Controls.Add(this.btnInvertDisabled);
            this.panelSidebar.Controls.Add(this.btnDisableEverything);
            this.panelSidebar.Controls.Add(this.btnEnableEverything);
            this.panelSidebar.Controls.Add(this.btnDuplicateSelected);
            this.panelSidebar.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelSidebar.Location = new System.Drawing.Point(662, 24);
            this.panelSidebar.Name = "panelSidebar";
            this.panelSidebar.Size = new System.Drawing.Size(159, 463);
            this.panelSidebar.TabIndex = 146;
            this.panelSidebar.Tag = "color:dark";
            // 
            // btnAddRow
            // 
            this.btnAddRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnAddRow.FlatAppearance.BorderSize = 0;
            this.btnAddRow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddRow.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnAddRow.ForeColor = System.Drawing.Color.Black;
            this.btnAddRow.Location = new System.Drawing.Point(14, 330);
            this.btnAddRow.Name = "btnAddRow";
            this.btnAddRow.Size = new System.Drawing.Size(135, 25);
            this.btnAddRow.TabIndex = 177;
            this.btnAddRow.TabStop = false;
            this.btnAddRow.Tag = "color:light";
            this.btnAddRow.Text = "Add New Row";
            this.btnAddRow.UseVisualStyleBackColor = false;
            this.btnAddRow.Click += new System.EventHandler(this.BtnAddRow_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.Gray;
            this.panel1.Controls.Add(this.cbShiftBlastlayer);
            this.panel1.Controls.Add(this.btnShiftBlastLayerDown);
            this.panel1.Controls.Add(this.btnShiftBlastLayerUp);
            this.panel1.Controls.Add(this.updownShiftBlastLayerAmount);
            this.panel1.Location = new System.Drawing.Point(14, 67);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(136, 60);
            this.panel1.TabIndex = 137;
            this.panel1.Tag = "color:lighter";
            // 
            // cbShiftBlastlayer
            // 
            this.cbShiftBlastlayer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cbShiftBlastlayer.ForeColor = System.Drawing.Color.White;
            this.cbShiftBlastlayer.FormattingEnabled = true;
            this.cbShiftBlastlayer.Location = new System.Drawing.Point(11, 7);
            this.cbShiftBlastlayer.Name = "cbShiftBlastlayer";
            this.cbShiftBlastlayer.Size = new System.Drawing.Size(114, 21);
            this.cbShiftBlastlayer.TabIndex = 148;
            this.cbShiftBlastlayer.Tag = "color:dark";
            // 
            // btnShiftBlastLayerDown
            // 
            this.btnShiftBlastLayerDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnShiftBlastLayerDown.FlatAppearance.BorderSize = 0;
            this.btnShiftBlastLayerDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShiftBlastLayerDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnShiftBlastLayerDown.ForeColor = System.Drawing.Color.Black;
            this.btnShiftBlastLayerDown.Location = new System.Drawing.Point(11, 34);
            this.btnShiftBlastLayerDown.Name = "btnShiftBlastLayerDown";
            this.btnShiftBlastLayerDown.Size = new System.Drawing.Size(21, 21);
            this.btnShiftBlastLayerDown.TabIndex = 147;
            this.btnShiftBlastLayerDown.TabStop = false;
            this.btnShiftBlastLayerDown.Tag = "color:light";
            this.btnShiftBlastLayerDown.Text = "◀";
            this.btnShiftBlastLayerDown.UseVisualStyleBackColor = false;
            this.btnShiftBlastLayerDown.Click += new System.EventHandler(this.btnShiftBlastLayerDown_Click);
            // 
            // btnShiftBlastLayerUp
            // 
            this.btnShiftBlastLayerUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnShiftBlastLayerUp.FlatAppearance.BorderSize = 0;
            this.btnShiftBlastLayerUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShiftBlastLayerUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnShiftBlastLayerUp.ForeColor = System.Drawing.Color.Black;
            this.btnShiftBlastLayerUp.Location = new System.Drawing.Point(103, 33);
            this.btnShiftBlastLayerUp.Name = "btnShiftBlastLayerUp";
            this.btnShiftBlastLayerUp.Size = new System.Drawing.Size(22, 22);
            this.btnShiftBlastLayerUp.TabIndex = 146;
            this.btnShiftBlastLayerUp.TabStop = false;
            this.btnShiftBlastLayerUp.Tag = "color:light";
            this.btnShiftBlastLayerUp.Text = "▶";
            this.btnShiftBlastLayerUp.UseVisualStyleBackColor = false;
            this.btnShiftBlastLayerUp.Click += new System.EventHandler(this.btnShiftBlastLayerUp_Click);
            // 
            // updownShiftBlastLayerAmount
            // 
            this.updownShiftBlastLayerAmount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.updownShiftBlastLayerAmount.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.updownShiftBlastLayerAmount.ForeColor = System.Drawing.Color.White;
            this.updownShiftBlastLayerAmount.Hexadecimal = true;
            this.updownShiftBlastLayerAmount.Location = new System.Drawing.Point(38, 33);
            this.updownShiftBlastLayerAmount.Name = "updownShiftBlastLayerAmount";
            this.updownShiftBlastLayerAmount.Size = new System.Drawing.Size(59, 22);
            this.updownShiftBlastLayerAmount.TabIndex = 145;
            this.updownShiftBlastLayerAmount.Tag = "color:dark";
            // 
            // pnMemoryTargetting
            // 
            this.pnMemoryTargetting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnMemoryTargetting.BackColor = System.Drawing.Color.Gray;
            this.pnMemoryTargetting.Controls.Add(this.lbBlastLayerSize);
            this.pnMemoryTargetting.Location = new System.Drawing.Point(14, 22);
            this.pnMemoryTargetting.Name = "pnMemoryTargetting";
            this.pnMemoryTargetting.Size = new System.Drawing.Size(135, 24);
            this.pnMemoryTargetting.TabIndex = 134;
            this.pnMemoryTargetting.Tag = "color:lighter";
            // 
            // lbBlastLayerSize
            // 
            this.lbBlastLayerSize.ForeColor = System.Drawing.Color.White;
            this.lbBlastLayerSize.Location = new System.Drawing.Point(5, 5);
            this.lbBlastLayerSize.Name = "lbBlastLayerSize";
            this.lbBlastLayerSize.Size = new System.Drawing.Size(120, 19);
            this.lbBlastLayerSize.TabIndex = 132;
            this.lbBlastLayerSize.Text = "Layer size:";
            // 
            // btnHelp
            // 
            this.btnHelp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnHelp.FlatAppearance.BorderSize = 0;
            this.btnHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHelp.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnHelp.ForeColor = System.Drawing.Color.Black;
            this.btnHelp.Image = ((System.Drawing.Image)(resources.GetObject("btnHelp.Image")));
            this.btnHelp.Location = new System.Drawing.Point(127, 2);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(27, 17);
            this.btnHelp.TabIndex = 176;
            this.btnHelp.TabStop = false;
            this.btnHelp.Tag = "color:dark";
            this.btnHelp.UseVisualStyleBackColor = false;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(11, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 13);
            this.label3.TabIndex = 135;
            this.label3.Text = "BlastLayer Info";
            // 
            // btnLoadCorrupt
            // 
            this.btnLoadCorrupt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLoadCorrupt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnLoadCorrupt.FlatAppearance.BorderSize = 0;
            this.btnLoadCorrupt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadCorrupt.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnLoadCorrupt.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnLoadCorrupt.Location = new System.Drawing.Point(14, 380);
            this.btnLoadCorrupt.Name = "btnLoadCorrupt";
            this.btnLoadCorrupt.Size = new System.Drawing.Size(135, 25);
            this.btnLoadCorrupt.TabIndex = 14;
            this.btnLoadCorrupt.TabStop = false;
            this.btnLoadCorrupt.Tag = "color:darker";
            this.btnLoadCorrupt.Text = "Load + Corrupt";
            this.btnLoadCorrupt.UseVisualStyleBackColor = false;
            this.btnLoadCorrupt.Click += new System.EventHandler(this.btnLoadCorrupt_Click);
            // 
            // btnRemoveSelected
            // 
            this.btnRemoveSelected.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnRemoveSelected.FlatAppearance.BorderSize = 0;
            this.btnRemoveSelected.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveSelected.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnRemoveSelected.ForeColor = System.Drawing.Color.Black;
            this.btnRemoveSelected.Location = new System.Drawing.Point(14, 273);
            this.btnRemoveSelected.Name = "btnRemoveSelected";
            this.btnRemoveSelected.Size = new System.Drawing.Size(135, 25);
            this.btnRemoveSelected.TabIndex = 139;
            this.btnRemoveSelected.TabStop = false;
            this.btnRemoveSelected.Tag = "color:light";
            this.btnRemoveSelected.Text = "Remove Selected";
            this.btnRemoveSelected.UseVisualStyleBackColor = false;
            this.btnRemoveSelected.Click += new System.EventHandler(this.btnRemoveSelected_Click);
            // 
            // btnCorrupt
            // 
            this.btnCorrupt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCorrupt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnCorrupt.FlatAppearance.BorderSize = 0;
            this.btnCorrupt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCorrupt.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnCorrupt.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnCorrupt.Location = new System.Drawing.Point(14, 406);
            this.btnCorrupt.Name = "btnCorrupt";
            this.btnCorrupt.Size = new System.Drawing.Size(135, 25);
            this.btnCorrupt.TabIndex = 13;
            this.btnCorrupt.TabStop = false;
            this.btnCorrupt.Tag = "color:darker";
            this.btnCorrupt.Text = "Apply Corruption";
            this.btnCorrupt.UseVisualStyleBackColor = false;
            this.btnCorrupt.Click += new System.EventHandler(this.btnCorrupt_Click);
            // 
            // btnSendToStash
            // 
            this.btnSendToStash.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSendToStash.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnSendToStash.FlatAppearance.BorderSize = 0;
            this.btnSendToStash.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSendToStash.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnSendToStash.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnSendToStash.Location = new System.Drawing.Point(14, 432);
            this.btnSendToStash.Name = "btnSendToStash";
            this.btnSendToStash.Size = new System.Drawing.Size(135, 25);
            this.btnSendToStash.TabIndex = 12;
            this.btnSendToStash.TabStop = false;
            this.btnSendToStash.Tag = "color:darker";
            this.btnSendToStash.Text = "Send To Stash";
            this.btnSendToStash.UseVisualStyleBackColor = false;
            this.btnSendToStash.Click += new System.EventHandler(this.btnSendToStash_Click);
            // 
            // btnRemoveDisabled
            // 
            this.btnRemoveDisabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnRemoveDisabled.FlatAppearance.BorderSize = 0;
            this.btnRemoveDisabled.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveDisabled.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnRemoveDisabled.ForeColor = System.Drawing.Color.Black;
            this.btnRemoveDisabled.Location = new System.Drawing.Point(14, 186);
            this.btnRemoveDisabled.Name = "btnRemoveDisabled";
            this.btnRemoveDisabled.Size = new System.Drawing.Size(135, 25);
            this.btnRemoveDisabled.TabIndex = 115;
            this.btnRemoveDisabled.TabStop = false;
            this.btnRemoveDisabled.Tag = "color:light";
            this.btnRemoveDisabled.Text = "Remove Disabled";
            this.btnRemoveDisabled.UseVisualStyleBackColor = false;
            this.btnRemoveDisabled.Click += new System.EventHandler(this.btnRemoveDisabled_Click);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(11, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(108, 13);
            this.label4.TabIndex = 136;
            this.label4.Text = "Shift Selected Rows";
            // 
            // btnDisable50
            // 
            this.btnDisable50.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnDisable50.FlatAppearance.BorderSize = 0;
            this.btnDisable50.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDisable50.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnDisable50.ForeColor = System.Drawing.Color.Black;
            this.btnDisable50.Location = new System.Drawing.Point(14, 134);
            this.btnDisable50.Name = "btnDisable50";
            this.btnDisable50.Size = new System.Drawing.Size(135, 25);
            this.btnDisable50.TabIndex = 114;
            this.btnDisable50.TabStop = false;
            this.btnDisable50.Tag = "color:light";
            this.btnDisable50.Text = "Disable 50%";
            this.btnDisable50.UseVisualStyleBackColor = false;
            this.btnDisable50.Click += new System.EventHandler(this.btnDisable50_Click);
            // 
            // btnInvertDisabled
            // 
            this.btnInvertDisabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnInvertDisabled.FlatAppearance.BorderSize = 0;
            this.btnInvertDisabled.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInvertDisabled.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnInvertDisabled.ForeColor = System.Drawing.Color.Black;
            this.btnInvertDisabled.Location = new System.Drawing.Point(14, 160);
            this.btnInvertDisabled.Name = "btnInvertDisabled";
            this.btnInvertDisabled.Size = new System.Drawing.Size(135, 25);
            this.btnInvertDisabled.TabIndex = 116;
            this.btnInvertDisabled.TabStop = false;
            this.btnInvertDisabled.Tag = "color:light";
            this.btnInvertDisabled.Text = "Invert Disabled";
            this.btnInvertDisabled.UseVisualStyleBackColor = false;
            this.btnInvertDisabled.Click += new System.EventHandler(this.btnInvertDisabled_Click);
            // 
            // btnDisableEverything
            // 
            this.btnDisableEverything.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnDisableEverything.FlatAppearance.BorderSize = 0;
            this.btnDisableEverything.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDisableEverything.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnDisableEverything.ForeColor = System.Drawing.Color.Black;
            this.btnDisableEverything.Location = new System.Drawing.Point(14, 216);
            this.btnDisableEverything.Name = "btnDisableEverything";
            this.btnDisableEverything.Size = new System.Drawing.Size(135, 25);
            this.btnDisableEverything.TabIndex = 128;
            this.btnDisableEverything.TabStop = false;
            this.btnDisableEverything.Tag = "color:light";
            this.btnDisableEverything.Text = "Disable Everything";
            this.btnDisableEverything.UseVisualStyleBackColor = false;
            this.btnDisableEverything.Click += new System.EventHandler(this.btnDisableEverything_Click);
            // 
            // btnEnableEverything
            // 
            this.btnEnableEverything.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnEnableEverything.FlatAppearance.BorderSize = 0;
            this.btnEnableEverything.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnableEverything.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnEnableEverything.ForeColor = System.Drawing.Color.Black;
            this.btnEnableEverything.Location = new System.Drawing.Point(14, 242);
            this.btnEnableEverything.Name = "btnEnableEverything";
            this.btnEnableEverything.Size = new System.Drawing.Size(135, 25);
            this.btnEnableEverything.TabIndex = 129;
            this.btnEnableEverything.TabStop = false;
            this.btnEnableEverything.Tag = "color:light";
            this.btnEnableEverything.Text = "Enable Everything";
            this.btnEnableEverything.UseVisualStyleBackColor = false;
            this.btnEnableEverything.Click += new System.EventHandler(this.btnEnableEverything_Click);
            // 
            // btnDuplicateSelected
            // 
            this.btnDuplicateSelected.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnDuplicateSelected.FlatAppearance.BorderSize = 0;
            this.btnDuplicateSelected.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDuplicateSelected.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnDuplicateSelected.ForeColor = System.Drawing.Color.Black;
            this.btnDuplicateSelected.Location = new System.Drawing.Point(14, 303);
            this.btnDuplicateSelected.Name = "btnDuplicateSelected";
            this.btnDuplicateSelected.Size = new System.Drawing.Size(135, 25);
            this.btnDuplicateSelected.TabIndex = 130;
            this.btnDuplicateSelected.TabStop = false;
            this.btnDuplicateSelected.Tag = "color:light";
            this.btnDuplicateSelected.Text = "Duplicate Selected";
            this.btnDuplicateSelected.UseVisualStyleBackColor = false;
            this.btnDuplicateSelected.Click += new System.EventHandler(this.btnDuplicateSelected_Click);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.cbFilterColumn);
            this.panel2.Controls.Add(this.tbFilter);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 245);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(662, 21);
            this.panel2.TabIndex = 148;
            this.panel2.Tag = "color:light";
            // 
            // cbFilterColumn
            // 
            this.cbFilterColumn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbFilterColumn.BackColor = System.Drawing.Color.White;
            this.cbFilterColumn.ForeColor = System.Drawing.Color.Black;
            this.cbFilterColumn.FormattingEnabled = true;
            this.cbFilterColumn.Location = new System.Drawing.Point(462, -1);
            this.cbFilterColumn.Name = "cbFilterColumn";
            this.cbFilterColumn.Size = new System.Drawing.Size(100, 21);
            this.cbFilterColumn.TabIndex = 149;
            // 
            // tbFilter
            // 
            this.tbFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbFilter.Location = new System.Drawing.Point(562, -1);
            this.tbFilter.Name = "tbFilter";
            this.tbFilter.Size = new System.Drawing.Size(100, 22);
            this.tbFilter.TabIndex = 7;
            // 
            // panelBottom
            // 
            this.panelBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panelBottom.Controls.Add(this.panel9);
            this.panelBottom.Controls.Add(this.label5);
            this.panelBottom.Controls.Add(this.panel5);
            this.panelBottom.Controls.Add(this.label2);
            this.panelBottom.Controls.Add(this.panel4);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.ForeColor = System.Drawing.Color.White;
            this.panelBottom.Location = new System.Drawing.Point(0, 266);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(662, 221);
            this.panelBottom.TabIndex = 149;
            this.panelBottom.Tag = "color:normal";
            // 
            // panel9
            // 
            this.panel9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel9.Controls.Add(this.label16);
            this.panel9.Controls.Add(this.upDownLifetime);
            this.panel9.Controls.Add(this.label1);
            this.panel9.Controls.Add(this.upDownExecuteFrame);
            this.panel9.Controls.Add(this.cbLoop);
            this.panel9.Location = new System.Drawing.Point(20, 102);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(118, 116);
            this.panel9.TabIndex = 4;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(-1, 46);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(47, 13);
            this.label16.TabIndex = 26;
            this.label16.Text = "Lifetime";
            // 
            // upDownLifetime
            // 
            this.upDownLifetime.Location = new System.Drawing.Point(3, 61);
            this.upDownLifetime.Name = "upDownLifetime";
            this.upDownLifetime.Size = new System.Drawing.Size(109, 22);
            this.upDownLifetime.TabIndex = 25;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 24;
            this.label1.Text = "Execute Frame";
            // 
            // upDownExecuteFrame
            // 
            this.upDownExecuteFrame.Location = new System.Drawing.Point(3, 19);
            this.upDownExecuteFrame.Name = "upDownExecuteFrame";
            this.upDownExecuteFrame.Size = new System.Drawing.Size(109, 22);
            this.upDownExecuteFrame.TabIndex = 12;
            // 
            // cbLoop
            // 
            this.cbLoop.AutoSize = true;
            this.cbLoop.Location = new System.Drawing.Point(6, 92);
            this.cbLoop.Name = "cbLoop";
            this.cbLoop.Size = new System.Drawing.Size(52, 17);
            this.cbLoop.TabIndex = 0;
            this.cbLoop.Text = "Loop";
            this.cbLoop.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(145, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(31, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Data";
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.btnNote);
            this.panel5.Controls.Add(this.panel8);
            this.panel5.Controls.Add(this.tbTiltValue);
            this.panel5.Controls.Add(this.label15);
            this.panel5.Controls.Add(this.label11);
            this.panel5.Controls.Add(this.cbSource);
            this.panel5.Controls.Add(this.panel6);
            this.panel5.Controls.Add(this.panel7);
            this.panel5.Controls.Add(this.label10);
            this.panel5.Controls.Add(this.upDownPrecision);
            this.panel5.Controls.Add(this.label9);
            this.panel5.Controls.Add(this.upDownAddress);
            this.panel5.Controls.Add(this.label8);
            this.panel5.Controls.Add(this.cbDomain);
            this.panel5.Location = new System.Drawing.Point(148, 24);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(484, 194);
            this.panel5.TabIndex = 2;
            // 
            // btnNote
            // 
            this.btnNote.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnNote.FlatAppearance.BorderSize = 0;
            this.btnNote.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnNote.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnNote.ForeColor = System.Drawing.Color.Black;
            this.btnNote.Location = new System.Drawing.Point(353, 100);
            this.btnNote.Name = "btnNote";
            this.btnNote.Size = new System.Drawing.Size(120, 88);
            this.btnNote.TabIndex = 142;
            this.btnNote.TabStop = false;
            this.btnNote.Tag = "color:light";
            this.btnNote.Text = "Open Note Editor";
            this.btnNote.UseVisualStyleBackColor = false;
            this.btnNote.Click += new System.EventHandler(this.btnNote_Click);
            // 
            // panel8
            // 
            this.panel8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel8.Controls.Add(this.cbStoreLimiterSource);
            this.panel8.Controls.Add(this.label17);
            this.panel8.Controls.Add(this.cbInvertLimiter);
            this.panel8.Controls.Add(this.cbLimiterTime);
            this.panel8.Controls.Add(this.label18);
            this.panel8.Controls.Add(this.cbLimiterList);
            this.panel8.Controls.Add(this.label19);
            this.panel8.Location = new System.Drawing.Point(128, 104);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(210, 84);
            this.panel8.TabIndex = 25;
            // 
            // cbStoreLimiterSource
            // 
            this.cbStoreLimiterSource.FormattingEnabled = true;
            this.cbStoreLimiterSource.Location = new System.Drawing.Point(107, 57);
            this.cbStoreLimiterSource.Name = "cbStoreLimiterSource";
            this.cbStoreLimiterSource.Size = new System.Drawing.Size(92, 21);
            this.cbStoreLimiterSource.TabIndex = 13;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(104, 43);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(99, 13);
            this.label17.TabIndex = 12;
            this.label17.Text = "Store Comparison";
            // 
            // cbInvertLimiter
            // 
            this.cbInvertLimiter.AutoSize = true;
            this.cbInvertLimiter.Location = new System.Drawing.Point(3, 47);
            this.cbInvertLimiter.Name = "cbInvertLimiter";
            this.cbInvertLimiter.Size = new System.Drawing.Size(92, 17);
            this.cbInvertLimiter.TabIndex = 11;
            this.cbInvertLimiter.Text = "Invert Limiter";
            this.cbInvertLimiter.UseVisualStyleBackColor = true;
            // 
            // cbLimiterTime
            // 
            this.cbLimiterTime.FormattingEnabled = true;
            this.cbLimiterTime.Location = new System.Drawing.Point(107, 19);
            this.cbLimiterTime.Name = "cbLimiterTime";
            this.cbLimiterTime.Size = new System.Drawing.Size(92, 21);
            this.cbLimiterTime.TabIndex = 10;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(104, 4);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(67, 13);
            this.label18.TabIndex = 9;
            this.label18.Text = "Limiter Time";
            // 
            // cbLimiterList
            // 
            this.cbLimiterList.FormattingEnabled = true;
            this.cbLimiterList.IntegralHeight = false;
            this.cbLimiterList.Location = new System.Drawing.Point(3, 19);
            this.cbLimiterList.MaxDropDownItems = 15;
            this.cbLimiterList.Name = "cbLimiterList";
            this.cbLimiterList.Size = new System.Drawing.Size(92, 21);
            this.cbLimiterList.TabIndex = 8;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(0, 4);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(61, 13);
            this.label19.TabIndex = 7;
            this.label19.Text = "Limiter List";
            // 
            // tbTiltValue
            // 
            this.tbTiltValue.AllowDecimal = false;
            this.tbTiltValue.AllowNegative = true;
            this.tbTiltValue.AllowSpace = false;
            this.tbTiltValue.Location = new System.Drawing.Point(4, 148);
            this.tbTiltValue.Name = "tbTiltValue";
            this.tbTiltValue.Size = new System.Drawing.Size(108, 22);
            this.tbTiltValue.TabIndex = 24;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(1, 132);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(54, 13);
            this.label15.TabIndex = 23;
            this.label15.Text = "Tilt Value";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(125, 4);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(42, 13);
            this.label11.TabIndex = 16;
            this.label11.Text = "Source";
            // 
            // cbSource
            // 
            this.cbSource.FormattingEnabled = true;
            this.cbSource.Location = new System.Drawing.Point(128, 19);
            this.cbSource.Name = "cbSource";
            this.cbSource.Size = new System.Drawing.Size(109, 21);
            this.cbSource.TabIndex = 15;
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.label12);
            this.panel6.Controls.Add(this.tbValue);
            this.panel6.Location = new System.Drawing.Point(128, 43);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(109, 49);
            this.panel6.TabIndex = 14;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(3, 2);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(36, 13);
            this.label12.TabIndex = 13;
            this.label12.Text = "Value";
            // 
            // tbValue
            // 
            this.tbValue.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tbValue.Font = new System.Drawing.Font("Consolas", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.tbValue.Location = new System.Drawing.Point(3, 17);
            this.tbValue.MaxLength = 16348;
            this.tbValue.Name = "tbValue";
            this.tbValue.Nullable = true;
            this.tbValue.Size = new System.Drawing.Size(100, 20);
            this.tbValue.TabIndex = 0;
            this.tbValue.Text = "FFFFFFFF";
            // 
            // panel7
            // 
            this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel7.Controls.Add(this.cbSourceDomain);
            this.panel7.Controls.Add(this.upDownSourceAddress);
            this.panel7.Controls.Add(this.label13);
            this.panel7.Controls.Add(this.label14);
            this.panel7.Controls.Add(this.cbStoreTime);
            this.panel7.Controls.Add(this.label6);
            this.panel7.Controls.Add(this.cbStoreType);
            this.panel7.Controls.Add(this.label7);
            this.panel7.Location = new System.Drawing.Point(257, 4);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(216, 88);
            this.panel7.TabIndex = 13;
            // 
            // cbSourceDomain
            // 
            this.cbSourceDomain.FormattingEnabled = true;
            this.cbSourceDomain.Location = new System.Drawing.Point(95, 18);
            this.cbSourceDomain.Name = "cbSourceDomain";
            this.cbSourceDomain.Size = new System.Drawing.Size(109, 21);
            this.cbSourceDomain.TabIndex = 11;
            // 
            // upDownSourceAddress
            // 
            this.upDownSourceAddress.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.upDownSourceAddress.Hexadecimal = true;
            this.upDownSourceAddress.Location = new System.Drawing.Point(95, 58);
            this.upDownSourceAddress.Name = "upDownSourceAddress";
            this.upDownSourceAddress.Size = new System.Drawing.Size(109, 22);
            this.upDownSourceAddress.TabIndex = 13;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(92, 44);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(86, 13);
            this.label13.TabIndex = 14;
            this.label13.Text = "Source Address";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(92, 3);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(85, 13);
            this.label14.TabIndex = 12;
            this.label14.Text = "Source Domain";
            // 
            // cbStoreTime
            // 
            this.cbStoreTime.FormattingEnabled = true;
            this.cbStoreTime.Location = new System.Drawing.Point(3, 58);
            this.cbStoreTime.Name = "cbStoreTime";
            this.cbStoreTime.Size = new System.Drawing.Size(83, 21);
            this.cbStoreTime.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 44);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Store Time";
            // 
            // cbStoreType
            // 
            this.cbStoreType.FormattingEnabled = true;
            this.cbStoreType.Location = new System.Drawing.Point(3, 18);
            this.cbStoreType.Name = "cbStoreType";
            this.cbStoreType.Size = new System.Drawing.Size(83, 21);
            this.cbStoreType.TabIndex = 8;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 3);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(60, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "Store Type";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(0, 88);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 13);
            this.label10.TabIndex = 12;
            this.label10.Text = "Precision";
            // 
            // upDownPrecision
            // 
            this.upDownPrecision.Location = new System.Drawing.Point(3, 102);
            this.upDownPrecision.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.upDownPrecision.Name = "upDownPrecision";
            this.upDownPrecision.Size = new System.Drawing.Size(109, 22);
            this.upDownPrecision.TabIndex = 11;
            this.upDownPrecision.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(1, 44);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(48, 13);
            this.label9.TabIndex = 10;
            this.label9.Text = "Address";
            // 
            // upDownAddress
            // 
            this.upDownAddress.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.upDownAddress.Hexadecimal = true;
            this.upDownAddress.Location = new System.Drawing.Point(4, 58);
            this.upDownAddress.Name = "upDownAddress";
            this.upDownAddress.Size = new System.Drawing.Size(109, 22);
            this.upDownAddress.TabIndex = 9;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(1, 3);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 13);
            this.label8.TabIndex = 8;
            this.label8.Text = "Domain";
            // 
            // cbDomain
            // 
            this.cbDomain.FormattingEnabled = true;
            this.cbDomain.Location = new System.Drawing.Point(4, 18);
            this.cbDomain.Name = "cbDomain";
            this.cbDomain.Size = new System.Drawing.Size(109, 21);
            this.cbDomain.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Settings";
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.cbBigEndian);
            this.panel4.Controls.Add(this.cbLocked);
            this.panel4.Controls.Add(this.cbEnabled);
            this.panel4.Location = new System.Drawing.Point(20, 24);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(118, 68);
            this.panel4.TabIndex = 0;
            // 
            // cbBigEndian
            // 
            this.cbBigEndian.AutoSize = true;
            this.cbBigEndian.Location = new System.Drawing.Point(3, 46);
            this.cbBigEndian.Name = "cbBigEndian";
            this.cbBigEndian.Size = new System.Drawing.Size(82, 17);
            this.cbBigEndian.TabIndex = 2;
            this.cbBigEndian.Text = "Big Endian";
            this.cbBigEndian.UseVisualStyleBackColor = true;
            // 
            // cbLocked
            // 
            this.cbLocked.AutoSize = true;
            this.cbLocked.Location = new System.Drawing.Point(3, 25);
            this.cbLocked.Name = "cbLocked";
            this.cbLocked.Size = new System.Drawing.Size(62, 17);
            this.cbLocked.TabIndex = 1;
            this.cbLocked.Text = "Locked";
            this.cbLocked.UseVisualStyleBackColor = true;
            // 
            // cbEnabled
            // 
            this.cbEnabled.AutoSize = true;
            this.cbEnabled.Location = new System.Drawing.Point(3, 4);
            this.cbEnabled.Name = "cbEnabled";
            this.cbEnabled.Size = new System.Drawing.Size(68, 17);
            this.cbEnabled.TabIndex = 0;
            this.cbEnabled.Text = "Enabled";
            this.cbEnabled.UseVisualStyleBackColor = true;
            // 
            // menuStripEx1
            // 
            this.menuStripEx1.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.menuStripEx1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStripEx1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.blastLayerToolStripMenuItem,
            this.saveStateToolStripMenuItem,
            this.rOMToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.menuStripEx1.Location = new System.Drawing.Point(0, 0);
            this.menuStripEx1.Name = "menuStripEx1";
            this.menuStripEx1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.menuStripEx1.Size = new System.Drawing.Size(821, 24);
            this.menuStripEx1.TabIndex = 145;
            this.menuStripEx1.Tag = "";
            this.menuStripEx1.Text = "menuStripEx1";
            // 
            // blastLayerToolStripMenuItem
            // 
            this.blastLayerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadFromFileblToolStripMenuItem,
            this.saveToFileblToolStripMenuItem,
            this.saveAsToFileblToolStripMenuItem,
            this.importBlastlayerblToolStripMenuItem,
            this.exportToCSVToolStripMenuItem});
            this.blastLayerToolStripMenuItem.Name = "blastLayerToolStripMenuItem";
            this.blastLayerToolStripMenuItem.Size = new System.Drawing.Size(72, 20);
            this.blastLayerToolStripMenuItem.Tag = "";
            this.blastLayerToolStripMenuItem.Text = "BlastLayer";
            // 
            // loadFromFileblToolStripMenuItem
            // 
            this.loadFromFileblToolStripMenuItem.Name = "loadFromFileblToolStripMenuItem";
            this.loadFromFileblToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.loadFromFileblToolStripMenuItem.Text = "&Load From File (.bl)";
            this.loadFromFileblToolStripMenuItem.Click += new System.EventHandler(this.loadFromFileblToolStripMenuItem_Click);
            // 
            // saveToFileblToolStripMenuItem
            // 
            this.saveToFileblToolStripMenuItem.Name = "saveToFileblToolStripMenuItem";
            this.saveToFileblToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.saveToFileblToolStripMenuItem.Text = "&Save to File (.bl)";
            this.saveToFileblToolStripMenuItem.Click += new System.EventHandler(this.saveToFileblToolStripMenuItem_Click);
            // 
            // saveAsToFileblToolStripMenuItem
            // 
            this.saveAsToFileblToolStripMenuItem.Name = "saveAsToFileblToolStripMenuItem";
            this.saveAsToFileblToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.saveAsToFileblToolStripMenuItem.Text = "&Save As to File (.bl)";
            this.saveAsToFileblToolStripMenuItem.Click += new System.EventHandler(this.saveAsToFileblToolStripMenuItem_Click);
            // 
            // importBlastlayerblToolStripMenuItem
            // 
            this.importBlastlayerblToolStripMenuItem.Name = "importBlastlayerblToolStripMenuItem";
            this.importBlastlayerblToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.importBlastlayerblToolStripMenuItem.Text = "&Import Blastlayer (.bl)";
            this.importBlastlayerblToolStripMenuItem.Click += new System.EventHandler(this.importBlastlayerblToolStripMenuItem_Click);
            // 
            // exportToCSVToolStripMenuItem
            // 
            this.exportToCSVToolStripMenuItem.Name = "exportToCSVToolStripMenuItem";
            this.exportToCSVToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.exportToCSVToolStripMenuItem.Text = "&Export to CSV";
            this.exportToCSVToolStripMenuItem.Click += new System.EventHandler(this.exportToCSVToolStripMenuItem_Click);
            // 
            // saveStateToolStripMenuItem
            // 
            this.saveStateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runOriginalSavestateToolStripMenuItem,
            this.replaceSavestateFromGHToolStripMenuItem,
            this.replaceSavestateFromFileToolStripMenuItem,
            this.saveSavestateToToolStripMenuItem});
            this.saveStateToolStripMenuItem.Name = "saveStateToolStripMenuItem";
            this.saveStateToolStripMenuItem.Size = new System.Drawing.Size(69, 20);
            this.saveStateToolStripMenuItem.Tag = "";
            this.saveStateToolStripMenuItem.Text = "SaveState";
            // 
            // runOriginalSavestateToolStripMenuItem
            // 
            this.runOriginalSavestateToolStripMenuItem.Name = "runOriginalSavestateToolStripMenuItem";
            this.runOriginalSavestateToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.runOriginalSavestateToolStripMenuItem.Text = "Run Original Savestate";
            this.runOriginalSavestateToolStripMenuItem.Click += new System.EventHandler(this.runOriginalSavestateToolStripMenuItem_Click);
            // 
            // replaceSavestateFromGHToolStripMenuItem
            // 
            this.replaceSavestateFromGHToolStripMenuItem.Name = "replaceSavestateFromGHToolStripMenuItem";
            this.replaceSavestateFromGHToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.replaceSavestateFromGHToolStripMenuItem.Text = "Replace Savestate from GH";
            this.replaceSavestateFromGHToolStripMenuItem.Click += new System.EventHandler(this.replaceSavestateFromGHToolStripMenuItem_Click);
            // 
            // replaceSavestateFromFileToolStripMenuItem
            // 
            this.replaceSavestateFromFileToolStripMenuItem.Name = "replaceSavestateFromFileToolStripMenuItem";
            this.replaceSavestateFromFileToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.replaceSavestateFromFileToolStripMenuItem.Text = "Replace Savestate from File";
            this.replaceSavestateFromFileToolStripMenuItem.Click += new System.EventHandler(this.replaceSavestateFromFileToolStripMenuItem_Click);
            // 
            // saveSavestateToToolStripMenuItem
            // 
            this.saveSavestateToToolStripMenuItem.Name = "saveSavestateToToolStripMenuItem";
            this.saveSavestateToToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.saveSavestateToToolStripMenuItem.Text = "Save Savestate to";
            this.saveSavestateToToolStripMenuItem.Click += new System.EventHandler(this.saveSavestateToToolStripMenuItem_Click);
            // 
            // rOMToolStripMenuItem
            // 
            this.rOMToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runRomWithoutBlastlayerToolStripMenuItem,
            this.replaceRomFromGHToolStripMenuItem,
            this.replaceRomFromFileToolStripMenuItem,
            this.bakeROMBlastunitsToFileToolStripMenuItem});
            this.rOMToolStripMenuItem.Name = "rOMToolStripMenuItem";
            this.rOMToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.rOMToolStripMenuItem.Tag = "";
            this.rOMToolStripMenuItem.Text = "ROM";
            // 
            // runRomWithoutBlastlayerToolStripMenuItem
            // 
            this.runRomWithoutBlastlayerToolStripMenuItem.Name = "runRomWithoutBlastlayerToolStripMenuItem";
            this.runRomWithoutBlastlayerToolStripMenuItem.Size = new System.Drawing.Size(257, 22);
            this.runRomWithoutBlastlayerToolStripMenuItem.Text = "Run Rom Without Blastlayer";
            this.runRomWithoutBlastlayerToolStripMenuItem.Click += new System.EventHandler(this.runRomWithoutBlastlayerToolStripMenuItem_Click);
            // 
            // replaceRomFromGHToolStripMenuItem
            // 
            this.replaceRomFromGHToolStripMenuItem.Name = "replaceRomFromGHToolStripMenuItem";
            this.replaceRomFromGHToolStripMenuItem.Size = new System.Drawing.Size(257, 22);
            this.replaceRomFromGHToolStripMenuItem.Text = "Replace Rom from GH";
            this.replaceRomFromGHToolStripMenuItem.Click += new System.EventHandler(this.replaceRomFromGHToolStripMenuItem_Click);
            // 
            // replaceRomFromFileToolStripMenuItem
            // 
            this.replaceRomFromFileToolStripMenuItem.Name = "replaceRomFromFileToolStripMenuItem";
            this.replaceRomFromFileToolStripMenuItem.Size = new System.Drawing.Size(257, 22);
            this.replaceRomFromFileToolStripMenuItem.Text = "Replace Rom from File";
            this.replaceRomFromFileToolStripMenuItem.Click += new System.EventHandler(this.replaceRomFromFileToolStripMenuItem_Click);
            // 
            // bakeROMBlastunitsToFileToolStripMenuItem
            // 
            this.bakeROMBlastunitsToFileToolStripMenuItem.Name = "bakeROMBlastunitsToFileToolStripMenuItem";
            this.bakeROMBlastunitsToFileToolStripMenuItem.Size = new System.Drawing.Size(257, 22);
            this.bakeROMBlastunitsToFileToolStripMenuItem.Text = "Bake ROM VALUE BlastUnits to File";
            this.bakeROMBlastunitsToFileToolStripMenuItem.Click += new System.EventHandler(this.bakeROMBlastunitsToFileToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sanitizeDuplicatesToolStripMenuItem,
            this.rasterizeVMDsToolStripMenuItem,
            this.bakeBlastunitsToVALUEToolStripMenuItem,
            this.openBlastGeneratorToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Tag = "";
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // sanitizeDuplicatesToolStripMenuItem
            // 
            this.sanitizeDuplicatesToolStripMenuItem.Name = "sanitizeDuplicatesToolStripMenuItem";
            this.sanitizeDuplicatesToolStripMenuItem.Size = new System.Drawing.Size(252, 22);
            this.sanitizeDuplicatesToolStripMenuItem.Text = "Sanitize Duplicates";
            this.sanitizeDuplicatesToolStripMenuItem.Click += new System.EventHandler(this.sanitizeDuplicatesToolStripMenuItem_Click);
            // 
            // rasterizeVMDsToolStripMenuItem
            // 
            this.rasterizeVMDsToolStripMenuItem.Name = "rasterizeVMDsToolStripMenuItem";
            this.rasterizeVMDsToolStripMenuItem.Size = new System.Drawing.Size(252, 22);
            this.rasterizeVMDsToolStripMenuItem.Text = "Rasterize VMDs";
            this.rasterizeVMDsToolStripMenuItem.Click += new System.EventHandler(this.rasterizeVMDsToolStripMenuItem_Click);
            // 
            // bakeBlastunitsToVALUEToolStripMenuItem
            // 
            this.bakeBlastunitsToVALUEToolStripMenuItem.Name = "bakeBlastunitsToVALUEToolStripMenuItem";
            this.bakeBlastunitsToVALUEToolStripMenuItem.Size = new System.Drawing.Size(252, 22);
            this.bakeBlastunitsToVALUEToolStripMenuItem.Text = "Bake Selected Blastunits to VALUE";
            this.bakeBlastunitsToVALUEToolStripMenuItem.Click += new System.EventHandler(this.bakeBlastunitsToVALUEToolStripMenuItem_Click);
            // 
            // openBlastGeneratorToolStripMenuItem
            // 
            this.openBlastGeneratorToolStripMenuItem.Name = "openBlastGeneratorToolStripMenuItem";
            this.openBlastGeneratorToolStripMenuItem.Size = new System.Drawing.Size(252, 22);
            this.openBlastGeneratorToolStripMenuItem.Text = "Open Blast Generator";
            this.openBlastGeneratorToolStripMenuItem.Click += new System.EventHandler(this.OpenBlastGeneratorToolStripMenuItem_Click);
            // 
            // RTC_NewBlastEditor_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(821, 487);
            this.Controls.Add(this.dgvBlastEditor);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelSidebar);
            this.Controls.Add(this.menuStripEx1);
            this.Font = new System.Drawing.Font("Segoe UI Symbol", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "RTC_NewBlastEditor_Form";
            this.Text = "Blast Editor";
            this.Load += new System.EventHandler(this.RTC_NewBlastEditorForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBlastEditor)).EndInit();
            this.panelSidebar.ResumeLayout(false);
            this.panelSidebar.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.updownShiftBlastLayerAmount)).EndInit();
            this.pnMemoryTargetting.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panelBottom.ResumeLayout(false);
            this.panelBottom.PerformLayout();
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.upDownLifetime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.upDownExecuteFrame)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.upDownSourceAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.upDownPrecision)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.upDownAddress)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.menuStripEx1.ResumeLayout(false);
            this.menuStripEx1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		public System.Windows.Forms.DataGridView dgvBlastEditor;
		public System.Windows.Forms.Panel panelSidebar;
		private System.Windows.Forms.Button btnHelp;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button btnLoadCorrupt;
		private System.Windows.Forms.Button btnRemoveSelected;
		private System.Windows.Forms.Button btnCorrupt;
		private System.Windows.Forms.Button btnSendToStash;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.ComboBox cbShiftBlastlayer;
		private System.Windows.Forms.Button btnShiftBlastLayerDown;
		private System.Windows.Forms.Button btnShiftBlastLayerUp;
		private NumericUpDownHexFix updownShiftBlastLayerAmount;
		private System.Windows.Forms.Button btnRemoveDisabled;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button btnDisable50;
		private System.Windows.Forms.Button btnInvertDisabled;
		private System.Windows.Forms.Panel pnMemoryTargetting;
		private System.Windows.Forms.Label lbBlastLayerSize;
		private System.Windows.Forms.Button btnDisableEverything;
		private System.Windows.Forms.Button btnEnableEverything;
		private System.Windows.Forms.Button btnDuplicateSelected;
		private MenuStrip menuStripEx1;
		private System.Windows.Forms.ToolStripMenuItem rOMToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem runRomWithoutBlastlayerToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem replaceRomFromGHToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem replaceRomFromFileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem bakeROMBlastunitsToFileToolStripMenuItem;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.ToolStripMenuItem blastLayerToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem loadFromFileblToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveToFileblToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveAsToFileblToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem importBlastlayerblToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exportToCSVToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveStateToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem runOriginalSavestateToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem replaceSavestateFromGHToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem replaceSavestateFromFileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveSavestateToToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem sanitizeDuplicatesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem rasterizeVMDsToolStripMenuItem;
		private System.Windows.Forms.Panel panelBottom;
		private System.Windows.Forms.Panel panel5;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.CheckBox cbLocked;
		private System.Windows.Forms.CheckBox cbEnabled;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.ComboBox cbDomain;
		private System.Windows.Forms.CheckBox cbBigEndian;
		private System.Windows.Forms.Label label9;
		private NumericUpDownHexFix upDownAddress;
		private System.Windows.Forms.Panel panel6;
		private System.Windows.Forms.Label label12;
		private UI_Extensions.HexTextBox tbValue;
		private System.Windows.Forms.Panel panel7;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ComboBox cbStoreType;
		private System.Windows.Forms.ComboBox cbStoreTime;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.ComboBox cbSource;
		private System.Windows.Forms.Label label10;
		private NumericUpDownHexFix upDownPrecision;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.ComboBox cbSourceDomain;
		private NumericUpDownHexFix upDownSourceAddress;
		private System.Windows.Forms.Panel panel8;
		private System.Windows.Forms.CheckBox cbInvertLimiter;
		private System.Windows.Forms.ComboBox cbLimiterTime;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.ComboBox cbLimiterList;
		private System.Windows.Forms.Label label19;
		private UI_Extensions.NumericTextBox tbTiltValue;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ComboBox cbFilterColumn;
		private System.Windows.Forms.TextBox tbFilter;
		private System.Windows.Forms.Panel panel9;
		private System.Windows.Forms.Label label16;
		private NumericUpDownHexFix upDownLifetime;
		private System.Windows.Forms.Label label1;
		private NumericUpDownHexFix upDownExecuteFrame;
		private System.Windows.Forms.CheckBox cbLoop;
		private System.Windows.Forms.Button btnNote;
		private System.Windows.Forms.ToolStripMenuItem bakeBlastunitsToVALUEToolStripMenuItem;
		private ComboBox cbStoreLimiterSource;
		private Label label17;
		private ToolStripMenuItem openBlastGeneratorToolStripMenuItem;
		private Button btnAddRow;
	}
}