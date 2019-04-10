using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using Newtonsoft.Json;
using RTCV.CorruptCore;
using RTCV.NetCore;
using static RTCV.UI.UI_Extensions;
using RTCV.NetCore.StaticTools;
using System.Linq;

namespace RTCV.UI
{
	public partial class RTC_GlitchHarvester_Form : Form, IAutoColorize
	{
		public string[] btnParentKeys = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null };
		public string[] btnAttachedRom = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null };

		public bool DontLoadSelectedStash = false;
		public bool DontLoadSelectedStockpile = false;

		private bool loadBeforeOperation = true;

		public Panel pnHideGlitchHarvester = new Panel();
		public Label lbConnectionStatus = new Label();
		public Button btnEmergencySaveStockpile = new Button();

		public bool UnsavedEdits { get; set; }

		Dictionary<string, TextBox> StateBoxes = new Dictionary<string, TextBox>();

		private bool isCorruptionApplied;
		public bool IsCorruptionApplied
		{
			get
			{
				return isCorruptionApplied;
			}
			set
			{
				if (value)
				{
					btnBlastToggle.BackColor = Color.FromArgb(224, 128, 128);
					btnBlastToggle.ForeColor = Color.Black;
					btnBlastToggle.Text = "BlastLayer : ON";

					S.GET<RTC_StockpilePlayer_Form>().btnBlastToggle.BackColor = Color.FromArgb(224, 128, 128);
					S.GET<RTC_StockpilePlayer_Form>().btnBlastToggle.ForeColor = Color.Black;
					S.GET<RTC_StockpilePlayer_Form>().btnBlastToggle.Text = "BlastLayer : ON     (Attempts to uncorrupt/recorrupt in real-time)";
				}
				else
				{
					btnBlastToggle.BackColor = S.GET<RTC_Core_Form>().btnLogo.BackColor;
					btnBlastToggle.ForeColor = Color.White;
					btnBlastToggle.Text = "BlastLayer : OFF";

					S.GET<RTC_StockpilePlayer_Form>().btnBlastToggle.BackColor = S.GET<RTC_Core_Form>().btnLogo.BackColor;
					S.GET<RTC_StockpilePlayer_Form>().btnBlastToggle.ForeColor = Color.White;
					S.GET<RTC_StockpilePlayer_Form>().btnBlastToggle.Text = "BlastLayer : OFF    (Attempts to uncorrupt/recorrupt in real-time)";
				}

				isCorruptionApplied = value;
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
					dataRow.Cells["Note"].Value = "";
				}
				else
				{
					dataRow.Cells["Note"].Value = "📝";
				}
			}
		}

		public RTC_GlitchHarvester_Form()
		{
			InitializeComponent();
			multiTB_Intensity.ValueChanged += (sender, args) => CorruptCore.CorruptCore.Intensity = multiTB_Intensity.Value;

			//btnSavestate_Click(btnSavestate01, null); //Selects first button as default

			#region textbox states to dico

			StateBoxes.Add("01", tbSavestate01);
			StateBoxes.Add("02", tbSavestate02);
			StateBoxes.Add("03", tbSavestate03);
			StateBoxes.Add("04", tbSavestate04);
			StateBoxes.Add("05", tbSavestate05);
			StateBoxes.Add("06", tbSavestate06);
			StateBoxes.Add("07", tbSavestate07);
			StateBoxes.Add("08", tbSavestate08);
			StateBoxes.Add("09", tbSavestate09);
			StateBoxes.Add("10", tbSavestate10);
			StateBoxes.Add("11", tbSavestate11);
			StateBoxes.Add("12", tbSavestate12);
			StateBoxes.Add("13", tbSavestate13);
			StateBoxes.Add("14", tbSavestate14);
			StateBoxes.Add("15", tbSavestate15);
			StateBoxes.Add("16", tbSavestate16);
			StateBoxes.Add("17", tbSavestate17);
			StateBoxes.Add("18", tbSavestate18);
			StateBoxes.Add("19", tbSavestate19);
			StateBoxes.Add("20", tbSavestate20);
			StateBoxes.Add("21", tbSavestate21);
			StateBoxes.Add("22", tbSavestate22);
			StateBoxes.Add("23", tbSavestate23);
			StateBoxes.Add("24", tbSavestate24);
			StateBoxes.Add("25", tbSavestate25);
			StateBoxes.Add("26", tbSavestate26);
			StateBoxes.Add("27", tbSavestate27);
			StateBoxes.Add("28", tbSavestate28);
			StateBoxes.Add("29", tbSavestate29);
			StateBoxes.Add("30", tbSavestate30);
			StateBoxes.Add("31", tbSavestate31);
			StateBoxes.Add("32", tbSavestate32);
			StateBoxes.Add("33", tbSavestate33);
			StateBoxes.Add("34", tbSavestate34);
			StateBoxes.Add("35", tbSavestate35);
			StateBoxes.Add("36", tbSavestate36);
			StateBoxes.Add("37", tbSavestate37);
			StateBoxes.Add("38", tbSavestate38);
			StateBoxes.Add("39", tbSavestate39);
			StateBoxes.Add("40", tbSavestate40);

			#endregion textbox states to dico

			pnHideGlitchHarvester.Location = new Point(0, 0);
			pnHideGlitchHarvester.Size = this.Size;
			pnHideGlitchHarvester.BackColor = Color.Black;
			pnHideGlitchHarvester.Controls.Add(lbConnectionStatus);

			lbConnectionStatus.Location = new Point(32, 32);
			lbConnectionStatus.Size = new Size(500, 32);
			lbConnectionStatus.ForeColor = Color.FromArgb(255, 192, 128);

			btnEmergencySaveStockpile.Text = "Emergency Save-As Stockpile";
			btnEmergencySaveStockpile.Location = new Point(32, 64);
			btnEmergencySaveStockpile.Size = new Size(200, 32);
			btnEmergencySaveStockpile.BackColor = Color.OrangeRed;
			btnEmergencySaveStockpile.Click += btnSaveStockpileAs_Click;
			pnHideGlitchHarvester.Controls.Add(btnEmergencySaveStockpile);

			dgvStockpile.RowsAdded += (o,e) => {
				RefreshNoteIcons();
			};

			this.MouseDoubleClick += ClearSelectedSKs;

			Controls.Add(pnHideGlitchHarvester);
			pnHideGlitchHarvester.Hide();

			cbRenderType.SelectedIndex = 0;
			
			lbStashHistory.DataSource = StockpileManager_UISide.StashHistory;
		}


		private void RTC_GH_Form_Load(object sender, EventArgs e)
		{
			/*
			foreach (Control ctrl in pnSavestateHolder.Controls)
				if (ctrl is Button)
					ctrl.Size = new Size(29, 25);
            */

			RefreshStashHistory();
			RefreshSavestateTextboxes();

			dgvStockpile.AllowDrop = true;
			dgvStockpile.DragDrop += dgvStockpile_DragDrop;
			dgvStockpile.DragEnter += dgvStockpile_DragEnter;

			pnSavestateHolder.AllowDrop = true;
			pnSavestateHolder.DragDrop += pnSavestateHolder_DragDrop;
			pnSavestateHolder.DragEnter += pnSavestateHolder_DragEnter;
		}

		public void RefreshStashHistorySelectLast()
		{
			RefreshStashHistory();
			DontLoadSelectedStash = true;
			lbStashHistory.ClearSelected();
			DontLoadSelectedStash = true;
			lbStashHistory.SelectedIndex = lbStashHistory.Items.Count - 1;
		}

		public void RefreshStashHistory()
		{
			DontLoadSelectedStash = true;
			var lastSelect = lbStashHistory.SelectedIndex;

			DontLoadSelectedStash = true;
			lbStashHistory.DataSource = null;

			DontLoadSelectedStash = true;
			//lbStashHistory.BeginUpdate();
			lbStashHistory.DataSource = StockpileManager_UISide.StashHistory;
			//lbStashHistory.EndUpdate();

			DontLoadSelectedStash = true;
			if (lastSelect < lbStashHistory.Items.Count)
				lbStashHistory.SelectedIndex = lastSelect;

			DontLoadSelectedStash = false;
		}

		public void btnSavestate_Click(object sender, EventArgs e)
		{
			try
			{
				((Button)sender).Visible = false;

				foreach (var item in pnSavestateHolder.Controls)
					if (item is Button button)
						button.ForeColor = Color.FromArgb(192, 255, 192);

				Button clickedButton = ((Button)sender);
				clickedButton.ForeColor = Color.OrangeRed;
				clickedButton.BringToFront();

				StockpileManager_UISide.CurrentSavestateKey = clickedButton.Text;
				StashKey psk = StockpileManager_UISide.GetCurrentSavestateStashkey();

				if (psk != null && !File.Exists(psk.RomFilename))
				{
					if (DialogResult.Yes == MessageBox.Show($"Can't find file {psk.RomFilename}\nGame name: {psk.GameName}\nSystem name: {psk.SystemName}\n\n Would you like to provide a new file for replacement?", "Error: File not found", MessageBoxButtons.YesNo))
					{
						OpenFileDialog ofd = new OpenFileDialog
						{
							DefaultExt = "*",
							Title = "Select Replacement File",
							Filter = "Any file|*.*",
							RestoreDirectory = true
						};
						if (ofd.ShowDialog() == DialogResult.OK)
						{
							string filename = ofd.FileName.ToString();
							string oldFilename = psk.RomFilename;
							for (int i = 1; i < 41; i++)
							{
								string key = i.ToString().PadLeft(2, '0');

								if (StockpileManager_UISide.SavestateStashkeyDico.ContainsKey(key))
								{
									StashKey sk = StockpileManager_UISide.SavestateStashkeyDico[key];
									if (sk.RomFilename == oldFilename)
										sk.RomFilename = filename;
								}
							}
						}
						else
						{
							clickedButton.ForeColor = Color.FromArgb(192, 255, 192);
							StockpileManager_UISide.CurrentSavestateKey = null;
							return;
						}
					}
					else
					{
						clickedButton.ForeColor = Color.FromArgb(192, 255, 192);
						StockpileManager_UISide.CurrentSavestateKey = null;
						return;
					}
				}


				if (cbSavestateLoadOnClick.Checked)
				{
					btnSaveLoad.Text = "LOAD";
					btnSaveLoad_Click(null, null);
				}
				//StockpileManager_UISide.LoadState(StockpileManager_UISide.getCurrentSavestateStashkey());
			}
			finally
			{
				((Button)sender).Visible = true;
			}
		}

		private void btnToggleSaveLoad_Click(object sender, EventArgs e)
		{
			if (btnSaveLoad.Text == "LOAD")
			{
				btnSaveLoad.Text = "SAVE";
				btnSaveLoad.ForeColor = Color.OrangeRed;
			}
			else
			{
				btnSaveLoad.Text = "LOAD";
				btnSaveLoad.ForeColor = Color.FromArgb(192, 255, 192);
			}
		}

		public void btnSaveLoad_Click(object sender, EventArgs e)
		{
			if (btnSaveLoad.Text == "LOAD")
			{
				StashKey psk = StockpileManager_UISide.GetCurrentSavestateStashkey();
				if (psk != null)
				{
					if (!File.Exists(psk.RomFilename))
						if (DialogResult.Yes == MessageBox.Show($"Can't find file {psk.RomFilename}\nGame name: {psk.GameName}\nSystem name: {psk.SystemName}\n\n Would you like to provide a new file for replacement?", "Error: File not found", MessageBoxButtons.YesNo))
						{
							OpenFileDialog ofd = new OpenFileDialog
							{
								DefaultExt = "*",
								Title = "Select Replacement File",
								Filter = "Any file|*.*",
								RestoreDirectory = true
							};
							if (ofd.ShowDialog() == DialogResult.OK)
							{
								string filename = ofd.FileName.ToString();
								string oldFilename = psk.RomFilename;
								for (int i = 1; i < 41; i++)
								{
									string key = i.ToString().PadLeft(2, '0');

									if (StockpileManager_UISide.SavestateStashkeyDico.ContainsKey(key))
									{
										StashKey sk = StockpileManager_UISide.SavestateStashkeyDico[key];
										if (sk.RomFilename == oldFilename)
											sk.RomFilename = filename;
									}
								}
							}
							else
								return;
						}

					StockpileManager_UISide.LoadState(psk);
				}
				else
					MessageBox.Show("Savestate box is empty");
			}
			else
			{
				if (StockpileManager_UISide.CurrentSavestateKey == null)
				{
					MessageBox.Show("No Savestate Box is currently selected in the Glitch Harvester's Savestate Manager");
					return;
				}

				StashKey sk = StockpileManager_UISide.SaveState(true);

				btnSaveLoad.Text = "LOAD";
				btnSaveLoad.ForeColor = Color.FromArgb(192, 255, 192);

				RefreshSavestateTextboxes();
			}
		}

		public void btnCorrupt_Click(object sender, EventArgs e)
		{
			Console.WriteLine("btnCorrupt Clicked");
			if (!btnCorrupt.Visible)
				return;

			try
			{
				//Shut off autocorrupt if it's on.
				//Leave this check here so we don't wastefully update the spec
				if(S.GET<RTC_Core_Form>().AutoCorrupt)
					S.GET<RTC_Core_Form>().AutoCorrupt = false;

				btnCorrupt.Visible = false;

				StashKey psk = StockpileManager_UISide.GetCurrentSavestateStashkey();

				if (btnCorrupt.Text.ToUpper() == "MERGE")
				{
					List<StashKey> sks = new List<StashKey>();

					//Reverse before merging because DataGridView selectedrows is backwards for some odd reason
					var reversed = dgvStockpile.SelectedRows.Cast<DataGridViewRow>().Reverse();
					foreach (DataGridViewRow row in reversed)
						sks.Add((StashKey)row.Cells[0].Value);

					IsCorruptionApplied = StockpileManager_UISide.MergeStashkeys(sks);

					RefreshStashHistorySelectLast();
					//lbStashHistory.TopIndex = lbStashHistory.Items.Count - 1;

					return;
				}


				if (rbCorrupt.Checked)
				{
					string romFilename = (string)RTCV.NetCore.AllSpec.VanguardSpec[VSPEC.OPENROMFILENAME];
					if (romFilename == null)
						return;
					if (romFilename.Contains("|"))
					{
						MessageBox.Show($"The Glitch Harvester attempted to corrupt a game bound to the following file:\n{romFilename}\n\nIt cannot be processed because the rom seems to be inside a Zip Archive\n(Bizhawk returned a filename with the chracter | in it)");
						return;
					}

					DontLoadSelectedStash = true;
					IsCorruptionApplied = StockpileManager_UISide.Corrupt(loadBeforeOperation);
					RefreshStashHistorySelectLast();
				}
				else if (rbInject.Checked)
				{
					if (StockpileManager_UISide.CurrentStashkey == null)
						throw new CustomException("CurrentStashkey in inject was somehow null! Report this to the devs and tell them how you caused this.", Environment.StackTrace);

					DontLoadSelectedStash = true;

					IsCorruptionApplied = StockpileManager_UISide.InjectFromStashkey(StockpileManager_UISide.CurrentStashkey, loadBeforeOperation);
					RefreshStashHistorySelectLast();
				}
				else if (rbOriginal.Checked)
				{
					if (StockpileManager_UISide.CurrentStashkey == null)
						throw new CustomException("CurrentStashkey in original was somehow null! Report this to the devs and tell them how you caused this.", Environment.StackTrace);

					DontLoadSelectedStash = true;
					IsCorruptionApplied = StockpileManager_UISide.OriginalFromStashkey(StockpileManager_UISide.CurrentStashkey);
				}

				if (StockpileManager_EmuSide.RenderAtLoad && loadBeforeOperation)
				{
					btnRender.Text = "Stop Render";
					btnRender.ForeColor = Color.OrangeRed;
				}
				else
				{
					btnRender.Text = "Start Render";
					btnRender.ForeColor = Color.White;
				}

				Console.WriteLine("Blast done");
			}
			finally
			{
				btnCorrupt.Visible = true;
			 }
		}

		private void RTC_GH_Form_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason != CloseReason.FormOwnerClosing)
			{
				S.GET<RTC_Core_Form>().btnGlitchHarvester.Text = S.GET<RTC_Core_Form>().btnGlitchHarvester.Text.Replace("○ ", "");
				e.Cancel = true;
				this.Hide();
			}
		}

		private void lbStashHistory_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				lbStashHistory.Enabled = false;
				btnStashUP.Enabled = false;
				btnStashDOWN.Enabled = false;
				btnAddStashToStockpile.Enabled = false;


				if (DontLoadSelectedStash || lbStashHistory.SelectedIndex == -1)
				{
					DontLoadSelectedStash = false;
					return;
				}

				dgvStockpile.ClearSelection();
				S.GET<RTC_StockpilePlayer_Form>().dgvStockpile.ClearSelection();

				if (!rbCorrupt.Checked && !rbInject.Checked && !rbOriginal.Checked)
					rbCorrupt.Checked = true;

				if (btnCorrupt.Text == "Merge")
				{
					rbCorrupt.Enabled = true;
					rbInject.Enabled = true;
					rbOriginal.Enabled = true;
					btnRenameSelected.Visible = true;
					btnRemoveSelectedStockpile.Text = "Remove Item";

					if (rbCorrupt.Checked)
						btnCorrupt.Text = "Blast/Send";
					else if (rbInject.Checked)
						btnCorrupt.Text = "Inject";
					else if (rbOriginal.Checked)
						btnCorrupt.Text = "Original";
				}

				StockpileManager_UISide.CurrentStashkey = StockpileManager_UISide.StashHistory[lbStashHistory.SelectedIndex];

				if (!cbLoadOnSelect.Checked)
					return;

				OneTimeExecute();
			}
			finally
			{
				lbStashHistory.Enabled = true;
				btnStashUP.Enabled = true;
				btnStashDOWN.Enabled = true;
				btnAddStashToStockpile.Enabled = true;
				((Control) sender).Focus();
			}
		}

		private void OneTimeExecute()
		{
			//Disable autocorrupt
			S.GET<RTC_Core_Form>().AutoCorrupt = false;

			if (rbCorrupt.Checked)
				IsCorruptionApplied = StockpileManager_UISide.ApplyStashkey(StockpileManager_UISide.CurrentStashkey, loadBeforeOperation);
			else if (rbInject.Checked)
			{
				IsCorruptionApplied = StockpileManager_UISide.InjectFromStashkey(StockpileManager_UISide.CurrentStashkey, loadBeforeOperation);
				RefreshStashHistory();
			}
			else if (rbOriginal.Checked)
				IsCorruptionApplied = StockpileManager_UISide.OriginalFromStashkey(StockpileManager_UISide.CurrentStashkey);

			if (StockpileManager_EmuSide.RenderAtLoad && loadBeforeOperation)
			{
				btnRender.Text = "Stop Render";
				btnRender.ForeColor = Color.OrangeRed;
			}
			else
			{
				btnRender.Text = "Start Render";
				btnRender.ForeColor = Color.White;
			}
		}

		private void rbInject_CheckedChanged(object sender, EventArgs e)
		{
			if (rbInject.Checked)
				btnCorrupt.Text = "Inject";
		}

		private void rbCorrupt_CheckedChanged(object sender, EventArgs e)
		{
			if (rbCorrupt.Checked)
				btnCorrupt.Text = "Blast/Send";
		}

		private void rbOriginal_CheckedChanged(object sender, EventArgs e)
		{
			if (rbOriginal.Checked)
				btnCorrupt.Text = "Original";
		}

		private void ClearSelectedSKs(object sender, MouseEventArgs e)
		{
			DontLoadSelectedStash = true;
			lbStashHistory.ClearSelected();
			DontLoadSelectedStash = true;
			dgvStockpile.ClearSelection();

			if (rbCorrupt.Checked)
				btnCorrupt.Text = "Blast/Send";
			else if (rbInject.Checked)
				btnCorrupt.Text = "Inject";
			else if (rbOriginal.Checked) btnCorrupt.Text = "Original";
		}

		private void btnAddStashToStockpile_Click(object sender, EventArgs e)
		{
			if (StockpileManager_UISide.CurrentStashkey != null && StockpileManager_UISide.CurrentStashkey.Alias != StockpileManager_UISide.CurrentStashkey.Key)
				AddStashToStockpile(false);
			else
				AddStashToStockpile(true);
		}

		public void AddStashToStockpile(bool askForName = true)
		{
			if (lbStashHistory.Items.Count == 0 || lbStashHistory.SelectedIndex == -1)
			{
				MessageBox.Show("Can't add the Stash to the Stockpile because none is selected in the Stash History");
				return;
			}

			string Name = "";
			string value = "";

			if (askForName)
			{
				if (GetInputBox("Glitch Harvester", "Enter the new Stash name:", ref value) == DialogResult.OK)
				{
					Name = value.Trim();
				}
				else
				{
					return;
				}
			}
			else
				Name = StockpileManager_UISide.CurrentStashkey.Alias;

			StockpileManager_UISide.CurrentStashkey = (StashKey)lbStashHistory.SelectedItem;

			if (String.IsNullOrWhiteSpace(Name))
				StockpileManager_UISide.CurrentStashkey.Alias = StockpileManager_UISide.CurrentStashkey.Key;
			else
				StockpileManager_UISide.CurrentStashkey.Alias = Name;


			StashKey sk = StockpileManager_UISide.CurrentStashkey;

			sk.BlastLayer.RasterizeVMDs();

			DataGridViewRow dataRow = dgvStockpile.Rows[dgvStockpile.Rows.Add()];
			dataRow.Cells["Item"].Value = sk;
			dataRow.Cells["GameName"].Value = sk.GameName;
			dataRow.Cells["SystemName"].Value = sk.SystemName;
			dataRow.Cells["SystemCore"].Value = sk.SystemCore;

			RefreshNoteIcons();

			StockpileManager_UISide.StashHistory.Remove(sk);

			RefreshStashHistory();

			DontLoadSelectedStash = true;
			lbStashHistory.ClearSelected();
			DontLoadSelectedStash = false;

			int nRowIndex = dgvStockpile.Rows.Count - 1;

			dgvStockpile.ClearSelection();
			dgvStockpile.Rows[nRowIndex].Selected = true;

			StockpileManager_UISide.StockpileChanged();

			UnsavedEdits = true;
		}

		private void btnClearStashHistory_Click(object sender, EventArgs e)
		{
			StockpileManager_UISide.StashHistory.Clear();
			RefreshStashHistory();

			//Force clean up
			GC.Collect();
			GC.WaitForPendingFinalizers();
		}

		private void btnRemoveSelectedStockpile_Click(object sender, EventArgs e) => RemoveSelected();

		public void RemoveSelected(bool force = false)
		{

			if (Control.ModifierKeys == Keys.Control || (dgvStockpile.SelectedRows.Count != 0 && (MessageBox.Show("Are you sure you want to remove the selected stockpile entries?", "Delete Stockpile Entry?", MessageBoxButtons.YesNo) == DialogResult.Yes)))
				foreach (DataGridViewRow row in dgvStockpile.SelectedRows)
					dgvStockpile.Rows.Remove(row);

			StockpileManager_UISide.StockpileChanged();

			UnsavedEdits = true;

			RedrawActionUI();

		}

		private void btnClearStockpile_Click(object sender, EventArgs e) => ClearStockpile();

		public void ClearStockpile(bool force = false)
		{

			if (force || MessageBox.Show("Are you sure you want to clear the stockpile?", "Clearing stockpile", MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				dgvStockpile.Rows.Clear();

				if (StockpileManager_UISide.CurrentStockpile != null)
				{
					StockpileManager_UISide.CurrentStockpile.Filename = null;
					StockpileManager_UISide.CurrentStockpile.ShortFilename = null;
				}

				S.GET<RTC_GlitchHarvester_Form>().btnSaveStockpile.Enabled = false;
				S.GET<RTC_GlitchHarvester_Form>().btnSaveStockpile.BackColor = Color.Gray;
				S.GET<RTC_GlitchHarvester_Form>().btnSaveStockpile.ForeColor = Color.DimGray;

				StockpileManager_UISide.StockpileChanged();

				UnsavedEdits = false;

				RedrawActionUI();

			}
		}

		private void LoadStockpile(string filename = null)
		{
			if (UnsavedEdits && MessageBox.Show("You have unsaved edits in the Glitch Harvester Stockpile. \n\n Are you sure you want to load without saving?",
				"Unsaved edits in Stockpile", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}

			if (Stockpile.Load(dgvStockpile, filename))
			{
				btnSaveStockpile.Enabled = true;
				btnSaveStockpile.BackColor = Color.Tomato;
				btnSaveStockpile.ForeColor = Color.Black;
				RefreshNoteIcons();
			}

			S.GET<RTC_StockpilePlayer_Form>().dgvStockpile.Rows.Clear();

			dgvStockpile.ClearSelection();
			StockpileManager_UISide.StockpileChanged();

			UnsavedEdits = false;
		}

		private void btnLoadStockpile_Click(object sender, MouseEventArgs e)
		{
			CorruptCore.CorruptCore.CheckForProblematicProcesses();

			Point locate = new Point(((Control)sender).Location.X + e.Location.X, ((Control)sender).Location.Y + e.Location.Y);

			ContextMenuStrip loadMenuItems = new ContextMenuStrip();
			loadMenuItems.Items.Add("Load Stockpile", null, new EventHandler((ob, ev) =>
			{
				try
				{
					LoadStockpile();
				}
				finally
				{
				}
			}));
			

			loadMenuItems.Items.Add("Load Bizhawk settings from Stockpile", null, new EventHandler((ob, ev) =>
			{
				try
				{
					if (UnsavedEdits && MessageBox.Show("You have unsaved edits in the Glitch Harvester Stockpile. \n\n This will restart Bizhawk. Are you sure you want to load without saving?",
						"Unsaved edits in Stockpile", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					Stockpile.LoadBizhawkConfigFromStockpile();
				}
				finally
				{
				}
			}));

			loadMenuItems.Items.Add("Restore Bizhawk config Backup", null, new EventHandler((ob, ev) =>
			{
				try
				{
					if (UnsavedEdits && MessageBox.Show(
						"You have unsaved edits in the Glitch Harvester Stockpile. \n\n This will restart Bizhawk. Are you sure you want to load without saving?",
						"Unsaved edits in Stockpile", MessageBoxButtons.YesNo) == DialogResult.No)
						return;
					Stockpile.RestoreBizhawkConfig();
				}
				finally
				{
				}
			})).Enabled = (File.Exists(CorruptCore.CorruptCore.bizhawkDir + Path.DirectorySeparatorChar + "backup_config.ini"));

			loadMenuItems.Show(this, locate);
		}

		private void btnSaveStockpileAs_Click(object sender, EventArgs e)
		{
			if (dgvStockpile.Rows.Count == 0)
			{
				MessageBox.Show("You cannot save the Stockpile because it is empty");
				return;
			}


			Stockpile sks = new Stockpile(dgvStockpile);
			if (Stockpile.Save(sks))
			{
				sendCurrentStockpileToSKS();
				S.GET<RTC_GlitchHarvester_Form>().btnSaveStockpile.Enabled = true;
				S.GET<RTC_GlitchHarvester_Form>().btnSaveStockpile.BackColor = Color.Tomato;
				S.GET<RTC_GlitchHarvester_Form>().btnSaveStockpile.ForeColor = Color.Black;
			}

			UnsavedEdits = false;

		}

		private void btnSaveStockpile_Click(object sender, EventArgs e)
		{

			Stockpile sks = new Stockpile(dgvStockpile);
			if (Stockpile.Save(sks, true))
				sendCurrentStockpileToSKS();

			UnsavedEdits = false;
		}

		public void btnBlastToggle_Click(object sender, EventArgs e)
		{
			if (StockpileManager_UISide.CurrentStashkey?.BlastLayer?.Layer == null || StockpileManager_UISide.CurrentStashkey?.BlastLayer?.Layer.Count == 0)
			{
				IsCorruptionApplied = false;
				return;
			}

			if (!IsCorruptionApplied)
			{
				IsCorruptionApplied = true;

				LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_SET_APPLYCORRUPTBL, true);
			}
			else
			{
				IsCorruptionApplied = false;

				LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_SET_APPLYUNCORRUPTBL, true);
				LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_CLEARSTEPBLASTUNITS, null, true);
			}
		}

		private void btnImportStockpile_Click(object sender, EventArgs e)
		{
			Stockpile.Import(null, dgvStockpile);
			RefreshNoteIcons();
		}

		private void btnStashUP_Click(object sender, EventArgs e)
		{
			if (lbStashHistory.SelectedIndex == -1)
				return;

			if (lbStashHistory.SelectedIndex == 0)
			{
				lbStashHistory.ClearSelected();
				lbStashHistory.SelectedIndex = lbStashHistory.Items.Count - 1;
			}
			else
			{
				int newPos = lbStashHistory.SelectedIndex--;
				lbStashHistory.ClearSelected();
				lbStashHistory.SelectedIndex = newPos;

			}
		}

		private void btnStashDOWN_Click(object sender, EventArgs e)
		{

			if (lbStashHistory.SelectedIndex == -1)
				return;

			if (lbStashHistory.SelectedIndex == lbStashHistory.Items.Count - 1)
			{
				lbStashHistory.ClearSelected();
				lbStashHistory.SelectedIndex = 0;
			}
			else
			{
				int newPos = lbStashHistory.SelectedIndex++;
				lbStashHistory.ClearSelected();
				lbStashHistory.SelectedIndex = newPos;

			}
		}

		private void btnStockpileUP_Click(object sender, EventArgs e)
		{

			if (dgvStockpile.SelectedRows.Count == 0)
				return;

			int currentSelectedIndex = dgvStockpile.SelectedRows[0].Index;

			if (currentSelectedIndex == 0)
			{
				dgvStockpile.ClearSelection();
				dgvStockpile.Rows[dgvStockpile.Rows.Count - 1].Selected = true;
			}
			else
			{
				dgvStockpile.ClearSelection();
				dgvStockpile.Rows[currentSelectedIndex - 1].Selected = true;
			}

			dgvStockpile_CellClick(dgvStockpile, null);
		}

		private void btnStockpileDOWN_Click(object sender, EventArgs e)
		{

			if (dgvStockpile.SelectedRows.Count == 0)
				return;

			int currentSelectedIndex = dgvStockpile.SelectedRows[0].Index;

			if (currentSelectedIndex == dgvStockpile.Rows.Count - 1)
			{
				dgvStockpile.ClearSelection();
				dgvStockpile.Rows[0].Selected = true;
			}
			else
			{
				dgvStockpile.ClearSelection();
				dgvStockpile.Rows[currentSelectedIndex + 1].Selected = true;
			}

			dgvStockpile_CellClick(dgvStockpile, null);
		}

		private void btnStockpileMoveSelectedUp_Click(object sender, EventArgs e)
		{
			if (dgvStockpile.SelectedRows.Count == 0)
				return;

			int count = dgvStockpile.Rows.Count;

			if (count < 2)
				return;

			int pos = dgvStockpile.SelectedRows[0].Index;
			DataGridViewRow row = dgvStockpile.Rows[pos];

			dgvStockpile.Rows.RemoveAt(pos);

			if (pos == 0)
			{
				int newpos = dgvStockpile.Rows.Add(row);
				dgvStockpile.ClearSelection();
				dgvStockpile.Rows[newpos].Selected = true;
			}
			else
			{
				int newpos = pos - 1;
				dgvStockpile.Rows.Insert(newpos, row);
				dgvStockpile.ClearSelection();
				dgvStockpile.Rows[newpos].Selected = true;
			}

			UnsavedEdits = true;

			StockpileManager_UISide.StockpileChanged();
		}

		private void btnStockpileMoveSelectedDown_Click(object sender, EventArgs e)
		{
			if (dgvStockpile.SelectedRows.Count == 0)
				return;

			int count = dgvStockpile.Rows.Count;

			if (count < 2)
				return;

			int pos = dgvStockpile.SelectedRows[0].Index;
			var row = dgvStockpile.Rows[pos];

			dgvStockpile.Rows.RemoveAt(pos);

			if (pos == count - 1)
			{
				int newpos = 0;
				dgvStockpile.Rows.Insert(newpos, row);
				dgvStockpile.ClearSelection();
				dgvStockpile.Rows[newpos].Selected = true;
			}
			else
			{
				int newpos = pos + 1;
				dgvStockpile.Rows.Insert(newpos, row);
				dgvStockpile.ClearSelection();
				dgvStockpile.Rows[newpos].Selected = true;
			}

			UnsavedEdits = true;

			StockpileManager_UISide.StockpileChanged();
		}

		Guid? intensitySliderToken = null;

		private void dgvStockpile_DragDrop(object sender, DragEventArgs e)
		{
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
			if (files?.Length > 0 && files[0]
				.Contains(".sks"))
			{
				LoadStockpile(files[0]);
			}

			//Bring the UI back to normal after a drag+drop to prevent weird merge stuff 
			RedrawActionUI();
		}

		private void dgvStockpile_DragEnter(object sender, DragEventArgs e)
		{
			e.Effect = DragDropEffects.Link;
		}

		private void pnSavestateHolder_DragEnter(object sender, DragEventArgs e)
		{
			e.Effect = DragDropEffects.Link;
		}

		private void pnSavestateHolder_DragDrop(object sender, DragEventArgs e)
		{
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
			if (files?.Length > 0 && files[0]
				.Contains(".ssk"))
			{
				loadSavestateList(files[0]);
			}

			//Bring the UI back to normal after a drag+drop to prevent weird merge stuff 
			RedrawActionUI();
		}


		public void btnSendRaw_Click(object sender, EventArgs e)
		{
			try
			{
				btnSendRaw.Visible = false;


				string romFilename = (string)RTCV.NetCore.AllSpec.VanguardSpec[VSPEC.OPENROMFILENAME];
				if (romFilename == null)
					return;
				if (romFilename.Contains("|"))
				{
					MessageBox.Show($"The Glitch Harvester attempted to corrupt a game bound to the following file:\n{romFilename}\n\nIt cannot be processed because the rom seems to be inside a Zip Archive\n(Bizhawk returned a filename with the chracter | in it)");
					return;
				}

				StashKey sk = LocalNetCoreRouter.QueryRoute<StashKey>(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_KEY_GETRAWBLASTLAYER, true);

				StockpileManager_UISide.CurrentStashkey = sk;
				StockpileManager_UISide.StashHistory.Add(StockpileManager_UISide.CurrentStashkey);


				DontLoadSelectedStash = true;
				RefreshStashHistorySelectLast();
				DontLoadSelectedStash = true;
				dgvStockpile.ClearSelection();
				DontLoadSelectedStash = false;
			}
			finally
			{
				btnSendRaw.Visible = true;
			}
		}

		private void renameStashKey(StashKey sk)
		{
			string value = "";

			if (GetInputBox("Glitch Harvester", "Enter the new Stash name:", ref value) == DialogResult.OK)
			{
				sk.Alias = value.Trim();
			}
			else
			{
				return;
			}
		}

		private void btnRenameSelected_Click(object sender, EventArgs e)
		{
			if (!btnRenameSelected.Visible)
				return;


			if (dgvStockpile.SelectedRows.Count != 0)
			{
				renameStashKey(dgvStockpile.SelectedRows[0].Cells[0].Value as StashKey);

				dgvStockpile.Refresh();
				//lbStockpile.RefreshItemsReal();
			}

			StockpileManager_UISide.StockpileChanged();

			UnsavedEdits = true;

		}

		private void cbAutoLoadState_CheckedChanged(object sender, EventArgs e)
		{
			loadBeforeOperation = cbAutoLoadState.Checked;
		}

		private void cbStashCorrupted_CheckedChanged(object sender, EventArgs e)
		{
			StockpileManager_UISide.StashAfterOperation = cbStashCorrupted.Checked;
		}

		private void btnBackPanelPage_Click(object sender, EventArgs e)
		{
			if (pnSavestateHolder.Location.X != 0)
				pnSavestateHolder.Location = new Point(pnSavestateHolder.Location.X + 150, pnSavestateHolder.Location.Y);
		}

		private void btnForwardPanelPage_Click(object sender, EventArgs e)
		{
			if (pnSavestateHolder.Location.X != -450)
				pnSavestateHolder.Location = new Point(pnSavestateHolder.Location.X - 150, pnSavestateHolder.Location.Y);
		}

		private void dgvStockpile_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				Point locate = new Point((sender as Control).Location.X + e.Location.X, (sender as Control).Location.Y + e.Location.Y);

				ContextMenuStrip columnsMenu = new ContextMenuStrip();
				(columnsMenu.Items.Add("Show Item Name", null,
						(ob, ev) => { dgvStockpile.Columns["Item"].Visible ^= true; }) as ToolStripMenuItem).Checked =
					dgvStockpile.Columns["Item"].Visible;
				(columnsMenu.Items.Add("Show Game Name", null,
						(ob, ev) => { dgvStockpile.Columns["GameName"].Visible ^= true; }) as ToolStripMenuItem)
					.Checked =
					dgvStockpile.Columns["GameName"].Visible;
				(columnsMenu.Items.Add("Show System Name", null,
						(ob, ev) => { dgvStockpile.Columns["SystemName"].Visible ^= true; }) as ToolStripMenuItem)
					.Checked =
					dgvStockpile.Columns["SystemName"].Visible;
				(columnsMenu.Items.Add("Show System Core", null,
						(ob, ev) => { dgvStockpile.Columns["SystemCore"].Visible ^= true; }) as ToolStripMenuItem)
					.Checked =
					dgvStockpile.Columns["SystemCore"].Visible;
				(columnsMenu.Items.Add("Show Note", null, (ob, ev) => { dgvStockpile.Columns["Note"].Visible ^= true; })
					as ToolStripMenuItem).Checked = dgvStockpile.Columns["Note"].Visible;

				columnsMenu.Items.Add(new ToolStripSeparator());
				((ToolStripMenuItem)columnsMenu.Items.Add("Open Selected Item in Blast Editor", null, new EventHandler((ob, ev) =>
				{
					if (S.GET<RTC_NewBlastEditor_Form>() != null)
					{
						var sk = (dgvStockpile.SelectedRows[0].Cells[0].Value as StashKey);
						OpenBlastEditor(sk);
					}
				}))).Enabled = (dgvStockpile.SelectedRows.Count == 1);

				columnsMenu.Items.Add(new ToolStripSeparator());
				((ToolStripMenuItem)columnsMenu.Items.Add("Generate VMD from Selected Item", null, new EventHandler((ob, ev) =>
				{
					var sk = (dgvStockpile.SelectedRows[0].Cells[0].Value as StashKey);
					MemoryDomains.GenerateVmdFromStashkey(sk);
					S.GET<RTC_VmdPool_Form>().RefreshVMDs();
				}))).Enabled = (dgvStockpile.SelectedRows.Count == 1);

				((ToolStripMenuItem)columnsMenu.Items.Add("Merge Selected Stashkeys", null, new EventHandler((ob, ev) =>
				{
					List<StashKey> sks = new List<StashKey>();
					foreach (DataGridViewRow row in dgvStockpile.SelectedRows)
						sks.Add((StashKey)row.Cells[0].Value);
					StockpileManager_UISide.MergeStashkeys(sks);
					RefreshStashHistory();
				}))).Enabled = (dgvStockpile.SelectedRows.Count > 1);

				/*
				if (!RTC_NetcoreImplementation.isStandaloneUI)
				{
					((ToolStripMenuItem)columnsMenu.Items.Add("[Multiplayer] Send Selected Item as a Blast", null, new EventHandler((ob, ev) => { RTC_NetcoreImplementation.Multiplayer?.SendBlastlayer(); }))).Enabled = RTC_NetcoreImplementation.Multiplayer != null && RTC_NetcoreImplementation.Multiplayer.side != NetworkSide.DISCONNECTED;
					((ToolStripMenuItem)columnsMenu.Items.Add("[Multiplayer] Send Selected Item as a Game State", null, new EventHandler((ob, ev) => { RTC_NetcoreImplementation.Multiplayer?.SendStashkey(); }))).Enabled = RTC_NetcoreImplementation.Multiplayer != null && RTC_NetcoreImplementation.Multiplayer.side != NetworkSide.DISCONNECTED;
				}*/

				columnsMenu.Show(this, locate);
			}
		}

		private void btnRerollSelected_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				Point locate = this.PointToClient(Cursor.Position);
				ContextMenuStrip rerollMenu = new ContextMenuStrip();
				rerollMenu.Items.Add("Configure Reroll", null, new EventHandler((ob, ev) =>
				{
					S.GET<RTC_Core_Form>().ShowPanelForm(S.GET<RTC_Settings_Form>(), false);
					S.GET<RTC_Settings_Form>().lbForm.SetFocusedForm(S.GET<RTC_SettingsCorrupt_Form>());
					S.GET<RTC_Core_Form>().BringToFront();
				}));

				rerollMenu.Show(this, locate);
			}
		}

		public void btnRerollSelected_Click(object sender, EventArgs e)
		{
			if (lbStashHistory.SelectedIndex != -1)
			{
				StockpileManager_UISide.CurrentStashkey = (StashKey)StockpileManager_UISide.StashHistory[lbStashHistory.SelectedIndex].Clone();
			}
			else if (dgvStockpile.SelectedRows.Count != 0 && dgvStockpile.SelectedRows[0].Cells[0].Value != null)
			{
				StockpileManager_UISide.CurrentStashkey = (StashKey)(dgvStockpile.SelectedRows[0].Cells[0].Value as StashKey)?.Clone();
				//StockpileManager_UISide.unsavedEdits = true;
			}
			else
				return;

			if (StockpileManager_UISide.CurrentStashkey != null)
			{
				StockpileManager_UISide.CurrentStashkey.BlastLayer.Reroll();

				StockpileManager_UISide.AddCurrentStashkeyToStash();
				RefreshStashHistory();

				StockpileManager_UISide.ApplyStashkey(StockpileManager_UISide.CurrentStashkey);
			}
		}

		private void btnSaveSavestateList_Click(object sender, EventArgs e)
		{
			try
			{
				SaveStateKeys ssk = new SaveStateKeys();

				for (int i = 1; i < 41; i++)
				{
					string key = i.ToString().PadLeft(2, '0');

					if (StockpileManager_UISide.SavestateStashkeyDico.ContainsKey(key))
					{
						ssk.StashKeys[i] = StockpileManager_UISide.SavestateStashkeyDico[key];
						ssk.Text[i] = StateBoxes[key].Text;
					}
					else
					{
						ssk.StashKeys[i] = null;
						ssk.Text[i] = null;
					}
				}

				string Filename;
				string ShortFilename;

				SaveFileDialog saveFileDialog1 = new SaveFileDialog
				{
					DefaultExt = "ssk",
					Title = "Savestate Keys File",
					Filter = "SSK files|*.ssk",
					RestoreDirectory = true
				};

				if (saveFileDialog1.ShowDialog() == DialogResult.OK)
				{
					Filename = saveFileDialog1.FileName;
					//ShortFilename = Filename.Substring(Filename.LastIndexOf(Path.DirectorySeparatorChar) + 1, Filename.Length - (Filename.LastIndexOf(Path.DirectorySeparatorChar) + 1));
					ShortFilename = Path.GetFileName(Filename);
				}
				else
					return;

				//clean temp folder
				Stockpile.EmptyFolder(Path.DirectorySeparatorChar + "WORKING\\TEMP");

				for (int i = 1; i < 41; i++)
				{
					StashKey key = ssk.StashKeys[i];

					if (key == null)
						continue;

					string statefilename = key.GameName + "." + key.ParentKey + ".timejump.State"; // get savestate name

					if (File.Exists(CorruptCore.CorruptCore.workingDir + Path.DirectorySeparatorChar + key.StateLocation.ToString() + Path.DirectorySeparatorChar + statefilename))
						File.Copy(CorruptCore.CorruptCore.workingDir + Path.DirectorySeparatorChar + key.StateLocation.ToString() + Path.DirectorySeparatorChar + statefilename, CorruptCore.CorruptCore.workingDir + Path.DirectorySeparatorChar + "TEMP" + Path.DirectorySeparatorChar + statefilename); // copy savestates to temp folder
					else
					{
						MessageBox.Show("Couldn't find savestate " + CorruptCore.CorruptCore.workingDir + Path.DirectorySeparatorChar +
										key.StateLocation.ToString() + Path.DirectorySeparatorChar + statefilename +
										"!\n\n. This is savestate index " + i + 1 + ".\nAborting save");
						Stockpile.EmptyFolder(Path.DirectorySeparatorChar + "WORKING\\TEMP");
						return;
					}

				}

				//Use two separate loops here in case the first one aborts. We don't want to update the StateLocation unless we know we're good
				for (int i = 1; i < 41; i++)
				{
					StashKey key = ssk.StashKeys[i];

					if (key == null)
						continue;
					key.StateLocation = StashKeySavestateLocation.SSK;
				}

				//Create keys.json
				using (FileStream fs = File.Open(CorruptCore.CorruptCore.workingDir + Path.DirectorySeparatorChar + "TEMP\\keys.json", FileMode.OpenOrCreate))
				{
					JsonHelper.Serialize(ssk, fs, Formatting.Indented);
					fs.Close();
				}

				//7z the temp folder to destination filename
				//string[] stringargs = { "-c", Filename, RTC_Core.rtcDir + Path.DirectorySeparatorChar + "TEMP4" + Path.DirectorySeparatorChar };
				//FastZipProgram.Exec(stringargs);

				string tempFilename = Filename + ".temp";

				System.IO.Compression.ZipFile.CreateFromDirectory(CorruptCore.CorruptCore.workingDir + Path.DirectorySeparatorChar + "TEMP" + Path.DirectorySeparatorChar, tempFilename, System.IO.Compression.CompressionLevel.Fastest, false);

				if (File.Exists(Filename))
					File.Delete(Filename);

				File.Move(tempFilename, Filename);

				//Move all the files from temp into SSK
				Stockpile.EmptyFolder(Path.DirectorySeparatorChar + "WORKING\\SSK");
				foreach (string file in Directory.GetFiles(CorruptCore.CorruptCore.workingDir + Path.DirectorySeparatorChar + "TEMP"))
					//File.Move(file, RTC_Core.workingDir + Path.DirectorySeparatorChar + "SSK" + Path.DirectorySeparatorChar + (file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar) + 1, file.Length - (file.LastIndexOf(Path.DirectorySeparatorChar) + 1))));
					File.Move(file, CorruptCore.CorruptCore.workingDir + Path.DirectorySeparatorChar + "SSK" + Path.DirectorySeparatorChar + Path.GetFileName(file));
			}
			catch (Exception ex)
			{

				string additionalInfo = "The Savestate Keys file could not be saved\n\n";

				var ex2 = new CustomException(ex.Message, additionalInfo + ex.StackTrace);

				if (CloudDebug.ShowErrorDialog(ex2, true) == DialogResult.Abort)
					throw new RTCV.NetCore.AbortEverythingException();

				return;
			}
		}

		private void loadSavestateList(string fileName = null)
		{
			if (fileName == null)
			{
				OpenFileDialog ofd = new OpenFileDialog
				{
					DefaultExt = "ssk",
					Title = "Open Savestate Keys File",
					Filter = "SSK files|*.ssk",
					RestoreDirectory = true
				};
				if (ofd.ShowDialog() == DialogResult.OK)
				{
					fileName = ofd.FileName;
				}
				else
					return;
			}

			if (!File.Exists(fileName))
			{
				MessageBox.Show("The Savestate Keys file wasn't found");
				return;
			}

			SaveStateKeys ssk;

			//Commit any used states to the SESSION folder
			commitUsedStatesToSession();

			try
			{

				Stockpile.EmptyFolder(Path.DirectorySeparatorChar + "WORKING\\TEMP");
				if (!Stockpile.Extract(fileName, Path.DirectorySeparatorChar + "WORKING\\SSK", "keys.json"))
					return;

				using (FileStream fs = File.Open(CorruptCore.CorruptCore.workingDir + Path.DirectorySeparatorChar + "SSK\\keys.json", FileMode.OpenOrCreate))
				{
					ssk = JsonHelper.Deserialize<SaveStateKeys>(fs);
				}
			}
			catch (Exception ex)
			{
				string additionalInfo = "The Savestate Keys file could not be loaded\n\n";

				var ex2 = new CustomException(ex.Message, additionalInfo + ex.StackTrace);

				if (CloudDebug.ShowErrorDialog(ex2, true) == DialogResult.Abort)
					throw new RTCV.NetCore.AbortEverythingException();

				return;
			}

			if (ssk == null)
			{
				MessageBox.Show("The Savestate Keys file was empty (null).\n");
				return;
			}


			for (int i = 1; i < 41; i++)
			{
				StashKey key = ssk.StashKeys[i];

				if (key == null)
					continue;

				//We have to set this first as we then change the other stuff
				key.StateLocation = StashKeySavestateLocation.SSK;

				string statefilename = key.GameName + "." + key.ParentKey + ".timejump.State"; // get savestate name
				string newStatePath = CorruptCore.CorruptCore.workingDir + Path.DirectorySeparatorChar + key.StateLocation + Path.DirectorySeparatorChar + statefilename;

				key.StateFilename = newStatePath;
				key.StateShortFilename = Path.GetFileName(newStatePath);
			}

			//clear the stockpile dico
			StockpileManager_UISide.SavestateStashkeyDico.Clear();

			//fill text/state controls/dico
			for (int i = 1; i < 41; i++)
			{
				string key = i.ToString().PadLeft(2, '0');

				if (ssk.StashKeys[i] != null)
				{
					if (!StockpileManager_UISide.SavestateStashkeyDico.ContainsKey(key))
						StockpileManager_UISide.SavestateStashkeyDico.Add(key, ssk.StashKeys[i]);
					else
						StockpileManager_UISide.SavestateStashkeyDico[key] = ssk.StashKeys[i];
				}

				StateBoxes[key].Text = "";

				if (ssk.Text[i] != null)
					StateBoxes[key].Text = ssk.Text[i];
			}


			RefreshSavestateTextboxes();
		}


		private void commitUsedStatesToSession()
		{
			var allStashKeys = new List<StashKey>();
			//Commit any used states to SESSION so we can safely unload the sk
			foreach (var row in dgvStockpile.Rows.Cast<DataGridViewRow>().ToList())
			{
				if (row.Cells[0].Value is StashKey sk)
					allStashKeys.Add(sk);
			}
			allStashKeys.AddRange(StockpileManager_UISide.StashHistory);
			allStashKeys.AddRange(S.GET<RTC_NewBlastEditor_Form>().GetStashKeys());
			allStashKeys.AddRange(S.GET<RTC_BlastGenerator_Form>().GetStashKeys());
			foreach (var sk in allStashKeys.Where(x => x?.StateLocation == StashKeySavestateLocation.SSK))
			{
				try
				{
					var stateName = sk.GameName + "." + sk.ParentKey + ".timejump.State"; // get savestate name
					File.Copy(CorruptCore.CorruptCore.workingDir + Path.DirectorySeparatorChar + "SSK" + Path.DirectorySeparatorChar + stateName
						, CorruptCore.CorruptCore.workingDir + Path.DirectorySeparatorChar + "SESSION" + Path.DirectorySeparatorChar + stateName, true);
					sk.StateLocation = StashKeySavestateLocation.SESSION;
				}
				catch (IOException e)
				{
					throw new CustomException("Couldn't copy savestate " + sk.StateShortFilename + " to SESSION! " + e.Message, e.StackTrace);
				}
			}
		}

		private void btnLoadSavestateList_Click(object sender, EventArgs e)
		{
			loadSavestateList();
		}


		public void RefreshSavestateTextboxes()
		{
			//fill text/state controls/dico
			for (int i = 1; i < 41; i++)
			{
				string key = i.ToString().PadLeft(2, '0');

				StateBoxes[key].Visible = StockpileManager_UISide.SavestateStashkeyDico.ContainsKey(key);
			}
		}

		private void lbStashHistory_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				Point locate = new Point(((Control)sender).Location.X + e.Location.X, ((Control)sender).Location.Y + e.Location.Y);

				ContextMenuStrip columnsMenu = new ContextMenuStrip();

				((ToolStripMenuItem)columnsMenu.Items.Add("Open Selected Item in Blast Editor", null, new EventHandler((ob, ev) =>
				{
					if (S.GET<RTC_NewBlastEditor_Form>() != null)
					{
						StashKey sk = StockpileManager_UISide.StashHistory[lbStashHistory.SelectedIndex];
						OpenBlastEditor(sk);
					}
				}))).Enabled = lbStashHistory.SelectedIndex != -1;

				columnsMenu.Items.Add(new ToolStripSeparator());

				((ToolStripMenuItem)columnsMenu.Items.Add("Rename selected item", null, new EventHandler((ob, ev) =>
				{
					StashKey sk = StockpileManager_UISide.StashHistory[lbStashHistory.SelectedIndex];
					renameStashKey(sk);
					RefreshStashHistory();
				}))).Enabled = lbStashHistory.SelectedIndex != -1;

				((ToolStripMenuItem)columnsMenu.Items.Add("Generate VMD from Selected Item", null, new EventHandler((ob, ev) =>
				{
					StashKey sk = StockpileManager_UISide.StashHistory[lbStashHistory.SelectedIndex];
					sk.BlastLayer.RasterizeVMDs();
					MemoryDomains.GenerateVmdFromStashkey(sk);
					S.GET<RTC_VmdPool_Form>().RefreshVMDs();
				}))).Enabled = lbStashHistory.SelectedIndex != -1;

				columnsMenu.Items.Add(new ToolStripSeparator());

				((ToolStripMenuItem)columnsMenu.Items.Add("Merge Selected Stashkeys", null, new EventHandler((ob, ev) =>
				{
					List<StashKey> sks = new List<StashKey>();
					foreach (StashKey sk in lbStashHistory.SelectedItems)
						sks.Add((StashKey)sk);

					StockpileManager_UISide.MergeStashkeys(sks);

					RefreshStashHistory();
				}))).Enabled = (lbStashHistory.SelectedIndex != -1 && lbStashHistory.SelectedItems.Count > 1);

				/*
				if (!RTC_NetcoreImplementation.isStandaloneUI)
				{
					columnsMenu.Items.Add(new ToolStripSeparator());
					((ToolStripMenuItem)columnsMenu.Items.Add("[Multiplayer] Pull State from peer", null, new EventHandler((ob, ev) =>
						{
							S.GET<RTC_Multiplayer_Form>().cbPullStateToGlitchHarvester.Checked = true;
							RTC_NetcoreImplementation.Multiplayer.SendCommand(new RTC_Command(CommandType.PULLSTATE), false);
						}))).Enabled = RTC_NetcoreImplementation.Multiplayer != null && RTC_NetcoreImplementation.Multiplayer.side != NetworkSide.DISCONNECTED;
				}*/

				columnsMenu.Show(this, locate);
			}
		}

		private void OpenBlastEditor(StashKey sk)
		{
			S.GET<RTC_NewBlastEditor_Form>().Close();
			S.SET(new RTC_NewBlastEditor_Form());

			//If the blastlayer is big, prompt them before opening it. Let's go with 5k for now.

			//TODO
			if (sk.BlastLayer.Layer.Count > 5000 && (DialogResult.Yes == MessageBox.Show($"You're trying to open a blastlayer of size " + sk.BlastLayer.Layer.Count + ". This could take a while. Are you sure you want to continue?", "Opening a large BlastLayer", MessageBoxButtons.YesNo)))
				S.GET<RTC_NewBlastEditor_Form>().LoadStashkey(sk);
			else if (sk.BlastLayer.Layer.Count <= 5000)
				S.GET<RTC_NewBlastEditor_Form>().LoadStashkey(sk);
		}

		private void sendCurrentStockpileToSKS()
		{
			foreach (DataGridViewRow dataRow in S.GET<RTC_GlitchHarvester_Form>().dgvStockpile.Rows)
			{
				StashKey sk = (StashKey)dataRow.Cells["Item"].Value;
			}
		}

		private void dgvStockpile_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				btnAddStashToStockpile.Enabled = false;
				dgvStockpile.Enabled = false;
				btnStockpileUP.Enabled = false;
				btnStockpileDOWN.Enabled = false;

				// Stockpile Note handling
				if (e != null)
				{
					var senderGrid = (DataGridView)sender;

					if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
						e.RowIndex >= 0)
					{
						StashKey sk = (StashKey)senderGrid.Rows[e.RowIndex].Cells["Item"].Value;
						S.SET(new RTC_NoteEditor_Form(sk, senderGrid.Rows[e.RowIndex].Cells["Note"]));
						S.GET<RTC_NoteEditor_Form>().Show();

						return;
					}
				}

				lbStashHistory.ClearSelected();
				S.GET<RTC_StockpilePlayer_Form>().dgvStockpile.ClearSelection();

				RedrawActionUI();

				if (dgvStockpile.SelectedRows.Count == 0)
					return;

				StockpileManager_UISide.CurrentStashkey = (dgvStockpile.SelectedRows[0].Cells[0].Value as StashKey);

				if (!cbLoadOnSelect.Checked)
					return;

				// Merge Execution
				if (dgvStockpile.SelectedRows.Count > 1)
				{
					List<StashKey> sks = new List<StashKey>();

					foreach (DataGridViewRow row in dgvStockpile.SelectedRows)
						sks.Add((StashKey)row.Cells[0].Value);
					//dgv is stupid since it selects rows backwards
					sks.Reverse();
					StockpileManager_UISide.MergeStashkeys(sks);

					if (StockpileManager_EmuSide.RenderAtLoad && loadBeforeOperation)
					{
						btnRender.Text = "Stop Render";
						btnRender.ForeColor = Color.OrangeRed;
					}
					else
					{
						btnRender.Text = "Start Render";
						btnRender.ForeColor = Color.White;
					}
					RefreshStashHistory();
					return;
				}

				OneTimeExecute();
			}
			finally
			{
				btnAddStashToStockpile.Enabled = true;
				dgvStockpile.Enabled = true;
				btnStockpileUP.Enabled = true;
				btnStockpileDOWN.Enabled = true;
			}
		}

		public void RedrawActionUI()
		{
			// Merge tool and ui change
			if (dgvStockpile.SelectedRows.Count > 1)
			{
				rbCorrupt.Checked = true;
				rbCorrupt.Enabled = false;
				rbInject.Enabled = false;
				rbOriginal.Enabled = false;
				btnCorrupt.Text = "Merge";
				btnRenameSelected.Visible = false;
				btnRemoveSelectedStockpile.Text = "Remove Items";

				rbCorrupt.Checked = true;
			}
			else
			{
				rbCorrupt.Enabled = true;
				rbInject.Enabled = true;
				rbOriginal.Enabled = true;
				btnRenameSelected.Visible = true;
				btnRemoveSelectedStockpile.Text = "Remove Item";

				if (rbCorrupt.Checked)
					btnCorrupt.Text = "Blast/Send";
				else if (rbInject.Checked)
					btnCorrupt.Text = "Inject";
				else if (rbOriginal.Checked)
					btnCorrupt.Text = "Original";
			}
		}

		private void cbRenderType_SelectedIndexChanged(object sender, EventArgs e)
		{
				Render.setType(cbRenderType.SelectedItem.ToString());
		}

		private void btnOpenRenderFolder_Click(object sender, EventArgs e)
		{
				Process.Start(CorruptCore.CorruptCore.rtcDir + Path.DirectorySeparatorChar + "RENDEROUTPUT" + Path.DirectorySeparatorChar);
		}

		private void btnRender_Click(object sender, EventArgs e)
		{
			if (btnRender.Text == "Start Render")
			{
				btnRender.Text ="Stop Render";
				btnRender.ForeColor = Color.OrangeRed;
				Render.StartRender();
			}
			else
			{
				btnRender.Text = "Start Render";
				btnRender.ForeColor = Color.White;
				Render.StopRender();
			}
		}

		private void cbRenderAtLoad_CheckedChanged(object sender, EventArgs e)
		{
			StockpileManager_EmuSide.RenderAtLoad = cbRenderAtLoad.Checked;
		}

		private void btnLoadSavestateList_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				Point locate = new Point((sender as Control).Location.X + e.Location.X, (sender as Control).Location.Y + e.Location.Y);

				ContextMenuStrip columnsMenu = new ContextMenuStrip();
				columnsMenu.Items.Add("Clear Savestate List", null, new EventHandler((ob, ev) =>
				{
					//Commit any used states to disk
					commitUsedStatesToSession();

					foreach (var item in pnSavestateHolder.Controls)
					{
						if (item is Button)
							(item as Button).ForeColor = Color.FromArgb(192, 255, 192);

						if (item is TextBox)
							(item as TextBox).Text = "";
					}

					for (int i = 1; i < 41; i++)
					{
						string key = i.ToString().PadLeft(2, '0');

						if (key == null)
							continue;

						if (StockpileManager_UISide.SavestateStashkeyDico.ContainsKey(key))
							StockpileManager_UISide.SavestateStashkeyDico.Remove(key);

					}

					StockpileManager_UISide.CurrentSavestateKey = null;

					RefreshSavestateTextboxes();
				}));

				columnsMenu.Show(this, locate);
			}
		}

		private void BlastRawStash()
		{
			LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.ASYNCBLAST, true);
			S.GET<RTC_GlitchHarvester_Form>().btnSendRaw_Click(null, null);
		}

		private void btnCorrupt_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				Point locate = new Point((sender as Control).Location.X + e.Location.X, (sender as Control).Location.Y + e.Location.Y);

				ContextMenuStrip columnsMenu = new ContextMenuStrip();
				columnsMenu.Items.Add("Blast + Send RAW To Stash", null, new EventHandler((ob, ev) =>
				{
					BlastRawStash();
				}));
				columnsMenu.Show(this, locate);
			}
		}

	}
}
