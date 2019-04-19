namespace RTCV.UI
{
    partial class RTC_StockpileManager_Form
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvStockpile = new RTCV.UI.DataGridViewDraggable();
            this.Item = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GameName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SystemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SystemCore = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Note = new System.Windows.Forms.DataGridViewButtonColumn();
            this.btnRenameSelected = new System.Windows.Forms.Button();
            this.btnImportStockpile = new System.Windows.Forms.Button();
            this.btnStockpileMoveSelectedDown = new System.Windows.Forms.Button();
            this.btnStockpileMoveSelectedUp = new System.Windows.Forms.Button();
            this.btnLoadStockpile = new System.Windows.Forms.Button();
            this.btnSaveStockpile = new System.Windows.Forms.Button();
            this.btnSaveStockpileAs = new System.Windows.Forms.Button();
            this.btnClearStockpile = new System.Windows.Forms.Button();
            this.btnStockpileDOWN = new System.Windows.Forms.Button();
            this.btnStockpileUP = new System.Windows.Forms.Button();
            this.btnRemoveSelectedStockpile = new System.Windows.Forms.Button();
            this.cbCompressStockpiles = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStockpile)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvStockpile
            // 
            this.dgvStockpile.AllowDrop = true;
            this.dgvStockpile.AllowUserToAddRows = false;
            this.dgvStockpile.AllowUserToDeleteRows = false;
            this.dgvStockpile.AllowUserToResizeRows = false;
            this.dgvStockpile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvStockpile.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvStockpile.BackgroundColor = System.Drawing.Color.Gray;
            this.dgvStockpile.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvStockpile.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvStockpile.ColumnHeadersHeight = 21;
            this.dgvStockpile.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvStockpile.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Item,
            this.GameName,
            this.SystemName,
            this.SystemCore,
            this.Note});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvStockpile.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvStockpile.GridColor = System.Drawing.Color.Black;
            this.dgvStockpile.Location = new System.Drawing.Point(13, 41);
            this.dgvStockpile.Name = "dgvStockpile";
            this.dgvStockpile.RightToLeft = System.Windows.Forms.RightToLeft.No;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvStockpile.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvStockpile.RowHeadersVisible = false;
            this.dgvStockpile.RowTemplate.Height = 25;
            this.dgvStockpile.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvStockpile.Size = new System.Drawing.Size(489, 408);
            this.dgvStockpile.TabIndex = 169;
            this.dgvStockpile.Tag = "color:dark1";
            // 
            // Item
            // 
            this.Item.FillWeight = 145F;
            this.Item.HeaderText = "Item Name";
            this.Item.Name = "Item";
            this.Item.ReadOnly = true;
            // 
            // GameName
            // 
            this.GameName.FillWeight = 76.73162F;
            this.GameName.HeaderText = "Game";
            this.GameName.Name = "GameName";
            this.GameName.ReadOnly = true;
            // 
            // SystemName
            // 
            this.SystemName.FillWeight = 60F;
            this.SystemName.HeaderText = "System";
            this.SystemName.Name = "SystemName";
            this.SystemName.ReadOnly = true;
            // 
            // SystemCore
            // 
            this.SystemCore.FillWeight = 60F;
            this.SystemCore.HeaderText = "Core";
            this.SystemCore.Name = "SystemCore";
            this.SystemCore.ReadOnly = true;
            this.SystemCore.Visible = false;
            // 
            // Note
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI Symbol", 14.25F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            this.Note.DefaultCellStyle = dataGridViewCellStyle2;
            this.Note.FillWeight = 23.01949F;
            this.Note.HeaderText = "Note";
            this.Note.MinimumWidth = 35;
            this.Note.Name = "Note";
            this.Note.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Note.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Note.Text = "";
            // 
            // btnRenameSelected
            // 
            this.btnRenameSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRenameSelected.BackColor = System.Drawing.Color.Gray;
            this.btnRenameSelected.FlatAppearance.BorderSize = 0;
            this.btnRenameSelected.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRenameSelected.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnRenameSelected.ForeColor = System.Drawing.Color.White;
            this.btnRenameSelected.Location = new System.Drawing.Point(336, 460);
            this.btnRenameSelected.Name = "btnRenameSelected";
            this.btnRenameSelected.Size = new System.Drawing.Size(86, 24);
            this.btnRenameSelected.TabIndex = 168;
            this.btnRenameSelected.TabStop = false;
            this.btnRenameSelected.Tag = "color:light1";
            this.btnRenameSelected.Text = "Rename Item";
            this.btnRenameSelected.UseVisualStyleBackColor = false;
            // 
            // btnImportStockpile
            // 
            this.btnImportStockpile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImportStockpile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnImportStockpile.FlatAppearance.BorderSize = 0;
            this.btnImportStockpile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImportStockpile.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnImportStockpile.ForeColor = System.Drawing.Color.Black;
            this.btnImportStockpile.Location = new System.Drawing.Point(346, 15);
            this.btnImportStockpile.Name = "btnImportStockpile";
            this.btnImportStockpile.Size = new System.Drawing.Size(50, 22);
            this.btnImportStockpile.TabIndex = 167;
            this.btnImportStockpile.TabStop = false;
            this.btnImportStockpile.Text = "Import";
            this.btnImportStockpile.UseVisualStyleBackColor = false;
            // 
            // btnStockpileMoveSelectedDown
            // 
            this.btnStockpileMoveSelectedDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStockpileMoveSelectedDown.BackColor = System.Drawing.Color.Gray;
            this.btnStockpileMoveSelectedDown.FlatAppearance.BorderSize = 0;
            this.btnStockpileMoveSelectedDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStockpileMoveSelectedDown.Font = new System.Drawing.Font("Segoe UI Symbol", 7F);
            this.btnStockpileMoveSelectedDown.ForeColor = System.Drawing.Color.White;
            this.btnStockpileMoveSelectedDown.Location = new System.Drawing.Point(466, 461);
            this.btnStockpileMoveSelectedDown.Name = "btnStockpileMoveSelectedDown";
            this.btnStockpileMoveSelectedDown.Size = new System.Drawing.Size(33, 24);
            this.btnStockpileMoveSelectedDown.TabIndex = 166;
            this.btnStockpileMoveSelectedDown.TabStop = false;
            this.btnStockpileMoveSelectedDown.Tag = "color:light1";
            this.btnStockpileMoveSelectedDown.Text = "▼▼";
            this.btnStockpileMoveSelectedDown.UseVisualStyleBackColor = false;
            // 
            // btnStockpileMoveSelectedUp
            // 
            this.btnStockpileMoveSelectedUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStockpileMoveSelectedUp.BackColor = System.Drawing.Color.Gray;
            this.btnStockpileMoveSelectedUp.FlatAppearance.BorderSize = 0;
            this.btnStockpileMoveSelectedUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStockpileMoveSelectedUp.Font = new System.Drawing.Font("Segoe UI Symbol", 7F);
            this.btnStockpileMoveSelectedUp.ForeColor = System.Drawing.Color.White;
            this.btnStockpileMoveSelectedUp.Location = new System.Drawing.Point(428, 461);
            this.btnStockpileMoveSelectedUp.Margin = new System.Windows.Forms.Padding(0);
            this.btnStockpileMoveSelectedUp.Name = "btnStockpileMoveSelectedUp";
            this.btnStockpileMoveSelectedUp.Size = new System.Drawing.Size(33, 24);
            this.btnStockpileMoveSelectedUp.TabIndex = 165;
            this.btnStockpileMoveSelectedUp.TabStop = false;
            this.btnStockpileMoveSelectedUp.Tag = "color:light1";
            this.btnStockpileMoveSelectedUp.Text = "▲▲";
            this.btnStockpileMoveSelectedUp.UseVisualStyleBackColor = false;
            // 
            // btnLoadStockpile
            // 
            this.btnLoadStockpile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadStockpile.BackColor = System.Drawing.Color.Orange;
            this.btnLoadStockpile.FlatAppearance.BorderSize = 0;
            this.btnLoadStockpile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadStockpile.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnLoadStockpile.ForeColor = System.Drawing.Color.Black;
            this.btnLoadStockpile.Location = new System.Drawing.Point(171, 15);
            this.btnLoadStockpile.Name = "btnLoadStockpile";
            this.btnLoadStockpile.Size = new System.Drawing.Size(50, 22);
            this.btnLoadStockpile.TabIndex = 160;
            this.btnLoadStockpile.TabStop = false;
            this.btnLoadStockpile.Text = "Load";
            this.btnLoadStockpile.UseVisualStyleBackColor = false;
            // 
            // btnSaveStockpile
            // 
            this.btnSaveStockpile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveStockpile.BackColor = System.Drawing.Color.LightGray;
            this.btnSaveStockpile.Enabled = false;
            this.btnSaveStockpile.FlatAppearance.BorderSize = 0;
            this.btnSaveStockpile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveStockpile.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnSaveStockpile.ForeColor = System.Drawing.Color.DimGray;
            this.btnSaveStockpile.Location = new System.Drawing.Point(284, 15);
            this.btnSaveStockpile.Name = "btnSaveStockpile";
            this.btnSaveStockpile.Size = new System.Drawing.Size(44, 22);
            this.btnSaveStockpile.TabIndex = 163;
            this.btnSaveStockpile.TabStop = false;
            this.btnSaveStockpile.Text = "Save";
            this.btnSaveStockpile.UseVisualStyleBackColor = false;
            // 
            // btnSaveStockpileAs
            // 
            this.btnSaveStockpileAs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveStockpileAs.BackColor = System.Drawing.Color.Firebrick;
            this.btnSaveStockpileAs.FlatAppearance.BorderSize = 0;
            this.btnSaveStockpileAs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveStockpileAs.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnSaveStockpileAs.ForeColor = System.Drawing.Color.Black;
            this.btnSaveStockpileAs.Location = new System.Drawing.Point(223, 15);
            this.btnSaveStockpileAs.Name = "btnSaveStockpileAs";
            this.btnSaveStockpileAs.Size = new System.Drawing.Size(59, 22);
            this.btnSaveStockpileAs.TabIndex = 159;
            this.btnSaveStockpileAs.TabStop = false;
            this.btnSaveStockpileAs.Text = "Save as";
            this.btnSaveStockpileAs.UseVisualStyleBackColor = false;
            // 
            // btnClearStockpile
            // 
            this.btnClearStockpile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearStockpile.BackColor = System.Drawing.Color.Gray;
            this.btnClearStockpile.FlatAppearance.BorderSize = 0;
            this.btnClearStockpile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearStockpile.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnClearStockpile.ForeColor = System.Drawing.Color.White;
            this.btnClearStockpile.Location = new System.Drawing.Point(414, 15);
            this.btnClearStockpile.Name = "btnClearStockpile";
            this.btnClearStockpile.Size = new System.Drawing.Size(28, 22);
            this.btnClearStockpile.TabIndex = 164;
            this.btnClearStockpile.TabStop = false;
            this.btnClearStockpile.Tag = "color:light1";
            this.btnClearStockpile.Text = "X";
            this.btnClearStockpile.UseVisualStyleBackColor = false;
            // 
            // btnStockpileDOWN
            // 
            this.btnStockpileDOWN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStockpileDOWN.BackColor = System.Drawing.Color.Gray;
            this.btnStockpileDOWN.FlatAppearance.BorderSize = 0;
            this.btnStockpileDOWN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStockpileDOWN.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.btnStockpileDOWN.ForeColor = System.Drawing.Color.White;
            this.btnStockpileDOWN.Location = new System.Drawing.Point(474, 15);
            this.btnStockpileDOWN.Name = "btnStockpileDOWN";
            this.btnStockpileDOWN.Size = new System.Drawing.Size(25, 22);
            this.btnStockpileDOWN.TabIndex = 162;
            this.btnStockpileDOWN.TabStop = false;
            this.btnStockpileDOWN.Tag = "color:light1";
            this.btnStockpileDOWN.Text = "▼";
            this.btnStockpileDOWN.UseVisualStyleBackColor = false;
            // 
            // btnStockpileUP
            // 
            this.btnStockpileUP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStockpileUP.BackColor = System.Drawing.Color.Gray;
            this.btnStockpileUP.FlatAppearance.BorderSize = 0;
            this.btnStockpileUP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStockpileUP.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.btnStockpileUP.ForeColor = System.Drawing.Color.White;
            this.btnStockpileUP.Location = new System.Drawing.Point(448, 15);
            this.btnStockpileUP.Name = "btnStockpileUP";
            this.btnStockpileUP.Size = new System.Drawing.Size(25, 22);
            this.btnStockpileUP.TabIndex = 161;
            this.btnStockpileUP.TabStop = false;
            this.btnStockpileUP.Tag = "color:light1";
            this.btnStockpileUP.Text = "▲";
            this.btnStockpileUP.UseVisualStyleBackColor = false;
            // 
            // btnRemoveSelectedStockpile
            // 
            this.btnRemoveSelectedStockpile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveSelectedStockpile.BackColor = System.Drawing.Color.Gray;
            this.btnRemoveSelectedStockpile.FlatAppearance.BorderSize = 0;
            this.btnRemoveSelectedStockpile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveSelectedStockpile.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnRemoveSelectedStockpile.ForeColor = System.Drawing.Color.White;
            this.btnRemoveSelectedStockpile.Location = new System.Drawing.Point(245, 460);
            this.btnRemoveSelectedStockpile.Name = "btnRemoveSelectedStockpile";
            this.btnRemoveSelectedStockpile.Size = new System.Drawing.Size(83, 24);
            this.btnRemoveSelectedStockpile.TabIndex = 158;
            this.btnRemoveSelectedStockpile.TabStop = false;
            this.btnRemoveSelectedStockpile.Tag = "color:light1";
            this.btnRemoveSelectedStockpile.Text = "Remove Item";
            this.btnRemoveSelectedStockpile.UseVisualStyleBackColor = false;
            // 
            // cbCompressStockpiles
            // 
            this.cbCompressStockpiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbCompressStockpiles.AutoSize = true;
            this.cbCompressStockpiles.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbCompressStockpiles.ForeColor = System.Drawing.Color.White;
            this.cbCompressStockpiles.Location = new System.Drawing.Point(103, 466);
            this.cbCompressStockpiles.Name = "cbCompressStockpiles";
            this.cbCompressStockpiles.Size = new System.Drawing.Size(136, 17);
            this.cbCompressStockpiles.TabIndex = 170;
            this.cbCompressStockpiles.TabStop = false;
            this.cbCompressStockpiles.Text = "Compresss Stockpiles";
            this.cbCompressStockpiles.UseVisualStyleBackColor = true;
            // 
            // RTC_StockpileManager_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(514, 499);
            this.Controls.Add(this.dgvStockpile);
            this.Controls.Add(this.btnRenameSelected);
            this.Controls.Add(this.btnImportStockpile);
            this.Controls.Add(this.btnStockpileMoveSelectedDown);
            this.Controls.Add(this.btnStockpileMoveSelectedUp);
            this.Controls.Add(this.btnLoadStockpile);
            this.Controls.Add(this.btnSaveStockpile);
            this.Controls.Add(this.btnSaveStockpileAs);
            this.Controls.Add(this.btnClearStockpile);
            this.Controls.Add(this.btnStockpileDOWN);
            this.Controls.Add(this.btnStockpileUP);
            this.Controls.Add(this.btnRemoveSelectedStockpile);
            this.Controls.Add(this.cbCompressStockpiles);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "RTC_StockpileManager_Form";
            this.Tag = "color:dark1";
            this.Text = "No Tool Selected";
            this.Load += new System.EventHandler(this.RTC_StockpileManager_Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStockpile)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public DataGridViewDraggable dgvStockpile;
        private System.Windows.Forms.DataGridViewTextBoxColumn Item;
        private System.Windows.Forms.DataGridViewTextBoxColumn GameName;
        private System.Windows.Forms.DataGridViewTextBoxColumn SystemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn SystemCore;
        private System.Windows.Forms.DataGridViewButtonColumn Note;
        public System.Windows.Forms.Button btnRenameSelected;
        private System.Windows.Forms.Button btnImportStockpile;
        private System.Windows.Forms.Button btnStockpileMoveSelectedDown;
        private System.Windows.Forms.Button btnStockpileMoveSelectedUp;
        private System.Windows.Forms.Button btnLoadStockpile;
        public System.Windows.Forms.Button btnSaveStockpile;
        private System.Windows.Forms.Button btnSaveStockpileAs;
        private System.Windows.Forms.Button btnClearStockpile;
        public System.Windows.Forms.Button btnStockpileDOWN;
        public System.Windows.Forms.Button btnStockpileUP;
        public System.Windows.Forms.Button btnRemoveSelectedStockpile;
        public System.Windows.Forms.CheckBox cbCompressStockpiles;
    }
}