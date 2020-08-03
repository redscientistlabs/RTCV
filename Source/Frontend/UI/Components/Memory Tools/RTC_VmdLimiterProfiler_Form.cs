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

    public partial class RTC_VmdLimiterProfiler_Form : ComponentForm, IAutoColorize, IBlockable
    {
        public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        private long currentDomainSize = 0;

        private string LimiterListHash;

        public RTC_VmdLimiterProfiler_Form()
        {
            InitializeComponent();
        }

        private void btnLoadDomains_Click(object sender, EventArgs e)
        {
            S.GET<RTC_MemoryDomains_Form>().RefreshDomainsAndKeepSelected();

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

        private void cbSelectedMemoryDomain_SelectedIndexChanged(object sender, EventArgs e)
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

        public long SafeStringToLong(string input)
        {
            try
            {
                if (input.IndexOf("0X", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return long.Parse(input.Substring(2), NumberStyles.HexNumber);
                }
                else
                {
                    return long.Parse(input, NumberStyles.HexNumber);
                }
            }
            catch (FormatException e)
            {
                Console.Write(e);
                return -1;
            }
        }

        public void ProfileDomain()
        {
        }

        private void btnGenerateVMD_Click(object sender, EventArgs e)
        {
            GenerateVMD();
        }

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
                GenDomain = cbSelectedMemoryDomain.SelectedItem.ToString()
            };

            if (string.IsNullOrWhiteSpace(tbVmdName.Text))
            {
                proto.VmdName = CorruptCore.RtcCore.GetRandomKey();
            }
            else
            {
                proto.VmdName = tbVmdName.Text;
            }

            proto.BigEndian = mi.BigEndian;
            proto.WordSize = mi.WordSize;
            proto.Padding = 0;

            var sk = S.GET<RTC_SavestateManager_Form>().CurrentSaveStateStashKey;
            if (sk == null && cbLoadBeforeGenerate.Checked && (AllSpec.VanguardSpec[VSPEC.SUPPORTS_SAVESTATES] as bool? ?? false))
            {
                MessageBox.Show("Load before generate is checked but no Savestate is selected in the Glitch Harvester!");
                return false;
            }
            var legalAdresses = LocalNetCoreRouter.QueryRoute<long[]>(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_LONGARRAY_FILTERDOMAIN, new object[] { mi.Name, LimiterListHash, cbLoadBeforeGenerate.Checked ? sk : null });
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
                DialogResult result = MessageBox.Show("The VMD you're trying to generate is larger than 32MB\n The VMD size is " + ((size / 1024 / 1024) + 1) + " MB (" + size / 1024f / 1024f / 1024f + " GB).\n Are you sure you want to continue?", "VMD Detected", MessageBoxButtons.YesNo);
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
            S.GET<RTC_VmdPool_Form>().RefreshVMDs();

            if (!AutoGenerate)
            {
                //Selects back the VMD Pool menu
                foreach (var item in UICore.mtForm.cbSelectBox.Items)
                {
                    if (((dynamic)item).value is RTC_VmdPool_Form)
                    {
                        UICore.mtForm.cbSelectBox.SelectedItem = item;
                        break;
                    }
                }
            }

            return true;
        }

        internal void AutoProfile(MemoryInterface mi, string limiter)
        {
            btnLoadDomains_Click(null, null);

            var ceForm = S.GET<RTC_CorruptionEngine_Form>();

            foreach (var item in cbSelectedMemoryDomain.Items)
                if (item.ToString() == mi.ToString())
                {
                    cbSelectedMemoryDomain.SelectedItem = item;
                    break;
                }

            foreach (ComboBoxItem<string> item in ceForm.cbVectorLimiterList.Items)
                if (item.Name == limiter)
                {
                    ceForm.cbVectorLimiterList.SelectedItem = item;
                    break;
                }

            ComboBoxItem<string> cbItem = (ComboBoxItem<string>)((ComboBox)ceForm.cbVectorLimiterList).SelectedItem;
            if (cbItem != null)
            {
                LimiterListHash = cbItem.Value;
            }

            tbVmdName.Text = $"{mi} -- {limiter}";

            GenerateVMD(true);
        }

        private void RTC_VmdLimiterProfiler_Form_Load(object sender, EventArgs e)
        {
            cbVectorLimiterList.DataSource = null;
            cbVectorLimiterList.DisplayMember = "Name";
            cbVectorLimiterList.ValueMember = "Value";

            //Do this here as if it's stuck into the designer, it keeps defaulting out
            cbVectorLimiterList.DataSource = CorruptCore.RtcCore.LimiterListBindingSource;

            if (CorruptCore.RtcCore.LimiterListBindingSource.Count > 0)
            {
                CbVectorLimiterList_SelectedIndexChanged(cbVectorLimiterList, null);
            }
        }

        private void CbVectorLimiterList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBoxItem<string> item = (ComboBoxItem<string>)((ComboBox)sender).SelectedItem;
            if (item != null)
            {
                LimiterListHash = item.Value;
            }
        }
    }
}
