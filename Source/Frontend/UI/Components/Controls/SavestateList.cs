namespace RTCV.UI.Components.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
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
        public List<SavestateHolder> controlList;
        private SavestateHolder _selectedHolder;
        private string saveStateWord = "Savestate";

        public SavestateHolder selectedHolder
        {
            get => _selectedHolder;
            set
            {
                _selectedHolder = value;
                CorruptCore.StockpileManager_UISide.CurrentSavestateStashKey = value?.sk;
            }
        }

        public StashKey CurrentSaveStateStashKey => selectedHolder?.sk ?? null;

        private int numPerPage;

        public int NumPerPage => numPerPage;

        //private BindingSource pagedSource;

        private BindingSource _DataSource;

        [TypeConverter("System.Windows.Forms.Design.DataSourceConverter, System.Design")]
        [Editor("System.Windows.Forms.Design.DataSourceListEditor, System.Design", typeof(UITypeEditor))]
        [AttributeProvider(typeof(IListSource))]
        public object DataSource
        {
            get => _DataSource;
            set
            {
                //Detach from old DataSource
                if (_DataSource != null)
                {
                    _DataSource.ListChanged -= _DataSource_ListChanged;
                }

                _DataSource = value as BindingSource;

                InitializeSavestateHolder();

                //Attach to new one
                if (_DataSource != null)
                {
                    _DataSource.ListChanged += _DataSource_ListChanged;
                    _DataSource.PositionChanged += _DataSource_PositionChanged;
                    _DataSource_PositionChanged(null, null);
                }
            }
        }

        private void _DataSource_PositionChanged(object sender, EventArgs e)
        {
            if (_DataSource.Position == -1)
            {
                for (int i = 0; i < controlList.Count; i++)
                {
                    controlList[i].SetStashKey(null, i + _DataSource.Position + 1);
                }
            }
            else
            {
                for (int i = 0; i < controlList.Count; i++)
                {
                    //Update it
                    if (i + _DataSource.Position < _DataSource.Count)
                    {
                        var x = (SaveStateKey)_DataSource[i + _DataSource.Position];
                        controlList[i].SetStashKey(x, i + _DataSource.Position);
                    }
                    else
                    {
                        controlList[i].SetStashKey(null, i + _DataSource.Position);
                    }
                }
            }

            RefreshForwardBackwardButtons();
        }

        private void _DataSource_ListChanged(object sender, ListChangedEventArgs e)
        {
            //Just refresh as it's cleaner and we're not dealing with so many that it causes perf problems
            _DataSource_PositionChanged(null, null);
        }

        public SavestateList()
        {
            InitializeComponent();
        }

        private void InitializeSavestateHolder()
        {
            //Nuke any old holder if it exists
            selectedHolder?.SetSelected(false);
            selectedHolder = null;

            int ssHeight = 22;
            int padding = 3;
            //Calculate how many we can fit within the space we have.
            numPerPage = (flowPanel.Height / (ssHeight + padding)) - 1;
            //Create the list
            flowPanel.Controls.Clear();
            controlList = new List<SavestateHolder>();
            for (int i = 0; i < numPerPage; i++)
            {
                var ssh = new SavestateHolder(i);
                ssh.btnSavestate.MouseDown += BtnSavestate_MouseDown;
                flowPanel.Controls.Add(ssh);
                controlList.Add(ssh);
            }
        }

        public void BtnSavestate_MouseDown(object sender, MouseEventArgs e)
        {
            Point locate = new Point(((Control)sender).Location.X + e.Location.X, ((Control)sender).Location.Y + e.Location.Y);

            if (e.Button == MouseButtons.Left)
            {
                selectedHolder?.SetSelected(false);
                selectedHolder = (SavestateHolder)((Button)sender).Parent;
                selectedHolder.SetSelected(true);

                if (selectedHolder.sk == null)
                {
                    return;
                }

                StashKey psk = selectedHolder.sk;

                if (psk != null && !File.Exists(psk.RomFilename))
                {
                    if (!checkAndFixingMissingStates(psk))
                    {
                        selectedHolder?.SetSelected(false);
                        return;
                    }
                }

                var smForm = (Parent as RTC_SavestateManager_Form);
                if (smForm != null && smForm.cbSavestateLoadOnClick.Checked)
                {
                    btnSaveLoad.Text = "LOAD";
                    btnSaveLoad_Click(null, null);
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                ContextMenuStrip cms = new ContextMenuStrip();
                cms.Items.Add("Delete entry", null, (ob, ev) =>
                {
                    var holder = (SavestateHolder)((Button)sender).Parent;
                    int indexToRemove = controlList.IndexOf(holder) + _DataSource.Position;
                    if (indexToRemove <= _DataSource.Count)
                    {
                        _DataSource.RemoveAt(indexToRemove);
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

                    RTC_NewBlastEditor_Form.OpenBlastEditor(newStashkey);
                });


                cms.Show((Control)sender, locate);
            }
        }

        private void RefreshForwardBackwardButtons()
        {
            btnForward.Enabled = _DataSource.Count >= _DataSource.Position + NumPerPage;
            btnBack.Enabled = _DataSource.Position > 0;
        }

        private void BtnForward_Click(object sender, EventArgs e)
        {
            if (_DataSource.Position + NumPerPage <= _DataSource.Count)
            {
                _DataSource.Position = _DataSource.Position + NumPerPage;
            }

            selectedHolder?.SetSelected(false);
            selectedHolder = controlList.First();
            selectedHolder.SetSelected(true);
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            _DataSource.Position = _DataSource.Position - NumPerPage;

            selectedHolder?.SetSelected(false);
            selectedHolder = controlList.First();
            selectedHolder.SetSelected(true);
        }

        public StashKey GetSelectedStashkey()
        {
            return selectedHolder?.sk;
        }

        private void BtnToggleSaveLoad_Click(object sender, EventArgs e)
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

        private bool checkAndFixingMissingStates(StashKey psk)
        {
            if (!File.Exists(psk.RomFilename))
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
                        string filename = ofd.FileName;
                        string oldFilename = psk.RomFilename;
                        foreach (var item in _DataSource.List.OfType<SaveStateKey>().Where(x => x.StashKey.RomFilename == oldFilename))
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
            StashKey psk = selectedHolder?.sk;
            if (psk != null)
            {
                if (!checkAndFixingMissingStates(psk))
                {
                    return;
                }

                StockpileManager_UISide.LoadState(psk);
            }
            else
            {
                MessageBox.Show($"{saveStateWord} box is empty");
            }
        }

        public void btnSaveLoad_Click(object sender, EventArgs e)
        {
            object renameSaveStateWord = AllSpec.VanguardSpec[VSPEC.RENAME_SAVESTATE];
            if (renameSaveStateWord != null && renameSaveStateWord is string s)
            {
                saveStateWord = s;
            }

            if (btnSaveLoad.Text == "LOAD")
            {
                LoadCurrentState();
                StockpileManager_UISide.CurrentStashkey = null;
                S.GET<RTC_GlitchHarvesterBlast_Form>().IsCorruptionApplied = false;
            }
            else
            {
                if (selectedHolder == null)
                {
                    MessageBox.Show($"No {saveStateWord} Box is currently selected in the Glitch Harvester's {saveStateWord} Manager");
                    return;
                }

                StashKey sk = StockpileManager_UISide.SaveState();

                if (sk == null)
                {
                    btnSaveLoad.Text = "LOAD";
                    btnSaveLoad.ForeColor = Color.FromArgb(192, 255, 192);
                    return;
                }

                StockpileManager_UISide.CurrentSavestateStashKey = sk;

                //Replace if there'a already a sk
                if (selectedHolder?.sk != null)
                {
                    int indexToReplace = controlList.IndexOf(selectedHolder) + _DataSource.Position;
                    if (sk != null)
                    {
                        var oldpos = _DataSource.Position; //We do this to prevent weird shifts when you insert over the something at the top of the last page
                        _DataSource.RemoveAt(indexToReplace);
                        _DataSource.Insert(indexToReplace, new SaveStateKey(sk, ""));
                        _DataSource.Position = oldpos;
                    }
                }
                //Otherwise add to the last box
                else
                {
                    if (sk != null)
                    {
                        _DataSource.Add(new SaveStateKey(sk, ""));
                        selectedHolder?.SetSelected(false);
                        selectedHolder = controlList.Where(x => x.sk == sk).First() ?? null;
                        selectedHolder?.SetSelected(true);
                    }
                }

                btnSaveLoad.Text = "LOAD";
                btnSaveLoad.ForeColor = Color.FromArgb(192, 255, 192);
            }
        }
    }
}
