using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

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
    using System.Collections.Generic;

    public partial class VmdLimiterProfilerForm : ComponentForm, IBlockable
    {
        private new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        private new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        private long currentDomainSize = 0;

        private string LimiterListHash;

        public VmdLimiterProfilerForm()
        {
            InitializeComponent();
            pbProgress.Visible = false; // these need to be visible in the designer
            lbProgress.Visible = false;
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

        private async void HandleGenerateVMDClick(object sender, EventArgs e) => await GenerateVMD();

        private async Task<bool> GenerateVMD(bool autoGenerate = false)
        {
            if (string.IsNullOrWhiteSpace(cbSelectedMemoryDomain.SelectedItem?.ToString()) || !MemoryDomains.MemoryInterfaces.ContainsKey(cbSelectedMemoryDomain.SelectedItem.ToString()))
            {
                cbSelectedMemoryDomain.Items.Clear();
                return false;
            }

            if (!autoGenerate && !string.IsNullOrWhiteSpace(tbVmdName.Text) && MemoryDomains.VmdPool.ContainsKey($"[V]{tbVmdName.Text}"))
            {
                MessageBox.Show("There is already a VMD with this name in the VMD Pool");
                return false;
            }

            if (autoGenerate && MemoryDomains.VmdPool.ContainsKey($"[V]{tbVmdName.Text}"))
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

            proto.SystemCore = AllSpec.VanguardSpec[VSPEC.SYSTEMCORE].ToString();
            proto.BigEndian = mi.BigEndian;
            proto.WordSize = mi.WordSize;
            proto.Padding = 0;

            var sk = S.GET<SavestateManagerForm>().CurrentSaveStateStashKey;
            if (sk == null && cbLoadBeforeGenerate.Checked && (AllSpec.VanguardSpec[VSPEC.SUPPORTS_SAVESTATES] as bool? ?? false))
            {
                MessageBox.Show("Load before generate is checked but no Savestate is selected in the Glitch Harvester!");
                return false;
            }
            
            bool killswitchWasEnabled = AutoKillSwitch.Enabled;
            AutoKillSwitch.Enabled = false;
            
            lbProgress.Visible = true;
            pbProgress.Visible = true;
            btnGenerateVMD.Enabled = false;
            lbProgress.Text = "Filtering the domain...";
            var legalAdresses = LocalNetCoreRouter.QueryRoute<long[]>(Endpoints.CorruptCore, NetCore.Commands.Remote.LongArrayFilterDomain, new object[] { mi.Name, LimiterListHash, cbLoadBeforeGenerate.Checked ? sk : null });
            if (legalAdresses == null || legalAdresses.Length == 0)
            {
                tbVmdName.Text = "";
                lbProgress.Visible = false;
                pbProgress.Visible = false;
                btnGenerateVMD.Enabled = true;
                AutoKillSwitch.Enabled = killswitchWasEnabled;
                return false;
            }

            List<long> singles;
            List<long[]> ranges;
            bool invert = cbInvert.Checked;
            if (invert)
            {
                proto.AddRanges.Add(new[] { 0, currentDomainSize });
                singles = proto.RemoveSingles;
                ranges = proto.RemoveRanges;
            }
            else
            {
                singles = proto.AddSingles;
                ranges = proto.AddRanges;
            }

            singles.AddRange(legalAdresses);

            if (singles.Count == 0 && singles.Count == 0)
            {
                //No add range was specified, use entire domain
                ranges.Add(new[] { 0, currentDomainSize });
            }

            //Precalc the size of the vmd
            //Ignore the fact that addranges and removeranges can overlap. 
            long size = singles.Count;
                
            foreach (var v in ranges)
            {
                long x = v[1] - v[0];
                size += x;
            }
            
            if (invert)
                size = currentDomainSize - size;

            //Verify they want to continue if the domain is larger than 32MB and they didn't manually set ranges
            if (size > 0x2000000)
            {
                DialogResult result = MessageBox.Show("The VMD you're trying to generate is larger than 32MB\n The VMD size is " + ((size / 1024 / 1024) + 1) + " MB (" + (size / 1024f / 1024f / 1024f) + " GB).\n Are you sure you want to continue?", "VMD Detected", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                {
                    AutoKillSwitch.Enabled = killswitchWasEnabled;
                    lbProgress.Visible = false;
                    pbProgress.Visible = false;
                    btnGenerateVMD.Enabled = true;
                    return false;
                }
            }

            Stopwatch watch = new Stopwatch();
            long value = 0;
            IProgress<int> progress = new Progress<int>(threads =>
            {
                if (currentDomainSize == 0)
                    return;
                
                long val = Math.Min(Interlocked.Add(ref value, 1423 * threads), currentDomainSize);
                float percentage = (float)val / currentDomainSize;
                
                TimeSpan remaining = TimeSpan.FromTicks((long)(watch.Elapsed.Ticks / Math.Max(percentage, 0.0001) - watch.Elapsed.Ticks));
                
                lbProgress.Text = $@"Generating VMD, {remaining:mm\:ss} remaining   0x{val:X}/0x{currentDomainSize:X} ({percentage:P2})";
                pbProgress.Value = (int)(percentage * 500);
            });
            
            lbProgress.Text = "Generating VMD...";
            watch.Start();
            VMD = await Task.Run(() => proto.Generate(progress));
            watch.Stop();

            if (VMD.Size == 0)
            {
                MessageBox.Show("The resulting VMD had no pointers so the operation got cancelled.");
                lbProgress.Visible = false;
                pbProgress.Visible = false;
                btnGenerateVMD.Enabled = true;
                AutoKillSwitch.Enabled = killswitchWasEnabled;
                return false;
            }

            Timer t = new Timer();
            lbProgress.Text = "Generating VMD on the Vanguard's side...";
            t.Interval = 100;
            long timeTaken = watch.ElapsedMilliseconds;
            long timeLeft = timeTaken;
            t.Tick += (s, e) =>
            {
                timeLeft -= 100;
                TimeSpan remaining = TimeSpan.FromMilliseconds(Math.Max(timeLeft, 0));
                lbProgress.Text = @$"Generating VMD on the Vanguard's side... ~{remaining:mm\:ss} remaining";
                pbProgress.Value = (int)(Math.Min(((float)(timeTaken - timeLeft) / timeTaken) * 500, 500));
            };
            t.Start();
            await Task.Run(() => MemoryDomains.AddVMD(VMD));
            t.Stop();
            
            lbProgress.Visible = false;
            pbProgress.Visible = false;
            
            AutoKillSwitch.Enabled = killswitchWasEnabled;

            tbVmdName.Text = "";
            cbSelectedMemoryDomain.SelectedIndex = -1;
            cbSelectedMemoryDomain.Items.Clear();

            currentDomainSize = 0;

            lbDomainSizeValue.Text = "######";
            lbEndianTypeValue.Text = "######";
            lbWordSizeValue.Text = "######";

            //refresh to vmd pool menu
            S.GET<VmdPoolForm>().RefreshVMDs();

            if (!autoGenerate)
            {
                //Selects back the VMD Pool menu
                S.GET<VmdPoolForm>().GetFocus();
            }

            btnGenerateVMD.Enabled = true;
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
