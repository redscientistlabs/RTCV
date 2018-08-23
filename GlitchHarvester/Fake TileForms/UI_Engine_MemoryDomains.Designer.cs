namespace RTCV.UI.TileForms
{
    partial class UI_Engine_MemoryDomains
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
            this.lbMemoryDomains = new System.Windows.Forms.ListBox();
            this.btnAutoSelectDomains = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbComponentFormName = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbMemoryDomains
            // 
            this.lbMemoryDomains.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.lbMemoryDomains.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbMemoryDomains.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lbMemoryDomains.ForeColor = System.Drawing.Color.White;
            this.lbMemoryDomains.FormattingEnabled = true;
            this.lbMemoryDomains.IntegralHeight = false;
            this.lbMemoryDomains.Location = new System.Drawing.Point(7, 62);
            this.lbMemoryDomains.Margin = new System.Windows.Forms.Padding(5);
            this.lbMemoryDomains.Name = "lbMemoryDomains";
            this.lbMemoryDomains.ScrollAlwaysVisible = true;
            this.lbMemoryDomains.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lbMemoryDomains.Size = new System.Drawing.Size(166, 207);
            this.lbMemoryDomains.TabIndex = 15;
            this.lbMemoryDomains.Tag = "color:dark";
            // 
            // btnAutoSelectDomains
            // 
            this.btnAutoSelectDomains.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnAutoSelectDomains.FlatAppearance.BorderSize = 0;
            this.btnAutoSelectDomains.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAutoSelectDomains.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnAutoSelectDomains.ForeColor = System.Drawing.Color.Black;
            this.btnAutoSelectDomains.Location = new System.Drawing.Point(7, 30);
            this.btnAutoSelectDomains.Name = "btnAutoSelectDomains";
            this.btnAutoSelectDomains.Size = new System.Drawing.Size(166, 24);
            this.btnAutoSelectDomains.TabIndex = 18;
            this.btnAutoSelectDomains.TabStop = false;
            this.btnAutoSelectDomains.Tag = "color:light";
            this.btnAutoSelectDomains.Text = "Auto-select";
            this.btnAutoSelectDomains.UseVisualStyleBackColor = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel1.Controls.Add(this.lbComponentFormName);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(245, 24);
            this.panel1.TabIndex = 20;
            // 
            // lbComponentFormName
            // 
            this.lbComponentFormName.AutoSize = true;
            this.lbComponentFormName.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.lbComponentFormName.ForeColor = System.Drawing.Color.White;
            this.lbComponentFormName.Location = new System.Drawing.Point(12, 3);
            this.lbComponentFormName.Name = "lbComponentFormName";
            this.lbComponentFormName.Size = new System.Drawing.Size(103, 15);
            this.lbComponentFormName.TabIndex = 122;
            this.lbComponentFormName.Text = "Memory Domains";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Location = new System.Drawing.Point(7, 277);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(78, 24);
            this.button1.TabIndex = 21;
            this.button1.TabStop = false;
            this.button1.Tag = "color:light";
            this.button1.Text = "Select all";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.button2.ForeColor = System.Drawing.Color.Black;
            this.button2.Location = new System.Drawing.Point(95, 277);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(78, 24);
            this.button2.TabIndex = 22;
            this.button2.TabStop = false;
            this.button2.Tag = "color:light";
            this.button2.Text = "Unselect all";
            this.button2.UseVisualStyleBackColor = false;
            // 
            // UI_Engine_MemoryDomains
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(180, 310);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lbMemoryDomains);
            this.Controls.Add(this.btnAutoSelectDomains);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "UI_Engine_MemoryDomains";
            this.Text = "UI_DummyTileForm";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.ListBox lbMemoryDomains;
        private System.Windows.Forms.Button btnAutoSelectDomains;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbComponentFormName;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}