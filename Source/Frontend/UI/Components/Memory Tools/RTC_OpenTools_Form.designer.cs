namespace RTCV.UI
{
    partial class RTC_OpenTools_Form
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
            this.btnOpenHexEditor = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOpenHexEditor
            // 
            this.btnOpenHexEditor.BackColor = System.Drawing.Color.Gray;
            this.btnOpenHexEditor.FlatAppearance.BorderSize = 0;
            this.btnOpenHexEditor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenHexEditor.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnOpenHexEditor.ForeColor = System.Drawing.Color.White;
            this.btnOpenHexEditor.Location = new System.Drawing.Point(6, 26);
            this.btnOpenHexEditor.Name = "btnOpenHexEditor";
            this.btnOpenHexEditor.Size = new System.Drawing.Size(354, 30);
            this.btnOpenHexEditor.TabIndex = 136;
            this.btnOpenHexEditor.TabStop = false;
            this.btnOpenHexEditor.Tag = "color:light1";
            this.btnOpenHexEditor.Text = "Open Hex Editor";
            this.btnOpenHexEditor.UseVisualStyleBackColor = false;
            this.btnOpenHexEditor.Click += new System.EventHandler(this.btnOpenHexEditor_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnOpenHexEditor);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(12, 14);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(366, 74);
            this.groupBox1.TabIndex = 137;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Hex Editor";
            // 
            // RTC_OpenTools_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(390, 250);
            this.Controls.Add(this.groupBox1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "RTC_OpenTools_Form";
            this.Tag = "color:dark3";
            this.Text = "Extra Tools Form";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HandleFormClosing);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
		private System.Windows.Forms.Button btnOpenHexEditor;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}