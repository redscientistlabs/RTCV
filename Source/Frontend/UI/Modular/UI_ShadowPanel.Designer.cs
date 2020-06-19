namespace RTCV.UI
{
    partial class UI_ShadowPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UI_ShadowPanel));
            this.pnFloater = new System.Windows.Forms.Panel();
            this.pnContainer = new System.Windows.Forms.Panel();
            this.btnLeft = new System.Windows.Forms.Button();
            this.btnRight = new System.Windows.Forms.Button();
            this.pnFloater.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnFloater
            // 
            this.pnFloater.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pnFloater.Controls.Add(this.pnContainer);
            this.pnFloater.Controls.Add(this.btnLeft);
            this.pnFloater.Controls.Add(this.btnRight);
            this.pnFloater.Location = new System.Drawing.Point(50, 50);
            this.pnFloater.Name = "pnFloater";
            this.pnFloater.Size = new System.Drawing.Size(500, 400);
            this.pnFloater.TabIndex = 0;
            this.pnFloater.Tag = "color:dark2";
            // 
            // pnContainer
            // 
            this.pnContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.pnContainer.Location = new System.Drawing.Point(11, 15);
            this.pnContainer.Name = "pnContainer";
            this.pnContainer.Size = new System.Drawing.Size(475, 320);
            this.pnContainer.TabIndex = 118;
            this.pnContainer.Tag = "color:dark1";
            // 
            // btnCancel
            // 
            this.btnLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLeft.BackColor = System.Drawing.Color.Gray;
            this.btnLeft.FlatAppearance.BorderSize = 0;
            this.btnLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLeft.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnLeft.ForeColor = System.Drawing.Color.White;
            this.btnLeft.Location = new System.Drawing.Point(11, 352);
            this.btnLeft.Name = "btnCancel";
            this.btnLeft.Size = new System.Drawing.Size(230, 33);
            this.btnLeft.TabIndex = 117;
            this.btnLeft.TabStop = false;
            this.btnLeft.Tag = "color:light1";
            this.btnLeft.Text = "Cancel";
            this.btnLeft.UseVisualStyleBackColor = false;
            this.btnLeft.Visible = false;
            this.btnLeft.Click += new System.EventHandler(this.btnLeft_Click);
            // 
            // btnOk
            // 
            this.btnRight.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRight.BackColor = System.Drawing.Color.Gray;
            this.btnRight.FlatAppearance.BorderSize = 0;
            this.btnRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRight.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnRight.ForeColor = System.Drawing.Color.White;
            this.btnRight.Location = new System.Drawing.Point(256, 352);
            this.btnRight.Name = "btnOk";
            this.btnRight.Size = new System.Drawing.Size(230, 33);
            this.btnRight.TabIndex = 116;
            this.btnRight.TabStop = false;
            this.btnRight.Tag = "color:light1";
            this.btnRight.Text = "OK";
            this.btnRight.UseVisualStyleBackColor = false;
            this.btnRight.Click += new System.EventHandler(this.btnRight_Click);
            // 
            // UI_ShadowPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(600, 500);
            this.Controls.Add(this.pnFloater);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "UI_ShadowPanel";
            this.Text = "ShadowPanel";
            this.Load += new System.EventHandler(this.UI_ShadowPanel_Load);
            this.pnFloater.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnFloater;
        private System.Windows.Forms.Button btnRight;
        private System.Windows.Forms.Panel pnContainer;
        private System.Windows.Forms.Button btnLeft;
    }
}