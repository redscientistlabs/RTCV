namespace RTCV.UI.Components.Controls
{
    partial class SavestateList
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
            this.flowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.pnBackForwardPage = new System.Windows.Forms.Panel();
            this.btnForward = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.pnChangeLoadSave = new System.Windows.Forms.Panel();
            this.btnToggleSaveLoad = new System.Windows.Forms.Button();
            this.btnSaveLoad = new System.Windows.Forms.Button();
            this.pnBackForwardPage.SuspendLayout();
            this.pnChangeLoadSave.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowPanel
            // 
            this.flowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowPanel.Location = new System.Drawing.Point(0, 35);
            this.flowPanel.Margin = new System.Windows.Forms.Padding(0);
            this.flowPanel.Name = "flowPanel";
            this.flowPanel.Size = new System.Drawing.Size(150, 245);
            this.flowPanel.TabIndex = 0;
            // 
            // pnBackForwardPage
            // 
            this.pnBackForwardPage.Controls.Add(this.btnForward);
            this.pnBackForwardPage.Controls.Add(this.btnBack);
            this.pnBackForwardPage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnBackForwardPage.Location = new System.Drawing.Point(0, 250);
            this.pnBackForwardPage.Name = "pnBackForwardPage";
            this.pnBackForwardPage.Size = new System.Drawing.Size(150, 30);
            this.pnBackForwardPage.TabIndex = 0;
            // 
            // btnForward
            // 
            this.btnForward.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnForward.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnForward.FlatAppearance.BorderSize = 0;
            this.btnForward.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnForward.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnForward.ForeColor = System.Drawing.Color.White;
            this.btnForward.Location = new System.Drawing.Point(78, 4);
            this.btnForward.Name = "btnForward";
            this.btnForward.Size = new System.Drawing.Size(71, 22);
            this.btnForward.TabIndex = 160;
            this.btnForward.TabStop = false;
            this.btnForward.Tag = "color:dark2";
            this.btnForward.Text = "▶";
            this.btnForward.UseVisualStyleBackColor = false;
            this.btnForward.Click += new System.EventHandler(this.BtnForward_Click);
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnBack.FlatAppearance.BorderSize = 0;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnBack.ForeColor = System.Drawing.Color.White;
            this.btnBack.Location = new System.Drawing.Point(0, 4);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(71, 22);
            this.btnBack.TabIndex = 159;
            this.btnBack.TabStop = false;
            this.btnBack.Tag = "color:dark2";
            this.btnBack.Text = "◀";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.BtnBack_Click);
            // 
            // pnChangeLoadSave
            // 
            this.pnChangeLoadSave.Controls.Add(this.btnToggleSaveLoad);
            this.pnChangeLoadSave.Controls.Add(this.btnSaveLoad);
            this.pnChangeLoadSave.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnChangeLoadSave.Location = new System.Drawing.Point(0, 0);
            this.pnChangeLoadSave.Name = "pnChangeLoadSave";
            this.pnChangeLoadSave.Size = new System.Drawing.Size(150, 35);
            this.pnChangeLoadSave.TabIndex = 1;
            // 
            // btnToggleSaveLoad
            // 
            this.btnToggleSaveLoad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnToggleSaveLoad.FlatAppearance.BorderSize = 0;
            this.btnToggleSaveLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToggleSaveLoad.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnToggleSaveLoad.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnToggleSaveLoad.Location = new System.Drawing.Point(0, 4);
            this.btnToggleSaveLoad.Name = "btnToggleSaveLoad";
            this.btnToggleSaveLoad.Size = new System.Drawing.Size(71, 24);
            this.btnToggleSaveLoad.TabIndex = 129;
            this.btnToggleSaveLoad.TabStop = false;
            this.btnToggleSaveLoad.Tag = "color:dark2";
            this.btnToggleSaveLoad.Text = "Change ->";
            this.btnToggleSaveLoad.UseVisualStyleBackColor = false;
            this.btnToggleSaveLoad.Click += new System.EventHandler(this.BtnToggleSaveLoad_Click);
            // 
            // btnSaveLoad
            // 
            this.btnSaveLoad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnSaveLoad.FlatAppearance.BorderSize = 0;
            this.btnSaveLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveLoad.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnSaveLoad.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnSaveLoad.Location = new System.Drawing.Point(78, 4);
            this.btnSaveLoad.Name = "btnSaveLoad";
            this.btnSaveLoad.Size = new System.Drawing.Size(71, 24);
            this.btnSaveLoad.TabIndex = 130;
            this.btnSaveLoad.TabStop = false;
            this.btnSaveLoad.Tag = "color:dark2";
            this.btnSaveLoad.Text = "LOAD";
            this.btnSaveLoad.UseVisualStyleBackColor = false;
            this.btnSaveLoad.Click += new System.EventHandler(this.HandleSaveLoadClick);
            this.btnSaveLoad.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnSaveLoad_MouseDown);
            // 
            // SavestateList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnBackForwardPage);
            this.Controls.Add(this.flowPanel);
            this.Controls.Add(this.pnChangeLoadSave);
            this.Margin = new System.Windows.Forms.Padding(1);
            this.Name = "SavestateList";
            this.Size = new System.Drawing.Size(150, 280);
            this.pnBackForwardPage.ResumeLayout(false);
            this.pnChangeLoadSave.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pnBackForwardPage;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnForward;
        private System.Windows.Forms.Panel pnChangeLoadSave;
        private System.Windows.Forms.Button btnToggleSaveLoad;
        public System.Windows.Forms.Button btnSaveLoad;
        public System.Windows.Forms.FlowLayoutPanel flowPanel;
    }
}
