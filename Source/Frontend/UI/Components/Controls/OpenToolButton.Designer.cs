namespace RTCV.UI.Components.Controls
{
    partial class OpenToolButton
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnOpenTool = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnOpenTool);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(0, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(205, 49);
            this.groupBox1.TabIndex = 138;
            this.groupBox1.TabStop = false;
            this.groupBox1.Tag = "color:dark3";
            this.groupBox1.Text = "Tool";
            // 
            // btnOpenTool
            // 
            this.btnOpenTool.BackColor = System.Drawing.Color.Gray;
            this.btnOpenTool.FlatAppearance.BorderSize = 0;
            this.btnOpenTool.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenTool.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnOpenTool.ForeColor = System.Drawing.Color.White;
            this.btnOpenTool.Location = new System.Drawing.Point(7, 16);
            this.btnOpenTool.Name = "btnOpenTool";
            this.btnOpenTool.Size = new System.Drawing.Size(190, 23);
            this.btnOpenTool.TabIndex = 136;
            this.btnOpenTool.TabStop = false;
            this.btnOpenTool.Tag = "color:light1";
            this.btnOpenTool.Text = "Open Tool";
            this.btnOpenTool.UseVisualStyleBackColor = false;
            // 
            // OpenToolButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Controls.Add(this.groupBox1);
            this.Name = "OpenToolButton";
            this.Size = new System.Drawing.Size(211, 57);
            this.Tag = "color:dark3";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnOpenTool;
    }
}
