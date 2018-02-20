namespace RTCV.TestForm
{
    partial class TestMainForm
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
            this.pnNetCoreCreate = new System.Windows.Forms.Panel();
            this.btnRestart = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.lbNetCoreStatus = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbCustomTarget = new System.Windows.Forms.TextBox();
            this.rbConnectTextbox = new System.Windows.Forms.RadioButton();
            this.rbConnectLoopback = new System.Windows.Forms.RadioButton();
            this.btnKillNetCore = new System.Windows.Forms.Button();
            this.btnStopNetCore = new System.Windows.Forms.Button();
            this.btnStartNetCore = new System.Windows.Forms.Button();
            this.tbNetCoreOutput = new System.Windows.Forms.TextBox();
            this.btnClearConsole = new System.Windows.Forms.Button();
            this.cbDisplayConsole = new System.Windows.Forms.CheckBox();
            this.cbShowDebug = new System.Windows.Forms.CheckBox();
            this.pnNetCoreCreate.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnNetCoreCreate
            // 
            this.pnNetCoreCreate.Controls.Add(this.btnRestart);
            this.pnNetCoreCreate.Controls.Add(this.label1);
            this.pnNetCoreCreate.Controls.Add(this.tbPort);
            this.pnNetCoreCreate.Controls.Add(this.lbNetCoreStatus);
            this.pnNetCoreCreate.Controls.Add(this.groupBox1);
            this.pnNetCoreCreate.Controls.Add(this.btnKillNetCore);
            this.pnNetCoreCreate.Controls.Add(this.btnStopNetCore);
            this.pnNetCoreCreate.Controls.Add(this.btnStartNetCore);
            this.pnNetCoreCreate.Location = new System.Drawing.Point(12, 12);
            this.pnNetCoreCreate.Name = "pnNetCoreCreate";
            this.pnNetCoreCreate.Size = new System.Drawing.Size(443, 119);
            this.pnNetCoreCreate.TabIndex = 0;
            // 
            // btnRestart
            // 
            this.btnRestart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRestart.Location = new System.Drawing.Point(343, 48);
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.Size = new System.Drawing.Size(87, 29);
            this.btnRestart.TabIndex = 5;
            this.btnRestart.Text = "Restart";
            this.btnRestart.UseVisualStyleBackColor = true;
            this.btnRestart.Click += new System.EventHandler(this.btnRestart_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 92);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Port";
            // 
            // tbPort
            // 
            this.tbPort.Location = new System.Drawing.Point(53, 89);
            this.tbPort.Name = "tbPort";
            this.tbPort.Size = new System.Drawing.Size(48, 20);
            this.tbPort.TabIndex = 2;
            this.tbPort.Text = "42069";
            // 
            // lbNetCoreStatus
            // 
            this.lbNetCoreStatus.AutoSize = true;
            this.lbNetCoreStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbNetCoreStatus.Location = new System.Drawing.Point(229, 15);
            this.lbNetCoreStatus.Name = "lbNetCoreStatus";
            this.lbNetCoreStatus.Size = new System.Drawing.Size(184, 20);
            this.lbNetCoreStatus.TabIndex = 4;
            this.lbNetCoreStatus.Text = "Status : UNINITIALIZED";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbCustomTarget);
            this.groupBox1.Controls.Add(this.rbConnectTextbox);
            this.groupBox1.Controls.Add(this.rbConnectLoopback);
            this.groupBox1.Location = new System.Drawing.Point(14, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(203, 71);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Target";
            // 
            // tbCustomTarget
            // 
            this.tbCustomTarget.Location = new System.Drawing.Point(87, 40);
            this.tbCustomTarget.Name = "tbCustomTarget";
            this.tbCustomTarget.Size = new System.Drawing.Size(100, 20);
            this.tbCustomTarget.TabIndex = 1;
            // 
            // rbConnectTextbox
            // 
            this.rbConnectTextbox.AutoSize = true;
            this.rbConnectTextbox.Location = new System.Drawing.Point(10, 41);
            this.rbConnectTextbox.Name = "rbConnectTextbox";
            this.rbConnectTextbox.Size = new System.Drawing.Size(77, 17);
            this.rbConnectTextbox.TabIndex = 1;
            this.rbConnectTextbox.Text = "Connect to";
            this.rbConnectTextbox.UseVisualStyleBackColor = true;
            this.rbConnectTextbox.CheckedChanged += new System.EventHandler(this.rbConnectTextbox_CheckedChanged);
            // 
            // rbConnectLoopback
            // 
            this.rbConnectLoopback.AutoSize = true;
            this.rbConnectLoopback.Checked = true;
            this.rbConnectLoopback.Location = new System.Drawing.Point(10, 18);
            this.rbConnectLoopback.Name = "rbConnectLoopback";
            this.rbConnectLoopback.Size = new System.Drawing.Size(131, 17);
            this.rbConnectLoopback.TabIndex = 0;
            this.rbConnectLoopback.TabStop = true;
            this.rbConnectLoopback.Text = "Connect on Loopback";
            this.rbConnectLoopback.UseVisualStyleBackColor = true;
            this.rbConnectLoopback.CheckedChanged += new System.EventHandler(this.rbConnectLoopback_CheckedChanged);
            // 
            // btnKillNetCore
            // 
            this.btnKillNetCore.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnKillNetCore.Location = new System.Drawing.Point(343, 83);
            this.btnKillNetCore.Name = "btnKillNetCore";
            this.btnKillNetCore.Size = new System.Drawing.Size(87, 29);
            this.btnKillNetCore.TabIndex = 2;
            this.btnKillNetCore.Text = "Kill";
            this.btnKillNetCore.UseVisualStyleBackColor = true;
            this.btnKillNetCore.Click += new System.EventHandler(this.btnKillNetCore_Click);
            // 
            // btnStopNetCore
            // 
            this.btnStopNetCore.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStopNetCore.Location = new System.Drawing.Point(227, 83);
            this.btnStopNetCore.Name = "btnStopNetCore";
            this.btnStopNetCore.Size = new System.Drawing.Size(110, 29);
            this.btnStopNetCore.TabIndex = 1;
            this.btnStopNetCore.Text = "Stop NetCore";
            this.btnStopNetCore.UseVisualStyleBackColor = true;
            this.btnStopNetCore.Click += new System.EventHandler(this.btnStopNetCore_Click);
            // 
            // btnStartNetCore
            // 
            this.btnStartNetCore.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartNetCore.Location = new System.Drawing.Point(227, 48);
            this.btnStartNetCore.Name = "btnStartNetCore";
            this.btnStartNetCore.Size = new System.Drawing.Size(110, 29);
            this.btnStartNetCore.TabIndex = 0;
            this.btnStartNetCore.Text = "Start NetCore";
            this.btnStartNetCore.UseVisualStyleBackColor = true;
            this.btnStartNetCore.Click += new System.EventHandler(this.btnStartNetCore_Click);
            // 
            // tbNetCoreOutput
            // 
            this.tbNetCoreOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbNetCoreOutput.BackColor = System.Drawing.Color.Black;
            this.tbNetCoreOutput.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbNetCoreOutput.ForeColor = System.Drawing.Color.White;
            this.tbNetCoreOutput.Location = new System.Drawing.Point(12, 172);
            this.tbNetCoreOutput.Multiline = true;
            this.tbNetCoreOutput.Name = "tbNetCoreOutput";
            this.tbNetCoreOutput.Size = new System.Drawing.Size(443, 262);
            this.tbNetCoreOutput.TabIndex = 5;
            // 
            // btnClearConsole
            // 
            this.btnClearConsole.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearConsole.Location = new System.Drawing.Point(12, 137);
            this.btnClearConsole.Name = "btnClearConsole";
            this.btnClearConsole.Size = new System.Drawing.Size(135, 29);
            this.btnClearConsole.TabIndex = 6;
            this.btnClearConsole.Text = "Clear Console";
            this.btnClearConsole.UseVisualStyleBackColor = true;
            this.btnClearConsole.Click += new System.EventHandler(this.btnClearConsole_Click);
            // 
            // cbDisplayConsole
            // 
            this.cbDisplayConsole.AutoSize = true;
            this.cbDisplayConsole.Checked = true;
            this.cbDisplayConsole.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbDisplayConsole.Location = new System.Drawing.Point(153, 149);
            this.cbDisplayConsole.Name = "cbDisplayConsole";
            this.cbDisplayConsole.Size = new System.Drawing.Size(95, 17);
            this.cbDisplayConsole.TabIndex = 7;
            this.cbDisplayConsole.Text = "Display Output";
            this.cbDisplayConsole.UseVisualStyleBackColor = true;
            // 
            // cbShowDebug
            // 
            this.cbShowDebug.AutoSize = true;
            this.cbShowDebug.Location = new System.Drawing.Point(254, 149);
            this.cbShowDebug.Name = "cbShowDebug";
            this.cbShowDebug.Size = new System.Drawing.Size(119, 17);
            this.cbShowDebug.TabIndex = 8;
            this.cbShowDebug.Text = "Show all debug info";
            this.cbShowDebug.UseVisualStyleBackColor = true;
            this.cbShowDebug.CheckedChanged += new System.EventHandler(this.cbShowDebug_CheckedChanged);
            // 
            // TestMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(469, 446);
            this.Controls.Add(this.cbShowDebug);
            this.Controls.Add(this.cbDisplayConsole);
            this.Controls.Add(this.btnClearConsole);
            this.Controls.Add(this.tbNetCoreOutput);
            this.Controls.Add(this.pnNetCoreCreate);
            this.Name = "TestMainForm";
            this.Text = "NetCore TestForm";
            this.pnNetCoreCreate.ResumeLayout(false);
            this.pnNetCoreCreate.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnNetCoreCreate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbPort;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbCustomTarget;
        private System.Windows.Forms.RadioButton rbConnectTextbox;
        private System.Windows.Forms.RadioButton rbConnectLoopback;
        private System.Windows.Forms.Button btnStartNetCore;
        private System.Windows.Forms.Button btnStopNetCore;
        private System.Windows.Forms.Button btnKillNetCore;
        private System.Windows.Forms.Label lbNetCoreStatus;
        private System.Windows.Forms.Button btnRestart;
        private System.Windows.Forms.TextBox tbNetCoreOutput;
        private System.Windows.Forms.Button btnClearConsole;
        private System.Windows.Forms.CheckBox cbDisplayConsole;
        private System.Windows.Forms.CheckBox cbShowDebug;
    }
}

