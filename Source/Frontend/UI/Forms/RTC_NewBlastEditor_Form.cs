using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json.Serialization;
using RTCV.CorruptCore;
using RTCV.NetCore;
using static RTCV.UI.UI_Extensions;
using RTCV.NetCore.StaticTools;

/**
 * The DataGridView is bound to the blastlayer
 * All validation is done within the dgv
 * The boxes at the bottom are unbound and manipulate the selected rows in the dgv, and thus, the validation is handled by the dgv
 * No maxmimum is set in the numericupdowns at the bottom as the dgv validates
 **/

/*
Applies in all cases & should be editable
 * bool IsEnabled 
 * bool IsLocked
 * 
 * string Domain 
 * long Address 
 * int Precision 
 * BlastUnitSource Source 

 * BigInteger TiltValue 
 * 
 * int ExecuteFrame 
 * int Lifetime 
 * bool Loop 
 * 
 * ActionTime LimiterTime 
 * string LimiterListHash 
 * bool InvertLimiter 
 *
 * string Note 


Applies for Store & should be editable
 * ActionTime StoreTime 
 * StoreType StoreType 
 * string SourceDomain 
 * long SourceAddress 


Applies for Value & should be editable
 * byte[] Value */

namespace RTCV.UI
{
	public partial class RTC_NewBlastEditor_Form : Form, IAutoColorize
	{

		private static Dictionary<string, MemoryInterface> _domainToMiDico;
		private static Dictionary<string, MemoryInterface> domainToMiDico
		{
			get => _domainToMiDico ?? (_domainToMiDico = new Dictionary<string, MemoryInterface>());
			set => _domainToMiDico = value;
		}
		private string[] domains = null;
		private string searchValue, searchColumn;
		public List<String> VisibleColumns;
		private string CurrentBlastLayerFile = "";
		private bool batchOperation = false;

		private int searchOffset = 0;
		private IEnumerable<BlastUnit> searchEnumerable;
		BindingList<BlastUnit> selectedBUs = new BindingList<BlastUnit>();
		ContextMenuStrip headerStrip;
		ContextMenuStrip cms;

		Dictionary<String, Control> property2ControlDico;

		private string a = null;
		int buttonFillWeight = 20;
		int checkBoxFillWeight = 25;
		int comboBoxFillWeight = 40;
		int textBoxFillWeight = 30;
		int numericUpDownFillWeight = 35;

		private enum buProperty
		{
			isEnabled,
			isLocked,
			Domain,
			Address,
			Precision,
			ValueString,
			Source,
			ExecuteFrame,
			Lifetime,
			Loop,
			LimiterTime,
			LimiterListHash,
			InvertLimiter,
			StoreTime,
			StoreLimiterSource,
			StoreType,
			SourceDomain,
			SourceAddress,
			Note
		}
		//We gotta cache this stuff outside of the scope of InitializeDGV
		//	private object actionTimeValues = 


		public RTC_NewBlastEditor_Form()
		{
			try
			{
				InitializeComponent();

				dgvBlastEditor.DataError += dgvBlastLayer_DataError;
				dgvBlastEditor.AutoGenerateColumns = false;
				dgvBlastEditor.SelectionChanged += dgvBlastEditor_SelectionChanged;
				dgvBlastEditor.ColumnHeaderMouseClick += dgvBlastEditor_ColumnHeaderMouseClick;
				dgvBlastEditor.CellValueChanged += dgvBlastEditor_CellValueChanged;
				dgvBlastEditor.CellMouseClick += dgvBlastEditor_CellMouseClick;
				dgvBlastEditor.CellMouseDoubleClick += dgvBlastEditor_CellMouseDoubleClick;
				dgvBlastEditor.RowsAdded += DgvBlastEditor_RowsAdded;
				dgvBlastEditor.RowsRemoved += DgvBlastEditor_RowsRemoved;
				dgvBlastEditor.CellFormatting += DgvBlastEditor_CellFormatting;
				dgvBlastEditor.MouseClick += DgvBlastEditor_Click;

				cbFilterColumn.SelectedValueChanged += (o, e) => { tbFilter_TextChanged(null, null); };
				tbFilter.TextChanged += tbFilter_TextChanged;

				cbEnabled.Validated += cbEnabled_Validated;
				cbLocked.Validated += CbLocked_Validated;
				cbBigEndian.Validated += CbBigEndian_Validated;
				cbLoop.Validated += CbLoop_Validated;

				cbDomain.Validated += cbDomain_Validated;
				upDownAddress.Validated += UpDownAddress_Validated;
				upDownPrecision.Validated += UpDownPrecision_Validated;
				tbTiltValue.Validated += TbTiltValue_Validated;


				upDownExecuteFrame.Validated += UpDownExecuteFrame_Validated;
				upDownLifetime.Validated += UpDownLifetime_Validated;

				cbSource.Validated += CbSource_Validated;
				tbValue.Validated += TbValue_Validated;

				cbInvertLimiter.Validated += CbInvertLimiter_Validated;
				cbLimiterTime.Validated += CbLimiterTime_Validated;
				cbStoreLimiterSource.Validated += cbStoreLimiterSource_Validated;
				cbLimiterList.Validated += CbLimiterList_Validated;

				upDownSourceAddress.Validated += UpDownSourceAddress_Validated;
				cbStoreTime.Validated += CbStoreTime_Validated;
				cbStoreType.Validated += CbStoreType_Validated;
				cbSourceDomain.Validated += CbSourceDomain_Validated;

				registerValueStringScrollEvents();


				//On today's episode of "why is the designer overriding these values every time I build"
				upDownExecuteFrame.Maximum = Int32.MaxValue;
				upDownPrecision.Maximum = 16348; //Textbox doesn't like more than ~20k
				upDownLifetime.Maximum = Int32.MaxValue;
				upDownSourceAddress.Maximum = Int32.MaxValue;
				upDownAddress.Maximum = Int32.MaxValue;

				this.FormClosed += RTC_NewBlastEditorForm_Close;
				this.FormClosing += RTC_NewBlastEditorForm_Closing;

			}
			catch(Exception ex)
			{
				string additionalInfo = "An error occurred while opening the BlastEditor Form\n\n";

				var ex2 = new CustomException(ex.Message, additionalInfo + ex.StackTrace);

				if (CloudDebug.ShowErrorDialog(ex2, true) == DialogResult.Abort)
					throw new RTCV.NetCore.AbortEverythingException();

			}
		}

		private void RTC_NewBlastEditorForm_Load(object sender, EventArgs e)
		{
			UICore.SetRTCColor(UICore.GeneralColor, this);
			domains = MemoryDomains.MemoryInterfaces?.Keys?.Concat(MemoryDomains.VmdPool.Values.Select(it => it.ToString())).ToArray();


			dgvBlastEditor.AllowUserToOrderColumns = true;
			SetDisplayOrder();
		}
		private void RTC_NewBlastEditorForm_Closing(object sender, FormClosingEventArgs e)
		{
			SaveDisplayOrder();
		}
		private void RTC_NewBlastEditorForm_Close(object sender, FormClosedEventArgs e)
		{
			//Clean up
			bs = null;
			_bs = null;
			currentSK = null;
			originalSK = null;
			domainToMiDico = null;
			//Force cleanup
			GC.Collect();
			GC.WaitForPendingFinalizers();
			this.Dispose();
		}



		private void registerValueStringScrollEvents()
		{
			tbValue.MouseWheel += tbValueScroll;
			dgvBlastEditor.MouseWheel += DgvBlastEditor_MouseWheel;
		}
	
		private void DgvBlastEditor_MouseWheel(object sender, MouseEventArgs e)
		{
			var owningRow = dgvBlastEditor.CurrentCell.OwningRow;


			if (dgvBlastEditor.CurrentCell == owningRow.Cells[buProperty.ValueString.ToString()] && dgvBlastEditor.IsCurrentCellInEditMode)
			{
				int precision = (int)dgvBlastEditor.CurrentCell.OwningRow.Cells[buProperty.Precision.ToString()].Value;
				dgvCellValueScroll(dgvBlastEditor.EditingControl, e, precision);

				((HandledMouseEventArgs)e).Handled = true;
			}

		}

		private void dgvCellValueScroll(object sender, MouseEventArgs e, int precision)
		{
			if (sender is TextBox tb)
			{
				var negative = (e.Delta < 0);
				var scrollBy = 1;
				if (negative)
					scrollBy *= -1;
				tb.Text = getShiftedHexString(tb.Text, scrollBy, precision);
			}
		}
		private void tbValueScroll(object sender, MouseEventArgs e)
		{
			if (sender is TextBox tb)
			{
				tb.Text = getShiftedHexString(tb.Text, e.Delta / SystemInformation.MouseWheelScrollDelta, Convert.ToInt32(upDownPrecision.Value));
			}
		}


