namespace RTCV.UI
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using Newtonsoft.Json;
    using RTCV.CorruptCore;
    using RTCV.NetCore;
    using RTCV.Common;
    using RTCV.UI.Components;

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

    #pragma warning disable CA2213 //Component designer classes generate their own Dispose method
    public partial class BlastGeneratorForm : Form, IColorize
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

        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private bool OpenedFromBlastEditor = false;
        private StashKey _sk = null;
        private ContextMenuStrip cms = new ContextMenuStrip();
        private bool initialized = false;
        private List<Control> allControls;

        private static Dictionary<string, MemoryInterface> domainToMiDico = new Dictionary<string, MemoryInterface>();
        private string[] domains = MemoryDomains.MemoryInterfaces?.Keys?.Concat(MemoryDomains.VmdPool.Values.Select(it => it.ToString())).ToArray();

        public BlastGeneratorForm()
        {
            InitializeComponent();

            //For some godforsaken reason, xmlSerializer deserialization wont fill this in as a bool so just use a string god help us all
            (dgvBlastGenerator.Columns["dgvEnabled"]).ValueType = typeof(string);

            Colors.SetRTCColor(Colors.GeneralColor, this);
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            dgvBlastGenerator.MouseClick += OnBlastGeneratorDGVMouseClick;
            dgvBlastGenerator.CellValueChanged += UpdateSelectedBlastGenerator;
            dgvBlastGenerator.CellMouseClick += OnCellMouseClick;
            dgvBlastGenerator.CellMouseDoubleClick += OnCellMouseDoubleClick;

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
            if (!RefreshDomains())
            {
                return;
            }

            AddDefaultRow();
            PopulateModeCombobox(dgvBlastGenerator.Rows[0]);
            OpenedFromBlastEditor = false;
            btnSendTo.Text = "Send to Stash";
            initialized = true;

            this.Show();
            this.BringToFront();
        }

        public void LoadStashkey(StashKey sk)
        {
            if (sk == null)
            {
                return;
            }

            if (!RefreshDomains())
            {
                return;
            }

            this._sk = (StashKey)sk.Clone();
            this._sk.BlastLayer = new BlastLayer();

            AddDefaultRow();
            PopulateModeCombobox(dgvBlastGenerator.Rows[0]);
            OpenedFromBlastEditor = true;
            btnSendTo.Text = "Send to Blast Editor";
            initialized = true;

            this.Show();
            this.BringToFront();
        }

        private void AddDefaultRow(object sender = null, EventArgs e = null)
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
                dgvBlastGenerator.Rows[lastrow].Cells["dgvStartAddress"].ValueType = typeof(decimal);
                dgvBlastGenerator.Rows[lastrow].Cells["dgvEndAddress"].ValueType = typeof(decimal);
                dgvBlastGenerator.Rows[lastrow].Cells["dgvParam1"].ValueType = typeof(decimal);
                dgvBlastGenerator.Rows[lastrow].Cells["dgvParam2"].ValueType = typeof(decimal);
                dgvBlastGenerator.Rows[lastrow].Cells["dgvLifetime"].ValueType = typeof(decimal);
                dgvBlastGenerator.Rows[lastrow].Cells["dgvExecuteFrame"].ValueType = typeof(decimal);
                dgvBlastGenerator.Rows[lastrow].Cells["dgvSeed"].ValueType = typeof(decimal);

                //These can't be null or else things go bad when trying to save and load them from a file. Include an M as they NEED to be decimal.
                ((DataGridViewNumericUpDownCell)dgvBlastGenerator.Rows[lastrow].Cells["dgvStartAddress"]).Value = 0M;
                ((DataGridViewNumericUpDownCell)dgvBlastGenerator.Rows[lastrow].Cells["dgvEndAddress"]).Value = 1M;
                ((DataGridViewNumericUpDownCell)dgvBlastGenerator.Rows[lastrow].Cells["dgvParam1"]).Value = 0M;
                ((DataGridViewNumericUpDownCell)dgvBlastGenerator.Rows[lastrow].Cells["dgvParam1"]).Maximum = ulong.MaxValue;
                ((DataGridViewNumericUpDownCell)dgvBlastGenerator.Rows[lastrow].Cells["dgvParam2"]).Value = 0M;
                ((DataGridViewNumericUpDownCell)dgvBlastGenerator.Rows[lastrow].Cells["dgvParam2"]).Maximum = ulong.MaxValue;
                ((DataGridViewNumericUpDownCell)dgvBlastGenerator.Rows[lastrow].Cells["dgvEndAddress"]).Value = 1M;
                ((DataGridViewNumericUpDownCell)dgvBlastGenerator.Rows[lastrow].Cells["dgvLifetime"]).Value = 1M;
                ((DataGridViewNumericUpDownCell)dgvBlastGenerator.Rows[lastrow].Cells["dgvExecuteFrame"]).Value = 0M;

                //Generate a random Seed
                ((DataGridViewTextBoxCell)dgvBlastGenerator.Rows[lastrow].Cells["dgvSeed"]).Value = RtcCore.RND.Next(int.MinValue, int.MaxValue);

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
                if (row.Cells["dgvDomain"] is DataGridViewComboBoxCell cell)
                {
                    object currentValue = cell.Value;

                    cell.Value = null;
                    cell.Items.Clear();

                    foreach (string domain in domains)
                    {
                        cell.Items.Add(domain);
                    }

                    if (currentValue != null && cell.Items.Contains(currentValue))
                    {
                        cell.Value = currentValue;
                    }
                    else if (cell.Items.Count > 0)
                    {
                        cell.Value = cell.Items[0];
                    }
                    else
                    {
                        cell.Value = null;
                    }
                }

                UpdateAddressRange(row);

                return true;
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                throw;
            }
        }

        private static void UpdateAddressRange(DataGridViewRow row)
        {
            if (row.Cells["dgvDomain"].Value == null)
            {
                return;
            }

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
            if (row.Cells["dgvMode"] is DataGridViewComboBoxCell cell)
            {
                cell.Value = null;
                cell.Items.Clear();

                switch (row.Cells["dgvType"].Value.ToString())
                {
                    case "Value":
                        foreach (BGValueMode type in Enum.GetValues(typeof(BGValueMode)))
                        {
                            cell.Items.Add(type.ToString());
                        }
                        break;
                    case "Store":
                        foreach (BGStoreMode type in Enum.GetValues(typeof(BGStoreMode)))
                        {
                            cell.Items.Add(type.ToString());
                        }
                        break;
                    default:
                        break;
                }
                cell.Value = cell.Items[0];
                cell.ToolTipText = "The generation mode. See the ? button on the sidebar for more info.";
            }
        }

        private void ApplyCorruption(object sender, EventArgs e)
        {
            try
            {
                btnLoadCorrupt.Enabled = false;
                btnSendTo.Enabled = false;
                btnJustCorrupt.Enabled = false;

                GenerateBlastLayers(false, true);
                //LocalNetCoreRouter.Route(NetCore.Endpoints.CorruptCore, NetCore.Commands.Basic.APPLYBLASTLAYER, new object[] { bl?.Clone() as BlastLayer, true }, true);
            }
            finally
            {
                btnLoadCorrupt.Enabled = true;
                btnSendTo.Enabled = true;
                btnJustCorrupt.Enabled = true;
            }
        }

        private void LoadAndCorrupt(object sender, EventArgs e)
        {
            string saveStateWord = "Savestate";

            object renameSaveStateWord = AllSpec.VanguardSpec[VSPEC.RENAME_SAVESTATE];
            if (renameSaveStateWord != null && renameSaveStateWord is string s)
            {
                saveStateWord = s;
            }

            try
            {
                btnLoadCorrupt.Enabled = false;
                btnSendTo.Enabled = false;
                btnJustCorrupt.Enabled = false;

                StashKey newSk = null;
                if (_sk == null)
                {
                    StashKey psk = StockpileManagerUISide.CurrentSavestateStashKey;
                    if (psk == null)
                    {
                        MessageBox.Show(
                            $"Could not perform the CORRUPT action\n\nEither no {saveStateWord} Box was selected in the {saveStateWord} Manager\nor the {saveStateWord} Box itself is empty.");
                        return;
                    }

                    newSk = new StashKey(RtcCore.GetRandomKey(), psk.ParentKey, null)
                    {
                        RomFilename = psk.RomFilename,
                        SystemName = psk.SystemName,
                        SystemCore = psk.SystemCore,
                        GameName = psk.GameName,
                        SyncSettings = psk.SyncSettings,
                        StateLocation = psk.StateLocation
                    };
                }
                else
                {
                    newSk = (StashKey)_sk.Clone();
                }

                BlastLayer bl = GenerateBlastLayers(true, true);
                if (bl == null)
                {
                    return;
                }

                newSk.BlastLayer = bl;
            }
            finally
            {
                btnLoadCorrupt.Enabled = true;
                btnSendTo.Enabled = true;
                btnJustCorrupt.Enabled = true;
            }
        }

        private void SendtoStash(object sender, EventArgs e)
        {
            string saveStateWord = "Savestate";

            object renameSaveStateWord = AllSpec.VanguardSpec[VSPEC.RENAME_SAVESTATE];
            if (renameSaveStateWord != null && renameSaveStateWord is string s)
            {
                saveStateWord = s;
            }

            btnLoadCorrupt.Enabled = false;
            btnSendTo.Enabled = false;
            btnJustCorrupt.Enabled = false;
            try
            {
                StashKey newSk = null;
                if (_sk == null)
                {
                    StashKey psk = StockpileManagerUISide.CurrentSavestateStashKey;
                    if (psk == null)
                    {
                        MessageBox.Show($"Could not perform the CORRUPT action\n\nEither no {saveStateWord} Box was selected in the {saveStateWord} Manager\nor the {saveStateWord} Box itself is empty.");
                        return;
                    }
                    newSk = new StashKey(RtcCore.GetRandomKey(), psk.ParentKey, null)
                    {
                        RomFilename = psk.RomFilename,
                        SystemName = psk.SystemName,
                        SystemCore = psk.SystemCore,
                        GameName = psk.GameName,
                        SyncSettings = psk.SyncSettings,
                        StateLocation = psk.StateLocation
                    };
                }
                else
                {
                    newSk = (StashKey)_sk.Clone();
                }

                BlastLayer bl = GenerateBlastLayers(true);
                if (bl == null)
                {
                    return;
                }

                newSk.BlastLayer = bl;
                if (OpenedFromBlastEditor)
                {
                    if (S.GET<BlastEditorForm>() == null || S.GET<BlastEditorForm>().IsDisposed)
                    {
                        S.SET(new BlastEditorForm());
                        S.GET<BlastEditorForm>().LoadStashkey((StashKey)newSk.Clone());
                    }
                    else
                    {
                        S.GET<BlastEditorForm>().ImportBlastLayer(newSk.BlastLayer);
                    }

                    {
                    }
                    S.GET<BlastEditorForm>().Show();
                }
                else
                {
                    StockpileManagerUISide.StashHistory.Add(newSk);
                    S.GET<StashHistoryForm>().RefreshStashHistory();
                    S.GET<StockpileManagerForm>().dgvStockpile.ClearSelection();
                    S.GET<StashHistoryForm>().lbStashHistory.ClearSelected();

                    S.GET<StashHistoryForm>().DontLoadSelectedStash = true;
                    S.GET<StashHistoryForm>().lbStashHistory.SelectedIndex = S.GET<StashHistoryForm>().lbStashHistory.Items.Count - 1;
                    StockpileManagerUISide.CurrentStashkey = StockpileManagerUISide.StashHistory[S.GET<StashHistoryForm>().lbStashHistory.SelectedIndex];
                }
            }
            finally
            {
                btnLoadCorrupt.Enabled = true;
                btnSendTo.Enabled = true;
                btnJustCorrupt.Enabled = true;
            }
        }

        private void UpdateSelectedBlastGenerator(object sender, DataGridViewCellEventArgs e)
        {
            if (!initialized || dgvBlastGenerator == null)
            {
                return;
            }

            if (e.ColumnIndex != (int)BlastGeneratorColumn.DgvRowDirty)
            {
                dgvBlastGenerator.Rows[e.RowIndex].Cells["dgvRowDirty"].Value = true;
            }

            if ((BlastGeneratorColumn)e.ColumnIndex == BlastGeneratorColumn.DgvType)
            {
                PopulateModeCombobox(dgvBlastGenerator.Rows[e.RowIndex]);
            }
            if ((BlastGeneratorColumn)e.ColumnIndex == BlastGeneratorColumn.DgvDomain)
            {
                UpdateAddressRange(dgvBlastGenerator.Rows[e.RowIndex]);
            }
        }

        private void OnBlastGeneratorDGVMouseClick(object sender, MouseEventArgs e)
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

        public BlastLayer GenerateBlastLayers(bool loadBeforeCorrupt = false, bool applyAfterCorrupt = false, bool resumeAfter = true)
        {
            string saveStateWord = "Savestate";

            object renameSaveStateWord = AllSpec.VanguardSpec[VSPEC.RENAME_SAVESTATE];
            if (renameSaveStateWord != null && renameSaveStateWord is string s)
            {
                saveStateWord = s;
            }

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
                        StashKey psk = StockpileManagerUISide.CurrentSavestateStashKey;
                        if (psk == null)
                        {
                            MessageBox.Show(
                                $"The Blast Generator could not perform the CORRUPT action\n\nEither no {saveStateWord} Box was selected in the {saveStateWord} Manager\nor the {saveStateWord} Box itself is empty.");
                            return null;
                        }
                        newSk = new StashKey(RtcCore.GetRandomKey(), psk.ParentKey, bl)
                        {
                            RomFilename = psk.RomFilename,
                            SystemName = psk.SystemName,
                            SystemCore = psk.SystemCore,
                            GameName = psk.GameName,
                            StateLocation = psk.StateLocation,
                            SyncSettings = psk.SyncSettings
                        };
                    }
                    else
                    {
                        newSk = (StashKey)_sk.Clone();
                    }
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
                            {
                                row.Cells["dgvBlastProtoReference"].Value = proto;
                            }
                        }
                        else
                        {
                            proto = (BlastGeneratorProto)row.Cells["dgvBlastProtoReference"].Value;
                        }
                    }
                    protoList.Add(proto);
                }

                List<BlastGeneratorProto> returnList = new List<BlastGeneratorProto>();

                returnList = LocalNetCoreRouter.QueryRoute<List<BlastGeneratorProto>>(NetCore.Endpoints.CorruptCore, NetCore.Commands.Basic.BlastGeneratorBlast, new object[] { newSk, protoList, loadBeforeCorrupt, applyAfterCorrupt, resumeAfter }, true);

                if (returnList == null)
                {
                    return null;
                }

                if (returnList.Count != protoList.Count)
                {
                    throw (new Exception("Got less protos back compared to protos sent. Aborting!"));
                }

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
                S.GET<GlitchHarvesterBlastForm>().IsCorruptionApplied = (applyAfterCorrupt && returnList.Count > 0);

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
                    {
                        note = value.ToString();
                    }
                    else
                    {
                        note = string.Empty;
                    }
                }
                else
                {
                    note = string.Empty;
                }

                string domain = row.Cells["dgvDomain"].Value.ToString();
                string type = row.Cells["dgvType"].Value.ToString();
                string mode = row.Cells["dgvMode"].Value.ToString();
                int precision = GetPrecisionSizeFromName(row.Cells["dgvPrecision"].Value.ToString());
                int interval = Convert.ToInt32(row.Cells["dgvInterval"].Value);
                long startAddress = Convert.ToInt64(row.Cells["dgvStartAddress"].Value);
                long endAddress = Convert.ToInt64(row.Cells["dgvEndAddress"].Value);
                ulong param1 = Convert.ToUInt64(row.Cells["dgvParam1"].Value);
                ulong param2 = Convert.ToUInt64(row.Cells["dgvParam2"].Value);
                int lifetime = Convert.ToInt32(row.Cells["dgvLifetime"].Value);
                int executeframe = Convert.ToInt32(row.Cells["dgvExecuteFrame"].Value);
                bool loop = Convert.ToBoolean(row.Cells["dgvLoop"].Value);
                int seed = Convert.ToInt32(row.Cells["dgvSeed"].Value);

                return new BlastGeneratorProto(note, type, domain, mode, precision, interval, startAddress, endAddress, param1, param2, lifetime, executeframe, loop, seed);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                throw;
            }
        }

        private static string GetPrecisionNameFromSize(int precision)
        {
            switch (precision)
            {
                case 1:
                    return "8-bit";

                case 2:
                    return "16-bit";

                case 4:
                    return "32-bit";

                case 8:
                    return "64-bit";

                default:
                    return null;
            }
        }

        private static int GetPrecisionSizeFromName(string precision)
        {
            switch (precision)
            {
                case "8-bit":
                    return 1;

                case "16-bit":
                    return 2;

                case "32-bit":
                    return 4;

                case "64-bit":
                    return 8;

                default:
                    return -1;
            }
        }

        private void NudgeStartAddressUp(object sender, EventArgs e)
        {
            NudgeParams("dgvStartAddress", updownNudgeStartAddress.Value);
        }

        private void NudgeStartAddressDown(object sender, EventArgs e)
        {
            NudgeParams("dgvStartAddress", updownNudgeStartAddress.Value, true);
        }

        private void NudgeEndAddressUp(object sender, EventArgs e)
        {
            NudgeParams("dgvEndAddress", updownNudgeEndAddress.Value);
        }

        private void NudgeEndAddressDown(object sender, EventArgs e)
        {
            NudgeParams("dgvEndAddress", updownNudgeEndAddress.Value, true);
        }

        private void NudgeParam1Up(object sender, EventArgs e)
        {
            NudgeParams("dgvParam1", updownNudgeParam1.Value);
        }

        private void NudgeParam1Down(object sender, EventArgs e)
        {
            NudgeParams("dgvParam1", updownNudgeParam1.Value, true);
        }

        private void NudgeParam2Up(object sender, EventArgs e)
        {
            NudgeParams("dgvParam2", updownNudgeParam2.Value);
        }

        private void NudgeParam2Down(object sender, EventArgs e)
        {
            NudgeParams("dgvParam2", updownNudgeParam2.Value, true);
        }

        private void NudgeParams(string column, decimal amount, bool shiftDown = false)
        {
            if (shiftDown)
            {
                foreach (DataGridViewRow selected in dgvBlastGenerator.SelectedRows)
                {
                    if ((Convert.ToDecimal(selected.Cells[column].Value) - amount) >= 0)
                    {
                        selected.Cells[column].Value = Convert.ToDecimal(selected.Cells[column].Value) - amount;
                    }
                    else
                    {
                        selected.Cells[column].Value = 0;
                    }
                }
            }
            else
            {
                foreach (DataGridViewRow selected in dgvBlastGenerator.SelectedRows)
                {
                    decimal max = ((DataGridViewNumericUpDownCell)selected.Cells[column]).Maximum;

                    if ((Convert.ToDecimal(selected.Cells[column].Value) - amount) <= max)
                    {
                        selected.Cells[column].Value = Convert.ToDecimal(selected.Cells[column].Value) + amount;
                    }
                    else
                    {
                        selected.Cells[column].Value = max;
                    }
                }
            }
        }

        private void HideSidebarToggle(object sender, EventArgs e)
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
                S.GET<MemoryDomainsForm>().RefreshDomainsAndKeepSelected();
                domainToMiDico.Clear();
                domains = MemoryDomains.MemoryInterfaces.Keys.Concat(MemoryDomains.VmdPool.Values.Select(it => it.ToString())).ToArray();
                foreach (string domain in domains)
                {
                    domainToMiDico.Add(domain, MemoryDomains.GetInterface(domain));
                }

                foreach (DataGridViewRow row in dgvBlastGenerator.Rows)
                {
                    PopulateDomainCombobox(row);
                }

                return true;
            }
            catch (NullReferenceException e)
            {
                MessageBox.Show("An error occurred in RTC while refreshing the domains\n" +
                                "Are you sure you don't have an invalid domain selected?\n" +
                                "Make sure any VMDs are loaded and you have the correct system loaded.\n");
                logger.Error(e, "Blast generator domains null!");
                return false;
            }
        }

        private void RefreshDomains(object sender, EventArgs e)
        {
            RefreshDomains();
        }

        private static void SaveDataGridView(DataGridView dgv)
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

            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "bg|*.bg"
            };
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
                    logger.Error(ex, "Error saving DataGridView");
                }
            }
        }

        private bool importDataGridView(DataGridView dgv)
        {
            return loadDataGridView(dgv, true);
        }

        private static object GetDefault(Type t)
        {
            Func<object> f = GetDefault<object>;
            return f.Method.GetGenericMethodDefinition().MakeGenericMethod(t).Invoke(null, null);
        }

        private static T GetDefault<T>()
        {
            return default(T);
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
                        //    NullValueHandling = NullValueHandling.Ignore,
                    };
                    dt = JsonConvert.DeserializeObject<DataTable>(File.ReadAllText(ofd.FileName), settings);
                    if (!import)
                    {
                        dgv.Rows.Clear();
                    }

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
                            if (item is DBNull)
                            {
                                dgv.Rows[lastrow].Cells[i].Value = GetDefault(dgv.Rows[lastrow].Cells[i].ValueType);
                            }
                            else
                            {
                                dgv.Rows[lastrow].Cells[i].Value = item;
                            }
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

        private void LoadBlastGenerator(object sender, EventArgs e)
        {
            loadDataGridView(dgvBlastGenerator);
        }

        private void SaveBlastGenerator(object sender, EventArgs e)
        {
            SaveDataGridView(dgvBlastGenerator);
        }

        private void ImportBlastGenerator(object sender, EventArgs e)
        {
            importDataGridView(dgvBlastGenerator);
        }

        private void ShowHelp(object sender, EventArgs e)
        {
            System.Diagnostics.ProcessStartInfo sInfo = new System.Diagnostics.ProcessStartInfo("https://corrupt.wiki/corruptors/rtc-real-time-corruptor/blast-generator.html");
            System.Diagnostics.Process.Start(sInfo);
        }

        private void OnCellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Note handling
            if (e != null)
            {
                DataGridView senderGrid = (DataGridView)sender;

                if (e.Button == MouseButtons.Left)
                {
                    if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                        e.RowIndex >= 0)
                    {
                        {
                            DataGridViewCell textCell = dgvBlastGenerator.Rows[e.RowIndex].Cells["dgvNoteText"];
                            DataGridViewCell buttonCell = dgvBlastGenerator.Rows[e.RowIndex].Cells["dgvNoteButton"];

                            NoteItem note = new NoteItem(textCell.Value == null ? "" : textCell.Value.ToString());
                            textCell.Value = note;
                            S.SET(new NoteEditorForm(note, buttonCell));
                            S.GET<NoteEditorForm>().Show();
                            return;
                        }
                    }
                }
                else if (e.Button == MouseButtons.Right)
                {
                    cms = new ContextMenuStrip();
                    if (e.RowIndex != -1 && e.ColumnIndex != -1)
                    {
                        if (e.ColumnIndex == dgvBlastGenerator.Columns["dgvSeed"].Index)
                        {
                            ((ToolStripMenuItem)cms.Items.Add("Reroll Seed", null, new EventHandler((ob, ev) =>
                            {
                                var cell = dgvBlastGenerator[e.ColumnIndex, e.RowIndex];
                                cell.Value = RtcCore.RND.Next(int.MinValue, int.MaxValue);
                            }))).Enabled = true;
                        }
                    }
                    cms.Show(dgvBlastGenerator, dgvBlastGenerator.PointToClient(Cursor.Position));
                }
            }
        }

        private void OnCellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
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

        private void OnUnitsInheritNoteChanged(object sender, EventArgs e)
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
            {
                return true;
            }

            foreach (var control in allControls)
            {
                //We assume focus on a TB or UpDown is edit mode
                if (control is TextBox || control is NumericUpDown)
                {
                    if (control.Focused)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public StashKey[] GetStashKeys()
        {
            return new[] { _sk };
        }

        public void Recolor()
        {
            Colors.SetRTCColor(Colors.GeneralColor, this);
        }
    }

    internal class NoteItem : INote
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
