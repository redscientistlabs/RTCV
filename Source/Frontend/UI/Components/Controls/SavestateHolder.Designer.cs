namespace RTCV.UI.Components.Controls
{
    partial class SavestateHolder
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
            this.btnSavestate = new System.Windows.Forms.Button();
            this.tbSavestate = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnSavestate
            // 
            this.btnSavestate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnSavestate.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnSavestate.FlatAppearance.BorderSize = 0;
            this.btnSavestate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSavestate.Font = new System.Drawing.Font("Segoe UI", 7F);
            this.btnSavestate.ForeColor = System.Drawing.Color.PaleGreen;
            this.btnSavestate.Location = new System.Drawing.Point(0, 0);
            this.btnSavestate.Name = "btnSavestate";
            this.btnSavestate.Size = new System.Drawing.Size(30, 22);
            this.btnSavestate.TabIndex = 137;
            this.btnSavestate.TabStop = false;
            this.btnSavestate.Tag = "color:dark2";
            this.btnSavestate.Text = "01";
            this.btnSavestate.UseVisualStyleBackColor = false;
            // 
            // tbSavestate
            // 
            this.tbSavestate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.tbSavestate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbSavestate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbSavestate.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.tbSavestate.ForeColor = System.Drawing.Color.White;
            this.tbSavestate.Location = new System.Drawing.Point(30, 0);
            this.tbSavestate.MaxLength = 400;
            this.tbSavestate.Name = "tbSavestate";
            this.tbSavestate.Size = new System.Drawing.Size(120, 22);
            this.tbSavestate.TabIndex = 138;
            this.tbSavestate.TabStop = false;
            this.tbSavestate.Tag = "color:dark2";
            this.tbSavestate.WordWrap = false;
            // 
            // SavestateHolder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.tbSavestate);
            this.Controls.Add(this.btnSavestate);
            this.Margin = new System.Windows.Forms.Padding(0, 3, 4, 0);
            this.MinimumSize = new System.Drawing.Size(150, 22);
            this.Name = "SavestateHolder";
            this.Size = new System.Drawing.Size(150, 22);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Button btnSavestate;
        private System.Windows.Forms.TextBox tbSavestate;
    }
}
