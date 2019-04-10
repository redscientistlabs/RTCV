namespace RTCV.UI
{
    partial class RTC_VmdAct_Form
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
            this.btnActiveTableSubtractFile = new System.Windows.Forms.Button();
            this.btnActiveTableGenerate = new System.Windows.Forms.Button();
            this.btnActiveTableQuickSave = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbUseCorePrecision = new System.Windows.Forms.CheckBox();
            this.cbActiveTableExclude100percent = new System.Windows.Forms.CheckBox();
            this.track_ActiveTableActivityThreshold = new System.Windows.Forms.TrackBar();
            this.label15 = new System.Windows.Forms.Label();
            this.nmActiveTableActivityThreshold = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.nmActiveTableCapOffset = new NumericUpDownHexFix();
            this.rbActiveTableCapBlockEnd = new System.Windows.Forms.RadioButton();
            this.rbActiveTableCapBlockStart = new System.Windows.Forms.RadioButton();
            this.rbActiveTableCapRandom = new System.Windows.Forms.RadioButton();
            this.nmActiveTableCapSize = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.cbActiveTableCapSize = new System.Windows.Forms.CheckBox();
            this.cbSelectedMemoryDomain = new System.Windows.Forms.ComboBox();
            this.btnActiveTableLoad = new System.Windows.Forms.Button();
            this.lbFreezeEngineNbDumps = new System.Windows.Forms.Label();
            this.lbActiveTableSize = new System.Windows.Forms.Label();
            this.cbAutoAddDump = new System.Windows.Forms.CheckBox();
            this.lbDomainAddressSize = new System.Windows.Forms.Label();
            this.btnActiveTableAddDump = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.btnActiveTableDumpsReset = new System.Windows.Forms.Button();
            this.nmAutoAddSec = new System.Windows.Forms.NumericUpDown();
            this.lbActiveStatus = new System.Windows.Forms.Label();
            this.btnActiveTableAddFile = new System.Windows.Forms.Button();
            this.btnLoadDomains = new System.Windows.Forms.Button();
            this.lbAutoAddEvery = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.track_ActiveTableActivityThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmActiveTableActivityThreshold)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmActiveTableCapOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmActiveTableCapSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmAutoAddSec)).BeginInit();
            this.SuspendLayout();
            // 
            // btnActiveTableSubtractFile
            // 
            this.btnActiveTableSubtractFile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnActiveTableSubtractFile.Enabled = false;
            this.btnActiveTableSubtractFile.FlatAppearance.BorderSize = 0;
            this.btnActiveTableSubtractFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActiveTableSubtractFile.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnActiveTableSubtractFile.ForeColor = System.Drawing.Color.Black;
            this.btnActiveTableSubtractFile.Location = new System.Drawing.Point(17, 180);
            this.btnActiveTableSubtractFile.Name = "btnActiveTableSubtractFile";
            this.btnActiveTableSubtractFile.Size = new System.Drawing.Size(88, 20);
            this.btnActiveTableSubtractFile.TabIndex = 123;
            this.btnActiveTableSubtractFile.Tag = "color:light";
            this.btnActiveTableSubtractFile.Text = "Subtract ACT";
            this.btnActiveTableSubtractFile.UseVisualStyleBackColor = false;
            this.btnActiveTableSubtractFile.Click += new System.EventHandler(this.btnActiveTableSubtractFile_Click);
            this.btnActiveTableSubtractFile.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // btnActiveTableGenerate
            // 
            this.btnActiveTableGenerate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnActiveTableGenerate.Enabled = false;
            this.btnActiveTableGenerate.FlatAppearance.BorderSize = 0;
            this.btnActiveTableGenerate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActiveTableGenerate.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnActiveTableGenerate.ForeColor = System.Drawing.Color.Black;
            this.btnActiveTableGenerate.Location = new System.Drawing.Point(17, 224);
            this.btnActiveTableGenerate.Name = "btnActiveTableGenerate";
            this.btnActiveTableGenerate.Size = new System.Drawing.Size(161, 20);
            this.btnActiveTableGenerate.TabIndex = 84;
            this.btnActiveTableGenerate.Tag = "color:light";
            this.btnActiveTableGenerate.Text = "Generate VMD from ACT";
            this.btnActiveTableGenerate.UseVisualStyleBackColor = false;
            this.btnActiveTableGenerate.Click += new System.EventHandler(this.btnActiveTableGenerate_Click);
            this.btnActiveTableGenerate.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // btnActiveTableQuickSave
            // 
            this.btnActiveTableQuickSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnActiveTableQuickSave.Enabled = false;
            this.btnActiveTableQuickSave.FlatAppearance.BorderSize = 0;
            this.btnActiveTableQuickSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActiveTableQuickSave.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnActiveTableQuickSave.ForeColor = System.Drawing.Color.Black;
            this.btnActiveTableQuickSave.Location = new System.Drawing.Point(107, 202);
            this.btnActiveTableQuickSave.Name = "btnActiveTableQuickSave";
            this.btnActiveTableQuickSave.Size = new System.Drawing.Size(71, 20);
            this.btnActiveTableQuickSave.TabIndex = 120;
            this.btnActiveTableQuickSave.Tag = "color:light";
            this.btnActiveTableQuickSave.Text = "Save ACT";
            this.btnActiveTableQuickSave.UseVisualStyleBackColor = false;
            this.btnActiveTableQuickSave.Click += new System.EventHandler(this.btnActiveTableQuickSave_Click);
            this.btnActiveTableQuickSave.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbUseCorePrecision);
            this.groupBox2.Controls.Add(this.cbActiveTableExclude100percent);
            this.groupBox2.Controls.Add(this.track_ActiveTableActivityThreshold);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.nmActiveTableActivityThreshold);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.groupBox1);
            this.groupBox2.Controls.Add(this.nmActiveTableCapSize);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.cbActiveTableCapSize);
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(187, 10);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(188, 234);
            this.groupBox2.TabIndex = 87;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Generation parameters";
            this.groupBox2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
			// 
			// cbUseCorePrecision
			// 
			this.cbUseCorePrecision.AutoSize = true;
            this.cbUseCorePrecision.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbUseCorePrecision.ForeColor = System.Drawing.Color.White;
            this.cbUseCorePrecision.Location = new System.Drawing.Point(14, 82);
            this.cbUseCorePrecision.Name = "cbUseCorePrecision";
            this.cbUseCorePrecision.Size = new System.Drawing.Size(121, 17);
            this.cbUseCorePrecision.TabIndex = 123;
            this.cbUseCorePrecision.Text = "Use Core Precision";
            this.cbUseCorePrecision.UseVisualStyleBackColor = true;
            this.cbUseCorePrecision.CheckedChanged += new System.EventHandler(this.cbUseCorePrecision_CheckedChanged);
            this.cbUseCorePrecision.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // cbActiveTableExclude100percent
            // 
            this.cbActiveTableExclude100percent.AutoSize = true;
            this.cbActiveTableExclude100percent.Checked = true;
            this.cbActiveTableExclude100percent.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbActiveTableExclude100percent.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbActiveTableExclude100percent.ForeColor = System.Drawing.Color.White;
            this.cbActiveTableExclude100percent.Location = new System.Drawing.Point(14, 67);
            this.cbActiveTableExclude100percent.Name = "cbActiveTableExclude100percent";
            this.cbActiveTableExclude100percent.Size = new System.Drawing.Size(154, 17);
            this.cbActiveTableExclude100percent.TabIndex = 122;
            this.cbActiveTableExclude100percent.Text = "Exclude ever-changing %";
            this.cbActiveTableExclude100percent.UseVisualStyleBackColor = true;
            this.cbActiveTableExclude100percent.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // track_ActiveTableActivityThreshold
            // 
            this.track_ActiveTableActivityThreshold.Location = new System.Drawing.Point(8, 36);
            this.track_ActiveTableActivityThreshold.Maximum = 9999;
            this.track_ActiveTableActivityThreshold.Name = "track_ActiveTableActivityThreshold";
            this.track_ActiveTableActivityThreshold.Size = new System.Drawing.Size(171, 45);
            this.track_ActiveTableActivityThreshold.TabIndex = 85;
            this.track_ActiveTableActivityThreshold.TickFrequency = 0;
            this.track_ActiveTableActivityThreshold.Scroll += new System.EventHandler(this.track_ActiveTableActivityThreshold_Scroll);
            this.track_ActiveTableActivityThreshold.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(164, 19);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(16, 13);
            this.label15.TabIndex = 121;
            this.label15.Text = "%";
            this.label15.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // nmActiveTableActivityThreshold
            // 
            this.nmActiveTableActivityThreshold.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nmActiveTableActivityThreshold.DecimalPlaces = 2;
            this.nmActiveTableActivityThreshold.ForeColor = System.Drawing.Color.White;
            this.nmActiveTableActivityThreshold.Location = new System.Drawing.Point(108, 15);
            this.nmActiveTableActivityThreshold.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            131072});
            this.nmActiveTableActivityThreshold.Name = "nmActiveTableActivityThreshold";
            this.nmActiveTableActivityThreshold.Size = new System.Drawing.Size(53, 22);
            this.nmActiveTableActivityThreshold.TabIndex = 120;
            this.nmActiveTableActivityThreshold.Tag = "color:dark";
            this.nmActiveTableActivityThreshold.ValueChanged += new System.EventHandler(this.nmActiveTableActivityThreshold_ValueChanged);
            this.nmActiveTableActivityThreshold.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(10, 19);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(100, 13);
            this.label14.TabIndex = 119;
            this.label14.Text = "Activity Threshold:";
            this.label14.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.nmActiveTableCapOffset);
            this.groupBox1.Controls.Add(this.rbActiveTableCapBlockEnd);
            this.groupBox1.Controls.Add(this.rbActiveTableCapBlockStart);
            this.groupBox1.Controls.Add(this.rbActiveTableCapRandom);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(14, 140);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(165, 88);
            this.groupBox1.TabIndex = 85;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Capping distribution";
            this.groupBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
			// 
			// label12
			// 
			this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(8, 65);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(42, 13);
            this.label12.TabIndex = 80;
            this.label12.Text = "Offset:";
            this.label12.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // nmActiveTableCapOffset
            // 
            this.nmActiveTableCapOffset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nmActiveTableCapOffset.Font = new System.Drawing.Font("Consolas", 8F);
            this.nmActiveTableCapOffset.ForeColor = System.Drawing.Color.White;
            this.nmActiveTableCapOffset.Hexadecimal = true;
            this.nmActiveTableCapOffset.Location = new System.Drawing.Point(52, 62);
            this.nmActiveTableCapOffset.Name = "nmActiveTableCapOffset";
            this.nmActiveTableCapOffset.Size = new System.Drawing.Size(64, 20);
            this.nmActiveTableCapOffset.TabIndex = 119;
            this.nmActiveTableCapOffset.Tag = "color:dark";
            this.nmActiveTableCapOffset.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // rbActiveTableCapBlockEnd
            // 
            this.rbActiveTableCapBlockEnd.AutoSize = true;
            this.rbActiveTableCapBlockEnd.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.rbActiveTableCapBlockEnd.Location = new System.Drawing.Point(8, 44);
            this.rbActiveTableCapBlockEnd.Name = "rbActiveTableCapBlockEnd";
            this.rbActiveTableCapBlockEnd.Size = new System.Drawing.Size(112, 17);
            this.rbActiveTableCapBlockEnd.TabIndex = 2;
            this.rbActiveTableCapBlockEnd.Text = "Block - From end";
            this.rbActiveTableCapBlockEnd.UseVisualStyleBackColor = true;
            this.rbActiveTableCapBlockEnd.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // rbActiveTableCapBlockStart
            // 
            this.rbActiveTableCapBlockStart.AutoSize = true;
            this.rbActiveTableCapBlockStart.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.rbActiveTableCapBlockStart.Location = new System.Drawing.Point(8, 29);
            this.rbActiveTableCapBlockStart.Name = "rbActiveTableCapBlockStart";
            this.rbActiveTableCapBlockStart.Size = new System.Drawing.Size(115, 17);
            this.rbActiveTableCapBlockStart.TabIndex = 1;
            this.rbActiveTableCapBlockStart.Text = "Block - From start";
            this.rbActiveTableCapBlockStart.UseVisualStyleBackColor = true;
            this.rbActiveTableCapBlockStart.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // rbActiveTableCapRandom
            // 
            this.rbActiveTableCapRandom.AutoSize = true;
            this.rbActiveTableCapRandom.Checked = true;
            this.rbActiveTableCapRandom.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.rbActiveTableCapRandom.Location = new System.Drawing.Point(8, 14);
            this.rbActiveTableCapRandom.Name = "rbActiveTableCapRandom";
            this.rbActiveTableCapRandom.Size = new System.Drawing.Size(68, 17);
            this.rbActiveTableCapRandom.TabIndex = 0;
            this.rbActiveTableCapRandom.TabStop = true;
            this.rbActiveTableCapRandom.Text = "Random";
            this.rbActiveTableCapRandom.UseVisualStyleBackColor = true;
            this.rbActiveTableCapRandom.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // nmActiveTableCapSize
            // 
            this.nmActiveTableCapSize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nmActiveTableCapSize.ForeColor = System.Drawing.Color.White;
            this.nmActiveTableCapSize.Location = new System.Drawing.Point(66, 118);
            this.nmActiveTableCapSize.Maximum = new decimal(new int[] {
            -1304428544,
            434162106,
            542,
            0});
            this.nmActiveTableCapSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmActiveTableCapSize.Name = "nmActiveTableCapSize";
            this.nmActiveTableCapSize.Size = new System.Drawing.Size(113, 22);
            this.nmActiveTableCapSize.TabIndex = 77;
            this.nmActiveTableCapSize.Tag = "color:dark";
            this.nmActiveTableCapSize.Value = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            this.nmActiveTableCapSize.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 121);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 13);
            this.label6.TabIndex = 118;
            this.label6.Text = "Cap size:";
            this.label6.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // cbActiveTableCapSize
            // 
            this.cbActiveTableCapSize.AutoSize = true;
            this.cbActiveTableCapSize.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbActiveTableCapSize.ForeColor = System.Drawing.Color.White;
            this.cbActiveTableCapSize.Location = new System.Drawing.Point(14, 97);
            this.cbActiveTableCapSize.Name = "cbActiveTableCapSize";
            this.cbActiveTableCapSize.Size = new System.Drawing.Size(129, 17);
            this.cbActiveTableCapSize.TabIndex = 77;
            this.cbActiveTableCapSize.Text = "Cap active table size";
            this.cbActiveTableCapSize.UseVisualStyleBackColor = true;
            this.cbActiveTableCapSize.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // cbSelectedMemoryDomain
            // 
            this.cbSelectedMemoryDomain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cbSelectedMemoryDomain.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSelectedMemoryDomain.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbSelectedMemoryDomain.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.cbSelectedMemoryDomain.ForeColor = System.Drawing.Color.White;
            this.cbSelectedMemoryDomain.FormattingEnabled = true;
            this.cbSelectedMemoryDomain.Location = new System.Drawing.Point(11, 62);
            this.cbSelectedMemoryDomain.Name = "cbSelectedMemoryDomain";
            this.cbSelectedMemoryDomain.Size = new System.Drawing.Size(169, 25);
            this.cbSelectedMemoryDomain.TabIndex = 125;
            this.cbSelectedMemoryDomain.Tag = "color:dark";
            this.cbSelectedMemoryDomain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // btnActiveTableLoad
            // 
            this.btnActiveTableLoad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnActiveTableLoad.Enabled = false;
            this.btnActiveTableLoad.FlatAppearance.BorderSize = 0;
            this.btnActiveTableLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActiveTableLoad.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnActiveTableLoad.ForeColor = System.Drawing.Color.Black;
            this.btnActiveTableLoad.Location = new System.Drawing.Point(17, 202);
            this.btnActiveTableLoad.Name = "btnActiveTableLoad";
            this.btnActiveTableLoad.Size = new System.Drawing.Size(88, 20);
            this.btnActiveTableLoad.TabIndex = 118;
            this.btnActiveTableLoad.Tag = "color:light";
            this.btnActiveTableLoad.Text = "Load ACT";
            this.btnActiveTableLoad.UseVisualStyleBackColor = false;
            this.btnActiveTableLoad.Click += new System.EventHandler(this.btnActiveTableLoad_Click);
            this.btnActiveTableLoad.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // lbFreezeEngineNbDumps
            // 
            this.lbFreezeEngineNbDumps.AutoSize = true;
            this.lbFreezeEngineNbDumps.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lbFreezeEngineNbDumps.ForeColor = System.Drawing.Color.White;
            this.lbFreezeEngineNbDumps.Location = new System.Drawing.Point(14, 117);
            this.lbFreezeEngineNbDumps.Name = "lbFreezeEngineNbDumps";
            this.lbFreezeEngineNbDumps.Size = new System.Drawing.Size(148, 13);
            this.lbFreezeEngineNbDumps.TabIndex = 86;
            this.lbFreezeEngineNbDumps.Text = "Memory dumps collected: #";
            this.lbFreezeEngineNbDumps.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // lbActiveTableSize
            // 
            this.lbActiveTableSize.AutoSize = true;
            this.lbActiveTableSize.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lbActiveTableSize.ForeColor = System.Drawing.Color.White;
            this.lbActiveTableSize.Location = new System.Drawing.Point(14, 148);
            this.lbActiveTableSize.Name = "lbActiveTableSize";
            this.lbActiveTableSize.Size = new System.Drawing.Size(136, 13);
            this.lbActiveTableSize.TabIndex = 85;
            this.lbActiveTableSize.Text = "Active table size: ######";
            this.lbActiveTableSize.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // cbAutoAddDump
            // 
            this.cbAutoAddDump.AutoSize = true;
            this.cbAutoAddDump.Enabled = false;
            this.cbAutoAddDump.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbAutoAddDump.ForeColor = System.Drawing.Color.White;
            this.cbAutoAddDump.Location = new System.Drawing.Point(17, 97);
            this.cbAutoAddDump.Name = "cbAutoAddDump";
            this.cbAutoAddDump.Size = new System.Drawing.Size(15, 14);
            this.cbAutoAddDump.TabIndex = 122;
            this.cbAutoAddDump.UseVisualStyleBackColor = true;
            this.cbAutoAddDump.CheckedChanged += new System.EventHandler(this.cbAutoAddDump_CheckedChanged);
            this.cbAutoAddDump.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // lbDomainAddressSize
            // 
            this.lbDomainAddressSize.AutoSize = true;
            this.lbDomainAddressSize.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lbDomainAddressSize.ForeColor = System.Drawing.Color.White;
            this.lbDomainAddressSize.Location = new System.Drawing.Point(14, 132);
            this.lbDomainAddressSize.Name = "lbDomainAddressSize";
            this.lbDomainAddressSize.Size = new System.Drawing.Size(160, 13);
            this.lbDomainAddressSize.TabIndex = 82;
            this.lbDomainAddressSize.Text = "Domain address size: ######";
            this.lbDomainAddressSize.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // btnActiveTableAddDump
            // 
            this.btnActiveTableAddDump.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnActiveTableAddDump.Enabled = false;
            this.btnActiveTableAddDump.FlatAppearance.BorderSize = 0;
            this.btnActiveTableAddDump.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActiveTableAddDump.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnActiveTableAddDump.ForeColor = System.Drawing.Color.Black;
            this.btnActiveTableAddDump.Location = new System.Drawing.Point(97, 34);
            this.btnActiveTableAddDump.Name = "btnActiveTableAddDump";
            this.btnActiveTableAddDump.Size = new System.Drawing.Size(83, 23);
            this.btnActiveTableAddDump.TabIndex = 121;
            this.btnActiveTableAddDump.Tag = "color:light";
            this.btnActiveTableAddDump.Text = "Add state";
            this.btnActiveTableAddDump.UseVisualStyleBackColor = false;
            this.btnActiveTableAddDump.Click += new System.EventHandler(this.btnActiveTableAddDump_Click);
            this.btnActiveTableAddDump.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label16.ForeColor = System.Drawing.Color.White;
            this.label16.Location = new System.Drawing.Point(154, 97);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(23, 13);
            this.label16.TabIndex = 122;
            this.label16.Text = "sec";
            this.label16.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // btnActiveTableDumpsReset
            // 
            this.btnActiveTableDumpsReset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnActiveTableDumpsReset.Enabled = false;
            this.btnActiveTableDumpsReset.FlatAppearance.BorderSize = 0;
            this.btnActiveTableDumpsReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActiveTableDumpsReset.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnActiveTableDumpsReset.ForeColor = System.Drawing.Color.Black;
            this.btnActiveTableDumpsReset.Location = new System.Drawing.Point(97, 9);
            this.btnActiveTableDumpsReset.Name = "btnActiveTableDumpsReset";
            this.btnActiveTableDumpsReset.Size = new System.Drawing.Size(83, 23);
            this.btnActiveTableDumpsReset.TabIndex = 83;
            this.btnActiveTableDumpsReset.Tag = "color:light";
            this.btnActiveTableDumpsReset.Text = "Initialize";
            this.btnActiveTableDumpsReset.UseVisualStyleBackColor = false;
            this.btnActiveTableDumpsReset.Click += new System.EventHandler(this.btnActiveTableDumpsReset_Click);
            this.btnActiveTableDumpsReset.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // nmAutoAddSec
            // 
            this.nmAutoAddSec.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nmAutoAddSec.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.nmAutoAddSec.ForeColor = System.Drawing.Color.White;
            this.nmAutoAddSec.Location = new System.Drawing.Point(117, 93);
            this.nmAutoAddSec.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmAutoAddSec.Name = "nmAutoAddSec";
            this.nmAutoAddSec.Size = new System.Drawing.Size(37, 22);
            this.nmAutoAddSec.TabIndex = 122;
            this.nmAutoAddSec.Tag = "color:dark";
            this.nmAutoAddSec.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmAutoAddSec.ValueChanged += new System.EventHandler(this.nmAutoAddSec_ValueChanged);
            this.nmAutoAddSec.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // lbActiveStatus
            // 
            this.lbActiveStatus.AutoSize = true;
            this.lbActiveStatus.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lbActiveStatus.ForeColor = System.Drawing.Color.White;
            this.lbActiveStatus.Location = new System.Drawing.Point(14, 164);
            this.lbActiveStatus.Name = "lbActiveStatus";
            this.lbActiveStatus.Size = new System.Drawing.Size(164, 13);
            this.lbActiveStatus.TabIndex = 87;
            this.lbActiveStatus.Text = "Active table status: NOT READY";
            this.lbActiveStatus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // btnActiveTableAddFile
            // 
            this.btnActiveTableAddFile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnActiveTableAddFile.Enabled = false;
            this.btnActiveTableAddFile.FlatAppearance.BorderSize = 0;
            this.btnActiveTableAddFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActiveTableAddFile.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnActiveTableAddFile.ForeColor = System.Drawing.Color.Black;
            this.btnActiveTableAddFile.Location = new System.Drawing.Point(107, 180);
            this.btnActiveTableAddFile.Name = "btnActiveTableAddFile";
            this.btnActiveTableAddFile.Size = new System.Drawing.Size(71, 20);
            this.btnActiveTableAddFile.TabIndex = 124;
            this.btnActiveTableAddFile.Tag = "color:light";
            this.btnActiveTableAddFile.Text = "Add ACT";
            this.btnActiveTableAddFile.UseVisualStyleBackColor = false;
            this.btnActiveTableAddFile.Click += new System.EventHandler(this.btnActiveTableAddFile_Click);
            this.btnActiveTableAddFile.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // btnLoadDomains
            // 
            this.btnLoadDomains.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnLoadDomains.FlatAppearance.BorderSize = 0;
            this.btnLoadDomains.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadDomains.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnLoadDomains.ForeColor = System.Drawing.Color.Black;
            this.btnLoadDomains.Location = new System.Drawing.Point(11, 9);
            this.btnLoadDomains.Name = "btnLoadDomains";
            this.btnLoadDomains.Size = new System.Drawing.Size(80, 48);
            this.btnLoadDomains.TabIndex = 126;
            this.btnLoadDomains.TabStop = false;
            this.btnLoadDomains.Tag = "color:light";
            this.btnLoadDomains.Text = "Load Domains";
            this.btnLoadDomains.UseVisualStyleBackColor = false;
            this.btnLoadDomains.Click += new System.EventHandler(this.btnLoadDomains_Click);
            this.btnLoadDomains.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // lbAutoAddEvery
            // 
            this.lbAutoAddEvery.AutoSize = true;
            this.lbAutoAddEvery.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lbAutoAddEvery.ForeColor = System.Drawing.Color.White;
            this.lbAutoAddEvery.Location = new System.Drawing.Point(30, 97);
            this.lbAutoAddEvery.Name = "lbAutoAddEvery";
            this.lbAutoAddEvery.Size = new System.Drawing.Size(85, 13);
            this.lbAutoAddEvery.TabIndex = 127;
            this.lbAutoAddEvery.Text = "Auto-add every";
            this.lbAutoAddEvery.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // RTC_VmdAct_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(390, 250);
            this.Controls.Add(this.lbAutoAddEvery);
            this.Controls.Add(this.btnLoadDomains);
            this.Controls.Add(this.cbSelectedMemoryDomain);
            this.Controls.Add(this.btnActiveTableAddFile);
            this.Controls.Add(this.btnActiveTableSubtractFile);
            this.Controls.Add(this.btnActiveTableGenerate);
            this.Controls.Add(this.btnActiveTableDumpsReset);
            this.Controls.Add(this.lbFreezeEngineNbDumps);
            this.Controls.Add(this.lbActiveStatus);
            this.Controls.Add(this.lbActiveTableSize);
            this.Controls.Add(this.nmAutoAddSec);
            this.Controls.Add(this.cbAutoAddDump);
            this.Controls.Add(this.btnActiveTableLoad);
            this.Controls.Add(this.lbDomainAddressSize);
            this.Controls.Add(this.btnActiveTableAddDump);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnActiveTableQuickSave);
            this.Controls.Add(this.label16);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "RTC_VmdAct_Form";
            this.Tag = "color:darker";
            this.Text = "Active Table Generator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HandleFormClosing);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.track_ActiveTableActivityThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmActiveTableActivityThreshold)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmActiveTableCapOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmActiveTableCapSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmAutoAddSec)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Button btnActiveTableSubtractFile;
        public System.Windows.Forms.Button btnActiveTableGenerate;
        public System.Windows.Forms.Button btnActiveTableQuickSave;
        private System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.CheckBox cbActiveTableExclude100percent;
        public System.Windows.Forms.TrackBar track_ActiveTableActivityThreshold;
        private System.Windows.Forms.Label label15;
        public System.Windows.Forms.NumericUpDown nmActiveTableActivityThreshold;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label12;
        public NumericUpDownHexFix nmActiveTableCapOffset;
        public System.Windows.Forms.RadioButton rbActiveTableCapBlockEnd;
        public System.Windows.Forms.RadioButton rbActiveTableCapBlockStart;
        public System.Windows.Forms.RadioButton rbActiveTableCapRandom;
        public System.Windows.Forms.NumericUpDown nmActiveTableCapSize;
        private System.Windows.Forms.Label label6;
        public System.Windows.Forms.CheckBox cbActiveTableCapSize;
        private System.Windows.Forms.Button btnActiveTableLoad;
        public System.Windows.Forms.Label lbFreezeEngineNbDumps;
        public System.Windows.Forms.Label lbActiveTableSize;
        public System.Windows.Forms.CheckBox cbAutoAddDump;
        public System.Windows.Forms.Label lbDomainAddressSize;
        public System.Windows.Forms.Button btnActiveTableAddDump;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button btnActiveTableDumpsReset;
        public System.Windows.Forms.NumericUpDown nmAutoAddSec;
        public System.Windows.Forms.Label lbActiveStatus;
        public System.Windows.Forms.Button btnActiveTableAddFile;
		private System.Windows.Forms.Button btnLoadDomains;
		public System.Windows.Forms.ComboBox cbSelectedMemoryDomain;
		private System.Windows.Forms.Label lbAutoAddEvery;
		public System.Windows.Forms.CheckBox cbUseCorePrecision;
	}
}