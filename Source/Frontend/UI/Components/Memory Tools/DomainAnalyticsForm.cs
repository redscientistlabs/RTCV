namespace RTCV.UI
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using RTCV.CorruptCore;
    using RTCV.NetCore;
    using RTCV.Common;
    using RTCV.UI.Modular;

    #pragma warning disable CA2213 //Component designer classes generate their own Dispose method
    public partial class DomainAnalyticsForm : ComponentForm, IBlockable
    {
        private new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        private new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        public DomainAnalyticsForm()
        {
            InitializeComponent();

            this.undockedSizable = false;
        }

        private bool FirstInit = false;

        private List<string> MemoryDumps = null;
        private Timer ActiveTableAutodump = null;

        private void AddDomainDump(object sender, EventArgs e)
        {
            if (cbSelectedMemoryDomain == null || MemoryDomains.GetInterface(cbSelectedMemoryDomain.SelectedItem.ToString()).Size.ToString() == null)
            {
                MessageBox.Show("Select a valid domain before continuing!");
                return;
            }
            if (MemoryDumps == null)
                return;

            string key = RtcCore.GetRandomKey();

            LocalNetCoreRouter.Route(NetCore.Endpoints.CorruptCore, NetCore.Commands.Remote.DomainActiveTableMakeDump, new object[] { cbSelectedMemoryDomain.SelectedItem.ToString(), key }, true);

            var keyPath = Path.Combine(RtcCore.workingDir, "MEMORYDUMPS", key + ".dmp");
            MemoryDumps.Add(keyPath);
            lbNbMemoryDumps.Text = "Memory dumps collected: " + MemoryDumps.Count.ToString();

            if (MemoryDumps.Count > 1)
                btnSendToAnalytics.Enabled = true;
        }

        private void InitializeDumpCollection(object sender, EventArgs e)
        {
            if (!FirstInit)
            {
                FirstInit = true;
                btnActiveTableDumpsReset.Text = "Reset";

                btnActiveTableAddDump.Font = new Font("Segoe UI", 8);
                btnActiveTableAddDump.Enabled = true;
                cbAutoAddDump.Enabled = true;
            }
            MemoryInterface mi = MemoryDomains.GetInterface(cbSelectedMemoryDomain.SelectedItem.ToString());
            if (mi == null)
            {
                MessageBox.Show("The currently selected domain doesn't exist!\nMake sure you have the correct core loaded and have refreshed the domains.");
                return;
            }
            decimal memoryDomainSize = mi.Size;

            //Verify they want to continue if the domain size is larger than 32MB
            if (memoryDomainSize > 0x2000000)
            {
                DialogResult result = MessageBox.Show("The domain you have selected is larger than 32MB\n The domain size is " + (memoryDomainSize / (1024 * 1024)) + "MB.\n Are you sure you want to continue?", "Large Domain Detected", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                    return;
            }

            lbDomainAddressSize.Text = "Domain size: 0x" + mi.Size.ToString("X");
            lbNbMemoryDumps.Text = "Memory dumps collected: 0";

            MemoryDumps = new List<string>();

            foreach (string file in Directory.GetFiles(Path.Combine(RtcCore.workingDir, "MEMORYDUMPS")))
                File.Delete(file);

            btnSendToAnalytics.Enabled = false;
        }

        private void RefreshDomains()
        {
            S.GET<MemoryDomainsForm>().RefreshDomainsAndKeepSelected();
            var temp = cbSelectedMemoryDomain.SelectedItem;

            cbSelectedMemoryDomain.Items.Clear();
            var domains = MemoryDomains.MemoryInterfaces?.Keys.Where(it => !it.Contains("[V]")).ToArray();
            if (domains?.Length > 0)
                cbSelectedMemoryDomain.Items.AddRange(domains);

            if (temp != null && cbSelectedMemoryDomain.Items.Contains(temp))
                cbSelectedMemoryDomain.SelectedItem = temp;
            else if (cbSelectedMemoryDomain.Items.Count > 0)
                cbSelectedMemoryDomain.SelectedIndex = 0;
        }

        private void LoadDomains(object sender, EventArgs e)
        {
            cbAutoAddDump.Checked = false;

            RefreshDomains();
            btnActiveTableDumpsReset.Enabled = true;
            btnActiveTableDumpsReset.Font = new Font("Segoe UI", 8);
            btnLoadDomains.Text = "Refresh Domains";
        }

        private void UpdateAutoAddDump(object sender, EventArgs e)
        {
            if (ActiveTableAutodump != null)
            {
                ActiveTableAutodump.Stop();
                ActiveTableAutodump = null;
            }

            if (cbAutoAddDump.Checked)
            {
                ActiveTableAutodump = new Timer();
                ActiveTableAutodump.Interval = Convert.ToInt32(nmAutoAddSec.Value) * 1000;
                ActiveTableAutodump.Tick += new EventHandler(AddDomainDump);
                ActiveTableAutodump.Start();
            }
        }

        private void UpdateAutoAddInterval(object sender, EventArgs e)
        {
            if (ActiveTableAutodump != null)
                ActiveTableAutodump.Interval = Convert.ToInt32(nmAutoAddSec.Value) * 1000;
        }

        private void SendToAnalytics(object sender, EventArgs e)
        {
            cbAutoAddDump.Checked = false;
            var mi = MemoryDomains.GetInterface(cbSelectedMemoryDomain.SelectedItem.ToString());
            AnalyticsToolForm.OpenAnalyticsTool(mi, MemoryDumps);
        }

        private void UpdateSelectedMemoryDomain(object sender, EventArgs e)
        {
            cbAutoAddDump.Checked = false;

            if (btnActiveTableDumpsReset.Text == "Reset")
                InitializeDumpCollection(sender, e);
        }
    }
}
