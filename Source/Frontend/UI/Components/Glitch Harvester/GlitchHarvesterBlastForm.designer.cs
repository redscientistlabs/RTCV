namespace RTCV.UI
{
    partial class GlitchHarvesterBlastForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GlitchHarvesterBlastForm));
            this.btnRenderOutput = new System.Windows.Forms.Button();
            this.btnGlitchHarvesterSettings = new System.Windows.Forms.Button();
            this.btnRerollSelected = new System.Windows.Forms.Button();
            this.btnCorrupt = new System.Windows.Forms.Button();
            this.btnBlastToggle = new System.Windows.Forms.Button();
            this.btnSendRaw = new System.Windows.Forms.Button();
            this.btnNewBlastLayerEditor = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnRenderOutput
            // 
            this.btnRenderOutput.BackColor = System.Drawing.Color.Gray;
            this.btnRenderOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRenderOutput.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnRenderOutput.FlatAppearance.BorderSize = 0;
            this.btnRenderOutput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRenderOutput.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnRenderOutput.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnRenderOutput.Image = ((System.Drawing.Image)(resources.GetObject("btnRenderOutput.Image")));
            this.btnRenderOutput.Location = new System.Drawing.Point(120, 35);
            this.btnRenderOutput.Margin = new System.Windows.Forms.Padding(2, 1, 0, 1);
            this.btnRenderOutput.Name = "btnRenderOutput";
            this.btnRenderOutput.Size = new System.Drawing.Size(32, 32);
            this.btnRenderOutput.TabIndex = 141;
            this.btnRenderOutput.TabStop = false;
            this.btnRenderOutput.Tag = "color:light1";
            this.btnRenderOutput.UseVisualStyleBackColor = false;
            this.btnRenderOutput.MouseDown += new System.Windows.Forms.MouseEventHandler(this.RenderOutput);
            // 
            // btnGlitchHarvesterSettings
            // 
            this.btnGlitchHarvesterSettings.BackColor = System.Drawing.Color.Gray;
            this.btnGlitchHarvesterSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGlitchHarvesterSettings.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnGlitchHarvesterSettings.FlatAppearance.BorderSize = 0;
            this.btnGlitchHarvesterSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGlitchHarvesterSettings.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnGlitchHarvesterSettings.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnGlitchHarvesterSettings.Image = ((System.Drawing.Image)(resources.GetObject("btnGlitchHarvesterSettings.Image")));
            this.btnGlitchHarvesterSettings.Location = new System.Drawing.Point(120, 0);
            this.btnGlitchHarvesterSettings.Margin = new System.Windows.Forms.Padding(2, 0, 0, 2);
            this.btnGlitchHarvesterSettings.Name = "btnGlitchHarvesterSettings";
            this.btnGlitchHarvesterSettings.Size = new System.Drawing.Size(32, 32);
            this.btnGlitchHarvesterSettings.TabIndex = 140;
            this.btnGlitchHarvesterSettings.TabStop = false;
            this.btnGlitchHarvesterSettings.Tag = "color:light1";
            this.btnGlitchHarvesterSettings.UseVisualStyleBackColor = false;
            this.btnGlitchHarvesterSettings.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OpenGlitchHarvesterSettings);
            // 
            // btnRerollSelected
            // 
            this.btnRerollSelected.BackColor = System.Drawing.Color.Gray;
            this.btnRerollSelected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRerollSelected.FlatAppearance.BorderSize = 0;
            this.btnRerollSelected.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRerollSelected.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnRerollSelected.ForeColor = System.Drawing.Color.White;
            this.btnRerollSelected.Location = new System.Drawing.Point(0, 99);
            this.btnRerollSelected.Margin = new System.Windows.Forms.Padding(0, 3, 2, 0);
            this.btnRerollSelected.Name = "btnRerollSelected";
            this.btnRerollSelected.Size = new System.Drawing.Size(47, 26);
            this.btnRerollSelected.TabIndex = 133;
            this.btnRerollSelected.TabStop = false;
            this.btnRerollSelected.Tag = "color:light1";
            this.btnRerollSelected.Text = "Reroll";
            this.btnRerollSelected.UseVisualStyleBackColor = false;
            this.btnRerollSelected.Click += new System.EventHandler(this.RerollSelected);
            this.btnRerollSelected.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnRerollButtonMouseDown);
            // 
            // btnCorrupt
            // 
            this.btnCorrupt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.tableLayoutPanel1.SetColumnSpan(this.btnCorrupt, 2);
            this.btnCorrupt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCorrupt.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnCorrupt.FlatAppearance.BorderSize = 0;
            this.btnCorrupt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCorrupt.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnCorrupt.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnCorrupt.Image = ((System.Drawing.Image)(resources.GetObject("btnCorrupt.Image")));
            this.btnCorrupt.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCorrupt.Location = new System.Drawing.Point(0, 0);
            this.btnCorrupt.Margin = new System.Windows.Forms.Padding(0, 0, 1, 2);
            this.btnCorrupt.Name = "btnCorrupt";
            this.btnCorrupt.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.btnCorrupt.Size = new System.Drawing.Size(117, 32);
            this.btnCorrupt.TabIndex = 137;
            this.btnCorrupt.TabStop = false;
            this.btnCorrupt.Tag = "color:dark2";
            this.btnCorrupt.Text = "  Corrupt";
            this.btnCorrupt.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCorrupt.UseVisualStyleBackColor = false;
            this.btnCorrupt.Click += new System.EventHandler(this.Corrupt);
            this.btnCorrupt.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnCorrupt_MouseDown);
            // 
            // btnBlastToggle
            // 
            this.btnBlastToggle.BackColor = System.Drawing.Color.DimGray;
            this.tableLayoutPanel1.SetColumnSpan(this.btnBlastToggle, 2);
            this.btnBlastToggle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnBlastToggle.FlatAppearance.BorderSize = 0;
            this.btnBlastToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBlastToggle.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnBlastToggle.ForeColor = System.Drawing.Color.White;
            this.btnBlastToggle.Location = new System.Drawing.Point(50, 99);
            this.btnBlastToggle.Margin = new System.Windows.Forms.Padding(1, 3, 0, 0);
            this.btnBlastToggle.Name = "btnBlastToggle";
            this.btnBlastToggle.Size = new System.Drawing.Size(102, 26);
            this.btnBlastToggle.TabIndex = 131;
            this.btnBlastToggle.TabStop = false;
            this.btnBlastToggle.Tag = "color:dark2";
            this.btnBlastToggle.Text = "BlastLayer : OFF";
            this.btnBlastToggle.UseVisualStyleBackColor = false;
            this.btnBlastToggle.Click += new System.EventHandler(this.BlastLayerToggle);
            // 
            // btnSendRaw
            // 
            this.btnSendRaw.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.tableLayoutPanel1.SetColumnSpan(this.btnSendRaw, 2);
            this.btnSendRaw.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSendRaw.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnSendRaw.FlatAppearance.BorderSize = 0;
            this.btnSendRaw.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSendRaw.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnSendRaw.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnSendRaw.Image = ((System.Drawing.Image)(resources.GetObject("btnSendRaw.Image")));
            this.btnSendRaw.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSendRaw.Location = new System.Drawing.Point(0, 35);
            this.btnSendRaw.Margin = new System.Windows.Forms.Padding(0, 1, 1, 1);
            this.btnSendRaw.Name = "btnSendRaw";
            this.btnSendRaw.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.btnSendRaw.Size = new System.Drawing.Size(117, 32);
            this.btnSendRaw.TabIndex = 139;
            this.btnSendRaw.TabStop = false;
            this.btnSendRaw.Tag = "color:dark2";
            this.btnSendRaw.Text = "  Raw to Stash";
            this.btnSendRaw.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSendRaw.UseVisualStyleBackColor = false;
            this.btnSendRaw.Click += new System.EventHandler(this.SendRawToStash);
            // 
            // btnNewBlastLayerEditor
            // 
            this.btnNewBlastLayerEditor.BackColor = System.Drawing.Color.Gray;
            this.tableLayoutPanel1.SetColumnSpan(this.btnNewBlastLayerEditor, 3);
            this.btnNewBlastLayerEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNewBlastLayerEditor.FlatAppearance.BorderSize = 0;
            this.btnNewBlastLayerEditor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewBlastLayerEditor.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnNewBlastLayerEditor.ForeColor = System.Drawing.Color.White;
            this.btnNewBlastLayerEditor.Location = new System.Drawing.Point(0, 70);
            this.btnNewBlastLayerEditor.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.btnNewBlastLayerEditor.Name = "btnNewBlastLayerEditor";
            this.btnNewBlastLayerEditor.Size = new System.Drawing.Size(152, 26);
            this.btnNewBlastLayerEditor.TabIndex = 142;
            this.btnNewBlastLayerEditor.TabStop = false;
            this.btnNewBlastLayerEditor.Tag = "color:light1";
            this.btnNewBlastLayerEditor.Text = "New Empty BlastLayer";
            this.btnNewBlastLayerEditor.UseVisualStyleBackColor = false;
            this.btnNewBlastLayerEditor.Click += new System.EventHandler(this.btnNewBlastLayerEditor_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.8877F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.6338F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.4785F));
            this.tableLayoutPanel1.Controls.Add(this.btnCorrupt, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnBlastToggle, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnRerollSelected, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnNewBlastLayerEditor, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnSendRaw, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnRenderOutput, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnGlitchHarvesterSettings, 2, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 13);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 27.42239F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 27.42239F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 22.88136F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 22.27386F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(152, 125);
            this.tableLayoutPanel1.TabIndex = 143;
            // 
            // GlitchHarvesterBlastForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(176, 152);
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(176, 152);
            this.Name = "GlitchHarvesterBlastForm";
            this.Tag = "color:dark1";
            this.Text = "Blast Tools";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HandleFormClosing);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Button btnCorrupt;
        public System.Windows.Forms.Button btnSendRaw;
        public System.Windows.Forms.Button btnRerollSelected;
        public System.Windows.Forms.Button btnBlastToggle;
        public System.Windows.Forms.Button btnGlitchHarvesterSettings;
        public System.Windows.Forms.Button btnRenderOutput;
        public System.Windows.Forms.Button btnNewBlastLayerEditor;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
