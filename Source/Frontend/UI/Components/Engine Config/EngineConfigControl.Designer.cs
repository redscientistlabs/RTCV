namespace RTCV.UI.Components.Engine_Config
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
            this.SuspendLayout();
            // 
            // engineGroupBox
            // 
            this.engineGroupBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.engineGroupBox.Location = new System.Drawing.Point(0, 3);
            this.engineGroupBox.Name = "engineGroupBox";
            this.engineGroupBox.Size = new System.Drawing.Size(420, 148);
            this.engineGroupBox.TabIndex = 0;
            this.engineGroupBox.TabStop = false;
            this.engineGroupBox.Tag = "color:dark1";
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
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.GroupBox engineGroupBox;
    }
}
