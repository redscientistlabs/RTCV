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
            this.btnOpenTool = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnOpenTool
            // 
            this.btnOpenTool.BackColor = System.Drawing.Color.Gray;
            this.btnOpenTool.FlatAppearance.BorderSize = 0;
            this.btnOpenTool.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenTool.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnOpenTool.ForeColor = System.Drawing.Color.White;
            this.btnOpenTool.Location = new System.Drawing.Point(5, 5);
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
            this.Controls.Add(this.btnOpenTool);
            this.Name = "OpenToolButton";
            this.Size = new System.Drawing.Size(195, 28);
            this.Tag = "color:dark3";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnOpenTool;
    }
}
