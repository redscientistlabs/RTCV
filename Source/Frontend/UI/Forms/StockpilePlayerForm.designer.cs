namespace RTCV.UI
{
    using System.Windows.Forms;

    partial class StockpilePlayerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StockpilePlayerForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnNextItem = new System.Windows.Forms.Button();
            this.btnReloadItem = new System.Windows.Forms.Button();
            this.btnPreviousItem = new System.Windows.Forms.Button();
            this.btnLoadStockpile = new System.Windows.Forms.Button();
            this.btnBlastToggle = new System.Windows.Forms.Button();
            this.dgvStockpile = new System.Windows.Forms.DataGridView();
            this.tbNoteBox = new System.Windows.Forms.RichTextBox();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.Item = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GameName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SystemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SystemCore = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Note = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStockpile)).BeginInit();
            this.SuspendLayout();
            // 
            // btnNextItem
            // 
            this.btnNextItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNextItem.BackColor = System.Drawing.Color.LightGreen;
            this.btnNextItem.FlatAppearance.BorderSize = 0;
            this.btnNextItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNextItem.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnNextItem.ForeColor = System.Drawing.Color.Black;
            this.btnNextItem.Location = new System.Drawing.Point(583, 16);
            this.btnNextItem.Name = "btnNextItem";
            this.btnNextItem.Size = new System.Drawing.Size(58, 32);
            this.btnNextItem.TabIndex = 127;
            this.btnNextItem.Text = "Next";
            this.btnNextItem.UseVisualStyleBackColor = false;
            this.btnNextItem.Click += new System.EventHandler(this.TrySelectNext);
            // 
            // btnReloadItem
            // 
            this.btnReloadItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReloadItem.BackColor = System.Drawing.Color.LightGreen;
            this.btnReloadItem.FlatAppearance.BorderSize = 0;
            this.btnReloadItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReloadItem.ForeColor = System.Drawing.Color.Black;
            this.btnReloadItem.Image = ((System.Drawing.Image)(resources.GetObject("btnReloadItem.Image")));
            this.btnReloadItem.Location = new System.Drawing.Point(539, 16);
            this.btnReloadItem.Name = "btnReloadItem";
            this.btnReloadItem.Size = new System.Drawing.Size(35, 32);
            this.btnReloadItem.TabIndex = 126;
            this.btnReloadItem.UseVisualStyleBackColor = false;
            this.btnReloadItem.Click += new System.EventHandler(this.TryReload);
            // 
            // btnPreviousItem
            // 
            this.btnPreviousItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPreviousItem.BackColor = System.Drawing.Color.LightGreen;
            this.btnPreviousItem.FlatAppearance.BorderSize = 0;
            this.btnPreviousItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPreviousItem.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnPreviousItem.ForeColor = System.Drawing.Color.Black;
            this.btnPreviousItem.Location = new System.Drawing.Point(456, 16);
            this.btnPreviousItem.Name = "btnPreviousItem";
            this.btnPreviousItem.Size = new System.Drawing.Size(74, 32);
            this.btnPreviousItem.TabIndex = 124;
            this.btnPreviousItem.Text = "Previous";
            this.btnPreviousItem.UseVisualStyleBackColor = false;
            this.btnPreviousItem.Click += new System.EventHandler(this.TrySelectPrevious);
            // 
            // btnLoadStockpile
            // 
            this.btnLoadStockpile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadStockpile.BackColor = System.Drawing.Color.Orange;
            this.btnLoadStockpile.FlatAppearance.BorderSize = 0;
            this.btnLoadStockpile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadStockpile.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnLoadStockpile.ForeColor = System.Drawing.Color.Black;
            this.btnLoadStockpile.Location = new System.Drawing.Point(369, 16);
            this.btnLoadStockpile.Name = "btnLoadStockpile";
            this.btnLoadStockpile.Size = new System.Drawing.Size(78, 32);
            this.btnLoadStockpile.TabIndex = 123;
            this.btnLoadStockpile.Text = "Load";
            this.btnLoadStockpile.UseVisualStyleBackColor = false;
            this.btnLoadStockpile.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TryLoadStockpile);
            // 
            // btnBlastToggle
            // 
            this.btnBlastToggle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBlastToggle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnBlastToggle.FlatAppearance.BorderSize = 0;
            this.btnBlastToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBlastToggle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnBlastToggle.ForeColor = System.Drawing.Color.White;
            this.btnBlastToggle.Location = new System.Drawing.Point(14, 469);
            this.btnBlastToggle.Name = "btnBlastToggle";
            this.btnBlastToggle.Size = new System.Drawing.Size(627, 32);
            this.btnBlastToggle.TabIndex = 131;
            this.btnBlastToggle.TabStop = false;
            this.btnBlastToggle.Tag = "color:dark2";
            this.btnBlastToggle.Text = "BlastLayer : OFF    (Attempts to uncorrupt/recorrupt in real-time)";
            this.btnBlastToggle.UseVisualStyleBackColor = false;
            this.btnBlastToggle.Click += new System.EventHandler(this.BlastLayerToggle);
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
            this.dgvStockpile.ColumnHeadersHeight = 21;
            this.dgvStockpile.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvStockpile.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Item,
            this.GameName,
            this.SystemName,
            this.SystemCore,
            this.Note});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI Symbol", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvStockpile.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvStockpile.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvStockpile.GridColor = System.Drawing.Color.Black;
            this.dgvStockpile.Location = new System.Drawing.Point(14, 64);
            this.dgvStockpile.MultiSelect = false;
            this.dgvStockpile.Name = "dgvStockpile";
            this.dgvStockpile.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.dgvStockpile.RowHeadersVisible = false;
            this.dgvStockpile.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI Symbol", 9F);
            this.dgvStockpile.RowTemplate.Height = 27;
            this.dgvStockpile.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvStockpile.Size = new System.Drawing.Size(627, 319);
            this.dgvStockpile.TabIndex = 142;
            this.dgvStockpile.Tag = "color:normal";
            this.dgvStockpile.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnStockpileCellClick);
            this.dgvStockpile.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnStockpileMouseDown);
            // 
            // tbNoteBox
            // 
            this.tbNoteBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbNoteBox.BackColor = System.Drawing.Color.Gray;
            this.tbNoteBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbNoteBox.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbNoteBox.ForeColor = System.Drawing.Color.White;
            this.tbNoteBox.Location = new System.Drawing.Point(14, 389);
            this.tbNoteBox.Name = "tbNoteBox";
            this.tbNoteBox.ReadOnly = true;
            this.tbNoteBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.tbNoteBox.Size = new System.Drawing.Size(627, 74);
            this.tbNoteBox.TabIndex = 143;
            this.tbNoteBox.Tag = "color:normal";
            this.tbNoteBox.Text = "Notes will appear here...";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Item Name";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 238;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.FillWeight = 45F;
            this.dataGridViewTextBoxColumn2.HeaderText = "Game";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 113;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.FillWeight = 45F;
            this.dataGridViewTextBoxColumn3.HeaderText = "System";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 112;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.FillWeight = 40F;
            this.dataGridViewTextBoxColumn4.HeaderText = "Core";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Width = 101;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 22F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(13, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(234, 41);
            this.label2.TabIndex = 83;
            this.label2.Text = "Stockpile Player";
            // 
            // Item
            // 
            this.Item.HeaderText = "Item Name";
            this.Item.Name = "Item";
            this.Item.ReadOnly = true;
            // 
            // GameName
            // 
            this.GameName.FillWeight = 45F;
            this.GameName.HeaderText = "Game";
            this.GameName.Name = "GameName";
            this.GameName.ReadOnly = true;
            // 
            // SystemName
            // 
            this.SystemName.FillWeight = 45F;
            this.SystemName.HeaderText = "System";
            this.SystemName.Name = "SystemName";
            this.SystemName.ReadOnly = true;
            // 
            // SystemCore
            // 
            this.SystemCore.FillWeight = 40F;
            this.SystemCore.HeaderText = "Core";
            this.SystemCore.Name = "SystemCore";
            // 
            // Note
            // 
            this.Note.FillWeight = 40F;
            this.Note.HeaderText = "Note";
            this.Note.MinimumWidth = 35;
            this.Note.Name = "Note";
            this.Note.ReadOnly = true;
            this.Note.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // StockpilePlayerForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(655, 515);
            this.Controls.Add(this.tbNoteBox);
            this.Controls.Add(this.btnBlastToggle);
            this.Controls.Add(this.btnNextItem);
            this.Controls.Add(this.dgvStockpile);
            this.Controls.Add(this.btnReloadItem);
            this.Controls.Add(this.btnPreviousItem);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnLoadStockpile);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(655, 515);
            this.Name = "StockpilePlayerForm";
            this.Tag = "color:dark1";
            this.Text = "Stockpile Player";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStockpile)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnLoadStockpile;
		private System.Windows.Forms.Button btnNextItem;
		private System.Windows.Forms.Button btnReloadItem;
		private System.Windows.Forms.Button btnPreviousItem;
		public System.Windows.Forms.Button btnBlastToggle;
		public DataGridView dgvStockpile;
		private System.Windows.Forms.RichTextBox tbNoteBox;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.Label label2;
        private DataGridViewTextBoxColumn Item;
        private DataGridViewTextBoxColumn GameName;
        private DataGridViewTextBoxColumn SystemName;
        private DataGridViewTextBoxColumn SystemCore;
        private DataGridViewTextBoxColumn Note;
    }
}
