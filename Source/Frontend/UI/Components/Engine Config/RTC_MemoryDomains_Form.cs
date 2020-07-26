namespace RTCV.UI
{
    using System;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;
    using RTCV.CorruptCore;
    using RTCV.NetCore;
    using RTCV.Common;
    using static RTCV.UI.UI_Extensions;

    public partial class RTC_MemoryDomains_Form : ComponentForm, IAutoColorize, IBlockable
    {
        public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);
        private System.Timers.Timer updateTimer;

        public RTC_MemoryDomains_Form()
        {
            InitializeComponent();
            updateTimer = new System.Timers.Timer
            {
                AutoReset = false,
                Interval = 300,
            };
            updateTimer.Elapsed += UpdateSelectedMemoryDomains;
        }

        private void UpdateSelectedMemoryDomains(object sender, EventArgs args)
        {
            SyncObjectSingleton.FormExecute(() =>
            {
                StringBuilder sb = new StringBuilder();
                foreach (var s in lbMemoryDomains.SelectedItems.Cast<string>().ToArray())
                {
                    sb.Append($"{s},");
                }

                logger.Trace("UpdateSelectedMemoryDomains Setting SELECTEDDOMAINS domains to {domains}", sb);
                AllSpec.UISpec.Update("SELECTEDDOMAINS", lbMemoryDomains.SelectedItems.Cast<string>().Distinct().ToArray());
            });
        }

        public void SetMemoryDomainsSelectedDomains(string[] _domains)
        {
            var oldState = this.Visible;

            for (int i = 0; i < lbMemoryDomains.Items.Count; i++)
            {
                if (_domains.Contains(lbMemoryDomains.Items[i].ToString()))
                {
                    lbMemoryDomains.SetSelected(i, true);
                }
                else
                {
                    lbMemoryDomains.SetSelected(i, false);
                }
            }

            UpdateSelectedMemoryDomains(null, null);
            this.Visible = oldState;
        }

        public void SetMemoryDomainsAllButSelectedDomains(string[] _blacklistedDomains)
        {
            var oldState = this.Visible;

            for (int i = 0; i < lbMemoryDomains.Items.Count; i++)
            {
                if (_blacklistedDomains?.Contains(lbMemoryDomains.Items[i].ToString()) ?? false)
                {
                    lbMemoryDomains.SetSelected(i, false);
                }
                else
                {
                    lbMemoryDomains.SetSelected(i, true);
                }
            }

            UpdateSelectedMemoryDomains(null, null);
            this.Visible = oldState;
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            RefreshDomains();

            for (int i = 0; i < lbMemoryDomains.Items.Count; i++)
            {
                lbMemoryDomains.SetSelected(i, true);
            }

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
            {
                lbMemoryDomains.Items.AddRange(MemoryDomains.MemoryInterfaces?.Keys.ToArray());
            }

            if (MemoryDomains.VmdPool.Count > 0)
            {
                lbMemoryDomains.Items.AddRange(MemoryDomains.VmdPool.Values.Select(it => it.ToString()).ToArray());
            }

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
                {
                    sb.Append($"{s},");
                }

                logger.Trace("RefreshDomainsAndKeepSelected override SELECTEDDOMAINS domains to {domains}", sb);
                RTCV.NetCore.AllSpec.UISpec.Update("SELECTEDDOMAINS", overrideDomains);
                SetMemoryDomainsSelectedDomains(overrideDomains);
            }
            //If we had old domains selected don't do anything
            else if (temp?.Length != 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var s in temp)
                {
                    sb.Append($"{s},");
                }

                logger.Trace("RefreshDomainsAndKeepSelected temp Setting SELECTEDDOMAINS domains to {domains}", sb);

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
            updateTimer.Stop();
            updateTimer.Start();

            //RTC_Restore.SaveRestore();
        }

        private void btnRefreshDomains_Click(object sender, EventArgs e)
        {
            RefreshDomains();
            RTCV.NetCore.AllSpec.UISpec.Update("SELECTEDDOMAINS", lbMemoryDomains.SelectedItems.Cast<string>().ToArray());
        }

        private void lbMemoryDomains_MouseDown(object sender, MouseEventArgs e)
        {
            //Point locate = new Point(((Control)sender).Location.X + e.Location.X, ((Control)sender).Location.Y + e.Location.Y);
            Point locate = new Point(e.Location.X, e.Location.Y);

            if (e.Button == MouseButtons.Right)
            {
                string vectorLimiter = S.GET<RTC_CorruptionEngine_Form>().CurrentVectorLimiterListName;
                var AutoLimitedDomains = MemoryDomains.AllMemoryInterfaces.Where(it => it.Value is VirtualMemoryDomain vmd && vmd.Name.Contains("->")).ToList();

                if (vectorLimiter != null)
                {
                    ContextMenuStrip cms = new ContextMenuStrip();
                    //cms.Items.Add($"Generate VMD using Vector Limiter", null, (ob, ev) => {}).Enabled = false;
                    var lbGen = new ToolStripLabel($"Limiter Profiler");
                    lbGen.Font = new Font(lbGen.Font, FontStyle.Italic);

                    cms.Items.Add(lbGen);
                    cms.Items.Add(new ToolStripSeparator());
                    cms.Items.Add($"Regenerate all Profiled VMDs", null, (ob, ev) =>
                    {
                        foreach (var mi in MemoryDomains.AllMemoryInterfaces.Where(it => it.Value is VirtualMemoryDomain && it.Key.Contains("->")))
                        {
                            var vmd = (mi.Value as VirtualMemoryDomain);
                            string domain;
                            if (vmd.CompactPointerDomains.Length > 0)
                                domain = vmd.CompactPointerDomains.FirstOrDefault();
                            else
                                domain = vmd.PointerDomains.FirstOrDefault();


                            if (domain != null)
                            {
                                string limiter = vmd.Name.Substring(vmd.Name.LastIndexOf('>') + 2);
                                S.GET<RTC_VmdLimiterProfiler_Form>().AutoProfile(MemoryDomains.AllMemoryInterfaces[domain], limiter);
                            }
                        }
                    }).Enabled = (AutoLimitedDomains.Count > 0);


                    var cbLoadState = new ToolStripMenuItem();
                    cbLoadState.Text = "Load GH State on Generate";
                    var vlpForm = S.GET<RTC_VmdLimiterProfiler_Form>();
                    cbLoadState.Checked = vlpForm.cbLoadBeforeGenerate.Checked;
                    cbLoadState.Click += (ob, ev) => {
                        vlpForm.cbLoadBeforeGenerate.Checked = !vlpForm.cbLoadBeforeGenerate.Checked;
                    };
                    cms.Items.Add(cbLoadState);

                    cms.Items.Add(new ToolStripSeparator());

                    foreach (var mi in MemoryDomains.AllMemoryInterfaces.Where(it => !(it.Value is VirtualMemoryDomain)))
                    {
                        var menu = new ToolStripMenuItem();

                        string extraVector = "";
                        if (MemoryDomains.VmdPool.ContainsKey($"[V]{mi.Value} -- {vectorLimiter}"))
                            extraVector = " (Regenerate)";

                        var currentListMenuItem = new ToolStripMenuItem();
                        currentListMenuItem.Text = mi.Key.ToString();

                        var vectorMenuItem = new ToolStripMenuItem();
                        vectorMenuItem.Text = $"Use Vector Engine Limiter: -> {vectorLimiter}" + extraVector;

                        vectorMenuItem.Click += (ob, ev) => {
                            S.GET<RTC_VmdLimiterProfiler_Form>().AutoProfile(mi.Value, vectorLimiter);
                        };

                        currentListMenuItem.DropDownItems.Add(vectorMenuItem);
                        currentListMenuItem.DropDownItems.Add(new ToolStripSeparator());

                        foreach (ComboBoxItem<string> listItem in S.GET<RTC_CorruptionEngine_Form>().cbVectorLimiterList.Items)
                        {
                            var listName = listItem.Name;
                            var subMenuItem = new ToolStripMenuItem();

                            string extra = "";
                            if (MemoryDomains.VmdPool.ContainsKey($"[V]{mi.Value} -- {listName}"))
                                extra = " (Regenerate)";

                            subMenuItem.Text = "-> " + listName + extra;

                            subMenuItem.Click += (ob, ev) => {
                                S.GET<RTC_VmdLimiterProfiler_Form>().AutoProfile(mi.Value, listName);
                            };

                            currentListMenuItem.DropDownItems.Add(subMenuItem);
                        }

                        cms.Items.Add(currentListMenuItem);
                    }

                    cms.Show((Control)sender, locate);
                }
            }
        }
    }
}
