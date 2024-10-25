namespace RTCV.UI
{
    using System;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Forms;
    using RTCV.CorruptCore;
    using RTCV.NetCore;
    using RTCV.Common;
    using RTCV.UI.Modular;

    public partial class VmdLimiterProfilerForm : ComponentForm, IBlockable
    {
        private new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        private new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        private long currentDomainSize = 0;

        private string LimiterListHash;

        public VmdLimiterProfilerForm()
        {
            InitializeComponent();
        }

        private void LoadDomains(object sender, EventArgs e)
        {
            S.GET<MemoryDomainsForm>().RefreshDomainsAndKeepSelected();

            cbSelectedMemoryDomain.Items.Clear();
            var domains = MemoryDomains.MemoryInterfaces?.Keys.Where(it => !it.Contains("[V]")).ToArray();
            if (domains?.Length > 0)
            {
                cbSelectedMemoryDomain.Items.AddRange(domains);
            }

            if (cbSelectedMemoryDomain.Items.Count > 0)
            {
                cbSelectedMemoryDomain.SelectedIndex = 0;
            }
        }

        private void HandleSelectedMemoryDomainChange(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cbSelectedMemoryDomain.SelectedItem?.ToString()) || !MemoryDomains.MemoryInterfaces.ContainsKey(cbSelectedMemoryDomain.SelectedItem.ToString()))
            {
                cbSelectedMemoryDomain.Items.Clear();
                return;
            }

            MemoryInterface mi = MemoryDomains.MemoryInterfaces[cbSelectedMemoryDomain.SelectedItem.ToString()];

            lbDomainSizeValue.Text = "0x" + mi.Size.ToString("X");
            lbWordSizeValue.Text = $"{mi.WordSize * 8} bits";
            lbEndianTypeValue.Text = (mi.BigEndian ? "Big" : "Little");

            currentDomainSize = Convert.ToInt64(mi.Size);

            updateInterface();
        }

        private void updateInterface()
        {
            MemoryInterface mi = MemoryDomains.MemoryInterfaces[cbSelectedMemoryDomain.SelectedItem.ToString()];

            long fullRange = mi.Size;

            btnGenerateVMD.Enabled = true;
        }

        public static void ProfileDomain()
        {
        }

        private void HandleGenerateVMDClick(object sender, EventArgs e) => GenerateVMD();

        private bool GenerateVMD(bool AutoGenerate = false)
        {
            if (string.IsNullOrWhiteSpace(cbSelectedMemoryDomain.SelectedItem?.ToString()) || !MemoryDomains.MemoryInterfaces.ContainsKey(cbSelectedMemoryDomain.SelectedItem.ToString()))
            {
                cbSelectedMemoryDomain.Items.Clear();
                return false;
            }

            if (!AutoGenerate && !string.IsNullOrWhiteSpace(tbVmdName.Text) && MemoryDomains.VmdPool.ContainsKey($"[V]{tbVmdName.Text}"))
            {
                MessageBox.Show("There is already a VMD with this name in the VMD Pool");
                return false;
            }

            if (AutoGenerate && MemoryDomains.VmdPool.ContainsKey($"[V]{tbVmdName.Text}"))
            {
                MemoryDomains.RemoveVMD($"[V]{tbVmdName.Text}");
            }

            MemoryInterface mi = MemoryDomains.MemoryInterfaces[cbSelectedMemoryDomain.SelectedItem.ToString()];
            VirtualMemoryDomain VMD = new VirtualMemoryDomain();
            VmdPrototype proto = new VmdPrototype
            {
                GenDomain = cbSelectedMemoryDomain.SelectedItem.ToString(),
                UsingRPC = mi.UsingRPC,
            };

            if (string.IsNullOrWhiteSpace(tbVmdName.Text))
            {
                proto.VmdName = RtcCore.GetRandomKey();
            }
            else
            {
                proto.VmdName = tbVmdName.Text;
            }

            proto.BigEndian = mi.BigEndian;
            proto.WordSize = mi.WordSize;
            proto.Padding = 0;

            var sk = S.GET<SavestateManagerForm>().CurrentSaveStateStashKey;
            if (sk == null && cbLoadBeforeGenerate.Checked && (AllSpec.VanguardSpec[VSPEC.SUPPORTS_SAVESTATES] as bool? ?? false))
            {
                MessageBox.Show("Load before generate is checked but no Savestate is selected in the Glitch Harvester!");
                return false;
            }
            var legalAdresses = LocalNetCoreRouter.QueryRoute<long[]>(NetCore.Endpoints.CorruptCore, NetCore.Commands.Remote.LongArrayFilterDomain, new object[] { mi.Name, LimiterListHash, cbLoadBeforeGenerate.Checked ? sk : null });
            if (legalAdresses == null || legalAdresses.Length == 0)
            {
                tbVmdName.Text = "";
                return false;
            }

            proto.AddSingles.AddRange(legalAdresses);

            if (proto.AddRanges.Count == 0 && proto.AddSingles.Count == 0)
            {
                //No add range was specified, use entire domain
                proto.AddRanges.Add(new long[] { 0, (currentDomainSize > long.MaxValue ? long.MaxValue : Convert.ToInt64(currentDomainSize)) });
            }

            //Precalc the size of the vmd
            //Ignore the fact that addranges and subtractranges can overlap. Only account for add
            long size = 0;
            foreach (var v in proto.AddSingles)
            {
                size++;
            }

            foreach (var v in proto.AddRanges)
            {
                long x = v[1] - v[0];
                size += x;
            }
            //If the size is still 0 and we have removals, we're gonna use the entire range then sub from it so size is now the size of the domain
            if (size == 0 &&
                (proto.RemoveSingles.Count > 0 || proto.RemoveRanges.Count > 0) ||
                (proto.RemoveSingles.Count == 0 && proto.RemoveRanges.Count == 0 && size == 0))
            {
                size = currentDomainSize;
            }

            foreach (var v in proto.RemoveSingles)
            {
                size--;
            }

            foreach (var v in proto.RemoveRanges)
            {
                long x = v[1] - v[0];
                size -= x;
            }

            //Verify they want to continue if the domain is larger than 32MB and they didn't manually set ranges
            if (size > 0x2000000)
            {
                DialogResult result = MessageBox.Show("The VMD you're trying to generate is larger than 32MB\n The VMD size is " + ((size / 1024 / 1024) + 1) + " MB (" + (size / 1024f / 1024f / 1024f) + " GB).\n Are you sure you want to continue?", "VMD Detected", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                {
                    return false;
                }
            }

            VMD = proto.Generate();

            if (VMD.Size == 0)
            {
                MessageBox.Show("The resulting VMD had no pointers so the operation got cancelled.");
                return false;
            }

            MemoryDomains.AddVMD(VMD);

            tbVmdName.Text = "";
            cbSelectedMemoryDomain.SelectedIndex = -1;
            cbSelectedMemoryDomain.Items.Clear();

            currentDomainSize = 0;

            lbDomainSizeValue.Text = "######";
            lbEndianTypeValue.Text = "######";
            lbWordSizeValue.Text = "######";

            //refresh to vmd pool menu
            S.GET<VmdPoolForm>().RefreshVMDs();

            if (!AutoGenerate)
            {
                //Selects back the VMD Pool menu
                S.GET<VmdPoolForm>().GetFocus();
            }

            return true;
        }

        internal void AutoProfile(MemoryInterface mi, string limiter)
        {
            LoadDomains(null, null);

            var ceForm = S.GET<CorruptionEngineForm>();

            foreach (var item in cbSelectedMemoryDomain.Items)
            {
                if (item.ToString() == mi.ToString())
                {
                    cbSelectedMemoryDomain.SelectedItem = item;
                    break;
                }
            }

            foreach (ComboBoxItem<string> item in ceForm.VectorEngineControl.cbVectorLimiterList.Items)
            {
                if (item.Name == limiter)
                {
                    ceForm.VectorEngineControl.cbVectorLimiterList.SelectedItem = item;
                    break;
                }
            }

            ComboBoxItem<string> cbItem = (ComboBoxItem<string>)((ComboBox)ceForm.VectorEngineControl.cbVectorLimiterList).SelectedItem;
            if (cbItem != null)
            {
                LimiterListHash = cbItem.Value;
            }

            tbVmdName.Text = $"{mi} -- {limiter}";

            GenerateVMD(true);
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            cbVectorLimiterList.DataSource = null;
            cbVectorLimiterList.DisplayMember = "Name";
            cbVectorLimiterList.ValueMember = "Value";

            //Do this here as if it's stuck into the designer, it keeps defaulting out
            cbVectorLimiterList.DataSource = RtcCore.LimiterListBindingSource;

            if (RtcCore.LimiterListBindingSource.Count > 0)
            {
                HandleVectorLimiterListSelectionChange(cbVectorLimiterList, null);
            }
        }

        private void HandleVectorLimiterListSelectionChange(object sender, EventArgs e)
        {
            ComboBoxItem<string> item = (ComboBoxItem<string>)((ComboBox)sender).SelectedItem;
            if (item != null)
            {
                LimiterListHash = item.Value;
            }
        }
    }
}
