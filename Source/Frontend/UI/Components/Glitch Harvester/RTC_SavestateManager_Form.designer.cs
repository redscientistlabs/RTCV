namespace RTCV.UI
{
    partial class RTC_SavestateManager_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RTC_SavestateManager_Form));
            this.btnSaveSavestateList = new System.Windows.Forms.Button();
            this.btnLoadSavestateList = new System.Windows.Forms.Button();
            this.cbSavestateLoadOnClick = new System.Windows.Forms.CheckBox();
            this.savestateList = new RTCV.UI.Components.Controls.SavestateList();
            this.SuspendLayout();
            // 
            // btnSaveSavestateList
            // 
            this.btnSaveSavestateList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnSaveSavestateList.FlatAppearance.BorderSize = 0;
            this.btnSaveSavestateList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveSavestateList.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnSaveSavestateList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnSaveSavestateList.Location = new System.Drawing.Point(89, 308);
            this.btnSaveSavestateList.Name = "btnSaveSavestateList";
            this.btnSaveSavestateList.Size = new System.Drawing.Size(73, 24);
            this.btnSaveSavestateList.TabIndex = 168;
            this.btnSaveSavestateList.TabStop = false;
            this.btnSaveSavestateList.Tag = "color:dark2";
            this.btnSaveSavestateList.Text = "Save List";
            this.btnSaveSavestateList.UseVisualStyleBackColor = false;
            this.btnSaveSavestateList.Click += new System.EventHandler(this.btnSaveSavestateList_Click);
            // 
            // btnLoadSavestateList
            // 
            this.btnLoadSavestateList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnLoadSavestateList.FlatAppearance.BorderSize = 0;
            this.btnLoadSavestateList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadSavestateList.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnLoadSavestateList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnLoadSavestateList.Location = new System.Drawing.Point(14, 308);
            this.btnLoadSavestateList.Name = "btnLoadSavestateList";
            this.btnLoadSavestateList.Size = new System.Drawing.Size(72, 24);
            this.btnLoadSavestateList.TabIndex = 167;
            this.btnLoadSavestateList.TabStop = false;
            this.btnLoadSavestateList.Tag = "color:dark2";
            this.btnLoadSavestateList.Text = "Load List";
            this.btnLoadSavestateList.UseVisualStyleBackColor = false;
            this.btnLoadSavestateList.Click += new System.EventHandler(this.btnLoadSavestateList_Click);
            this.btnLoadSavestateList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnLoadSavestateList_MouseDown);
            // 
            // cbSavestateLoadOnClick
            // 
            this.cbSavestateLoadOnClick.AutoSize = true;
            this.cbSavestateLoadOnClick.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbSavestateLoadOnClick.ForeColor = System.Drawing.Color.White;
            this.cbSavestateLoadOnClick.Location = new System.Drawing.Point(13, 287);
            this.cbSavestateLoadOnClick.Name = "cbSavestateLoadOnClick";
            this.cbSavestateLoadOnClick.Size = new System.Drawing.Size(121, 17);
            this.cbSavestateLoadOnClick.TabIndex = 163;
            this.cbSavestateLoadOnClick.TabStop = false;
            this.cbSavestateLoadOnClick.Text = "Load state on click";
            this.cbSavestateLoadOnClick.UseVisualStyleBackColor = true;
            // 
            // savestateList
            // 
            this.savestateList.DataSource = null;
            this.savestateList.Location = new System.Drawing.Point(12, 0);
            this.savestateList.Margin = new System.Windows.Forms.Padding(1);
            this.savestateList.Name = "savestateList";
            this.savestateList.Size = new System.Drawing.Size(150, 280);
            this.savestateList.TabIndex = 169;
            // 
            // RTC_SavestateManager_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(172, 347);
            this.Controls.Add(this.savestateList);
            this.Controls.Add(this.btnSaveSavestateList);
            this.Controls.Add(this.btnLoadSavestateList);
            this.Controls.Add(this.cbSavestateLoadOnClick);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RTC_SavestateManager_Form";
            this.Tag = "color:dark1";
            this.Text = "Savestate Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HandleFormClosing);
            this.Load += new System.EventHandler(this.RTC_SavestateManager_Form_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSaveSavestateList;
        private System.Windows.Forms.Button btnLoadSavestateList;
        public System.Windows.Forms.CheckBox cbSavestateLoadOnClick;
        public Components.Controls.SavestateList savestateList;
    }
}