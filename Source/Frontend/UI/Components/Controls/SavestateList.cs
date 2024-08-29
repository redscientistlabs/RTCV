namespace RTCV.UI.Components.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.Drawing.Design;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using System.Threading.Tasks;
    using RTCV.CorruptCore;
    using RTCV.NetCore;
    using RTCV.Common;
    using SlimDX.DirectWrite;

    public partial class SavestateList : UserControl
    {
        private List<SavestateHolder> _controlList;
        private SavestateHolder _selectedHolder;
        private string _saveStateWord = "Savestate";

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

        public int NumPerPage => _numPerPage;

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
        }

        private void InitializeSavestateHolder()
        {
            //Nuke any old holder if it exists
            SelectedHolder?.SetSelected(false);
            SelectedHolder = null;

            var ssHeight = 22;
            var padding = 3;
            //Calculate how many we can fit within the space we have.
            _numPerPage = (flowPanel.Height / (ssHeight + padding)) - 1;
            //Create the list
            flowPanel.Controls.Clear();
            _controlList = new List<SavestateHolder>();
            for (var i = 0; i < _numPerPage; i++)
            {
                var ssh = new SavestateHolder(i);
                ssh.btnSavestate.MouseDown += BtnSavestate_MouseDown;
                flowPanel.Controls.Add(ssh);
                _controlList.Add(ssh);
            }
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
                SelectedHolder = (SavestateHolder)((Button)sender).Parent;
                SelectedHolder.SetSelected(true);

                if (SelectedHolder.sk == null)
                {
                    return;
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
                var cms = new ContextMenuStrip();
                cms.Items.Add("Delete entry", null, (ob, ev) =>
                {
                    var holder = (SavestateHolder)((Button)sender).Parent;
                    var holderIndex = _controlList.IndexOf(holder);
                    if (holderIndex != -1)
                    {
                        var indexToRemove = holderIndex + _dataSource.Position;
                        if (indexToRemove >= 0 && indexToRemove <= _dataSource.Count)
                        {
                            _dataSource.RemoveAt(indexToRemove);
                            S.GET<SavestateManagerForm>().UnsavedEdits = true;
                        }
                    }
                });

                cms.Items.Add("New Blastlayer from this Savestate (Blast Editor)", null, (ob, ev) =>
                {
                    var holder = (SavestateHolder)((Button)sender).Parent;
                    var psk = holder.sk;

                    if (psk == null)
                    {
                        MessageBox.Show("There is no savestate associated with this box. Make a savestate and try again.");
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
                });

                cms.Items.Add("Save to this entry", null, (ob, ev) =>
                {
                    var holder = (SavestateHolder)((Button)sender).Parent;
                    StashKey sk = StockpileManagerUISide.SaveState();
                    RegisterStashKeyTo(holder, sk);
                });

                cms.Items.Add("Load this entry", null, (ob, ev) =>
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
                    LocalNetCoreRouter.Route(NetCore.Endpoints.CorruptCore, NetCore.Commands.Remote.ClearBlastlayerCache, false);
                });
                

                cms.Show((Control)sender, locate);
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
                var cms = new ContextMenuStrip();
                cms.Items.Add("New Savestate", null, (ob, ev) =>
                {
                    NewSavestateNow();
                });

                if(S.GET<StockpileManagerForm>().dgvStockpile.SelectedRows.Count > 0)
                    cms.Items.Add("Import State from selected Stockpile Item", null, (ob, ev) => NewSavestateFromStockpile());
                
                cms.Items.Add("Import State from File", null, (ob, ev) => NewSavestateFromFile());

                cms.Show((Control)sender, locate);
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

        private void NewSavestateFromStockpile()
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
            newSk.ParentKey = null;
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
