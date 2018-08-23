namespace RTCV.UI.TileForms
{
    partial class UI_BlastManipulator
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.rbOriginal = new System.Windows.Forms.RadioButton();
            this.rbInject = new System.Windows.Forms.RadioButton();
            this.rbCorrupt = new System.Windows.Forms.RadioButton();
            this.labelErrorDelay = new System.Windows.Forms.Label();
            this.btnRerollSelected = new System.Windows.Forms.Button();
            this.btnBlastToggle = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel1.Controls.Add(this.rbOriginal);
            this.panel1.Controls.Add(this.rbInject);
            this.panel1.Controls.Add(this.rbCorrupt);
            this.panel1.Location = new System.Drawing.Point(5, 31);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(102, 48);
            this.panel1.TabIndex = 136;
            this.panel1.Tag = "color:dark";
            // 
            // rbOriginal
            // 
            this.rbOriginal.AutoSize = true;
            this.rbOriginal.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.rbOriginal.ForeColor = System.Drawing.Color.White;
            this.rbOriginal.Location = new System.Drawing.Point(6, 29);
            this.rbOriginal.Name = "rbOriginal";
            this.rbOriginal.Size = new System.Drawing.Size(67, 17);
            this.rbOriginal.TabIndex = 85;
            this.rbOriginal.Text = "Original";
            this.rbOriginal.UseVisualStyleBackColor = true;
            // 
            // rbInject
            // 
            this.rbInject.AutoSize = true;
            this.rbInject.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.rbInject.ForeColor = System.Drawing.Color.White;
            this.rbInject.Location = new System.Drawing.Point(6, 15);
            this.rbInject.Name = "rbInject";
            this.rbInject.Size = new System.Drawing.Size(53, 17);
            this.rbInject.TabIndex = 84;
            this.rbInject.Text = "Inject";
            this.rbInject.UseVisualStyleBackColor = true;
            // 
            // rbCorrupt
            // 
            this.rbCorrupt.AutoSize = true;
            this.rbCorrupt.Checked = true;
            this.rbCorrupt.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.rbCorrupt.ForeColor = System.Drawing.Color.White;
            this.rbCorrupt.Location = new System.Drawing.Point(6, 1);
            this.rbCorrupt.Name = "rbCorrupt";
            this.rbCorrupt.Size = new System.Drawing.Size(65, 17);
            this.rbCorrupt.TabIndex = 83;
            this.rbCorrupt.TabStop = true;
            this.rbCorrupt.Text = "Corrupt";
            this.rbCorrupt.UseVisualStyleBackColor = true;
            // 
            // labelErrorDelay
            // 
            this.labelErrorDelay.AutoSize = true;
            this.labelErrorDelay.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.labelErrorDelay.ForeColor = System.Drawing.Color.White;
            this.labelErrorDelay.Location = new System.Drawing.Point(8, 7);
            this.labelErrorDelay.Name = "labelErrorDelay";
            this.labelErrorDelay.Size = new System.Drawing.Size(117, 19);
            this.labelErrorDelay.TabIndex = 140;
            this.labelErrorDelay.Text = "Blast Manipulator";
            // 
            // btnRerollSelected
            // 
            this.btnRerollSelected.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnRerollSelected.FlatAppearance.BorderSize = 0;
            this.btnRerollSelected.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRerollSelected.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnRerollSelected.ForeColor = System.Drawing.Color.White;
            this.btnRerollSelected.Location = new System.Drawing.Point(8, 81);
            this.btnRerollSelected.Name = "btnRerollSelected";
            this.btnRerollSelected.Size = new System.Drawing.Size(119, 22);
            this.btnRerollSelected.TabIndex = 149;
            this.btnRerollSelected.TabStop = false;
            this.btnRerollSelected.Tag = "color:darker";
            this.btnRerollSelected.Text = "Reroll Selected";
            this.btnRerollSelected.UseVisualStyleBackColor = false;
            // 
            // btnBlastToggle
            // 
            this.btnBlastToggle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnBlastToggle.FlatAppearance.BorderSize = 0;
            this.btnBlastToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBlastToggle.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnBlastToggle.ForeColor = System.Drawing.Color.White;
            this.btnBlastToggle.Location = new System.Drawing.Point(8, 106);
            this.btnBlastToggle.Name = "btnBlastToggle";
            this.btnBlastToggle.Size = new System.Drawing.Size(119, 22);
            this.btnBlastToggle.TabIndex = 148;
            this.btnBlastToggle.TabStop = false;
            this.btnBlastToggle.Tag = "color:darker";
            this.btnBlastToggle.Text = "BlastLayer : OFF";
            this.btnBlastToggle.UseVisualStyleBackColor = false;
            // 
            // UI_BlastParameters
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(135, 135);
            this.Controls.Add(this.btnRerollSelected);
            this.Controls.Add(this.btnBlastToggle);
            this.Controls.Add(this.labelErrorDelay);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "UI_BlastParameters";
            this.Text = "UI_DummyTileForm";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.RadioButton rbOriginal;
        public System.Windows.Forms.RadioButton rbInject;
        public System.Windows.Forms.RadioButton rbCorrupt;
        private System.Windows.Forms.Label labelErrorDelay;
        public System.Windows.Forms.Button btnRerollSelected;
        public System.Windows.Forms.Button btnBlastToggle;
    }
}