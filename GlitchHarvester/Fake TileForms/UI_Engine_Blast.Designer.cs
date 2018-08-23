namespace RTCV.UI.TileForms
{
    partial class UI_Engine_Blast
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
            this.btnManualBlast = new System.Windows.Forms.Button();
            this.btnAutoCorrupt = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // btnCorrupt
            // 
            this.btnCorrupt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnCorrupt.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnCorrupt.FlatAppearance.BorderSize = 0;
            this.btnCorrupt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCorrupt.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnCorrupt.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnCorrupt.Location = new System.Drawing.Point(8, 77);
            this.btnCorrupt.Name = "btnCorrupt";
            this.btnCorrupt.Size = new System.Drawing.Size(115, 22);
            this.btnCorrupt.TabIndex = 135;
            this.btnCorrupt.TabStop = false;
            this.btnCorrupt.Tag = "color:darker";
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
            this.btnSendRaw.Location = new System.Drawing.Point(8, 101);
            this.btnSendRaw.Name = "btnSendRaw";
            this.btnSendRaw.Size = new System.Drawing.Size(115, 22);
            this.btnSendRaw.TabIndex = 136;
            this.btnSendRaw.TabStop = false;
            this.btnSendRaw.Tag = "color:darker";
            this.btnSendRaw.Text = "Send Raw to Stash";
            this.btnSendRaw.UseVisualStyleBackColor = false;
            // 
            // btnManualBlast
            // 
            this.btnManualBlast.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnManualBlast.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnManualBlast.FlatAppearance.BorderSize = 0;
            this.btnManualBlast.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnManualBlast.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnManualBlast.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnManualBlast.Location = new System.Drawing.Point(7, 10);
            this.btnManualBlast.Name = "btnManualBlast";
            this.btnManualBlast.Size = new System.Drawing.Size(115, 22);
            this.btnManualBlast.TabIndex = 137;
            this.btnManualBlast.TabStop = false;
            this.btnManualBlast.Tag = "color:darker";
            this.btnManualBlast.Text = "Manual Blast";
            this.btnManualBlast.UseVisualStyleBackColor = false;
            // 
            // btnAutoCorrupt
            // 
            this.btnAutoCorrupt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnAutoCorrupt.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnAutoCorrupt.FlatAppearance.BorderSize = 0;
            this.btnAutoCorrupt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAutoCorrupt.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnAutoCorrupt.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnAutoCorrupt.Location = new System.Drawing.Point(7, 34);
            this.btnAutoCorrupt.Name = "btnAutoCorrupt";
            this.btnAutoCorrupt.Size = new System.Drawing.Size(115, 22);
            this.btnAutoCorrupt.TabIndex = 138;
            this.btnAutoCorrupt.TabStop = false;
            this.btnAutoCorrupt.Tag = "color:darker";
            this.btnAutoCorrupt.Text = "Start Auto-Corrupt";
            this.btnAutoCorrupt.UseVisualStyleBackColor = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.panel1.Location = new System.Drawing.Point(0, 62);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(135, 10);
            this.panel1.TabIndex = 139;
            // 
            // UI_Engine_Blast
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(130, 130);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnManualBlast);
            this.Controls.Add(this.btnAutoCorrupt);
            this.Controls.Add(this.btnCorrupt);
            this.Controls.Add(this.btnSendRaw);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "UI_Engine_Blast";
            this.Text = "UI_DummyTileForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCorrupt;
        public System.Windows.Forms.Button btnSendRaw;
        private System.Windows.Forms.Button btnManualBlast;
        public System.Windows.Forms.Button btnAutoCorrupt;
        private System.Windows.Forms.Panel panel1;
    }
}