namespace RTCV.UI.TileForms
{
    partial class UI_StashHistory
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
            this.btnAddStashToStockpile = new System.Windows.Forms.Button();
            this.btnClearStashHistory = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lbStashHistory = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // btnAddStashToStockpile
            // 
            this.btnAddStashToStockpile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnAddStashToStockpile.FlatAppearance.BorderSize = 0;
            this.btnAddStashToStockpile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddStashToStockpile.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnAddStashToStockpile.ForeColor = System.Drawing.Color.Black;
            this.btnAddStashToStockpile.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnAddStashToStockpile.Location = new System.Drawing.Point(10, 200);
            this.btnAddStashToStockpile.Name = "btnAddStashToStockpile";
            this.btnAddStashToStockpile.Size = new System.Drawing.Size(113, 38);
            this.btnAddStashToStockpile.TabIndex = 113;
            this.btnAddStashToStockpile.TabStop = false;
            this.btnAddStashToStockpile.Tag = "color:light";
            this.btnAddStashToStockpile.Text = "Send Item To Stockpile";
            this.btnAddStashToStockpile.UseVisualStyleBackColor = false;
            // 
            // btnClearStashHistory
            // 
            this.btnClearStashHistory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnClearStashHistory.FlatAppearance.BorderSize = 0;
            this.btnClearStashHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearStashHistory.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnClearStashHistory.ForeColor = System.Drawing.Color.Black;
            this.btnClearStashHistory.Location = new System.Drawing.Point(10, 244);
            this.btnClearStashHistory.Name = "btnClearStashHistory";
            this.btnClearStashHistory.Size = new System.Drawing.Size(113, 24);
            this.btnClearStashHistory.TabIndex = 112;
            this.btnClearStashHistory.TabStop = false;
            this.btnClearStashHistory.Tag = "color:light";
            this.btnClearStashHistory.Text = "Clear History";
            this.btnClearStashHistory.UseVisualStyleBackColor = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(8, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 19);
            this.label2.TabIndex = 109;
            this.label2.Text = "Stash History";
            // 
            // lbStashHistory
            // 
            this.lbStashHistory.BackColor = System.Drawing.Color.Gray;
            this.lbStashHistory.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbStashHistory.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lbStashHistory.ForeColor = System.Drawing.Color.White;
            this.lbStashHistory.FormattingEnabled = true;
            this.lbStashHistory.IntegralHeight = false;
            this.lbStashHistory.Location = new System.Drawing.Point(12, 32);
            this.lbStashHistory.Name = "lbStashHistory";
            this.lbStashHistory.ScrollAlwaysVisible = true;
            this.lbStashHistory.Size = new System.Drawing.Size(113, 162);
            this.lbStashHistory.TabIndex = 108;
            this.lbStashHistory.Tag = "color:normal";
            // 
            // UI_StashHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(135, 280);
            this.Controls.Add(this.btnAddStashToStockpile);
            this.Controls.Add(this.btnClearStashHistory);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbStashHistory);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "UI_StashHistory";
            this.Text = "UI_DummyTileForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAddStashToStockpile;
        private System.Windows.Forms.Button btnClearStashHistory;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.ListBox lbStashHistory;
    }
}