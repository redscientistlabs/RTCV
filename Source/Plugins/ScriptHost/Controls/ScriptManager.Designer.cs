namespace RTCV.Plugins.ScriptHost.Controls
{
    partial class ScriptManager
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
            this.tbLog = new System.Windows.Forms.RichTextBox();
            this.btnRunSynchronously = new System.Windows.Forms.Button();
            this.btnRunAsync = new System.Windows.Forms.Button();
            this.scintilla = new ScintillaNET.Scintilla();
            this.SuspendLayout();
            // 
            // tbLog
            // 
            this.tbLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbLog.BackColor = System.Drawing.Color.Black;
            this.tbLog.ForeColor = System.Drawing.Color.White;
            this.tbLog.Location = new System.Drawing.Point(126, 254);
            this.tbLog.Name = "tbLog";
            this.tbLog.Size = new System.Drawing.Size(528, 68);
            this.tbLog.TabIndex = 1;
            this.tbLog.Text = "";
            // 
            // btnRunSynchronously
            // 
            this.btnRunSynchronously.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRunSynchronously.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnRunSynchronously.ForeColor = System.Drawing.Color.White;
            this.btnRunSynchronously.Location = new System.Drawing.Point(3, 261);
            this.btnRunSynchronously.Name = "btnRunSynchronously";
            this.btnRunSynchronously.Size = new System.Drawing.Size(117, 23);
            this.btnRunSynchronously.TabIndex = 2;
            this.btnRunSynchronously.Text = "Run";
            this.btnRunSynchronously.UseVisualStyleBackColor = false;
            this.btnRunSynchronously.Click += new System.EventHandler(this.btnRunSynchronously_Click);
            // 
            // btnRunAsync
            // 
            this.btnRunAsync.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRunAsync.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnRunAsync.ForeColor = System.Drawing.Color.White;
            this.btnRunAsync.Location = new System.Drawing.Point(3, 290);
            this.btnRunAsync.Name = "btnRunAsync";
            this.btnRunAsync.Size = new System.Drawing.Size(117, 23);
            this.btnRunAsync.TabIndex = 3;
            this.btnRunAsync.Text = "Run Async";
            this.btnRunAsync.UseVisualStyleBackColor = false;
            this.btnRunAsync.Click += new System.EventHandler(this.btnRunAsync_Click);
            // 
            // scintilla
            // 
            this.scintilla.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scintilla.AnnotationVisible = ScintillaNET.Annotation.Standard;
            this.scintilla.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.scintilla.Location = new System.Drawing.Point(0, 3);
            this.scintilla.Name = "scintilla";
            this.scintilla.Size = new System.Drawing.Size(654, 245);
            this.scintilla.TabIndex = 4;
            // 
            // ScriptManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Controls.Add(this.scintilla);
            this.Controls.Add(this.btnRunAsync);
            this.Controls.Add(this.btnRunSynchronously);
            this.Controls.Add(this.tbLog);
            this.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "ScriptManager";
            this.Size = new System.Drawing.Size(657, 329);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.RichTextBox tbLog;
        private System.Windows.Forms.Button btnRunSynchronously;
        private System.Windows.Forms.Button btnRunAsync;
        private ScintillaNET.Scintilla scintilla;
    }
}
