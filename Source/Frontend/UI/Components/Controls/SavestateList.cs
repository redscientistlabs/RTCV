using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RTCV.CorruptCore;

namespace RTCV.UI.Components.Controls
{
    public partial class SavestateList : UserControl
    {
        private List<SavestateHolder> controlList;
        private SavestateHolder _selectedHolder;

        private SavestateHolder selectedHolder
        {
            get => _selectedHolder;
            set
            {
                _selectedHolder = value;
                CorruptCore.StockpileManager_UISide.CurrentSavestateStashKey = value.sk;
            }
        }


        private int numPerPage;

        public int NumPerPage
        {
            get => numPerPage;
        }

        //private BindingSource pagedSource;

        private BindingSource _DataSource;

        [TypeConverter("System.Windows.Forms.Design.DataSourceConverter, System.Design")]
        [Editor("System.Windows.Forms.Design.DataSourceListEditor, System.Design", typeof(UITypeEditor))]
        [AttributeProvider(typeof(IListSource))]
        public object DataSource
        {
            get { return _DataSource; }
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
                        controlList[i].SetStashKey(null, i + _DataSource.Position);
                }
            }
        }

        void _DataSource_ListChanged(object sender, ListChangedEventArgs e)
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

        private void BtnSavestate_MouseDown(object sender, MouseEventArgs e)
        {
            Point locate = new Point(((Control)sender).Location.X + e.Location.X, ((Control)sender).Location.Y + e.Location.Y);

            if (e.Button == MouseButtons.Left)
            {
                selectedHolder?.SetSelected(false);
                selectedHolder = (SavestateHolder)((Button)sender).Parent;
                selectedHolder.SetSelected(true);

                if (selectedHolder.sk == null)
                    return;

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
                        _DataSource.RemoveAt(indexToRemove);
                });
                cms.Show((Control)sender, locate);
            }
        }

        private void BtnForward_Click(object sender, EventArgs e)
        {

            if (_DataSource.Position + NumPerPage <= _DataSource.Count)
                _DataSource.Position = _DataSource.Position + NumPerPage;
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            _DataSource.Position = _DataSource.Position - NumPerPage;
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
                        foreach (var item in _DataSource.List.OfType<SaveStateKey>().Where(x => x.StashKey.RomFilename == oldFilename))
                        {
                            item.StashKey.RomFilename = filename;
                        }
                    }
                    else
                        return false;
                }
            return true;
        }

        public void btnSaveLoad_Click(object sender, EventArgs e)
        {
            if (btnSaveLoad.Text == "LOAD")
            {
                StashKey psk = selectedHolder?.sk;
                if (psk != null)
                {
                    if (!checkAndFixingMissingStates(psk))
                        return;
                    StockpileManager_UISide.LoadState(psk);
                }
                else
                    MessageBox.Show("Savestate box is empty");
            }
            else
            {
                if (selectedHolder == null)
                {
                    MessageBox.Show("No Savestate Box is currently selected in the Glitch Harvester's Savestate Manager");
                    return;
                }

                StashKey sk = StockpileManager_UISide.SaveState();
                StockpileManager_UISide.CurrentSavestateStashKey = sk;

                //Replace if there'a already a sk
                if (selectedHolder?.sk != null)
                {
                    int indexToReplace = controlList.IndexOf(selectedHolder) + _DataSource.Position;
                    if (sk != null)
                    {
                        _DataSource.RemoveAt(indexToReplace);
                        _DataSource.Insert(indexToReplace, new SaveStateKey(sk, ""));
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
