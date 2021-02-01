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
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            //
            // btnRenderOutput
            //
            this.btnRenderOutput.BackColor = System.Drawing.Color.Gray;
            this.btnRenderOutput.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnRenderOutput.FlatAppearance.BorderSize = 0;
            this.btnRenderOutput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRenderOutput.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnRenderOutput.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnRenderOutput.Image = ((System.Drawing.Image)(resources.GetObject("btnRenderOutput.Image")));
            this.btnRenderOutput.Location = new System.Drawing.Point(131, 48);
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
            this.btnGlitchHarvesterSettings.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnGlitchHarvesterSettings.FlatAppearance.BorderSize = 0;
            this.btnGlitchHarvesterSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGlitchHarvesterSettings.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnGlitchHarvesterSettings.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnGlitchHarvesterSettings.Image = ((System.Drawing.Image)(resources.GetObject("btnGlitchHarvesterSettings.Image")));
            this.btnGlitchHarvesterSettings.Location = new System.Drawing.Point(131, 13);
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
            this.btnRerollSelected.FlatAppearance.BorderSize = 0;
            this.btnRerollSelected.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRerollSelected.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnRerollSelected.ForeColor = System.Drawing.Color.White;
            this.btnRerollSelected.Location = new System.Drawing.Point(11, 83);
            this.btnRerollSelected.Name = "btnRerollSelected";
            this.btnRerollSelected.Size = new System.Drawing.Size(152, 26);
            this.btnRerollSelected.TabIndex = 133;
            this.btnRerollSelected.TabStop = false;
            this.btnRerollSelected.Tag = "color:light1";
            this.btnRerollSelected.Text = "Reroll Selected";
            this.btnRerollSelected.UseVisualStyleBackColor = false;
            this.btnRerollSelected.Click += new System.EventHandler(this.RerollSelected);
            this.btnRerollSelected.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnRerollButtonMouseDown);
            //
            // btnCorrupt
            //
            this.btnCorrupt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnCorrupt.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnCorrupt.FlatAppearance.BorderSize = 0;
            this.btnCorrupt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCorrupt.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnCorrupt.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnCorrupt.Image = ((System.Drawing.Image)(resources.GetObject("btnCorrupt.Image")));
            this.btnCorrupt.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCorrupt.Location = new System.Drawing.Point(11, 13);
            this.btnCorrupt.Name = "btnCorrupt";
            this.btnCorrupt.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.btnCorrupt.Size = new System.Drawing.Size(115, 32);
            this.btnCorrupt.TabIndex = 137;
            this.btnCorrupt.TabStop = false;
            this.btnCorrupt.Tag = "color:dark2";
            this.btnCorrupt.Text = "  Corrupt";
            this.btnCorrupt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCorrupt.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCorrupt.UseVisualStyleBackColor = false;
            this.btnCorrupt.Click += new System.EventHandler(this.Corrupt);
            this.btnCorrupt.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnCorrupt_MouseDown);
            //
            // btnBlastToggle
            //
            this.btnBlastToggle.BackColor = System.Drawing.Color.Gray;
            this.btnBlastToggle.FlatAppearance.BorderSize = 0;
            this.btnBlastToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBlastToggle.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnBlastToggle.ForeColor = System.Drawing.Color.White;
            this.btnBlastToggle.Location = new System.Drawing.Point(11, 112);
            this.btnBlastToggle.Name = "btnBlastToggle";
            this.btnBlastToggle.Size = new System.Drawing.Size(152, 26);
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
            this.btnSendRaw.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnSendRaw.FlatAppearance.BorderSize = 0;
            this.btnSendRaw.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSendRaw.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnSendRaw.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnSendRaw.Image = ((System.Drawing.Image)(resources.GetObject("btnSendRaw.Image")));
            this.btnSendRaw.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSendRaw.Location = new System.Drawing.Point(11, 48);
            this.btnSendRaw.Name = "btnSendRaw";
            this.btnSendRaw.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.btnSendRaw.Size = new System.Drawing.Size(115, 32);
            this.btnSendRaw.TabIndex = 139;
            this.btnSendRaw.TabStop = false;
            this.btnSendRaw.Tag = "color:dark2";
            this.btnSendRaw.Text = "  Raw to Stash";
            this.btnSendRaw.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSendRaw.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSendRaw.UseVisualStyleBackColor = false;
            this.btnSendRaw.Click += new System.EventHandler(this.SendRawToStash);
            //
            // label5
            //
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label5.Location = new System.Drawing.Point(37, 88);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 14);
            this.label5.TabIndex = 138;
            //
            // GlitchHarvesterBlastForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(270, 232);
            this.Controls.Add(this.btnRenderOutput);
            this.Controls.Add(this.btnGlitchHarvesterSettings);
            this.Controls.Add(this.btnRerollSelected);
            this.Controls.Add(this.btnCorrupt);
            this.Controls.Add(this.btnBlastToggle);
            this.Controls.Add(this.btnSendRaw);
            this.Controls.Add(this.label5);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GlitchHarvesterBlastForm";
            this.Tag = "color:dark1";
            this.Text = "Blast Tools";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HandleFormClosing);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Button btnCorrupt;
        public System.Windows.Forms.Button btnSendRaw;
        public System.Windows.Forms.Button btnRerollSelected;
        public System.Windows.Forms.Button btnBlastToggle;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.Button btnGlitchHarvesterSettings;
        public System.Windows.Forms.Button btnRenderOutput;
    }
}
