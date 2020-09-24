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
            this.gbVectorEngine = new System.Windows.Forms.GroupBox();
            this.cbVectorUnlockPrecision = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbVectorEngineValueText1 = new System.Windows.Forms.Label();
            this.cbVectorValueList = new System.Windows.Forms.ComboBox();
            this.label18 = new System.Windows.Forms.Label();
            this.pnLimiterList = new System.Windows.Forms.Panel();
            this.lbVectorEngineLimiterText1 = new System.Windows.Forms.Label();
            this.cbVectorLimiterList = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.comboBox6 = new System.Windows.Forms.ComboBox();
            this.label19 = new System.Windows.Forms.Label();
            this.gbClusterEngine = new System.Windows.Forms.GroupBox();
            this.pnClusterLimiterList = new System.Windows.Forms.Panel();
            this.clusterFilterAll = new System.Windows.Forms.CheckBox();
            this.label29 = new System.Windows.Forms.Label();
            this.clusterDirection = new System.Windows.Forms.ComboBox();
            this.clusterSplitUnits = new System.Windows.Forms.CheckBox();
            this.label28 = new System.Windows.Forms.Label();
            this.clusterChunkModifier = new System.Windows.Forms.NumericUpDown();
            this.label25 = new System.Windows.Forms.Label();
            this.cbClusterMethod = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.clusterChunkSize = new System.Windows.Forms.NumericUpDown();
            this.cbClusterLimiterList = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.comboBox9 = new System.Windows.Forms.ComboBox();
            this.label22 = new System.Windows.Forms.Label();
            this.pnCustomPrecision.SuspendLayout();
            this.gbVectorEngine.SuspendLayout();
            this.panel2.SuspendLayout();
            this.pnLimiterList.SuspendLayout();
            this.gbClusterEngine.SuspendLayout();
            this.pnClusterLimiterList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.clusterChunkModifier)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clusterChunkSize)).BeginInit();
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
            // gbVectorEngine
            // 
            this.gbVectorEngine.Controls.Add(this.cbVectorUnlockPrecision);
            this.gbVectorEngine.Controls.Add(this.panel2);
            this.gbVectorEngine.Controls.Add(this.pnLimiterList);
            this.gbVectorEngine.Controls.Add(this.comboBox6);
            this.gbVectorEngine.Controls.Add(this.label19);
            this.gbVectorEngine.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.gbVectorEngine.ForeColor = System.Drawing.Color.White;
            this.gbVectorEngine.Location = new System.Drawing.Point(880, 156);
            this.gbVectorEngine.Name = "gbVectorEngine";
            this.gbVectorEngine.Size = new System.Drawing.Size(420, 151);
            this.gbVectorEngine.TabIndex = 144;
            this.gbVectorEngine.TabStop = false;
            this.gbVectorEngine.Visible = false;
            this.gbVectorEngine.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // cbVectorUnlockPrecision
            // 
            this.cbVectorUnlockPrecision.AutoSize = true;
            this.cbVectorUnlockPrecision.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic);
            this.cbVectorUnlockPrecision.ForeColor = System.Drawing.Color.White;
            this.cbVectorUnlockPrecision.Location = new System.Drawing.Point(357, 15);
            this.cbVectorUnlockPrecision.Name = "cbVectorUnlockPrecision";
            this.cbVectorUnlockPrecision.Size = new System.Drawing.Size(59, 17);
            this.cbVectorUnlockPrecision.TabIndex = 144;
            this.cbVectorUnlockPrecision.Text = "Unlock";
            this.cbVectorUnlockPrecision.UseVisualStyleBackColor = true;
            this.cbVectorUnlockPrecision.CheckedChanged += new System.EventHandler(this.UpdateVectorUnlockPrecision);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.panel2.Controls.Add(this.lbVectorEngineValueText1);
            this.panel2.Controls.Add(this.cbVectorValueList);
            this.panel2.Controls.Add(this.label18);
            this.panel2.Location = new System.Drawing.Point(6, 94);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(408, 47);
            this.panel2.TabIndex = 135;
            this.panel2.Tag = "color:dark2";
            this.panel2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // lbVectorEngineValueText1
            // 
            this.lbVectorEngineValueText1.AutoSize = true;
            this.lbVectorEngineValueText1.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lbVectorEngineValueText1.Location = new System.Drawing.Point(165, 25);
            this.lbVectorEngineValueText1.Name = "lbVectorEngineValueText1";
            this.lbVectorEngineValueText1.Size = new System.Drawing.Size(108, 13);
            this.lbVectorEngineValueText1.TabIndex = 138;
            this.lbVectorEngineValueText1.Text = "Replacement values";
            this.lbVectorEngineValueText1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // cbVectorValueList
            // 
            this.cbVectorValueList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.cbVectorValueList.DataSource = ((object)(resources.GetObject("cbVectorValueList.DataSource")));
            this.cbVectorValueList.DisplayMember = "Name";
            this.cbVectorValueList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVectorValueList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbVectorValueList.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbVectorValueList.ForeColor = System.Drawing.Color.White;
            this.cbVectorValueList.FormattingEnabled = true;
            this.cbVectorValueList.IntegralHeight = false;
            this.cbVectorValueList.Location = new System.Drawing.Point(8, 19);
            this.cbVectorValueList.MaxDropDownItems = 15;
            this.cbVectorValueList.Name = "cbVectorValueList";
            this.cbVectorValueList.Size = new System.Drawing.Size(152, 21);
            this.cbVectorValueList.TabIndex = 81;
            this.cbVectorValueList.Tag = "color:normal";
            this.cbVectorValueList.ValueMember = "Value";
            this.cbVectorValueList.SelectedIndexChanged += new System.EventHandler(this.UpdateVectorValueList);
            this.cbVectorValueList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label18.Location = new System.Drawing.Point(5, 4);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(56, 13);
            this.label18.TabIndex = 80;
            this.label18.Text = "Value list:";
            this.label18.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // pnLimiterList
            // 
            this.pnLimiterList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.pnLimiterList.Controls.Add(this.lbVectorEngineLimiterText1);
            this.pnLimiterList.Controls.Add(this.cbVectorLimiterList);
            this.pnLimiterList.Controls.Add(this.label13);
            this.pnLimiterList.Location = new System.Drawing.Point(6, 40);
            this.pnLimiterList.Name = "pnLimiterList";
            this.pnLimiterList.Size = new System.Drawing.Size(408, 47);
            this.pnLimiterList.TabIndex = 134;
            this.pnLimiterList.Tag = "color:dark2";
            this.pnLimiterList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // lbVectorEngineLimiterText1
            // 
            this.lbVectorEngineLimiterText1.AutoSize = true;
            this.lbVectorEngineLimiterText1.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lbVectorEngineLimiterText1.Location = new System.Drawing.Point(165, 24);
            this.lbVectorEngineLimiterText1.Name = "lbVectorEngineLimiterText1";
            this.lbVectorEngineLimiterText1.Size = new System.Drawing.Size(104, 13);
            this.lbVectorEngineLimiterText1.TabIndex = 141;
            this.lbVectorEngineLimiterText1.Text = "Comparison values";
            this.lbVectorEngineLimiterText1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // cbVectorLimiterList
            // 
            this.cbVectorLimiterList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.cbVectorLimiterList.DataSource = ((object)(resources.GetObject("cbVectorLimiterList.DataSource")));
            this.cbVectorLimiterList.DisplayMember = "Name";
            this.cbVectorLimiterList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVectorLimiterList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbVectorLimiterList.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbVectorLimiterList.ForeColor = System.Drawing.Color.White;
            this.cbVectorLimiterList.FormattingEnabled = true;
            this.cbVectorLimiterList.IntegralHeight = false;
            this.cbVectorLimiterList.Location = new System.Drawing.Point(8, 19);
            this.cbVectorLimiterList.MaxDropDownItems = 15;
            this.cbVectorLimiterList.Name = "cbVectorLimiterList";
            this.cbVectorLimiterList.Size = new System.Drawing.Size(152, 21);
            this.cbVectorLimiterList.TabIndex = 78;
            this.cbVectorLimiterList.Tag = "color:normal";
            this.cbVectorLimiterList.ValueMember = "Value";
            this.cbVectorLimiterList.SelectedIndexChanged += new System.EventHandler(this.UpdateVectorLimiterList);
            this.cbVectorLimiterList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label13.Location = new System.Drawing.Point(6, 4);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(62, 13);
            this.label13.TabIndex = 79;
            this.label13.Text = "Limiter list:";
            this.label13.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // comboBox6
            // 
            this.comboBox6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.comboBox6.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox6.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.comboBox6.ForeColor = System.Drawing.Color.White;
            this.comboBox6.FormattingEnabled = true;
            this.comboBox6.Location = new System.Drawing.Point(0, 9);
            this.comboBox6.Name = "comboBox6";
            this.comboBox6.Size = new System.Drawing.Size(165, 21);
            this.comboBox6.TabIndex = 82;
            this.comboBox6.Tag = "color:dark1";
            this.comboBox6.Visible = false;
            this.comboBox6.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic);
            this.label19.Location = new System.Drawing.Point(170, 16);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(161, 13);
            this.label19.TabIndex = 77;
            this.label19.Text = "Corrupts 32-bit values using lists";
            this.label19.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // gbClusterEngine
            // 
            this.gbClusterEngine.Controls.Add(this.pnClusterLimiterList);
            this.gbClusterEngine.Controls.Add(this.comboBox9);
            this.gbClusterEngine.Controls.Add(this.label22);
            this.gbClusterEngine.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.gbClusterEngine.ForeColor = System.Drawing.Color.White;
            this.gbClusterEngine.Location = new System.Drawing.Point(880, 308);
            this.gbClusterEngine.Name = "gbClusterEngine";
            this.gbClusterEngine.Size = new System.Drawing.Size(420, 151);
            this.gbClusterEngine.TabIndex = 148;
            this.gbClusterEngine.TabStop = false;
            this.gbClusterEngine.Visible = false;
            // 
            // pnClusterLimiterList
            // 
            this.pnClusterLimiterList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.pnClusterLimiterList.Controls.Add(this.clusterFilterAll);
            this.pnClusterLimiterList.Controls.Add(this.label29);
            this.pnClusterLimiterList.Controls.Add(this.clusterDirection);
            this.pnClusterLimiterList.Controls.Add(this.clusterSplitUnits);
            this.pnClusterLimiterList.Controls.Add(this.label28);
            this.pnClusterLimiterList.Controls.Add(this.clusterChunkModifier);
            this.pnClusterLimiterList.Controls.Add(this.label25);
            this.pnClusterLimiterList.Controls.Add(this.cbClusterMethod);
            this.pnClusterLimiterList.Controls.Add(this.label11);
            this.pnClusterLimiterList.Controls.Add(this.clusterChunkSize);
            this.pnClusterLimiterList.Controls.Add(this.cbClusterLimiterList);
            this.pnClusterLimiterList.Controls.Add(this.label12);
            this.pnClusterLimiterList.Location = new System.Drawing.Point(6, 40);
            this.pnClusterLimiterList.Name = "pnClusterLimiterList";
            this.pnClusterLimiterList.Size = new System.Drawing.Size(408, 104);
            this.pnClusterLimiterList.TabIndex = 134;
            this.pnClusterLimiterList.Tag = "color:dark2";
            this.pnClusterLimiterList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // clusterFilterAll
            // 
            this.clusterFilterAll.AutoSize = true;
            this.clusterFilterAll.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.clusterFilterAll.ForeColor = System.Drawing.Color.White;
            this.clusterFilterAll.Location = new System.Drawing.Point(296, 27);
            this.clusterFilterAll.Name = "clusterFilterAll";
            this.clusterFilterAll.Size = new System.Drawing.Size(68, 17);
            this.clusterFilterAll.TabIndex = 151;
            this.clusterFilterAll.Text = "Filter All";
            this.clusterFilterAll.UseVisualStyleBackColor = true;
            this.clusterFilterAll.CheckedChanged += new System.EventHandler(this.UpdateClusterFilterAll);
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label29.Location = new System.Drawing.Point(293, 52);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(96, 13);
            this.label29.TabIndex = 150;
            this.label29.Text = "Cluster Direction:";
            // 
            // clusterDirection
            // 
            this.clusterDirection.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.clusterDirection.DisplayMember = "Name";
            this.clusterDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.clusterDirection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.clusterDirection.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.clusterDirection.ForeColor = System.Drawing.Color.White;
            this.clusterDirection.FormattingEnabled = true;
            this.clusterDirection.IntegralHeight = false;
            this.clusterDirection.Location = new System.Drawing.Point(296, 68);
            this.clusterDirection.MaxDropDownItems = 15;
            this.clusterDirection.Name = "clusterDirection";
            this.clusterDirection.Size = new System.Drawing.Size(106, 21);
            this.clusterDirection.TabIndex = 149;
            this.clusterDirection.Tag = "color:normal";
            this.clusterDirection.ValueMember = "Value";
            this.clusterDirection.SelectedIndexChanged += new System.EventHandler(this.UpdateClusterDirection);
            // 
            // clusterSplitUnits
            // 
            this.clusterSplitUnits.AutoSize = true;
            this.clusterSplitUnits.Checked = true;
            this.clusterSplitUnits.CheckState = System.Windows.Forms.CheckState.Checked;
            this.clusterSplitUnits.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.clusterSplitUnits.ForeColor = System.Drawing.Color.White;
            this.clusterSplitUnits.Location = new System.Drawing.Point(296, 4);
            this.clusterSplitUnits.Name = "clusterSplitUnits";
            this.clusterSplitUnits.Size = new System.Drawing.Size(106, 17);
            this.clusterSplitUnits.TabIndex = 144;
            this.clusterSplitUnits.Text = "Split Blast Units";
            this.clusterSplitUnits.UseVisualStyleBackColor = true;
            this.clusterSplitUnits.CheckedChanged += new System.EventHandler(this.UpdateClusterSplitUnits);
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label28.Location = new System.Drawing.Point(175, 52);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(88, 13);
            this.label28.TabIndex = 148;
            this.label28.Text = "Rotate Amount:";
            // 
            // clusterChunkModifier
            // 
            this.clusterChunkModifier.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.clusterChunkModifier.Enabled = false;
            this.clusterChunkModifier.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.clusterChunkModifier.ForeColor = System.Drawing.Color.White;
            this.clusterChunkModifier.Location = new System.Drawing.Point(178, 67);
            this.clusterChunkModifier.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.clusterChunkModifier.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.clusterChunkModifier.Name = "clusterChunkModifier";
            this.clusterChunkModifier.Size = new System.Drawing.Size(100, 22);
            this.clusterChunkModifier.TabIndex = 147;
            this.clusterChunkModifier.Tag = "color:normal";
            this.clusterChunkModifier.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label25.Location = new System.Drawing.Point(6, 52);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(51, 13);
            this.label25.TabIndex = 146;
            this.label25.Text = "Method:";
            // 
            // cbClusterMethod
            // 
            this.cbClusterMethod.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.cbClusterMethod.DisplayMember = "Name";
            this.cbClusterMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbClusterMethod.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbClusterMethod.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbClusterMethod.ForeColor = System.Drawing.Color.White;
            this.cbClusterMethod.FormattingEnabled = true;
            this.cbClusterMethod.IntegralHeight = false;
            this.cbClusterMethod.Location = new System.Drawing.Point(9, 68);
            this.cbClusterMethod.MaxDropDownItems = 15;
            this.cbClusterMethod.Name = "cbClusterMethod";
            this.cbClusterMethod.Size = new System.Drawing.Size(152, 21);
            this.cbClusterMethod.TabIndex = 145;
            this.cbClusterMethod.Tag = "color:normal";
            this.cbClusterMethod.ValueMember = "Value";
            this.cbClusterMethod.SelectedIndexChanged += new System.EventHandler(this.UpdateClusterMethod);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label11.Location = new System.Drawing.Point(176, 3);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(106, 13);
            this.label11.TabIndex = 142;
            this.label11.Text = "Cluster Chunk Size:";
            this.label11.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // clusterChunkSize
            // 
            this.clusterChunkSize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.clusterChunkSize.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.clusterChunkSize.ForeColor = System.Drawing.Color.White;
            this.clusterChunkSize.Location = new System.Drawing.Point(178, 19);
            this.clusterChunkSize.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.clusterChunkSize.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.clusterChunkSize.Name = "clusterChunkSize";
            this.clusterChunkSize.Size = new System.Drawing.Size(100, 22);
            this.clusterChunkSize.TabIndex = 144;
            this.clusterChunkSize.Tag = "color:normal";
            this.clusterChunkSize.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.clusterChunkSize.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // cbClusterLimiterList
            // 
            this.cbClusterLimiterList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.cbClusterLimiterList.DisplayMember = "Name";
            this.cbClusterLimiterList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbClusterLimiterList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbClusterLimiterList.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbClusterLimiterList.ForeColor = System.Drawing.Color.White;
            this.cbClusterLimiterList.FormattingEnabled = true;
            this.cbClusterLimiterList.IntegralHeight = false;
            this.cbClusterLimiterList.Location = new System.Drawing.Point(8, 19);
            this.cbClusterLimiterList.MaxDropDownItems = 15;
            this.cbClusterLimiterList.Name = "cbClusterLimiterList";
            this.cbClusterLimiterList.Size = new System.Drawing.Size(152, 21);
            this.cbClusterLimiterList.TabIndex = 78;
            this.cbClusterLimiterList.Tag = "color:normal";
            this.cbClusterLimiterList.ValueMember = "Value";
            this.cbClusterLimiterList.SelectedIndexChanged += new System.EventHandler(this.UpdateClusterLimiterList);
            this.cbClusterLimiterList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label12.Location = new System.Drawing.Point(6, 4);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(62, 13);
            this.label12.TabIndex = 79;
            this.label12.Text = "Limiter list:";
            this.label12.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // comboBox9
            // 
            this.comboBox9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.comboBox9.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox9.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.comboBox9.ForeColor = System.Drawing.Color.White;
            this.comboBox9.FormattingEnabled = true;
            this.comboBox9.Location = new System.Drawing.Point(0, 9);
            this.comboBox9.Name = "comboBox9";
            this.comboBox9.Size = new System.Drawing.Size(165, 21);
            this.comboBox9.TabIndex = 82;
            this.comboBox9.Tag = "color:dark1";
            this.comboBox9.Visible = false;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic);
            this.label22.Location = new System.Drawing.Point(170, 16);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(173, 13);
            this.label22.TabIndex = 77;
            this.label22.Text = "Swaps Values with neighbor Values";
            this.label22.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            // 
            // CorruptionEngineForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(1448, 1097);
            this.Controls.Add(this.gbClusterEngine);
            this.Controls.Add(this.gbVectorEngine);
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
            this.gbVectorEngine.ResumeLayout(false);
            this.gbVectorEngine.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.pnLimiterList.ResumeLayout(false);
            this.pnLimiterList.PerformLayout();
            this.gbClusterEngine.ResumeLayout(false);
            this.gbClusterEngine.PerformLayout();
            this.pnClusterLimiterList.ResumeLayout(false);
            this.pnClusterLimiterList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.clusterChunkModifier)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clusterChunkSize)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel pnCustomPrecision;
		public System.Windows.Forms.ComboBox cbCustomPrecision;
		public System.Windows.Forms.ComboBox cbSelectedEngine;
		private System.Windows.Forms.GroupBox gbSelectedEngine;
		private System.Windows.Forms.GroupBox gbVectorEngine;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label lbVectorEngineValueText1;
		public System.Windows.Forms.ComboBox cbVectorValueList;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.Panel pnLimiterList;
		private System.Windows.Forms.Label lbVectorEngineLimiterText1;
		public System.Windows.Forms.ComboBox cbVectorLimiterList;
		private System.Windows.Forms.Label label13;
		public System.Windows.Forms.ComboBox comboBox6;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
        public Components.Controls.MultiUpDown nmAlignment;
        private System.Windows.Forms.GroupBox gbClusterEngine;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown clusterChunkSize;
        private System.Windows.Forms.Panel pnClusterLimiterList;
        public System.Windows.Forms.ComboBox cbClusterLimiterList;
        private System.Windows.Forms.Label label12;
        public System.Windows.Forms.ComboBox comboBox9;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label25;
        public System.Windows.Forms.ComboBox cbClusterMethod;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.NumericUpDown clusterChunkModifier;
        public System.Windows.Forms.CheckBox clusterSplitUnits;
        private System.Windows.Forms.Label label29;
        public System.Windows.Forms.ComboBox clusterDirection;
        public System.Windows.Forms.CheckBox clusterFilterAll;
        public System.Windows.Forms.CheckBox cbVectorUnlockPrecision;
    }
}
