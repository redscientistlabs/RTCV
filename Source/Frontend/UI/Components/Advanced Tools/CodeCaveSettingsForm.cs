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
    using System.Text;

#pragma warning disable CA2213 //Component designer classes generate their own Dispose method
    public partial class CodeCaveSettingsForm : ComponentForm, IBlockable
    {
        private new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        private new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        public CodeCaveSettingsForm()
        {
            InitializeComponent();
            updateTimer = new System.Timers.Timer
            {
                AutoReset = false,
                Interval = 300,
            };
            updateTimer.Elapsed += UpdateSelectedMemoryDomains;

            this.undockedSizable = false;
        }

        private bool FirstInit = false;

        private List<string> MemoryDumps = null;
        private Timer ActiveTableAutodump = null;
        private System.Timers.Timer updateTimer;

        private void UpdateSelectedMemoryDomains(object sender, EventArgs args)
        {
            SyncObjectSingleton.FormExecute(() =>
            {
                StringBuilder sb = new StringBuilder();
                foreach (var s in lbMemoryDomains.SelectedItems.Cast<string>().ToArray())
                {
                    sb.Append($"{s},");
                }

                logger.Trace("UpdateSelectedMemoryDomains Setting SELECTEDDOMAINS_FORCAVESEARCH domains to {domains}", sb);
                string[] output = lbMemoryDomains.SelectedItems.Cast<string>().Distinct().ToArray();
                AllSpec.UISpec.Update(UISPEC.SELECTEDDOMAINS_FORCAVESEARCH, output);

                SyncObjectSingleton.FormExecute(() =>
                {
                    UISideHooks.OnSelectedDomainsChanged(output);
                });
            });
        }
        public void RefreshDomains()
        {
            var oldState = this.Visible;
            lbMemoryDomains.Items.Clear();
            if (MemoryDomains.MemoryInterfaces != null)
            {
                lbMemoryDomains.Items.AddRange(MemoryDomains.MemoryInterfaces?.Keys.ToArray());
            }

            if (MemoryDomains.VmdPool.Count > 0)
            {
                lbMemoryDomains.Items.AddRange(MemoryDomains.VmdPool.Values.Select(it => it.ToString()).ToArray());
            }

            this.Visible = oldState;
        }

        private void lbMemoryDomains_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateTimer.Stop();
            updateTimer.Start();
        }

        private void btnLoadDomains_Click(object sender, EventArgs e)
        {
            RefreshDomains();
        }
    }
}
