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
    using RTCV.CorruptCore;
    using RTCV.NetCore;
    using RTCV.Common;

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

        internal void BtnSavestate_MouseDown(object sender, MouseEventArgs e)
        {
            var locate = new Point(((Control)sender).Location.X + e.Location.X, ((Control)sender).Location.Y + e.Location.Y);

            if (e.Button == MouseButtons.Left)
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
                        if (indexToRemove <= _dataSource.Count)
                        {
                            _dataSource.RemoveAt(indexToRemove);
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


                cms.Show((Control)sender, locate);
            }
        }

        private void RefreshForwardBackwardButtons()
        {
            btnForward.Enabled = _dataSource.Count >= _dataSource.Position + NumPerPage;
            btnBack.Enabled = _dataSource.Position > 0;
        }

        private void BtnForward_Click(object sender, EventArgs e)
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
        public async void HandleSaveLoadClick(object sender, EventArgs e)
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

                StashKey sk = await StockpileManagerUISide.SaveState();

                if (sk == null)
                {
                    btnSaveLoad.Text = "LOAD";
                    btnSaveLoad.ForeColor = Color.FromArgb(192, 255, 192);
                    return;
                }

                StockpileManagerUISide.CurrentSavestateStashKey = sk;

                //Replace if there'a already a sk
                if (SelectedHolder?.sk != null)
                {
                    var indexToReplace = _controlList.IndexOf(SelectedHolder) + _dataSource.Position;
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
                        SelectedHolder = _controlList.Where(x => x.sk == sk).First() ?? null;
                        SelectedHolder?.SetSelected(true);
                    }
                }

                btnSaveLoad.Text = "LOAD";
                btnSaveLoad.ForeColor = Color.FromArgb(192, 255, 192);
            }
        }
    }
}
