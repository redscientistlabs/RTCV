namespace RTCV.UI
{
    partial class RTC_GlitchHarvesterBlast_Form
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
            this.btnCorrupt = new System.Windows.Forms.Button();
            this.btnSendRaw = new System.Windows.Forms.Button();
            this.btnRerollSelected = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rbOriginal = new System.Windows.Forms.RadioButton();
            this.rbInject = new System.Windows.Forms.RadioButton();
            this.rbCorrupt = new System.Windows.Forms.RadioButton();
            this.btnBlastToggle = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCorrupt
            // 
            this.btnCorrupt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnCorrupt.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnCorrupt.FlatAppearance.BorderSize = 0;
            this.btnCorrupt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCorrupt.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.btnCorrupt.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnCorrupt.Location = new System.Drawing.Point(14, 14);
            this.btnCorrupt.Name = "btnCorrupt";
            this.btnCorrupt.Size = new System.Drawing.Size(133, 28);
            this.btnCorrupt.TabIndex = 137;
            this.btnCorrupt.TabStop = false;
            this.btnCorrupt.Tag = "color:dark2";
            this.btnCorrupt.Text = "Blast/Send";
            this.btnCorrupt.UseVisualStyleBackColor = false;
            // 
            // btnSendRaw
            // 
            this.btnSendRaw.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnSendRaw.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnSendRaw.FlatAppearance.BorderSize = 0;
            this.btnSendRaw.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSendRaw.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnSendRaw.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnSendRaw.Location = new System.Drawing.Point(14, 45);
            this.btnSendRaw.Name = "btnSendRaw";
            this.btnSendRaw.Size = new System.Drawing.Size(133, 24);
            this.btnSendRaw.TabIndex = 139;
            this.btnSendRaw.TabStop = false;
            this.btnSendRaw.Tag = "color:dark2";
            this.btnSendRaw.Text = "Send Raw to Stash";
            this.btnSendRaw.UseVisualStyleBackColor = false;
            // 
            // btnRerollSelected
            // 
            this.btnRerollSelected.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnRerollSelected.FlatAppearance.BorderSize = 0;
            this.btnRerollSelected.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRerollSelected.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnRerollSelected.ForeColor = System.Drawing.Color.White;
            this.btnRerollSelected.Location = new System.Drawing.Point(87, 75);
            this.btnRerollSelected.Name = "btnRerollSelected";
            this.btnRerollSelected.Size = new System.Drawing.Size(60, 48);
            this.btnRerollSelected.TabIndex = 133;
            this.btnRerollSelected.TabStop = false;
            this.btnRerollSelected.Tag = "color:dark2";
            this.btnRerollSelected.Text = "Reroll Selected";
            this.btnRerollSelected.UseVisualStyleBackColor = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel1.Controls.Add(this.rbOriginal);
            this.panel1.Controls.Add(this.rbInject);
            this.panel1.Controls.Add(this.rbCorrupt);
            this.panel1.Location = new System.Drawing.Point(14, 76);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(71, 48);
            this.panel1.TabIndex = 76;
            this.panel1.Tag = "color:dark1";
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
            // btnBlastToggle
            // 
            this.btnBlastToggle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnBlastToggle.FlatAppearance.BorderSize = 0;
            this.btnBlastToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBlastToggle.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnBlastToggle.ForeColor = System.Drawing.Color.White;
            this.btnBlastToggle.Location = new System.Drawing.Point(14, 128);
            this.btnBlastToggle.Name = "btnBlastToggle";
            this.btnBlastToggle.Size = new System.Drawing.Size(133, 22);
            this.btnBlastToggle.TabIndex = 131;
            this.btnBlastToggle.TabStop = false;
            this.btnBlastToggle.Tag = "color:dark2";
            this.btnBlastToggle.Text = "BlastLayer : OFF";
            this.btnBlastToggle.UseVisualStyleBackColor = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label5.Location = new System.Drawing.Point(40, 81);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 14);
            this.label5.TabIndex = 138;
            // 
            // RTC_VmdNoTool_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(161, 164);
            this.Controls.Add(this.btnRerollSelected);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnCorrupt);
            this.Controls.Add(this.btnBlastToggle);
            this.Controls.Add(this.btnSendRaw);
            this.Controls.Add(this.label5);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "RTC_VmdNoTool_Form";
            this.Tag = "color:dark1";
            this.Text = "No Tool Selected";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Button btnCorrupt;
        public System.Windows.Forms.Button btnSendRaw;
        public System.Windows.Forms.Button btnRerollSelected;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.RadioButton rbOriginal;
        public System.Windows.Forms.RadioButton rbInject;
        public System.Windows.Forms.RadioButton rbCorrupt;
        public System.Windows.Forms.Button btnBlastToggle;
        private System.Windows.Forms.Label label5;
    }
}