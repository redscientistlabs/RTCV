namespace RTCV.UI.Components.EngineConfig.Engines
{
    partial class CustomEngine
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnOpenCustomEngine = new System.Windows.Forms.Button();
            this.engineGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // engineGroupBox
            // 
            this.engineGroupBox.Controls.Add(this.label2);
            this.engineGroupBox.Controls.Add(this.label3);
            this.engineGroupBox.Controls.Add(this.btnOpenCustomEngine);
            this.engineGroupBox.Controls.SetChildIndex(this.placeholderComboBox, 0);
            this.engineGroupBox.Controls.SetChildIndex(this.btnOpenCustomEngine, 0);
            this.engineGroupBox.Controls.SetChildIndex(this.label3, 0);
            this.engineGroupBox.Controls.SetChildIndex(this.label2, 0);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Italic);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(181, 30);
            this.label2.MaximumSize = new System.Drawing.Size(205, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(204, 26);
            this.label2.TabIndex = 150;
            this.label2.Text = "It has so many options, there\'s no way we could fit them all here!";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(169, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(123, 13);
            this.label3.TabIndex = 149;
            this.label3.Text = "Create your own engine!";
            // 
            // btnOpenCustomEngine
            // 
            this.btnOpenCustomEngine.BackColor = System.Drawing.Color.Gray;
            this.btnOpenCustomEngine.FlatAppearance.BorderSize = 0;
            this.btnOpenCustomEngine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenCustomEngine.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnOpenCustomEngine.ForeColor = System.Drawing.Color.White;
            this.btnOpenCustomEngine.Location = new System.Drawing.Point(6, 117);
            this.btnOpenCustomEngine.Name = "btnOpenCustomEngine";
            this.btnOpenCustomEngine.Size = new System.Drawing.Size(159, 24);
            this.btnOpenCustomEngine.TabIndex = 148;
            this.btnOpenCustomEngine.TabStop = false;
            this.btnOpenCustomEngine.Tag = "color:light1";
            this.btnOpenCustomEngine.Text = "Open Custom Engine";
            this.btnOpenCustomEngine.UseVisualStyleBackColor = false;
            // 
            // CustomEngine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "CustomEngine";
            this.engineGroupBox.ResumeLayout(false);
            this.engineGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnOpenCustomEngine;
    }
}
