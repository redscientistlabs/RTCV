using RTCV.CorruptCore;
using RTCV.NetCore;
using RTCV.NetCore.StaticTools;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace RTCV.UI
{
	// 0  dgvBlastProtoReference
	// 1  dgvRowDirty
	// 2  dgvEnabled
	// 3  dgvNoteText
	// 4  dgvDomain
	// 5  dgvPrecision
	// 6  dgvType
	// 7  dgvMode
	// 8  dgvInterval
	// 9  dgvStartAddress
	// 10 dgvEndAddress
	// 11 dgvParam1
	// 12 dgvParam2
	// 13 dgvSeed
	// 14 dgvNoteButton

	//TYPE = BLASTUNITTYPE
	//MODE = GENERATIONMODE

	public partial class RTC_BlastGenerator_Form : Form
	{
		private enum BlastGeneratorColumn
		{
			dgvBlastProtoReference,
			DgvRowDirty,
			DgvEnabled,
			DgvNoteText,
			DgvDomain,
			DgvPrecision,
			DgvType,
			DgvMode,
			dgvInterval,
			DgvStartAddress,
			DgvEndAddress,
			DgvParam1,
			DgvParam2,
			DgvLifetime,
			DgvExecuteFrame,
			DgvLoop,
			DgvSeed,
			DgvNoteButton
		}

		public static BlastLayer CurrentBlastLayer = null;
		public bool OpenedFromBlastEditor = false;
		private StashKey sk = null;
		private ContextMenuStrip cms = new ContextMenuStrip();
		private bool initialized = false;
		private List<Control> allControls;

		private static Dictionary<string, MemoryInterface> domainToMiDico = new Dictionary<string, MemoryInterface>();
		private string[] domains = MemoryDomains.MemoryInterfaces?.Keys?.Concat(MemoryDomains.VmdPool.Values.Select(it => it.ToString())).ToArray();

		public RTC_BlastGenerator_Form()
		{
			InitializeComponent();

			//For some godforsaken reason, xmlSerializer deserialization wont fill this in as a bool so just use a string god help us all 
			(dgvBlastGenerator.Columns["dgvEnabled"]).ValueType = typeof(string);
		}

		private void RTC_BlastGeneratorForm_Load(object sender, EventArgs e)
		{
			dgvBlastGenerator.MouseClick += dgvBlastGenerator_MouseClick;
			dgvBlastGenerator.CellValueChanged += dgvBlastGenerator_CellValueChanged;
			dgvBlastGenerator.CellClick += dgvBlastGenerator_CellClick;
			dgvBlastGenerator.CellMouseDoubleClick += DgvBlastGenerator_CellMouseDoubleClick;

			UICore.SetRTCColor(UICore.GeneralColor, this);
			getAllControls(this);
		}

		private void getAllControls(Control container)
		{
			allControls = new List<Control>();
			foreach (Control c in container.Controls)
			{
				getAllControls(c);
				allControls.Add(c);
			}
		}
		public void LoadNoStashKey()
		{
			RefreshDomains();
			AddDefaultRow();
			PopulateModeCombobox(dgvBlastGenerator.Rows[0]);
			OpenedFromBlastEditor = false;
			btnSendTo.Text = "Send to Stash";
			initialized = true;

			this.Show();
			this.BringToFront();
		}

		public void LoadStashkey(StashKey _sk)
		{
			if (_sk == null)
				return;

			sk = (StashKey)_sk.Clone();
			sk.BlastLayer = new BlastLayer();

			RefreshDomains();
			AddDefaultRow();
			PopulateModeCombobox(dgvBlastGenerator.Rows[0]);
			OpenedFromBlastEditor = true;
			btnSendTo.Text = "Send to Blast Editor";
			initialized = true;

			this.Show();
			this.BringToFront();
		}

		private void AddDefaultRow()
		{
			try
			{
				//Add an empty row and populate with default values
				dgvBlastGenerator.Rows.Add();
				int lastrow = dgvBlastGenerator.RowCount - 1;
				//Set up the DGV based on the current state of Bizhawk
				(dgvBlastGenerator.Rows[lastrow].Cells["dgvRowDirty"]).Value = true;


				//For some godforsaken reason, xmlSerializer deserialization wont fill this in as a bool so just use a string god help us all 
				(dgvBlastGenerator.Rows[lastrow].Cells["dgvEnabled"]).ValueType = typeof(string);
				((DataGridViewCheckBoxCell)(dgvBlastGenerator.Rows[lastrow].Cells["dgvEnabled"])).TrueValue = "true";
				((DataGridViewCheckBoxCell)(dgvBlastGenerator.Rows[lastrow].Cells["dgvEnabled"])).FalseValue = "false";
				(dgvBlastGenerator.Rows[lastrow].Cells["dgvEnabled"]).Value = "true";

				((DataGridViewComboBoxCell)dgvBlastGenerator.Rows[lastrow].Cells["dgvPrecision"]).Value = ((DataGridViewComboBoxCell)dgvBlastGenerator.Rows[0].Cells["dgvPrecision"]).Items[0];
				((DataGridViewComboBoxCell)dgvBlastGenerator.Rows[lastrow].Cells["dgvType"]).Value = ((DataGridViewComboBoxCell)dgvBlastGenerator.Rows[0].Cells["dgvType"]).Items[0];


				//We need to make the rows type decimal as the NumericUpDown is formatted as string by default (due to the potential for commas)
				dgvBlastGenerator.Rows[lastrow].Cells["dgvStartAddress"].ValueType = typeof(System.Decimal);
				dgvBlastGenerator.Rows[lastrow].Cells["dgvEndAddress"].ValueType = typeof(System.Decimal);
				dgvBlastGenerator.Rows[lastrow].Cells["dgvParam1"].ValueType = typeof(System.Decimal);
				dgvBlastGenerator.Rows[lastrow].Cells["dgvParam2"].ValueType = typeof(System.Decimal);
				dgvBlastGenerator.Rows[lastrow].Cells["dgvLifetime"].ValueType = typeof(System.Decimal);
				dgvBlastGenerator.Rows[lastrow].Cells["dgvExecuteFrame"].ValueType = typeof(System.Decimal);
				dgvBlastGenerator.Rows[lastrow].Cells["dgvSeed"].ValueType = typeof(System.Decimal);


				//These can't be null or else things go bad when trying to save and load them from a file. Include an M as they NEED to be decimal.
				((DataGridViewNumericUpDownCell)dgvBlastGenerator.Rows[lastrow].Cells["dgvStartAddress"]).Value = 0M;
				((DataGridViewNumericUpDownCell)dgvBlastGenerator.Rows[lastrow].Cells["dgvEndAddress"]).Value = 1M;
				((DataGridViewNumericUpDownCell)dgvBlastGenerator.Rows[lastrow].Cells["dgvParam1"]).Value = 0M;
				((DataGridViewNumericUpDownCell)dgvBlastGenerator.Rows[lastrow].Cells["dgvParam2"]).Value = 0M;
				((DataGridViewNumericUpDownCell)dgvBlastGenerator.Rows[lastrow].Cells["dgvEndAddress"]).Value = 1M;
				((DataGridViewNumericUpDownCell)dgvBlastGenerator.Rows[lastrow].Cells["dgvLifetime"]).Value = 1M;
				((DataGridViewNumericUpDownCell)dgvBlastGenerator.Rows[lastrow].Cells["dgvExecuteFrame"]).Value = 0M;


				//Generate a random seed
				((DataGridViewTextBoxCell)dgvBlastGenerator.Rows[lastrow].Cells["dgvSeed"]).Value = CorruptCore.CorruptCore.RND.Next();


				PopulateDomainCombobox(dgvBlastGenerator.Rows[lastrow]);
				PopulateModeCombobox(dgvBlastGenerator.Rows[lastrow]);
				// (dgvBlastGenerator.Rows[lastrow].Cells["dgvMode"] as DataGridViewComboBoxCell).Value = (dgvBlastGenerator.Rows[0].Cells["dgvMode"] as DataGridViewComboBoxCell).Items[0];


				//For some reason, setting the minimum on the DGV to 1 doesn't change the fact it inserts with a count of 0
				(dgvBlastGenerator.Rows[lastrow].Cells["dgvInterval"]).Value = 1;
			}
			catch (Exception ex)
			{
				MessageBox.Show(
					"An error occurred in RTC while adding a new row.\n\n" +
					"Your session is probably broken\n\n\n" +
					ex.ToString() + ex.StackTrace);
			}
		}

		private bool PopulateDomainCombobox(DataGridViewRow row)
		{
			try
			{
				if (row.Cells["dgvDomain"] is DataGridViewComboBoxCell)
				{
					DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)row.Cells["dgvDomain"];
					object currentValue = cell.Value;

					cell.Value = null;
					cell.Items.Clear();

					foreach (string domain in domains)
					{
						cell.Items.Add(domain);
					}


					if (currentValue != null && cell.Items.Contains(currentValue))
						cell.Value = currentValue;
					else if (cell.Items.Count > 0)
						cell.Value = cell.Items[0];
					else
						cell.Value = null;

				}

				UpdateAddressRange(row);

				return true;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private static void UpdateAddressRange(DataGridViewRow row)
		{
				if (row.Cells["dgvDomain"].Value == null)
					return;

				if (!domainToMiDico.ContainsKey(row.Cells["dgvDomain"]
					.Value.ToString()))
				{
					MessageBox.Show("Couldn't find the selected domain " + row.Cells["dgvDomain"]
						.Value.ToString() + ".\nAre you sure you have the right core loaded?");
				}
				

				long size = domainToMiDico[row.Cells["dgvDomain"].Value.ToString()].Size;

				((DataGridViewNumericUpDownCell)row.Cells["dgvStartAddress"]).Maximum = size;
				((DataGridViewNumericUpDownCell)row.Cells["dgvEndAddress"]).Maximum = size;
		}

		private static void PopulateModeCombobox(DataGridViewRow row)
		{
			DataGridViewComboBoxCell cell = row.Cells["dgvMode"] as DataGridViewComboBoxCell;

			if (cell != null)
			{
				cell.Value = null;
				cell.Items.Clear();


				switch (row.Cells["dgvType"].Value.ToString())
				{
					case "Value":
						foreach (BGValueModes type in Enum.GetValues(typeof(BGValueModes)))
						{
							cell.Items.Add(type.ToString());
						}
						break;
					case "Store":
						foreach (BGStoreModes type in Enum.GetValues(typeof(BGStoreModes)))
						{
							cell.Items.Add(type.ToString());
						}
						break;
					default:
						break;
				}
				cell.Value = cell.Items[0];
			}
		}

		private void btnJustCorrupt_Click(object sender, EventArgs e)
		{
			try
			{
				btnLoadCorrupt.Enabled = false;
				btnSendTo.Enabled = false;
				btnJustCorrupt.Enabled = false;

				GenerateBlastLayers(false, true);
				//LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.APPLYBLASTLAYER, new object[] { bl?.Clone() as BlastLayer, true }, true);
			}
			finally
			{

				btnLoadCorrupt.Enabled = true;
				btnSendTo.Enabled = true;
				btnJustCorrupt.Enabled = true;

			}
		}

		private void btnLoadCorrupt_Click(object sender, EventArgs e)
		{
			try
			{ 
				btnLoadCorrupt.Enabled = false;
				btnSendTo.Enabled = false;
				btnJustCorrupt.Enabled = false;

				StashKey newSk = null;
				if (sk == null)
				{
					StashKey psk = StockpileManager_UISide.GetCurrentSavestateStashkey();
					if (psk == null)
					{
						MessageBox.Show(
							"Could not perform the CORRUPT action\n\nEither no Savestate Box was selected in the Savestate Manager\nor the Savetate Box itself is empty.");
						return;
					}

					newSk = new StashKey(CorruptCore.CorruptCore.GetRandomKey(), psk.ParentKey, null)
					{
						RomFilename = psk.RomFilename, SystemName = psk.SystemName, SystemCore = psk.SystemCore
						, GameName = psk.GameName, SyncSettings = psk.SyncSettings, StateLocation = psk.StateLocation
					};
				}
				else
					newSk = (StashKey) sk.Clone();

				BlastLayer bl = GenerateBlastLayers(true, true);
				if (bl == null)
					return;
				newSk.BlastLayer = bl;
			}
			finally
			{
				btnLoadCorrupt.Enabled = true;
				btnSendTo.Enabled = true;
				btnJustCorrupt.Enabled = true;
			}
		}

		private void btnSendTo_Click(object sender, EventArgs e)
		{

			btnLoadCorrupt.Enabled = false;
			btnSendTo.Enabled = false;
			btnJustCorrupt.Enabled = false;
			try
			{
				StashKey newSk = null;
				if (sk == null)
				{
					StashKey psk = StockpileManager_UISide.GetCurrentSavestateStashkey();
					if (psk == null)
					{
						MessageBox.Show("Could not perform the CORRUPT action\n\nEither no Savestate Box was selected in the Savestate Manager\nor the Savetate Box itself is empty.");
						return;
					}
					newSk = new StashKey(CorruptCore.CorruptCore.GetRandomKey(), psk.ParentKey, null);
					newSk.RomFilename = psk.RomFilename;
					newSk.SystemName = psk.SystemName;
					newSk.SystemCore = psk.SystemCore;
					newSk.GameName = psk.GameName;
					newSk.SyncSettings = psk.SyncSettings;
				}
				else
				{
					newSk = (StashKey)sk.Clone();
				}

				BlastLayer bl = GenerateBlastLayers(true);
				if (bl == null)
					return;
				newSk.BlastLayer = bl;
				if (OpenedFromBlastEditor)
				{
					if (S.GET<RTC_NewBlastEditor_Form>() == null || S.GET<RTC_NewBlastEditor_Form>().IsDisposed)
					{
						S.SET(new RTC_NewBlastEditor_Form());
						S.GET<RTC_NewBlastEditor_Form>().LoadStashkey((StashKey)newSk.Clone());
					}
					else
						S.GET<RTC_NewBlastEditor_Form>().ImportBlastLayer(newSk.BlastLayer);
					{

					}
					S.GET<RTC_NewBlastEditor_Form>().Show();
				}
				else
				{
					StockpileManager_UISide.StashHistory.Add(newSk);
					S.GET<RTC_GlitchHarvester_Form>().RefreshStashHistory();
					S.GET<RTC_GlitchHarvester_Form>().dgvStockpile.ClearSelection();
					S.GET<RTC_GlitchHarvester_Form>().lbStashHistory.ClearSelected();

					S.GET<RTC_GlitchHarvester_Form>().DontLoadSelectedStash = true;
					S.GET<RTC_GlitchHarvester_Form>().lbStashHistory.SelectedIndex = S.GET<RTC_GlitchHarvester_Form>().lbStashHistory.Items.Count - 1;
					StockpileManager_UISide.CurrentStashkey = StockpileManager_UISide.StashHistory[S.GET<RTC_GlitchHarvester_Form>().lbStashHistory.SelectedIndex];
				}
			}
			finally
			{
				btnLoadCorrupt.Enabled = true;
				btnSendTo.Enabled = true;
				btnJustCorrupt.Enabled = true;
			}
		}


		private void dgvBlastGenerator_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (!initialized || dgvBlastGenerator == null)
				return;

			if (e.ColumnIndex != (int)BlastGeneratorColumn.DgvRowDirty)
				dgvBlastGenerator.Rows[e.RowIndex].Cells["dgvRowDirty"].Value = true;

			if ((BlastGeneratorColumn)e.ColumnIndex == BlastGeneratorColumn.DgvType)
			{
				PopulateModeCombobox(dgvBlastGenerator.Rows[e.RowIndex]);
			}
			if ((BlastGeneratorColumn)e.ColumnIndex == BlastGeneratorColumn.DgvDomain)
			{
				UpdateAddressRange(dgvBlastGenerator.Rows[e.RowIndex]);
			}
		}

		private void dgvBlastGenerator_MouseClick(object sender, MouseEventArgs e)
		{
			int currentMouseOverColumn = dgvBlastGenerator.HitTest(e.X, e.Y).ColumnIndex;
			int currentMouseOverRow = dgvBlastGenerator.HitTest(e.X, e.Y).RowIndex;

			if (e.Button == MouseButtons.Left)
			{
				if (currentMouseOverRow == -1)
				{
					dgvBlastGenerator.EndEdit();
					dgvBlastGenerator.ClearSelection();
				}
			}
		}


		public BlastLayer GenerateBlastLayers(bool loadBeforeCorrupt = false, bool applyAfterCorrupt = false)
		{
			StashKey newSk = null;
			try
			{
				RefreshDomains();

				BlastLayer bl = new BlastLayer();

				if (loadBeforeCorrupt)
				{
					//If opened from engine config, use the GH state
					if (!OpenedFromBlastEditor)
					{
						StashKey psk = StockpileManager_UISide.GetCurrentSavestateStashkey();
						if (psk == null)
						{
							MessageBox.Show(
								"The Blast Generator could not perform the CORRUPT action\n\nEither no Savestate Box was selected in the Savestate Manager\nor the Savetate Box itself is empty.");
							return null;
						}
						newSk = new StashKey(CorruptCore.CorruptCore.GetRandomKey(), psk.ParentKey, bl);
						newSk.RomFilename = psk.RomFilename;
						newSk.SystemName = psk.SystemName;
						newSk.SystemCore = psk.SystemCore;
						newSk.GameName = psk.GameName;
						newSk.SyncSettings = psk.SyncSettings;
					}
					else
						newSk = (StashKey)sk.Clone();
				}

				List<BlastGeneratorProto> protoList = new List<BlastGeneratorProto>();

				foreach (DataGridViewRow row in dgvBlastGenerator.Rows)
				{
					BlastGeneratorProto proto = null;
					//Why the hell can't you get the checked state from a dgvCheckbox why do I need to make this a string aaaaaaaaaaa
					if (row.Cells["dgvEnabled"].Value.ToString() == "true" ||
						row.Cells["dgvEnabled"].Value.ToString() == "True")
					{
						if ((bool)row.Cells["dgvRowDirty"].Value == true)
						{
							proto = CreateProtoFromRow(row);
							if (proto != null)
								row.Cells["dgvBlastProtoReference"].Value = proto;
						}
						else
						{
							proto = (BlastGeneratorProto)row.Cells["dgvBlastProtoReference"].Value;
						}
					}
					protoList.Add(proto);
				}


				List<BlastGeneratorProto> returnList = new List<BlastGeneratorProto>();

				returnList = NetCore.LocalNetCoreRouter.QueryRoute<List<BlastGeneratorProto>>(NetCore.NetcoreCommands.CORRUPTCORE, NetCore.NetcoreCommands.BLASTGENERATOR_BLAST, new object[] { newSk, protoList, loadBeforeCorrupt, applyAfterCorrupt }, true);

				if (returnList == null)
					return null;

				if (returnList.Count != protoList.Count)
					throw (new Exception("Got less protos back compared to protos sent. Aborting!"));

				//The return list is in the same order as the original list so we can go by index here
				for (int i = 0; i < dgvBlastGenerator.RowCount; i++)
				{
					//Why the hell can't you get the checked state from a dgvCheckbox why do I need to make this a string aaaaaaaaaaa
					if (dgvBlastGenerator.Rows[i].Cells["dgvEnabled"].Value.ToString() == "true" || dgvBlastGenerator.Rows[i].Cells["dgvEnabled"].Value.ToString() == "True")
					{
						dgvBlastGenerator.Rows[i].Cells["dgvBlastProtoReference"].Value = returnList[i];
						dgvBlastGenerator.Rows[i].Cells["dgvRowDirty"].Value = false;

						bl.Layer.AddRange(returnList[i].bl.Layer);
					}
				}
				return bl;
			}
			catch (Exception ex)
			{
				MessageBox.Show("Something went wrong when generating the blastlayers. \n" +
								"Are you sure your input is correct and you have the correct core loaded?\n\n" +
								ex.ToString() +
								ex.StackTrace);
				return null;
			}
			finally
			{
			}
		}

		//We don't always know how the parameters are going to be used
		//Because of this, we create the prototype using the raw value and flip the bytes as needed inside the generator
		//An example would be that the param could be an address, or a value
		//If we flipped to big endian here, that'd goof the handling of addresses
		//As such, we always create the prototype with the raw value.
		private BlastGeneratorProto CreateProtoFromRow(DataGridViewRow row)
		{
			try
			{
				string note;
				if (cbUnitsShareNote.Checked)
				{
					var value = row.Cells["dgvNoteText"].Value;
					if (value != null)
						note = value.ToString();
					else
						note = String.Empty;
				}
				else
					note = String.Empty;


				string domain = row.Cells["dgvDomain"].Value.ToString();
				string type = row.Cells["dgvType"].Value.ToString();
				string mode = row.Cells["dgvMode"].Value.ToString();
				int precision = GetPrecisionSizeFromName(row.Cells["dgvPrecision"].Value.ToString());
				int interval = Convert.ToInt32(row.Cells["dgvInterval"].Value);
				long startAddress = Convert.ToInt64(row.Cells["dgvStartAddress"].Value);
				long endAddress = Convert.ToInt64(row.Cells["dgvEndAddress"].Value);
				long param1 = Convert.ToInt64(row.Cells["dgvParam1"].Value);
				long param2 = Convert.ToInt64(row.Cells["dgvParam2"].Value);
				int lifetime = Convert.ToInt32(row.Cells["dgvLifetime"].Value);
				int executeframe = Convert.ToInt32(row.Cells["dgvExecuteFrame"].Value);
				bool loop = Convert.ToBoolean(row.Cells["dgvLoop"].Value);
				int seed = Convert.ToInt32(row.Cells["dgvSeed"].Value);

				return new BlastGeneratorProto(note, type, domain, mode, precision, interval, startAddress, endAddress, param1, param2, lifetime, executeframe, loop, seed);
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private string GetPrecisionNameFromSize(int precision)
		{
			switch (precision)
			{
				case 1:
					return "8-bit";

				case 2:
					return "16-bit";

				case 4:
					return "32-bit";

				default:
					return null;
			}
		}

		private int GetPrecisionSizeFromName(string precision)
		{
			switch (precision)
			{
				case "8-bit":
					return 1;

				case "16-bit":
					return 2;

				case "32-bit":
					return 4;

				default:
					return -1;
			}
		}

		private void btnAddRow_Click(object sender, EventArgs e)
		{
			AddDefaultRow();
		}

		private void btnNudgeStartAddressUp_Click(object sender, EventArgs e)
		{
			NudgeParams("dgvStartAddress", updownNudgeStartAddress.Value);
		}

		private void btnNudgeStartAddressDown_Click(object sender, EventArgs e)
		{
			NudgeParams("dgvStartAddress", updownNudgeStartAddress.Value, true);
		}

		private void btnNudgeEndAddressUp_Click(object sender, EventArgs e)
		{
			NudgeParams("dgvEndAddress", updownNudgeEndAddress.Value);
		}

		private void btnNudgeEndAddressDown_Click(object sender, EventArgs e)
		{
			NudgeParams("dgvEndAddress", updownNudgeEndAddress.Value, true);
		}

		private void btnNudgeParam1Up_Click(object sender, EventArgs e)
		{
			NudgeParams("dgvParam1", updownNudgeParam1.Value);
		}

		private void btnNudgeParam1Down_Click(object sender, EventArgs e)
		{
			NudgeParams("dgvParam1", updownNudgeParam1.Value, true);
		}

		private void btnNudgeParam2Up_Click(object sender, EventArgs e)
		{
			NudgeParams("dgvParam2", updownNudgeParam2.Value);
		}

		private void btnNudgeParam2Down_Click(object sender, EventArgs e)
		{
			NudgeParams("dgvParam2", updownNudgeParam2.Value, true);
		}

		private void NudgeParams(string column, decimal amount, bool shiftDown = false)
		{
			if (shiftDown)
				foreach (DataGridViewRow selected in dgvBlastGenerator.SelectedRows)
				{
					if ((Convert.ToDecimal(selected.Cells[column].Value) - amount) >= 0)

						selected.Cells[column].Value = Convert.ToDecimal(selected.Cells[column].Value) - amount;
					else
						selected.Cells[column].Value = 0;
				}
			else
			{
				foreach (DataGridViewRow selected in dgvBlastGenerator.SelectedRows)
				{
					decimal max = ((DataGridViewNumericUpDownCell)selected.Cells[column]).Maximum;

					if ((Convert.ToDecimal(selected.Cells[column].Value) - amount) <= max)
						selected.Cells[column].Value = Convert.ToDecimal(selected.Cells[column].Value) + amount;
					else
						selected.Cells[column].Value = max;
				}
			}
		}

		private void btnHideSidebar_Click(object sender, EventArgs e)
		{
			if (btnHideSidebar.Text == "▶")
			{
				panelSidebar.Visible = false;
				btnHideSidebar.Text = "◀";
			}
			else
			{
				panelSidebar.Visible = true;
				btnHideSidebar.Text = "▶";
			}
		}

		private bool RefreshDomains()
		{
			try
			{
				S.GET<RTC_MemoryDomains_Form>().RefreshDomainsAndKeepSelected();
				domainToMiDico.Clear();
				domains = MemoryDomains.MemoryInterfaces.Keys.Concat(MemoryDomains.VmdPool.Values.Select(it => it.ToString())).ToArray();
				foreach (string domain in domains)
				{
					domainToMiDico.Add(domain, MemoryDomains.GetInterface(domain));
				}

				foreach (DataGridViewRow row in dgvBlastGenerator.Rows)
					PopulateDomainCombobox(row);
				return true;
			}
			catch (Exception ex)
			{
				throw new Exception(
							"An error occurred in RTC while refreshing the domains\n" +
							"Are you sure you don't have an invalid domain selected?\n" +
							"Make sure any VMDs are loaded and you have the correct core loaded in Bizhawk\n" +
							ex.ToString() + ex.StackTrace);
				return false;
			}
		}

		private void btnRefreshDomains_Click(object sender, EventArgs e)
		{
			RefreshDomains();
		}

		private void SaveDataGridView(DataGridView dgv)
		{
			DataTable dt = new DataTable();
			foreach (DataGridViewColumn column in dgv.Columns)
			{
				dt.Columns.Add();
			}

			object[] cellValues = new object[dgv.Columns.Count];
			foreach (DataGridViewRow row in dgv.Rows)
			{
				for (int i = 0; i < row.Cells.Count; i++)
				{
					cellValues[i] = row.Cells[i].Value;
				}
				dt.Rows.Add(cellValues);
			}

			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Filter = "bg|*.bg";
			if (sfd.ShowDialog() == DialogResult.OK)
			{
				try
				{
					var settings = new JsonSerializerSettings
					{
						//NullValueHandling = NullValueHandling.Ignore,
					};
					var s = JsonConvert.SerializeObject(dt, settings);
					File.WriteAllText(sfd.FileName, s);

				}
				catch (Exception ex)
				{
					Console.WriteLine(ex);
				}
			}
		}

		private bool importDataGridView(DataGridView dgv)
		{
			return loadDataGridView(dgv, true);
		}

		private bool loadDataGridView(DataGridView dgv, bool import = false)
		{
			OpenFileDialog ofd = new OpenFileDialog
			{
				Filter = "bg|*.bg"
			};
			if (ofd.ShowDialog() == DialogResult.OK)
			{
				try
				{
					DataTable dt = null;
					var settings = new JsonSerializerSettings
					{
					//	NullValueHandling = NullValueHandling.Ignore,
					};
					dt = JsonConvert.DeserializeObject<DataTable>(File.ReadAllText(ofd.FileName), settings);
					if (!import)
						dgv.Rows.Clear();

					foreach (DataRow row in dt.Rows)
					{
						dgv.Rows.Add();

						int lastrow = dgvBlastGenerator.RowCount - 1;

						//We need to populate the comboboxes or else things go bad
						//To do this, we load the type first so we can populate the modes, then add the domain to the domain combobox.
						//If it's invalid, they'll know on generation. If they load the correct core and press refresh, it'll maintain its selection
						dgv[(int)BlastGeneratorColumn.DgvType, lastrow].Value = row.ItemArray[(int)BlastGeneratorColumn.DgvType];

						PopulateModeCombobox(dgv.Rows[lastrow]);

						//If the domain doesn't exist, cancel out
						if (!domains.Contains(row.ItemArray[(int)BlastGeneratorColumn.DgvDomain].ToString()))
						{
							MessageBox.Show("Skipping row as domain couldn't be found! Are you sure you have the right core and all VMDs loaded?");
							continue;
						}

						(dgv.Rows[lastrow].Cells["dgvDomain"] as DataGridViewComboBoxCell)?.Items.Add(row.ItemArray[(int)BlastGeneratorColumn.DgvDomain]);

						for (int i = 0; i < dgv.Rows[lastrow].Cells.Count; i++)
						{
							var item = row.ItemArray[i];
							if(item is DBNull)
								dgv.Rows[lastrow].Cells[i].Value = 0;
							else
								dgv.Rows[lastrow].Cells[i].Value = item;

						}
						//Override these two
						dgv[(int)BlastGeneratorColumn.dgvBlastProtoReference, lastrow].Value = null;
						dgv[(int)BlastGeneratorColumn.DgvRowDirty, lastrow].Value = true;
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.ToString() + ex.StackTrace);
					return false;
				}
			}
			return true;
		}

		private void loadFromFileblToolStripMenuItem_Click(object sender, EventArgs e)
		{
			loadDataGridView(dgvBlastGenerator);
		}

		private void saveAsToFileblToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveDataGridView(dgvBlastGenerator);
		}

		private void importBlastlayerblToolStripMenuItem_Click(object sender, EventArgs e)
		{
			importDataGridView(dgvBlastGenerator);
		}

		private void btnHelp_Click(object sender, EventArgs e)
		{
			System.Diagnostics.ProcessStartInfo sInfo = new System.Diagnostics.ProcessStartInfo("https://corrupt.wiki/corruptors/rtc-real-time-corruptor/blast-generator.html");
			System.Diagnostics.Process.Start(sInfo);
		}

		private void dgvBlastGenerator_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			// Note handling
			if (e != null)
			{
				DataGridView senderGrid = (DataGridView)sender;

				if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
					e.RowIndex >= 0)
				{
					{
						DataGridViewCell textCell = dgvBlastGenerator.Rows[e.RowIndex].Cells["dgvNoteText"];
						DataGridViewCell buttonCell = dgvBlastGenerator.Rows[e.RowIndex].Cells["dgvNoteButton"];

						NoteItem note = new NoteItem(textCell.Value == null ? "" : textCell.Value.ToString());
						textCell.Value = note;
						S.SET(new RTC_NoteEditor_Form(note, buttonCell));
						S.GET<RTC_NoteEditor_Form>().Show();
						return;
					}
				}
			}
		}

		private void DgvBlastGenerator_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				if (e.RowIndex == -1)
				{
					dgvBlastGenerator.EndEdit();
					dgvBlastGenerator.ClearSelection();
				}
			}
		}
		public void RefreshNoteIcons()
		{
			foreach (DataGridViewRow row in dgvBlastGenerator.Rows)
			{
				DataGridViewCell textCell = row.Cells["dgvNoteText"];
				DataGridViewCell buttonCell = row.Cells["dgvNoteButton"];

				buttonCell.Value = string.IsNullOrWhiteSpace(textCell.Value?.ToString()) ? string.Empty : "📝";
			}
		}

		private void CbUnitsShareNote_CheckedChanged(object sender, EventArgs e)
		{
			//mark the rows as dirty
			foreach (DataGridViewRow row in dgvBlastGenerator.Rows)
			{
				row.Cells["dgvRowDirty"].Value = true;
			}
		}

		public bool IsUserEditing()
		{
			if (dgvBlastGenerator.IsCurrentCellInEditMode)
				return true;
			foreach (var control in allControls)
			{
				//We assume focus on a TB or UpDown is edit mode
				if (control is TextBox || control is NumericUpDown)
				{
					if (control.Focused)
						return true;
				}
			}
			return false;
		}

		public StashKey[] GetStashKeys()
		{
			return new[] {sk};
		}
	}
	class NoteItem : INote
	{
		public string Note { get; set; }
		public NoteItem(string note)
		{
			Note = note;
		}
		public override string ToString()
		{
			return Note;
		}
	}
}
