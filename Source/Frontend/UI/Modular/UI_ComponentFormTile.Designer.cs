namespace RTCV.UI
{
    partial class UI_ComponentFormTile
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UI_ComponentFormTile));
            this.lbComponentFormName = new System.Windows.Forms.Label();
            this.pnComponentFormHost = new RTCV.UI.ComponentPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.pnComponentFormHost.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbComponentFormName
            // 
            this.lbComponentFormName.AutoSize = true;
            this.lbComponentFormName.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.lbComponentFormName.ForeColor = System.Drawing.Color.White;
            this.lbComponentFormName.Location = new System.Drawing.Point(5, 4);
            this.lbComponentFormName.Name = "lbComponentFormName";
            this.lbComponentFormName.Size = new System.Drawing.Size(133, 15);
            this.lbComponentFormName.TabIndex = 121;
            this.lbComponentFormName.Text = "ComponentForm Name";
            this.lbComponentFormName.MouseDown += new System.Windows.Forms.MouseEventHandler(this.UI_ComponentFormTile_MouseDown);
            // 
            // pnComponentFormHost
            // 
            this.pnComponentFormHost.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnComponentFormHost.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pnComponentFormHost.Controls.Add(this.label4);
            this.pnComponentFormHost.Location = new System.Drawing.Point(0, 24);
            this.pnComponentFormHost.Margin = new System.Windows.Forms.Padding(0);
            this.pnComponentFormHost.Name = "pnComponentFormHost";
            this.pnComponentFormHost.Size = new System.Drawing.Size(280, 256);
            this.pnComponentFormHost.TabIndex = 122;
            this.pnComponentFormHost.Tag = "color:dark1";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label4.Location = new System.Drawing.Point(5, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(270, 238);
            this.label4.TabIndex = 121;
            this.label4.Tag = "color:light2";
            this.label4.Text = "Component is detached/unavailable";
            // 
            // UI_ComponentFormTile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(280, 280);
            this.Controls.Add(this.pnComponentFormHost);
            this.Controls.Add(this.lbComponentFormName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "UI_ComponentFormTile";
            this.Tag = "color:dark3";
            this.Text = "ComponentForm Tile";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.UI_ComponentFormTile_MouseDown);
            this.pnComponentFormHost.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ComponentPanel pnComponentFormHost;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.Label lbComponentFormName;
    }
}