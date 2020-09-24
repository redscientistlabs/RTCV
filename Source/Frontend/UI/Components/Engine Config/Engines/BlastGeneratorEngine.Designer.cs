namespace RTCV.UI.Components.EngineConfig.Engines
{
    partial class BlastGeneratorEngine
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BlastGeneratorEngine));
            this.label10 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.btnOpenBlastGenerator = new System.Windows.Forms.Button();
            this.engineGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // engineGroupBox
            // 
            this.engineGroupBox.Controls.Add(this.label10);
            this.engineGroupBox.Controls.Add(this.label21);
            this.engineGroupBox.Controls.Add(this.btnOpenBlastGenerator);
            this.engineGroupBox.Controls.SetChildIndex(this.placeholderComboBox, 0);
            this.engineGroupBox.Controls.SetChildIndex(this.btnOpenBlastGenerator, 0);
            this.engineGroupBox.Controls.SetChildIndex(this.label21, 0);
            this.engineGroupBox.Controls.SetChildIndex(this.label10, 0);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Italic);
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(172, 32);
            this.label10.MaximumSize = new System.Drawing.Size(205, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(197, 104);
            this.label10.TabIndex = 150;
            this.label10.Text = resources.GetString("label10.Text");
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic);
            this.label21.ForeColor = System.Drawing.Color.White;
            this.label21.Location = new System.Drawing.Point(168, 12);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(216, 13);
            this.label21.TabIndex = 149;
            this.label21.Text = "Imports corruption from the Blast Generator";
            // 
            // btnOpenBlastGenerator
            // 
            this.btnOpenBlastGenerator.BackColor = System.Drawing.Color.Gray;
            this.btnOpenBlastGenerator.FlatAppearance.BorderSize = 0;
            this.btnOpenBlastGenerator.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenBlastGenerator.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnOpenBlastGenerator.ForeColor = System.Drawing.Color.White;
            this.btnOpenBlastGenerator.Location = new System.Drawing.Point(5, 116);
            this.btnOpenBlastGenerator.Name = "btnOpenBlastGenerator";
            this.btnOpenBlastGenerator.Size = new System.Drawing.Size(159, 24);
            this.btnOpenBlastGenerator.TabIndex = 148;
            this.btnOpenBlastGenerator.TabStop = false;
            this.btnOpenBlastGenerator.Tag = "color:light1";
            this.btnOpenBlastGenerator.Text = "Open Blast Generator";
            this.btnOpenBlastGenerator.UseVisualStyleBackColor = false;
            this.btnOpenBlastGenerator.Click += new System.EventHandler(this.OpenBlastGenerator);
            // 
            // BlastGeneratorEngine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "BlastGeneratorEngine";
            this.engineGroupBox.ResumeLayout(false);
            this.engineGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Button btnOpenBlastGenerator;
    }
}
