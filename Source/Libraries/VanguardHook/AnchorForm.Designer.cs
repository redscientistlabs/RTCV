
namespace VanguardHook
{
    partial class AnchorForm
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
            this.btnSaveLoad = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSaveLoad
            // 
            this.btnSaveLoad.Location = new System.Drawing.Point(3, 12);
            this.btnSaveLoad.Name = "btnSaveLoad";
            this.btnSaveLoad.Size = new System.Drawing.Size(75, 23);
            this.btnSaveLoad.TabIndex = 0;
            this.btnSaveLoad.Text = "Reload";
            this.btnSaveLoad.UseVisualStyleBackColor = true;
            this.btnSaveLoad.Click += new System.EventHandler(this.btnSaveLoad_Click);
            // 
            // AnchorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(266, 139);
            this.Controls.Add(this.btnSaveLoad);
            this.Name = "AnchorForm";
            this.Text = "Extra Controls";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSaveLoad;
    }
}