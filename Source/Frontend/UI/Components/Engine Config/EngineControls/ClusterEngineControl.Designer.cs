namespace RTCV.UI.Components.EngineConfig.EngineControls
{
    partial class ClusterEngineControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
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
            this.label22 = new System.Windows.Forms.Label();
            this.engineGroupBox.SuspendLayout();
            this.pnClusterLimiterList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.clusterChunkModifier)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clusterChunkSize)).BeginInit();
            this.SuspendLayout();
            // 
            // engineGroupBox
            // 
            this.engineGroupBox.Controls.Add(this.pnClusterLimiterList);
            this.engineGroupBox.Controls.Add(this.label22);
            this.engineGroupBox.Controls.SetChildIndex(this.placeholderComboBox, 0);
            this.engineGroupBox.Controls.SetChildIndex(this.label22, 0);
            this.engineGroupBox.Controls.SetChildIndex(this.pnClusterLimiterList, 0);
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
            this.pnClusterLimiterList.Location = new System.Drawing.Point(5, 36);
            this.pnClusterLimiterList.Name = "pnClusterLimiterList";
            this.pnClusterLimiterList.Size = new System.Drawing.Size(408, 104);
            this.pnClusterLimiterList.TabIndex = 149;
            this.pnClusterLimiterList.Tag = "color:dark2";
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
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label29.ForeColor = System.Drawing.Color.White;
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
            this.label28.ForeColor = System.Drawing.Color.White;
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
            this.clusterChunkModifier.ValueChanged += new System.EventHandler(this.UpdateClusterModifier);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label25.ForeColor = System.Drawing.Color.White;
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
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(176, 3);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(106, 13);
            this.label11.TabIndex = 142;
            this.label11.Text = "Cluster Chunk Size:";
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
            this.clusterChunkSize.ValueChanged += new System.EventHandler(this.UpdateClusterChunkSize);
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
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label12.ForeColor = System.Drawing.Color.White;
            this.label12.Location = new System.Drawing.Point(6, 4);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(62, 13);
            this.label12.TabIndex = 79;
            this.label12.Text = "Limiter list:";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic);
            this.label22.ForeColor = System.Drawing.Color.White;
            this.label22.Location = new System.Drawing.Point(169, 12);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(173, 13);
            this.label22.TabIndex = 148;
            this.label22.Text = "Swaps Values with neighbor Values";
            // 
            // ClusterEngineControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "ClusterEngineControl";
            this.engineGroupBox.ResumeLayout(false);
            this.engineGroupBox.PerformLayout();
            this.pnClusterLimiterList.ResumeLayout(false);
            this.pnClusterLimiterList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.clusterChunkModifier)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clusterChunkSize)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnClusterLimiterList;
        public System.Windows.Forms.CheckBox clusterFilterAll;
        private System.Windows.Forms.Label label29;
        public System.Windows.Forms.ComboBox clusterDirection;
        public System.Windows.Forms.CheckBox clusterSplitUnits;
        private System.Windows.Forms.Label label28;
        internal System.Windows.Forms.NumericUpDown clusterChunkModifier;
        private System.Windows.Forms.Label label25;
        public System.Windows.Forms.ComboBox cbClusterMethod;
        private System.Windows.Forms.Label label11;
        internal System.Windows.Forms.NumericUpDown clusterChunkSize;
        public System.Windows.Forms.ComboBox cbClusterLimiterList;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label22;
    }
}
