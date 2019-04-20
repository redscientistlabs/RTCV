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
        private SavestateHolder selectedHolder = null;


        private int numPerPage;

        public int NumPerPage
        {
            get => numPerPage;
        }

        private BindingSource pagedSource;

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
            for (int i = 0; i < controlList.Count; i++)
            {
                if (i + _DataSource.Position < _DataSource.Count)
                {
                    var x = (Tuple<StashKey, string>) _DataSource[i + _DataSource.Position];
                    controlList[i].SetStashKey(x.Item1, x.Item2, i + _DataSource.Position);
                }
                else
                    controlList[i].SetStashKey(null, null, i + _DataSource.Position);
            }
        }

        void _DataSource_ListChanged(object sender, ListChangedEventArgs e)
        {

            if (e.ListChangedType == ListChangedType.ItemAdded ||
                e.ListChangedType == ListChangedType.ItemChanged ||
                e.ListChangedType == ListChangedType.ItemMoved)
                if (e.NewIndex < _DataSource.Position + numPerPage)
                {
                    var x = (Tuple<StashKey, string>) _DataSource[e.NewIndex];
                    controlList[e.NewIndex - _DataSource.Position].SetStashKey(x.Item1, x.Item2, e.NewIndex);
                }

            //If deleted just refresh as it's easier than moving everything ourselves
            if (e.ListChangedType == ListChangedType.ItemDeleted)
            {
                _DataSource_PositionChanged(null, null);
            }
        }

        public SavestateList()
        {
            InitializeComponent();
        }

        private void InitializeSavestateHolder()
        {
            int padding = 8;
            int ssHeight = 22;
            //Calculate how many we can fit within the space we have.
            numPerPage = flowPanel.Height / (ssHeight + padding);
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
            Point locate = new Point(((Control) sender).Location.X + e.Location.X, ((Control) sender).Location.Y + e.Location.Y);

            if (e.Button == MouseButtons.Left)
            {
                selectedHolder?.SetSelected(false);
                selectedHolder = (SavestateHolder) ((Button) sender).Parent;
                selectedHolder.SetSelected(true);

                if (selectedHolder.sk == null)
                    return;

                StockpileManager_UISide.CurrentSavestateKey = selectedHolder.sk.Key;
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
                            selectedHolder?.SetSelected(false);
                            StockpileManager_UISide.CurrentSavestateKey = null;
                            return;
                        }
                    }
                    else
                    {
                        selectedHolder?.SetSelected(false);
                        StockpileManager_UISide.CurrentSavestateKey = null;
                        return;
                    }
                }

                if (e.Button == MouseButtons.Right)
                {
                    ContextMenuStrip cms = new ContextMenuStrip();
                    cms.Items.Add("Delete entry", null, (ob, ev) =>
                    {
                        var holder = (SavestateHolder) ((Button) sender).Parent;
                        int indexToRemove = controlList.IndexOf(holder) + _DataSource.Position;
                        if (indexToRemove <= _DataSource.Count)
                            _DataSource.RemoveAt(indexToRemove);
                    });
                    cms.Show((Control) sender, locate);
                }
            }
        }

        private void BtnForward_Click(object sender, EventArgs e)
        {

            if (_DataSource.Position + NumPerPage < _DataSource.Count - 1)
                _DataSource.Position = _DataSource.Position + NumPerPage;
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            _DataSource.Position = _DataSource.Position - NumPerPage;
        }

        public StashKey GetSelectedStashkey()
        {
            return selectedHolder.sk;
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
                        foreach (var sk in StockpileManager_UISide.SavestateStashkeyDico.Values.Where(x => x.RomFilename == oldFilename))
                        {
                            sk.RomFilename = filename;
                        }
                    }
                    else
                        return false;
                }
            return true;
        }

        private void BtnSaveLoad_Click(object sender, EventArgs e)
        {
            if (btnSaveLoad.Text == "LOAD")
            {
                StashKey psk = selectedHolder.sk;
                if (psk != null)
                {
                    if (checkAndFixingMissingStates(psk))
                        return;
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

            }
        }
    }
}
