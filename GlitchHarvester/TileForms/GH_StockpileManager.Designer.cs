namespace RTCV.GlitchHarvester.TileForms
{
    partial class GH_StockpileManager
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvStockpile = new System.Windows.Forms.DataGridView();
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
            this.label3 = new System.Windows.Forms.Label();
            this.btnRemoveSelectedStockpile = new System.Windows.Forms.Button();
            this.cbCompressStockpiles = new System.Windows.Forms.CheckBox();
            this.Item = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GameName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SystemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SystemCore = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Note = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStockpile)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvStockpile
            // 
            this.dgvStockpile.AllowUserToAddRows = false;
            this.dgvStockpile.AllowUserToDeleteRows = false;
            this.dgvStockpile.AllowUserToResizeRows = false;
            this.dgvStockpile.BackgroundColor = System.Drawing.Color.Gray;
            this.dgvStockpile.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvStockpile.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStockpile.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Item,
            this.GameName,
            this.SystemName,
            this.SystemCore,
            this.Note});
            this.dgvStockpile.GridColor = System.Drawing.Color.Black;
            this.dgvStockpile.Location = new System.Drawing.Point(10, 40);
            this.dgvStockpile.Name = "dgvStockpile";
            this.dgvStockpile.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.dgvStockpile.RowHeadersVisible = false;
            this.dgvStockpile.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvStockpile.Size = new System.Drawing.Size(404, 342);
            this.dgvStockpile.TabIndex = 157;
            this.dgvStockpile.Tag = "color:normal";
            // 
            // btnRenameSelected
            // 
            this.btnRenameSelected.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnRenameSelected.FlatAppearance.BorderSize = 0;
            this.btnRenameSelected.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRenameSelected.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnRenameSelected.ForeColor = System.Drawing.Color.Black;
            this.btnRenameSelected.Location = new System.Drawing.Point(165, 390);
            this.btnRenameSelected.Name = "btnRenameSelected";
            this.btnRenameSelected.Size = new System.Drawing.Size(65, 24);
            this.btnRenameSelected.TabIndex = 156;
            this.btnRenameSelected.TabStop = false;
            this.btnRenameSelected.Tag = "color:light";
            this.btnRenameSelected.Text = "Rename";
            this.btnRenameSelected.UseVisualStyleBackColor = false;
            // 
            // btnImportStockpile
            // 
            this.btnImportStockpile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnImportStockpile.FlatAppearance.BorderSize = 0;
            this.btnImportStockpile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImportStockpile.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnImportStockpile.ForeColor = System.Drawing.Color.Black;
            this.btnImportStockpile.Location = new System.Drawing.Point(311, 9);
            this.btnImportStockpile.Name = "btnImportStockpile";
            this.btnImportStockpile.Size = new System.Drawing.Size(50, 22);
            this.btnImportStockpile.TabIndex = 155;
            this.btnImportStockpile.TabStop = false;
            this.btnImportStockpile.Text = "Import";
            this.btnImportStockpile.UseVisualStyleBackColor = false;
            // 
            // btnStockpileMoveSelectedDown
            // 
            this.btnStockpileMoveSelectedDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnStockpileMoveSelectedDown.FlatAppearance.BorderSize = 0;
            this.btnStockpileMoveSelectedDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStockpileMoveSelectedDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.btnStockpileMoveSelectedDown.ForeColor = System.Drawing.Color.Black;
            this.btnStockpileMoveSelectedDown.Location = new System.Drawing.Point(380, 390);
            this.btnStockpileMoveSelectedDown.Name = "btnStockpileMoveSelectedDown";
            this.btnStockpileMoveSelectedDown.Size = new System.Drawing.Size(33, 24);
            this.btnStockpileMoveSelectedDown.TabIndex = 154;
            this.btnStockpileMoveSelectedDown.TabStop = false;
            this.btnStockpileMoveSelectedDown.Tag = "color:light";
            this.btnStockpileMoveSelectedDown.Text = "▼▼";
            this.btnStockpileMoveSelectedDown.UseVisualStyleBackColor = false;
            // 
            // btnStockpileMoveSelectedUp
            // 
            this.btnStockpileMoveSelectedUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnStockpileMoveSelectedUp.FlatAppearance.BorderSize = 0;
            this.btnStockpileMoveSelectedUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStockpileMoveSelectedUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.btnStockpileMoveSelectedUp.ForeColor = System.Drawing.Color.Black;
            this.btnStockpileMoveSelectedUp.Location = new System.Drawing.Point(344, 390);
            this.btnStockpileMoveSelectedUp.Margin = new System.Windows.Forms.Padding(0);
            this.btnStockpileMoveSelectedUp.Name = "btnStockpileMoveSelectedUp";
            this.btnStockpileMoveSelectedUp.Size = new System.Drawing.Size(33, 24);
            this.btnStockpileMoveSelectedUp.TabIndex = 153;
            this.btnStockpileMoveSelectedUp.TabStop = false;
            this.btnStockpileMoveSelectedUp.Tag = "color:light";
            this.btnStockpileMoveSelectedUp.Text = "▲▲";
            this.btnStockpileMoveSelectedUp.UseVisualStyleBackColor = false;
            // 
            // btnLoadStockpile
            // 
            this.btnLoadStockpile.BackColor = System.Drawing.Color.Orange;
            this.btnLoadStockpile.FlatAppearance.BorderSize = 0;
            this.btnLoadStockpile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadStockpile.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnLoadStockpile.ForeColor = System.Drawing.Color.Black;
            this.btnLoadStockpile.Location = new System.Drawing.Point(151, 9);
            this.btnLoadStockpile.Name = "btnLoadStockpile";
            this.btnLoadStockpile.Size = new System.Drawing.Size(50, 22);
            this.btnLoadStockpile.TabIndex = 147;
            this.btnLoadStockpile.TabStop = false;
            this.btnLoadStockpile.Text = "Load";
            this.btnLoadStockpile.UseVisualStyleBackColor = false;
            // 
            // btnSaveStockpile
            // 
            this.btnSaveStockpile.BackColor = System.Drawing.Color.LightGray;
            this.btnSaveStockpile.Enabled = false;
            this.btnSaveStockpile.FlatAppearance.BorderSize = 0;
            this.btnSaveStockpile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveStockpile.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnSaveStockpile.ForeColor = System.Drawing.Color.DimGray;
            this.btnSaveStockpile.Location = new System.Drawing.Point(265, 9);
            this.btnSaveStockpile.Name = "btnSaveStockpile";
            this.btnSaveStockpile.Size = new System.Drawing.Size(44, 22);
            this.btnSaveStockpile.TabIndex = 151;
            this.btnSaveStockpile.TabStop = false;
            this.btnSaveStockpile.Text = "Save";
            this.btnSaveStockpile.UseVisualStyleBackColor = false;
            // 
            // btnSaveStockpileAs
            // 
            this.btnSaveStockpileAs.BackColor = System.Drawing.Color.Firebrick;
            this.btnSaveStockpileAs.FlatAppearance.BorderSize = 0;
            this.btnSaveStockpileAs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveStockpileAs.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnSaveStockpileAs.ForeColor = System.Drawing.Color.Black;
            this.btnSaveStockpileAs.Location = new System.Drawing.Point(204, 9);
            this.btnSaveStockpileAs.Name = "btnSaveStockpileAs";
            this.btnSaveStockpileAs.Size = new System.Drawing.Size(59, 22);
            this.btnSaveStockpileAs.TabIndex = 146;
            this.btnSaveStockpileAs.TabStop = false;
            this.btnSaveStockpileAs.Text = "Save as";
            this.btnSaveStockpileAs.UseVisualStyleBackColor = false;
            // 
            // btnClearStockpile
            // 
            this.btnClearStockpile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnClearStockpile.FlatAppearance.BorderSize = 0;
            this.btnClearStockpile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearStockpile.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnClearStockpile.ForeColor = System.Drawing.Color.Black;
            this.btnClearStockpile.Location = new System.Drawing.Point(10, 390);
            this.btnClearStockpile.Name = "btnClearStockpile";
            this.btnClearStockpile.Size = new System.Drawing.Size(92, 24);
            this.btnClearStockpile.TabIndex = 152;
            this.btnClearStockpile.TabStop = false;
            this.btnClearStockpile.Tag = "color:light";
            this.btnClearStockpile.Text = "Clear Stockpile";
            this.btnClearStockpile.UseVisualStyleBackColor = false;
            // 
            // btnStockpileDOWN
            // 
            this.btnStockpileDOWN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnStockpileDOWN.FlatAppearance.BorderSize = 0;
            this.btnStockpileDOWN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStockpileDOWN.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnStockpileDOWN.ForeColor = System.Drawing.Color.Black;
            this.btnStockpileDOWN.Location = new System.Drawing.Point(390, 9);
            this.btnStockpileDOWN.Name = "btnStockpileDOWN";
            this.btnStockpileDOWN.Size = new System.Drawing.Size(25, 22);
            this.btnStockpileDOWN.TabIndex = 150;
            this.btnStockpileDOWN.TabStop = false;
            this.btnStockpileDOWN.Tag = "color:light";
            this.btnStockpileDOWN.Text = "▼";
            this.btnStockpileDOWN.UseVisualStyleBackColor = false;
            // 
            // btnStockpileUP
            // 
            this.btnStockpileUP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnStockpileUP.FlatAppearance.BorderSize = 0;
            this.btnStockpileUP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStockpileUP.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnStockpileUP.ForeColor = System.Drawing.Color.Black;
            this.btnStockpileUP.Location = new System.Drawing.Point(363, 9);
            this.btnStockpileUP.Name = "btnStockpileUP";
            this.btnStockpileUP.Size = new System.Drawing.Size(25, 22);
            this.btnStockpileUP.TabIndex = 149;
            this.btnStockpileUP.TabStop = false;
            this.btnStockpileUP.Tag = "color:light";
            this.btnStockpileUP.Text = "▲";
            this.btnStockpileUP.UseVisualStyleBackColor = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(9, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(122, 19);
            this.label3.TabIndex = 148;
            this.label3.Text = "Stockpile Manager";
            // 
            // btnRemoveSelectedStockpile
            // 
            this.btnRemoveSelectedStockpile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnRemoveSelectedStockpile.FlatAppearance.BorderSize = 0;
            this.btnRemoveSelectedStockpile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveSelectedStockpile.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnRemoveSelectedStockpile.ForeColor = System.Drawing.Color.Black;
            this.btnRemoveSelectedStockpile.Location = new System.Drawing.Point(105, 390);
            this.btnRemoveSelectedStockpile.Name = "btnRemoveSelectedStockpile";
            this.btnRemoveSelectedStockpile.Size = new System.Drawing.Size(57, 24);
            this.btnRemoveSelectedStockpile.TabIndex = 145;
            this.btnRemoveSelectedStockpile.TabStop = false;
            this.btnRemoveSelectedStockpile.Tag = "color:light";
            this.btnRemoveSelectedStockpile.Text = "Remove";
            this.btnRemoveSelectedStockpile.UseVisualStyleBackColor = false;
            // 
            // cbCompressStockpiles
            // 
            this.cbCompressStockpiles.AutoSize = true;
            this.cbCompressStockpiles.Checked = true;
            this.cbCompressStockpiles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCompressStockpiles.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbCompressStockpiles.ForeColor = System.Drawing.Color.White;
            this.cbCompressStockpiles.Location = new System.Drawing.Point(261, 388);
            this.cbCompressStockpiles.Name = "cbCompressStockpiles";
            this.cbCompressStockpiles.Size = new System.Drawing.Size(78, 30);
            this.cbCompressStockpiles.TabIndex = 158;
            this.cbCompressStockpiles.TabStop = false;
            this.cbCompressStockpiles.Text = "Compress\nStockpiles";
            this.cbCompressStockpiles.UseVisualStyleBackColor = true;
            // 
            // Item
            // 
            this.Item.HeaderText = "Item Name";
            this.Item.Name = "Item";
            this.Item.ReadOnly = true;
            this.Item.Width = 200;
            // 
            // GameName
            // 
            this.GameName.HeaderText = "Game";
            this.GameName.Name = "GameName";
            this.GameName.ReadOnly = true;
            this.GameName.Width = 106;
            // 
            // SystemName
            // 
            this.SystemName.HeaderText = "System";
            this.SystemName.Name = "SystemName";
            this.SystemName.ReadOnly = true;
            this.SystemName.Width = 45;
            // 
            // SystemCore
            // 
            this.SystemCore.HeaderText = "Core";
            this.SystemCore.Name = "SystemCore";
            this.SystemCore.Visible = false;
            this.SystemCore.Width = 150;
            // 
            // Note
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            this.Note.DefaultCellStyle = dataGridViewCellStyle2;
            this.Note.HeaderText = "Note";
            this.Note.Name = "Note";
            this.Note.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Note.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Note.Text = "";
            this.Note.Width = 35;
            // 
            // GH_StockpileManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(425, 425);
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
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnRemoveSelectedStockpile);
            this.Controls.Add(this.cbCompressStockpiles);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "GH_StockpileManager";
            this.Text = "GH_DummyTileForm";
            ((System.ComponentModel.ISupportInitialize)(this.dgvStockpile)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.DataGridView dgvStockpile;
        private System.Windows.Forms.Button btnRenameSelected;
        private System.Windows.Forms.Button btnImportStockpile;
        private System.Windows.Forms.Button btnStockpileMoveSelectedDown;
        private System.Windows.Forms.Button btnStockpileMoveSelectedUp;
        private System.Windows.Forms.Button btnLoadStockpile;
        public System.Windows.Forms.Button btnSaveStockpile;
        private System.Windows.Forms.Button btnSaveStockpileAs;
        private System.Windows.Forms.Button btnClearStockpile;
        public System.Windows.Forms.Button btnStockpileDOWN;
        public System.Windows.Forms.Button btnStockpileUP;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnRemoveSelectedStockpile;
        public System.Windows.Forms.CheckBox cbCompressStockpiles;
        private System.Windows.Forms.DataGridViewTextBoxColumn Item;
        private System.Windows.Forms.DataGridViewTextBoxColumn GameName;
        private System.Windows.Forms.DataGridViewTextBoxColumn SystemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn SystemCore;
        private System.Windows.Forms.DataGridViewButtonColumn Note;
    }
}