		private void SetDisplayOrder()
		{
			if (!Params.IsParamSet("BLASTEDITOR_COLUMN_ORDER"))
				return;
			//Names split with commas
			var s = Params.ReadParam("BLASTEDITOR_COLUMN_ORDER");
			var order = s.Split(',');

			//Use a foreach and keep track in-case the number of entries changes
			int i = 0;
			foreach (var c in order)
			{
				if (dgvBlastEditor.Columns.Cast<DataGridViewColumn>().Any(x => x.Name == c))
				{
					dgvBlastEditor.Columns[c].DisplayIndex = i;
					i++;
				}
			}
		}

		private void SaveDisplayOrder()
		{
			var cols = dgvBlastEditor.Columns.Cast<DataGridViewColumn>().OrderBy(x => x.DisplayIndex);
			StringBuilder sb = new StringBuilder();
			foreach (var c in cols)
				sb.Append(c.Name + ",");
			Params.SetParam("BLASTEDITOR_COLUMN_ORDER", sb.ToString());
		}

		private void DgvBlastEditor_Click(object sender, MouseEventArgs e)
		{
			//Exit edit mode if you click away from a cell
			var ht = dgvBlastEditor.HitTest(e.X, e.Y);

			if (ht.Type != DataGridViewHitTestType.Cell)
				dgvBlastEditor.EndEdit();

		}

		private void dgvBlastEditor_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			// Note handling
			if (e != null && e.RowIndex != -1)
			{
				if (e.ColumnIndex == dgvBlastEditor.Columns[buProperty.Note.ToString()]?.Index)
				{
					BlastUnit bu = dgvBlastEditor.Rows[e.RowIndex].DataBoundItem as BlastUnit;
					if (bu != null)
					{
						S.SET(new RTC_NoteEditor_Form(bu, dgvBlastEditor[e.ColumnIndex, e.RowIndex]));
						S.GET<RTC_NoteEditor_Form>().Show();
					}
				}
			}

			if (e.Button == MouseButtons.Left)
			{
				if (e.RowIndex == -1)
				{
					dgvBlastEditor.EndEdit();
					dgvBlastEditor.ClearSelection();
				}
			}
			else if (e.Button == MouseButtons.Right)
			{
				//End the edit if they're right clicking somewhere else
				if (dgvBlastEditor.CurrentCell.ColumnIndex != e.ColumnIndex)
				{
					dgvBlastEditor.EndEdit();
				}

				cms = new ContextMenuStrip();

				if (e.RowIndex != -1 && e.ColumnIndex != -1)
				{
					PopulateGenericContextMenu();
					//Can't use a switch statement because dynamic
					if (dgvBlastEditor.Columns[e.ColumnIndex] == dgvBlastEditor.Columns[buProperty.Address.ToString()] ||
						dgvBlastEditor.Columns[e.ColumnIndex] == dgvBlastEditor.Columns[buProperty.SourceAddress.ToString()])
					{
						cms.Items.Add(new ToolStripSeparator());
						PopulateAddressContextMenu(dgvBlastEditor[e.ColumnIndex, e.RowIndex]);
					}
					cms.Show(dgvBlastEditor, dgvBlastEditor.PointToClient(Cursor.Position));
				}
			}
		}

