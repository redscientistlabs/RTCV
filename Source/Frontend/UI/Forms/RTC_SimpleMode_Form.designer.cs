namespace RTCV.UI
{
    partial class RTC_SimpleMode_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RTC_SimpleMode_Form));
            this.btnSwitchNormalMode = new System.Windows.Forms.Button();
            this.gbSimpleGlitchHarvester = new System.Windows.Forms.GroupBox();
            this.btnLoadGhSavestate = new System.Windows.Forms.Button();
            this.lbSimpleGlitchHarvesterHelp = new System.Windows.Forms.Label();
            this.btnCreateGhSavestate = new System.Windows.Forms.Button();
            this.btnGlitchHarvesterCorrupt = new System.Windows.Forms.Button();
            this.gbRealTimeCorruption = new System.Windows.Forms.GroupBox();
            this.lbIntensityHelp = new System.Windows.Forms.Label();
            this.pnIntensity = new System.Windows.Forms.Panel();
            this.btnManualBlast = new System.Windows.Forms.Button();
            this.btnAutoCorrupt = new System.Windows.Forms.Button();
            this.gbEngineParameters = new System.Windows.Forms.GroupBox();
            this.updownMaxInfiniteUnits = new RTCV.UI.Components.Controls.MultiUpDown();
            this.lbMaxUnits = new System.Windows.Forms.Label();
            this.cbClearRewind = new System.Windows.Forms.CheckBox();
            this.btnClearInfiniteUnits = new System.Windows.Forms.Button();
            this.lbEngineDescription = new System.Windows.Forms.Label();
            this.btnShuffleAlgorithm = new System.Windows.Forms.Button();
            this.gbTargetType = new System.Windows.Forms.GroupBox();
            this.btnHelp = new System.Windows.Forms.Button();
            this.rbModernPlatforms = new System.Windows.Forms.RadioButton();
            this.rbClassicPlatforms = new System.Windows.Forms.RadioButton();
            this.lbConnectionStatus = new System.Windows.Forms.Label();
            this.btnBlastToggle = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbSimpleGlitchHarvester.SuspendLayout();
            this.gbRealTimeCorruption.SuspendLayout();
            this.gbEngineParameters.SuspendLayout();
            this.gbTargetType.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSwitchNormalMode
            // 
            this.btnSwitchNormalMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSwitchNormalMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnSwitchNormalMode.FlatAppearance.BorderSize = 0;
            this.btnSwitchNormalMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSwitchNormalMode.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnSwitchNormalMode.ForeColor = System.Drawing.Color.White;
            this.btnSwitchNormalMode.Image = ((System.Drawing.Image)(resources.GetObject("btnSwitchNormalMode.Image")));
            this.btnSwitchNormalMode.Location = new System.Drawing.Point(499, 512);
            this.btnSwitchNormalMode.Name = "btnSwitchNormalMode";
            this.btnSwitchNormalMode.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.btnSwitchNormalMode.Size = new System.Drawing.Size(192, 32);
            this.btnSwitchNormalMode.TabIndex = 183;
            this.btnSwitchNormalMode.TabStop = false;
            this.btnSwitchNormalMode.Tag = "color:dark2";
            this.btnSwitchNormalMode.Text = "  Switch to Normal Mode";
            this.btnSwitchNormalMode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSwitchNormalMode.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSwitchNormalMode.UseVisualStyleBackColor = false;
            this.btnSwitchNormalMode.Click += new System.EventHandler(this.btnSwitchNormalMode_Click);
            // 
            // gbSimpleGlitchHarvester
            // 
            this.gbSimpleGlitchHarvester.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbSimpleGlitchHarvester.Controls.Add(this.btnLoadGhSavestate);
            this.gbSimpleGlitchHarvester.Controls.Add(this.lbSimpleGlitchHarvesterHelp);
            this.gbSimpleGlitchHarvester.Controls.Add(this.btnCreateGhSavestate);
            this.gbSimpleGlitchHarvester.Controls.Add(this.btnGlitchHarvesterCorrupt);
            this.gbSimpleGlitchHarvester.ForeColor = System.Drawing.Color.White;
            this.gbSimpleGlitchHarvester.Location = new System.Drawing.Point(14, 391);
            this.gbSimpleGlitchHarvester.Name = "gbSimpleGlitchHarvester";
            this.gbSimpleGlitchHarvester.Size = new System.Drawing.Size(677, 108);
            this.gbSimpleGlitchHarvester.TabIndex = 182;
            this.gbSimpleGlitchHarvester.TabStop = false;
            this.gbSimpleGlitchHarvester.Text = "Simple Glitch Harvester";
            this.gbSimpleGlitchHarvester.Visible = false;
            // 
            // btnLoadGhSavestate
            // 
            this.btnLoadGhSavestate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnLoadGhSavestate.FlatAppearance.BorderSize = 0;
            this.btnLoadGhSavestate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadGhSavestate.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnLoadGhSavestate.ForeColor = System.Drawing.Color.White;
            this.btnLoadGhSavestate.Image = ((System.Drawing.Image)(resources.GetObject("btnLoadGhSavestate.Image")));
            this.btnLoadGhSavestate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLoadGhSavestate.Location = new System.Drawing.Point(11, 63);
            this.btnLoadGhSavestate.Name = "btnLoadGhSavestate";
            this.btnLoadGhSavestate.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.btnLoadGhSavestate.Size = new System.Drawing.Size(156, 32);
            this.btnLoadGhSavestate.TabIndex = 170;
            this.btnLoadGhSavestate.TabStop = false;
            this.btnLoadGhSavestate.Tag = "color:dark2";
            this.btnLoadGhSavestate.Text = "  Load Savestate";
            this.btnLoadGhSavestate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLoadGhSavestate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLoadGhSavestate.UseVisualStyleBackColor = false;
            this.btnLoadGhSavestate.Click += new System.EventHandler(this.btnLoadGhSavestate_Click);
            // 
            // lbSimpleGlitchHarvesterHelp
            // 
            this.lbSimpleGlitchHarvesterHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbSimpleGlitchHarvesterHelp.BackColor = System.Drawing.Color.Transparent;
            this.lbSimpleGlitchHarvesterHelp.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lbSimpleGlitchHarvesterHelp.ForeColor = System.Drawing.Color.White;
            this.lbSimpleGlitchHarvesterHelp.Location = new System.Drawing.Point(394, 32);
            this.lbSimpleGlitchHarvesterHelp.Name = "lbSimpleGlitchHarvesterHelp";
            this.lbSimpleGlitchHarvesterHelp.Size = new System.Drawing.Size(271, 53);
            this.lbSimpleGlitchHarvesterHelp.TabIndex = 135;
            this.lbSimpleGlitchHarvesterHelp.Text = "This is a very simplified version of RTC\'s Glitch Harvester. It allows you to cre" +
    "ate a savestate and then corrupt it as many times you want.";
            // 
            // btnCreateGhSavestate
            // 
            this.btnCreateGhSavestate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreateGhSavestate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnCreateGhSavestate.FlatAppearance.BorderSize = 0;
            this.btnCreateGhSavestate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreateGhSavestate.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnCreateGhSavestate.ForeColor = System.Drawing.Color.White;
            this.btnCreateGhSavestate.Image = ((System.Drawing.Image)(resources.GetObject("btnCreateGhSavestate.Image")));
            this.btnCreateGhSavestate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCreateGhSavestate.Location = new System.Drawing.Point(11, 22);
            this.btnCreateGhSavestate.Name = "btnCreateGhSavestate";
            this.btnCreateGhSavestate.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.btnCreateGhSavestate.Size = new System.Drawing.Size(364, 32);
            this.btnCreateGhSavestate.TabIndex = 169;
            this.btnCreateGhSavestate.TabStop = false;
            this.btnCreateGhSavestate.Tag = "color:dark2";
            this.btnCreateGhSavestate.Text = "  Create and select a Glitch Harvester savestate";
            this.btnCreateGhSavestate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCreateGhSavestate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCreateGhSavestate.UseVisualStyleBackColor = false;
            this.btnCreateGhSavestate.Click += new System.EventHandler(this.btnCreateGhSavestate_Click);
            // 
            // btnGlitchHarvesterCorrupt
            // 
            this.btnGlitchHarvesterCorrupt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGlitchHarvesterCorrupt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnGlitchHarvesterCorrupt.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnGlitchHarvesterCorrupt.FlatAppearance.BorderSize = 0;
            this.btnGlitchHarvesterCorrupt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGlitchHarvesterCorrupt.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnGlitchHarvesterCorrupt.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnGlitchHarvesterCorrupt.Image = ((System.Drawing.Image)(resources.GetObject("btnGlitchHarvesterCorrupt.Image")));
            this.btnGlitchHarvesterCorrupt.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGlitchHarvesterCorrupt.Location = new System.Drawing.Point(201, 62);
            this.btnGlitchHarvesterCorrupt.Name = "btnGlitchHarvesterCorrupt";
            this.btnGlitchHarvesterCorrupt.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.btnGlitchHarvesterCorrupt.Size = new System.Drawing.Size(174, 32);
            this.btnGlitchHarvesterCorrupt.TabIndex = 138;
            this.btnGlitchHarvesterCorrupt.TabStop = false;
            this.btnGlitchHarvesterCorrupt.Tag = "color:dark2";
            this.btnGlitchHarvesterCorrupt.Text = "  Load and Corrupt";
            this.btnGlitchHarvesterCorrupt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGlitchHarvesterCorrupt.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnGlitchHarvesterCorrupt.UseVisualStyleBackColor = false;
            this.btnGlitchHarvesterCorrupt.Click += new System.EventHandler(this.btnGlitchHarvesterCorrupt_Click);
            // 
            // gbRealTimeCorruption
            // 
            this.gbRealTimeCorruption.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbRealTimeCorruption.Controls.Add(this.lbIntensityHelp);
            this.gbRealTimeCorruption.Controls.Add(this.pnIntensity);
            this.gbRealTimeCorruption.Controls.Add(this.btnManualBlast);
            this.gbRealTimeCorruption.Controls.Add(this.btnAutoCorrupt);
            this.gbRealTimeCorruption.ForeColor = System.Drawing.Color.White;
            this.gbRealTimeCorruption.Location = new System.Drawing.Point(13, 266);
            this.gbRealTimeCorruption.Name = "gbRealTimeCorruption";
            this.gbRealTimeCorruption.Size = new System.Drawing.Size(678, 119);
            this.gbRealTimeCorruption.TabIndex = 181;
            this.gbRealTimeCorruption.TabStop = false;
            this.gbRealTimeCorruption.Text = "Real-Time Corruption";
            this.gbRealTimeCorruption.Visible = false;
            // 
            // lbIntensityHelp
            // 
            this.lbIntensityHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbIntensityHelp.BackColor = System.Drawing.Color.Transparent;
            this.lbIntensityHelp.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lbIntensityHelp.ForeColor = System.Drawing.Color.White;
            this.lbIntensityHelp.Location = new System.Drawing.Point(396, 70);
            this.lbIntensityHelp.Name = "lbIntensityHelp";
            this.lbIntensityHelp.Size = new System.Drawing.Size(265, 37);
            this.lbIntensityHelp.TabIndex = 5;
            this.lbIntensityHelp.Text = "The intensity controls the power of a Manual Blast. Auto-Corrupt fires a Blast ev" +
    "ery frame.";
            // 
            // pnIntensity
            // 
            this.pnIntensity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnIntensity.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.pnIntensity.Location = new System.Drawing.Point(3, 14);
            this.pnIntensity.Name = "pnIntensity";
            this.pnIntensity.Size = new System.Drawing.Size(377, 97);
            this.pnIntensity.TabIndex = 134;
            // 
            // btnManualBlast
            // 
            this.btnManualBlast.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnManualBlast.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.btnManualBlast.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnManualBlast.FlatAppearance.BorderSize = 0;
            this.btnManualBlast.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnManualBlast.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.btnManualBlast.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnManualBlast.Image = ((System.Drawing.Image)(resources.GetObject("btnManualBlast.Image")));
            this.btnManualBlast.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnManualBlast.Location = new System.Drawing.Point(385, 26);
            this.btnManualBlast.Name = "btnManualBlast";
            this.btnManualBlast.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.btnManualBlast.Size = new System.Drawing.Size(118, 34);
            this.btnManualBlast.TabIndex = 121;
            this.btnManualBlast.TabStop = false;
            this.btnManualBlast.Tag = "color:dark3";
            this.btnManualBlast.Text = " Manual Blast";
            this.btnManualBlast.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnManualBlast.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnManualBlast.UseVisualStyleBackColor = false;
            this.btnManualBlast.Click += new System.EventHandler(this.btnManualBlast_Click);
            // 
            // btnAutoCorrupt
            // 
            this.btnAutoCorrupt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAutoCorrupt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.btnAutoCorrupt.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnAutoCorrupt.FlatAppearance.BorderSize = 0;
            this.btnAutoCorrupt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAutoCorrupt.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.btnAutoCorrupt.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnAutoCorrupt.Image = ((System.Drawing.Image)(resources.GetObject("btnAutoCorrupt.Image")));
            this.btnAutoCorrupt.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAutoCorrupt.Location = new System.Drawing.Point(515, 26);
            this.btnAutoCorrupt.Name = "btnAutoCorrupt";
            this.btnAutoCorrupt.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.btnAutoCorrupt.Size = new System.Drawing.Size(150, 34);
            this.btnAutoCorrupt.TabIndex = 122;
            this.btnAutoCorrupt.TabStop = false;
            this.btnAutoCorrupt.Tag = "color:dark3";
            this.btnAutoCorrupt.Text = " Start Auto-Corrupt";
            this.btnAutoCorrupt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAutoCorrupt.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAutoCorrupt.UseVisualStyleBackColor = false;
            this.btnAutoCorrupt.Click += new System.EventHandler(this.btnAutoCorrupt_Click);
            // 
            // gbEngineParameters
            // 
            this.gbEngineParameters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbEngineParameters.Controls.Add(this.updownMaxInfiniteUnits);
            this.gbEngineParameters.Controls.Add(this.lbMaxUnits);
            this.gbEngineParameters.Controls.Add(this.cbClearRewind);
            this.gbEngineParameters.Controls.Add(this.btnClearInfiniteUnits);
            this.gbEngineParameters.Controls.Add(this.lbEngineDescription);
            this.gbEngineParameters.Controls.Add(this.btnShuffleAlgorithm);
            this.gbEngineParameters.ForeColor = System.Drawing.Color.White;
            this.gbEngineParameters.Location = new System.Drawing.Point(13, 149);
            this.gbEngineParameters.Name = "gbEngineParameters";
            this.gbEngineParameters.Size = new System.Drawing.Size(678, 110);
            this.gbEngineParameters.TabIndex = 133;
            this.gbEngineParameters.TabStop = false;
            this.gbEngineParameters.Text = "Engine Parameters";
            this.gbEngineParameters.Visible = false;
            // 
            // updownMaxInfiniteUnits
            // 
            this.updownMaxInfiniteUnits.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.updownMaxInfiniteUnits.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.updownMaxInfiniteUnits.ForeColor = System.Drawing.Color.White;
            this.updownMaxInfiniteUnits.Hexadecimal = false;
            this.updownMaxInfiniteUnits.Location = new System.Drawing.Point(447, 77);
            this.updownMaxInfiniteUnits.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.updownMaxInfiniteUnits.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.updownMaxInfiniteUnits.Name = "updownMaxInfiniteUnits";
            this.updownMaxInfiniteUnits.Size = new System.Drawing.Size(70, 22);
            this.updownMaxInfiniteUnits.TabIndex = 196;
            this.updownMaxInfiniteUnits.Tag = "color:dark1";
            this.updownMaxInfiniteUnits.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.updownMaxInfiniteUnits.Visible = false;
            // 
            // lbMaxUnits
            // 
            this.lbMaxUnits.AutoSize = true;
            this.lbMaxUnits.Location = new System.Drawing.Point(378, 82);
            this.lbMaxUnits.Name = "lbMaxUnits";
            this.lbMaxUnits.Size = new System.Drawing.Size(63, 13);
            this.lbMaxUnits.TabIndex = 197;
            this.lbMaxUnits.Text = "Max ∞ Units";
            this.lbMaxUnits.Visible = false;
            // 
            // cbClearRewind
            // 
            this.cbClearRewind.AutoSize = true;
            this.cbClearRewind.BackColor = System.Drawing.Color.Transparent;
            this.cbClearRewind.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbClearRewind.ForeColor = System.Drawing.Color.White;
            this.cbClearRewind.Location = new System.Drawing.Point(377, 54);
            this.cbClearRewind.Name = "cbClearRewind";
            this.cbClearRewind.Size = new System.Drawing.Size(167, 17);
            this.cbClearRewind.TabIndex = 195;
            this.cbClearRewind.Text = "Clear Step Units on Rewind";
            this.cbClearRewind.UseVisualStyleBackColor = false;
            this.cbClearRewind.Visible = false;
            this.cbClearRewind.CheckedChanged += new System.EventHandler(this.CbClearRewind_CheckedChanged);
            // 
            // btnClearInfiniteUnits
            // 
            this.btnClearInfiniteUnits.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearInfiniteUnits.BackColor = System.Drawing.Color.Gray;
            this.btnClearInfiniteUnits.FlatAppearance.BorderSize = 0;
            this.btnClearInfiniteUnits.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearInfiniteUnits.Font = new System.Drawing.Font("Segoe UI Symbol", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnClearInfiniteUnits.ForeColor = System.Drawing.Color.White;
            this.btnClearInfiniteUnits.Location = new System.Drawing.Point(546, 51);
            this.btnClearInfiniteUnits.Name = "btnClearInfiniteUnits";
            this.btnClearInfiniteUnits.Size = new System.Drawing.Size(117, 23);
            this.btnClearInfiniteUnits.TabIndex = 180;
            this.btnClearInfiniteUnits.TabStop = false;
            this.btnClearInfiniteUnits.Tag = "color:light1";
            this.btnClearInfiniteUnits.Text = "Clear infinite units";
            this.btnClearInfiniteUnits.UseVisualStyleBackColor = false;
            this.btnClearInfiniteUnits.Visible = false;
            this.btnClearInfiniteUnits.Click += new System.EventHandler(this.btnClearInfiniteUnits_Click);
            // 
            // lbEngineDescription
            // 
            this.lbEngineDescription.AutoSize = true;
            this.lbEngineDescription.BackColor = System.Drawing.Color.Transparent;
            this.lbEngineDescription.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lbEngineDescription.ForeColor = System.Drawing.Color.White;
            this.lbEngineDescription.Location = new System.Drawing.Point(9, 26);
            this.lbEngineDescription.Name = "lbEngineDescription";
            this.lbEngineDescription.Size = new System.Drawing.Size(240, 65);
            this.lbEngineDescription.TabIndex = 5;
            this.lbEngineDescription.Text = "Auto-Selected Engine: Vector Engine \nParameters: Limiter:One , Value:Two\n\nThis en" +
    "gine is made for corrupting 3d games \nand 2d games made for 3d-era consoles.";
            // 
            // btnShuffleAlgorithm
            // 
            this.btnShuffleAlgorithm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShuffleAlgorithm.BackColor = System.Drawing.Color.Gray;
            this.btnShuffleAlgorithm.FlatAppearance.BorderSize = 0;
            this.btnShuffleAlgorithm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShuffleAlgorithm.Font = new System.Drawing.Font("Segoe UI Symbol", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnShuffleAlgorithm.ForeColor = System.Drawing.Color.White;
            this.btnShuffleAlgorithm.Location = new System.Drawing.Point(377, 22);
            this.btnShuffleAlgorithm.Name = "btnShuffleAlgorithm";
            this.btnShuffleAlgorithm.Size = new System.Drawing.Size(286, 23);
            this.btnShuffleAlgorithm.TabIndex = 179;
            this.btnShuffleAlgorithm.TabStop = false;
            this.btnShuffleAlgorithm.Tag = "color:light1";
            this.btnShuffleAlgorithm.Text = "Shuffle algorithm (Spice things up)";
            this.btnShuffleAlgorithm.UseVisualStyleBackColor = false;
            this.btnShuffleAlgorithm.Click += new System.EventHandler(this.btnShuffleAlgorithm_Click);
            // 
            // gbTargetType
            // 
            this.gbTargetType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbTargetType.Controls.Add(this.btnHelp);
            this.gbTargetType.Controls.Add(this.rbModernPlatforms);
            this.gbTargetType.Controls.Add(this.rbClassicPlatforms);
            this.gbTargetType.Controls.Add(this.lbConnectionStatus);
            this.gbTargetType.ForeColor = System.Drawing.Color.White;
            this.gbTargetType.Location = new System.Drawing.Point(13, 62);
            this.gbTargetType.Name = "gbTargetType";
            this.gbTargetType.Size = new System.Drawing.Size(680, 81);
            this.gbTargetType.TabIndex = 132;
            this.gbTargetType.TabStop = false;
            this.gbTargetType.Text = "Target Type";
            // 
            // btnHelp
            // 
            this.btnHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnHelp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnHelp.FlatAppearance.BorderSize = 0;
            this.btnHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHelp.Font = new System.Drawing.Font("Segoe UI Light", 7F, System.Drawing.FontStyle.Bold);
            this.btnHelp.ForeColor = System.Drawing.Color.White;
            this.btnHelp.Location = new System.Drawing.Point(657, 9);
            this.btnHelp.Margin = new System.Windows.Forms.Padding(0);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(20, 20);
            this.btnHelp.TabIndex = 184;
            this.btnHelp.TabStop = false;
            this.btnHelp.Tag = "color:dark2";
            this.btnHelp.Text = "?";
            this.btnHelp.UseVisualStyleBackColor = false;
            this.btnHelp.Visible = false;
            // 
            // rbModernPlatforms
            // 
            this.rbModernPlatforms.AutoSize = true;
            this.rbModernPlatforms.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.rbModernPlatforms.Location = new System.Drawing.Point(357, 43);
            this.rbModernPlatforms.Name = "rbModernPlatforms";
            this.rbModernPlatforms.Size = new System.Drawing.Size(286, 23);
            this.rbModernPlatforms.TabIndex = 4;
            this.rbModernPlatforms.TabStop = true;
            this.rbModernPlatforms.Text = "Modern Platforms (32bit/64bit, 3d games)";
            this.rbModernPlatforms.UseVisualStyleBackColor = true;
            this.rbModernPlatforms.CheckedChanged += new System.EventHandler(this.rbModernPlatforms_CheckedChanged);
            // 
            // rbClassicPlatforms
            // 
            this.rbClassicPlatforms.AutoSize = true;
            this.rbClassicPlatforms.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.rbClassicPlatforms.Location = new System.Drawing.Point(47, 42);
            this.rbClassicPlatforms.Name = "rbClassicPlatforms";
            this.rbClassicPlatforms.Size = new System.Drawing.Size(269, 23);
            this.rbClassicPlatforms.TabIndex = 3;
            this.rbClassicPlatforms.TabStop = true;
            this.rbClassicPlatforms.Text = "Classic Platforms (8bit/16bit, 2d games)";
            this.rbClassicPlatforms.UseVisualStyleBackColor = true;
            this.rbClassicPlatforms.CheckedChanged += new System.EventHandler(this.rbClassicPlatforms_CheckedChanged);
            // 
            // lbConnectionStatus
            // 
            this.lbConnectionStatus.AutoSize = true;
            this.lbConnectionStatus.BackColor = System.Drawing.Color.Transparent;
            this.lbConnectionStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lbConnectionStatus.ForeColor = System.Drawing.Color.White;
            this.lbConnectionStatus.Location = new System.Drawing.Point(152, 15);
            this.lbConnectionStatus.Name = "lbConnectionStatus";
            this.lbConnectionStatus.Size = new System.Drawing.Size(391, 19);
            this.lbConnectionStatus.TabIndex = 2;
            this.lbConnectionStatus.Text = "What would best describe the game you are trying to corrupt?";
            // 
            // btnBlastToggle
            // 
            this.btnBlastToggle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnBlastToggle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnBlastToggle.FlatAppearance.BorderSize = 0;
            this.btnBlastToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBlastToggle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnBlastToggle.ForeColor = System.Drawing.Color.White;
            this.btnBlastToggle.Location = new System.Drawing.Point(14, 512);
            this.btnBlastToggle.Name = "btnBlastToggle";
            this.btnBlastToggle.Size = new System.Drawing.Size(471, 32);
            this.btnBlastToggle.TabIndex = 131;
            this.btnBlastToggle.TabStop = false;
            this.btnBlastToggle.Tag = "color:dark2";
            this.btnBlastToggle.Text = "BlastLayer : OFF    (Attempts to uncorrupt/recorrupt in real-time)";
            this.btnBlastToggle.UseVisualStyleBackColor = false;
            this.btnBlastToggle.Visible = false;
            this.btnBlastToggle.Click += new System.EventHandler(this.btnBlastToggle_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 22F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(12, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(199, 41);
            this.label2.TabIndex = 83;
            this.label2.Text = "Simple Mode";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Item Name";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 238;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.FillWeight = 45F;
            this.dataGridViewTextBoxColumn2.HeaderText = "Game";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 113;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.FillWeight = 45F;
            this.dataGridViewTextBoxColumn3.HeaderText = "System";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 112;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.FillWeight = 40F;
            this.dataGridViewTextBoxColumn4.HeaderText = "Core";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Width = 101;
            // 
            // RTC_SimpleMode_Form
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(705, 558);
            this.Controls.Add(this.btnSwitchNormalMode);
            this.Controls.Add(this.gbSimpleGlitchHarvester);
            this.Controls.Add(this.gbRealTimeCorruption);
            this.Controls.Add(this.gbEngineParameters);
            this.Controls.Add(this.gbTargetType);
            this.Controls.Add(this.btnBlastToggle);
            this.Controls.Add(this.label2);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(655, 515);
            this.Name = "RTC_SimpleMode_Form";
            this.Tag = "color:dark1";
            this.Text = "Stockpile Player";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RTC_SimpleMode_Form_FormClosing);
            this.gbSimpleGlitchHarvester.ResumeLayout(false);
            this.gbRealTimeCorruption.ResumeLayout(false);
            this.gbEngineParameters.ResumeLayout(false);
            this.gbEngineParameters.PerformLayout();
            this.gbTargetType.ResumeLayout(false);
            this.gbTargetType.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
		public System.Windows.Forms.Button btnBlastToggle;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox gbTargetType;
        public System.Windows.Forms.Label lbConnectionStatus;
        private System.Windows.Forms.Button btnShuffleAlgorithm;
        private System.Windows.Forms.RadioButton rbModernPlatforms;
        private System.Windows.Forms.RadioButton rbClassicPlatforms;
        public System.Windows.Forms.Label lbEngineDescription;
        private System.Windows.Forms.Button btnClearInfiniteUnits;
        private System.Windows.Forms.GroupBox gbEngineParameters;
        public System.Windows.Forms.Button btnManualBlast;
        public System.Windows.Forms.Button btnAutoCorrupt;
        private System.Windows.Forms.GroupBox gbRealTimeCorruption;
        private System.Windows.Forms.GroupBox gbSimpleGlitchHarvester;
        public System.Windows.Forms.Label lbIntensityHelp;
        public System.Windows.Forms.Button btnGlitchHarvesterCorrupt;
        public System.Windows.Forms.Label lbSimpleGlitchHarvesterHelp;
        private System.Windows.Forms.Button btnCreateGhSavestate;
        public System.Windows.Forms.Panel pnIntensity;
        public System.Windows.Forms.Button btnSwitchNormalMode;
        public System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.Button btnLoadGhSavestate;
        public Components.Controls.MultiUpDown updownMaxInfiniteUnits;
        private System.Windows.Forms.Label lbMaxUnits;
        public System.Windows.Forms.CheckBox cbClearRewind;
    }
}