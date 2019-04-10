using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using RTCV.CorruptCore;
using static RTCV.UI.UI_Extensions;
using RTCV.NetCore.StaticTools;
using RTCV.NetCore;

namespace RTCV.UI
{
	public partial class RTC_StockpilePlayer_Form : Form, IAutoColorize
	{
		public bool DontLoadSelectedStockpile = false;
		private bool currentlyLoading = false;

		public RTC_StockpilePlayer_Form()
		{
			InitializeComponent();
			dgvStockpile.DragDrop += dgvStockpile_DragDrop;
			dgvStockpile.DragEnter += dgvStockpile_DragEnter;
			dgvStockpile.RowsAdded += (o, e) =>
			{
				RefreshNoteIcons();
			};
		}

		private void dgvStockpile_DragEnter(object sender, DragEventArgs e)
		{
			e.Effect = DragDropEffects.Link;
		}

		private void dgvStockpile_DragDrop(object sender, DragEventArgs e)
		{
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
			if (files.Length > 0 && files[0]
				.Contains(".sks"))
			{
				Stockpile.Load(dgvStockpile, files[0]);
			}

		}

		private void RTC_BE_Form_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason != CloseReason.FormOwnerClosing)
			{
				e.Cancel = true;
				this.Hide();
			}
		}

		private void btnPreviousItem_Click(object sender, EventArgs e)
		{
			try
			{
				btnPreviousItem.Visible = false;

				if (dgvStockpile.SelectedRows.Count == 0)
					return;

				int CurrentSelectedIndex = dgvStockpile.SelectedRows[0].Index;

				if (CurrentSelectedIndex == 0)
				{
					dgvStockpile.ClearSelection();
					dgvStockpile.Rows[dgvStockpile.Rows.Count - 1].Selected = true;
				}
				else
				{
					dgvStockpile.ClearSelection();
					dgvStockpile.Rows[CurrentSelectedIndex - 1].Selected = true;
				}

				dgvStockpile_CellClick(null, null);
			}
			finally
			{
				btnPreviousItem.Visible = true;
			}
		}

		private void btnNextItem_Click(object sender, EventArgs e)
		{
			try
			{
				btnNextItem.Visible = false;

				if (dgvStockpile.SelectedRows.Count == 0)
					return;

				int CurrentSelectedIndex = dgvStockpile.SelectedRows[0].Index;

				if (CurrentSelectedIndex == dgvStockpile.Rows.Count - 1)
				{
					dgvStockpile.ClearSelection();
					dgvStockpile.Rows[0].Selected = true;
				}
				else
				{
					dgvStockpile.ClearSelection();
					dgvStockpile.Rows[CurrentSelectedIndex + 1].Selected = true;
				}

				dgvStockpile_CellClick(null, null);
			}
			finally
			{
				btnNextItem.Visible = true;
			}
		}

		private void btnReloadItem_Click(object sender, EventArgs e)
		{
			try
			{
				btnReloadItem.Visible = false;
				dgvStockpile_CellClick(null, null);
			}
			finally
			{
				btnReloadItem.Visible = true;
			}
		}

		private void btnBlastToggle_Click(object sender, EventArgs e)
		{
			S.GET<RTC_GlitchHarvester_Form>().btnBlastToggle_Click(null, null);
		}

		private void dgvStockpile_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				Point locate = new Point((sender as Control).Location.X + e.Location.X, (sender as Control).Location.Y + e.Location.Y);

				ToolStripSeparator stripSeparator = new ToolStripSeparator();
				stripSeparator.Paint += stripSeparator_Paint;

				ContextMenuStrip columnsMenu = new ContextMenuStrip();
				(columnsMenu.Items.Add("Show Item Name", null, new EventHandler((ob, ev) => { dgvStockpile.Columns["Item"].Visible ^= true; })) as ToolStripMenuItem).Checked = dgvStockpile.Columns["Item"].Visible;
				(columnsMenu.Items.Add("Show Game Name", null, new EventHandler((ob, ev) => { dgvStockpile.Columns["GameName"].Visible ^= true; })) as ToolStripMenuItem).Checked = dgvStockpile.Columns["GameName"].Visible;
				(columnsMenu.Items.Add("Show System Name", null, new EventHandler((ob, ev) => { dgvStockpile.Columns["SystemName"].Visible ^= true; })) as ToolStripMenuItem).Checked = dgvStockpile.Columns["SystemName"].Visible;
				(columnsMenu.Items.Add("Show System Core", null, new EventHandler((ob, ev) => { dgvStockpile.Columns["SystemCore"].Visible ^= true; })) as ToolStripMenuItem).Checked = dgvStockpile.Columns["SystemCore"].Visible;
				(columnsMenu.Items.Add("Show Note", null, new EventHandler((ob, ev) => { dgvStockpile.Columns["Note"].Visible ^= true; })) as ToolStripMenuItem).Checked = dgvStockpile.Columns["Note"].Visible;
				columnsMenu.Items.Add(stripSeparator);
				(columnsMenu.Items.Add("Load on Select", null, new EventHandler((ob, ev) => { S.GET<RTC_GlitchHarvester_Form>().cbLoadOnSelect.Checked ^= true; })) as ToolStripMenuItem).Checked = S.GET<RTC_GlitchHarvester_Form>().cbLoadOnSelect.Checked;
				(columnsMenu.Items.Add("Clear Cheats/Freezes on Rewind", null, new EventHandler((ob, ev) => { S.GET<RTC_CorruptionEngine_Form>().cbClearCheatsOnRewind.Checked ^= true; })) as ToolStripMenuItem).Checked = S.GET<RTC_CorruptionEngine_Form>().cbClearCheatsOnRewind.Checked;
				(columnsMenu.Items.Add("Clear Pipes on Rewind", null, new EventHandler((ob, ev) => { S.GET<RTC_CorruptionEngine_Form>().cbClearPipesOnRewind.Checked ^= true; })) as ToolStripMenuItem).Checked = S.GET<RTC_CorruptionEngine_Form>().cbClearPipesOnRewind.Checked;

				columnsMenu.Show(this, locate);
			}
		}

		private void stripSeparator_Paint(object sender, PaintEventArgs e)
		{
			ToolStripSeparator stripSeparator = sender as ToolStripSeparator;
			ContextMenuStrip menuStrip = stripSeparator.Owner as ContextMenuStrip;
			e.Graphics.FillRectangle(new SolidBrush(Color.Transparent), new Rectangle(0, 0, stripSeparator.Width, stripSeparator.Height));
			using (Pen pen = new Pen(Color.LightGray, 1))
			{
				e.Graphics.DrawLine(pen, new Point(23, stripSeparator.Height / 2), new Point(menuStrip.Width, stripSeparator.Height / 2));
			}
		}

		private void btnLoadStockpile_MouseDown(object sender, MouseEventArgs e)
		{
			Point locate = new Point((sender as Control).Location.X + e.Location.X, (sender as Control).Location.Y + e.Location.Y);

			ContextMenuStrip LoadMenuItems = new ContextMenuStrip();
			LoadMenuItems.Items.Add("Load Stockpile", null, new EventHandler((ob, ev) =>
			{
				try
				{
					DontLoadSelectedStockpile = true;
					LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_CLOSEGAME, true);

					if (Stockpile.Load(dgvStockpile))
					{
						S.GET<RTC_GlitchHarvester_Form>().dgvStockpile.Rows.Clear();
					}
					dgvStockpile.ClearSelection();
					RefreshNoteIcons();
				}
				catch (Exception ex)
				{
					string additionalInfo = "Loading Failure ->\n\n";

					var ex2 = new CustomException(ex.Message, additionalInfo + ex.StackTrace);

					if (CloudDebug.ShowErrorDialog(ex2, true) == DialogResult.Abort)
						throw new RTCV.NetCore.AbortEverythingException();

				}
			}));

			LoadMenuItems.Items.Add("Load Bizhawk settings from Stockpile", null, new EventHandler((ob, ev) =>
			{
				try
				{
					Stockpile.LoadBizhawkConfigFromStockpile();
				}
				catch (Exception ex)
				{
					string additionalInfo = "Loading Settings Failure ->\n\n";

					var ex2 = new CustomException(ex.Message, additionalInfo + ex.StackTrace);

					if (CloudDebug.ShowErrorDialog(ex2, true) == DialogResult.Abort)
						throw new RTCV.NetCore.AbortEverythingException();
				}
			}));

			LoadMenuItems.Items.Add("Restore Bizhawk config Backup", null, new EventHandler((ob, ev) =>
			{
				try
				{
					Stockpile.RestoreBizhawkConfig();
				}
				finally
				{
				}
			})).Enabled = (File.Exists(CorruptCore.CorruptCore.bizhawkDir + Path.DirectorySeparatorChar + "backup_config.ini"));

			LoadMenuItems.Show(this, locate);
		}

		private void dgvStockpile_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (currentlyLoading || e?.RowIndex == -1)
				return;
   			try
			{
				//dgvStockpile.Enabled = false;
				currentlyLoading = true;

				if (e != null)
				{
					var senderGrid = (DataGridView)sender;

					StashKey sk = (StashKey)senderGrid.Rows[e.RowIndex].Cells["Item"].Value;

					if (sk.Note != null)
						tbNoteBox.Text = sk.Note.Replace("\n", Environment.NewLine);
					else
						tbNoteBox.Text = "";

					if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
						e.RowIndex >= 0)
					{
						S.SET(new RTC_NoteEditor_Form(sk, senderGrid.Rows[e.RowIndex].Cells["Note"]));
						S.GET<RTC_NoteEditor_Form>().Show();
						return;
					}
				}

				if (dgvStockpile.SelectedRows.Count > 0)
				{
					//Shut autocorrupt off because people (Vinny) kept turning it on to add to corruptions then forgetting to turn it off
					S.GET<RTC_Core_Form>().AutoCorrupt = false; 

					S.GET<RTC_GlitchHarvester_Form>().rbCorrupt.Checked = true;
					StockpileManager_UISide.CurrentStashkey = (dgvStockpile.SelectedRows[0].Cells[0].Value as StashKey);
					StockpileManager_UISide.ApplyStashkey(StockpileManager_UISide.CurrentStashkey);

					S.GET<RTC_GlitchHarvester_Form>().lbStashHistory.ClearSelected();
					S.GET<RTC_GlitchHarvester_Form>().dgvStockpile.ClearSelection();

					S.GET<RTC_GlitchHarvester_Form>().IsCorruptionApplied = !(StockpileManager_UISide.CurrentStashkey.BlastLayer == null || StockpileManager_UISide.CurrentStashkey.BlastLayer.Layer.Count == 0);
				}
			}
			finally
			{
				currentlyLoading = false;
				//dgvStockpile.Enabled = true;
			}
		}

		public void RefreshNoteIcons()
		{
			foreach (DataGridViewRow dataRow in dgvStockpile.Rows)
			{
				StashKey sk = (StashKey)dataRow.Cells["Item"].Value;
				if (sk == null)
					continue;
				if (String.IsNullOrWhiteSpace(sk.Note))
				{
					dataRow.Cells["Note"].Value = String.Empty;
				}
				else
				{
					dataRow.Cells["Note"].Value = "📝";
				}
			}
		}
	}
}
