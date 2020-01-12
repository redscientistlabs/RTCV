using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using RTCV.CorruptCore;
using RTCV.NetCore;
using RTCV.UI;
using static RTCV.UI.UI_Extensions;
using RTCV.NetCore.StaticTools;

namespace RTCV.UI
{
    public partial class RTC_DomainAnalytics_Form : ComponentForm, IAutoColorize, IBlockable
    {
        public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        public RTC_DomainAnalytics_Form()
        {
            InitializeComponent();

            this.undockedSizable = false;
        }

        public bool ActLoadedFromFile = false;
        public bool FirstInit = false;
        public bool _activeTableReady = false;

        public bool UseActiveTable = false;
        public bool UseCorePrecision = false;
        public List<string> ActiveTableDumps = null;
        public long[] ActiveTableActivity = null;
        public long[] ActiveTableGenerated = null;
        public double ActivityThreshold = 0;
        public Timer ActiveTableAutodump = null;

        public string _currentFilename = null;

        public byte[] GetDumpFromFile(string key)
        {
            return File.ReadAllBytes(Path.Combine(CorruptCore.RtcCore.workingDir, "MEMORYDUMPS", key + ".dmp"));
        }

        private void btnActiveTableAddDump_Click(object sender, EventArgs e)
        {
            if (cbSelectedMemoryDomain == null || MemoryDomains.GetInterface(cbSelectedMemoryDomain.SelectedItem.ToString()).Size.ToString() == null)
            {
                MessageBox.Show("Select a valid domain before continuing!");
                return;
            }
            if (ActiveTableDumps == null)
                return;

            string key = CorruptCore.RtcCore.GetRandomKey();

            LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_DOMAIN_ACTIVETABLE_MAKEDUMP, new object[] { cbSelectedMemoryDomain.SelectedItem.ToString(), key }, true);

            ActiveTableDumps.Add(key);
            lbNbMemoryDumps.Text = "Memory dumps collected: " + ActiveTableDumps.Count.ToString();

            if (ActiveTableDumps.Count > 1)
                btnSendToAnalytics.Enabled = true;
        }

        private void btnActiveTableDumpsReset_Click(object sender, EventArgs e)
        {
            ActLoadedFromFile = false;

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

            ActiveTableGenerated = null;

            ActiveTableDumps = new List<string>();

            foreach (string file in Directory.GetFiles(Path.Combine(CorruptCore.RtcCore.workingDir, "MEMORYDUMPS")))
                File.Delete(file);

            btnSendToAnalytics.Enabled = false;
        }

        private void RefreshDomains()
        {
            S.GET<RTC_MemoryDomains_Form>().RefreshDomainsAndKeepSelected();
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

        private void btnLoadDomains_Click(object sender, EventArgs e)
        {
            cbAutoAddDump.Checked = false;

            RefreshDomains();
            btnActiveTableDumpsReset.Enabled = true;
            btnActiveTableDumpsReset.Font = new Font("Segoe UI", 8);
            btnLoadDomains.Text = "Refresh Domains";
        }

        private void cbAutoAddDump_CheckedChanged(object sender, EventArgs e)
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
                ActiveTableAutodump.Tick += new EventHandler(btnActiveTableAddDump_Click);
                ActiveTableAutodump.Start();
            }
        }

        private void nmAutoAddSec_ValueChanged(object sender, EventArgs e)
        {
            if (ActiveTableAutodump != null)
                ActiveTableAutodump.Interval = Convert.ToInt32(nmAutoAddSec.Value) * 1000;
        }

        private void RTC_VmdAct_Form_Load(object sender, EventArgs e)
        {
        }

        private void btnSendToAnalytics_Click(object sender, EventArgs e)
        {
            cbAutoAddDump.Checked = false;
        }

        private void cbSelectedMemoryDomain_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbAutoAddDump.Checked = false;

            if (btnActiveTableDumpsReset.Text == "Reset")
                btnActiveTableDumpsReset_Click(sender, e);
        }
    }
}
