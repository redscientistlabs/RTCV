namespace RTCV.UI.Components.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.Drawing.Design;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using CorruptCore;
    using NetCore;
    using RTCV.Common;

    public partial class SavestateList : UserControl
    {
        private List<SavestateHolder> _controlList;
        private SavestateHolder _selectedHolder;
        private string _saveStateWord = "Savestate";
        private bool _wasSaveWhenFilledSlotWasLastSelected;

        public SavestateHolder SelectedHolder
        {
            get => _selectedHolder;
            set
            {
                _selectedHolder = value;
                StockpileManagerUISide.CurrentSavestateStashKey = value?.sk;
            }
        }

        public StashKey CurrentSaveStateStashKey => SelectedHolder?.sk ?? null;

        private int _numPerPage;

        private int NumPerPage => _numPerPage;

        private BindingSource _dataSource;

        [TypeConverter("System.Windows.Forms.Design.DataSourceConverter, System.Design")]
        [Editor("System.Windows.Forms.Design.DataSourceListEditor, System.Design", typeof(UITypeEditor))]
        [AttributeProvider(typeof(IListSource))]
        public object DataSource
        {
            get => _dataSource;
            set
            {
                //Detach from old DataSource
                if (_dataSource != null)
                {
                    _dataSource.ListChanged -= DataSource_ListChanged;
                }

                _dataSource = value as BindingSource;

                InitializeSavestateHolder();

                //Attach to new one
                if (_dataSource != null)
                {
                    _dataSource.ListChanged += DataSource_ListChanged;
                    _dataSource.PositionChanged += DataSource_PositionChanged;
                    DataSource_PositionChanged(null, null);
                }
            }
        }

        private void DataSource_PositionChanged(object sender, EventArgs e)
        {
            if (_dataSource.Position == -1)
            {
                for (var i = 0; i < _controlList.Count; i++)
                {
                    _controlList[i].SetStashKey(null, i + _dataSource.Position + 1);
                }
            }
            else
            {
                for (var i = 0; i < _controlList.Count; i++)
                {
                    //Update it
                    if (i + _dataSource.Position < _dataSource.Count)
                    {
                        var x = (SaveStateKey)_dataSource[i + _dataSource.Position];
                        _controlList[i].SetStashKey(x, i + _dataSource.Position);
                    }
                    else
                    {
                        _controlList[i].SetStashKey(null, i + _dataSource.Position);
                    }
                }
            }

            RefreshForwardBackwardButtons();
        }

        private void DataSource_ListChanged(object sender, ListChangedEventArgs e)
        {
            //Just refresh as it's cleaner and we're not dealing with so many that it causes perf problems
            DataSource_PositionChanged(null, null);
        }

        public SavestateList()
        {
            InitializeComponent();
            Resize += (s, ev) => CalculateStatesPerPage();
        }

        private void InitializeSavestateHolder()
        {
            //Nuke any old holder if it exists
            SelectedHolder?.SetSelected(false);
            SelectedHolder = null;
            flowPanel.Controls.Clear();
            _controlList = new List<SavestateHolder>();
            CalculateStatesPerPage();
        }

        private void flowPanel_SizeChanged(object sender, EventArgs e)
        {
            flowPanel.SuspendLayout();
            foreach (Control control in flowPanel.Controls)
            {
                control.Width = flowPanel.Width;
            }
            flowPanel.ResumeLayout();
        }

        private void CalculateStatesPerPage()
        {
            var ssHeight = 22;
            var padding = 3;
            //Calculate how many we can fit within the space we have.
            _numPerPage = ((flowPanel.Height - 2) / (ssHeight + padding)) - 1;
            if (_numPerPage < 0)
                _numPerPage = 0;
            if (_controlList.Count == _numPerPage)
                return;
            
            flowPanel.SuspendLayout();
            if (_numPerPage > _controlList.Count)
            {
                for (var i = _controlList.Count; i < _numPerPage; i++)
                {
                    var ssh = new SavestateHolder(i);
                    ssh.btnSavestate.MouseDown += BtnSavestate_MouseDown;
                    flowPanel.Controls.Add(ssh);
                    _controlList.Add(ssh);
                }
            }
            else
            {
                for (var i = _controlList.Count; i > _numPerPage; i--)
                {
                    flowPanel.Controls.Remove(_controlList[i - 1]);
                    _controlList.RemoveAt(i - 1);
                }
            }
            
            if (!(_dataSource is null))
                DataSource_PositionChanged(null, null);
            if (Parent is IColorize colorize)
                colorize.Recolor();
            flowPanel.ResumeLayout();
        }

        public void BtnSavestate_MouseDown(object sender, MouseEventArgs e)
        {
            Point locate;

            if (e != null)
                locate = new Point(((Control)sender).Location.X + e.Location.X, ((Control)sender).Location.Y + e.Location.Y);
            else
                locate = new Point(0, 0);

            if (e == null || e.Button == MouseButtons.Left)
            {
                SelectedHolder?.SetSelected(false);
                bool hadEmptySlotSelected = SelectedHolder is { sk: null };
                SelectedHolder = (SavestateHolder)((Button)sender).Parent;
                SelectedHolder.SetSelected(true);

                if (SelectedHolder.sk == null)
                {
                    btnSaveLoad.Text = "SAVE";
                    btnSaveLoad.ForeColor = Color.OrangeRed;
                    return;
                }

                if (hadEmptySlotSelected)
                {
                    if (_wasSaveWhenFilledSlotWasLastSelected)
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
                else
                {
                    _wasSaveWhenFilledSlotWasLastSelected = btnSaveLoad.Text == "SAVE";
                }

                StashKey psk = SelectedHolder.sk;

                if (psk != null && !File.Exists(psk.RomFilename))
                {
                    if (!CheckAndFixingMissingStates(psk))
                    {
                        SelectedHolder?.SetSelected(false);
                        return;
                    }
                }

                var smForm = (Parent as SavestateManagerForm);
                if (smForm != null && smForm.cbSavestateLoadOnClick.Checked)
                {
                    btnSaveLoad.Text = "LOAD";
                    HandleSaveLoadClick(null, null);
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                var holder = (SavestateHolder)((Button)sender).Parent;
                var holderIndex = _controlList.IndexOf(holder);
                var indexToRemove = holderIndex + _dataSource.Position;
                new ContextMenuBuilder()
                    .If(holderIndex != -1 && indexToRemove >= 0 && indexToRemove < _dataSource.Count)
                    .AddItem("Delete Entry", (ob, ev) =>
                    {
                        _dataSource.RemoveAt(indexToRemove);
                        S.GET<SavestateManagerForm>().UnsavedEdits = true;
                    }).EndIf()
                    .If(holder.sk != null).AddItem("New Blastlayer From This Savestate (Blast Editor)", (ob, ev) =>
                    {
                        var holder = (SavestateHolder)((Button)sender).Parent;
                        var psk = holder.sk;

                        if (psk == null)
                        {
                            MessageBox.Show(
                                "There is no savestate associated with this box. Make a savestate and try again.");
                            return;
                        }

                        var newStashkey = new StashKey(RtcCore.GetRandomKey(), psk.ParentKey, null)
                        {
                            RomFilename = psk.RomFilename,
                            SystemName = psk.SystemName,
                            SystemCore = psk.SystemCore,
                            GameName = psk.GameName,
                            SyncSettings = psk.SyncSettings,
                            StateLocation = psk.StateLocation
                        };

                        newStashkey.BlastLayer = new BlastLayer();

                        BlastEditorForm.OpenBlastEditor(newStashkey);
                    }).EndIf()
                    .AddItem("Save to this entry", (ob, ev) =>
                    {
                        var holder = (SavestateHolder)((Button)sender).Parent;
                        StashKey sk = StockpileManagerUISide.SaveState();
                        RegisterStashKeyTo(holder, sk);
                    })
                    .AddItem("Load this entry", (ob, ev) =>
                    {
                        var holder = (SavestateHolder)((Button)sender).Parent;
                        StashKey psk = holder.sk;
                        if (psk != null)
                        {
                            if (!CheckAndFixingMissingStates(psk))
                            {
                                return;
                            }

                            StockpileManagerUISide.LoadState(psk);
                        }
                        else
                        {
                            MessageBox.Show($"{_saveStateWord} box is empty");
                        }

                        StockpileManagerUISide.CurrentStashkey = null;
                        S.GET<GlitchHarvesterBlastForm>().IsCorruptionApplied = false;
                        LocalNetCoreRouter.Route(NetCore.Endpoints.CorruptCore,
                            NetCore.Commands.Remote.ClearBlastlayerCache, false);
                    })
                    .Build()
                    .Show((Control)sender, locate);
            }
        }

        private void RefreshForwardBackwardButtons()
        {
            btnForward.Enabled = _dataSource.Count >= _dataSource.Position + NumPerPage;
            btnBack.Enabled = _dataSource.Position > 0;
        }

        public void NewSavestateNow()
        {
            //yes this automates the UI. ew.

            //Search for the first empty
            SavestateHolder firstEmpty = null;
            do
            {
                firstEmpty = flowPanel.Controls.Cast<SavestateHolder>().FirstOrDefault(it => it.sk == null);

                if (firstEmpty == null)
                    BtnForward_Click(null, null); //switch page if necessary
            } while (firstEmpty == null);

            Control ctl = firstEmpty.btnSavestate;
            BtnSavestate_MouseDown(ctl, null);  //select savestate box

            if (btnSaveLoad.Text == "LOAD")
                BtnToggleSaveLoad_Click(null, null); //switch to SAVE if still in Load

            HandleSaveLoadClick(null, null);    //SAVE
        }

        public void BtnForward_Click(object sender, EventArgs e)
        {
            if (_dataSource.Position + NumPerPage <= _dataSource.Count)
            {
                _dataSource.Position += NumPerPage;
            }

            SelectedHolder?.SetSelected(false);
            SelectedHolder = _controlList.First();
            SelectedHolder.SetSelected(true);
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            _dataSource.Position -= NumPerPage;

            SelectedHolder?.SetSelected(false);
            SelectedHolder = _controlList.First();
            SelectedHolder.SetSelected(true);
        }

        public StashKey GetSelectedStashkey()
        {
            return SelectedHolder?.sk;
        }

        public void BtnToggleSaveLoad_Click(object sender, EventArgs e)
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
            if (SelectedHolder is { sk: { } })
            {
                _wasSaveWhenFilledSlotWasLastSelected = btnSaveLoad.Text == "SAVE";
            }
        }

        private bool CheckAndFixingMissingStates(StashKey psk)
        {
            if (psk.RomFilename == "IGNORE")
                return true;

            if (!File.Exists(psk.RomFilename))
            {
                if (DialogResult.Yes == MessageBox.Show($"Can't find file {psk.RomFilename}\nGame name: {psk.GameName}\nSystem name: {psk.SystemName}\n\n Would you like to provide a new file for replacement?", "Error: File not found", MessageBoxButtons.YesNo))
                {
                    var ofd = new OpenFileDialog
                    {
                        DefaultExt = "*",
                        Title = "Select Replacement File",
                        Filter = "Any file|*.*",
                        RestoreDirectory = true
                    };
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        var filename = ofd.FileName;
                        var oldFilename = psk.RomFilename;
                        foreach (var item in _dataSource.List.OfType<SaveStateKey>().Where(x => x.StashKey.RomFilename == oldFilename))
                        {
                            item.StashKey.RomFilename = filename;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public void LoadCurrentState()
        {
            StashKey psk = SelectedHolder?.sk;
            if (psk != null)
            {
                if (!CheckAndFixingMissingStates(psk))
                {
                    return;
                }

                StockpileManagerUISide.LoadState(psk);
            }
            else
            {
                MessageBox.Show($"{_saveStateWord} box is empty");
            }
        }

        [SuppressMessage("Microsoft.Design", "IDE1006", Justification = "Designer-originated method")]
        public void HandleSaveLoadClick(object sender, EventArgs e)
        {
            var renameSaveStateWord = AllSpec.VanguardSpec[VSPEC.RENAME_SAVESTATE];
            if (renameSaveStateWord != null && renameSaveStateWord is string s)
            {
                _saveStateWord = s;
            }

            if (btnSaveLoad.Text == "LOAD")
            {
                LoadCurrentState();
                StockpileManagerUISide.CurrentStashkey = null;
                S.GET<GlitchHarvesterBlastForm>().IsCorruptionApplied = false;
                LocalNetCoreRouter.Route(NetCore.Endpoints.CorruptCore, NetCore.Commands.Remote.ClearBlastlayerCache, false);
            }
            else
            {
                if (SelectedHolder == null)
                {
                    bool hasSavedItems = _controlList.FirstOrDefault(it => it.HasState()) != null;

                    if (hasSavedItems)
                    {
                        MessageBox.Show($"No {_saveStateWord} Box is currently selected in the Glitch Harvester's {_saveStateWord} Manager");
                        return;
                    }
                    else
                    {
                        //select first one
                        var holder = _controlList.First();
                        holder.SetSelected(true);
                        SelectedHolder = holder;
                    }
                }

                StashKey sk = StockpileManagerUISide.SaveState();
                if (sk != null)
                    RegisterStashKeyToSelected(sk);

                btnSaveLoad.Text = "LOAD";
                btnSaveLoad.ForeColor = Color.FromArgb(192, 255, 192);
                _wasSaveWhenFilledSlotWasLastSelected = false;
            }
        }

        private void RegisterStashKeyToSelected(StashKey sk) => RegisterStashKeyTo(SelectedHolder, sk);
        private void RegisterStashKeyTo(SavestateHolder holder, StashKey sk)
        {
            StockpileManagerUISide.CurrentSavestateStashKey = sk;

            //Replace if there's already a sk
            if (holder?.sk != null)
            {
                var indexToReplace = _controlList.IndexOf(holder) + _dataSource.Position;
                if (sk != null)
                {
                    var oldpos = _dataSource.Position; //We do this to prevent weird shifts when you insert over the something at the top of the last page
                    _dataSource.RemoveAt(indexToReplace);
                    _dataSource.Insert(indexToReplace, new SaveStateKey(sk, ""));
                    _dataSource.Position = oldpos;
                }
            }
            //Otherwise add to the last box
            else
            {
                if (sk != null)
                {
                    _dataSource.Add(new SaveStateKey(sk, ""));
                    SelectedHolder?.SetSelected(false);
                    SelectedHolder = _controlList.First(x => x.sk == sk);
                    SelectedHolder?.SetSelected(true);
                }
            }
            S.GET<SavestateManagerForm>().UnsavedEdits = true;
        }

        private void btnSaveLoad_MouseDown(object sender, MouseEventArgs e)
        {
            Point locate;

            if (e != null)
                locate = new Point(e.Location.X, e.Location.Y);
            else
                locate = new Point(0, 0);


            if (e.Button == MouseButtons.Right)
            {
                bool stockpileHasSelection = S.GET<StockpileManagerForm>().dgvStockpile.SelectedRows.Count > 0;
                new ContextMenuBuilder()
                    .AddItem("New Savestate", (ob, ev)
                        => NewSavestateNow())
                    .If(stockpileHasSelection).AddItem("Import State From Selected Stockpile Item", (ob, ev)
                        => NewSavestateFromStockpile()).EndIf()
                    .AddItem("Import State from File", (ob, ev)
                        => NewSavestateFromFile())
                    .Build()
                    .Show((Control)sender, locate);
            }
        }

        internal void LoadPreviousSavestateNow()
        {
            var sk = SelectedHolder?.sk;

            if (sk == null) //quickly evade empty slots
            {
                return;
            }

            var holders = flowPanel.Controls.Cast<SavestateHolder>();
            SavestateHolder prevHolder = null;
            foreach (var holder in holders)
            {
                if (holder?.sk == sk)
                {
                    break;
                }
                prevHolder = holder;
            }

            if (prevHolder == null)
            {
                return;
            }


            StockpileManagerUISide.LoadState(prevHolder.sk);
            StockpileManagerUISide.CurrentStashkey = null;
            S.GET<GlitchHarvesterBlastForm>().IsCorruptionApplied = false;
            LocalNetCoreRouter.Route(NetCore.Endpoints.CorruptCore, NetCore.Commands.Remote.ClearBlastlayerCache, false);
        }

        public void NewSavestateFromStockpile()
        {
            //yes this automates the UI. ew.

            //Search for the first empty
            SavestateHolder firstEmpty = null;
            do
            {
                firstEmpty = flowPanel.Controls.Cast<SavestateHolder>().FirstOrDefault(it => it.sk == null);

                if (firstEmpty == null)
                    BtnForward_Click(null, null); //switch page if necessary
            } while (firstEmpty == null);

            Control ctl = firstEmpty.btnSavestate;
            BtnSavestate_MouseDown(ctl, null);  //select savestate box


            var sm = S.GET<StockpileManagerForm>();
            var sk = sm.GetSelectedStashKey();

            if (sk != null)
            {
                var newSk = (StashKey)sk.Clone();

                newSk.Key = newSk.ParentKey;
                newSk.ParentKey = null;
                newSk.BlastLayer = new BlastLayer();
                //newSk.StateShortFilename = Path.GetFileName(newSk.GetSavestateFullPath());
                //newSk.StateData = File.ReadAllBytes(newSk.GetSavestateFullPath());
                //newSk.DeployState();
                string prevWorkingPath = sk.GetSavestateFullPath();
                string workingpath = newSk.GetSavestateFullPath();
                string skspath = Path.Combine(RtcCore.workingDir, "SKS", Path.GetFileName(prevWorkingPath));

                if (!File.Exists(skspath)) //it it wasn't from a stockpile, revert to session folder
                    skspath = Path.Combine(RtcCore.workingDir, "SESSION", Path.GetFileName(prevWorkingPath));

                if (File.Exists(skspath) && !File.Exists(workingpath))
                    File.Copy(skspath, workingpath);

                StockpileManagerUISide.CurrentStashkey = sk;
                StockpileManagerUISide.OriginalFromStashkey(sk);

                //var t = StockpileManagerUISide.LoadState(newSk, true, false); //will cause problems with heavy emus
                //t.Wait();

                RegisterStashKeyToSelected(newSk);
            }
        }

        private void NewSavestateFromFile()
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

            string filename = openSavestateDialog.FileName;
            
            //yes this automates the UI. ew.

            //Search for the first empty
            SavestateHolder firstEmpty = null;
            do
            {
                firstEmpty = flowPanel.Controls.Cast<SavestateHolder>().FirstOrDefault(it => it.sk == null);

                if (firstEmpty == null)
                    BtnForward_Click(null, null); //switch page if necessary
            } while (firstEmpty == null);

            Control ctl = firstEmpty.btnSavestate;
            BtnSavestate_MouseDown(ctl, null);  //select savestate box


            StashKey sk = StockpileManagerUISide.SaveState();

            //Let's hope the game name is correct!
            File.Copy(filename, sk.GetSavestateFullPath(), true);

            var sm = S.GET<StockpileManagerForm>();

            var newSk = (StashKey)sk.Clone();

            newSk.Key = newSk.ParentKey;
            newSk.BlastLayer = new BlastLayer();

            string prevWorkingPath = sk.GetSavestateFullPath();
            string workingpath = newSk.GetSavestateFullPath();
            string skspath = Path.Combine(RtcCore.workingDir, "SKS", Path.GetFileName(prevWorkingPath));

            if (!File.Exists(skspath)) //it it wasn't from a stockpile, revert to session folder
                skspath = Path.Combine(RtcCore.workingDir, "SESSION", Path.GetFileName(prevWorkingPath));

            if (File.Exists(skspath) && !File.Exists(workingpath))
                File.Copy(skspath, workingpath);

            StockpileManagerUISide.CurrentStashkey = sk;
            StockpileManagerUISide.OriginalFromStashkey(sk);

            RegisterStashKeyToSelected(newSk);
        }
    }
}
