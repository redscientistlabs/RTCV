/*
 * The DataGridView is bound to the blastlayer
 * All validation is done within the dgv
 * The boxes at the bottom are unbound and manipulate the selected rows in the dgv, and thus, the validation is handled by the dgv
 * No maxmimum is set in the numericupdowns at the bottom as the dgv validates
 */

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
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Numerics;
    using System.Text;
    using System.Windows.Forms;
    using RTCV.CorruptCore;
    using RTCV.CorruptCore.Extensions;
    using RTCV.NetCore;
    using RTCV.Common;
    using RTCV.UI.Components;

    #pragma warning disable CA2213 //Component designer classes generate their own Dispose method
    public partial class BlastEditorForm : Modular.ColorizedForm
    {
        private const int buttonFillWeight = 20;
        private const int checkBoxFillWeight = 25;
        private const int comboBoxFillWeight = 40;
        private const int textBoxFillWeight = 30;
        private const int numericUpDownFillWeight = 35;

        private Dictionary<string, MemoryInterface> _domainToMiDico;

        private Dictionary<string, MemoryInterface> DomainToMiDico
        {
            get => _domainToMiDico ?? (_domainToMiDico = new Dictionary<string, MemoryInterface>());
            set => _domainToMiDico = value;
        }

        private string[] _domains;
        public List<string> VisibleColumns { get; set; }
        private string CurrentBlastLayerFile = "";
        private bool batchOperation;
        private ContextMenuStrip headerStrip;
        private ContextMenuStrip cms;
        private Dictionary<string, Control> property2ControlDico;
        private NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private enum BuProperty
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
            LoopTiming,
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
        //    private object actionTimeValues =

        public BlastEditorForm()
        {
            try
            {
                InitializeComponent();

                dgvBlastEditor.AutoGenerateColumns = false;

                registerValueStringScrollEvents();

                //On today's episode of "why is the designer overriding these values every time I build"
                upDownExecuteFrame.Maximum = int.MaxValue;
                upDownLoopTiming.Maximum = int.MaxValue;
                upDownPrecision.Maximum = 16348; //Textbox doesn't like more than ~20k
                upDownLifetime.Maximum = int.MaxValue;
                upDownSourceAddress.Maximum = int.MaxValue;
                upDownAddress.Maximum = int.MaxValue;
                dontShowBlastlayerNameInTitleToolStripMenuItem.Checked = Params.IsParamSet("DONT_SHOW_BLASTLAYER_NAME_IN_EDITOR");
            }
            catch (Exception ex)
            {
                if (CloudDebug.ShowErrorDialog(ex, true) == DialogResult.Abort)
                {
                    throw new AbortEverythingException();
                }
            }
        }

        private void OnFormDragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            foreach (var f in files)
            {
                if (f.Contains(".bl"))
                {
                    BlastLayer temp = BlastTools.LoadBlastLayerFromFile(f);
                    ImportBlastLayer(temp);
                }
            }
        }

        private void OnFormDragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Link;
        }

        public static bool OpenBlastEditor(StashKey sk = null, bool silent = false)
        {
            if (S.GET<BlastEditorForm>().Visible)
            {
                silent = false;
            }

            //S.GET<BlastEditorForm>().Close();
            var BEF = new BlastEditorForm();

            S.SET(BEF);

            if (sk == null)
            {
                sk = new StashKey();
            }

            //If the blastlayer is big, prompt them before opening it. Let's go with 5k for now.

            //TODO
            if (sk.BlastLayer.Layer.Count > 5000 && (DialogResult.Yes == MessageBox.Show($"You're trying to open a blastlayer of size " + sk.BlastLayer.Layer.Count + ". This could take a while. Are you sure you want to continue?", "Opening a large BlastLayer", MessageBoxButtons.YesNo)))
            {
                BEF.LoadStashkey(sk, silent);
                return true;
            }
            else if (sk.BlastLayer.Layer.Count <= 5000)
            {
                BEF.LoadStashkey(sk, silent);
                return true;
            }
            else
            {
                return false;
            }
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            _domains = MemoryDomains.MemoryInterfaces?.Keys?.Concat(MemoryDomains.VmdPool.Values.Select(it => it.ToString())).ToArray();

            dgvBlastEditor.AllowUserToOrderColumns = true;
            SetDisplayOrder();
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e) => SaveDisplayOrder();

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            //Clean up
            bs = null;
            _bs = null;
            currentSK = null;
            originalSK = null;
            DomainToMiDico = null;
            //Force cleanup
            GC.Collect();
            GC.WaitForPendingFinalizers();
            this.Dispose();
        }

        private void registerValueStringScrollEvents()
        {
            tbValue.MouseWheel += tbValueScroll;
            dgvBlastEditor.MouseWheel += OnBlastEditorMouseWheel;
        }

        private void OnBlastEditorMouseWheel(object sender, MouseEventArgs e)
        {
            var owningRow = dgvBlastEditor.CurrentCell?.OwningRow;

            if (dgvBlastEditor.CurrentCell == owningRow?.Cells[BuProperty.ValueString.ToString()] && dgvBlastEditor.IsCurrentCellInEditMode)
            {
                var precision = (int)dgvBlastEditor.CurrentCell.OwningRow.Cells[BuProperty.Precision.ToString()].Value;
                dgvCellValueScroll(dgvBlastEditor.EditingControl, e, precision);

                ((HandledMouseEventArgs)e).Handled = true;
            }
        }

        private static void dgvCellValueScroll(object sender, MouseEventArgs e, int precision)
        {
            if (sender is TextBox tb)
            {
                var scrollBy = 1;
                if (e.Delta < 0)
                {
                    scrollBy = -1;
                }

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
            {
                return;
            }
            //Names split with commas
            var s = Params.ReadParam("BLASTEDITOR_COLUMN_ORDER");
            var order = s.Split(',');

            //Use a foreach and keep track in-case the number of entries changes
            var i = 0;
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
            var sb = new StringBuilder();
            foreach (var c in cols)
            {
                sb.Append(c.Name + ",");
            }

            Params.SetParam("BLASTEDITOR_COLUMN_ORDER", sb.ToString());
        }

        private void OnBlastEditorMouseClick(object sender, MouseEventArgs e)
        {
            //Exit edit mode if you click away from a cell
            var ht = dgvBlastEditor.HitTest(e.X, e.Y);

            if (ht.Type != DataGridViewHitTestType.Cell)
            {
                dgvBlastEditor.EndEdit();
            }
        }

        private void OnBlastEditorCellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Note handling
            if (e != null && e.RowIndex != -1 &&
                e.ColumnIndex == dgvBlastEditor.Columns[BuProperty.Note.ToString()]?.Index &&
                dgvBlastEditor.Rows[e.RowIndex].DataBoundItem is BlastUnit bu)
            {
                S.SET(new NoteEditorForm(bu, dgvBlastEditor[e.ColumnIndex, e.RowIndex]));
                S.GET<NoteEditorForm>().Show();
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
                if (dgvBlastEditor.CurrentCell != null && dgvBlastEditor.CurrentCell.ColumnIndex != e.ColumnIndex)
                {
                    dgvBlastEditor.EndEdit();
                }

                cms = new ContextMenuStrip();

                if (e.RowIndex != -1 && e.ColumnIndex != -1)
                {
                    PopulateGenericContextMenu();
                    //Can't use a switch statement because dynamic
                    if (dgvBlastEditor.Columns[e.ColumnIndex] == dgvBlastEditor.Columns[BuProperty.Address.ToString()] ||
                        dgvBlastEditor.Columns[e.ColumnIndex] == dgvBlastEditor.Columns[BuProperty.SourceAddress.ToString()])
                    {
                        cms.Items.Add(new ToolStripSeparator());
                        PopulateAddressContextMenu(dgvBlastEditor[e.ColumnIndex, e.RowIndex]);
                    }
                    cms.Show(dgvBlastEditor, dgvBlastEditor.PointToClient(Cursor.Position));
                }
            }
        }

        private void OnBlastEditorCellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
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

                //generate temporary blastlayer for batch processing
                List<BlastUnit> layer = new List<BlastUnit>();
                foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
                {
                    var bu = (BlastUnit)row.DataBoundItem;
                    layer.Add(bu);
                }
                var tempBl = new BlastLayer(layer);

                //Ensure reroll is tone on the Vanguard CorruptCore
                var rerolledBl = LocalNetCoreRouter.QueryRoute<BlastLayer>(NetCore.Endpoints.CorruptCore, NetCore.Commands.Remote.RerollBlastLayer, tempBl, true);

                //update BlastUnit with new data
                for (int i =0;i< rerolledBl.Layer.Count;i++)
                {
                    var bu = tempBl.Layer[i];
                    var newBu = rerolledBl.Layer[i];

                    bu.Domain = newBu.Domain;
                    bu.Address = newBu.Address;
                    bu.Value = newBu.Value;
                    bu.SourceAddress = newBu.SourceAddress;
                    bu.SourceDomain = newBu.SourceDomain;
                }

                dgvBlastEditor.Refresh();
                UpdateBottom();
            }))).Enabled = true;

            ((ToolStripMenuItem)cms.Items.Add("Break Down Selected Unit(s)", null, new EventHandler((ob, ev) =>
            {
                BreakDownUnits(true);
            }))).Enabled = dgvBlastEditor.SelectedRows.Count > 0;

            ((ToolStripMenuItem)cms.Items.Add("Bake Selected Unit(s) to VALUE", null, new EventHandler((ob, ev) =>
            {
                BakeBlastUnitsToValue(true);
            }))).Enabled = dgvBlastEditor.SelectedRows.Count > 0;
        }

        public void BreakDownUnits(bool breakSelected = false)
        {
            List<DataGridViewRow> targetRows;

            if (breakSelected)
            {
                targetRows = dgvBlastEditor.SelectedRows.Cast<DataGridViewRow>().ToList();
            }
            else
            {
                targetRows = dgvBlastEditor.Rows.Cast<DataGridViewRow>().ToList();
            }

            //Important we ToArray() this or else the ienumerable will become invalidated
            var blastUnits = targetRows.Select(x => (BlastUnit)x.DataBoundItem).ToArray();

            dgvBlastEditor.DataSource = null;
            batchOperation = true;

            foreach (var bu in blastUnits)
            {
                BlastUnit[] brokenUnits = bu.GetBreakdown();

                if (brokenUnits == null || brokenUnits.Length < 2)
                {
                    continue;
                }

                foreach (BlastUnit unit in brokenUnits)
                {
                    bs.Add(unit);
                }
            }

            bs = new BindingSource { DataSource = new SortableBindingList<BlastUnit>(currentSK.BlastLayer.Layer) };
            batchOperation = false;
            dgvBlastEditor.DataSource = bs;
            updateMaximum(this, dgvBlastEditor.Rows.Cast<DataGridViewRow>().ToList());
            dgvBlastEditor.Refresh();
            UpdateBottom();
        }

        private void PopulateAddressContextMenu(DataGridViewCell cell)
        {
            ((ToolStripMenuItem)cms.Items.Add("Open Selected Address in Hex Editor", null, new EventHandler((ob, ev) =>
            {
                if (!(dgvBlastEditor.Rows[cell.RowIndex]?.DataBoundItem is BlastUnit bu))
                {
                    return;
                }

                if (cell.OwningColumn == dgvBlastEditor.Columns[BuProperty.Address.ToString()])
                {
                    LocalNetCoreRouter.Route(NetCore.Endpoints.CorruptCore, NetCore.Commands.Emulator.OpenHexEditorAddress, new object[] { bu.Domain, bu.Address });
                }

                if (cell.OwningColumn == dgvBlastEditor.Columns[BuProperty.SourceAddress.ToString()])
                {
                    LocalNetCoreRouter.Route(NetCore.Endpoints.CorruptCore, NetCore.Commands.Emulator.OpenHexEditorAddress, new object[] { bu.SourceDomain, bu.SourceAddress });
                }
            }))).Enabled = true;
        }

        private void OnBlastEditorCellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewColumn changedColumn = dgvBlastEditor.Columns[e.ColumnIndex];

            //If the Domain or SourceDomain changed update the Maximum Value
            if (changedColumn.Name == BuProperty.Domain.ToString())
            {
                updateMaximum(this, dgvBlastEditor.Rows[e.RowIndex].Cells[BuProperty.Address.ToString()] as DataGridViewNumericUpDownCell, dgvBlastEditor.Rows[e.RowIndex].Cells[BuProperty.Domain.ToString()].Value.ToString());
            }
            else if (changedColumn.Name == BuProperty.SourceDomain.ToString())
            {
                updateMaximum(this, dgvBlastEditor.Rows[e.RowIndex].Cells[BuProperty.SourceAddress.ToString()] as DataGridViewNumericUpDownCell, dgvBlastEditor.Rows[e.RowIndex].Cells[BuProperty.SourceDomain.ToString()].Value.ToString());
            }
            UpdateBottom();
        }

        private void OnSourceDomainValidated(object sender, EventArgs e)
        {
            var value = cbSourceDomain.SelectedItem;
            foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows.Cast<DataGridViewRow>().Where(x => (x.DataBoundItem as BlastUnit)?.IsLocked == false))
            {
                row.Cells[BuProperty.SourceDomain.ToString()]
                    .Value = value;
            }

            UpdateBottom();
        }

        private void OnStoreTypeValidated(object sender, EventArgs e)
        {
            var value = cbStoreType.SelectedItem;
            foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows.Cast<DataGridViewRow>().Where(x => (x.DataBoundItem as BlastUnit)?.IsLocked == false))
            {
                row.Cells[BuProperty.StoreType.ToString()].Value = value;
            }

            UpdateBottom();
        }

        private void OnStoreTimeValidated(object sender, EventArgs e)
        {
            var value = cbStoreTime.SelectedItem;
            foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows.Cast<DataGridViewRow>().Where(x => (x.DataBoundItem as BlastUnit)?.IsLocked == false))
            {
                row.Cells[BuProperty.StoreTime.ToString()].Value = value;
            }

            UpdateBottom();
        }

        private void OnLimiterListValidated(object sender, EventArgs e)
        {
            var value = ((ComboBoxItem<string>)(cbLimiterList?.SelectedItem))?.Value ?? null;
            foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows.Cast<DataGridViewRow>().Where(x => (x.DataBoundItem as BlastUnit)?.IsLocked == false))
            {
                row.Cells[BuProperty.LimiterListHash.ToString()].Value = value; // We gotta use the value
            }

            UpdateBottom();
        }

        private void OnBigEndianValidated(object sender, EventArgs e)
        {
            var value = cbBigEndian.Checked;
            //Big Endian isn't available in the DGV so we operate on the actual BU then refresh
            //Todo - change this?
            foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows.Cast<DataGridViewRow>().Where(x => (x.DataBoundItem as BlastUnit)?.IsLocked == false))
            {
                ((BlastUnit)row.DataBoundItem).BigEndian = value;
            }
            dgvBlastEditor.Refresh();
            UpdateBottom();
        }

        private void OnValueValidated(object sender, EventArgs e)
        {
            var value = tbValue.Text;
            foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows.Cast<DataGridViewRow>().Where(x => (x.DataBoundItem as BlastUnit)?.IsLocked == false))
            {
                row.Cells[BuProperty.ValueString.ToString()].Value = value;
            }

            UpdateBottom();
        }

        private void OnSourceValidated(object sender, EventArgs e)
        {
            var value = cbSource.SelectedItem;
            foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows.Cast<DataGridViewRow>().Where(x => (x.DataBoundItem as BlastUnit)?.IsLocked == false))
            {
                row.Cells[BuProperty.Source.ToString()].Value = value;
            }

            UpdateBottom();
        }

        private void OnTiltValueValidated(object sender, EventArgs e)
        {
            if (!BigInteger.TryParse(tbTiltValue.Text, out BigInteger value))
            {
                value = 0;
            }

            //Tilt isn't stored within the DGV so operate on the BUs. No validation neccesary as it's a bigint
            foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows.Cast<DataGridViewRow>().Where(x => (x.DataBoundItem as BlastUnit)?.IsLocked == false))
            {
                (row.DataBoundItem as BlastUnit).TiltValue = value;
            }
            UpdateBottom();
        }

        private void OnLifetimeValidated(object sender, EventArgs e)
        {
            var value = upDownLifetime.Value;
            if (value > int.MaxValue)
            {
                value = int.MaxValue;
            }

            foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows.Cast<DataGridViewRow>().Where(x => (x.DataBoundItem as BlastUnit)?.IsLocked == false))
            {
                row.Cells[BuProperty.Lifetime.ToString()].Value = value;
            }

            UpdateBottom();
            dgvBlastEditor.Refresh();
        }

        private void OnExecuteFrameValidated(object sender, EventArgs e)
        {
            var value = upDownExecuteFrame.Value;
            if (value > int.MaxValue)
            {
                value = int.MaxValue;
            }

            foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows.Cast<DataGridViewRow>().Where(x => (x.DataBoundItem as BlastUnit)?.IsLocked == false))
            {
                row.Cells[BuProperty.ExecuteFrame.ToString()].Value = value;
            }

            UpdateBottom();
            dgvBlastEditor.Refresh();
        }

        private void OnLoopTimingValidated(object sender, EventArgs e)
        {
            var value = upDownLoopTiming.Value;
            if (value > int.MaxValue)
            {
                value = int.MaxValue;
            }

            foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows.Cast<DataGridViewRow>().Where(x => (x.DataBoundItem as BlastUnit)?.IsLocked == false))
            {
                row.Cells[BuProperty.LoopTiming.ToString()].Value = (int?)Convert.ToInt32(value);
            }

            UpdateBottom();
            dgvBlastEditor.Refresh();
        }

        private void OnPrecisionValidated(object sender, EventArgs e)
        {
            var value = upDownPrecision.Value;

            if (value > int.MaxValue)
            {
                value = int.MaxValue;
            }

            foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows.Cast<DataGridViewRow>().Where(x => (x.DataBoundItem as BlastUnit)?.IsLocked == false))
            {
                row.Cells[BuProperty.Precision.ToString()].Value = value;
            }

            UpdateBottom();
            dgvBlastEditor.Refresh();
        }

        private void OnAddressValidated(object sender, EventArgs e)
        {
            var value = upDownAddress.Value;
            if (value > int.MaxValue)
            {
                value = int.MaxValue;
            }

            foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows.Cast<DataGridViewRow>().Where(x => (x.DataBoundItem as BlastUnit)?.IsLocked == false))
            {
                row.Cells[BuProperty.Address.ToString()].Value = value;
            }

            UpdateBottom();
        }

        private void OnSourceAddressValidated(object sender, EventArgs e)
        {
            var value = upDownSourceAddress.Value;
            if (value > int.MaxValue)
            {
                value = int.MaxValue;
            }

            foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows.Cast<DataGridViewRow>().Where(x => (x.DataBoundItem as BlastUnit)?.IsLocked == false))
            {
                row.Cells[BuProperty.SourceAddress.ToString()].Value = value;
            }

            UpdateBottom();
        }

        private void OnLockedValidated(object sender, EventArgs e)
        {
            var value = cbLocked.Checked;
            foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
            {
                row.Cells[BuProperty.isLocked.ToString()].Value = value;
            }

            UpdateBottom();
        }

        private void OnLimiterTimeValidated(object sender, EventArgs e)
        {
            var value = cbLimiterTime.SelectedItem;
            foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows.Cast<DataGridViewRow>().Where(x => (x.DataBoundItem as BlastUnit)?.IsLocked == false))
            {
                row.Cells[BuProperty.LimiterTime.ToString()].Value = value;
            }

            UpdateBottom();
        }

        private void OnStoreLimiterSourceValidated(object sender, EventArgs e)
        {
            var value = cbStoreLimiterSource.SelectedItem;
            foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows.Cast<DataGridViewRow>().Where(x => (x.DataBoundItem as BlastUnit)?.IsLocked == false))
            {
                row.Cells[BuProperty.StoreLimiterSource.ToString()].Value = value;
            }

            UpdateBottom();
        }

        private void OnInvertLimiterValidated(object sender, EventArgs e)
        {
            var value = cbInvertLimiter.Checked;
            foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows.Cast<DataGridViewRow>().Where(x => (x.DataBoundItem as BlastUnit)?.IsLocked == false))
            {
                row.Cells[BuProperty.InvertLimiter.ToString()].Value = value;
            }

            UpdateBottom();
        }

        private void OnEnabledValidated(object sender, EventArgs e)
        {
            var value = cbEnabled.Checked;
            foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows.Cast<DataGridViewRow>().Where(x => (x.DataBoundItem as BlastUnit)?.IsLocked == false))
            {
                row.Cells[BuProperty.isEnabled.ToString()].Value = value;
            }

            UpdateBottom();
        }

        private void OnDomainValidated(object sender, EventArgs e)
        {
            var value = cbDomain.SelectedItem;

            if (!_domains.Contains(value))
            {
                return;
            }

            foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows.Cast<DataGridViewRow>().Where(x => (x.DataBoundItem as BlastUnit)?.IsLocked == false))
            {
                row.Cells[BuProperty.Domain.ToString()].Value = value;
            }

            UpdateBottom();
        }

        private void OnLoopValidated(object sender, EventArgs e)
        {
            var value = cbLoop.Checked;
            foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows.Cast<DataGridViewRow>().Where(x => (x.DataBoundItem as BlastUnit)?.IsLocked == false))
            {
                row.Cells[BuProperty.Loop.ToString()].Value = value;
            }

            UpdateBottom();
        }

        private void OnBlastEditorColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                headerStrip = new ContextMenuStrip();
                headerStrip.Items.Add("Select columns to show", null, new EventHandler((ob, ev) =>
                {
                    var cs = new ColumnSelector();
                    cs.LoadColumnSelector(dgvBlastEditor.Columns);
                }));

                headerStrip.Show(MousePosition);
            }

            RefreshAllNoteIcons();
        }

        private static void updateMaximum(BlastEditorForm BEF, List<DataGridViewRow> rows)
        {
            foreach (DataGridViewRow row in rows)
            {
                var bu = row.DataBoundItem as BlastUnit;
                var domain = bu.Domain;
                var sourceDomain = bu.SourceDomain;

                if (domain != null && BEF.DomainToMiDico.ContainsKey(bu.Domain ?? ""))
                {
                    (row.Cells[BuProperty.Address.ToString()] as DataGridViewNumericUpDownCell).Maximum = BEF.DomainToMiDico[domain].Size - 1;
                }

                if (sourceDomain != null && BEF.DomainToMiDico.ContainsKey(bu.SourceDomain ?? ""))
                {
                    (row.Cells[BuProperty.SourceAddress.ToString()] as DataGridViewNumericUpDownCell).Maximum = BEF.DomainToMiDico[sourceDomain].Size - 1;
                }
            }
        }

        private static void updateMaximum(BlastEditorForm BEF, DataGridViewNumericUpDownCell cell, string domain)
        {
            if (BEF.DomainToMiDico.ContainsKey(domain))
            {
                cell.Maximum = BEF.DomainToMiDico[domain].Size - 1;
            }
            else
            {
                cell.Maximum = int.MaxValue;
            }
        }

        private void UpdateBottom()
        {
            if (dgvBlastEditor.SelectedRows.Count > 0)
            {
                var lastRow = dgvBlastEditor.SelectedRows[dgvBlastEditor.SelectedRows.Count - 1];
                var bu = (BlastUnit)lastRow.DataBoundItem;

                if (DomainToMiDico.ContainsKey(bu.Domain ?? string.Empty))
                {
                    upDownAddress.Maximum = DomainToMiDico[bu.Domain].Size - 1;
                }
                else
                {
                    upDownAddress.Maximum = int.MaxValue;
                }

                if (DomainToMiDico.ContainsKey(bu.SourceDomain ?? string.Empty))
                {
                    upDownSourceAddress.Maximum = DomainToMiDico[bu.SourceDomain].Size - 1;
                }
                else
                {
                    upDownSourceAddress.Maximum = int.MaxValue;
                }

                cbDomain.SelectedItem = bu.Domain;
                cbEnabled.Checked = bu.IsEnabled;
                cbLocked.Checked = bu.IsLocked;
                cbBigEndian.Checked = bu.BigEndian;

                upDownAddress.Value = bu.Address;
                upDownPrecision.Value = bu.Precision;
                tbValue.Text = bu.ValueString;
                upDownExecuteFrame.Value = bu.ExecuteFrame;
                upDownLoopTiming.Value = (bu.LoopTiming ?? -1);
                upDownLifetime.Value = bu.Lifetime;
                cbLoop.Checked = bu.Loop;
                cbLimiterTime.SelectedItem = bu.LimiterTime;
                cbStoreLimiterSource.SelectedItem = bu.StoreLimiterSource;

                cbLimiterList.SelectedItem = RtcCore.LimiterListBindingSource.FirstOrDefault(x => x.Value == bu.LimiterListHash);

                cbInvertLimiter.Checked = bu.InvertLimiter;
                cbStoreTime.SelectedItem = bu.StoreTime;
                cbStoreType.SelectedItem = bu.StoreType;
                cbSourceDomain.SelectedItem = bu.SourceDomain;
                cbSource.SelectedItem = bu.Source;
                upDownSourceAddress.Value = bu.SourceAddress;

                tbTiltValue.Text = bu.TiltValue.ToString();
            }
        }

        private void OnBlastEditorSelectionChange(object sender, EventArgs e)
        {
            UpdateBottom();

            var col = new List<DataGridViewRow>();
            //For some reason DataGridViewRowCollection and DataGridViewSelectedRowCollection aren't directly compatible???
            foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
            {
                col.Add(row);
            }

            //Rather than setting all these values at load, we set it on the fly
            updateMaximum(this, col);
        }

        private void OnBlastEditorCellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //Bug in DGV. If you don't read the value back, it goes into edit mode on first click if you read the selectedrow within SelectionChanged. Why? No idea.
            _ = dgvBlastEditor.Rows[e.RowIndex].Cells[0].Value;
        }

        private void OnFilterTextChanged(object sender, EventArgs e)
        {
            if (tbFilter.Text.Length == 0)
            {
                dgvBlastEditor.DataSource = bs;
                _bs = null;
                RefreshAllNoteIcons();
                return;
            }

            var value = ((ComboBoxItem<string>)cbFilterColumn?.SelectedItem)?.Value;
            if (value == null)
            {
                return;
            }

            _bs = new BindingSource();
            switch (((ComboBoxItem<string>)cbFilterColumn.SelectedItem).Value)
            {
                //If it's an address or a source address we want hexadecimal
                case "Address":
                    _bs.DataSource = currentSK.BlastLayer.Layer.Where(x => x.Address.ToString("X").IndexOf(tbFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                    break;
                case "SourceAddress":
                    _bs.DataSource = currentSK.BlastLayer.Layer.Where(x => x.SourceAddress.ToString("X").IndexOf(tbFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                    break;
                default: //Otherwise just use reflection and dig it out
                    _bs.DataSource = currentSK.BlastLayer.Layer.Where(x => x?.GetType().GetProperty(value)?.GetValue(x) != null && x.GetType().GetProperty(value)?.GetValue(x).ToString().IndexOf(tbFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
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
            cbDomain.DataSource = _domains;

            cbSourceDomain.BindingContext = new BindingContext();
            cbSourceDomain.DataSource = _domains;

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

            cbLimiterList.DataSource = RtcCore.LimiterListBindingSource;
            cbLimiterList.DisplayMember = "Name";
            cbLimiterList.ValueMember = "Value";

            cbStoreType.DataSource = storeType;

            property2ControlDico.Add(BuProperty.Address.ToString(), upDownAddress);
            property2ControlDico.Add(BuProperty.Domain.ToString(), cbDomain);
            property2ControlDico.Add(BuProperty.Source.ToString(), cbSource);
            property2ControlDico.Add(BuProperty.ExecuteFrame.ToString(), upDownExecuteFrame);
            property2ControlDico.Add(BuProperty.LoopTiming.ToString(), upDownLoopTiming);
            property2ControlDico.Add(BuProperty.InvertLimiter.ToString(), cbInvertLimiter);
            property2ControlDico.Add(BuProperty.isEnabled.ToString(), cbEnabled);
            property2ControlDico.Add(BuProperty.isLocked.ToString(), cbLocked);
            property2ControlDico.Add(BuProperty.Lifetime.ToString(), upDownLifetime);
            property2ControlDico.Add(BuProperty.LimiterListHash.ToString(), cbLimiterList);
            property2ControlDico.Add(BuProperty.LimiterTime.ToString(), cbLimiterTime);
            property2ControlDico.Add(BuProperty.Loop.ToString(), cbLoop);
            property2ControlDico.Add(BuProperty.Note.ToString(), btnNote);
            property2ControlDico.Add(BuProperty.Precision.ToString(), upDownPrecision);
            property2ControlDico.Add(BuProperty.SourceAddress.ToString(), upDownSourceAddress);
            property2ControlDico.Add(BuProperty.SourceDomain.ToString(), cbSourceDomain);
            property2ControlDico.Add(BuProperty.StoreTime.ToString(), cbStoreTime);
            property2ControlDico.Add(BuProperty.StoreLimiterSource.ToString(), cbStoreTime);
            property2ControlDico.Add(BuProperty.StoreType.ToString(), cbStoreType);
            property2ControlDico.Add(BuProperty.ValueString.ToString(), tbValue);
        }

        private void InitializeDGV()
        {
            VisibleColumns = new List<string>();
            var blastUnitSource = Enum.GetValues(typeof(BlastUnitSource));

            var enabled = CreateColumn(BuProperty.isEnabled.ToString(), BuProperty.isEnabled.ToString(), "Enabled",
                new DataGridViewCheckBoxColumn());
            enabled.SortMode = DataGridViewColumnSortMode.Automatic;
            dgvBlastEditor.Columns.Add(enabled);

            var locked = CreateColumn(BuProperty.isLocked.ToString(), BuProperty.isLocked.ToString(), "Locked",
                new DataGridViewCheckBoxColumn());
            locked.SortMode = DataGridViewColumnSortMode.Automatic;
            dgvBlastEditor.Columns.Add(locked);

            //Do this one separately as we need to populate the Combobox
            var source = CreateColumn(BuProperty.Source.ToString(), BuProperty.Source.ToString(), "Source", new DataGridViewComboBoxColumn()) as DataGridViewComboBoxColumn;
            foreach (var item in blastUnitSource)
            {
                source.Items.Add(item);
            }

            dgvBlastEditor.Columns.Add(source);

            //Do this one separately as we need to populate the Combobox
            var domain = CreateColumn(BuProperty.Domain.ToString(), BuProperty.Domain.ToString(), "Domain", new DataGridViewComboBoxColumn()) as DataGridViewComboBoxColumn;
            domain.DataSource = _domains;
            domain.SortMode = DataGridViewColumnSortMode.Automatic;
            dgvBlastEditor.Columns.Add(domain);

            var address = (DataGridViewNumericUpDownColumn)CreateColumn(BuProperty.Address.ToString(), BuProperty.Address.ToString(), "Address", new DataGridViewNumericUpDownColumn());
            address.Hexadecimal = true;
            address.SortMode = DataGridViewColumnSortMode.Automatic;
            address.Increment = 1;
            dgvBlastEditor.Columns.Add(address);

            var precision = (DataGridViewNumericUpDownColumn)CreateColumn(BuProperty.Precision.ToString(), BuProperty.Precision.ToString(), "Precision", new DataGridViewNumericUpDownColumn());
            precision.Minimum = 1;
            precision.Maximum = int.MaxValue;
            precision.SortMode = DataGridViewColumnSortMode.Automatic;
            dgvBlastEditor.Columns.Add(precision);

            var valuestring = CreateColumn(BuProperty.ValueString.ToString(), BuProperty.ValueString.ToString(),
                "Value", new DataGridViewTextBoxColumn());
            valuestring.DefaultCellStyle.Tag = "numeric";
            valuestring.SortMode = DataGridViewColumnSortMode.Automatic;
            ((DataGridViewTextBoxColumn)valuestring).MaxInputLength = 16348; //textbox doesn't like larger than ~20k
            dgvBlastEditor.Columns.Add(valuestring);

            var executeFrame = CreateColumn(BuProperty.ExecuteFrame.ToString(), BuProperty.ExecuteFrame.ToString(),
                "Execute Frame", new DataGridViewNumericUpDownColumn());
            executeFrame.SortMode = DataGridViewColumnSortMode.Automatic;
            ((DataGridViewNumericUpDownColumn)(executeFrame)).Maximum = int.MaxValue;
            dgvBlastEditor.Columns.Add(executeFrame);

            var loopTimng = CreateColumn(BuProperty.LoopTiming.ToString(), BuProperty.LoopTiming.ToString(),
                "Loop Timing", new DataGridViewNumericUpDownColumn());
            loopTimng.SortMode = DataGridViewColumnSortMode.Automatic;
            ((DataGridViewNumericUpDownColumn)(loopTimng)).Maximum = int.MaxValue;
            dgvBlastEditor.Columns.Add(loopTimng);

            var lifetime = CreateColumn(BuProperty.Lifetime.ToString(), BuProperty.Lifetime.ToString(),
                "Lifetime", new DataGridViewNumericUpDownColumn());
            lifetime.SortMode = DataGridViewColumnSortMode.Automatic;
            ((DataGridViewNumericUpDownColumn)(lifetime)).Maximum = int.MaxValue;
            dgvBlastEditor.Columns.Add(lifetime);

            var loop = CreateColumn(BuProperty.Loop.ToString(), BuProperty.Loop.ToString(),
                "Loop", new DataGridViewCheckBoxColumn());
            loop.SortMode = DataGridViewColumnSortMode.Automatic;
            dgvBlastEditor.Columns.Add(loop);

            var limiterTime = CreateColumn(BuProperty.LimiterTime.ToString(), BuProperty.LimiterTime.ToString(), "Limiter Time", new DataGridViewComboBoxColumn()) as DataGridViewComboBoxColumn;
            foreach (var item in Enum.GetValues(typeof(LimiterTime)))
            {
                limiterTime.Items.Add(item);
            }

            dgvBlastEditor.Columns.Add(limiterTime);

            var limiterHash = CreateColumn(BuProperty.LimiterListHash.ToString(), BuProperty.LimiterListHash.ToString(), "Limiter List", new DataGridViewComboBoxColumn()) as DataGridViewComboBoxColumn;
            limiterHash.DataSource = RtcCore.LimiterListBindingSource;
            limiterHash.DisplayMember = "Name";
            limiterHash.ValueMember = "Value";
            limiterHash.MaxDropDownItems = 15;
            dgvBlastEditor.Columns.Add(limiterHash);

            var storeLimiterSource = CreateColumn(BuProperty.StoreLimiterSource.ToString(), BuProperty.StoreLimiterSource.ToString(), "Store Limiter Source", new DataGridViewComboBoxColumn()) as DataGridViewComboBoxColumn;
            foreach (var item in Enum.GetValues(typeof(StoreLimiterSource)))
            {
                storeLimiterSource.Items.Add(item);
            }

            dgvBlastEditor.Columns.Add(storeLimiterSource);

            dgvBlastEditor.Columns.Add(CreateColumn(BuProperty.InvertLimiter.ToString(), BuProperty.InvertLimiter.ToString(), "Invert Limiter", new DataGridViewCheckBoxColumn()));

            var storeTime = CreateColumn(BuProperty.StoreTime.ToString(), BuProperty.StoreTime.ToString(), "Store Time", new DataGridViewComboBoxColumn()) as DataGridViewComboBoxColumn;
            foreach (var item in Enum.GetValues(typeof(StoreTime)))
            {
                storeTime.Items.Add(item);
            }

            storeTime.SortMode = DataGridViewColumnSortMode.Automatic;
            dgvBlastEditor.Columns.Add(storeTime);

            var storeType = CreateColumn(BuProperty.StoreType.ToString(), BuProperty.StoreType.ToString(), "Store Type", new DataGridViewComboBoxColumn()) as DataGridViewComboBoxColumn;
            storeType.DataSource = Enum.GetValues(typeof(StoreType));
            storeType.SortMode = DataGridViewColumnSortMode.Automatic;
            dgvBlastEditor.Columns.Add(storeType);

            //Do this one separately as we need to populate the Combobox
            var sourceDomain = CreateColumn(BuProperty.SourceDomain.ToString(), BuProperty.SourceDomain.ToString(), "Source Domain", new DataGridViewComboBoxColumn()) as DataGridViewComboBoxColumn;
            sourceDomain.DataSource = _domains;
            sourceDomain.SortMode = DataGridViewColumnSortMode.Automatic;
            dgvBlastEditor.Columns.Add(sourceDomain);

            var sourceAddress = (DataGridViewNumericUpDownColumn)CreateColumn(BuProperty.SourceAddress.ToString(), BuProperty.SourceAddress.ToString(), "Source Address", new DataGridViewNumericUpDownColumn());
            sourceAddress.Hexadecimal = true;
            sourceAddress.SortMode = DataGridViewColumnSortMode.Automatic;
            sourceAddress.Increment = 1;
            dgvBlastEditor.Columns.Add(sourceAddress);

            dgvBlastEditor.Columns.Add(CreateColumn("", BuProperty.Note.ToString(), "Note", new DataGridViewButtonColumn()));

            if (Params.IsParamSet("BLASTEDITOR_VISIBLECOLUMNS"))
            {
                var str = Params.ReadParam("BLASTEDITOR_VISIBLECOLUMNS");
                var columns = str.Split(',');
                foreach (var column in columns)
                {
                    VisibleColumns.Add(column);
                }
            }
            else
            {
                VisibleColumns.Add(BuProperty.isEnabled.ToString());
                VisibleColumns.Add(BuProperty.isLocked.ToString());
                VisibleColumns.Add(BuProperty.Source.ToString());
                VisibleColumns.Add(BuProperty.Domain.ToString());
                VisibleColumns.Add(BuProperty.Address.ToString());
                VisibleColumns.Add(BuProperty.Address.ToString());
                VisibleColumns.Add(BuProperty.Precision.ToString());
                VisibleColumns.Add(BuProperty.ValueString.ToString());
                VisibleColumns.Add(BuProperty.Note.ToString());
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
                {
                    cbFilterColumn.Items.Add(new ComboBoxItem<string>(column.HeaderText, column.Name));
                }
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

            cbShiftBlastlayer.Items.Add(new ComboBoxItem<string>(BuProperty.Address.ToString(), BuProperty.Address.ToString()));
            cbShiftBlastlayer.Items.Add(new ComboBoxItem<string>("Source Address", BuProperty.SourceAddress.ToString()));
            cbShiftBlastlayer.Items.Add(new ComboBoxItem<string>("Value", BuProperty.ValueString.ToString()));
            cbShiftBlastlayer.Items.Add(new ComboBoxItem<string>(BuProperty.Lifetime.ToString(), BuProperty.Lifetime.ToString()));
            cbShiftBlastlayer.Items.Add(new ComboBoxItem<string>("Execute Frame", BuProperty.ExecuteFrame.ToString()));
            cbShiftBlastlayer.SelectedIndex = 0;
        }

        public void RefreshVisibleColumns()
        {
            foreach (DataGridViewColumn column in dgvBlastEditor.Columns)
            {
                column.Visible = VisibleColumns.Contains(column.Name);
            }
            dgvBlastEditor.Refresh();
        }

        private static DataGridViewColumn CreateColumn(string dataPropertyName, string columnName, string displayName,
            DataGridViewColumn column, int fillWeight = -1)
        {
            if (fillWeight == -1)
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

        /*
        private DataGridViewColumn CreateColumnUnbound(string columnName, string displayName,
            DataGridViewColumn column, int fillWeight = -1)
        {
            return CreateColumn(String.Empty, columnName, displayName, column, fillWeight);
        }
        */

        private StashKey originalSK = null;

        private StashKey _currentSK = null;
        internal StashKey currentSK
        {
            get => _currentSK;
            set
            {
                _currentSK = value;
                SetTitle(value?.Alias ?? "Unnamed");
            }
        }

        private BindingSource bs = null;
        private BindingSource _bs = null;

        internal void LoadStashkey(StashKey sk, bool silent = false)
        {
            if (!RefreshDomains())
            {
                MessageBox.Show($"Loading domains failed! Aborting load. Check to make sure the RTC and {RtcCore.VanguardImplementationName} are connected.");
                this.Close();
                return;
            }
            var buDomains = new List<string>();
            foreach (var bu in sk.BlastLayer.Layer)
            {
                if (!buDomains.Contains(bu.Domain))
                {
                    buDomains.Add(bu.Domain);
                }

                if (bu.SourceDomain != null && !buDomains.Contains(bu.SourceDomain))
                {
                    buDomains.Add(bu.SourceDomain);
                }
            }

            foreach (var domain in buDomains)
            {
                if (DomainToMiDico.ContainsKey(domain))
                {
                    continue;
                }

                MessageBox.Show("This blastlayer references domain " + domain + " which couldn't be found!\nAre you sure you have the correct core loaded?");
                this.Hide();
                return;
            }

            originalSK = sk;
            currentSK = sk.Clone() as StashKey;

            bs = new BindingSource { DataSource = new SortableBindingList<BlastUnit>(currentSK.BlastLayer.Layer) };

            bs.CurrentChanged += (o, e) =>
            {
                if (batchOperation)
                {
                    if (e is HandledEventArgs h)
                    {
                        h.Handled = true;
                    }
                }
            };


            dgvBlastEditor.DataSource = bs;
            InitializeDGV();
            InitializeBottom();

            if (!silent)
            {
                this.Show();
                this.BringToFront();
                RefreshAllNoteIcons();
            }
        }

        private bool RefreshDomains()
        {
            try
            {
                S.GET<MemoryDomainsForm>().RefreshDomainsAndKeepSelected();
                DomainToMiDico?.Clear();
                if (MemoryDomains.MemoryInterfaces == null)
                {
                    return false;
                }

                _domains = MemoryDomains.MemoryInterfaces.Keys.Concat(MemoryDomains.VmdPool.Values.Select(it => it.ToString())).ToArray();
                foreach (var domain in _domains)
                {
                    DomainToMiDico.Add(domain, MemoryDomains.GetInterface(domain));
                }
                if (DomainToMiDico.Keys.Count > 0)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"An error occurred in RTC while refreshing the domains\nAre you sure you don't have an invalid domain selected?\nMake sure any VMDs are loaded and you have the correct core loaded in {RtcCore.VanguardImplementationName}\n{ex}"
                );
            }
        }

        private void OnBlastEditorDataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show(e.Exception.ToString() + "\nRow:" + e.RowIndex + "\nColumn" + e.ColumnIndex + "\n" + e.Context + "\n" + dgvBlastEditor[e.ColumnIndex, e.RowIndex].Value?.ToString());
        }

        public void Disable50(object sender, EventArgs e)
        {
            foreach (BlastUnit bu in currentSK.BlastLayer.Layer.
                Where(x => x.IsLocked == false))
            {
                bu.IsEnabled = true;
            }

            var unlocked = currentSK.BlastLayer.Layer.Where(x => !x.IsLocked).ToList();
            foreach (BlastUnit bu in unlocked
                .OrderBy(_ => RtcCore.RND.Next())
                .Take(unlocked.Count / 2))
            {
                bu.IsEnabled = false;
            }
            dgvBlastEditor.Refresh();
        }

        public void InvertDisabled(object sender, EventArgs e)
        {
            foreach (BlastUnit bu in currentSK.BlastLayer.Layer.
                Where(x => !x.IsLocked))
            {
                bu.IsEnabled = !bu.IsEnabled;
            }
            dgvBlastEditor.Refresh();
        }

        public void RemoveDisabled(object sender, EventArgs e)
        {
            var buToRemove = new List<BlastUnit>();

            dgvBlastEditor.SuspendLayout();
            batchOperation = true;
            var oldBS = dgvBlastEditor.DataSource;
            dgvBlastEditor.DataSource = null;
            foreach (BlastUnit bu in currentSK.BlastLayer.Layer.
                Where(x =>
                !x.IsLocked &&
                !x.IsEnabled))
            {
                buToRemove.Add(bu);
            }

            foreach (BlastUnit bu in buToRemove)
            {
                bs.Remove(bu);
                if (_bs != null && _bs.Contains(bu))
                {
                    _bs.Remove(bu);
                }
            }
            batchOperation = false;
            dgvBlastEditor.DataSource = oldBS;
            RefreshAllNoteIcons();
            dgvBlastEditor.ResumeLayout();
        }

        public void DisableEverything(object sender, EventArgs e)
        {
            foreach (BlastUnit bu in currentSK.BlastLayer.Layer.
                Where(x =>
                x.IsLocked == false))
            {
                bu.IsEnabled = false;
            }
            dgvBlastEditor.Refresh();
        }

        public void EnableEverything(object sender, EventArgs e)
        {
            foreach (BlastUnit bu in currentSK.BlastLayer.Layer.
                Where(x =>
                x.IsLocked == false))
            {
                bu.IsEnabled = true;
            }
            dgvBlastEditor.Refresh();
        }

        public void RemoveSelected(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
            {
                if ((row.DataBoundItem as BlastUnit).IsLocked == false)
                {
                    var bu = row.DataBoundItem as BlastUnit;
                    bs.Remove(bu);
                    //Todo replace how this works
                    if (_bs != null && _bs.Contains(bu))
                    {
                        _bs.Remove(bu);
                    }
                }
            }
        }

        public void DuplicateSelected(object sender, EventArgs e)
        {
            if (dgvBlastEditor.SelectedRows.Count == 0)
            {
                return;
            }

            var reversed = dgvBlastEditor.SelectedRows.Cast<DataGridViewRow>().Reverse()?.ToArray();
            foreach (DataGridViewRow row in reversed)
            {
                if ((row.DataBoundItem as BlastUnit).IsLocked == false)
                {
                    var bu = ((row.DataBoundItem as BlastUnit).Clone() as BlastUnit);
                    bs.Add(bu);
                }
            }
            RefreshAllNoteIcons();
        }

        public void SendToStash(object sender, EventArgs e)
        {
            if (currentSK.ParentKey == null)
            {
                MessageBox.Show("There's no savestate associated with this Stashkey!\nAssociate one in the menu to send this to the stash.");
                return;
            }
            var newSk = (StashKey)currentSK.Clone();

            StockpileManagerUISide.StashHistory.Add(newSk);

            S.GET<StashHistoryForm>().RefreshStashHistory();
            S.GET<StockpileManagerForm>().dgvStockpile.ClearSelection();
            S.GET<StashHistoryForm>().lbStashHistory.ClearSelected();

            S.GET<StashHistoryForm>().DontLoadSelectedStash = true;
            S.GET<StashHistoryForm>().lbStashHistory.SelectedIndex = S.GET<StashHistoryForm>().lbStashHistory.Items.Count - 1;
            StockpileManagerUISide.CurrentStashkey = StockpileManagerUISide.StashHistory[S.GET<StashHistoryForm>().lbStashHistory.SelectedIndex];
        }

        public void OpenNoteEditor(object sender, EventArgs e)
        {
            if (dgvBlastEditor.SelectedRows.Count == 0)
            {
                return;
            }

            var temp = new BlastLayer();
            var cellList = new List<DataGridViewCell>();
            foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
            {
                if (row.DataBoundItem is BlastUnit bu)
                {
                    temp.Layer.Add(bu);
                    cellList.Add(row.Cells[BuProperty.Note.ToString()]);
                }
            }

            S.SET(new NoteEditorForm(temp, cellList));
            S.GET<NoteEditorForm>().Show();
        }

        public void SanitizeDuplicates(object sender, EventArgs e)
        {
            dgvBlastEditor.ClearSelection();

            dgvBlastEditor.DataSource = null;
            batchOperation = true;
            currentSK.BlastLayer.SanitizeDuplicates();
            bs = new BindingSource { DataSource = new SortableBindingList<BlastUnit>(currentSK.BlastLayer.Layer) };
            batchOperation = false;
            dgvBlastEditor.DataSource = bs;
            dgvBlastEditor.Refresh();
            UpdateBottom();
        }

        public void RasterizeVMDs()
        {
            dgvBlastEditor.ClearSelection();

            dgvBlastEditor.DataSource = null;
            batchOperation = true;
            currentSK.BlastLayer.RasterizeVMDs();
            bs = new BindingSource { DataSource = new SortableBindingList<BlastUnit>(currentSK.BlastLayer.Layer) };

            batchOperation = false;
            dgvBlastEditor.DataSource = bs;
            updateMaximum(this, dgvBlastEditor.Rows.Cast<DataGridViewRow>().ToList());
            dgvBlastEditor.Refresh();
            UpdateBottom();
        }



        public void ReplaceRomFromGlitchHarvester(object sender, EventArgs e)
        {
            StashKey temp = StockpileManagerUISide.CurrentSavestateStashKey;

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

        public void ReplaceRomFromFile(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Loading this rom will invalidate the associated savestate. You'll need to set a new savestate for the Blastlayer. Continue?", "Invalidate State?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                var openRomDialog = new OpenFileDialog
                {
                    Title = "Open ROM File",
                    Filter = "any file|*.*",
                    RestoreDirectory = true
                };

                if (openRomDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                var filename = openRomDialog.FileName;

                LocalNetCoreRouter.Route(NetCore.Endpoints.Vanguard, NetCore.Commands.Remote.LoadROM, filename, true);

                var temp = new StashKey(RtcCore.GetRandomKey(), currentSK.ParentKey, currentSK.BlastLayer);

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
        public void BakeROMBlastunitsToFile(string filename = null)
        {
            if (filename == null)
            {
                var originalFilename = currentSK.RomFilename.Split('\\');
                var saveRomDialog = new SaveFileDialog
                {
                    //DefaultExt = "rom";
                    FileName = originalFilename[originalFilename.Length - 1],
                    Title = "Save Rom File",
                    Filter = "rom files|*.*",
                    RestoreDirectory = true
                };

                if (saveRomDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                filename = saveRomDialog.FileName;
            }

            RomParts rp = MemoryDomains.GetRomParts(currentSK.SystemName, currentSK.RomFilename);

            File.Copy(currentSK.RomFilename, filename, true);
            using (var output = new FileStream(filename, FileMode.Open))
            {
                foreach (BlastUnit bu in currentSK.BlastLayer.Layer)
                {
                    if (bu.Source == BlastUnitSource.VALUE)
                    {
                        //We don't want to modify the original
                        var outvalue = (byte[])bu.Value.Clone();
                        outvalue.AddValueToByteArrayUnchecked(bu.TiltValue, bu.BigEndian);
                        //Flip it if it's big endian
                        if (bu.BigEndian)
                        {
                            outvalue.FlipBytes();
                        }

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

        private void BakeROMBlastunitsToFile(object sender, EventArgs e)
        {
            BakeROMBlastunitsToFile(null);
        }

        private void RunOriginalSavestate(object sender, EventArgs e)
        {
            originalSK.RunOriginal();
        }

        public void ReplaceSavestateFromGlitchHarvester(object sender, EventArgs e)
        {
            StashKey temp = StockpileManagerUISide.CurrentSavestateStashKey;
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

            //also replace these for the original stashkey
            originalSK.ParentKey = temp.ParentKey;
            originalSK.SyncSettings = temp.SyncSettings;
            originalSK.StateLocation = temp.StateLocation;
        }

        public void ReplaceSavestateFromFileToolStrip(string filename = null)
        {
            if (filename == null)
            {
                var openSavestateDialog = new OpenFileDialog
                {
                    DefaultExt = "state",
                    Title = "Open Savestate File",
                    Filter = "state files|*.state",
                    RestoreDirectory = true
                };
                if (openSavestateDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                filename = openSavestateDialog.FileName;
            }

            var oldKey = currentSK.ParentKey;
            var oldSS = currentSK.SyncSettings;

            //Get a new key
            currentSK.ParentKey = RtcCore.GetRandomKey();
            //Null the syncsettings out
            currentSK.SyncSettings = null;

            //Let's hope the game name is correct!
            File.Copy(filename, currentSK.GetSavestateFullPath(), true);

            //Attempt to load and if it fails, don't let them update it.
            if (!StockpileManagerUISide.LoadState(currentSK))
            {
                currentSK.ParentKey = oldKey;
                currentSK.SyncSettings = oldSS;
                return;
            }

            //Grab the syncsettings
            var temp = new StashKey(RtcCore.GetRandomKey(), currentSK.ParentKey, currentSK.BlastLayer);
            currentSK.SyncSettings = temp.SyncSettings;
        }
        private void ReplaceSavestateFromFile(object sender, EventArgs e)
        {
            ReplaceSavestateFromFileToolStrip(null);
        }

        private void SaveSavestateTo(string filename = null)
        {
            if (filename == null)
            {
                var saveSavestateDialog = new SaveFileDialog
                {
                    DefaultExt = "state",
                    Title = "Save Savestate File",
                    Filter = "state files|*.state",
                    RestoreDirectory = true
                };
                if (saveSavestateDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                filename = saveSavestateDialog.FileName;
            }

            File.Copy(currentSK.GetSavestateFullPath(), filename, true);
        }

        private void SaveSavestateTo(object sender, EventArgs e)
        {
            SaveSavestateTo(null);
        }

        public void SaveBlastLayerToFile(object sender, EventArgs e)
        {
            //If there's no blastlayer file already set, don't quicksave
            if (CurrentBlastLayerFile.Length == 0)
            {
                BlastTools.SaveBlastLayerToFile(currentSK.BlastLayer);
            }
            else
            {
                BlastTools.SaveBlastLayerToFile(currentSK.BlastLayer, CurrentBlastLayerFile);
            }

            CurrentBlastLayerFile = BlastTools.LastBlastLayerSavePath;
        }

        public void SaveAsBlastLayerToFile(object sender, EventArgs e)
        {
            BlastTools.SaveBlastLayerToFile(currentSK.BlastLayer);
            CurrentBlastLayerFile = BlastTools.LastBlastLayerSavePath;
        }

        public void ImportBlastLayer(object sender, EventArgs e)
        {
            BlastLayer temp = BlastTools.LoadBlastLayerFromFile();
            ImportBlastLayer(temp);
        }

        public void LoadBlastLayerFromFile(object sender, EventArgs e)
        {
            BlastLayer temp = BlastTools.LoadBlastLayerFromFile();
            if (temp != null)
            {
                LoadBlastlayer(temp);
            }
        }

        public void LoadBlastlayer(BlastLayer bl, bool import = false)
        {
            if (bl == null)
            {
                logger.Trace("LoadBlastLayer had an empty bl");
                return;
            }
            List<BlastUnit> checkUnits()
            {
                var warned = new List<string>();
                var unitsToLoad = new List<BlastUnit>();
                foreach (BlastUnit bu in bl.Layer)
                {
                    if (_domains.Contains(bu.Domain) &&
                        (string.IsNullOrWhiteSpace(bu.SourceDomain) || _domains.Contains(bu.SourceDomain)))
                    {
                        unitsToLoad.Add(bu);
                    }
                    else
                    {
                        //If we've already warned them about the specific domain, don't warn them again.
                        if (warned.Contains(bu.Domain) &&
                            (string.IsNullOrEmpty(bu.SourceDomain) || warned.Contains(bu.SourceDomain)))
                        {
                            continue;
                        }

                        if (MessageBox.Show($"Imported blastlayer references an invalid domain.\n" +
                                            $"The current unit being imported has the following parameters.\n" +
                                            $"Domain: {bu.Domain}" +
                                            $"SourceDomain {bu.SourceDomain ?? "EMPTY"}\n\n" +
                                            $"Silence warning & continue importing valid units?",
                                "Invalid Domain in Imported Unit",
                                MessageBoxButtons.OKCancel) == DialogResult.OK)
                        {
                            warned.Add(bu.Domain);
                            if (!string.IsNullOrWhiteSpace(bu.SourceDomain))
                            {
                                warned.Add(bu.SourceDomain);
                            }
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                return unitsToLoad;
            }

            var l = checkUnits();
            if (l == null)
            {
                return;
            }

            if (import)
            {
                foreach (var bu in l)
                {
                    bs.Add(bu);
                }
            }
            else
            {
                currentSK.BlastLayer = new BlastLayer(l);
                bs = new BindingSource { DataSource = new SortableBindingList<BlastUnit>(currentSK.BlastLayer.Layer) };
                dgvBlastEditor.DataSource = bs;
            }
            dgvBlastEditor.ResetBindings();
            RefreshAllNoteIcons();
            dgvBlastEditor.Refresh();
        }

        public void ImportBlastLayer(BlastLayer bl)
        {
            LoadBlastlayer(bl, true);
        }

        private static string GenerateCSV(DataGridView dgv)
        {
            var sb = new StringBuilder();
            var headers = dgv.Columns.Cast<DataGridViewColumn>();

            sb.AppendLine(string.Join(CultureInfo.CurrentCulture.TextInfo.ListSeparator, headers.Select(column => "\"" + column.HeaderText + "\"").ToArray()));
            foreach (DataGridViewRow row in dgv.Rows)
            {
                var cells = row.Cells.Cast<DataGridViewCell>();
                sb.AppendLine(string.Join(CultureInfo.CurrentCulture.TextInfo.ListSeparator, cells.Select(cell => "\"" + cell.Value + "\"").ToArray()));
            }
            return sb.ToString();
        }

        public void ExportToCSV(string filename = null)
        {
            if (currentSK.BlastLayer.Layer.Count == 0)
            {
                MessageBox.Show("Can't save because the provided blastlayer is empty.");
                return;
            }

            if (filename == null)
            {
                var saveCsvDialog = new SaveFileDialog
                {
                    DefaultExt = "csv",
                    Title = "Export to csv",
                    Filter = "csv files|*.csv",
                    RestoreDirectory = true
                };

                if (saveCsvDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                filename = saveCsvDialog.FileName;
            }

            File.WriteAllText(filename, GenerateCSV(dgvBlastEditor), Encoding.UTF8);
        }


        public void BakeBlastUnitsToValue(bool bakeSelected = false)
        {
            try
            {
                //Generate a blastlayer from the current selected rows
                var bl = new BlastLayer();

                IEnumerable<DataGridViewRow> targetRows;

                targetRows = bakeSelected ?
                    dgvBlastEditor.SelectedRows.Cast<DataGridViewRow>() :
                    dgvBlastEditor.Rows.Cast<DataGridViewRow>();

                batchOperation = true;

                foreach (var selected in targetRows
                    .Where((item => ((BlastUnit)item.DataBoundItem).IsLocked == false)))
                {
                    var bu = (BlastUnit)selected.DataBoundItem;

                    //They have to be enabled to get a backup
                    bu.IsEnabled = true;
                    bl.Layer.Add(bu);
                }

                //Bake them
                BlastLayer newBlastLayer = LocalNetCoreRouter.QueryRoute<BlastLayer>(NetCore.Endpoints.CorruptCore, NetCore.Commands.Remote.BlastToolsGetAppliedBackupLayer, new object[] { bl, currentSK }, true);

                var i = 0;
                //Insert the new one where the old row was, then remove the old row.
                foreach (var selected in targetRows
                    .Where((item => ((BlastUnit)item.DataBoundItem).IsLocked == false)))
                {
                    bs.Insert(selected.Index, newBlastLayer.Layer[i]);
                    i++;
                    bs.Remove((BlastUnit)selected.DataBoundItem);
                }

                batchOperation = false;
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong in when baking to VALUE.\n" +
                                           "Your blast editor session may be broke depending on when it failed.\n" +
                                           "You should probably send a copy of this error and what you did to cause it to the RTC devs.\n\n" +
                                           ex.ToString());
            }
            finally
            {
            }
        }

        public void LoadCorrupt(object sender, EventArgs e)
        {
            if (currentSK.ParentKey == null)
            {
                MessageBox.Show("There's no savestate associated with this Stashkey!\nAssociate one in the menu to be able to load.");
                return;
            }

            var newSk = (StashKey)currentSK.Clone();
            S.GET<GlitchHarvesterBlastForm>().IsCorruptionApplied = newSk.Run();
        }

        public void LoadOriginal()
        {
            if (currentSK.ParentKey == null)
            {
                MessageBox.Show("There's no savestate associated with this Stashkey!\nAssociate one in the menu to be able to load.");
                return;
            }

            var newSk = (StashKey)currentSK.Clone();
            newSk.BlastLayer.Layer.Clear();
            newSk.Run();
        }

        public void Corrupt(object sender, EventArgs e)
        {
            var newSk = (StashKey)currentSK.Clone();
            S.GET<GlitchHarvesterBlastForm>().IsCorruptionApplied = StockpileManagerUISide.ApplyStashkey(newSk, false);
        }

        private static void RefreshNoteIcons(DataGridViewRowCollection rows)
        {
            foreach (DataGridViewRow row in rows)
            {
                DataGridViewCell buttonCell = row.Cells[BuProperty.Note.ToString()];
                buttonCell.Value = string.IsNullOrWhiteSpace((row.DataBoundItem as BlastUnit)?.Note) ? string.Empty : "📝";
            }
        }

        public void RefreshAllNoteIcons()
        {
            RefreshNoteIcons(dgvBlastEditor.Rows);
        }



        public void ShiftBlastLayerDown(object sender, EventArgs e)
        {
            var amount = updownShiftBlastLayerAmount.Value;
            var column = ((ComboBoxItem<string>)cbShiftBlastlayer?.SelectedItem)?.Value;

            if (column == null)
            {
                return;
            }

            var rows = dgvBlastEditor.SelectedRows.Cast<DataGridViewRow>()
                .Where((item => ((BlastUnit)item.DataBoundItem).IsLocked == false))
                .ToList();
            ShiftBlastLayer(amount, column, rows, true);
        }

        public void ShiftBlastLayerUp(object sender, EventArgs e)
        {
            var amount = updownShiftBlastLayerAmount.Value;
            var column = ((ComboBoxItem<string>)cbShiftBlastlayer?.SelectedItem)?.Value;

            if (column == null)
            {
                return;
            }

            var rows = dgvBlastEditor.SelectedRows.Cast<DataGridViewRow>()
                .Where((item => ((BlastUnit)item.DataBoundItem).IsLocked == false))
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
                        {
                            u.Value = Convert.ToInt64(u.Value) - amount;
                        }
                        else
                        {
                            u.Value = 0;
                        }
                    }
                    else
                    {
                        if ((Convert.ToInt64(u.Value) + amount) <= u.Maximum)
                        {
                            u.Value = Convert.ToInt64(u.Value) + amount;
                        }
                        else
                        {
                            u.Value = u.Maximum;
                        }
                    }
                }
                else if (cell.OwningColumn.Name == BuProperty.ValueString.ToString())
                {
                    var _amount = shiftDown ? 0 - amount : amount;
                    var precision = (int)row.Cells[BuProperty.Precision.ToString()].Value;
                    cell.Value = getShiftedHexString((string)cell.Value, _amount, precision);
                }
                else
                {
                    throw new NotImplementedException("Invalid column type.");
                }
            }
            dgvBlastEditor.Refresh();
            UpdateBottom();
        }

        private static string getShiftedHexString(string value, decimal amount, int precision)
        {
            //Convert the string we have into a byte array
            var valueBytes = value.ToByteArrayPadLeft(precision);
            if (valueBytes == null)
            {
                return value;
            }

            valueBytes.AddValueToByteArrayUnchecked(new BigInteger(amount), true);
            return BitConverter.ToString(valueBytes).Replace("-", string.Empty);
        }

        private void ShowHelp(object sender, EventArgs e)
        {
            var startInfo = new System.Diagnostics.ProcessStartInfo("https://corrupt.wiki/corruptors/rtc-real-time-corruptor/blast-editor.html");
            System.Diagnostics.Process.Start(startInfo);
        }

        private void OpenBlastGenerator(object sender, EventArgs e)
        {
            if (S.GET<BlastGeneratorForm>() != null)
            {
                S.GET<BlastGeneratorForm>().Close();
            }

            S.SET(new BlastGeneratorForm());

            var bgForm = S.GET<BlastGeneratorForm>();
            bgForm.LoadStashkey(currentSK);
        }

        private void AddRow(object sender, EventArgs e)
        {
            var bu = new BlastUnit(new byte[] { 0 }, _domains[0], 0, 1, MemoryDomains.GetInterface(_domains[0]).BigEndian);
            bs.Add(bu);
        }

        private void UpdateLayerSize()
        {
            lbBlastLayerSize.Text = "Size: " + currentSK.BlastLayer.Layer.Count;
        }

        public void OpenSanitizeTool(object sender, EventArgs e)
        {
            OpenSanitizeTool(false);
        }

        public void OpenSanitizeTool(bool lockUI = true)
        {
            if (currentSK?.BlastLayer?.Layer == null)
            {
                return;
            }
            SanitizeToolForm.OpenSanitizeTool(currentSK, lockUI);
        }

        public StashKey[] GetStashKeys()
        {
            return new[] { currentSK, originalSK };
        }

        public void ImportBlastlayerFromCorruptedFile(string filename = null)
        {
            if (filename == null)
            {
                var openFileDialog = new OpenFileDialog
                {
                    DefaultExt = "*",
                    Title = "Open Corrupted File",
                    Filter = "Any file|*.*",
                    RestoreDirectory = true
                };
                if (openFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                filename = openFileDialog.FileName;
            }

            var bl = LocalNetCoreRouter.QueryRoute<BlastLayer>(NetCore.Endpoints.CorruptCore, NetCore.Commands.Remote.BLGetDiffBlastLayer, filename);

            ImportBlastLayer(bl);
        }

        private void importBlastlayerFromCorruptedFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportBlastlayerFromCorruptedFile(null);
        }

        private void showBlastlayerNameInTitleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dontShowBlastlayerNameInTitleToolStripMenuItem.Checked)
            {
                Params.SetParam("DONT_SHOW_BLASTLAYER_NAME_IN_EDITOR");
            }
            else
            {
                Params.RemoveParam("DONT_SHOW_BLASTLAYER_NAME_IN_EDITOR");
            }

            SetTitle(currentSK?.Alias ?? "Unsaved");
        }

        private void SetTitle(string name)
        {
            if (dontShowBlastlayerNameInTitleToolStripMenuItem.Checked)
            {
                this.Text = "Blast Editor";
            }
            else
            {
                this.Text = "Blast Editor - " + name;
            }
        }

        private void NewBlastLayer(object sender, EventArgs e)
        {
            bs.Clear();
            dgvBlastEditor.ResetBindings();
            RefreshAllNoteIcons();
            dgvBlastEditor.Refresh();
        }

        public bool AddStashToStockpile()
        {
            SendToStash(null, null);
            return S.GET<StashHistoryForm>().AddStashToStockpileFromUI();
        }

        private void AddStashToStockpile(object sender, EventArgs e) => AddStashToStockpile();
        private void BreakDownAllBlastUnits(object sender, EventArgs e) => BreakDownUnits();
        private void OnBlastEditorRowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e) => UpdateLayerSize();
        private void OnBlastEditorRowsAdded(object sender, DataGridViewRowsAddedEventArgs e) => UpdateLayerSize();
        private void ExportBlastLayerToCSV(object sender, EventArgs e) => ExportToCSV(null);
        private void BakeBlastUnitsToValue(object sender, EventArgs e) => BakeBlastUnitsToValue();
        private void RunRomWithoutBlastLayer(object sender, EventArgs e) => currentSK.RunOriginal();
        private void RasterizeVMDs(object sender, EventArgs e) => RasterizeVMDs();
    }
}
