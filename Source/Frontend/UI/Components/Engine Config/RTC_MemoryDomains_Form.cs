using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RTCV.CorruptCore;
using RTCV.NetCore;
using static RTCV.UI.UI_Extensions;
using RTCV.NetCore.StaticTools;

namespace RTCV.UI
{
	public partial class RTC_MemoryDomains_Form : ComponentForm, IAutoColorize
	{
		public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
		public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);
        private Timer updateTimer;
		public RTC_MemoryDomains_Form()
		{
			InitializeComponent();
            updateTimer = new Timer
            {
                Enabled = true, 
                Interval = 300,
            };
            updateTimer.Tick += UpdateSelectedMemoryDomains;
        }

        private void UpdateSelectedMemoryDomains(object sender, EventArgs args)
        {
            AllSpec.UISpec.Update("SELECTEDDOMAINS", lbMemoryDomains.SelectedItems.Cast<string>().Distinct().ToArray());
        }

        public void SetMemoryDomainsSelectedDomains(string[] _domains)
        {
            var oldState = this.Visible;

			for (int i = 0; i < lbMemoryDomains.Items.Count; i++)
				if (_domains.Contains(lbMemoryDomains.Items[i].ToString()))
					lbMemoryDomains.SetSelected(i, true);
				else
					lbMemoryDomains.SetSelected(i, false);

            UpdateSelectedMemoryDomains(null, null);
            this.Visible = oldState;
        }

		public void SetMemoryDomainsAllButSelectedDomains(string[] _blacklistedDomains)
        {
            var oldState = this.Visible;

			for (
				int i = 0; i < lbMemoryDomains.Items.Count; i++)
				if (_blacklistedDomains?.Contains(lbMemoryDomains.Items[i].ToString()) ?? false)
					lbMemoryDomains.SetSelected(i, false);
				else
					lbMemoryDomains.SetSelected(i, true);

            UpdateSelectedMemoryDomains(null, null);
            this.Visible = oldState;
        }

		private void btnSelectAll_Click(object sender, EventArgs e)
		{
			RefreshDomains();


			for (int i = 0; i < lbMemoryDomains.Items.Count; i++)
				lbMemoryDomains.SetSelected(i, true);


            UpdateSelectedMemoryDomains(null, null);
        }

		private void btnAutoSelectDomains_Click(object sender, EventArgs e)
        {
            LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_DOMAIN_REFRESHDOMAINS, true);
            RefreshDomains();
			SetMemoryDomainsAllButSelectedDomains((string[])RTCV.NetCore.AllSpec.VanguardSpec[VSPEC.MEMORYDOMAINS_BLACKLISTEDDOMAINS] ?? new string[] { });
		}

		public void RefreshDomains()
		{
            var oldState = this.Visible;
			lbMemoryDomains.Items.Clear();
			if (MemoryDomains.MemoryInterfaces != null)
				lbMemoryDomains.Items.AddRange(MemoryDomains.MemoryInterfaces?.Keys.ToArray());

			if (MemoryDomains.VmdPool.Count > 0)
				lbMemoryDomains.Items.AddRange(MemoryDomains.VmdPool.Values.Select(it => it.ToString()).ToArray());

            this.Visible = oldState;
        }

		public void RefreshDomainsAndKeepSelected(string[] overrideDomains = null)
		{
			var temp = (string[])RTCV.NetCore.AllSpec.UISpec["SELECTEDDOMAINS"];
			var oldDomain = lbMemoryDomains.Items;



			RefreshDomains(); //refresh and reload domains

			if (overrideDomains != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var s in overrideDomains)
                    sb.Append($"{s},");
                Console.WriteLine($"RefreshDomainsAndKeepSelected override SELECTEDDOMAINS domains to {sb}");
                RTCV.NetCore.AllSpec.UISpec.Update("SELECTEDDOMAINS", overrideDomains);
				SetMemoryDomainsSelectedDomains(overrideDomains);
			}
			//If we had old domains selected don't do anything
			else if (temp?.Length != 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var s in temp)
                    sb.Append($"{s},");
                Console.WriteLine($"RefreshDomainsAndKeepSelected temp Setting SELECTEDDOMAINS domains to {sb}");

                RTCV.NetCore.AllSpec.UISpec.Update("SELECTEDDOMAINS", temp);
				SetMemoryDomainsSelectedDomains(temp);
			}
			else
			{
				SetMemoryDomainsAllButSelectedDomains((string[])RTCV.NetCore.AllSpec.VanguardSpec[VSPEC.MEMORYDOMAINS_BLACKLISTEDDOMAINS] ?? new string[0]);
			}
		}

		private void lbMemoryDomains_SelectedIndexChanged(object sender, EventArgs e)
		{
            StringBuilder sb = new StringBuilder();
            foreach (var s in lbMemoryDomains.SelectedItems.Cast<string>().ToArray())
                sb.Append($"{s},");
            Console.WriteLine($"lbIndexChanged Setting SELECTEDDOMAINS domains to {sb}");

            updateTimer.Stop();
            updateTimer.Start();

			//RTC_Restore.SaveRestore();
		}


		private void btnRefreshDomains_Click(object sender, EventArgs e)
		{
			RefreshDomains();
			RTCV.NetCore.AllSpec.UISpec.Update("SELECTEDDOMAINS", lbMemoryDomains.SelectedItems.Cast<string>().ToArray());
		}
	}
}