		private void dgvBlastEditor_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				dgvBlastEditor.BeginEdit(false);
			}
		}


		private void PopulateGenericContextMenu()
		{
			((ToolStripMenuItem)cms.Items.Add("Re-roll Selected Row(s)", null, new EventHandler((ob, ev) =>
			{
				foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
				{
					BlastUnit bu = (BlastUnit)row.DataBoundItem;
					bu.Reroll();
				}
				dgvBlastEditor.Refresh();
				UpdateBottom();
			}))).Enabled = true;
		}

		private void PopulateAddressContextMenu(DataGridViewCell cell)
		{
			((ToolStripMenuItem)cms.Items.Add("Open Selected Address in Hex Editor", null, new EventHandler((ob, ev) =>
			{
				BlastUnit bu = (BlastUnit)dgvBlastEditor.Rows[cell.RowIndex].DataBoundItem;
				if (cell.OwningColumn == dgvBlastEditor.Columns[buProperty.Address.ToString()])
					LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.EMU_OPEN_HEXEDITOR_ADDRESS, new object[] { bu.Domain, bu.Address });

				if (cell.OwningColumn == dgvBlastEditor.Columns[buProperty.SourceAddress.ToString()])
					LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.EMU_OPEN_HEXEDITOR_ADDRESS, new object[] { bu.SourceDomain, bu.SourceAddress });
			}))).Enabled = true;
		}

		private void dgvBlastEditor_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			DataGridViewColumn changedColumn = dgvBlastEditor.Columns[e.ColumnIndex];

			//If the Domain or SourceDomain changed update the Maximum Value
			if (changedColumn.Name == buProperty.Domain.ToString())
			{
				updateMaximum(dgvBlastEditor.Rows[e.RowIndex].Cells[buProperty.Address.ToString()] as DataGridViewNumericUpDownCell, dgvBlastEditor.Rows[e.RowIndex].Cells[buProperty.Domain.ToString()].Value.ToString());
			}
			else if (changedColumn.Name == buProperty.SourceDomain.ToString())
			{
				updateMaximum(dgvBlastEditor.Rows[e.RowIndex].Cells[buProperty.SourceAddress.ToString()] as DataGridViewNumericUpDownCell, dgvBlastEditor.Rows[e.RowIndex].Cells[buProperty.SourceDomain.ToString()].Value.ToString());
			}
			UpdateBottom();
		}

		private void CbSourceDomain_Validated(object sender, EventArgs e)
		{
			var value = cbSourceDomain.SelectedItem;
			foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
				row.Cells[buProperty.SourceDomain.ToString()]
					.Value = value;
			UpdateBottom();
		}

		private void CbStoreType_Validated(object sender, EventArgs e)
		{
			var value = cbStoreType.SelectedItem;
			foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
				row.Cells[buProperty.StoreType.ToString()].Value = value;
			UpdateBottom();
		}

		private void CbStoreTime_Validated(object sender, EventArgs e)
		{
			var value = cbStoreTime.SelectedItem;
			foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
				row.Cells[buProperty.StoreTime.ToString()].Value = value;
			UpdateBottom();
		}

		private void CbLimiterList_Validated(object sender, EventArgs e)
		{
			var value = ((ComboBoxItem<String>)(cbLimiterList?.SelectedItem))?.Value ?? null;
			foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
				row.Cells[buProperty.LimiterListHash.ToString()].Value = value; // We gotta use the value
			UpdateBottom();
		}

		private void CbBigEndian_Validated(object sender, EventArgs e)
		{
			var value = cbBigEndian.Checked;
			//Big Endian isn't available in the DGV so we operate on the actual BU then refresh
			//Todo - change this?
			foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
			{
				((BlastUnit)row.DataBoundItem).BigEndian = value;
			}
			dgvBlastEditor.Refresh();
			UpdateBottom();
		}

		private void TbValue_Validated(object sender, EventArgs e)
		{
			var value = tbValue.Text;
			foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
				row.Cells[buProperty.ValueString.ToString()].Value = value;
			UpdateBottom();
		}

		private void CbSource_Validated(object sender, EventArgs e)
		{
			var value = cbSource.SelectedItem;
			foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
				row.Cells[buProperty.Source.ToString()].Value = value;
			UpdateBottom();
		}

		private void TbTiltValue_Validated(object sender, EventArgs e)
		{
			if (!BigInteger.TryParse(tbTiltValue.Text, out BigInteger value))
				value = 0;

			//Tilt isn't stored within the DGV so operate on the BUs. No validation neccesary as it's a bigint
			foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
			{
				(row.DataBoundItem as BlastUnit).TiltValue = value;
			}
			UpdateBottom();
		}
		private void UpDownLifetime_Validated(object sender, EventArgs e)
		{
			var value = upDownLifetime.Value;
			if (value > Int32.MaxValue)
				value = Int32.MaxValue;
			foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
				row.Cells[buProperty.Lifetime.ToString()].Value = value;

			UpdateBottom();
			dgvBlastEditor.Refresh();
		}
		private void UpDownExecuteFrame_Validated(object sender, EventArgs e)
		{
			var value = upDownExecuteFrame.Value;
			if (value > Int32.MaxValue)
				value = Int32.MaxValue;
			foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
				row.Cells[buProperty.ExecuteFrame.ToString()].Value = value;

			UpdateBottom();
			dgvBlastEditor.Refresh();
		}

		private void UpDownPrecision_Validated(object sender, EventArgs e)
		{
			var value = upDownPrecision.Value;

			if (value > Int32.MaxValue)
				value = Int32.MaxValue;

			foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
				row.Cells[buProperty.Precision.ToString()].Value = value;
			UpdateBottom();
			dgvBlastEditor.Refresh();
		}

		private void UpDownAddress_Validated(object sender, EventArgs e)
		{
			var value = upDownAddress.Value;
			if (value > Int32.MaxValue)
				value = Int32.MaxValue;
			foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
				row.Cells[buProperty.Address.ToString()].Value = value;
			UpdateBottom();
		}

		private void UpDownSourceAddress_Validated(object sender, EventArgs e)
		{
			var value = upDownSourceAddress.Value;
			if (value > Int32.MaxValue)
				value = Int32.MaxValue;
			foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
				row.Cells[buProperty.SourceAddress.ToString()].Value = value;
			UpdateBottom();
		}

		private void CbLocked_Validated(object sender, EventArgs e)
		{
			var value = cbLocked.Checked;
			foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
				row.Cells[buProperty.isLocked.ToString()].Value = value;
			UpdateBottom();
		}

		private void CbLimiterTime_Validated(object sender, EventArgs e)
		{
			var value = cbLimiterTime.SelectedItem;
			foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
				row.Cells[buProperty.LimiterTime.ToString()].Value = value;
			UpdateBottom();
		}
		
		private void cbStoreLimiterSource_Validated(object sender, EventArgs e)
		{
			var value = cbStoreLimiterSource.SelectedItem;
			foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
				row.Cells[buProperty.StoreLimiterSource.ToString()].Value = value;
			UpdateBottom();
		}


		private void CbInvertLimiter_Validated(object sender, EventArgs e)
		{
			var value = cbInvertLimiter.Checked;
			foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
				row.Cells[buProperty.InvertLimiter.ToString()].Value = value;
			UpdateBottom();
		}
		private void cbEnabled_Validated(object sender, EventArgs e)
		{
			var value = cbEnabled.Checked;
			foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
				row.Cells[buProperty.isEnabled.ToString()].Value = value;
			UpdateBottom();
		}

		private void cbDomain_Validated(object sender, EventArgs e)
		{
			var value = cbDomain.SelectedItem;

			if (!domains.Contains(value))
				return;

				foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
				row.Cells[buProperty.Domain.ToString()].Value = value;
			UpdateBottom();
		}


		private void CbLoop_Validated(object sender, EventArgs e)
		{
			var value = cbLoop.Checked;
			foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
				row.Cells[buProperty.Loop.ToString()].Value = value;
			UpdateBottom();
		}


		private void dgvBlastEditor_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if(e.Button == MouseButtons.Right)
			{
				headerStrip = new ContextMenuStrip();
				headerStrip.Items.Add("Select columns to show", null, new EventHandler((ob, ev) =>
				{
					ColumnSelector cs = new ColumnSelector();
					cs.LoadColumnSelector(dgvBlastEditor.Columns);
				}));

				headerStrip.Show(MousePosition);
			}
		}


		private void updateMaximum(List<DataGridViewRow> rows)
		{
			foreach (DataGridViewRow row in rows)
			{
				BlastUnit bu = row.DataBoundItem as BlastUnit;
				string domain = bu.Domain;
				string sourceDomain = bu.SourceDomain;
				

				if(domain != null && domainToMiDico.ContainsKey(bu.Domain ?? ""))
					(row.Cells[buProperty.Address.ToString()] as DataGridViewNumericUpDownCell).Maximum = domainToMiDico[domain].Size - 1;
				if(sourceDomain != null && domainToMiDico.ContainsKey(bu.SourceDomain ?? ""))
					(row.Cells[buProperty.SourceAddress.ToString()] as DataGridViewNumericUpDownCell).Maximum = domainToMiDico[sourceDomain].Size - 1;
			}
		}

		private void updateMaximum(DataGridViewNumericUpDownCell cell, String domain)
		{
			if (domainToMiDico.ContainsKey(domain))
				cell.Maximum = domainToMiDico[domain].Size - 1;
			else
				cell.Maximum = Int32.MaxValue;

		}
		
		private void UpdateBottom()
		{
			if (dgvBlastEditor.SelectedRows.Count > 0)
			{
				var lastRow = dgvBlastEditor.SelectedRows[dgvBlastEditor.SelectedRows.Count - 1];

				/*
				cbDomain.SelectedItem = (String)(lastRow.Cells[buProperty.Domain.ToString()].Value);
				cbEnabled.Checked = (bool)(lastRow.Cells[buProperty.isEnabled.ToString()].Value);
				cbLocked.Checked = (bool)(lastRow.Cells[buProperty.isLocked.ToString()].Value);
				upDownAddress.Value = (long)(lastRow.Cells[buProperty.Address.ToString()].Value);
				upDownPrecision.Value = (int)(lastRow.Cells[buProperty.Precision.ToString()].Value);
				tbValue.Text = (String)(lastRow.Cells[buProperty.ValueString.ToString()].Value);
				upDownExecuteFrame.Value = (int)(lastRow.Cells[buProperty.ExecuteFrame.ToString()].Value);
				upDownLifetime.Value = (int)(lastRow.Cells[buProperty.Lifetime.ToString()].Value);
				cbLoop.Checked = (bool)(lastRow.Cells[buProperty.Loop.ToString()].Value);
				cbLimiterTime.SelectedItem = (ActionTime)(lastRow.Cells[buProperty.LimiterTime.ToString()].Value);
				cbLimiterList.SelectedItem = (String)(lastRow.Cells[buProperty.LimiterHash.ToString()].Value);
				cbInvertLimiter.Checked = (bool)(lastRow.Cells[buProperty.InvertLimiter.ToString()].Value);
				cbStoreTime.SelectedItem = (ActionTime)(lastRow.Cells[buProperty.StoreTime.ToString()].Value);
				cbStoreType.SelectedItem = (StoreType)(lastRow.Cells[buProperty.StoreType.ToString()].Value);
				cbSourceDomain.SelectedItem = (String)(lastRow.Cells[buProperty.SourceDomain.ToString()].Value);
				cbSource.SelectedItem = (BlastUnitSource)(lastRow.Cells[buProperty.Source.ToString()].Value);
				upDownSourceAddress.Value = (long)(lastRow.Cells[buProperty.SourceAddress.ToString()].Value);

				tbTiltValue.Text = (lastRow.DataBoundItem as BlastUnit).TiltValue.ToString();*/
				BlastUnit bu = (BlastUnit)lastRow.DataBoundItem;



				if (domainToMiDico.ContainsKey(bu.Domain ?? String.Empty))
					upDownAddress.Maximum = domainToMiDico[bu.Domain].Size -1;
				else
					upDownAddress.Maximum = Int32.MaxValue;

				if (domainToMiDico.ContainsKey(bu.SourceDomain ?? String.Empty))
					upDownSourceAddress.Maximum = domainToMiDico[bu.SourceDomain].Size -1;
				else
					upDownSourceAddress.Maximum = Int32.MaxValue;


				cbDomain.SelectedItem = bu.Domain;
				cbEnabled.Checked = bu.IsEnabled;
				cbLocked.Checked = bu.IsLocked;
				cbBigEndian.Checked = bu.BigEndian;

				upDownAddress.Value = bu.Address;
				upDownPrecision.Value = bu.Precision;
				tbValue.Text = bu.ValueString;
				upDownExecuteFrame.Value = bu.ExecuteFrame;
				upDownLifetime.Value = bu.Lifetime;
				cbLoop.Checked = bu.Loop;
				cbLimiterTime.SelectedItem = bu.LimiterTime;
				cbStoreLimiterSource.SelectedItem = bu.StoreLimiterSource;

				cbLimiterList.SelectedItem = CorruptCore.CorruptCore.LimiterListBindingSource.FirstOrDefault(x => x.Value == bu.LimiterListHash);

				cbInvertLimiter.Checked = bu.InvertLimiter;
				cbStoreTime.SelectedItem = bu.StoreTime;
				cbStoreType.SelectedItem = bu.StoreType;
				cbSourceDomain.SelectedItem = bu.SourceDomain;
				cbSource.SelectedItem = bu.Source;
				upDownSourceAddress.Value = bu.SourceAddress;


				tbTiltValue.Text = bu.TiltValue.ToString();

			}
		}

		private void dgvBlastEditor_SelectionChanged(object sender, EventArgs e)
		{
			UpdateBottom();

			List<DataGridViewRow> col = new List<DataGridViewRow>();
			//For some reason DataGridViewRowCollection and DataGridViewSelectedRowCollection aren't directly compatible???
			foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
				col.Add(row);

			//Rather than setting all these values at load, we set it on the fly
			updateMaximum(col);
		}

		private void DgvBlastEditor_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			//Bug in DGV. If you don't read the value back, it goes into edit mode on first click if you read the selectedrow within SelectionChanged. Why? No idea.
			var dummy = dgvBlastEditor.Rows[e.RowIndex].Cells[0].Value;
		}

		private void tbFilter_TextChanged(object sender, EventArgs e)
		{

			if (tbFilter.Text.Length == 0)
			{
				dgvBlastEditor.DataSource = bs;
				_bs = null;
				RefreshAllNoteIcons();
				return;
			}
				

			string value = ((ComboBoxItem<String>)cbFilterColumn?.SelectedItem)?.Value;
			if (value == null)
				return;

			_bs = new BindingSource();
			switch (((ComboBoxItem<String>)cbFilterColumn.SelectedItem).Value)
			{
				//If it's an address or a source address we want decimal
				case "Address":
					_bs.DataSource = currentSK.BlastLayer.Layer.Where(x => x.Address.ToString("X").ToUpper().Substring(0, tbFilter.Text.Length.Clamp(0, x.Address.ToString("X").Length)) == tbFilter.Text.ToUpper()).ToList();
					break;
				case "SourceAddress":
					_bs.DataSource = currentSK.BlastLayer.Layer.Where(x => x.SourceAddress.ToString("X").ToUpper().Substring(0, tbFilter.Text.Length.Clamp(0, x.SourceAddress.ToString("X").Length)) == tbFilter.Text.ToUpper()).ToList();
					break;
				default: //Otherwise just use reflection and dig it out
					_bs.DataSource = currentSK.BlastLayer.Layer.Where(x => x?.GetType()?.GetProperty(value)?.GetValue(x) != null && (x.GetType()?.GetProperty(value)?.GetValue(x).ToString().ToUpper().Substring(0, tbFilter.Text.Length) == tbFilter.Text.ToUpper())).ToList();
					break;
			}
			dgvBlastEditor.DataSource = _bs;
			RefreshAllNoteIcons();
		}
	
		private void InitializeBottom()
		{
			property2ControlDico = new Dictionary<string, Control>();

			var storeType = Enum.GetValues(typeof(StoreType));
			var blastUnitSource = Enum.GetValues(typeof(BlastUnitSource));

			cbDomain.BindingContext = new BindingContext();
			cbDomain.DataSource = domains;

			cbSourceDomain.BindingContext = new BindingContext();
			cbSourceDomain.DataSource = domains;

			foreach (var item in Enum.GetValues(typeof(LimiterTime)))
			{
				cbLimiterTime.Items.Add(item);
			}
			foreach (var item in Enum.GetValues(typeof(StoreLimiterSource)))
			{
				cbStoreLimiterSource.Items.Add(item);
			}
			foreach (var item in Enum.GetValues(typeof(StoreTime)))
			{
				cbStoreTime.Items.Add(item);
			}
			foreach (var item in blastUnitSource)
			{
				cbSource.Items.Add(item);
			}

			cbLimiterList.DataSource = CorruptCore.CorruptCore.LimiterListBindingSource;
			cbLimiterList.DisplayMember = "Name";
			cbLimiterList.ValueMember = "Value";

			cbStoreType.DataSource = storeType;

			property2ControlDico.Add(buProperty.Address.ToString(), upDownAddress);
			property2ControlDico.Add(buProperty.Domain.ToString(), cbDomain);
			property2ControlDico.Add(buProperty.Source.ToString(), cbSource);
			property2ControlDico.Add(buProperty.ExecuteFrame.ToString(), upDownExecuteFrame);
			property2ControlDico.Add(buProperty.InvertLimiter.ToString(), cbInvertLimiter);
			property2ControlDico.Add(buProperty.isEnabled.ToString(), cbEnabled);
			property2ControlDico.Add(buProperty.isLocked.ToString(), cbLocked);
			property2ControlDico.Add(buProperty.Lifetime.ToString(), upDownLifetime);
			property2ControlDico.Add(buProperty.LimiterListHash.ToString(), cbLimiterList);
			property2ControlDico.Add(buProperty.LimiterTime.ToString(), cbLimiterTime);
			property2ControlDico.Add(buProperty.Loop.ToString(), cbLoop);
			property2ControlDico.Add(buProperty.Note.ToString(), btnNote);
			property2ControlDico.Add(buProperty.Precision.ToString(), upDownPrecision);
			property2ControlDico.Add(buProperty.SourceAddress.ToString(), upDownSourceAddress);
			property2ControlDico.Add(buProperty.SourceDomain.ToString(), cbSourceDomain);
			property2ControlDico.Add(buProperty.StoreTime.ToString(), cbStoreTime);
			property2ControlDico.Add(buProperty.StoreLimiterSource.ToString(), cbStoreTime);
			property2ControlDico.Add(buProperty.StoreType.ToString(), cbStoreType);
			property2ControlDico.Add(buProperty.ValueString.ToString(), tbValue);
		}



		private void InitializeDGV()
		{

			VisibleColumns = new List<string>();
			var blastUnitSource = Enum.GetValues(typeof(BlastUnitSource));


			var enabled = CreateColumn(buProperty.isEnabled.ToString(), buProperty.isEnabled.ToString(), "Enabled"
				, new DataGridViewCheckBoxColumn());
			enabled.SortMode = DataGridViewColumnSortMode.Automatic;
			dgvBlastEditor.Columns.Add(enabled);

			var locked = CreateColumn(buProperty.isLocked.ToString(), buProperty.isLocked.ToString(), "Locked"
				, new DataGridViewCheckBoxColumn());
			locked.SortMode = DataGridViewColumnSortMode.Automatic;
			dgvBlastEditor.Columns.Add(locked);

			//Do this one separately as we need to populate the Combobox
			DataGridViewComboBoxColumn source = CreateColumn(buProperty.Source.ToString(), buProperty.Source.ToString(), "Source", new DataGridViewComboBoxColumn()) as DataGridViewComboBoxColumn;
			foreach (var item in blastUnitSource)
				source.Items.Add(item);
			dgvBlastEditor.Columns.Add(source);

			//Do this one separately as we need to populate the Combobox
			DataGridViewComboBoxColumn domain = CreateColumn(buProperty.Domain.ToString(), buProperty.Domain.ToString(), "Domain", new DataGridViewComboBoxColumn()) as DataGridViewComboBoxColumn;
			domain.DataSource = domains;
			domain.SortMode = DataGridViewColumnSortMode.Automatic;
			dgvBlastEditor.Columns.Add(domain);

			DataGridViewNumericUpDownColumn address = (DataGridViewNumericUpDownColumn)CreateColumn(buProperty.Address.ToString(), buProperty.Address.ToString(), "Address", new DataGridViewNumericUpDownColumn());
			address.Hexadecimal = true;
			address.SortMode = DataGridViewColumnSortMode.Automatic;
			address.Increment = 1;
			dgvBlastEditor.Columns.Add(address);



			
			
			DataGridViewNumericUpDownColumn precision = (DataGridViewNumericUpDownColumn)CreateColumn(buProperty.Precision.ToString(), buProperty.Precision.ToString(), "Precision", new DataGridViewNumericUpDownColumn());
			precision.Minimum = 1;
			precision.Maximum = Int32.MaxValue;
			precision.SortMode = DataGridViewColumnSortMode.Automatic;
			dgvBlastEditor.Columns.Add(precision);


			var valuestring = CreateColumn(buProperty.ValueString.ToString(), buProperty.ValueString.ToString(), "Value"
				, new DataGridViewTextBoxColumn());
			valuestring.DefaultCellStyle.Tag = "numeric";
			valuestring.SortMode = DataGridViewColumnSortMode.Automatic;
			((DataGridViewTextBoxColumn)valuestring).MaxInputLength = 16348; //textbox doesn't like larger than ~20k
			dgvBlastEditor.Columns.Add(valuestring);


			var executeFrame = CreateColumn(buProperty.ExecuteFrame.ToString(), buProperty.ExecuteFrame.ToString()
				, "Execute Frame", new DataGridViewNumericUpDownColumn());
			executeFrame.SortMode = DataGridViewColumnSortMode.Automatic;
			((DataGridViewNumericUpDownColumn)(executeFrame)).Maximum = Int32.MaxValue;
			dgvBlastEditor.Columns.Add(executeFrame);

			var lifetime = CreateColumn(buProperty.Lifetime.ToString(), buProperty.Lifetime.ToString(), "Lifetime"
				, new DataGridViewNumericUpDownColumn());
			lifetime.SortMode = DataGridViewColumnSortMode.Automatic;
			((DataGridViewNumericUpDownColumn)(lifetime)).Maximum = Int32.MaxValue;
			dgvBlastEditor.Columns.Add(lifetime);

			var loop = CreateColumn(buProperty.Loop.ToString(), buProperty.Loop.ToString(), "Loop"
				, new DataGridViewCheckBoxColumn());
			loop.SortMode = DataGridViewColumnSortMode.Automatic;
			dgvBlastEditor.Columns.Add(loop);


			DataGridViewComboBoxColumn limiterTime = CreateColumn(buProperty.LimiterTime.ToString(), buProperty.LimiterTime.ToString(), "Limiter Time", new DataGridViewComboBoxColumn()) as DataGridViewComboBoxColumn;
			foreach (var item in Enum.GetValues(typeof(LimiterTime)))
				limiterTime.Items.Add(item);
			dgvBlastEditor.Columns.Add(limiterTime);

			DataGridViewComboBoxColumn limiterHash = CreateColumn(buProperty.LimiterListHash.ToString(), buProperty.LimiterListHash.ToString(), "Limiter List", new DataGridViewComboBoxColumn()) as DataGridViewComboBoxColumn;
			limiterHash.DataSource = CorruptCore.CorruptCore.LimiterListBindingSource;
			limiterHash.DisplayMember = "Name";
			limiterHash.ValueMember = "Value";
			limiterHash.MaxDropDownItems = 15;
			dgvBlastEditor.Columns.Add(limiterHash);

			DataGridViewComboBoxColumn storeLimiterSource = CreateColumn(buProperty.StoreLimiterSource.ToString(), buProperty.StoreLimiterSource.ToString(), "Store Limiter Source", new DataGridViewComboBoxColumn()) as DataGridViewComboBoxColumn;
			foreach (var item in Enum.GetValues(typeof(StoreLimiterSource)))
				storeLimiterSource.Items.Add(item);
			dgvBlastEditor.Columns.Add(storeLimiterSource);

			dgvBlastEditor.Columns.Add(CreateColumn(buProperty.InvertLimiter.ToString(), buProperty.InvertLimiter.ToString(), "Invert Limiter", new DataGridViewCheckBoxColumn()));


			DataGridViewComboBoxColumn storeTime = CreateColumn(buProperty.StoreTime.ToString(), buProperty.StoreTime.ToString(), "Store Time", new DataGridViewComboBoxColumn()) as DataGridViewComboBoxColumn;
			foreach (var item in Enum.GetValues(typeof(StoreTime)))
				storeTime.Items.Add(item);
			storeTime.SortMode = DataGridViewColumnSortMode.Automatic;
			dgvBlastEditor.Columns.Add(storeTime);
			
			DataGridViewComboBoxColumn storeType = CreateColumn(buProperty.StoreType.ToString(), buProperty.StoreType.ToString(), "Store Type", new DataGridViewComboBoxColumn()) as DataGridViewComboBoxColumn;
			storeType.DataSource = Enum.GetValues(typeof(StoreType));
			storeType.SortMode = DataGridViewColumnSortMode.Automatic;
			dgvBlastEditor.Columns.Add(storeType);

			//Do this one separately as we need to populate the Combobox
			DataGridViewComboBoxColumn sourceDomain = CreateColumn(buProperty.SourceDomain.ToString(), buProperty.SourceDomain.ToString(), "Source Domain", new DataGridViewComboBoxColumn()) as DataGridViewComboBoxColumn;
			sourceDomain.DataSource = domains;
			sourceDomain.SortMode = DataGridViewColumnSortMode.Automatic;
			dgvBlastEditor.Columns.Add(sourceDomain);

			DataGridViewNumericUpDownColumn sourceAddress = (DataGridViewNumericUpDownColumn)CreateColumn(buProperty.SourceAddress.ToString(), buProperty.SourceAddress.ToString(), "Source Address", new DataGridViewNumericUpDownColumn());
			sourceAddress.Hexadecimal = true;
			sourceAddress.SortMode = DataGridViewColumnSortMode.Automatic;
			sourceAddress.Increment = 1;
			dgvBlastEditor.Columns.Add(sourceAddress);


			dgvBlastEditor.Columns.Add(CreateColumn("", buProperty.Note.ToString(), "Note", new DataGridViewButtonColumn()));



			if (RTCV.NetCore.Params.IsParamSet("BLASTEDITOR_VISIBLECOLUMNS"))
			{
				string str = RTCV.NetCore.Params.ReadParam("BLASTEDITOR_VISIBLECOLUMNS");
				string[] columns = str.Split(',');
				foreach (string column in columns)
				{
					VisibleColumns.Add(column);
				}
			}
			else
			{
				VisibleColumns.Add(buProperty.isEnabled.ToString());
				VisibleColumns.Add(buProperty.isLocked.ToString());
				VisibleColumns.Add(buProperty.Source.ToString());
				VisibleColumns.Add(buProperty.Domain.ToString());
				VisibleColumns.Add(buProperty.Address.ToString());
				VisibleColumns.Add(buProperty.Address.ToString());
				VisibleColumns.Add(buProperty.Precision.ToString());
				VisibleColumns.Add(buProperty.ValueString.ToString());
				VisibleColumns.Add(buProperty.Note.ToString());
			}

			RefreshVisibleColumns();

			PopulateFilterCombobox();
			PopulateShiftCombobox();
		}


		private void PopulateFilterCombobox()
		{
			cbFilterColumn.SelectedItem = null;
			cbFilterColumn.Items.Clear();

			//Populate the filter ComboBox
			cbFilterColumn.DisplayMember = "Name";
			cbFilterColumn.ValueMember = "Value";
			foreach (DataGridViewColumn column in dgvBlastEditor.Columns)
			{
				//Exclude button and checkbox
				if (!(column is DataGridViewCheckBoxColumn || column is DataGridViewButtonColumn))// && column.Visible)
					cbFilterColumn.Items.Add(new ComboBoxItem<String>(column.HeaderText, column.Name));
			}
			cbFilterColumn.SelectedIndex = 0;
		}

		private void PopulateShiftCombobox()
		{
			cbShiftBlastlayer.SelectedItem = null;
			cbShiftBlastlayer.Items.Clear();

			//Populate the filter ComboBox
			cbShiftBlastlayer.DisplayMember = "Name";
			cbShiftBlastlayer.ValueMember = "Value";

			cbShiftBlastlayer.Items.Add(new ComboBoxItem<String>(buProperty.Address.ToString(), buProperty.Address.ToString()));
			cbShiftBlastlayer.Items.Add(new ComboBoxItem<String>("Source Address", buProperty.SourceAddress.ToString()));
			cbShiftBlastlayer.Items.Add(new ComboBoxItem<String>("Value", buProperty.ValueString.ToString()));
			cbShiftBlastlayer.Items.Add(new ComboBoxItem<String>(buProperty.Lifetime.ToString(), buProperty.Lifetime.ToString()));
			cbShiftBlastlayer.Items.Add(new ComboBoxItem<String>("Execute Frame", buProperty.ExecuteFrame.ToString()));
			cbShiftBlastlayer.SelectedIndex = 0;
		}


		public void RefreshVisibleColumns()
		{	
			foreach (DataGridViewColumn column in dgvBlastEditor.Columns)
			{
				if (VisibleColumns.Contains(column.Name))
					column.Visible = true;
				else
					column.Visible = false;
			}
			dgvBlastEditor.Refresh();
		}



		private DataGridViewColumn CreateColumn(string dataPropertyName, string columnName, string displayName,
			DataGridViewColumn column, int fillWeight = -1)
		{

			if(fillWeight == -1)
			{

				switch (column)
				{
					case DataGridViewButtonColumn s:
						s.FillWeight = buttonFillWeight;
						break;
					case DataGridViewCheckBoxColumn s:
						s.FillWeight = checkBoxFillWeight;
						break;
					case DataGridViewComboBoxColumn s:
						s.FillWeight = comboBoxFillWeight;
						break;
					case DataGridViewTextBoxColumn s:
						s.FillWeight = textBoxFillWeight;
						break;
					case DataGridViewNumericUpDownColumn s:
						s.FillWeight = numericUpDownFillWeight;
						break;
				}
			}
			else
			{
				column.FillWeight = fillWeight;
			}


			column.DataPropertyName = dataPropertyName;
			column.Name = columnName;

			column.HeaderText = displayName;

			return column;
		}


		private DataGridViewColumn CreateColumnUnbound(string columnName, string displayName,
			DataGridViewColumn column, int fillWeight = -1)
		{
			return CreateColumn(String.Empty, columnName, displayName, column, fillWeight);
		}

		StashKey originalSK = null;
		StashKey currentSK = null;
		BindingSource bs = null;
		BindingSource _bs = null;
		public void LoadStashkey(StashKey sk)
		{
			if (!RefreshDomains())
			{
				MessageBox.Show("Loading domains failed! Aborting load. Check to make sure the RTC and Bizhawk are connected.");
				this.Close();
				return;
			}
			List<String> buDomains = new List<String>();
			foreach (var bu in sk.BlastLayer.Layer)
			{
				if (!buDomains.Contains(bu.Domain))
					buDomains.Add(bu.Domain);
				if (bu.SourceDomain != null && !buDomains.Contains(bu.SourceDomain))
					buDomains.Add(bu.SourceDomain);
			}

			foreach (string domain in buDomains)
			{
				if (domainToMiDico.ContainsKey(domain))
					continue;

				MessageBox.Show("This blastlayer references domain " + domain + " which couldn't be found!\nAre you sure you have the correct core loaded?");
				this.Hide();
				return;
			}

			originalSK = sk;
			currentSK = sk.Clone() as StashKey;



			bs = new BindingSource {DataSource = new SortableBindingList<BlastUnit>(currentSK.BlastLayer.Layer)};

			bs.CurrentChanged += (o, e) =>
			{
				if (batchOperation)
				{
					if(e is HandledEventArgs h)
						h.Handled = true;
				}
			};

			dgvBlastEditor.DataSource = bs;
			InitializeDGV();
			InitializeBottom();
			this.Show();
			this.BringToFront();
			RefreshAllNoteIcons();
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
				if(domainToMiDico.Keys.Count > 0)
					return true;
				return false;
			}
			catch (Exception ex)
			{
				throw new Exception(
							"An error occurred in RTC while refreshing the domains\n" +
							"Are you sure you don't have an invalid domain selected?\n" +
							"Make sure any VMDs are loaded and you have the correct core loaded in Bizhawk\n" +
							ex.ToString()
							);
			}
		}

		private void dgvBlastLayer_DataError(object sender, DataGridViewDataErrorEventArgs e)
		{

			MessageBox.Show(e.Exception.ToString() + "\nRow:" + e.RowIndex + "\nColumn" + e.ColumnIndex + "\n" + e.Context + "\n" + dgvBlastEditor[e.ColumnIndex, e.RowIndex].Value?.ToString());
		}

		public void btnDisable50_Click(object sender, EventArgs e)
		{
			foreach (BlastUnit bu in currentSK.BlastLayer.Layer.
				Where(x => x.IsLocked == false))
			{
				bu.IsEnabled = true;
			}

			foreach (BlastUnit bu in currentSK.BlastLayer.Layer
				.Where(x => x.IsLocked == false)
				.OrderBy(x => CorruptCore.CorruptCore.RND.Next())
				.Take(currentSK.BlastLayer.Layer.Count / 2))
			{
				bu.IsEnabled = false;
			}
			dgvBlastEditor.Refresh();
		}

		public void btnInvertDisabled_Click(object sender, EventArgs e)
		{
			foreach (BlastUnit bu in currentSK.BlastLayer.Layer.
				Where(x => x.IsLocked == false))
			{
				bu.IsEnabled = !bu.IsEnabled;
			}
			dgvBlastEditor.Refresh();
		}

		public void btnRemoveDisabled_Click(object sender, EventArgs e)
		{
			List<BlastUnit> buToRemove = new List<BlastUnit>();

			dgvBlastEditor.SuspendLayout();
			batchOperation = true;
			var oldBS = dgvBlastEditor.DataSource;
			dgvBlastEditor.DataSource = null;
			foreach (BlastUnit bu in currentSK.BlastLayer.Layer.
				Where(x => 
				x.IsLocked == false &&
				x.IsEnabled == false))
			{
				buToRemove.Add(bu);
			}

			foreach (BlastUnit bu in buToRemove)
			{
				bs.Remove(bu);
				if (_bs != null && _bs.Contains(bu))
					_bs.Remove(bu);
			}
			batchOperation = false;
			dgvBlastEditor.DataSource = oldBS;
			RefreshAllNoteIcons();
			dgvBlastEditor.ResumeLayout();
		}

		private void btnDisableEverything_Click(object sender, EventArgs e)
		{
			foreach (BlastUnit bu in currentSK.BlastLayer.Layer.
				Where(x =>
				x.IsLocked == false))
			{
				bu.IsEnabled = false;
			}
			dgvBlastEditor.Refresh();
		}

		private void btnEnableEverything_Click(object sender, EventArgs e)
		{
			foreach (BlastUnit bu in currentSK.BlastLayer.Layer.
				Where(x =>
				x.IsLocked == false))
			{
				bu.IsEnabled = true;
			}
			dgvBlastEditor.Refresh();
		}

		private void btnRemoveSelected_Click(object sender, EventArgs e)
		{
			foreach(DataGridViewRow row in dgvBlastEditor.SelectedRows)
			{
				if ((row.DataBoundItem as BlastUnit).IsLocked == false)
				{
					var bu = row.DataBoundItem as BlastUnit;
					bs.Remove(bu);
					//Todo replace how this works
					if (_bs != null && _bs.Contains(bu))
						bs.Remove(bu);
				}		
			}
		}

		private void btnDuplicateSelected_Click(object sender, EventArgs e)
		{
			if (dgvBlastEditor.SelectedRows.Count == 0)
				return;

			var reversed = dgvBlastEditor.SelectedRows.Cast<DataGridViewRow>().Reverse()?.ToArray();
			foreach (DataGridViewRow row in reversed)
			{
				if ((row.DataBoundItem as BlastUnit).IsLocked == false)
				{
					BlastUnit bu = ((row.DataBoundItem as BlastUnit).Clone() as BlastUnit);
					bs.Add(bu);
				}
			}
			RefreshAllNoteIcons();
		}

		public void btnSendToStash_Click(object sender, EventArgs e)
		{
			if (currentSK.ParentKey == null)
			{
				MessageBox.Show("There's no savestate associated with this Stashkey!\nAssociate one in the menu to send this to the stash.");
				return;
			}
			StashKey newSk = (StashKey)currentSK.Clone();
			//newSk.Key = RTC_Core.GetRandomKey();
			//newSk.Alias = null;

			StockpileManager_UISide.StashHistory.Add(newSk);

			S.GET<RTC_GlitchHarvester_Form>().RefreshStashHistory();
			S.GET<RTC_GlitchHarvester_Form>().dgvStockpile.ClearSelection();
			S.GET<RTC_GlitchHarvester_Form>().lbStashHistory.ClearSelected();

			S.GET<RTC_GlitchHarvester_Form>().DontLoadSelectedStash = true;
			S.GET<RTC_GlitchHarvester_Form>().lbStashHistory.SelectedIndex = S.GET<RTC_GlitchHarvester_Form>().lbStashHistory.Items.Count - 1;
			StockpileManager_UISide.CurrentStashkey = StockpileManager_UISide.StashHistory[S.GET<RTC_GlitchHarvester_Form>().lbStashHistory.SelectedIndex];

		}

		private void btnSearchAgain_Click(object sender, EventArgs e)
		{
			if (searchEnumerable.Count() != 0 && searchOffset < searchEnumerable.Count())
				bs.Position = bs.IndexOf(searchEnumerable.ElementAt(searchOffset));
			else
			{ 
				MessageBox.Show("Reached end of list without finding anything.");
			}
			searchOffset++;
		}

		private void btnNote_Click(object sender, EventArgs e)
		{
			if (dgvBlastEditor.SelectedRows.Count > 0)
			{
				BlastLayer temp = new BlastLayer();
				List<DataGridViewCell> cellList = new List<DataGridViewCell>();
				foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
				{
					if (row.DataBoundItem is BlastUnit bu)
					{
						temp.Layer.Add(bu);
						cellList.Add(row.Cells[buProperty.Note.ToString()]);
					}					
				}

				S.SET(new RTC_NoteEditor_Form(temp, cellList));
				S.GET<RTC_NoteEditor_Form>().Show();
			}
		}

		private void sanitizeDuplicatesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			List<BlastUnit> bul = new List<BlastUnit>(currentSK.BlastLayer.Layer.ToArray().Reverse());
			List<long> usedAddresses = new List<long>();

			foreach (BlastUnit bu in bul)
			{
				if (!usedAddresses.Contains(bu.Address) && !bu.IsLocked)
					usedAddresses.Add(bu.Address);
				else
				{
					bs.Remove(bu);
				}
			}

		}

		private void rasterizeVMDsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			foreach (BlastUnit bu in bs)
			{
				bu.RasterizeVMDs();
			}

			updateMaximum(dgvBlastEditor.Rows.Cast<DataGridViewRow>().ToList());
			dgvBlastEditor.Refresh();
			UpdateBottom();
		}

		private void runRomWithoutBlastlayerToolStripMenuItem_Click(object sender, EventArgs e)
		{
			currentSK.RunOriginal();
		}

		private void replaceRomFromGHToolStripMenuItem_Click(object sender, EventArgs e)
		{
			StashKey temp = StockpileManager_UISide.GetCurrentSavestateStashkey();

			if (temp == null)
			{
				MessageBox.Show("There is no savestate selected in the Glitch Harvester, or the current selected box is empty");
				return;
			}
			currentSK.ParentKey = null;
			currentSK.RomFilename = temp.RomFilename;
			currentSK.RomData = temp.RomData;
			currentSK.GameName = temp.GameName;
			currentSK.SystemName = temp.SystemName;
			currentSK.SystemDeepName = temp.SystemDeepName;
			currentSK.SystemCore = temp.SystemCore;
			currentSK.SyncSettings = temp.SyncSettings;
		}

		private void replaceRomFromFileToolStripMenuItem_Click(object sender, EventArgs e)
		{

			DialogResult dialogResult = MessageBox.Show("Loading this rom will invalidate the associated savestate. You'll need to set a new savestate for the Blastlayer. Continue?", "Invalidate State?", MessageBoxButtons.YesNo);
			if (dialogResult == DialogResult.Yes)
			{
				string filename;
				OpenFileDialog ofd = new OpenFileDialog
				{
					Title = "Open ROM File",
					Filter = "any file|*.*",
					RestoreDirectory = true
				};
				if (ofd.ShowDialog() == DialogResult.OK)
				{
					filename = ofd.FileName.ToString();
				}
				else
					return;

				LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_LOADROM, filename, true);

				StashKey temp = new StashKey(CorruptCore.CorruptCore.GetRandomKey(), currentSK.ParentKey, currentSK.BlastLayer);

				// We have to null this as to properly create a stashkey, we need to use it in the constructor,
				// but then the user needs to provide a savestate
				currentSK.ParentKey = null;
				
				currentSK.RomFilename = temp.RomFilename;
				currentSK.GameName = temp.GameName;
				currentSK.SystemName = temp.SystemName;
				currentSK.SystemDeepName = temp.SystemDeepName;
				currentSK.SystemCore = temp.SystemCore;
				currentSK.SyncSettings = temp.SyncSettings;
			}
		}

		private void bakeROMBlastunitsToFileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string[] originalFilename = currentSK.RomFilename.Split('\\');
			string filename;
			SaveFileDialog sfd = new SaveFileDialog();
			//sfd.DefaultExt = "rom";
			sfd.FileName = originalFilename[originalFilename.Length - 1];
			sfd.Title = "Save Rom File";
			sfd.Filter = "rom files|*.*";
			sfd.RestoreDirectory = true;
			if (sfd.ShowDialog() == DialogResult.OK)
				filename = sfd.FileName.ToString();
			else
				return;
			RomParts rp = MemoryDomains.GetRomParts(currentSK.SystemName, currentSK.RomFilename);

			File.Copy(currentSK.RomFilename, filename, true);
			using (FileStream output = new FileStream(filename, FileMode.Open))
			{
				foreach (BlastUnit bu in currentSK.BlastLayer.Layer)
				{
					if (bu.Source == BlastUnitSource.VALUE)
					{
						//We don't want to modify the original
						byte[] outvalue = (byte[])bu.Value.Clone();
						CorruptCore_Extensions.AddValueToByteArrayUnchecked(ref outvalue, bu.TiltValue, bu.BigEndian);
						//Flip it if it's big endian
						if (bu.BigEndian)
							outvalue.FlipBytes();

						if (bu.Domain == rp.PrimaryDomain)
						{
							output.Position = bu.Address + rp.SkipBytes;
							output.Write(outvalue, 0, outvalue.Length);
						}
						else if (bu.Domain == rp.SecondDomain)
						{
							output.Position = bu.Address + MemoryDomains.MemoryInterfaces[rp.SecondDomain].Size + rp.SkipBytes;
							output.Write(outvalue, 0, outvalue.Length);
						}
					}
				}
			}
		}

		private void runOriginalSavestateToolStripMenuItem_Click(object sender, EventArgs e)
		{
			originalSK.RunOriginal();
		}

		private void replaceSavestateFromGHToolStripMenuItem_Click(object sender, EventArgs e)
		{

			StashKey temp = StockpileManager_UISide.GetCurrentSavestateStashkey();
			if (temp == null)
			{
				MessageBox.Show("There is no savestate selected in the glitch harvester, or the current selected box is empty");
				return;
			}

			//If the core doesn't match, abort
			if (currentSK.SystemCore != temp.SystemCore)
			{
				MessageBox.Show("The core associated with the current ROM and the core associated with the selected savestate don't match. Aborting!");
				return;
			}

			//If the game name differs, make sure they know what they're doing
			//There are times it'd be fine with a differing name yet savestates would still work (romhacks)
			if (currentSK.GameName != temp.GameName)
			{
				DialogResult dialogResult = MessageBox.Show(
					"You're attempting to replace a savestate associated with " +
					currentSK.GameName +
					" with a savestate associated with " +
					temp.GameName + ".\n" +
					"This probably won't work unless you also update the ROM.\n" +
					"Updating the ROM will invalidate the savestate, so if you're changing both ROM and state, do that first.\n\n" +
					"Are you sure you want to continue?", "Game mismatch", MessageBoxButtons.YesNo);
				if (dialogResult == DialogResult.No)
				{
					return;
				}
			}

			//We only need the ParentKey and the SyncSettings here as everything else will match
			currentSK.ParentKey = temp.ParentKey;
			currentSK.SyncSettings = temp.SyncSettings;
			currentSK.StateLocation = temp.StateLocation;
		}

		private void replaceSavestateFromFileToolStripMenuItem_Click(object sender, EventArgs e)
		{

			string filename;

			OpenFileDialog ofd = new OpenFileDialog
			{
				DefaultExt = "state",
				Title = "Open Savestate File",
				Filter = "state files|*.state",
				RestoreDirectory = true
			};
			if (ofd.ShowDialog() == DialogResult.OK)
			{
				filename = ofd.FileName.ToString();
			}
			else
				return;

			string oldKey = currentSK.ParentKey;
			string oldSS = currentSK.SyncSettings;

			//Get a new key
			currentSK.ParentKey = CorruptCore.CorruptCore.GetRandomKey();
			//Null the syncsettings out
			currentSK.SyncSettings = null;

			//Let's hope the game name is correct!
			File.Copy(filename, currentSK.GetSavestateFullPath(), true);

			//Attempt to load and if it fails, don't let them update it.
			if (!StockpileManager_UISide.LoadState(currentSK))
			{
				currentSK.ParentKey = oldKey;
				currentSK.SyncSettings = oldSS;
				return;
			}

			//Grab the syncsettings
			StashKey temp = new StashKey(CorruptCore.CorruptCore.GetRandomKey(), currentSK.ParentKey, currentSK.BlastLayer);
			currentSK.SyncSettings = temp.SyncSettings;
		}

		private void saveSavestateToToolStripMenuItem_Click(object sender, EventArgs e)
		{

			string filename;
			SaveFileDialog ofd = new SaveFileDialog
			{
				DefaultExt = "state",
				Title = "Save Savestate File",
				Filter = "state files|*.state",
				RestoreDirectory = true
			};
			if (ofd.ShowDialog() == DialogResult.OK)
			{
				filename = ofd.FileName;
			}
			else
				return;

			File.Copy(currentSK.GetSavestateFullPath(), filename, true);
		}

		private void loadFromFileblToolStripMenuItem_Click(object sender, EventArgs e)
		{

			BlastLayer temp = BlastTools.LoadBlastLayerFromFile();
			if (temp != null)
			{
				currentSK.BlastLayer = temp;
				bs = new BindingSource {DataSource = new SortableBindingList<BlastUnit>(currentSK.BlastLayer.Layer)};
			}
			dgvBlastEditor.DataSource = bs;
			dgvBlastEditor.ResetBindings();
			dgvBlastEditor.Refresh();
		}

		private void saveToFileblToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//If there's no blastlayer file already set, don't quicksave
			if (CurrentBlastLayerFile == "")
				BlastTools.SaveBlastLayerToFile(currentSK.BlastLayer);
			else
				BlastTools.SaveBlastLayerToFile(currentSK.BlastLayer, CurrentBlastLayerFile);

			CurrentBlastLayerFile = BlastTools.LastBlastLayerSavePath;
		}

		private void saveAsToFileblToolStripMenuItem_Click(object sender, EventArgs e)
		{
			BlastTools.SaveBlastLayerToFile(currentSK.BlastLayer);
			CurrentBlastLayerFile = BlastTools.LastBlastLayerSavePath;
		}

		private void importBlastlayerblToolStripMenuItem_Click(object sender, EventArgs e)
		{
			BlastLayer temp = BlastTools.LoadBlastLayerFromFile();
			ImportBlastLayer(temp);
		}

		public void ImportBlastLayer(BlastLayer bl)
		{
			if (bl != null)
			{
				foreach (BlastUnit bu in bl.Layer)
					bs.Add(bu);
			}
			dgvBlastEditor.ResetBindings();
			RefreshAllNoteIcons();
			dgvBlastEditor.Refresh();
		}

		private void exportToCSVToolStripMenuItem_Click(object sender, EventArgs e)
		{

			string filename;

			if (currentSK.BlastLayer.Layer.Count == 0)
			{
				MessageBox.Show("Can't save because the provided blastlayer is empty.");
				return;
			}

			SaveFileDialog saveFileDialog1 = new SaveFileDialog
			{
				DefaultExt = "csv",
				Title = "Export to csv",
				Filter = "csv files|*.csv",
				RestoreDirectory = true
			};

			if (saveFileDialog1.ShowDialog() == DialogResult.OK)
			{
				filename = saveFileDialog1.FileName;
			}
			else
				return;
			CSVGenerator csv = new CSVGenerator();
			File.WriteAllText(filename, csv.GenerateFromDGV(dgvBlastEditor), Encoding.UTF8);
		}

		private void bakeBlastunitsToVALUEToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				//Generate a blastlayer from the current selected rows
				BlastLayer bl = new BlastLayer();
				foreach (DataGridViewRow selected in dgvBlastEditor.SelectedRows.Cast<DataGridViewRow>()
					.Where((item => ((BlastUnit)item.DataBoundItem).IsLocked == false)))
				{
					BlastUnit bu = (BlastUnit)selected.DataBoundItem;
					
					//They have to be enabled to get a backup
					bu.IsEnabled = true;
					bl.Layer.Add(bu);
				}

				//Bake them
				BlastLayer newBlastLayer = LocalNetCoreRouter.QueryRoute<BlastLayer>(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_BLASTTOOLS_GETAPPLIEDBACKUPLAYER, new object[] {bl, currentSK}, true);

				int i = 0;
				//Insert the new one where the old row was, then remove the old row.
				foreach (DataGridViewRow selected in dgvBlastEditor.SelectedRows.Cast<DataGridViewRow>()
					.Where((item => ((BlastUnit)item.DataBoundItem).IsLocked == false)))
				{
					bs.Insert(selected.Index, newBlastLayer.Layer[i]);
					i++;
					bs.Remove((BlastUnit)selected.DataBoundItem);
				}
			}
			catch (Exception ex)
			{
				throw new System.Exception("Something went wrong in when baking to VALUE.\n" +
				                           "Your blast editor session may be broke depending on when it failed.\n" +
				                           "You should probably send a copy of this error and what you did to cause it to the RTC devs.\n\n" +
				                           ex.ToString());
			}
			finally
			{
			}
		}

		public void btnLoadCorrupt_Click(object sender, EventArgs e)
		{

			if (currentSK.ParentKey == null)
			{
				MessageBox.Show("There's no savestate associated with this Stashkey!\nAssociate one in the menu to be able to load.");
				return;
			}
			
			StashKey newSk = (StashKey)currentSK.Clone();
			newSk.Run();
		}

		public void btnCorrupt_Click(object sender, EventArgs e)
		{
			StashKey newSk = (StashKey)currentSK.Clone();
			StockpileManager_UISide.ApplyStashkey(newSk, false);
		}

		public void RefreshNoteIcons(DataGridViewRowCollection rows)
		{
			foreach(DataGridViewRow row in rows)
			{
				DataGridViewCell buttonCell = row.Cells[buProperty.Note.ToString()];
				buttonCell.Value = string.IsNullOrWhiteSpace((row.DataBoundItem as BlastUnit)?.Note) ? string.Empty : "📝";
			}
		}
		public void RefreshAllNoteIcons()
		{
			RefreshNoteIcons(dgvBlastEditor.Rows);
		}


		private void DgvBlastEditor_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
		{
			UpdateLayerSize();
		}

		private void DgvBlastEditor_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
		{
			UpdateLayerSize();
		}


		public void btnShiftBlastLayerDown_Click(object sender, EventArgs e)
		{
			var amount = updownShiftBlastLayerAmount.Value;
			var column = ((ComboBoxItem<String>)cbShiftBlastlayer?.SelectedItem)?.Value;

			if (column == null)
				return;

			var rows = dgvBlastEditor.SelectedRows.Cast<DataGridViewRow>()
				.Where((item => ((BlastUnit)item.DataBoundItem).IsLocked == false))
				.ToList();
			ShiftBlastLayer(amount, column, rows, true);
		}

		public void btnShiftBlastLayerUp_Click(object sender, EventArgs e)
		{
			var amount = updownShiftBlastLayerAmount.Value;
			var column = ((ComboBoxItem<String>)cbShiftBlastlayer?.SelectedItem)?.Value;

			if (column == null)
				return;

			var rows = dgvBlastEditor.SelectedRows.Cast<DataGridViewRow>()
				.Where((item => ((BlastUnit) item.DataBoundItem).IsLocked == false))
				.ToList();
			ShiftBlastLayer(amount, column, rows, false);
		}


		private void ShiftBlastLayer(decimal amount, string column, List<DataGridViewRow> rows, bool shiftDown)
		{
			foreach (DataGridViewRow row in rows) 
			{
				var cell = row.Cells[column];

				//Can't use a switch statement because tostring is evaluated at runtime
				if (cell is DataGridViewNumericUpDownCell u)
					{
						if (shiftDown)
							{
								if ((Convert.ToInt64(u.Value) - amount) >= 0)
									u.Value = Convert.ToInt64(u.Value) - amount;
								else
									u.Value = 0;
						}
						else
						{
							if ((Convert.ToInt64(u.Value) + amount) <= u.Maximum)
								u.Value = Convert.ToInt64(u.Value) + amount;
							else
								u.Value = u.Maximum;
						}
					}
					else if (cell.OwningColumn.Name == buProperty.ValueString.ToString())
					{
					if (shiftDown)
						amount = 0 - amount;
						int precision = (int)row.Cells[buProperty.Precision.ToString()].Value;
						cell.Value = getShiftedHexString((string)cell.Value, amount, precision);
					}
					else
					{
						throw new NotImplementedException("Invalid column type.");
					}

				}
			dgvBlastEditor.Refresh();
			UpdateBottom();
		}

		private string getShiftedHexString(string value, decimal amount, int precision)
		{
			//Convert the string we have into a byte array
			var valueBytes= CorruptCore_Extensions.StringToByteArrayPadLeft(value, precision);
			if (valueBytes == null)
				return value;
			CorruptCore_Extensions.AddValueToByteArrayUnchecked(ref valueBytes, new BigInteger(amount), true);
			return BitConverter.ToString(valueBytes).Replace("-", string.Empty);
		}

		private void btnHelp_Click(object sender, EventArgs e)
		{
			System.Diagnostics.ProcessStartInfo sInfo = new System.Diagnostics.ProcessStartInfo("https://corrupt.wiki/corruptors/rtc-real-time-corruptor/blast-editor.html");
			System.Diagnostics.Process.Start(sInfo);
		}

		private void OpenBlastGeneratorToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var bgForm = S.GET<RTC_BlastGenerator_Form>();

			if (S.GET<RTC_BlastGenerator_Form>() != null)
				S.GET<RTC_BlastGenerator_Form>().Close();
			S.SET(new RTC_BlastGenerator_Form());
			bgForm = S.GET<RTC_BlastGenerator_Form>();
			bgForm.LoadStashkey(currentSK);
		}

		private void BtnAddRow_Click(object sender, EventArgs e)
		{
			BlastUnit bu = new BlastUnit(new byte[] {0}, domains[0], 0, 1, MemoryDomains.GetInterface(domains[0]).BigEndian);
			bs.Add(bu);
		}

		private void UpdateLayerSize()
		{
			lbBlastLayerSize.Text = "Size: " + currentSK.BlastLayer.Layer.Count;
		}

		public StashKey[] GetStashKeys()
		{
			return new[] {currentSK, originalSK};
		}
	}
}
