namespace RTCV.UI.Components.EngineConfig
{
    partial class EngineConfigControl
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
            this.engineGroupBox = new System.Windows.Forms.GroupBox();
            this.placeholderComboBox = new System.Windows.Forms.ComboBox();
            this.engineGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // engineGroupBox
            // 
            this.engineGroupBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.engineGroupBox.Controls.Add(this.placeholderComboBox);
            this.engineGroupBox.Location = new System.Drawing.Point(0, 3);
            this.engineGroupBox.Name = "engineGroupBox";
            this.engineGroupBox.Size = new System.Drawing.Size(420, 148);
            this.engineGroupBox.TabIndex = 0;
            this.engineGroupBox.TabStop = false;
            this.engineGroupBox.Tag = "color:dark1";
            // 
            // placeholderComboBox
            // 
            this.placeholderComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.placeholderComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.placeholderComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.placeholderComboBox.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.placeholderComboBox.ForeColor = System.Drawing.Color.White;
            this.placeholderComboBox.FormattingEnabled = true;
            this.placeholderComboBox.Location = new System.Drawing.Point(0, 6);
            this.placeholderComboBox.Name = "placeholderComboBox";
            this.placeholderComboBox.Size = new System.Drawing.Size(165, 21);
            this.placeholderComboBox.TabIndex = 147;
            this.placeholderComboBox.Tag = "color:normal";
            this.placeholderComboBox.Visible = false;
            // 
            // EngineConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Controls.Add(this.engineGroupBox);
            this.Name = "EngineConfigControl";
            this.Size = new System.Drawing.Size(420, 151);
            this.Tag = "color:dark1";
            this.engineGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.GroupBox engineGroupBox;
        public System.Windows.Forms.ComboBox placeholderComboBox;
    }
}
