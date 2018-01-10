namespace RTCV.GlitchHarvester.TileForms
{
    partial class GH_RenderOutput
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
            this.label4 = new System.Windows.Forms.Label();
            this.cbRenderType = new System.Windows.Forms.ComboBox();
            this.cbRenderAtLoad = new System.Windows.Forms.CheckBox();
            this.btnRender = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.labelIntensity = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(6, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 147;
            this.label4.Text = "Type:";
            // 
            // cbRenderType
            // 
            this.cbRenderType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cbRenderType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRenderType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbRenderType.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbRenderType.ForeColor = System.Drawing.Color.White;
            this.cbRenderType.FormattingEnabled = true;
            this.cbRenderType.Items.AddRange(new object[] {
            "NONE",
            "WAV",
            "AVI",
            "MPEG"});
            this.cbRenderType.Location = new System.Drawing.Point(41, 33);
            this.cbRenderType.Name = "cbRenderType";
            this.cbRenderType.Size = new System.Drawing.Size(83, 21);
            this.cbRenderType.TabIndex = 146;
            this.cbRenderType.TabStop = false;
            this.cbRenderType.Tag = "color:dark";
            // 
            // cbRenderAtLoad
            // 
            this.cbRenderAtLoad.AutoSize = true;
            this.cbRenderAtLoad.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbRenderAtLoad.ForeColor = System.Drawing.Color.White;
            this.cbRenderAtLoad.Location = new System.Drawing.Point(8, 58);
            this.cbRenderAtLoad.Name = "cbRenderAtLoad";
            this.cbRenderAtLoad.Size = new System.Drawing.Size(121, 17);
            this.cbRenderAtLoad.TabIndex = 144;
            this.cbRenderAtLoad.TabStop = false;
            this.cbRenderAtLoad.Text = "Render file at load";
            this.cbRenderAtLoad.UseVisualStyleBackColor = true;
            // 
            // btnRender
            // 
            this.btnRender.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnRender.FlatAppearance.BorderSize = 0;
            this.btnRender.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRender.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnRender.ForeColor = System.Drawing.Color.White;
            this.btnRender.Location = new System.Drawing.Point(8, 79);
            this.btnRender.Name = "btnRender";
            this.btnRender.Size = new System.Drawing.Size(115, 22);
            this.btnRender.TabIndex = 145;
            this.btnRender.TabStop = false;
            this.btnRender.Tag = "color:darker";
            this.btnRender.Text = "Start Render";
            this.btnRender.UseVisualStyleBackColor = false;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(8, 103);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(115, 22);
            this.button1.TabIndex = 148;
            this.button1.TabStop = false;
            this.button1.Tag = "color:darker";
            this.button1.Text = "Open Folder";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // labelIntensity
            // 
            this.labelIntensity.AutoSize = true;
            this.labelIntensity.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.labelIntensity.ForeColor = System.Drawing.Color.White;
            this.labelIntensity.Location = new System.Drawing.Point(5, 6);
            this.labelIntensity.Name = "labelIntensity";
            this.labelIntensity.Size = new System.Drawing.Size(101, 19);
            this.labelIntensity.TabIndex = 177;
            this.labelIntensity.Text = "Render Output";
            // 
            // GH_RenderOutput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(135, 135);
            this.Controls.Add(this.labelIntensity);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbRenderType);
            this.Controls.Add(this.cbRenderAtLoad);
            this.Controls.Add(this.btnRender);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "GH_RenderOutput";
            this.Text = "GH_DummyTileForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.ComboBox cbRenderType;
        public System.Windows.Forms.CheckBox cbRenderAtLoad;
        public System.Windows.Forms.Button btnRender;
        public System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label labelIntensity;
    }
}